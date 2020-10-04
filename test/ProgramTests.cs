using System.Threading.Tasks;
using Xunit;

namespace CurrencyConverter.Tests
{
    public class ProgramTests
    {
        private const string sampleFile = "./TestFiles/sample-input-file.csv";

        [Fact]
        public async Task Should_Convert_Given_Sample()
        {
            var result = await Main(sampleFile);
            Assert.Equal(59033, result);
        }

        [Fact]
        public async Task Should_Convert_Complex_Sample()
        {
        }

        private async Task<int> Main(string filePath)
        {
            var file = await Program.ReadFile(filePath);
            var graph = Program.BuildGraph(file);
            var shortestPath = Program.FindPath(graph, file.SourceCurrency, file.TargetCurrency);
            var result = Program.ConvertAmount(shortestPath, file.Amount);
            return result;
        }
    }
}
