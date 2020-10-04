using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using CurrencyConverter.Models;

namespace CurrencyConverter
{
    /// <summary>
    /// Read the input file of the program.
    /// </summary>
    public class InputFile
    {
        private readonly string path;

        public Currency SourceCurrency { get; private set; }

        public Currency TargetCurrency { get; private set; }

        public int Amount { get; private set; }

        /// <summary>
        /// Store rates as tuples of source currency, tagret currency, exchange rate.
        /// </summary>
        public IList<(Currency from, Currency to, decimal rate)> Rates { get; private set; } = new List<(Currency, Currency, decimal)>();

        public InputFile(string path)
        {
            this.path = path;
        }

        public async Task Read()
        {
            string cd = Directory.GetCurrentDirectory();
            string fullPath = Path.Combine(cd, path);
            string[] lines = await File.ReadAllLinesAsync(fullPath);

            if (lines.Length < 2)
            {
                throw new ApplicationException("The input file must contains at least 2 lines");
            }

            // First line
            string[] first = lines[0].Split(";");
            SourceCurrency = first[0];
            TargetCurrency = first[2];
            Amount = int.Parse(first[1]);

            // Second line ignore, reading rates until end of file

            for (int i = 2; i != lines.Length; i++)
            {
                string[] rateParts = lines[i].Split(";");
                // Currencies created implicitly here
                var from = rateParts[0];
                var to = rateParts[1];
                decimal rate = Decimal.Parse(rateParts[2], CultureInfo.InvariantCulture);
                Rates.Add((from, to, rate));
            }
        }
    }
}
