using System;
using System.Collections.Generic;
using System.Linq;
using EmpresaX.POS.Domain.Shared;
using EmpresaX.POS.Domain.Shared.ValueObjects;
using EmpresaX.POS.Domain.Vendas.Enums;
using EmpresaX.POS.Domain.CRM;

namespace EmpresaX.POS.Domain.Vendas
{
    public class Venda : Entity, IAggregateRoot
    {
        public DateTime Data { get; private set; }
        public string Descricao { get; private set; }
        public Money Valor { get; private set; }
        public FormaPagamento FormaPagamento { get; private set; }
        public StatusVenda Status { get; private set; }
        public string? Observacoes { get; private set; }
        
        // Relacionamento com Cliente (opcional)
        public Guid? ClienteId { get; private set; }
        public Cliente? Cliente { get; private set; }

        // Para Entity Framework
        protected Venda() { }

        public Venda(DateTime data, string descricao, Money valor, FormaPagamento formaPagamento, 
                     string? observacoes = null, Cliente? cliente = null)
        {
            if (string.IsNullOrWhiteSpace(descricao))
                throw new ArgumentException("Descrição não pode ser vazia", nameof(descricao));

            if (valor == null)
                throw new ArgumentNullException(nameof(valor));

            if (!valor.EhPositivo())
                throw new ArgumentException("Valor da venda deve ser positivo", nameof(valor));

            Data = data;
            Descricao = descricao.Trim();
            Valor = valor;
            FormaPagamento = formaPagamento;
            Observacoes = observacoes?.Trim();
            Cliente = cliente;
            ClienteId = cliente?.Id;
            Status = StatusVenda.Pendente;
        }

        // Métodos de negócio
        public void Finalizar()
        {
            if (Status == StatusVenda.Finalizada)
                throw new InvalidOperationException("Venda já está finalizada");

            if (Status == StatusVenda.Cancelada)
                throw new InvalidOperationException("Venda cancelada não pode ser finalizada");

            Status = StatusVenda.Finalizada;
            MarcarComoAtualizado();
        }

        public void Cancelar(string? motivoCancelamento = null)
        {
            if (Status == StatusVenda.Cancelada)
                throw new InvalidOperationException("Venda já está cancelada");

            if (Status == StatusVenda.Finalizada)
                throw new InvalidOperationException("Venda finalizada não pode ser cancelada");

            Status = StatusVenda.Cancelada;
            
            if (!string.IsNullOrWhiteSpace(motivoCancelamento))
            {
                Observacoes = string.IsNullOrWhiteSpace(Observacoes) 
                    ? $"CANCELADA: {motivoCancelamento}"
                    : $"{Observacoes} | CANCELADA: {motivoCancelamento}";
            }

            MarcarComoAtualizado();
        }

        public void AtualizarDescricao(string novaDescricao)
        {
            if (Status == StatusVenda.Finalizada)
                throw new InvalidOperationException("Não é possível alterar venda finalizada");

            if (Status == StatusVenda.Cancelada)
                throw new InvalidOperationException("Não é possível alterar venda cancelada");

            if (string.IsNullOrWhiteSpace(novaDescricao))
                throw new ArgumentException("Descrição não pode ser vazia", nameof(novaDescricao));

            Descricao = novaDescricao.Trim();
            MarcarComoAtualizado();
        }

        public void AlterarFormaPagamento(FormaPagamento novaForma)
        {
            if (Status == StatusVenda.Finalizada)
                throw new InvalidOperationException("Não é possível alterar forma de pagamento de venda finalizada");

            if (Status == StatusVenda.Cancelada)
                throw new InvalidOperationException("Não é possível alterar forma de pagamento de venda cancelada");

            FormaPagamento = novaForma;
            MarcarComoAtualizado();
        }

        public void AdicionarObservacao(string observacao)
        {
            if (string.IsNullOrWhiteSpace(observacao))
                return;

            if (string.IsNullOrWhiteSpace(Observacoes))
            {
                Observacoes = observacao.Trim();
            }
            else
            {
                Observacoes += $" | {observacao.Trim()}";
            }

            MarcarComoAtualizado();
        }

        // Métodos de consulta
        public bool PodeSerAlterada() => Status == StatusVenda.Pendente;
        public bool EstaFinalizada() => Status == StatusVenda.Finalizada;
        public bool EstaCancelada() => Status == StatusVenda.Cancelada;
        public bool TemCliente() => Cliente != null;

        // Métodos para relatórios (baseados na planilha analisada)
        public bool EhVendaDoDia(DateTime data) => Data.Date == data.Date;
        public bool EhVendaDoMes(int ano, int mes) => Data.Year == ano && Data.Month == mes;
    }
}
