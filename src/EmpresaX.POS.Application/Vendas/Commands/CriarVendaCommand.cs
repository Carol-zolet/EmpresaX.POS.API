using System;
using System.Collections.Generic;

namespace EmpresaX.POS.Application.Vendas.Commands
{
    // Objeto para carregar os dados de uma nova venda
    public record CriarVendaCommand(
        Guid ClienteId,
        List<ItemVendaDto> Itens,
        string MetodoPagamento
    );

    // DTO para representar um item dentro da venda
    public record ItemVendaDto(Guid ProdutoId, int Quantidade, decimal PrecoUnitario);
}
