using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CurrencyConverter.Extensions;
using CurrencyConverter.GraphModel;

namespace CurrencyConverter.Models
{
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

        public void Prepend(Edge previousEdge)
        {
            edges.Insert(0, previousEdge);
            Length++;
        }

        public int ConvertThrough(int amount) =>
            edges
                .Aggregate<Edge, decimal>(
                    amount,
                    (aggregate, edge) =>
                    {
                        aggregate *= edge.Rate;
                        return aggregate.Round4();
                    })
                .Round0();

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

        public override string ToString() => $"Path from {From} to {To} of length {Length}: {string.Join(" => ", edges)}";

        public IEnumerator<Edge> GetEnumerator() => edges.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => edges.GetEnumerator();
    }
}
