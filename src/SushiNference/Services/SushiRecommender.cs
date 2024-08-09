using Microsoft.ML.Probabilistic.Algorithms;
using Microsoft.ML.Probabilistic.Distributions;
using Microsoft.ML.Probabilistic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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

    public (Gaussian[], Gaussian[]) RunInference()
    {
        Console.WriteLine($"Loaded {sushiItems.Count} sushi items");
        Console.WriteLine($"Loaded preferences for {userPreferences.Count} users");

        var userCount = new Microsoft.ML.Probabilistic.Models.Range(userPreferences.Count);
        var sushiCount = new Microsoft.ML.Probabilistic.Models.Range(sushiItems.Count);
        var ratingCount = new Microsoft.ML.Probabilistic.Models.Range(userPreferences.Count * sushiItems.Count);

        Console.WriteLine("Creating model variables...");

        var userPreferenceVars = Variable.Array<double>(userCount);
        var sushiQualityVars = Variable.Array<double>(sushiCount);

        userPreferenceVars[userCount] = Variable.GaussianFromMeanAndVariance(0, 1).ForEach(userCount);
        sushiQualityVars[sushiCount] = Variable.GaussianFromMeanAndVariance(0, 1).ForEach(sushiCount);

        var users = Variable.Array<int>(ratingCount);
        var sushis = Variable.Array<int>(ratingCount);
        var observedRatings = Variable.Array<double>(ratingCount);

        Console.WriteLine("Setting up rating model...");

        using (Variable.ForEach(ratingCount))
        {
            var u = users[ratingCount];
            var s = sushis[ratingCount];
            
            var ratingMean = userPreferenceVars[u] + sushiQualityVars[s];
            observedRatings[ratingCount] = Variable.GaussianFromMeanAndVariance(ratingMean, 0.1);
        }

        Console.WriteLine("Preparing observed data...");

        var flattenedUsers = new List<int>();
        var flattenedSushis = new List<int>();
        var flattenedRatings = new List<double>();

        for (int u = 0; u < userPreferences.Count; u++)
        {
            for (int s = 0; s < sushiItems.Count; s++)
            {
                flattenedUsers.Add(u);
                flattenedSushis.Add(s);
                flattenedRatings.Add(userPreferences[u].Ratings[s]);
            }
        }

        Console.WriteLine("Attaching observed data to the model...");

        users.ObservedValue = flattenedUsers.ToArray();
        sushis.ObservedValue = flattenedSushis.ToArray();
        observedRatings.ObservedValue = flattenedRatings.ToArray();

        Console.WriteLine("Performing inference...");

        var inferenceEngine = new InferenceEngine(new VariationalMessagePassing());
        var inferredUserPreferences = inferenceEngine.Infer<Gaussian[]>(userPreferenceVars);
        var inferredSushiQualities = inferenceEngine.Infer<Gaussian[]>(sushiQualityVars);

        Console.WriteLine("Inference completed.");

        Console.WriteLine("Inference completed. Displaying results...");

        Console.WriteLine("\nPredicted ratings for User 0 (sample of first 5 sushi items):");
        for (int i = 0; i < Math.Min(5, sushiItems.Count); i++)
        {
            var predictedRating = inferredUserPreferences[0].GetMean() + inferredSushiQualities[i].GetMean();
            Console.WriteLine($"{sushiItems[i].Name}: {predictedRating:F8}");
        }
        

        return (inferredUserPreferences, inferredSushiQualities);
    }
}
