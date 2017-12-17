using System;
using System.IO;

namespace ApplicationInsights.DataGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            var filePath = "C:/Mik/stockData.json";
            var from = DateTimeOffset.Now - TimeSpan.FromHours(48);

            var generator = new DataGenerator();
            var dataP1 = generator.GenerateStockData("P1", from, DateTimeOffset.Now);
            var dataP2 = generator.GenerateStockData("P2", from, DateTimeOffset.Now);
            var dataP3 = generator.GenerateStockData("P3", from, DateTimeOffset.Now);
            var dataP4 = generator.GenerateStockData("P4", from, DateTimeOffset.Now);
            var dataP5 = generator.GenerateStockData("P5", from, DateTimeOffset.Now);
            var dataP6 = generator.GenerateStockData("P6", from, DateTimeOffset.Now);
            File.AppendAllLines(filePath, dataP1);
            File.AppendAllLines(filePath, dataP2);
            File.AppendAllLines(filePath, dataP3);
            File.AppendAllLines(filePath, dataP4);
            File.AppendAllLines(filePath, dataP5);
            File.AppendAllLines(filePath, dataP6);

            var numberOfLines = dataP1.Count + dataP2.Count + dataP3.Count + dataP4.Count + dataP5.Count + dataP6.Count;
            Console.WriteLine($"Successfuly wrote {numberOfLines} lines to file {filePath}.");
            Console.ReadKey();
        }
    }
}
