using System;
using CurrencyConverter.Extensions;

namespace CurrencyConverter.Graph
{
    /// <summary>
    /// The edge of a graph, representing an exchange rate between two currencies.
    /// </summary>
    public class Edge : IEquatable<Edge>
    {
        public Node From { get; private set; }

        public Node To { get; private set; }

        public decimal Rate { get; private set; }

        public Edge(Node from, Node to, decimal rate)
        {
            From = from;
            To = to;
            Rate = rate;
        }

        /// <summary>
        /// Inverse the exchange rate in order to convert from target currency to source currency. 
        /// </summary>
        public void Reverse()
        {
            var oldFrom = From;
            From = To;
            To = oldFrom;
            Rate = Rate.Inverse();
        }

        /// <summary>
        /// Check that two edges are equivalent, even if reversed.
        /// </summary>
        public bool HasEquivalentRate(Edge other)
        {
            if (From == other.From && To == other.To)
            {
                return Rate == other.Rate;
            }
            
            return Rate.Inverse() == other.Rate || Rate == other.Rate.Inverse();
        }

        public override string ToString() => $"Edge {From?.Currency}--->{To?.Currency}@{Rate}";

        public bool Equals(Edge other) =>
            (From == other.From && To == other.To) ||
            (From == other.To && To == other.From);
        public override bool Equals(object obj) => obj is Edge e && Equals(e);
        public override int GetHashCode() => this.From.GetHashCode() + this.To.GetHashCode();
        public static bool operator ==(Edge a, Edge b) => a?.Equals(b) ?? b is null;
        public static bool operator !=(Edge a, Edge b) => !(a == b);
    }
}