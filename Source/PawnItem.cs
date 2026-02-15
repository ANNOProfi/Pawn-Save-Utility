using Verse;

namespace PawnSaveUtility
{
    public class PawnItem
    {
        public string def;

        public string stuffDef;

        public int stackCount;

        public int hitPoints;

        public string styleDef;

        public PawnItem()
        {

        }

        public PawnItem(Thing item)
        {
            def = item.def.defName;
            stackCount = item.stackCount;
            hitPoints = item.HitPoints;
            styleDef = item.StyleDef?.defName;
            stuffDef = item.Stuff?.defName;
        }
    }
}