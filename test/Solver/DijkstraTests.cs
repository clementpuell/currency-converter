using CurrencyConverter.Graph;
using CurrencyConverter.Models;
using CurrencyConverter.Solver;
using Xunit;

namespace CurrencyConverter.Tests.Solver
{
    public class DijkstraTests
    {
        [Fact]
        public void Should_Solve_Direct_Graph()
        {
            var graph = new ConversionGraph(("EUR", "JPY", default));

            graph.Build();
            var dijkstra = new Dijkstra(graph);
            var path = dijkstra.Solve("EUR", "JPY");

            AssertPathIs(path, ("EUR", "JPY"));
        }

        [Fact]
        public void Should_Solve_Complex_Graph()
        {
            var graph = new ConversionGraph(
                ("EUR", "AUD", default),
                ("AUD", "GBP", default),
                ("AUD", "CHF", default),
                ("CHF", "INR", default),
                ("INR", "KRW", default),
                ("INR", "USD", default),
                ("USD", "GBP", default),
                ("CAD", "USD", default),
                ("MXN", "CAD", default),
                ("JPY", "CAD", default),
                ("KWU", "THB", default),
                ("USD", "INR", default),
                ("CAD", "JPY", default),
                ("JPY", "INR", default));

            graph.Build();
            var dijkstra = new Dijkstra(graph);
            var path = dijkstra.Solve("EUR", "JPY");

            AssertPathIs(path,
                ("EUR", "AUD"),
                ("AUD", "CHF"),
                ("CHF", "INR"),
                ("INR", "JPY"));
        }

        [Fact]
        public void Should_Not_Solve_For_Unreachable_Node()
        {
            var graph = new ConversionGraph(
                ("EUR", "AUD", default),
                ("CAD", "JPY", default));

            graph.Build();
            var dijkstra = new Dijkstra(graph);
            var path = dijkstra.Solve("EUR", "JPY");

            Assert.False(path.IsValid());
        }

        [Fact]
        public void Should_Not_Solve_For_Unknown_Starting_Node()
        {
            var graph = new ConversionGraph(("CAD", "JPY", default));

            graph.Build();
            var dijkstra = new Dijkstra(graph);
            var path = dijkstra.Solve("EUR", "JPY");

            Assert.False(path.IsValid());
        }

        [Fact]
        public void Should_Not_Solve_For_Unknown_Destination_Node()
        {
            var graph = new ConversionGraph(("EUR", "CAD", default));

            graph.Build();
            var dijkstra = new Dijkstra(graph);
            var path = dijkstra.Solve("EUR", "JPY");

            Assert.False(path.IsValid());
        }

        /// <summary>
        /// Check that the found path is composed exactly of the given hop from one currency to the next.
        /// </summary>
        private void AssertPathIs(Path path, params (Currency from, Currency to)[] hops)
        {
            Assert.Equal(hops.Length, path.Length);

            int i = 0;
            foreach (Edge edge in path)
            {
                var (from, to) = hops[i];
                Assert.Equal(from, edge.From.Currency);
                Assert.Equal(to, edge.To.Currency);
                i++;
            }
        }
    }
}