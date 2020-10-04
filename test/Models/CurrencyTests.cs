using Xunit;

namespace CurrencyConverter.Tests.Models
{
    public class CurrencyTests
    {
        [Fact]
        public void Should_Be_Equals()
        {
            var currency1 = "EUR";
            var currency2 = "EUR";
            Assert.Equal(currency1, currency2);
        }

        [Fact]
        public void Should_Be_Different()
        {
            var currency1 = "EUR";
            var currency2 = "USD";
            Assert.NotEqual(currency1, currency2);
        }
    }
}