using RimWorld;
using Verse;
using System.Collections.Generic;
using UnityEngine.Pool;
using System.Xml.Serialization;
using System.IO;
using System;

namespace PawnSaveUtility
{
    public class PawnData
    {
        public string def;

        public string kindDef;

        public string faction;

        public PawnName name;

        public string gender;

        public PawnStory story;

        public PawnSkills skills;

        public PawnGenes genes;

        public PawnStyle style;

        public PawnAge age;

        public PawnHealth health;

        public PawnInventory inventory;

        public PawnEquipment equipment;

        public PawnApparel apparel;

        public PawnRoyalty royalty;

        public PawnCreepJoiner creepjoiner;

        public string dateGenerated;

        public PawnData()
        {

        }

        public PawnData(Pawn pawn)
        {
            ModLog.Log("Adding defName");
            def = pawn.def.defName;

            ModLog.Log("Adding pawnKind");
            kindDef = pawn.kindDef.defName;

            ModLog.Log("Adding faction");
            faction = pawn.Faction.def.defName;

            ModLog.Log("Adding name");
            name = new(pawn.Name);

            ModLog.Log("Adding gender");
            gender = pawn.gender.ToString();

            ModLog.Log("Adding story");
            story= new(pawn.story);

            ModLog.Log("Adding skills");
            skills = new(pawn.skills);

            ModLog.Log("Adding genes");
            genes = new(pawn.genes);

            ModLog.Log("Adding style");
            style = new(pawn.style);

            ModLog.Log("Adding age");
            age = new(pawn.ageTracker);

            ModLog.Log("Adding health");
            health = new(pawn.health);

            ModLog.Log("Adding inventory");
            inventory = new(pawn.inventory);

            ModLog.Log("Adding equipment");
            equipment = new(pawn.equipment);

            ModLog.Log("Adding apparel");
            apparel = new(pawn.apparel);

            ModLog.Log("Adding title");
            royalty = new(pawn.royalty);

            if(pawn.IsCreepJoiner)
            {
                ModLog.Log("Adding creepjoiner");
                creepjoiner = new(pawn.creepjoiner);
            }

            ModLog.Log("Adding timestamp");
            dateGenerated = GenDate.DateFullStringWithHourAt(Find.TickManager.TicksAbs, Find.WorldGrid.LongLatOf(pawn.Map.Tile));
        }

        public void SavePawn(string path, Ideo saveIdeo)
        {
            string folderPath = Path.Combine(path, Faction.OfPlayer.def.LabelCap);

            if(!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            string pawnName = name.firstName;

            if(!name.nickName.NullOrEmpty())
            {
                pawnName += " '"+name.nickName+"'";
            }

            pawnName += " "+name.lastName;

            string FileName = Path.Combine(folderPath, pawnName+" - "+dateGenerated);
            
            string FileNameWithExtension = FileName+".xml";

            var writer = new StreamWriter(FileNameWithExtension);
            var serializer = new XmlSerializer(GetType());
            serializer.Serialize(writer, this);
            writer.Flush();

            if(saveIdeo != null)
            {
                LongEventHandler.QueueLongEvent(delegate
                {
                    GameDataSaveLoader.SaveIdeo(saveIdeo, FileName+".rid");
                }, "SavingLongEvent", doAsynchronously: false, null);
            }
        }
    }
}