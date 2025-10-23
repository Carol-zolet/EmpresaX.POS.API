namespace EmpresaX.POS.Application.Vendas.DTOs
{
    public record RelatorioVendasDto(
        DateTime PeriodoInicio,
        DateTime PeriodoFim,
        int TotalVendas,
        decimal ValorTotal,
        decimal TicketMedio,
        List<VendaDto> Vendas,
        Dictionary<string, int> VendasPorFormaPagamento,
        Dictionary<string, decimal> ValoresPorFormaPagamento
    );

    public record ResumoVendasDiaDto(
        DateTime Data,
        int TotalVendas,
        decimal ValorTotal,
        List<VendaDto> Vendas
    );
}
