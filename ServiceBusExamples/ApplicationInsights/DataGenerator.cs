using System;
using System.Collections.Generic;

namespace ApplicationInsights
{
    public class DataGenerator
    {
        public List<string> GenerateStockData(string productId, DateTimeOffset from, DateTimeOffset to, int itemsPerHour = 180)
        {
            var result = new List<string>();
            var actualTime = from;
            var deltaInSeconds = 3600 / itemsPerHour;
            var lastStockValue = 112;
            var rand = new Random();
            while (actualTime < to)
            {
                lastStockValue += rand.Next(-4, 5);
                var json = "{\"ProductId\": \"" + productId + "\", \"Stock\": \"" + lastStockValue
                    + "\", \"UpdatedAt\": \"" + actualTime.ToString("yyyy-MM-ddTHH:mm:ssZ") + "\"}";
                result.Add(json);

                actualTime += TimeSpan.FromSeconds(deltaInSeconds);
            }

            return result;
        }
    }
}
