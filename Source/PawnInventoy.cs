using Verse;
using System.Collections.Generic;

namespace PawnSaveUtility
{
    public class PawnInventory
    {
        public List<PawnItem> items = new List<PawnItem>();

        public PawnInventory()
        {

        }

        public PawnInventory(Pawn_InventoryTracker inventoryTracker)
        {
            foreach(Thing thing in inventoryTracker.innerContainer)
            {
                items.Add(new PawnItem(thing));
            }
        }
    }
}