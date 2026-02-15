using Verse;

namespace PawnSaveUtility
{
    public class PawnAge
    {
        public long biologicalTicks;

        public PawnAge()
        {

        }

        public PawnAge(Pawn_AgeTracker ageTracker)
        {
            biologicalTicks = ageTracker.AgeBiologicalTicks; 
        }
    }
}