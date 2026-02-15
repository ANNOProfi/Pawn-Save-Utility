using Verse;
using System.Collections.Generic;
using RimWorld;

namespace PawnSaveUtility
{
    public class PawnApparel
    {
        public List<PawnApparelItem> items = new List<PawnApparelItem>();

        public PawnApparel()
        {

        }

        public PawnApparel(Pawn_ApparelTracker apparelTracker)
        {
            foreach(Apparel thing in apparelTracker.WornApparel)
            {
                items.Add(new PawnApparelItem(thing));
            }
        }
    }
}