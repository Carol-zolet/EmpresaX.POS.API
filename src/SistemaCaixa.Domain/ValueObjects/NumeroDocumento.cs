namespace SistemaCaixa.Domain.ValueObjects;

public sealed record NumeroDocumento
{
    public string Valor { get; }

    public NumeroDocumento(string valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            throw new ArgumentException("Número do documento é obrigatório", nameof(valor));
        Valor = valor.Trim();
    }

    public override string ToString() => Valor;
}
