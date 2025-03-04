using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RelicCharts
{
        public class Relic
        {
            private string _name = "Lith A6";
            private string[] contents = new string[] { "Bronco Prime Blueprint", "Masseter Prime Handle", "Hildryn Prime Systems Blueprint", "Shade Prime Systems", "2X Forma Blueprint", "Akarius Prime Receiver" };
            public double[] DropRates { get; set; }
            public Refinements Refinement { get; set; }
            public int VoidTraceCost { get; set; }
            public string RelicName { get; set; }
            public string[] Contents { get; set; }
            public enum Refinements
            {
                Intact,
                Exceptional,
                Flawless,
                Radiant
            }

            public Relic(Refinements refinement)
            {
                RelicName = "Lith A6";
                Contents = contents;
                Refinement = refinement;
                SetDropRates(refinement);
            }

            private void SetDropRates(Refinements refinement)
            {
                switch (refinement)
                {
                    case Refinements.Intact:
                        DropRates = new double[] { 25.33, 25.33, 25.33, 11, 11, 2 };
                        VoidTraceCost = 0;
                        break;
                    case Refinements.Exceptional:
                        DropRates = new double[] { 23.33, 23.33, 23.33, 13, 13, 4 };
                        VoidTraceCost = 25;
                        break;
                    case Refinements.Flawless:
                        DropRates = new double[] { 20, 20, 20, 17, 17, 6 };
                        VoidTraceCost = 50;
                        break;
                    case Refinements.Radiant:
                        DropRates = new double[] { 16.67, 16.67, 16.67, 20, 20, 10 };
                        VoidTraceCost = 100;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(refinement));
                }
            }


        }
}
