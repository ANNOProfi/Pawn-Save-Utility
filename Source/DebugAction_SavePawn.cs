using LudeonTK;
using Verse;
using System.Xml.Serialization;
using System.IO;
using System;
using RimWorld;
using System.Linq;
using System.Collections.Generic;
using TMPro;

namespace PawnSaveUtility
{
    public static class DebugAction_SavePawn
    {
        [DebugAction("Pawn Save Utility", "SavePawn", actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
        private static void SavePawn(Pawn pawn)
        {
            PawnData pawnData = new(pawn);

            pawnData.SavePawn(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "PawnSaver"), pawn.Ideo);
        }

        [DebugAction("Pawn Save Utility", "LoadPawn", actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
        private static void SpawnLoadedPawn()
        {
            List<DebugMenuOption> list = new List<DebugMenuOption>();

            string folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "PawnSaver");
            string colonyPath = Path.Combine(folderPath, "New Arrivals");
            //string FileName = Path.Combine(colonyPath, "Afeeba 'Feeb' Dafo - 6th of Aprimay, 5500, 6h");

            foreach(string fileName in Directory.GetFiles(colonyPath))
            {
                if(fileName.Last() != 'l')
                {
                    continue;
                }
                ModLog.Log("File "+fileName);
                string label = "["+colonyPath.Replace(folderPath, "").Remove(0, 1)+"] "+fileName.Replace(colonyPath, "").Remove(0, 1);
                list.Add(new DebugMenuOption(label, DebugMenuOptionMode.Action, delegate
                {
                    LoadPawn(fileName.Remove(fileName.Length-4));
                    ModLog.Log("Selected pawn "+fileName);
                }));
                Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
            }
        }

        private static void LoadPawn(string FileName)
        {
            

            string FileNameWithExtensionPawn = FileName+".xml";
            string FileNameWithExtensionIdeo = FileName+".rid";

            ModLog.Log("Loading File");

            var stream = System.IO.File.OpenRead(FileNameWithExtensionPawn);
            var serializer = new XmlSerializer(typeof(PawnData));
            PawnData pawnData = serializer.Deserialize(stream) as PawnData;

            ModLog.Log("Loading kindDef");
            PawnKindDef kindDef = DefDatabase<PawnKindDef>.GetNamed(pawnData.kindDef) ?? PawnKindDefOf.Villager;

            ModLog.Log("Loading faction");
            FactionDef factionDef = DefDatabase<FactionDef>.GetNamed(pawnData.faction);

            Faction faction = Find.FactionManager.FirstFactionOfDef(factionDef) ?? Faction.OfPlayer;

            //var request = new PawnGenerationRequest(kindDef, faction);

            //request.AllowAddictions = false;

            ModLog.Log("Generating pawn");
            Pawn pawn = PawnGenerator.GeneratePawn(kindDef, faction);

            pawn.health.RemoveAllHediffs();

            pawn.inventory.DestroyAll();

            pawn.equipment.DestroyAllEquipment();

            pawn.apparel.DestroyAll();

            ModLog.Log("Loading name");
            pawn.Name = new NameTriple(pawnData.name.firstName ?? "", pawnData.name.nickName ?? "", pawnData.name.lastName ?? "");

            ModLog.Log("Loading gender");
            pawn.gender = pawnData.gender switch
            {
                "Male" => Gender.Male,
                "Female" => Gender.Female,
                _ => Gender.None,
            };

            ModLog.Log("Loading story");
            ModLog.Log("Loading bodyType");
            pawn.story.bodyType = DefDatabase<BodyTypeDef>.GetNamed(pawnData.story.bodyType) ?? BodyTypeDefOf.Thin;

            ModLog.Log("Loading headType");
            pawn.story.headType = DefDatabase<HeadTypeDef>.GetNamed(pawnData.story.headType);

            ModLog.Log("Loading hair");
            pawn.story.hairDef = DefDatabase<HairDef>.GetNamed(pawnData.story.hairDef) ?? HairDefOf.Bald;

            ModLog.Log("Loading childhood");
            pawn.story.Childhood = DefDatabase<BackstoryDef>.GetNamed(pawnData.story.childhood);

            ModLog.Log("Loading adulthood");
            pawn.story.Adulthood = DefDatabase<BackstoryDef>.GetNamed(pawnData.story.adulthood);

            ModLog.Log("Loading birthLastName");
            pawn.story.birthLastName = pawnData.story.birthLastName;

            ModLog.Log("Loading favoriteColor");
            if(!pawnData.story.favoriteColorDef.NullOrEmpty())
            {
                pawn.story.favoriteColor = DefDatabase<ColorDef>.GetNamed(pawnData.story.favoriteColorDef) ?? ColorDefOf.PlanGray;
            }

            ModLog.Log("Clearing traits");
            for(int i = pawn.story.traits.allTraits.Count - 1; i >= 0; i--)
            {
                pawn.story.traits.RemoveTrait(pawn.story.traits.allTraits[i]);
            }

            ModLog.Log("Loading traits");
            foreach(PawnTrait trait in pawnData.story.allTraits)
            {
                TraitDef traitDef = DefDatabase<TraitDef>.GetNamed(trait.def);

                if(traitDef != null)
                {
                    pawn.story.traits.GainTrait(new Trait(traitDef, trait.degree));
                }
            }

            ModLog.Log("Loading skills");
            foreach(PawnSkill skill in pawnData.skills.skills)
            {
                SkillDef skillDef = DefDatabase<SkillDef>.GetNamed(skill.def);

                if(skillDef != null)
                {
                    pawn.skills.GetSkill(skillDef).Level = skill.level;
                    pawn.skills.GetSkill(skillDef).passion = skill.passion switch
                    {
                        "Major" => Passion.Major,
                        "Minor" => Passion.Minor,
                        "None" => Passion.None,
                        _ => throw new NotImplementedException(),
                    };
                }
            }

            if(ModLister.BiotechInstalled)
            {
                pawn.genes.Endogenes.Clear();

                ModLog.Log("Loading xenotype");
                XenotypeDef xenotypeDef = DefDatabase<XenotypeDef>.GetNamed(pawnData.genes.xenotype) ?? XenotypeDefOf.Baseliner;

                pawn.genes.SetXenotype(xenotypeDef);

                ModLog.Log("Loading genes");
                foreach(PawnGene gene in pawnData.genes.endogenes)
                {
                    GeneDef geneDef = DefDatabase<GeneDef>.GetNamed(gene.def);

                    if(geneDef != null)
                    {
                        pawn.genes.AddGene(geneDef, false);
                    }
                }
            }

            ModLog.Log("Loading style");
            ModLog.Log("Loading beard");
            if(!pawnData.style.beardDef.NullOrEmpty())
            {
                pawn.style.beardDef = DefDatabase<BeardDef>.GetNamed(pawnData.style.beardDef) ?? BeardDefOf.NoBeard;
            }

            ModLog.Log("Loading faceTattoo");
            if(!pawnData.style.faceTattoo.NullOrEmpty())
            {
                pawn.style.FaceTattoo = DefDatabase<TattooDef>.GetNamed(pawnData.style.faceTattoo) ?? TattooDefOf.NoTattoo_Face;
            }

            ModLog.Log("Loading bodyTattoo");
            if(!pawnData.style.bodyTattoo.NullOrEmpty())
            {
                pawn.style.BodyTattoo = DefDatabase<TattooDef>.GetNamed(pawnData.style.bodyTattoo) ?? TattooDefOf.NoTattoo_Body;
            }

            ModLog.Log("Loading age");
            pawn.ageTracker.AgeBiologicalTicks = pawnData.age.biologicalTicks;

            ModLog.Log("Loading health");
            foreach(PawnHediff pawnHediff in pawnData.health.hediffs)
            {
                HediffDef hediffDef = DefDatabase<HediffDef>.GetNamed(pawnHediff.def);
                BodyPartDef bodyPartDef = null;
                if(pawnHediff.bodyPartDef != null)
                {
                    bodyPartDef = DefDatabase<BodyPartDef>.GetNamed(pawnHediff.bodyPartDef);
                }
                
                BodyPartRecord bodyPart = null;

                if(bodyPartDef != null)
                {
                    bodyPart = pawn.health.hediffSet.GetBodyPartRecord(bodyPartDef);
                }

                if(hediffDef != null && bodyPart != null)
                {
                    Hediff hediff = HediffMaker.MakeHediff(hediffDef, pawn, bodyPart);
                    hediff.ageTicks = pawnHediff.ageTicks;
                    if(pawnHediff.sourceDef != null)
                    {
                        ThingDef sourceDef = DefDatabase<ThingDef>.GetNamed(pawnHediff.sourceDef);
                        if(sourceDef != null)
                        {
                            hediff.sourceDef = sourceDef;
                        }
                    }
                    hediff.Severity = pawnHediff.severity;
                    HediffComp_GetsPermanent hediffComp = hediff.TryGetComp<HediffComp_GetsPermanent>();
                    if(hediffComp != null)
                    {
                        hediffComp.isPermanentInt = pawnHediff.isPermanent;
                    }

                    pawn.health.AddHediff(hediff, bodyPart);
                }
            }

            ModLog.Log("Loading inventory");
            foreach(PawnItem pawnItem in pawnData.inventory.items)
            {
                ModLog.Log("Loading inventory thing "+pawnItem.def);
                ThingDef thingDef = DefDatabase<ThingDef>.GetNamed(pawnItem.def);

                if(thingDef != null)
                {
                    ThingDef stuffDef = null;

                    if(pawnItem.stuffDef != null)
                    {
                        stuffDef = DefDatabase<ThingDef>.GetNamed(pawnItem.stuffDef) ?? GenStuff.DefaultStuffFor(thingDef);
                    }

                    Thing thing = ThingMaker.MakeThing(thingDef, stuffDef);

                    thing.stackCount = pawnItem.stackCount;
                    thing.HitPoints = pawnItem.hitPoints;

                    if(pawnItem.styleDef != null)
                    {
                        ThingStyleDef styleDef = DefDatabase<ThingStyleDef>.GetNamed(pawnItem.styleDef);

                        if(styleDef != null)
                        {
                            thing.StyleDef = styleDef;
                        }
                    }

                    pawn.inventory.TryAddAndUnforbid(thing);
                }
            }

            ModLog.Log("Loading equipment");
            foreach(PawnItem pawnItem in pawnData.equipment.items)
            {
                ThingDef thingDef = DefDatabase<ThingDef>.GetNamed(pawnItem.def);

                if(thingDef != null)
                {
                    ThingDef stuffDef = null;

                    if(pawnItem.stuffDef != null)
                    {
                        stuffDef = DefDatabase<ThingDef>.GetNamed(pawnItem.stuffDef) ?? GenStuff.DefaultStuffFor(thingDef);
                    }

                    ThingWithComps thing = (ThingWithComps)ThingMaker.MakeThing(thingDef, stuffDef);

                    thing.stackCount = pawnItem.stackCount;
                    thing.HitPoints = pawnItem.hitPoints;

                    if(pawnItem.styleDef != null)
                    {
                        ThingStyleDef styleDef = DefDatabase<ThingStyleDef>.GetNamed(pawnItem.styleDef);

                        if(styleDef != null)
                        {
                            thing.StyleDef = styleDef;
                        }
                    }

                    pawn.equipment.AddEquipment(thing);
                }
            }

            ModLog.Log("Loading apparel");
            foreach(PawnApparelItem pawnApparelItem in pawnData.apparel.items)
            {
                ThingDef thingDef = DefDatabase<ThingDef>.GetNamed(pawnApparelItem.def);

                if(thingDef != null)
                {
                    ThingDef stuffDef = null;

                    if(pawnApparelItem.stuffDef != null)
                    {
                        stuffDef = DefDatabase<ThingDef>.GetNamed(pawnApparelItem.stuffDef) ?? GenStuff.DefaultStuffFor(thingDef);
                    }

                    Apparel thing = (Apparel)ThingMaker.MakeThing(thingDef, stuffDef);

                    thing.HitPoints = pawnApparelItem.hitPoints;

                    if(pawnApparelItem.styleDef != null)
                    {
                        ThingStyleDef styleDef = DefDatabase<ThingStyleDef>.GetNamed(pawnApparelItem.styleDef);

                        if(styleDef != null)
                        {
                            thing.StyleDef = styleDef;
                        }
                    }

                    thing.WornByCorpse = pawnApparelItem.tainted;

                    pawn.apparel.Wear(thing);
                }
            }

            if(ModsConfig.RoyaltyActive)
            {
                ModLog.Log("loading title");
                foreach(PawnRoyalTitle title in pawnData.royalty.royalTitles)
                {
                    Faction titleFaction = Find.FactionManager.FirstFactionOfDef(DefDatabase<FactionDef>.GetNamed(title.factionDef));

                    if(titleFaction != null)
                    {
                        RoyalTitleDef titleDef = DefDatabase<RoyalTitleDef>.GetNamed(title.titleDef);

                        if(titleDef != null)
                        {
                            pawn.royalty.SetTitle(titleFaction, titleDef, false, false, false);
                        }
                    }
                }

                foreach(PawnPermit permit in pawnData.royalty.permits)
                {
                    Faction titleFaction = Find.FactionManager.FirstFactionOfDef(DefDatabase<FactionDef>.GetNamed(permit.factionDef));

                    if(titleFaction != null)
                    {
                        RoyalTitlePermitDef permitDef = DefDatabase<RoyalTitlePermitDef>.GetNamed(permit.permitDef);

                        if(permitDef != null)
                        {
                            pawn.royalty.AddPermit(permitDef, titleFaction);
                        }
                    }
                }

                foreach(PawnFavor favor in pawnData.royalty.favor)
                {
                    Faction titleFaction = Find.FactionManager.FirstFactionOfDef(DefDatabase<FactionDef>.GetNamed(favor.factionDef));

                    if(titleFaction != null)
                    {
                        pawn.royalty.SetFavor(titleFaction, favor.favor, false);
                    }
                }
            }

            if(ModsConfig.IdeologyActive)
            {
                ModLog.Log("Loading ideo");
                if(File.Exists(FileNameWithExtensionIdeo))
                {
                    ModLog.Log("Ideo found");
                    PreLoadUtility.CheckVersionAndLoad(FileNameWithExtensionIdeo, ScribeMetaHeaderUtility.ScribeHeaderMode.Ideo, delegate
                    {
                        if (GameDataSaveLoader.TryLoadIdeo(FileNameWithExtensionIdeo, out var ideo))
                        {
                            ModLog.Log("Applying Ideo");
                            if(!Find.IdeoManager.IdeosListForReading.Contains(ideo))
                            {
                                Find.IdeoManager.Add(ideo);
                            }
                            pawn.ideo.SetIdeo(IdeoGenerator.InitLoadedIdeo(ideo));
                        }
                        else
                        {
                            ModLog.Error("Could not load Ideo");
                        }
                    });
                }
                else
                {
                    ModLog.Log("Ideo not found");
                }
            }

            if(ModsConfig.AnomalyActive)
            {
                ModLog.Log("Loading creepjoiner");
                if(pawnData.creepjoiner.isCreepJoiner)
                {
                    CreepJoinerFormKindDef creepJoinerForm = DefDatabase<CreepJoinerFormKindDef>.GetNamed(pawnData.creepjoiner.formDef) ?? DefDatabase<CreepJoinerFormKindDef>.GetNamed("LeatheryStranger");
                    CreepJoinerDownsideDef creepJoinerDownside = DefDatabase<CreepJoinerDownsideDef>.GetNamed(pawnData.creepjoiner.downSideDef) ?? CreepJoinerDownsideDefOf.OrganDecay;
                    CreepJoinerBenefitDef creepJoinerBenefit = DefDatabase<CreepJoinerBenefitDef>.GetNamed(pawnData.creepjoiner.downSideDef) ?? DefDatabase<CreepJoinerBenefitDef>.GetNamed("Occultist");
                    CreepJoinerAggressiveDef creepJoinerAggressive = DefDatabase<CreepJoinerAggressiveDef>.GetNamed(pawnData.creepjoiner.aggressiveDef) ?? DefDatabase<CreepJoinerAggressiveDef>.GetNamed("Assault");
                    CreepJoinerRejectionDef creepJoinerRejection = DefDatabase<CreepJoinerRejectionDef>.GetNamed(pawnData.creepjoiner.rejectionDef) ?? DefDatabase<CreepJoinerRejectionDef>.GetNamed("Departure");

                    if(pawn.creepjoiner == null)
                    {
                        pawn.creepjoiner = new(pawn);
                    }

                    pawn.creepjoiner.form = creepJoinerForm;
                    pawn.creepjoiner.downside = creepJoinerDownside;
                    pawn.creepjoiner.benefit = creepJoinerBenefit;
                    pawn.creepjoiner.aggressive = creepJoinerAggressive;
                    pawn.creepjoiner.rejection = creepJoinerRejection;

                    pawn.creepjoiner.joinedTick = GenTicks.TicksGame-pawnData.creepjoiner.joinedTicksAgo;
                }
            }

            IntVec3 cell = UI.MouseCell();

            GenSpawn.Spawn(pawn, cell, Find.CurrentMap);
            pawn.Rotation = Rot4.South;
        }
    }
}