using CurrencyConverter.Models;

namespace CurrencyConverter.Solver
{
    /// <summary>
    /// Describe an object capable to find a conversion path between two currencies of a graph.
    /// </summary>
    interface IPathSolver
    {
        /// <summary>
        /// Find the shortest path between the two given currencies.
        /// </summary>
        Path Solve(Currency from, Currency to);
    }
}
