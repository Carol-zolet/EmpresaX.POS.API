using System.Collections.Generic;

namespace EmpresaX.POS.API.Domain.Entities
{
    public class ProdutoMetadata
    {
        public List<string> Tags { get; set; } = new();
        public string? Cor { get; set; }
        public string? Material { get; set; }
    }
}


