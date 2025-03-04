using System;
using RelicSpace;
using MathNet.Numerics.Random;
using MathNet.Numerics.Distributions;
using System.Security.Cryptography;

namespace InstanceGenerator
{
    class InstanceGeneration
    {
        public static void ItemGeneration(Relic relic)
        {
            System.Random rng = SystemRandomSource.Default;
            //Random rand = new Random();
            
            int itteration = 1000;
            double addedWeights = 0;

            string target = @"TempText";
            string directory = AppDomain.CurrentDomain.BaseDirectory;

            string filename = $"{relic.RelicName}_{relic.Refinement}_Drops.txt";
            string chancesFile = $"{relic.RelicName}_{relic.Refinement}_RolledValues.txt";
            string generatedValue = $"GeneratedValues.txt";

            string fullDropPath = Path.Combine(directory, target, filename);
            string fullValuePath = Path.Combine(directory, target, chancesFile);
            string fullGeneratedPath = Path.Combine(directory, target ,generatedValue);

            using (FileStream fs = File.Create(fullDropPath)) { }
            ;
            using (FileStream fs = File.Create(fullValuePath)) { }
            ;
            using (FileStream fs = File.Create(fullGeneratedPath)) { }
            ;

                List<string> relicDrops = new List<string>();

            for (int i = 0; i < 6; i++)
            {
                addedWeights += relic.DropRates[i];
            }

            for (int i = 0; i < itteration; i++)
            {
                double randomValue = rng.NextDouble();
                double cumulativeWeight = 0;

                for (int j = relic.DropRates.Length - 1; j >= 0; j--)
                {
                    cumulativeWeight += relic.DropRates[j]/100;

                    if (randomValue <= cumulativeWeight)
                    {
                        relicDrops.Add(relic.Contents[j]);

                        File.AppendAllText(fullGeneratedPath, 
                        Math.Round(randomValue, 2) + Environment.NewLine);

                        File.AppendAllText(fullValuePath, 
                        relic.DropRates[j] + Environment.NewLine);

                        File.AppendAllText(fullDropPath, 
                        relic.Contents[j] + Environment.NewLine);
                        break;
                    }
                }
            }
        }
    }
}
