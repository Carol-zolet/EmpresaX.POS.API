using System.ComponentModel.DataAnnotations;

namespace EmpresaX.POS.Application.CRM.Commands
{
    public record CriarClienteCommand
    {
        [Required(ErrorMessage = "Primeiro nome é obrigatório")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Primeiro nome deve ter entre 2 e 50 caracteres")]
        public string PrimeiroNome { get; init; } = string.Empty;

        [Required(ErrorMessage = "Sobrenome é obrigatório")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Sobrenome deve ter entre 2 e 50 caracteres")]
        public string Sobrenome { get; init; } = string.Empty;

        [Required(ErrorMessage = "Email é obrigatório")]
        [EmailAddress(ErrorMessage = "Email deve ter um formato válido")]
        public string Email { get; init; } = string.Empty;

        [Phone(ErrorMessage = "Telefone deve ter um formato válido")]
        public string? Telefone { get; init; }

        [StringLength(500, ErrorMessage = "Observações não podem exceder 500 caracteres")]
        public string? Observacoes { get; init; }
    }
}

