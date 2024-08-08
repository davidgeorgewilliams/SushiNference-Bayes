using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SushiNference.Models;

namespace SushiNference.Services
{
    public static class DataLoader
    {
        public static List<SushiItem> LoadSushiItems(string filePath)
        {
            var sushiItems = new List<SushiItem>();
            foreach (var line in File.ReadLines(filePath).Skip(1)) // Skip header
            {
                var parts = line.Split('\t');
                sushiItems.Add(new SushiItem
                {
                    Id = int.Parse(parts[0]),
                    Name = parts[1],
                    Style = parts[2],
                    MajorGroup = parts[3],
                    MinorGroup = parts[4],
                    Oiliness = double.Parse(parts[5]),
                    IsEaten = parts[6] == "1",
                    Frequency = double.Parse(parts[7])
                });
            }
            return sushiItems;
        }

        public static List<UserPreference> LoadUserPreferences(string filePath)
        {
            var userPreferences = new List<UserPreference>();
            foreach (var line in File.ReadLines(filePath))
            {
                var parts = line.Split(' ');
                userPreferences.Add(new UserPreference
                {
                    UserId = int.Parse(parts[0]),
                    Ratings = parts.Skip(1).Select(int.Parse).ToArray()
                });
            }
            return userPreferences;
        }
    }
}