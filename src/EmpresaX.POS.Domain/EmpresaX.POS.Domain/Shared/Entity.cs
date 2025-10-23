using System;

namespace EmpresaX.POS.Domain.Shared
{
    public abstract class Entity
    {
        public Guid Id { get; protected set; } = Guid.NewGuid();
        public DateTime DataCriacao { get; protected set; } = DateTime.UtcNow;
        public DateTime? DataAtualizacao { get; protected set; }

        protected void MarcarComoAtualizado()
        {
            DataAtualizacao = DateTime.UtcNow;
        }

        public override bool Equals(object obj)
        {
            if (obj is not Entity entity) return false;
            return Id == entity.Id;
        }

        public override int GetHashCode() => Id.GetHashCode();
    }

    public interface IAggregateRoot { }
}
