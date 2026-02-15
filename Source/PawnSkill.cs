using RimWorld;

namespace PawnSaveUtility
{
    public class PawnSkill
    {
        public string def;

        public int level;

        public string passion;

        public PawnSkill(SkillRecord skill)
        {
            def = skill.def.defName;

            level = skill.Level;

            passion = skill.passion.ToString();
        }

        public PawnSkill()
        {

        }
    }
}