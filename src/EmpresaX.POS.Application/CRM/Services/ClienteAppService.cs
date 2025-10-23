using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmpresaX.POS.Application.CRM.Commands;
using EmpresaX.POS.Application.CRM.DTOs;
using EmpresaX.POS.Domain.CRM;
using EmpresaX.POS.Domain.Shared.ValueObjects;

namespace EmpresaX.POS.Application.CRM.Services
{
    public class ClienteAppService // : IClienteAppService (descomente quando criar a interface)
    {
        private readonly IClienteRepository _clienteRepository;

        public ClienteAppService(IClienteRepository clienteRepository)
        {
            _clienteRepository = clienteRepository;
        }

        public async Task<ClienteDto> CriarClienteAsync(CriarClienteCommand command)
        {
            // 1. Cria os Value Objects a partir dos dados simples do Comando
            var nome = new Nome(command.PrimeiroNome, command.Sobrenome);
            var email = new Email(command.Email);
            var telefone = !string.IsNullOrWhiteSpace(command.Telefone) 
                ? new Telefone(command.Telefone) 
                : null;

            // 2. Valida a regra de negócio (se já existe cliente com este email)
            if (await _clienteRepository.ExisteComEmailAsync(email))
            {
                throw new InvalidOperationException("Já existe um cliente cadastrado com este email.");
            }

            // 3. Cria a entidade de domínio usando os Value Objects
            var cliente = new Cliente(nome, email, telefone, command.Observacoes);

            await _clienteRepository.AdicionarAsync(cliente);
            await _clienteRepository.SalvarAlteracoesAsync();

            // 4. Mapeia a entidade para um DTO para retornar ao cliente da API
            return MapearParaDto(cliente);
        }

        public async Task<ClienteDto> AtualizarClienteAsync(AtualizarClienteCommand command)
        {
            var cliente = await _clienteRepository.ObterPorIdAsync(command.Id)
                ?? throw new ArgumentException("Cliente não encontrado", nameof(command.Id));

            var nome = new Nome(command.PrimeiroNome, command.Sobrenome);
            var telefone = !string.IsNullOrWhiteSpace(command.Telefone) 
                ? new Telefone(command.Telefone) 
                : null;

            // Usa o método da própria entidade para atualizar, garantindo as regras de negócio
            cliente.AtualizarInformacoes(nome, telefone, command.Observacoes);

            await _clienteRepository.SalvarAlteracoesAsync();

            return MapearParaDto(cliente);
        }

        public async Task<ClienteDto?> ObterPorIdAsync(Guid id)
        {
            var cliente = await _clienteRepository.ObterPorIdAsync(id);
            return cliente != null ? MapearParaDto(cliente) : null;
        }

        public async Task<List<ClienteDto>> ListarTodosAsync()
        {
            var clientes = await _clienteRepository.ListarTodosAsync();
            return clientes.Select(MapearParaDto).ToList();
        }

        public async Task InativarClienteAsync(Guid id)
        {
            var cliente = await _clienteRepository.ObterPorIdAsync(id)
                ?? throw new ArgumentException("Cliente não encontrado", nameof(id));

            cliente.Inativar();
            await _clienteRepository.SalvarAlteracoesAsync();
        }

        public async Task AtivarClienteAsync(Guid id)
        {
            var cliente = await _clienteRepository.ObterPorIdAsync(id)
                ?? throw new ArgumentException("Cliente não encontrado", nameof(id));

            cliente.Ativar();
            await _clienteRepository.SalvarAlteracoesAsync();
        }

        // Método privado para converter a entidade de domínio em um DTO simples
        private static ClienteDto MapearParaDto(Cliente cliente) => new(
            cliente.Id,
            $"{cliente.Nome.PrimeiroNome} {cliente.Nome.Sobrenome}", // Concatena para criar o NomeCompleto
            cliente.Email.Endereco, // Extrai a string do Value Object
            cliente.Telefone?.Numero, // Extrai a string do Value Object
            cliente.Status.ToString(),
            cliente.Observacoes,
            cliente.DataCriacao,
            cliente.DataAtualizacao
        );
    }
}