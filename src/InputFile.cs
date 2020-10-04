using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using CurrencyConverter.Models;

namespace CurrencyConverter
{
    public class InputFile
    {
        private readonly string path;

        public Currency SourceCurrency { get; private set; }

        public Currency TargetCurrency { get; private set; }

        public int Amount { get; private set; }

        public IList<(Currency from, Currency to, decimal rate)> Rates { get; private set; } = new List<(Currency, Currency, decimal)>();

        public InputFile(string path)
        {
            this.path = path;
        }

        public async Task Read()
        {
            string cd = Directory.GetCurrentDirectory();
            string fullPath = System.IO.Path.Combine(cd, path);
            string[] lines = await File.ReadAllLinesAsync(fullPath);

            if (lines.Length < 2)
            {
                throw new ApplicationException("The input file must contains at least 2 lines");
            }

            string[] first = lines[0].Split(";");

            SourceCurrency = new Currency(first[0]);
            TargetCurrency = new Currency(first[2]);
            Amount = int.Parse(first[1]);

            string second = lines[1];
            int nbRates = int.Parse(second);

            if (nbRates > 0)
            {
                for (int i = 2; i != lines.Length; i++)
                {
                    string[] rateParts = lines[i].Split(";");
                    var from = new Currency(rateParts[0]);
                    var to = new Currency(rateParts[1]);
                    decimal rate = Decimal.Parse(rateParts[2], CultureInfo.InvariantCulture);
                    Rates.Add((from, to, rate));
                }
            }
        }
    }
}
