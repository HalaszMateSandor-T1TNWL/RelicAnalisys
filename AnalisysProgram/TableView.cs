using System;
using ConsoleTables;
using RelicSpace;
using RelicAnalisys;

namespace TableGenerator
{
    class TableView
    {
        public static void DrawChiSquareTable()
        {
            var table = new ConsoleTable("Degrees of freedom (df)", null, null, null, null, null, "Chi^2 Value", null, null, null, null, null);
            table.AddRow(1,0.004,0.02,0.06,0.15,0.46,1.07,1.64,2.71,3.84,6.63,10.83)
                 .AddRow(2, 0.10, 0.21, 0.45, 0.71, 1.39, 2.41, 3.22, 4.61, 5.99, 9.21, 13.82)
                 .AddRow(3, 0.35, 0.58, 1.01, 1.42, 2.37, 3.66, 4.64, 6.25, 7.81, 11.34, 16.27)
                 .AddRow(4, 0.71, 1.06, 1.65, 2.20, 3.36, 4.88, 5.99, 7.78, 9.49, 13.28, 18.47)
                 .AddRow(5, 1.14, 1.61, 2.34, 3.00, 4.35, 6.06, 7.29, 9.24, 11.07, 15.09, 20.52)
                 .AddRow(6, 1.63, 2.20, 3.07, 3.83, 5.35, 7.23, 8.56, 10.64, 12.59, 16.81, 22.46)
                 .AddRow(7, 2.17, 2.83, 3.82, 4.67, 6.35, 8.38, 9.80, 12.01, 14.07, 18.48, 24.32)
                 .AddRow(8, 2.73, 3.49, 4.59, 5.53, 7.34, 9.52, 11.03, 13.36, 15.51, 20.09, 26.12)
                 .AddRow(9, 3.32, 4.17, 5.38, 6.39, 8.34, 10.66, 12.24, 14.68, 16.92, 21.67, 27.88)
                 .AddRow(10, 3.94, 4.87, 6.18, 7.27, 9.34, 11.78, 13.44, 15.99, 18.31, 23.21, 29.59)
                 .AddRow("p-value (probability)", 0.95, 0.90,0.80, 0.70,0.50,0.30,0.20,0.10,0.05,0.01,0.001);
            table.Options.EnableCount = false;
            table.Write();
        }

        public static void DrawTraceRequirements()
        {
            var table = new ConsoleTable(" ","Intact", "Exceptional", "Flawless", "Radiant");
            table.AddRow("Void Trace Cost", 0, 25, 50, 100)
                 .AddRow("Rare Item Chance","2%","4%","6%","10%");
            table.Options.EnableCount = false;
            table.Write();
        }

        public static void DrawRelicContents(Relic relic)
        {
            var table = new ConsoleTable("Item", "Drop Rate");
            for (int i = 0; i < relic.Contents.Length; i++)
            {
                table.AddRow(relic.Contents[i], relic.DropRates[i]);
            }
            table.Options.EnableCount = false;
            table.Write();
        }

        public static void DrawRelicDetails(List<double> chances, List<Relic> relic)
        {
            var table = new ConsoleTable("Relic refinement","Chance for at least one rare item","Expected relics per player", "Expected Traces per player");
            for (int i = 0; i < chances.Count; i++)
            {
                table.AddRow(relic[i].Refinement, Math.Round(chances[i]*100,2), Math.Round((1 / chances[i]),2) + " ~ " + Math.Round((1 / chances[i])), Math.Round((1 / chances[i]) * relic[i].VoidTraceCost));
            }
            table.Options.EnableCount = false;
            table.Write();
        }

    }
}
