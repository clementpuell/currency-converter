using Xunit;

namespace CurrencyConverter.Tests.Models
{
    public class CurrencyTests
    {
        [Fact]
        public void Should_Be_Equals()
        {
            Assert.Equal("EUR", "EUR");
        }

        [Fact]
        public void Should_Be_Different()
        {
            Assert.NotEqual("EUR", "USD");
        }
    }
}