namespace EmpresaX.POS.Application.Vendas.DTOs
{
    public record VendaDto(
        Guid Id,
        DateTime Data,
        string Descricao,
        decimal Valor,
        string FormaPagamento,
        string Status,
        string? Observacoes,
        ClienteResumoDto? Cliente,
        DateTime DataCriacao,
        DateTime? DataAtualizacao
    );

    public record ClienteResumoDto(
        Guid Id,
        string Nome,
        string Email
    );
}
