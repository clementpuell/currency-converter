using CurrencyConverter.GraphModel;
using CurrencyConverter.Models;
using Xunit;

namespace CurrencyConverter.Tests.Models
{
    public class PathTests
    {
        [Fact]
        public void Should_Add_Some_Edges()
        {
            // EUR --1.5--> JPY --1.8--> USD
            var path = new Path("EUR", "USD");
            path.Prepend(new Edge(new Node("JPY"), new Node("USD"), 1.8m));
            path.Prepend(new Edge(new Node("EUR"), new Node("JPY"), 1.5m));

            Assert.Equal(2, path.Length);
        }

        [Fact]
        public void Should_Convert_Amount()
        {
            // EUR --1.5--> JPY --1.8--> USD
            var path = new Path("EUR", "USD");
            path.Prepend(new Edge(new Node("JPY"), new Node("USD"), 1.8m));
            path.Prepend(new Edge(new Node("EUR"), new Node("JPY"), 1.5m));

            int result = path.ConvertThrough(100);

            Assert.Equal(270, result);
        }

        [Fact]
        public void Should_Convert_Amount_With_Rounding()
        {
            // EUR --0.3157--> JPY --1.4395--> USD --2.6482--> INR
            var path = new Path("EUR", "INR");
            path.Prepend(new Edge(new Node("USD"), new Node("INR"), 2.6482m));
            path.Prepend(new Edge(new Node("JPY"), new Node("USD"), 1.4395m));
            path.Prepend(new Edge(new Node("EUR"), new Node("JPY"), 0.3157m));

            int result = path.ConvertThrough(1000);

            Assert.Equal(1203, result);

            path.ToString();
        }

        [Fact]
        public void Should_Be_Valid()
        {
            // EUR --1.5--> JPY --1.8--> USD
            var path = new Path("EUR", "USD");
            path.Prepend(new Edge(new Node("JPY"), new Node("USD"), 1.8m));
            path.Prepend(new Edge(new Node("EUR"), new Node("JPY"), 1.5m));
            
            Assert.True(path.IsValid());
        }

        [Fact]
        public void Should_Be_Invalid()
        {
            // EUR --1.5--> JPY // INR --1.8--> USD
            var path = new Path("EUR", "USD");
            path.Prepend(new Edge(new Node("INR"), new Node("USD"), 1.8m));
            path.Prepend(new Edge(new Node("EUR"), new Node("JPY"), 1.5m));

            Assert.False(path.IsValid());
        }
    }
}