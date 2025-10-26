namespace SistemaCaixa.Domain.ValueObjects;

/// <summary>
/// Value Object que representa valores monetários com precisão decimal
/// </summary>
public sealed record Dinheiro
{
    public decimal Valor { get; }
    public string Moeda { get; }

    private Dinheiro(decimal valor, string moeda)
    {
        if (moeda != "BRL")
            throw new ArgumentException("Apenas BRL é suportado", nameof(moeda));

        Valor = Math.Round(valor, 2);
        Moeda = moeda;
    }

    public static Dinheiro FromBRL(decimal valor) => new(valor, "BRL");
    
    public static Dinheiro Zero => new(0, "BRL");

    public Dinheiro Somar(Dinheiro outro)
    {
        ValidarMesmaMoeda(outro);
        return new Dinheiro(Valor + outro.Valor, Moeda);
    }

    public Dinheiro Subtrair(Dinheiro outro)
    {
        ValidarMesmaMoeda(outro);
        return new Dinheiro(Valor - outro.Valor, Moeda);
    }

    public bool EhPositivo() => Valor > 0;
    public bool EhNegativo() => Valor < 0;

    private void ValidarMesmaMoeda(Dinheiro outro)
    {
        if (Moeda != outro.Moeda)
            throw new InvalidOperationException("Não é possível operar com moedas diferentes");
    }

    public override string ToString() => $"{Moeda} {Valor:N2}";
}
