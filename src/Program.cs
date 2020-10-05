using System;
using System.Threading.Tasks;
using CurrencyConverter.Graph;
using CurrencyConverter.Models;
using CurrencyConverter.Solver;

namespace CurrencyConverter
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            if (args.Length != 1)
            {
                WriteError("Usage: CurrencyConverter.exe <path to file>");
                return;
            }

            try
            {
                var file = await ReadFile(args[0]);
                var graph = BuildGraph(file);
                var shortestPath = FindPath(graph, file.SourceCurrency, file.TargetCurrency);
                CheckPath(graph, shortestPath);
                var result = ConvertAmount(shortestPath, file.Amount);

                WriteSuccess($"{file.Amount} {file.SourceCurrency} is {result} {file.TargetCurrency}.");
                WriteSuccess($"Conversion path: {shortestPath}.");
            }
            catch (Exception ex)
            {
                WriteError(ex.Message);
            }
        }

        /// <summary>
        /// Read input file.
        /// </summary>
        public static async Task<InputFile> ReadFile(string path)
        {
            var file = new InputFile(path);
            await file.Read();
            return file;
        }

        /// <summary>
        /// Build a graph representing all possible exchange rates.
        /// </summary>
        public static ConversionGraph BuildGraph(InputFile file)
        {
            var graph = new ConversionGraph(file);
            graph.Build();
            return graph;
        }

        /// <summary>
        /// Find the shortest path between two currencies in the graph.
        /// </summary>
        public static Path FindPath(ConversionGraph graph, Currency from, Currency to)
        {
            var solver = new Dijkstra(graph);
            return solver.Solve(from, to);
        }

        /// <summary>
        /// Validate that the found path is valid, i.e. entirely connected. Throws otherwise.
        /// </summary>
        public static void CheckPath(ConversionGraph graph, Path path)
        {
            if (!path.IsValid())
            {
                throw new ApplicationException($"The found path is not entirely connected:\r\n\t{path}\r\n\t{graph}.");
            }
        }

        /// <summary>
        /// Convert the source amount using the found path.
        /// </summary>
        public static int ConvertAmount(Path path, int amount)
        {
            return path.Convert(amount);
        }

        private static void WriteSuccess(string value)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(value);
            Console.ResetColor();
        }

        private static void WriteError(string value)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(value);
            Console.ResetColor();
        }
    }
}
