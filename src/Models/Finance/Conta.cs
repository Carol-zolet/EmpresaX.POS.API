using System.ComponentModel.DataAnnotations;

namespace EmpresaX.POS.API.Models.Finance
{
    public class Conta
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Nome { get; set; } = string.Empty;

        [Required]
        public TipoConta Tipo { get; set; }

        [Required]
        public decimal SaldoInicial { get; set; }

        [StringLength(50)]
        public string? NumeroConta { get; set; }

        [StringLength(20)]
        public string? Agencia { get; set; }

        [StringLength(200)]
        public string? Banco { get; set; }

        public bool Ativa { get; set; } = true;

        public DateTime DataCriacao { get; set; } = DateTime.Now;

        // Navigation Properties
        public virtual ICollection<Movimentacao> Movimentacoes { get; set; } = new List<Movimentacao>();
        public virtual ICollection<FechamentoCaixa> Fechamentos { get; set; } = new List<FechamentoCaixa>();
    }

    public enum TipoConta
    {
        Caixa = 0,
        Banco = 1,
        Poupanca = 2,
        Investimento = 3
    }
}
