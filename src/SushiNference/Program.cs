using System;
using SushiNference.Services;

namespace SushiNference
{
    class Program
    {
        static void Main(string[] args)
        {
            var sushiItems = DataLoader.LoadSushiItems("../../../data/sushi3b.idata");
            var userPreferences = DataLoader.LoadUserPreferences("../../../data/sushi3b.5000.10.score");

            var recommender = new SushiRecommender(sushiItems, userPreferences);
            recommender.RunInference();
        }
    }
}