namespace EmpresaX.POS.Application.CRM.DTOs
{
    public record ClienteDto(
        Guid Id,
        string NomeCompleto,
        string Email,
        string? Telefone,
        string Status,
        string? Observacoes,
        DateTime DataCriacao,
        DateTime? DataAtualizacao
    );
}
