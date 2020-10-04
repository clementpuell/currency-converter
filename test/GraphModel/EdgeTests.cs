using CurrencyConverter.Extensions;
using CurrencyConverter.GraphModel;
using Xunit;

namespace CurrencyConverter.Tests.GraphModel
{
    public class EdgeTests
    {
        private Node node1 = new Node("EUR");
        private Node node2 = new Node("USD");
        private Node node3 = new Node("JPY");

        [Fact]
        public void Should_Be_Equals()
        {
            var edge1 = new Edge(node1, node2, default);
            var edge2 = new Edge(node1, node2, default);
            Assert.Equal(edge1, edge2);
        }

        [Fact]
        public void Should_Be_Equals_When_Reversed()
        {
            var edge1 = new Edge(node1, node2, default);
            var edge2 = new Edge(node2, node1, default);
            Assert.Equal(edge1, edge2);
        }

        [Fact]
        public void Should_Be_Different()
        {
            var edge1 = new Edge(node1, node2, default);
            var edge2 = new Edge(node1, node3, default);
            Assert.NotEqual(edge1, edge2);
        }

        [Fact]
        public void Should_Reverse()
        {
            var edge1 = new Edge(node1, node2, 1.5m);
            var edge2 = new Edge(node1, node2, 1.5m);

            edge2.Reverse();

            Assert.Equal(edge1.To, edge2.From);
            Assert.Equal(edge1.From, edge2.To);
            Assert.Equal(edge1.Rate.Inverse(), edge2.Rate);
        }

        [Fact]
        public void Should_Have_Equivalent_Rate()
        {
            var edge1 = new Edge(node1, node2, 1.5m);
            var edge2 = new Edge(node2, node1, 0.6667m);
            Assert.True(edge1.HasEquivalentRate(edge2));
        }

        [Fact]
        public void Should_Not_Have_Equivalent_Rate()
        {
            var edge1 = new Edge(node1, node2, 1.5m);
            var edge2 = new Edge(node1, node2, 0.6667m);
            Assert.False(edge1.HasEquivalentRate(edge2));
        }
    }
}
