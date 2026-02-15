using RimWorld;

namespace PawnSaveUtility
{
    public class PawnApparelItem
    {
        public string def;

        public string stuffDef;

        public int hitPoints;

        public string styleDef;

        public bool tainted;

        public PawnApparelItem()
        {

        }

        public PawnApparelItem(Apparel item)
        {
            def = item.def.defName;
            hitPoints = item.HitPoints;
            styleDef = item.StyleDef?.defName;
            tainted = item.WornByCorpse;
            stuffDef = item.Stuff?.defName;
        }
    }
}