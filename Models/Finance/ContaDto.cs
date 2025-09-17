namespace EmpresaX.POS.API.Models.Finance
{
    public class ContaDto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;
        public decimal Saldo { get; set; }
        public DateTime DataCriacao { get; set; }
        public bool Ativa { get; set; } = true;
    }

    public class CreateContaDto
    {
        public string Nome { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;
        public decimal Saldo { get; set; }
    }
}
