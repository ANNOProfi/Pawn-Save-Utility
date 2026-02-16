using UnityEngine;
using Verse;

namespace PawnSaveUtility
{
    public class PawnSaveUtilityMod : Mod
    {
        public static PSU_Settings settings;

        private string intervalBuffer = 60.ToString();

        private string conversionBuffer = 0.5f.ToString();

        public PawnSaveUtilityMod(ModContentPack content) : base(content)
        {
            settings = GetSettings<PSU_Settings>();
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            base.DoSettingsWindowContents(inRect);
            settings.DoWindowContents(inRect);
        }

        public override string SettingsCategory()
        {
            return "PawnSaveUtility";
        }
    }
}