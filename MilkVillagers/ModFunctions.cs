using StardewModdingAPI;
using StardewValley;
using System;
using System.Collections.Generic;

namespace MilkVillagers
{

    public static class ModFunctions
    {
        public static void LogVerbose(string message, LogLevel Level = LogLevel.Trace, bool Force = false)
        {
            if (TempRefs.Verbose || Force) TempRefs.Monitor.Log(message, Level);
        }

        public static bool LookingAtNPC(int[] target, int[] NPC)
        {
            return target[0] == NPC[0] && target[1] == NPC[1];
        }

        public static NPC FindTarget(GameLocation location, int[] target, int[] farmerPos)
        {
            int[] NPC = new int[2];
            using (List<NPC>.Enumerator enumerator = location.characters.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    NPC current = enumerator.Current;
                    NPC[0] = current.getTileX();
                    NPC[1] = current.getTileY();
                    if (LookingAtNPC(target, NPC) || LookingAtNPC(farmerPos, NPC))
                        return current;
                }
            }
            return null;
        }

        //TODO implement this check when getting the farmer being looked at.
        public static Farmer FindTargetFarmer(GameLocation location, int[] target, int[] farmerpos)
        {
            int[] tempPos = new int[2];
            foreach (Farmer who in Game1.getOnlineFarmers())
            {
                tempPos[0] = who.getTileX();
                tempPos[1] = who.getTileY();
                if (location == who.currentLocation && (LookingAtNPC(target, tempPos) || LookingAtNPC(farmerpos, tempPos)))
                    return who;
            }
            return null;
        }
    }
}
