using System.Collections.Generic;
using System.Linq;
using CurrencyConverter.Graph;
using CurrencyConverter.Models;

namespace CurrencyConverter.Solver
{
    /// <summary>
    /// Implements the Dijkstra's algorithm to find the shortest path between two given currencies of a graph.
    /// </summary>
    public class Dijkstra : IPathSolver
    {
        private readonly ConversionGraph graph;

        private Path shortestPath;

        /// <summary>
        /// List of unvisited nodes, along their shortest distance to the start.
        /// </summary>
        private IDictionary<Node, int> unvisited;

        /// <summary>
        /// List of shortest distances to the start.
        /// </summary>
        private IDictionary<Node, int> distances;

        /// <summary>
        /// List of previous node through the shortest path.
        /// </summary>
        private IDictionary<Node, Node> previous;

        public Dijkstra(ConversionGraph graph)
        {
            this.graph = graph;
        }

        /// <summary>
        /// Find the shortest path between the two given currencies.
        /// </summary>
        public Path Solve(Currency from, Currency to)
        {
            shortestPath = new Path(from, to);
            var fromNode = graph.FindNode(from);
            var toNode = graph.FindNode(to);

            if (fromNode != null && toNode != null)
            {
                Init(fromNode);
                Explore(toNode);
                ConstructPath(fromNode, toNode);
            }

            return shortestPath;
        }

        /// <summary>
        /// Mark nodes unvisited, with max distances and with unknown previous nodes.
        /// </summary>
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

            SetDistance(start, 0);
        }

        /// <summary>
        /// Explore the whole graph one neighbor at a time, keeping track its shortest distance to start.
        /// </summary>
        private void Explore(Node to)
        {
            while (unvisited.Count != 0)
            {
                var currentNode = FindNextUnvisitedNode();
                unvisited.Remove(currentNode);

                // Early return if the destination has already been reached
                if (currentNode == to)
                {
                    return;
                }

                foreach (var neighbor in GetUnvisitedNeighbors(currentNode))
                {
                    int alternative = distances[currentNode] + 1;
                    if (alternative < distances[neighbor])
                    {
                        // Shorter path found
                        SetDistance(neighbor, alternative);
                        previous[neighbor] = currentNode;
                    }
                }
            }
        }

        /// <summary>
        /// Reconstruct the shortest path between the two given nodes, starting from the end.
        /// The path consist of oriented edges, not nodes, as edges contains the currency rates.
        /// </summary>
        private void ConstructPath(Node from, Node to)
        {
            var currentNode = to;
            var previousNode = previous[currentNode];
            while (previousNode != null)
            {
                // The edge may be reversed here
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

        /// <summary>
        /// Gets the next node to explore, the one with the minimum distance.
        /// </summary>
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
