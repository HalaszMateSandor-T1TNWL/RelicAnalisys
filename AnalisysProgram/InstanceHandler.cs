using System;
using TableGenerator;
using RelicSpace;
using System.Diagnostics;
using System.Threading;
using ItemGeneration;
using RelicAnalisys;
using InstanceGenerator;
using System.Runtime.InteropServices;

namespace InstanceHandling
{
    class InstanceHandler
    {
        public static void instanceHandler(List<Relic> relics)
        {
            if (!Directory.Exists("TempText"))
            {
                Directory.CreateDirectory("TempText");
            }

            Console.WriteLine("Please enter the command you'd like to execute.\n[Type 'help' for a list of commands]");
            string command;
            do
            {
                command = CheckedReadIn();
                switch (command)
                {
                    case "help":
                        Help();
                        break;
                    case "relics":
                        Table(relics);
                        break;
                    case "invalid":
                        Console.WriteLine("Command was not recognised, please try again!\n");
                        break;
                    case "chit":
                        Console.WriteLine("The chi squared table is read in different rows depending on the degrees of freedom you have.\nCurrent degree of freedom: 2\n");
                        Thread.Sleep(1500);
                        TableView.DrawChiSquareTable();
                        break;
                    case "simulate":
                        Simulate(relics);
                        break;
                    case "relicsd":
                        Table(relics);
                        TableView.DrawTraceRequirements();
                        TableView.DrawRelicDetails(Analitics.atLeastOneRare(relics),relics);
                        break;
                    case "charts":
                        Open();
                        break;
                    case "quit":
                        Console.WriteLine("\nThank you for using the Simulacrum, Tenno!\n");
                        Thread.Sleep(1500);
                        break;
                }
            } while (command != "quit");
        }
        #region Function Calls
        public static void Help()
        {
            Console.WriteLine("The following commands are available:\n");
            Console.WriteLine("help - displays a list of commands to get you started");
            Console.WriteLine("relicsd - other than displaying the relics, it also displays multiple tables with general statistics");
            Console.WriteLine("charts - opens a graphical application with multiple charts");
            Console.WriteLine("relics - displays the relics available for simulation");
            Console.WriteLine("simulate - runs the simulation sequence");
            Console.WriteLine("chiT - displays the chi square table");
            Console.WriteLine("quit - exits the program");
        }

        public static string CheckedReadIn()
        {
            string[] acceptable = { "relics", "simulate", "chit", 
                                    "quit", "help", "all", "intact", "exceptional", 
                                    "flawless", "radiant", "relicsd", "charts" };
            string command = Console.ReadLine().ToLower();
            while (string.IsNullOrEmpty(command))
            {
                Console.WriteLine("Please enter a command or type 'help' to get a list of commands to get started!\n");
                command = Console.ReadLine().ToLower();
            }
            if (acceptable.Contains(command))
            {
                return command;
            }
            else
                Console.WriteLine("Something went wrong, restarting...\n");
                return command = "invalid";
        }

        public static void Open()
        {
            string directory = AppDomain.CurrentDomain.BaseDirectory;
            string target = "RelicCharts.exe";

            string filepath = Path.Combine(directory, target);
            try
            {
                if (File.Exists(filepath))
                {
                    Process.Start(new ProcessStartInfo(filepath) { UseShellExecute = true });
                }
                else
                {
                    Console.WriteLine($"File '{target}' not found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error opening file or URL: " + ex.Message);
            }
        }

        public static void Table(List<Relic> relics)
        {
            for (int i = 0; i < 4; i++)
            {
                Console.WriteLine($"Details for [{relics[i].Refinement}]{relics[i].RelicName}:");
                TableView.DrawRelicContents(relics[i]);
                Console.WriteLine("Press Enter to continue\n");
                Console.ReadLine();
            }
        }

        public static int CheckedReadInt()
        {
            int iterationCounter = 0;
            string intInput;
            int ok = 0;
            do 
            {
                intInput = Console.ReadLine();
                while (string.IsNullOrEmpty(intInput))
                {
                    Console.WriteLine("Please enter a number for the simulation to proceed.\n");
                    intInput = Console.ReadLine();
                }
                while (int.TryParse(intInput, out iterationCounter) != true)
                {
                    Console.WriteLine("Please only enter numbers\n");
                    intInput = Console.ReadLine();
                }
                switch (iterationCounter)
                {
                    case <= 0:
                        Console.WriteLine("Iteration counter can not be lower than 0\n");
                        ok = 1;
                        break;
                    case > 100:
                        Console.WriteLine("Each iteration of the simulation will generate 1000 items and analyse them, this might take a while\n");
                        ok = 0;
                        return iterationCounter;
                        break;
                    default:
                        ok = 0;
                        return iterationCounter;
                }
            } while (ok == 1);
            return iterationCounter;
        }

        public static void Simulate(List<Relic> relics)
        {
            string errorpath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"TempText", "ErrorsForRareItemDrops.txt");
            int simulationCount;
            string command;
            Console.WriteLine("Generating Relics...\n");
            Thread.Sleep(500);
            Console.WriteLine("How many times would like to simulate the relic drops?\n");
            simulationCount = CheckedReadInt();
            Console.WriteLine("The avaliable relics are:\n");
            Table(relics);
            Console.WriteLine("Type the refinement of the relic (Highlited in brackets: []) for individual analysis or type \"all\" for a full run of the program.\n");
            command = CheckedReadIn();
            for (int i = 0; i < simulationCount; i++)
            {
                switch (command)
                {
                    case "intact":
                        InstanceGeneration.ItemGeneration(relics[0]);
                        Analitics.itemsDroppedAnalisys(relics[0]);
                        Analitics.deviationFromExpectedValue(relics[0], errorpath);
                        Analitics.chiSquareTest(relics[0]);
                        break;
                    case "exceptional":
                        InstanceGeneration.ItemGeneration(relics[1]);
                        Analitics.itemsDroppedAnalisys(relics[1]);
                        Analitics.deviationFromExpectedValue(relics[1], errorpath);
                        Analitics.chiSquareTest(relics[1]);
                        break;
                    case "flawless":
                        InstanceGeneration.ItemGeneration(relics[2]);
                        Analitics.itemsDroppedAnalisys(relics[2]);
                        Analitics.deviationFromExpectedValue(relics[2], errorpath);
                        Analitics.chiSquareTest(relics[2]);
                        break;
                    case "radiant":
                        InstanceGeneration.ItemGeneration(relics[3]);
                        Analitics.itemsDroppedAnalisys(relics[3]);
                        Analitics.deviationFromExpectedValue(relics[3], errorpath);
                        Analitics.chiSquareTest(relics[3]);
                        break;
                    case "all":
                        for (int j = 0; j < relics.Count; j++)
                        {
                            InstanceGeneration.ItemGeneration(relics[j]);
                            Analitics.itemsDroppedAnalisys(relics[j]);
                            Analitics.deviationFromExpectedValue(relics[j], errorpath);
                            Analitics.chiSquareTest(relics[j]);
                        }
                        break;
                }
            }
        }
        #endregion Fucntion Calls
    }
}
