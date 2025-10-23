using System.ComponentModel.DataAnnotations;

namespace EmpresaX.POS.Application.CRM.Commands
{
    public record AtualizarClienteCommand
    {
        [Required]
        public Guid Id { get; init; }

        [Required(ErrorMessage = "Primeiro nome � obrigat�rio")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Primeiro nome deve ter entre 2 e 50 caracteres")]
        public string PrimeiroNome { get; init; } = string.Empty;

        [Required(ErrorMessage = "Sobrenome � obrigat�rio")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Sobrenome deve ter entre 2 e 50 caracteres")]
        public string Sobrenome { get; init; } = string.Empty;

        [Phone(ErrorMessage = "Telefone deve ter um formato v�lido")]
        public string? Telefone { get; init; }

        [StringLength(500, ErrorMessage = "Observa��es n�o podem exceder 500 caracteres")]
        public string? Observacoes { get; init; }
    }
}

