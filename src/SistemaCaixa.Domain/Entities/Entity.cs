namespace SistemaCaixa.Domain.Entities;

public abstract class Entity
{
    public override bool Equals(object? obj)
    {
        if (obj is not Entity other)
            return false;
        if (ReferenceEquals(this, other))
            return true;
        return GetType() == other.GetType() && GetHashCode() == other.GetHashCode();
    }

    public override int GetHashCode()
    {
        // Pode ser sobrescrito nas entidades concretas
        return base.GetHashCode();
    }
}