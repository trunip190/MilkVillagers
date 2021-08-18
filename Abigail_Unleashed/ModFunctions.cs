using StardewValley;
using System.Collections.Generic;

namespace MilkVillagers
{
    public static class ModFunctions
    {
        public static bool LookingAtNPC(int[] target, int[] NPC)
        {
            return target[0] == NPC[0] && target[1] == NPC[1];
        }

        public static NPC FindTarget(int[] target, int[] farmerPos)
        {
            int[] NPC = new int[2];
            using (List<NPC>.Enumerator enumerator = Game1.currentLocation.characters.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    NPC current = enumerator.Current;
                    NPC[0] = current.getTileX();
                    NPC[1] = current.getTileY();
                    if (ModFunctions.LookingAtNPC(target, NPC) || ModFunctions.LookingAtNPC(farmerPos, NPC))
                        return current;
                }
            }
            return (NPC)null;
        }
    }
}
