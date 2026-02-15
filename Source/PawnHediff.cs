using Verse;

namespace PawnSaveUtility
{
    public class PawnHediff
    {
        public string def;

        public int ageTicks;

        public float severity;

        public int level;

        public string sourceDef;

        public string bodyPartDef;

        public bool isPermanent;

        public PawnHediff()
        {

        }

        public PawnHediff(Hediff hediff)
        {
            def = hediff.def.defName;
            ageTicks = hediff.ageTicks;
            severity = hediff.Severity;
            sourceDef = hediff.sourceDef?.defName;
            bodyPartDef = hediff.Part?.def.defName;
            
            HediffComp_GetsPermanent permanentComp = hediff.TryGetComp<HediffComp_GetsPermanent>();
            if(permanentComp != null)
            {
                isPermanent = permanentComp.isPermanentInt;
            }

            if (hediff is Hediff_Level hediffLevel)
            {
                level = hediffLevel.level;
                severity = level;
            }
        }
    }
}