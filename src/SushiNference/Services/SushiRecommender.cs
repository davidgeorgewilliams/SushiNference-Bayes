using Microsoft.ML.Probabilistic.Distributions;
using Microsoft.ML.Probabilistic.Models;
using Microsoft.ML.Probabilistic.Math;
using System;
using System.Collections.Generic;
using SushiNference.Models;

public class SushiRecommender
{
    private List<SushiItem> sushiItems;
    private List<UserPreference> userPreferences;

    public SushiRecommender(List<SushiItem> sushiItems, List<UserPreference> userPreferences)
    {
        this.sushiItems = sushiItems;
        this.userPreferences = userPreferences;
    }

    public void RunInference()
    {
        Console.WriteLine($"Loaded {sushiItems.Count} sushi items");
        Console.WriteLine($"Loaded preferences for {userPreferences.Count} users");

        // Create a simple collaborative filtering model
        var numUsers = userPreferences.Count;
        var numItems = sushiItems.Count;
        var userFactors = new Variable<Vector>[numUsers];
        var itemFactors = new Variable<Vector>[numItems];
        var ratings = new Variable<double>[numUsers, numItems];

        for (int u = 0; u < numUsers; u++)
        {
            userFactors[u] = Variable.VectorGaussianFromMeanAndPrecision(
                Vector.FromArray(new double[] { 0, 0 }),
                PositiveDefiniteMatrix.IdentityScaledBy(2, 1)).Named($"user{u}");
        }

        for (int i = 0; i < numItems; i++)
        {
            itemFactors[i] = Variable.VectorGaussianFromMeanAndPrecision(
                Vector.FromArray(new double[] { 0, 0 }),
                PositiveDefiniteMatrix.IdentityScaledBy(2, 1)).Named($"item{i}");
        }

        for (int u = 0; u < numUsers; u++)
        {
            for (int i = 0; i < numItems; i++)
            {
                ratings[u, i] = Variable.GaussianFromMeanAndPrecision(
                    Variable.InnerProduct(userFactors[u], itemFactors[i]), 1).Named($"rating{u},{i}");
            }
        }

        // Observe the known ratings
        for (int u = 0; u < numUsers; u++)
        {
            for (int i = 0; i < numItems; i++)
            {
                ratings[u, i].ObservedValue = userPreferences[u].Ratings[i];
            }
        }

        // Perform inference
        var engine = new InferenceEngine();
        Console.WriteLine("Performing inference...");

        // Infer factors for a specific user (e.g., user 0)
        var userFactorPosterior = engine.Infer<VectorGaussian>(userFactors[0]);
        Console.WriteLine($"User 0 factor posterior mean: {userFactorPosterior.GetMean()}");

        // Predict ratings for user 0
        Console.WriteLine("Predicted ratings for user 0:");
        for (int i = 0; i < numItems; i++)
        {
            var predictedRating = engine.Infer<Gaussian>(ratings[0, i]);
            Console.WriteLine($"{sushiItems[i].Name}: {predictedRating.GetMean():F2}");
        }
    }
}