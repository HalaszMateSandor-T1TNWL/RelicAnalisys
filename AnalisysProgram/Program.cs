using System;
using ItemGeneration;
using RelicSpace;
using RelicAnalisys;
using InstanceGenerator;
using TableGenerator;
using InstanceHandling;

namespace RelicSimulator
{
    class Program
    {
        public static void Main(string[] args)
        {
            List<Relic> relics = Generator.relicGenerator();
            InstanceHandling.InstanceHandler.instanceHandler(relics);
            //Analitics.itemsDroppedAnalisys(relics[0]); Ignore these, these were for testing purposes
            //Plotter.MostChosenValues(relics[0]); I'm pretty sure this doesn't even do anything anymore...
            
        }
    }
}
