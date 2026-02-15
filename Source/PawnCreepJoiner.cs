using Verse;
using RimWorld;

namespace PawnSaveUtility
{
    public class PawnCreepJoiner
    {
        public bool isCreepJoiner;

        public int joinedTicksAgo;

        public string formDef;

        public string benefitDef;

        public string downSideDef;

        public string aggressiveDef;

        public string rejectionDef;

        public PawnCreepJoiner()
        {

        }

        public PawnCreepJoiner(Pawn_CreepJoinerTracker creepjoiner)
        {
            isCreepJoiner = creepjoiner.Pawn.IsCreepJoiner;
            joinedTicksAgo = GenTicks.TicksGame-creepjoiner.joinedTick;
            formDef = creepjoiner.form.defName;
            benefitDef = creepjoiner.benefit.defName;
            downSideDef = creepjoiner.downside.defName;
            aggressiveDef = creepjoiner.aggressive.defName;
            rejectionDef = creepjoiner.rejection.defName;
        }
    }
}