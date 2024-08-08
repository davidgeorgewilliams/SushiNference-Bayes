using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.ML.Probabilistic.Distributions;
using ScottPlot;
using SushiNference.Models;
using SushiNference.Services;

namespace SushiNference.Evaluation
{
    public class SushiRecommenderEvaluator
    {
        private SushiRecommender recommender;
        private List<UserPreference> testSet;

        public SushiRecommenderEvaluator(SushiRecommender recommender, List<UserPreference> testSet)
        {
            this.recommender = recommender;
            this.testSet = testSet;
        }

        public double CalculateRMSE(Gaussian[] inferredUserPreferences, Gaussian[] inferredSushiQualities)
        {
            double sumSquaredError = 0;
            int count = 0;

            // Create a dictionary to map UserIds to their index in inferredUserPreferences
            var userIdToIndex = new Dictionary<int, int>();
            for (int i = 0; i < inferredUserPreferences.Length; i++)
            {
                userIdToIndex[i] = i;  // Assuming the index in inferredUserPreferences corresponds to UserId
            }

            foreach (var user in testSet)
            {
                if (!userIdToIndex.TryGetValue(user.UserId, out int userIndex))
                {
                    continue; // Skip this user if we don't have inferred preferences
                }

                var validRatings = user.Ratings.Take(inferredSushiQualities.Length).ToArray();
                for (int s = 0; s < validRatings.Length; s++)
                {
                    double actualRating = validRatings[s];
                    double predictedRating = inferredUserPreferences[userIndex].GetMean() + inferredSushiQualities[s].GetMean();
                    sumSquaredError += Math.Pow(actualRating - predictedRating, 2);
                    count++;
                }
            }

            return count > 0 ? Math.Sqrt(sumSquaredError / count) : 0;
        }

        public double CalculateMAE(Gaussian[] inferredUserPreferences, Gaussian[] inferredSushiQualities)
        {
            double sumAbsoluteError = 0;
            int count = 0;

            var userIdToIndex = new Dictionary<int, int>();
            for (int i = 0; i < inferredUserPreferences.Length; i++)
            {
                userIdToIndex[i] = i;
            }

            foreach (var user in testSet)
            {
                if (!userIdToIndex.TryGetValue(user.UserId, out int userIndex))
                {
                    continue;
                }

                var validRatings = user.Ratings.Take(inferredSushiQualities.Length).ToArray();
                for (int s = 0; s < validRatings.Length; s++)
                {
                    double actualRating = validRatings[s];
                    double predictedRating = inferredUserPreferences[userIndex].GetMean() + inferredSushiQualities[s].GetMean();
                    sumAbsoluteError += Math.Abs(actualRating - predictedRating);
                    count++;
                }
            }

            return count > 0 ? sumAbsoluteError / count : 0;
        }

        public (double precision, double recall) CalculatePrecisionRecallAtK(Gaussian[] inferredUserPreferences, Gaussian[] inferredSushiQualities, int k)
        {
            double sumPrecision = 0;
            double sumRecall = 0;
            int validUserCount = 0;

            var userIdToIndex = new Dictionary<int, int>();
            for (int i = 0; i < inferredUserPreferences.Length; i++)
            {
                userIdToIndex[i] = i;
            }

            foreach (var user in testSet)
            {
                if (!userIdToIndex.TryGetValue(user.UserId, out int userIndex))
                {
                    continue;
                }

                var validRatings = user.Ratings.Take(inferredSushiQualities.Length).ToArray();

                var actualTopK = validRatings
                    .Select((rating, index) => new { Rating = rating, Index = index })
                    .OrderByDescending(x => x.Rating)
                    .Take(k)
                    .Select(x => x.Index)
                    .ToHashSet();

                var predictedRatings = Enumerable.Range(0, validRatings.Length)
                    .Select(s => inferredUserPreferences[userIndex].GetMean() + inferredSushiQualities[s].GetMean())
                    .ToList();

                var predictedTopK = predictedRatings
                    .Select((rating, index) => new { Rating = rating, Index = index })
                    .OrderByDescending(x => x.Rating)
                    .Take(k)
                    .Select(x => x.Index)
                    .ToHashSet();

                int truePositives = actualTopK.Intersect(predictedTopK).Count();
                sumPrecision += (double)truePositives / k;
                sumRecall += (double)truePositives / actualTopK.Count;
                validUserCount++;
            }

            return validUserCount > 0 ? (sumPrecision / validUserCount, sumRecall / validUserCount) : (0, 0);
        }
        public void PlotPerformance(List<double> rmseList, List<double> maeList, List<double> precisionList, List<double> recallList)
        {
            var plt = new ScottPlot.Plot();

            double[] epochs = Enumerable.Range(1, rmseList.Count).Select(i => (double)i).ToArray();

            plt.Add.Scatter(epochs, rmseList.ToArray()).LegendText = "RMSE";
            plt.Add.Scatter(epochs, maeList.ToArray()).LegendText = "MAE";
            plt.Add.Scatter(epochs, precisionList.ToArray()).LegendText = "Precision@10";
            plt.Add.Scatter(epochs, recallList.ToArray()).LegendText = "Recall@10";


            plt.Title("SushiRecommender Performance Over Epochs");
            plt.XLabel("Epoch");
            plt.YLabel("Metric Value");
            plt.Legend.Alignment = Alignment.UpperRight;

            plt.SavePng("sushi_recommender_performance.png", 600, 400);
            Console.WriteLine("Performance plot saved as sushi_recommender_performance.png");
        }
    }
}