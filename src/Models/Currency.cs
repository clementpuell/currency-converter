using System;

namespace CurrencyConverter.Models
{
    /// <summary>
    /// Represents a currency participating in echange rates.
    /// </summary>
    public readonly struct Currency : IEquatable<Currency>
    {
        public readonly string Code { get; }

        public Currency(string code)
        {
            Code = code;
        }

        public override string ToString() => Code?.ToString();

        public bool Equals(Currency other) => Code == other.Code;
        public override bool Equals(object obj) => obj is Currency c && Equals(c);
        public override int GetHashCode() => this.Code.GetHashCode();
        public static bool operator ==(Currency a, Currency b) => a.Equals(b);
        public static bool operator !=(Currency a, Currency b) => !(a == b);

        public static implicit operator Currency(string code) => new Currency(code);
    }
}
