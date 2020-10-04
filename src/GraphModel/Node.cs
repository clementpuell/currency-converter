using System;
using System.Collections.Generic;
using System.Linq;
using CurrencyConverter.Models;

namespace CurrencyConverter.GraphModel
{
    public class Node : IEquatable<Node>
    {
        public Currency Currency { get; }

        public ISet<Edge> Edges { get; } = new HashSet<Edge>();

        public IEnumerable<Node> Neighbors => Edges.Select(e => this == e.From ? e.To : e.From);

        public Node(Currency currency)
        {
            Currency = currency;
        }

        public void Connect(Edge edge) => Edges.Add(edge);

        public Edge GetOrientedEdgeTo(Node neighbor)
        {
            var edge = Edges.SingleOrDefault(e => e.From == neighbor || e.To == neighbor);
            if (edge?.From != this)
            {
                edge?.Reverse();
            }

            return edge;
        }

        // public Node AdvanceTowards(Currency currency)
        // {
        //     throw new NotImplementedException();
        // }

        public override string ToString() => $"Node {Currency} (connected to {string.Join(",", Edges)})";

        public bool Equals(Node other) => Currency == other?.Currency;
        public override bool Equals(object obj) => obj is Node n && Equals(n);
        public override int GetHashCode() => this.Currency.GetHashCode();
        public static bool operator ==(Node a, Node b) => a?.Equals(b) ?? b is null;
        public static bool operator !=(Node a, Node b) => !(a == b);
    }
}
