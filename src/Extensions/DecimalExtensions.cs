using System;

namespace CurrencyConverter.Extensions
{
    public static class DecimalExtensions
    {
        public static int Round0(this decimal value)
        {
            return (int)decimal.Round(value);
        }

        public static decimal Round4(this decimal value)
        {
            return decimal.Round(value, 4, MidpointRounding.AwayFromZero);
        }

        public static decimal Inverse(this decimal value)
        {
            decimal inverse = 1 / value;
            return inverse.Round4();
        }
    }
}