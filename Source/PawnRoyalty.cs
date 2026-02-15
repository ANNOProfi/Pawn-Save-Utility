using System.Collections.Generic;
using RimWorld;
using Verse;

namespace PawnSaveUtility
{
    public class PawnRoyalty
    {
        public List<PawnRoyalTitle> royalTitles = new List<PawnRoyalTitle>();

        public List<PawnPermit> permits = new List<PawnPermit>();

        public List<PawnFavor> favor = new List<PawnFavor>();

        public PawnRoyalty()
        {

        }

        public PawnRoyalty(Pawn_RoyaltyTracker royaltyTracker)
        {
            foreach(RoyalTitle title in royaltyTracker.AllTitlesForReading)
            {
                royalTitles.Add(new PawnRoyalTitle(title));
            }

            foreach(FactionPermit permit in royaltyTracker.AllFactionPermits)
            {
                permits.Add(new PawnPermit(permit));
            }

            foreach(Faction faction in Find.FactionManager.AllFactions)
            {
                if(faction.def.HasRoyalTitles)
                {
                    favor.Add(new PawnFavor(faction, royaltyTracker.GetFavor(faction)));
                }
            }
        }
    }
}