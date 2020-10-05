using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CurrencyConverter.Extensions;
using CurrencyConverter.Graph;
using CurrencyConverter.Models;

namespace CurrencyConverter.Solver
{
    /// <summary>
    /// Represents of path of edges from a source currency to a target currency.
    /// </summary>
    public class Path : IEnumerable<Edge>
    {
        private IList<Edge> edges = new List<Edge>();

        public Currency From { get; }

        public Currency To { get; }

        public int Length { get; private set; }

        public Path(Currency from, Currency to)
        {
            From = from;
            To = to;
        }

        /// <summary>
        /// Insert a segment at the beginning of the path.
        /// </summary>
        public void Prepend(Edge previousEdge)
        {
            edges.Insert(0, previousEdge);
            Length++;
        }

        /// <summary>
        /// Find the equivalent amount in the target currency by running the initial amount through the path of exchange rates.
        /// </summary>
        public int Convert(int amount) =>
            edges
                .Aggregate<Edge, decimal>(
                    amount,
                    (aggregate, edge) =>
                    {
                        // Each rate is multiplied with the next then rounded
                        aggregate *= edge.Rate;
                        return aggregate.Round4();
                    })
                .Round0();

        /// <summary>
        /// Check that all segments of the path are properly connected from start to finish.
        /// </summary>
        public bool IsValid()
        {
            var next = From;
            foreach (var edge in edges)
            {
                if (next != edge.From.Currency)
                {
                    return false;
                }

                next = edge.To.Currency;
            }

            return next == To;
        }

        public override string ToString() => $"Path from {From} to {To} of length {Length}: {{ {string.Join(" ; ", edges)} }}";

        public IEnumerator<Edge> GetEnumerator() => edges.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => edges.GetEnumerator();
    }
}
