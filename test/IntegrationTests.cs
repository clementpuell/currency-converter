using System.Threading.Tasks;
using Xunit;

namespace CurrencyConverter.Tests
{
    public class IntegrationTests
    {
        private const string sampleFile = "./TestFiles/sample-input-file.csv";
        private const string complexFile = "./TestFiles/complex-input-file.csv";

        [Fact]
        public async Task Should_Convert_Given_Sample()
        {
            var result = await Run(sampleFile);
            Assert.Equal(59033, result);
        }

        [Fact]
        public async Task Should_Convert_Complex_Sample()
        {
            var result = await Run(complexFile);
            Assert.Equal(1274713, result);
        }

        private async Task<int> Run(string filePath)
        {
            var file = await Program.ReadFile(filePath);
            var graph = Program.BuildGraph(file);
            var shortestPath = Program.FindPath(graph, file.SourceCurrency, file.TargetCurrency);
            Program.CheckPath(graph, shortestPath);
            return Program.ConvertAmount(shortestPath, file.Amount);
        }
    }
}
