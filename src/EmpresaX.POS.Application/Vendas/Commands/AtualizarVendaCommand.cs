using System.ComponentModel.DataAnnotations;

namespace EmpresaX.POS.Application.Vendas.Commands
{
    public record AtualizarVendaCommand
    {
        [Required]
        public Guid Id { get; init; }

        [Required(ErrorMessage = "Descrição é obrigatória")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "Descrição deve ter entre 3 e 200 caracteres")]
        public string Descricao { get; init; } = string.Empty;

        [Required(ErrorMessage = "Forma de pagamento é obrigatória")]
        [Range(1, 5, ErrorMessage = "Forma de pagamento inválida")]
        public int FormaPagamento { get; init; }

        [StringLength(500, ErrorMessage = "Observações não podem exceder 500 caracteres")]
        public string? Observacoes { get; init; }
    }
}
