using System;
using System.Threading.Tasks;
using CurrencyConverter.GraphModel;

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
        }

        static async Task<InputFile> ReadFile(string path)
        {
            var file = new InputFile("file1.csv");
            await file.Read();
            return file;
        }

        static Graph BuildGraph(InputFile file)
        {
            var graph = new Graph(file);
            graph.Build();
            return graph;
        }

        static void FindPath()
        {

        }

        static void ComputeAmount()
        {

        }
    }
}
