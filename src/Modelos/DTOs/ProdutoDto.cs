namespace EmpresaX.POS.API.Modelos.DTOs
{
    public class ProdutoDto
    {
        public int Id { get; set; }
        public string? Nome { get; set; }
        public decimal Preco { get; set; }
        public int Estoque { get; set; }
    }
}
