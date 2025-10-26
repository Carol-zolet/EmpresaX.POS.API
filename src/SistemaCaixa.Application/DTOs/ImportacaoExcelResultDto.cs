namespace SistemaCaixa.Application.DTOs;

public class ImportacaoExcelResultDto
{
    public bool Sucesso { get; set; }
    public Guid? CaixaId { get; set; }
    public int TotalLinhasProcessadas { get; set; }
    public int MovimentosImportados { get; set; }
    public int ErrosEncontrados { get; set; }
    public List<ErroImportacaoDto> Erros { get; set; } = new();
    public ResumoImportacaoDto Resumo { get; set; }
}

public class ErroImportacaoDto
{
    public int Linha { get; set; }
    public string Campo { get; set; }
    public string Mensagem { get; set; }
    public string ValorOriginal { get; set; }
}

public class ResumoImportacaoDto
{
    public DateTime DataAbertura { get; set; }
    public decimal SaldoInicial { get; set; }
    public decimal TotalEntradas { get; set; }
    public decimal TotalSaidas { get; set; }
    public decimal SaldoFinal { get; set; }
    public int QuantidadeMovimentos { get; set; }
}

public class LinhaExcelDto
{
    public int NumeroLinha { get; set; }
    public int? Dia { get; set; }
    public string DescricaoGeral { get; set; }
    public string ClassificacaoConta { get; set; }
    public decimal? Entrada { get; set; }
    public decimal? Saida { get; set; }
    public decimal? Saldo { get; set; }
}
