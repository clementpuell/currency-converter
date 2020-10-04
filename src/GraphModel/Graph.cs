using System;
using System.Collections.Generic;
using System.Linq;
using CurrencyConverter.Models;

namespace CurrencyConverter.GraphModel
{
    /// <summary>
    /// A graph of exhcnage rates, representing all possible direct conversion between currencies.
    /// </summary>
    public class Graph
    {
        private readonly InputFile file;
        private HashSet<Node> _nodes = new HashSet<Node>();
        private HashSet<Edge> _edges = new HashSet<Edge>();

        public ISet<Node> Nodes => _nodes;
        public ISet<Edge> Edges => _edges;

        public Graph(InputFile file)
        {
            this.file = file;
        }

        /// <summary>
        /// Build the graph based on the given input file.
        /// </summary>
        public void Build()
        {
            // De-duplicating will be done automatically by the sets, which will store each item at most once.
            // Two nodes for the same currency are considered identical.
            // Two edges for the same nodes are considered identical, whatever their orientation.
            foreach (var line in file.Rates)
            {
                // Either create new nodes, or reuse existing ones for the same currency.
                var from = GetOrCreateNode(line.from);
                var to = GetOrCreateNode(line.to);
                _nodes.Add(from);
                _nodes.Add(to);

                var edge = new Edge(from, to, line.rate);
                if (_edges.Add(edge))
                {
                    // Connect the current edge to its nodes
                    from.Connect(edge);
                    to.Connect(edge);
                }
                else
                {
                    // Not mandatory but check that an opposite exchange rate is indeed the inverse of an existing one.
                    _edges.TryGetValue(edge, out var other);
                    if (!edge.HasEquivalentRate(other))
                    {
                        throw new ApplicationException($"{edge} is equivalent to {other} but their rates are not inversed.");
                    }
                }
            }
        }

        private Node GetOrCreateNode(Currency currency)
        {
            var newNode = new Node(currency);
            _nodes.TryGetValue(newNode, out var node);
            node ??= newNode;
            return node;
        }

        /// <summary>
        /// Gets the single node representing this currency.
        /// </summary>
        public Node FindNode(Currency currency) => _nodes.SingleOrDefault(n => n.Currency == currency);
    }
}
