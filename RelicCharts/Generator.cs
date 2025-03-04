using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RelicCharts
{
    class Generator
    {
        public static List<Relic> RelicGenerator()
        {
            List<Relic> relics = new List<Relic>();
            relics.Add(new Relic(Relic.Refinements.Intact));
            relics.Add(new Relic(Relic.Refinements.Exceptional));
            relics.Add(new Relic(Relic.Refinements.Flawless));
            relics.Add(new Relic(Relic.Refinements.Radiant));
            return relics;
        }
    }
}
