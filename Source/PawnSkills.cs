using RimWorld;
using System.Collections.Generic;

namespace PawnSaveUtility
{
    public class PawnSkills
    {
        public List<PawnSkill> skills = new List<PawnSkill>();

        public PawnSkills(Pawn_SkillTracker skillTracker)
        {
            foreach(SkillRecord skill in skillTracker.skills)
            {
                skills.Add(new PawnSkill(skill));
            }
        }

        public PawnSkills()
        {
            
        }
    }
}