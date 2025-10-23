using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmpresaX.POS.API.Models
{
    [Table("FechamentosCaixa")]
    public class FechamentoCaixa
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public DateTime DataAbertura { get; set; }
        
        public DateTime? DataFechamento { get; set; }
        
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal SaldoInicial { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal SaldoFinal { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalEntradas { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalSaidas { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal DiferenciaCaixa { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string Usuario { get; set; } = "";
        
        public StatusCaixa Status { get; set; }
        
        [MaxLength(500)]
        public string? Observacoes { get; set; }
        
        public virtual ICollection<MovimentacaoCaixa> Movimentacoes { get; set; } = new List<MovimentacaoCaixa>();
    }

    [Table("MovimentacoesCaixa")]
    public class MovimentacaoCaixa
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public int FechamentoCaixaId { get; set; }
        
        [Required]
        public DateTime DataHora { get; set; }
        
        [Required]
        public TipoMovimento Tipo { get; set; }
        
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Valor { get; set; }
        
        [Required]
        [MaxLength(200)]
        public string Descricao { get; set; } = "";
        
        [MaxLength(100)]
        public string? Categoria { get; set; }
        
        public virtual FechamentoCaixa FechamentoCaixa { get; set; } = null!;
    }

    [Table("ConciliacoesBancarias")]
    public class ConciliacaoBancaria
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public DateTime DataConciliacao { get; set; }
        
        [Required]
        [MaxLength(20)]
        public string ContaBancaria { get; set; } = "";
        
        [Required]
        [MaxLength(100)]
        public string NomeArquivo { get; set; } = "";
        
        public int RegistrosProcessados { get; set; }
        
        public int RegistrosConciliados { get; set; }
        
        public int RegistrosPendentes { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal ValorConciliado { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal ValorPendente { get; set; }
        
        public StatusConciliacao Status { get; set; }
        
        [MaxLength(500)]
        public string? Observacoes { get; set; }
        
        public virtual ICollection<ItemConciliacao> Itens { get; set; } = new List<ItemConciliacao>();
    }

    [Table("ItensConciliacao")]
    public class ItemConciliacao
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public int ConciliacaoBancariaId { get; set; }
        
        [Required]
        public DateTime DataTransacao { get; set; }
        
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Valor { get; set; }
        
        [Required]
        [MaxLength(500)]
        public string Descricao { get; set; } = "";
        
        [MaxLength(50)]
        public string? DocumentoReferencia { get; set; }
        
        public StatusItemConciliacao Status { get; set; }
        
        public int? ContaPagarId { get; set; }
        
        [Column(TypeName = "decimal(5,2)")]
        public decimal? ScoreConfianca { get; set; }
        
        [MaxLength(200)]
        public string? MotivoNaoConciliado { get; set; }
        
        public virtual ConciliacaoBancaria ConciliacaoBancaria { get; set; } = null!;
    }

    public enum StatusCaixa
    {
        Aberto = 1,
        Fechado = 2,
        Conferido = 3
    }

    public enum TipoMovimento
    {
        Entrada = 1,
        Saida = 2
    }

    public enum StatusConciliacao
    {
        EmProcessamento = 1,
        Concluida = 2,
        ComDivergencias = 3
    }

    public enum StatusItemConciliacao
    {
        Pendente = 1,
        Conciliado = 2,
        Divergente = 3,
        Manual = 4
    }
}


