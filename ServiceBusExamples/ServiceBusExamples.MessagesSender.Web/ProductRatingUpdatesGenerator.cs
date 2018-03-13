using ServiceBusExamples.MessagesSender.NetCore.Web.Dto;
using System;
using System.Collections.Generic;

namespace ServiceBusExamples.MessagesSender.NetCore.Web
{
    public static class ProductRatingUpdatesGenerator
    {
        public static List<ProductRatingUpdateMessage> GetMessages(int numberOfMessages)
        {
            var messages = new List<ProductRatingUpdateMessage>();
            var rand = new Random();
            for (int i = 0; i < numberOfMessages; i++)
            {
                messages.Add(new ProductRatingUpdateMessage
                {
                    ProductId = rand.Next(1, 50),
                    SellerId = rand.Next(1, 5),
                    RatingCount = 10,
                    RatingSum = rand.Next(10, 50)
                });
            }

            return messages;
        }
    }
}
