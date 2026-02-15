using RimWorld;

namespace PawnSaveUtility
{
    public class PawnStyle
    {
        public string beardDef;

        public string faceTattoo;

        public string bodyTattoo;

        public PawnStyle()
        {

        }

        public PawnStyle(Pawn_StyleTracker styleTracker)
        {
            beardDef = styleTracker.beardDef.defName;

            faceTattoo = styleTracker.FaceTattoo.defName;

            bodyTattoo = styleTracker.BodyTattoo.defName;
        }
    }
}