using System;

namespace EmpresaX.POS.Domain.Entities
{
    public class Caixa
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public DateTime DataAbertura { get; set; }
        public DateTime? DataFechamento { get; set; }
        public decimal ValorAbertura { get; set; }
        public decimal ValorCalculadoFechamento { get; set; }
        public decimal ValorInformadoFechamento { get; set; }
        public decimal Diferenca { get; set; }
        public string? Observacoes { get; set; }
    }
}
