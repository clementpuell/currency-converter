using System;
using System.Collections.Generic;
using System.Linq;
using CurrencyConverter.Models;

namespace CurrencyConverter.Graph
{
    /// <summary>
    /// A node of the graph, representing a currency participating in exchange rates.
    /// </summary>
    public class Node : IEquatable<Node>
    {
        public Currency Currency { get; }

        public ISet<Edge> Edges { get; } = new HashSet<Edge>();

        public IEnumerable<Node> Neighbors => Edges.Select(e => this == e.From ? e.To : e.From);

        public Node(Currency currency)
        {
            Currency = currency;
        }

        public void Connect(Edge edge)
        {
            if (edge.From != this && edge.To != this)
            {
                throw new ApplicationException($"Cannot connect edge {edge} to {this} because the node does not appear on either side of the edge.");
            }

            Edges.Add(edge);
        }

        /// <summary>
        /// Gets the edge connecting this node to another one.
        /// The returned edge may be reversed to be oriented towards the neighbor.
        /// </summary>
        public Edge GetOrientedEdgeTo(Node neighbor)
        {
            var edge = Edges.SingleOrDefault(e => e.From == neighbor || e.To == neighbor);
            if (edge?.From != this)
            {
                edge?.Reverse();
            }

            return edge;
        }

        public override string ToString() => $"Node {Currency} (connected to {string.Join(",", Edges)})";

        public bool Equals(Node other) => Currency == other?.Currency;
        public override bool Equals(object obj) => obj is Node n && Equals(n);
        public override int GetHashCode() => this.Currency.GetHashCode();
        public static bool operator ==(Node a, Node b) => a?.Equals(b) ?? b is null;
        public static bool operator !=(Node a, Node b) => !(a == b);
    }
}
