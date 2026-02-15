using RimWorld;

namespace PawnSaveUtility
{
    public class PawnFavor
    {
        public int favor;

        public string factionDef;

        public PawnFavor()
        {

        }

        public PawnFavor(Faction faction, int favor)
        {
            factionDef = faction.def.defName;

            this.favor = favor;
        }
    }
}