using RimWorld;
using Verse;
using System.Collections.Generic;

namespace PawnSaveUtility
{
    public class PawnStory
    {
        public string bodyType;

        public string headType;

        public string hairDef;

        public string childhood;

        public string adulthood;

        public string birthLastName;

        public string favoriteColorDef;

        public List<PawnTrait> allTraits = new List<PawnTrait>();

        public PawnStory(Pawn_StoryTracker storyTracker)
        {
            ModLog.Log("Adding bodyType");
            bodyType = storyTracker.bodyType.defName;

            ModLog.Log("Adding headType");
            headType = storyTracker.headType.defName;

            ModLog.Log("Adding hair");
            hairDef = storyTracker.hairDef.defName;

            ModLog.Log("Adding childhood");
            childhood = storyTracker.Childhood?.defName;

            ModLog.Log("Adding adulthood");
            adulthood = storyTracker.Adulthood?.defName;

            ModLog.Log("Adding birthLastName");
            birthLastName = storyTracker.birthLastName;

            ModLog.Log("Adding favoriteColor");
            favoriteColorDef = storyTracker.favoriteColor?.defName;

            ModLog.Log("Adding traits");
            foreach(Trait trait in storyTracker.traits.allTraits)
            {
                allTraits.Add(new PawnTrait(trait.def.defName, trait.Degree));
            }
        }

        public PawnStory()
        {
            
        }
    }
}