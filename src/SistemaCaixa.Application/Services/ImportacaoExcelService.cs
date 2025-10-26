using ClosedXML.Excel;
using MediatR;
using Microsoft.Extensions.Logging;
using SistemaCaixa.Application.DTOs;
using SistemaCaixa.Domain.ValueObjects;
using System.Globalization;

namespace SistemaCaixa.Application.Services;

public class ImportacaoExcelService : IImportacaoExcelService
{
    private readonly IMediator _mediator;
    private readonly ILogger<ImportacaoExcelService> _logger;

    public ImportacaoExcelService(
        IMediator mediator,
        ILogger<ImportacaoExcelService> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public async Task<ImportacaoExcelResultDto> ImportarPlanilhaAsync(
        Stream fileStream,
        string fileName,
        string operador,
        CancellationToken cancellationToken = default)
    {
        var resultado = new ImportacaoExcelResultDto();
        try
        {
            _logger.LogInformation("Iniciando importação de planilha: {FileName} por {Operador}", fileName, operador);
            using var workbook = new XLWorkbook(fileStream);
            var worksheet = workbook.Worksheet(1);
            var metadados = ExtrairMetadados(worksheet);
            resultado.Resumo = new ResumoImportacaoDto { DataAbertura = metadados.DataReferencia };
            var saldoInicial = ExtrairSaldoInicial(worksheet);
            if (!saldoInicial.HasValue)
            {
                resultado.Erros.Add(new ErroImportacaoDto { Linha = 4, Campo = "Saldo Inicial", Mensagem = "Saldo inicial não encontrado na planilha" });
                resultado.Sucesso = false;
                return resultado;
            }
            var comandoAbrirCaixa = new AbrirCaixaCommand(Dinheiro.FromBRL(saldoInicial.Value), operador);
            var resultadoAbrirCaixa = await _mediator.Send(comandoAbrirCaixa, cancellationToken);
            if (!resultadoAbrirCaixa.IsSuccess)
            {
                resultado.Erros.Add(new ErroImportacaoDto { Linha = 0, Campo = "Abertura de Caixa", Mensagem = resultadoAbrirCaixa.Error });
                resultado.Sucesso = false;
                return resultado;
            }
            resultado.CaixaId = resultadoAbrirCaixa.Data.Id;
            resultado.Resumo.SaldoInicial = saldoInicial.Value;
            var linhas = ExtrairLinhasMovimentos(worksheet, metadados.DataReferencia);
            resultado.TotalLinhasProcessadas = linhas.Count;
            foreach (var linha in linhas)
            {
                try
                {
                    await ProcessarLinhaAsync(linha, resultado.CaixaId.Value, cancellationToken);
                    resultado.MovimentosImportados++;
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Erro ao processar linha {Linha}: {Descricao}", linha.NumeroLinha, linha.DescricaoGeral);
                    resultado.Erros.Add(new ErroImportacaoDto { Linha = linha.NumeroLinha, Campo = "Movimento", Mensagem = ex.Message, ValorOriginal = linha.DescricaoGeral });
                    resultado.ErrosEncontrados++;
                }
            }
            resultado.Resumo.TotalEntradas = linhas.Where(l => l.Entrada.HasValue).Sum(l => l.Entrada.Value);
            resultado.Resumo.TotalSaidas = linhas.Where(l => l.Saida.HasValue).Sum(l => l.Saida.Value);
            resultado.Resumo.SaldoFinal = saldoInicial.Value + resultado.Resumo.TotalEntradas - resultado.Resumo.TotalSaidas;
            resultado.Resumo.QuantidadeMovimentos = resultado.MovimentosImportados;
            resultado.Sucesso = resultado.ErrosEncontrados == 0;
            _logger.LogInformation("Importação concluída: {Importados} movimentos, {Erros} erros", resultado.MovimentosImportados, resultado.ErrosEncontrados);
            return resultado;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro fatal ao importar planilha");
            resultado.Sucesso = false;
            resultado.Erros.Add(new ErroImportacaoDto { Linha = 0, Campo = "Importação", Mensagem = $"Erro ao processar arquivo: {ex.Message}" });
            return resultado;
        }
    }

    private MetadadosPlanilha ExtrairMetadados(IXLWorksheet worksheet)
    {
        var titulo = worksheet.Cell(1, 1).GetString();
        var metadados = new MetadadosPlanilha();
        if (titulo.Contains("NOVEMBRO", StringComparison.OrdinalIgnoreCase))
        {
            metadados.Mes = 11;
            metadados.Ano = DateTime.Now.Year;
        }
        else if (titulo.Contains("DEZEMBRO", StringComparison.OrdinalIgnoreCase))
        {
            metadados.Mes = 12;
            metadados.Ano = DateTime.Now.Year;
        }
        metadados.DataReferencia = new DateTime(metadados.Ano, metadados.Mes, 1);
        return metadados;
    }

    private decimal? ExtrairSaldoInicial(IXLWorksheet worksheet)
    {
        var celula = worksheet.Cell(4, 4);
        var valorTexto = celula.GetString();
        return ParseValorBRL(valorTexto);
    }

    private List<LinhaExcelDto> ExtrairLinhasMovimentos(IXLWorksheet worksheet, DateTime dataReferencia)
    {
        var linhas = new List<LinhaExcelDto>();
        var linhaInicio = 9;
        var linhaAtual = linhaInicio;
        while (true)
        {
            var diaTexto = worksheet.Cell(linhaAtual, 1).GetString();
            if (string.IsNullOrWhiteSpace(diaTexto))
                break;
            if (diaTexto.Contains("Total", StringComparison.OrdinalIgnoreCase) || diaTexto.Contains("Saldo", StringComparison.OrdinalIgnoreCase))
            {
                linhaAtual++;
                continue;
            }
            var linha = new LinhaExcelDto
            {
                NumeroLinha = linhaAtual,
                Dia = ParseDia(diaTexto),
                DescricaoGeral = worksheet.Cell(linhaAtual, 2).GetString(),
                ClassificacaoConta = worksheet.Cell(linhaAtual, 3).GetString(),
                Entrada = ParseValorBRL(worksheet.Cell(linhaAtual, 4).GetString()),
                Saida = ParseValorBRL(worksheet.Cell(linhaAtual, 5).GetString()),
                Saldo = ParseValorBRL(worksheet.Cell(linhaAtual, 6).GetString())
            };
            if (!string.IsNullOrWhiteSpace(linha.DescricaoGeral) && (linha.Entrada.HasValue || linha.Saida.HasValue))
            {
                linhas.Add(linha);
            }
            linhaAtual++;
        }
        return linhas;
    }

    private async Task ProcessarLinhaAsync(LinhaExcelDto linha, Guid caixaId, CancellationToken cancellationToken)
    {
        TipoMovimento tipo;
        decimal valor;
        if (linha.Entrada.HasValue && linha.Entrada.Value > 0)
        {
            tipo = TipoMovimento.Entrada;
            valor = linha.Entrada.Value;
        }
        else if (linha.Saida.HasValue && linha.Saida.Value > 0)
        {
            tipo = TipoMovimento.Saida;
            valor = linha.Saida.Value;
        }
        else
        {
            return;
        }
        var comando = new RegistrarMovimentoCommand(
            CaixaId: caixaId,
            Tipo: tipo,
            Valor: Dinheiro.FromBRL(valor),
            Descricao: $"{linha.DescricaoGeral} - {linha.ClassificacaoConta}",
            NumeroDocumento: null,
            Data: null
        );
        var resultado = await _mediator.Send(comando, cancellationToken);
        if (!resultado.IsSuccess)
        {
            throw new InvalidOperationException(resultado.Error);
        }
    }

    private int? ParseDia(string texto)
    {
        if (string.IsNullOrWhiteSpace(texto))
            return null;
        var numeroTexto = new string(texto.Where(char.IsDigit).ToArray());
        if (int.TryParse(numeroTexto, out var dia) && dia >= 1 && dia <= 31)
            return dia;
        return null;
    }

    public decimal? ParseValorBRL(string texto)
    {
        if (string.IsNullOrWhiteSpace(texto))
            return null;
        texto = texto.Replace("R$", "").Replace(".", "").Replace(",", ".").Trim();
        if (decimal.TryParse(texto, NumberStyles.Number, CultureInfo.InvariantCulture, out var valor))
        {
            return valor;
        }
        return null;
    }

    private class MetadadosPlanilha
    {
        public int Mes { get; set; }
        public int Ano { get; set; }
        public DateTime DataReferencia { get; set; }
    }
}
