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
            foreach (var line in File.ReadLines(filePath))
            {
                var parts = line.Split('\t');
                sushiItems.Add(new SushiItem
                {
                    Id = int.Parse(parts[0]),
                    Name = parts[1],
                    Style = int.Parse(parts[2]) == 0 ? "maki" : "otherwise",
                    MajorGroup = int.Parse(parts[3]) == 0 ? "seafood" : "otherwise",
                    MinorGroup = GetMinorGroup(int.Parse(parts[4])),
                    Heaviness = double.Parse(parts[5]),
                    EatingFrequency = double.Parse(parts[6]),
                    NormalizedPrice = double.Parse(parts[7]),
                    SoldFrequency = double.Parse(parts[8])
                });
            }
            return sushiItems;
        }

        private static string GetMinorGroup(int groupNumber)
        {
            return groupNumber switch
            {
                0 => "aomono (blue-skinned fish)",
                1 => "akami (red meat fish)",
                2 => "shiromi (white-meat fish)",
                3 => "tare (baste; for eel or sea eel)",
                4 => "clam or shell",
                5 => "squid or octopus",
                6 => "shrimp or crab",
                7 => "roe",
                8 => "other seafood",
                9 => "egg",
                10 => "meat other than fish",
                11 => "vegetables",
                _ => "unknown"
            };
        }

        public static List<UserPreference> LoadUserPreferences(string filePath)
        {
            var userPreferences = new List<UserPreference>();
            int userId = 0;
            foreach (var line in File.ReadLines(filePath))
            {
                var ratings = line.Split(' ').Select(s => int.Parse(s)).ToArray();
                userPreferences.Add(new UserPreference
                {
                    UserId = userId,
                    Ratings = ratings
                });
                userId++;
            }
            return userPreferences;
        }
    }
}