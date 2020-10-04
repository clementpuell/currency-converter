using CurrencyConverter.GraphModel;
using Xunit;

namespace CurrencyConverter.Tests.GraphModel
{
    public class GraphTests
    {
        private readonly InputFile file;

        public GraphTests()
        {
            file = new InputFile("TestFiles/sample-input-file.csv");
            file.Read().Wait();
        }

        [Fact]
        public void Should_Build_Sample_Graph()
        {
            var graph = new Graph(file);

            var expectedEge1 = new Edge(new Node("AUD"), new Node("CHF"), 0.9661m);
            var expectedEge2 = new Edge(new Node("JPY"), new Node("KRW"), 13.1151m);
            var expectedEge3 = new Edge(new Node("EUR"), new Node("CHF"), 1.2053m);
            var expectedEge4 = new Edge(new Node("AUD"), new Node("JPY"), 86.0305m);
            var expectedEge5 = new Edge(new Node("EUR"), new Node("USD"), 1.2989m);
            var expectedEge6 = new Edge(new Node("JPY"), new Node("INR"), 0.6571m);

            graph.Build();

            Assert.Equal(7, graph.Nodes.Count);
            Assert.Equal(6, graph.Edges.Count);
            Assert.Contains(expectedEge1, graph.Edges);
            Assert.Contains(expectedEge2, graph.Edges);
            Assert.Contains(expectedEge3, graph.Edges);
            Assert.Contains(expectedEge4, graph.Edges);
            Assert.Contains(expectedEge5, graph.Edges);
            Assert.Contains(expectedEge6, graph.Edges);
        }

        [Fact]
        public void Should_Find_Node()
        {
            var graph = new Graph(file);
            graph.Build();

            var eurNode = graph.FindNode("EUR");
            Assert.Equal("EUR", eurNode.Currency);

            var inrNode = graph.FindNode("INR");
            Assert.Equal("INR", inrNode.Currency);
        }

        [Fact]
        public void Should_Not_Find_Node_When_Not_Exists()
        {
            var graph = new Graph(file);
            graph.Build();

            var kwuNode = graph.FindNode("KWU");
            Assert.Null(kwuNode);
        }
    }
}
