using System;
using System.Linq;
using System.Threading.Tasks;
using EmpresaX.POS.Application.Vendas.Commands;
using EmpresaX.POS.Domain.Vendas; // Entidade de domínio
using EmpresaX.POS.Domain.Vendas.Enums; // FormaPagamento
using EmpresaX.POS.Domain.Shared.ValueObjects; // Money
// Adicione outros usings necessários para seus repositórios

namespace EmpresaX.POS.Application.Vendas.Services
{
    public class VendaAppService // : IVendaAppService
    {
        // private readonly IVendaRepository _vendaRepository;
        // public VendaAppService(IVendaRepository vendaRepository) { _vendaRepository = vendaRepository; }

        // --- MÉTODO ADICIONADO ---
        public async Task CriarVendaAsync(CriarVendaCommand command)
        {
            // 1) Calcular total a partir dos itens do comando
            var totalDecimal = (command.Itens == null || command.Itens.Count == 0)
                ? 0m
                : command.Itens.Sum(i => i.PrecoUnitario * i.Quantidade);

            var valorTotal = new Money(totalDecimal, "BRL");

            // 2) Converter método de pagamento (string) para enum, com fallback
            var formaPagamento = Enum.TryParse<FormaPagamento>(command.MetodoPagamento, true, out var parsed)
                ? parsed
                : FormaPagamento.Dinheiro;

            // 3) Montar dados mínimos para a Venda (descrição padrão e data atual)
            var data = DateTime.UtcNow;
            var descricao = $"Venda {data:yyyy-MM-dd HH:mm}";

            // 4) Criar entidade de domínio (sem cliente carregado neste nível)
            var novaVenda = new Venda(
                data,
                descricao,
                valorTotal,
                formaPagamento,
                observacoes: null,
                cliente: null // Poderá ser associado posteriormente ao carregar a entidade Cliente
            );

            // 5) Persistência (exemplo - depende da sua infraestrutura)
            // await _vendaRepository.AdicionarAsync(novaVenda);
            // await _vendaRepository.UnitOfWork.CommitAsync();

            await Task.CompletedTask; // Mantém método assíncrono compilável por enquanto
        }
    }
}