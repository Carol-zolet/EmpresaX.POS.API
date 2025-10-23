using EmpresaX.POS.API.Modelos.DTOs;

namespace EmpresaX.POS.API.Services
{
    public class ContaService : IContaService
    {
        // Simulação de dados em memória
        private readonly List<ContaDto> _contas = new List<ContaDto>
        {
            new ContaDto { Id = 1, Descricao = "Conta de Luz", Valor = 250.00m, DataVencimento = DateTime.Today.AddDays(5), Pago = false },
            new ContaDto { Id = 2, Descricao = "Conta de Água", Valor = 120.50m, DataVencimento = DateTime.Today.AddDays(10), Pago = false }
        };
        private int _nextId = 3;

        public Task<IEnumerable<ContaDto>> GetAllAsync()
        {
            return Task.FromResult<IEnumerable<ContaDto>>(_contas);
        }

        public Task<ContaDto?> GetByIdAsync(int id)
        {
            var conta = _contas.FirstOrDefault(c => c.Id == id);
            return Task.FromResult(conta);
        }

        public Task<ContaDto> CreateAsync(CreateContaDto dto)
        {
            var novaConta = new ContaDto
            {
                Id = _nextId++,
                Descricao = dto.Descricao,
                Valor = dto.Valor,
                DataVencimento = dto.DataVencimento,
                Pago = false
            };
            _contas.Add(novaConta);
            return Task.FromResult(novaConta);
        }

        public Task UpdateAsync(int id, ContaDto conta)
        {
            var contaExistente = _contas.FirstOrDefault(c => c.Id == id);
            if (contaExistente == null)
                throw new KeyNotFoundException($"Conta com ID {id} não encontrada.");

            contaExistente.Descricao = conta.Descricao;
            contaExistente.Valor = conta.Valor;
            contaExistente.DataVencimento = conta.DataVencimento;
            contaExistente.Pago = conta.Pago;

            return Task.CompletedTask;
        }

        public Task DeleteAsync(int id)
        {
            var conta = _contas.FirstOrDefault(c => c.Id == id);
            if (conta == null)
                throw new KeyNotFoundException($"Conta com ID {id} não encontrada.");

            _contas.Remove(conta);
            return Task.CompletedTask;
        }
    }
}
