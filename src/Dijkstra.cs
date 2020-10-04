using System;
using System.Collections.Generic;
using System.Linq;
using CurrencyConverter.GraphModel;
using CurrencyConverter.Models;

namespace CurrencyConverter
{
    public class Dijkstra
    {
        private readonly Graph graph;

        private Path shortestPath;

        private Node currentNode;
        private IDictionary<Node, int> unvisited;
        private IDictionary<Node, int> distances;
        private IDictionary<Node, Node> previous;

        public Dijkstra(Graph graph)
        {
            this.graph = graph;
        }

        public Path Solve(Currency from, Currency to)
        {
            shortestPath = new Path(from, to);
            var fromNode = graph.FindNode(from);
            var toNode = graph.FindNode(to);

            Init(fromNode);
            Compute(toNode);
            ConstructPath(fromNode, toNode);

            if (!shortestPath.IsValid())
            {
                throw new ApplicationException($"Oops, looks like the found path is not valid: {shortestPath}");
            }

            return shortestPath;
        }

        private void Init(Node start)
        {
            unvisited = new Dictionary<Node, int>();
            distances = new Dictionary<Node, int>();
            previous = new Dictionary<Node, Node>();
            foreach (var node in graph.Nodes)
            {
                unvisited.Add(node, int.MaxValue);
                distances.Add(node, int.MaxValue);
                previous.Add(node, null);
            }

            currentNode = start;
            SetDistance(start, 0);
        }

        private void Compute(Node to)
        {
            while (unvisited.Count != 0)
            {
                currentNode = FindNextUnvisitedNode();
                unvisited.Remove(currentNode);

                // Early return if the destination node has been reached
                if (currentNode == to)
                {
                    return;
                }

                foreach (var neighbor in GetUnvisitedNeighbors(currentNode))
                {
                    int alternative = distances[currentNode] + 1;
                    if (alternative < distances[neighbor])
                    {
                        SetDistance(neighbor, alternative);
                        previous[neighbor] = currentNode;
                    }
                }
            }
        }

        private void ConstructPath(Node from, Node to)
        {
            var currentNode = to;
            var previousNode = previous[currentNode];
            while (previousNode != null)
            {
                var orientedEdge = previousNode.GetOrientedEdgeTo(currentNode);
                shortestPath.Prepend(orientedEdge);
                currentNode = previousNode;
                previousNode = previous[previousNode];
            }
        }

        private void SetDistance(Node n, int distance)
        {
            distances[n] = distance;
            if (unvisited.ContainsKey(n))
            {
                unvisited[n] = distance;
            }
        }

        private Node FindNextUnvisitedNode()
        {
            return unvisited.OrderBy(x => x.Value).First().Key;
        }

        private IEnumerable<Node> GetUnvisitedNeighbors(Node n)
        {
            return unvisited.Keys.Intersect(n.Neighbors).ToList();
        }
    }
}
