namespace EmpresaX.POS.API.Modelos.DTOs
{
    public class ContaDto
    {
        public int Id { get; set; }
        public string? Descricao { get; set; }
        public decimal Valor { get; set; }
        public DateTime DataVencimento { get; set; }
        public bool Pago { get; set; }
    }
}
