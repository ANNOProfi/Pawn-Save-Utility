using RimWorld;
using UnityEngine;
using Verse;
using System.IO;
using System;

namespace PawnSaveUtility
{

    public class PSU_Settings : ModSettings
    {
        //Use Mod.settings.setting to refer to this setting.

        public string savePath;

        public void DoWindowContents(Rect wrect)
        {
            var options = new Listing_Standard();
            options.Begin(wrect);

            options.Label("FolderPath".Translate());
            savePath = options.TextEntry(savePath ?? "");

            options.End();
        }
        
        public override void ExposeData()
        {
            Scribe_Values.Look(ref savePath, "savePath", "");
        }
    }
}
