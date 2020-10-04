using System;
using System.Linq;
using System.Threading.Tasks;
using CurrencyConverter.Models;
using Xunit;

namespace CurrencyConverter.Tests
{
    public class InputFileTests
    {
        private string simpleFile = "./simple-input-file.csv";
        private string emptyFile = "./empty-input-file.csv";
        private string invalidFilePath = "./test-invalid-input-file.csv";

        [Fact]
        public async Task Should_Read_Valid_File()
        {
            var file = new InputFile(simpleFile);
            await file.Read();

            var expectedSourceCurrency = "EUR";
            var expectedTargetCurrency = "JPY";
            var expectedAmount = 550m;
            var expectedNbRates = 3;
            var expectedFirstRate = ("AUD", "CHF", 0.9661m);
            var expectedLastRate = ("EUR", "CHF", 1.2053m);

            Assert.Equal(expectedSourceCurrency, file.SourceCurrency);
            Assert.Equal(expectedTargetCurrency, file.TargetCurrency);
            Assert.Equal(expectedAmount, file.Amount);
            Assert.Equal(expectedNbRates, file.Rates.Count());
            Assert.Equal(expectedFirstRate, file.Rates.First());
            Assert.Equal(expectedLastRate, file.Rates.Last());
        }

        [Fact]
        public async Task Should_Read_Valid_Empty_File()
        {
            var file = new InputFile(emptyFile);
            await file.Read();

            var expectedCurrency = "";
            var expectedAmount = 0m;
            var expectedNbRates = 0;

            Assert.Equal(expectedCurrency, file.SourceCurrency);
            Assert.Equal(expectedCurrency, file.TargetCurrency);
            Assert.Equal(expectedAmount, file.Amount);
            Assert.Equal(expectedNbRates, file.Rates.Count());
        }

        [Fact]
        public async Task Should_Throw_On_Invalid_File()
        {
            var file = new InputFile(invalidFilePath);
            await Assert.ThrowsAnyAsync<Exception>(() => file.Read());
        }
    }
}