namespace EmpresaX.POS.API.DTOs
{
    public class ProdutoDto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public decimal Preco { get; set; }
        public int Estoque { get; set; }
    }

    public class CreateProdutoRequest
    {
        public string Nome { get; set; }
        public decimal Preco { get; set; }
        public int Estoque { get; set; }
    }

    public class UpdateProdutoRequest
    {
        public string Nome { get; set; }
        public decimal Preco { get; set; }
        public int Estoque { get; set; }
    }
}