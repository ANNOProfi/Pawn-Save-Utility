using RimWorld;

namespace PawnSaveUtility
{
    public class PawnRoyalTitle
    {
        public string titleDef;

        public string factionDef;

        public PawnRoyalTitle()
        {

        }

        public PawnRoyalTitle(RoyalTitle title)
        {
            titleDef = title.def.defName;

            factionDef = title.faction.def.defName;
        }
    }
}