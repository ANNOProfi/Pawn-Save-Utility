using System.Collections.Generic;
using RimWorld;

namespace PawnSaveUtility
{
    public class PawnAbilities
    {
        public List<string> abilityDefs = new List<string>();

        public PawnAbilities()
        {

        }

        public PawnAbilities(Pawn_AbilityTracker abilityTracker)
        {
            foreach(Ability ability in abilityTracker.AllAbilitiesForReading)
            {
                abilityDefs.Add(ability.def.defName);
            }
        }
    }
}