using RimWorld;

namespace PawnSaveUtility
{
    public class PawnPermit
    {
        public string permitDef;

        public string factionDef;

        public PawnPermit()
        {

        }

        public PawnPermit(FactionPermit permit)
        {
            permitDef = permit.Permit.defName;

            factionDef = permit.Faction.def.defName;
        }
    }
}