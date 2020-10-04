using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace CurrencyConverter.Tests
{
    public class InputFileTests
    {
        private string simpleFile = "./TestFiles/simple-input-file.csv";
        private string emptyFile = "./TestFiles/empty-input-file.csv";
        private string invalidFilePath = "./TestFiles/invalid-input-file.csv";

        [Fact]
        public async Task Should_Read_Valid_File()
        {
            var file = new InputFile(simpleFile);
            await file.Read();

            Assert.Equal("EUR", file.SourceCurrency);
            Assert.Equal("JPY", file.TargetCurrency);
            Assert.Equal(550m, file.Amount);
            Assert.Equal(3, file.Rates.Count());
            Assert.Equal(("AUD", "CHF", 0.9661m), file.Rates.First());
            Assert.Equal(("EUR", "CHF", 1.2053m), file.Rates.Last());
        }

        [Fact]
        public async Task Should_Read_Valid_Empty_File()
        {
            var file = new InputFile(emptyFile);
            await file.Read();

            Assert.Equal("", file.SourceCurrency);
            Assert.Equal("", file.TargetCurrency);
            Assert.Equal(0m, file.Amount);
            Assert.Equal(0, file.Rates.Count());
        }

        [Fact]
        public async Task Should_Throw_On_Invalid_File()
        {
            var file = new InputFile(invalidFilePath);
            await Assert.ThrowsAnyAsync<Exception>(() => file.Read());
        }
    }
}
