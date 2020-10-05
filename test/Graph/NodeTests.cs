using System;
using CurrencyConverter.Graph;
using Xunit;

namespace CurrencyConverter.Tests.Graph
{
    public class NodeTests
    {
        Node eur, usd, jpy;

        public NodeTests()
        {
            eur = new Node("EUR");
            usd = new Node("USD");
            jpy = new Node("JPY");
        }

        [Fact]
        public void Should_Connect_Edge()
        {
            eur.Connect(new Edge(eur, usd, default));
            
            Edge edge = Assert.Single(eur.Edges);
            Assert.Equal(eur, edge.From);
            Assert.Equal(usd, edge.To);
        }

        [Fact]
        public void Should_Refuse_To_Connect_Edge()
        {
            Assert.Throws<ApplicationException>(() => eur.Connect(new Edge(usd, jpy, default)));
        }

        [Fact]
        public void Should_Get_Neighbors()
        {
            eur.Connect(new Edge(eur, usd, default));
            eur.Connect(new Edge(eur, jpy, default));

            Assert.Contains(usd, eur.Neighbors);
            Assert.Contains(jpy, eur.Neighbors);
        }

        [Fact]
        public void Should_Get_Original_Edge_To_Neighbor()
        {
            eur.Connect(new Edge(eur, usd, default));
            eur.Connect(new Edge(eur, jpy, default));

            Edge edgeToUsd = eur.GetOrientedEdgeTo(usd);

            Assert.Equal(eur, edgeToUsd.From);
            Assert.Equal(usd, edgeToUsd.To);
        }

        [Fact]
        public void Should_Get_Reversed_Edge_To_Neighbor()
        {
            eur.Connect(new Edge(usd, eur, default));
            eur.Connect(new Edge(jpy, eur, default));

            Edge edgeToUsd = eur.GetOrientedEdgeTo(usd);

            Assert.Equal(eur, edgeToUsd.From);
            Assert.Equal(usd, edgeToUsd.To);
        }

        [Fact]
        public void Should_Be_Equals()
        {
            Assert.Equal(usd, usd);
            Assert.Equal(new Node("USD"), usd);
        }

        [Fact]
        public void Sould_Be_Different()
        {
            Assert.NotEqual(eur, usd);
        }
    }
}
