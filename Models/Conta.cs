namespace EmpresaX.POS.API.Models
{
    public class Conta
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public decimal Saldo { get; set; }
        public DateTime DataCriacao { get; set; }
    }
}
