using System;
using EmpresaX.POS.Domain.Shared;
using EmpresaX.POS.Domain.Shared.ValueObjects;
using EmpresaX.POS.Domain.CRM.Enums;

namespace EmpresaX.POS.Domain.CRM
{
    public class Cliente : Entity, IAggregateRoot
    {
        public Nome Nome { get; private set; }
        public Email Email { get; private set; }
        public Telefone? Telefone { get; private set; }
        public StatusCliente Status { get; private set; }
        public string? Observacoes { get; private set; }

        protected Cliente() { }

        public Cliente(Nome nome, Email email, Telefone? telefone = null, string? observacoes = null)
        {
            Nome = nome ?? throw new ArgumentNullException(nameof(nome));
            Email = email ?? throw new ArgumentNullException(nameof(email));
            Telefone = telefone;
            Observacoes = observacoes?.Trim();
            Status = StatusCliente.Ativo;
        }

        public void AtualizarInformacoes(Nome nome, Telefone? telefone = null, string? observacoes = null)
        {
            Nome = nome ?? throw new ArgumentNullException(nameof(nome));
            Telefone = telefone;
            Observacoes = observacoes?.Trim();
            MarcarComoAtualizado();
        }

        public void AtualizarEmail(Email novoEmail)
        {
            Email = novoEmail ?? throw new ArgumentNullException(nameof(novoEmail));
            MarcarComoAtualizado();
        }

        public void Ativar()
        {
            if (Status == StatusCliente.Ativo) return;
            Status = StatusCliente.Ativo;
            MarcarComoAtualizado();
        }

        public void Inativar()
        {
            if (Status == StatusCliente.Inativo) return;
            Status = StatusCliente.Inativo;
            MarcarComoAtualizado();
        }

        public void Bloquear()
        {
            Status = StatusCliente.Bloqueado;
            MarcarComoAtualizado();
        }

        public bool EstaAtivo() => Status == StatusCliente.Ativo;
    }
}
