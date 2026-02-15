using System.Collections.Generic;
using RimWorld;
using UnityEngine.Pool;
using Verse;

namespace PawnSaveUtility
{
    public class PawnGenes
    {
        public string xenotype;

        public List<PawnGene> endogenes = new List<PawnGene>();

        public PawnGenes(Pawn_GeneTracker geneTracker)
        {
            xenotype = geneTracker.Xenotype.defName;

            foreach(Gene gene in geneTracker.GenesListForReading)
            {
                endogenes.Add(new PawnGene(gene));
            }
        }

        public PawnGenes()
        {
            
        }      
    }
}