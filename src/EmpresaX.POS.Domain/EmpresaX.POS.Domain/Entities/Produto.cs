namespace EmpresaX.POS.Domain.Entities
{
    public class Produto
    {
        public int Id { get; set; }
        public string? Nome { get; set; }
        public decimal Preco { get; set; }
        public int Estoque { get; set; }
    }
}
