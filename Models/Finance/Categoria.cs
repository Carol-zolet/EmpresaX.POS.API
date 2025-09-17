using System.ComponentModel.DataAnnotations;

namespace EmpresaX.POS.API.Models.Finance
{
    public class Categoria
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Nome { get; set; } = string.Empty;

        [Required]
        public TipoCategoria Tipo { get; set; }

        [StringLength(500)]
        public string? Descricao { get; set; }

        public bool Ativa { get; set; } = true;

        public DateTime DataCriacao { get; set; } = DateTime.Now;

        // Navigation Properties
        public virtual ICollection<Movimentacao> Movimentacoes { get; set; } = new List<Movimentacao>();
    }

    public enum TipoCategoria
    {
        Receita = 0,
        Despesa = 1
    }
}
