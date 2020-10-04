using System;

namespace CurrencyConverter.Extensions
{
    public static class DecimalExtensions
    {
        public static decimal Inverse(this decimal value)
        {
            decimal inverse = 1 / value;
            return decimal.Round(inverse, 4, MidpointRounding.AwayFromZero);
        }
    }
}