namespace EmpresaX.POS.API.Domain.Entities
{
    public class Produto
    {
        public int Id { get; set; }
        public string? Nome { get; set; }
        public decimal Preco { get; set; }
        public int Estoque { get; set; }

        /// <summary>
        /// Propriedade para armazenar dados semiestruturados, mapeada para uma coluna JSONB.
        /// Inicializada para evitar referências nulas.
        /// </summary>
        public ProdutoMetadata Metadata { get; set; } = new();
    }
}

