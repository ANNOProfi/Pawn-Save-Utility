using Verse;

namespace PawnSaveUtility
{
    public class PawnGene
    {
        public string def;

        public PawnGene()
        {

        }

        public PawnGene(Gene gene)
        {
            def = gene.def.defName;
        }
    }
}