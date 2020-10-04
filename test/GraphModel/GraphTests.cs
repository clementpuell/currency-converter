using CurrencyConverter.GraphModel;
using CurrencyConverter.Models;
using Xunit;

namespace CurrencyConverter.Tests.GraphModel
{
    public class GraphTests
    {
        private readonly InputFile file1;

        public GraphTests()
        {
            file1 = new InputFile("test-valid-input-file-1.csv");
            file1.Read().Wait();
        }

        [Fact]
        public void Should_Build_Sample_Graph()
        {
            var graph = new Graph(file1);

            var expectedEge1 = new Edge(
                new Node("AUD"),
                new Node("CHF"),
                0.9661m);
            var expectedEge2 = new Edge(
                new Node("JPY"),
                new Node("KRW"),
                13.1151m);
            var expectedEge3 = new Edge(
                new Node("EUR"),
                new Node("CHF"),
                1.2053m);
            var expectedEge4 = new Edge(
                new Node("AUD"),
                new Node("JPY"),
                86.0305m);
            var expectedEge5 = new Edge(
                new Node("EUR"),
                new Node("USD"),
                1.2989m);
            var expectedEge6 = new Edge(
                new Node("JPY"),
                new Node("INR"),
                0.6571m);
            var expectedEge7 = new Edge(
                new Node("KRW"),
                new Node("USD"),
                0.0009m);

            graph.Build();

            Assert.Equal(7, graph.Nodes.Count);
            Assert.Equal(7, graph.Edges.Count);
            Assert.Contains(expectedEge1, graph.Edges);
            Assert.Contains(expectedEge2, graph.Edges);
            Assert.Contains(expectedEge3, graph.Edges);
            Assert.Contains(expectedEge4, graph.Edges);
            Assert.Contains(expectedEge5, graph.Edges);
            Assert.Contains(expectedEge6, graph.Edges);
        }

        [Fact]
        public void Should_Find_Starting_Node()
        {
            var graph = new Graph(file1);
            graph.Build();

            var eurNode = graph.FindNode("EUR");
            Assert.Equal("EUR", eurNode.Currency);

            var inrNode = graph.FindNode("INR");
            Assert.Equal("INR", inrNode.Currency);
        }

        [Fact]
        public void Should_Not_Find_Starting_Node_When_Not_Exists()
        {
            var graph = new Graph(file1);
            graph.Build();

            var kwuNode = graph.FindNode("KWU");
            Assert.Null(kwuNode);
        }
    }
}