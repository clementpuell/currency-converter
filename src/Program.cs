using System;
using System.Threading.Tasks;
using CurrencyConverter.GraphModel;
using CurrencyConverter.Models;

namespace CurrencyConverter
{
    class Program
    {
        static async Task Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Usage: CurrencyConverter.exe <path to file>");
                return;
            }

            var file = await ReadFile(args[0]);
            var graph = BuildGraph(file);
            var shortestPath = FindPath(graph, file.SourceCurrency, file.TargetCurrency);
            var result = ConvertAmount(shortestPath, file.Amount);

            string outAmount = $"{file.Amount} {file.SourceCurrency} is {result} {file.TargetCurrency}.";
            string outPath = $"Conversion path: {shortestPath}.";
            Console.WriteLine(outAmount);
            Console.WriteLine(outPath);
        }

        static async Task<InputFile> ReadFile(string path)
        {
            var file = new InputFile(path);
            await file.Read();
            return file;
        }

        static Graph BuildGraph(InputFile file)
        {
            var graph = new Graph(file);
            graph.Build();
            return graph;
        }

        static Path FindPath(Graph graph, Currency from, Currency to)
        {
            var dijkstra = new Dijkstra(graph);
            return dijkstra.Solve(from, to);
        }

        static int ConvertAmount(Path path, int amount)
        {
            return path.ConvertThrough(amount);
        }
    }
}
