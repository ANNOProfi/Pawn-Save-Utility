using RimWorld;
using Verse;

namespace PawnSaveUtility
{
    public class PawnTrait
    {
        public string def;

        public int degree;

        public PawnTrait()
        {
            
        }

        public PawnTrait(string def, int degree)
        {
            this.def = def;

            this.degree = degree;
        }
    }
}