using System;
using System.Collections.Generic;

namespace CurrencyConverter.Models
{
    public struct Currency : IEquatable<Currency>
    {
        public string Code { get; }

        internal Currency(string code)
        {
            Code = code;
        }

        public override string ToString() => Code?.ToString();

        public bool Equals(Currency other) => Code == other.Code;
        public override bool Equals(object obj) => obj is Currency c && Equals(c);
        public override int GetHashCode() => this.Code.GetHashCode();
        public static bool operator ==(Currency a, Currency b) => a.Equals(b);
        public static bool operator !=(Currency a, Currency b) => !(a == b);

        public static implicit operator Currency(string code) => CurrencyPool.Create(code);
    }

    public class CurrencyPool
    {
        private static CurrencyPool instance = new CurrencyPool();

        private readonly IDictionary<string, Currency> pool = new Dictionary<string, Currency>();

        private CurrencyPool() { }

        public static Currency Create(string code)
        {
            if (instance.pool.TryGetValue(code, out var existing))
            {
                return existing;
            }

            var @new = new Currency(code);
            instance.pool.Add(code, @new);
            return @new;
        }
    }
}
