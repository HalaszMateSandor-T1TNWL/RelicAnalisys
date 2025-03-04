using System;
using RelicSpace;

namespace ItemGeneration
{
    class Generator
    {
        public static List<Relic> relicGenerator()
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
