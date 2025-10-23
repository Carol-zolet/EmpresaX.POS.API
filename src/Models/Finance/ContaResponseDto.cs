namespace EmpresaX.POS.API.Models.Finance
{
    public class ContaResponseDto
    {
        public int Id      { get; set; }
        public string Nome { get; set; } = default!;
        public string Tipo { get; set; } = default!;
        // Adicione outros campos que sua API realmente expõe
    }
}


