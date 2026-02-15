using Verse;
using System.Collections.Generic;

namespace PawnSaveUtility
{
    public class PawnEquipment
    {
        public List<PawnItem> items = new List<PawnItem>();

        public PawnEquipment()
        {

        }

        public PawnEquipment(Pawn_EquipmentTracker equipmentTracker)
        {
            foreach(Thing thing in equipmentTracker.AllEquipmentListForReading)
            {
                items.Add(new PawnItem(thing));
            }
        }
    }
}