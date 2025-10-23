using ExcelDataReader;
using System;
using System.IO;
using System.Threading.Tasks;
using EmpresaX.POS.API.Modelos.DTOs; // Supondo que você terá DTOs para isso
using Microsoft.Extensions.Logging;

namespace EmpresaX.POS.API.Services
{
    public class ImportacaoService : IImportacaoService
    {
        private readonly ILogger<ImportacaoService> _logger;
        // No futuro, você injetará o serviço que salva as transações bancárias
        // private readonly ITransacaoBancariaService _transacaoService;

        public ImportacaoService(ILogger<ImportacaoService> logger)
        {
            _logger = logger;
            // Importante: linha necessária para o ExcelDataReader funcionar
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
        }

        public async Task ProcessarExtratoBancario(Stream fileStream)
        {
            _logger.LogInformation("Iniciando processamento do extrato bancário.");

            using (var reader = ExcelReaderFactory.CreateBinaryReader(fileStream))
            {
                // Pula a primeira linha (cabeçalho)
                reader.Read();

                while (reader.Read()) // Lê cada linha da planilha
                {
                    try
                    {
                        // Lê os dados das colunas (ajuste os números das colunas conforme sua planilha)
                        var data = reader.GetValue(0)?.ToString();
                        var descricao = reader.GetValue(1)?.ToString();
                        var valor = Convert.ToDecimal(reader.GetValue(2));

                        if (!string.IsNullOrWhiteSpace(descricao))
                        {
                            // TODO: Crie uma entidade TransacaoBancaria e um serviço para salvá-la
                            // Por enquanto, vamos apenas registrar no log
                            _logger.LogInformation($"Linha lida: Data={data}, Descrição='{descricao}', Valor={valor}");

                            // Exemplo de como seria no futuro:
                            // var novaTransacao = new CreateTransacaoBancariaDto { ... };
                            // await _transacaoService.CreateAsync(novaTransacao);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Erro ao processar uma linha da planilha. Linha ignorada.");
                    }
                }
            }
            _logger.LogInformation("Processamento do extrato concluído.");
        }
    }
}

