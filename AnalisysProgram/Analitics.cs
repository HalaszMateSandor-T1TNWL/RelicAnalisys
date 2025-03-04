using System;
using System.IO;
using MathNet;
using MathNet.Numerics;
using MathNet.Numerics.Distributions;
using MathNet.Numerics.Statistics;
using RelicSpace;
using ScottPlot;

namespace RelicAnalisys
{
    class Analitics
    {
        public static void itemsDroppedAnalisys(Relic relic)
        {
            int counter = 0;

            string textTarget = @"TempText";
            string directory = AppDomain.CurrentDomain.BaseDirectory;
            string textFilename = $"{relic.RelicName}_{relic.Refinement}_Drops.txt";

            string fullDropPath = Path.Combine(directory, textTarget, textFilename);

            using (StreamReader readFile = new StreamReader(fullDropPath))
            {
                string? ln;
                while ((ln = readFile.ReadLine()) != null)
                {
                    counter++;
                }
            }
            ;

            string[] drops = File.ReadAllLines(fullDropPath);
            var query = drops.GroupBy(x => x).Select(x => new { Item = x.Key, Count = x.Count() }).OrderBy(x => x.Count);

            Console.WriteLine($"\nFor {relic.RelicName} of {relic.Refinement} level these are the results:\n");
            foreach (var item in query)
            {
                Console.WriteLine($"{item.Item} : {item.Count}");
            }
        }

        public static void deviationFromExpectedValue(Relic relic, string errorname)
        {
            string textTarget = @"TempText";
            string directory = AppDomain.CurrentDomain.BaseDirectory;
            string textFilename = $"{relic.RelicName}_{relic.Refinement}_Drops.txt";
            string filename = Path.Combine(directory, textTarget, textFilename);

            string[] drops = File.ReadAllLines(filename);

            var query = drops.GroupBy(x => x).Select(x => new { Item = x.Key, Count = x.Count() }).OrderByDescending(x => x.Count);
            var observedRare = query.Where(x => x.Item == "Akarius Prime Receiver").Select(x => x.Count).First();

            double expectedValue = 1000.00 * (relic.DropRates[5] / 100);

            double deviation = observedRare - expectedValue;

            File.AppendAllText(errorname, deviation + Environment.NewLine);
            Console.WriteLine($"\nThe expected value is: Total number of iterations * (droprate/100), in this case: {expectedValue}\nThe Deviation from the expected value is: {deviation}");
        }

        public static double chiSquareTest(Relic relic)
        {
            string textTarget = @"TempText";
            string directory = AppDomain.CurrentDomain.BaseDirectory;
            string textFilename = $"{relic.RelicName}_{relic.Refinement}_Drops.txt";

            string filename = Path.Combine(directory, textTarget, textFilename);

            string[] drops = File.ReadAllLines(filename);
            var query = drops.GroupBy(x => x).Select(x => new { Item = x.Key, Count = x.Count() }).OrderByDescending(x => x.Count);
            var observedCommon = query.Where(x => x.Item == "Bronco Prime Blueprint" || x.Item == "Masseter Prime Handle" || x.Item == "Hildryn Prime Systems Blueprint").Select(x => x.Count).FirstOrDefault();
            var observedUncommon = query.Where(x => x.Item == "Shade Prime Systems" || x.Item == "2X Forma Blueprint").Select (x => x.Count).FirstOrDefault();
            var observedRare = query.Where(x => x.Item == "Akarius Prime Receiver").Select(x => x.Count).FirstOrDefault();

            double expectedCommon = 1000.00 * (relic.DropRates[0] / 100);
            double expectedUncommon = 1000.00 * (relic.DropRates[3] / 100);
            double expectedRare = 1000.00 * (relic.DropRates[5] / 100);

            double chiSquare = (Math.Pow(observedRare - expectedRare, 2) / expectedRare) + (Math.Pow(observedCommon - expectedCommon, 2) / expectedCommon) + (Math.Pow(observedUncommon - expectedUncommon, 2) / expectedUncommon);
            Console.WriteLine("The chi square value is: " + chiSquare);
            return chiSquare;
        }

        public static void exactlyXnumberRare(Relic relic)
        {
            int numberOfPlayers = 0;
            int numberOfRareItems = 0;
            double probability = 0;
            Console.WriteLine($"For {relic.RelicName} of {relic.Refinement} level:\n");
            Console.WriteLine("How many players will be running the mission?\n");
            int.TryParse(Console.ReadLine(), out numberOfPlayers);
            switch (numberOfPlayers)
            {
                case <= 0:
                    numberOfPlayers = 1;
                    numberOfRareItems = 1;
                    break;
                case 1:
                    numberOfRareItems = 1;
                    break;
                case >1:
                    Console.WriteLine("How many rare items are you expecting?\n");
                    int.TryParse(Console.ReadLine(), out numberOfRareItems);
                    break;
            }
            switch (numberOfRareItems)
            {
                case < 0:
                    probability = 0;
                    break;
                case > 4:
                    numberOfRareItems = numberOfPlayers;
                    probability = SpecialFunctions.Binomial(numberOfPlayers, numberOfRareItems) * Math.Pow(relic.DropRates[5] / 100, numberOfRareItems) * Math.Pow(1 - relic.DropRates[5] / 100, numberOfPlayers - numberOfRareItems);
                    break;
                default:
                    probability = SpecialFunctions.Binomial(numberOfPlayers, numberOfRareItems) * Math.Pow(relic.DropRates[5] / 100, numberOfRareItems) * Math.Pow(1 - relic.DropRates[5] / 100, numberOfPlayers - numberOfRareItems);
                    break;
            }
            Console.WriteLine($"The probability of {numberOfRareItems} items being dropped based on {numberOfPlayers} players is:");
            Console.WriteLine(probability*100 + "%\n");
        }

        public static List<double> atLeastOneRare(List<Relic> relics)
        {
            double summation = 0;
            List<double> chances = new List<double>();
           for (int i = 0; i < relics.Count; i++)
           {
                for (int j = 1; j <= 4; j++)
                    {
                        summation += SpecialFunctions.Binomial(4, j) * Math.Pow(relics[i].DropRates[5] / 100, j) * Math.Pow(1 - relics[i].DropRates[5] / 100, 4 - j);
                    }
                chances.Add(summation);
                summation = 0;
           }
            return chances;
        }   
    }
}
