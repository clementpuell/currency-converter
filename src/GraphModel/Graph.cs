using System;
using System.Collections.Generic;
using System.Linq;
using CurrencyConverter.Models;

namespace CurrencyConverter.GraphModel
{
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

        public void Build()
        {
            // De-duplicating will be done automatically by the sets, which will not add the same item twice.
            // Two nodes for the same currency are considered identical.
            // Two edges for the same nodes are considered identical, whatever their orientation.
            foreach (var line in file.Rates)
            {
                var newFromNode = new Node(line.from);
                _nodes.TryGetValue(newFromNode, out var from);
                from ??= newFromNode;

                var newToNode = new Node(line.to);
                _nodes.TryGetValue(newToNode, out var to);
                to ??= newToNode;

                _nodes.Add(from);
                _nodes.Add(to);

                var edge = new Edge(from, to, line.rate);
                if (_edges.Add(edge))
                {
                    from.Connect(edge);
                    to.Connect(edge);
                }
                else
                {
                    _edges.TryGetValue(edge, out var other);
                    if (!edge.HasEquivalentRate(other))
                    {
                        throw new ApplicationException($"{edge} is equivalent to {other} but their rates are not.");
                    }
                }
            }
        }

        public Node FindNode(Currency currency) => _nodes.SingleOrDefault(n => n.Currency == currency);
    }
}
