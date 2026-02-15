using System.Collections.Generic;
using Verse;

namespace PawnSaveUtility
{
    public class PawnHealth
    {
        public List<PawnHediff> hediffs = new List<PawnHediff>();

        public PawnHealth()
        {

        }

        public PawnHealth(Pawn_HealthTracker healthTracker)
        {
            foreach(Hediff hediff in healthTracker.hediffSet.hediffs)
            {
                ModLog.Log("Adding hediff "+hediff.def.defName);
                hediffs.Add(new PawnHediff(hediff));
            }
        }
    }
}