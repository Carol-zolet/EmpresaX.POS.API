using System;
using System.Globalization;

namespace EmpresaX.POS.Domain.Shared.ValueObjects
{
    public record Money
    {
        public decimal Valor { get; init; }
        public string Moeda { get; init; }

        public Money(decimal valor, string moeda = "BRL")
        {
            if (valor < 0)
                throw new ArgumentException("Valor monetário não pode ser negativo", nameof(valor));

            if (string.IsNullOrWhiteSpace(moeda))
                throw new ArgumentException("Moeda não pode ser vazia", nameof(moeda));

            if (moeda.Length != 3)
                throw new ArgumentException("Moeda deve ter 3 caracteres (ex: BRL, USD)", nameof(moeda));

            // Arredondar para 2 casas decimais (centavos)
            Valor = Math.Round(valor, 2, MidpointRounding.AwayFromZero);
            Moeda = moeda.ToUpperInvariant();
        }

        // Operações matemáticas
        public Money Somar(Money outro)
        {
            ValidarMoeda(outro);
            return new Money(Valor + outro.Valor, Moeda);
        }

        public Money Subtrair(Money outro)
        {
            ValidarMoeda(outro);
            var resultado = Valor - outro.Valor;
            
            if (resultado < 0)
                throw new InvalidOperationException("Operação resultaria em valor negativo");
                
            return new Money(resultado, Moeda);
        }

        public Money Multiplicar(decimal fator)
        {
            if (fator < 0)
                throw new ArgumentException("Fator não pode ser negativo", nameof(fator));
                
            return new Money(Valor * fator, Moeda);
        }

        public Money Dividir(decimal divisor)
        {
            if (divisor <= 0)
                throw new ArgumentException("Divisor deve ser maior que zero", nameof(divisor));
                
            return new Money(Valor / divisor, Moeda);
        }

        // Operadores
        public static Money operator +(Money a, Money b) => a.Somar(b);
        public static Money operator -(Money a, Money b) => a.Subtrair(b);
        public static Money operator *(Money money, decimal fator) => money.Multiplicar(fator);
        public static Money operator *(decimal fator, Money money) => money.Multiplicar(fator);
        public static Money operator /(Money money, decimal divisor) => money.Dividir(divisor);

        // Comparações
        public static bool operator >(Money a, Money b)
        {
            a.ValidarMoeda(b);
            return a.Valor > b.Valor;
        }

        public static bool operator <(Money a, Money b)
        {
            a.ValidarMoeda(b);
            return a.Valor < b.Valor;
        }

        public static bool operator >=(Money a, Money b) => a > b || a.Equals(b);
        public static bool operator <=(Money a, Money b) => a < b || a.Equals(b);

        // Métodos utilitários
        public bool EhZero() => Valor == 0;
        public bool EhPositivo() => Valor > 0;

        public Money Percentual(decimal percentual)
        {
            if (percentual < 0 || percentual > 100)
                throw new ArgumentException("Percentual deve estar entre 0 e 100", nameof(percentual));
                
            return new Money(Valor * (percentual / 100), Moeda);
        }

        // Formatação
        public string FormatarMoeda()
        {
            return Moeda switch
            {
                "BRL" => Valor.ToString("C", new CultureInfo("pt-BR")),
                "USD" => Valor.ToString("C", new CultureInfo("en-US")),
                "EUR" => Valor.ToString("C", new CultureInfo("de-DE")),
                _ => $"{Moeda} {Valor:F2}"
            };
        }

        public string FormatarSemSimbolo()
        {
            return Valor.ToString("F2", CultureInfo.InvariantCulture);
        }

        // Conversões implícitas
        public static implicit operator decimal(Money money) => money.Valor;
        
        // Factory methods
        public static Money Zero(string moeda = "BRL") => new(0, moeda);
        public static Money Reais(decimal valor) => new(valor, "BRL");
        public static Money Dolares(decimal valor) => new(valor, "USD");

        // Validação
        private void ValidarMoeda(Money outro)
        {
            if (Moeda != outro.Moeda)
                throw new InvalidOperationException($"Não é possível operar entre moedas diferentes: {Moeda} e {outro.Moeda}");
        }

        public override string ToString() => FormatarMoeda();
    }
}
