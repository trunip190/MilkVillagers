using StardewModdingAPI;
using StardewValley;
using System.Collections.Generic;
using sObject = StardewValley.Object;

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

        public static sObject NewItem(string Villager)
        {
            sObject result;

            switch (Villager)
            {
                case "Abigail's Milk": { result = new sObject(TempRefs.MilkAbig, 1, quality: 2); goto cleanup; }
                case "Caroline's Milk": { result = new sObject(TempRefs.MilkCaro, 1, quality: 2); goto cleanup; }
                case "Emily's Milk": { result = new sObject(TempRefs.MilkEmil, 1, quality: 2); goto cleanup; }
                case "Evelyn's Milk": { result = new sObject(TempRefs.MilkEvel, 1, quality: 0); goto cleanup; }
                case "Haley's Milk": { result = new sObject(TempRefs.MilkHale, 1, quality: 2); goto cleanup; }
                case "Jodi's Milk": { result = new sObject(TempRefs.MilkJodi, 1, quality: 2); goto cleanup; }
                case "Leah's Milk": { result = new sObject(TempRefs.MilkLeah, 1, quality: 2); goto cleanup; }
                case "Marnie's Milk": { result = new sObject(TempRefs.MilkMarn, 1, quality: 2); goto cleanup; }
                case "Maru's Milk": { result = new sObject(TempRefs.MilkMaru, 1, quality: 2); goto cleanup; }
                case "Pam's Milk": { result = new sObject(TempRefs.MilkPam, 1, quality: 0); goto cleanup; }
                case "Penny's Milk": { result = new sObject(TempRefs.MilkPenn, 1, quality: 2); goto cleanup; }
                case "Robin's Milk": { result = new sObject(TempRefs.MilkRobi, 1, quality: 2); goto cleanup; }
                case "Sandy's Milk": { result = new sObject(TempRefs.MilkSand, 1, quality: 2); goto cleanup; }

                case "Sophia's Milk ": { result = new sObject(TempRefs.MilkSophia, 1, quality: 2); goto cleanup; }
                case "Olivia's Milk": { result = new sObject(TempRefs.MilkOlivia, 1, quality: 2); goto cleanup; }
                case "Susan's Milk": { result = new sObject(TempRefs.MilkSusan, 1, quality: 2); goto cleanup; }
                case "Claire's Milk": { result = new sObject(TempRefs.MilkClaire, 1, quality: 2); goto cleanup; }


                case "Alex's Cum": { result = new sObject(TempRefs.MilkAlex, 1, quality: 2); goto cleanup; }
                case "Clint's Cum": { result = new sObject(TempRefs.MilkClint, 1, quality: 1); goto cleanup; }
                case "Demetrius's Cum": { result = new sObject(TempRefs.MilkDemetrius, 1, quality: 2); goto cleanup; }
                case "Elliott's Cum": { result = new sObject(TempRefs.MilkElliott, 1, quality: 2); goto cleanup; }
                case "George's Cum": { result = new sObject(TempRefs.MilkGeorge, 1, quality: 0); goto cleanup; }
                case "Gunther's Cum": { result = new sObject(TempRefs.MilkGunther, 1, quality: 1); goto cleanup; }
                case "Gus's Cum": { result = new sObject(TempRefs.MilkGus, 1, quality: 1); goto cleanup; }
                case "Harvey's Cum": { result = new sObject(TempRefs.MilkHarv, 1, quality: 2); goto cleanup; }
                case "Kent's Cum": { result = new sObject(TempRefs.MilkKent, 1, quality: 2); goto cleanup; }
                case "Lewis's Cum": { result = new sObject(TempRefs.MilkLewis, 1, quality: 0); goto cleanup; }
                case "Linus's Cum": { result = new sObject(TempRefs.MilkLinus, 1, quality: 2); goto cleanup; }
                case "Marlon's Cum": { result = new sObject(TempRefs.MilkMarlon, 1, quality: 2); goto cleanup; }
                case "Morris's Cum": { result = new sObject(TempRefs.MilkMorris, 1, quality: 0); goto cleanup; }
                case "Pierre's Cum": { result = new sObject(TempRefs.MilkPierre, 1, quality: 1); goto cleanup; }
                case "Sam's Cum": { result = new sObject(TempRefs.MilkSam, 1, quality: 2); goto cleanup; }
                case "Sebastian's Cum": { result = new sObject(TempRefs.MilkSeb, 1, quality: 2); goto cleanup; }
                case "Shane's Cum": { result = new sObject(TempRefs.MilkShane, 1, quality: 2); goto cleanup; }
                case "Willy's Cum": { result = new sObject(TempRefs.MilkWilly, 1, quality: 1); goto cleanup; }


                case "Dwarf's Essence": { result = new sObject(TempRefs.MilkDwarf, 1, quality: 1); goto cleanup; }
                case "Krobus Essence": { result = new sObject(TempRefs.MilkKrobus, 1, quality: 2); goto cleanup; }
                case "Mr. Qi's Essence": { result = new sObject(TempRefs.MilkQi, 1, quality: 2); goto cleanup; }
                case "Wizard's Essence": { result = new sObject(TempRefs.MilkWiz, 1, quality: 2); goto cleanup; }

                case "Andy's Cum": { result = new sObject(TempRefs.MilkAndy, 1, quality: 2); goto cleanup; }
                case "Victor's Cum": { result = new sObject(TempRefs.MilkVictor, 1, quality: 2); goto cleanup; }
                case "Martin's Cum": { result = new sObject(TempRefs.MilkMartin, 1, quality: 2); goto cleanup; }

                default:
                    result = new sObject(184, 1, quality: 1);
                    break;
            }

        cleanup:
            return result;
        }

        public static sObject NewItem(Farmer who, sObject Item)
        {
            sObject result = Item;

            switch (Item.Name)
            {
                case "Abigail's Milk": { result.Quality = 2; goto cleanup; }
                case "Caroline's Milk": { result.Quality = 2; goto cleanup; }
                case "Emily's Milk": { result.Quality = 2; goto cleanup; }
                case "Evelyn's Milk": { result.Quality = 0; goto cleanup; }
                case "Haley's Milk": { result.Quality = 2; goto cleanup; }
                case "Jodi's Milk": { result.Quality = 2; goto cleanup; }
                case "Leah's Milk": { result.Quality = 2; goto cleanup; }
                case "Marnie's Milk": { result.Quality = 2; goto cleanup; }
                case "Maru's Milk": { result.Quality = 2; goto cleanup; }
                case "Pam's Milk": { result.Quality = 0; goto cleanup; }
                case "Penny's Milk": { result.Quality = 2; goto cleanup; }
                case "Robin's Milk": { result.Quality = 2; goto cleanup; }
                case "Sandy's Milk": { result.Quality = 2; goto cleanup; }

                case "Sophia's Milk": { result.Quality = 2; goto cleanup; }
                case "Olivia's Milk": { result.Quality = 2; goto cleanup; }
                case "Susan's Milk": { result.Quality = 2; goto cleanup; }
                case "Claire's Milk": { result.Quality = 2; goto cleanup; }


                case "Alex's Cum": { result.Quality = 2; goto cleanup; }
                case "Clint's Cum": { result.Quality = 1; goto cleanup; }
                case "Demetrius's Cum": { result.Quality = 2; goto cleanup; }
                case "Elliott's Cum": { result.Quality = 2; goto cleanup; }
                case "George's Cum": { result.Quality = 0; goto cleanup; }
                case "Gunther's Cum": { result.Quality = 1; goto cleanup; }
                case "Gus's Cum": { result.Quality = 1; goto cleanup; }
                case "Harvey's Cum": { result.Quality = 2; goto cleanup; }
                case "Kent's Cum": { result.Quality = 2; goto cleanup; }
                case "Lewis's Cum": { result.Quality = 0; goto cleanup; }
                case "Linus's Cum": { result.Quality = 2; goto cleanup; }
                case "Marlon's Cum": { result.Quality = 2; goto cleanup; }
                case "Morris's Cum": { result.Quality = 0; goto cleanup; }
                case "Pierre's Cum": { result.Quality = 1; goto cleanup; }
                case "Sam's Cum": { result.Quality = 2; goto cleanup; }
                case "Sebastian's Cum": { result.Quality = 2; goto cleanup; }
                case "Shane's Cum": { result.Quality = 2; goto cleanup; }
                case "Willy's Cum": { result.Quality = 1; goto cleanup; }


                case "Dwarf's Essence": { result.Quality = 1; goto cleanup; }
                case "Krobus Essence": { result.Quality = 2; goto cleanup; }
                case "Mr. Qi's Essence": { result.Quality = 2; goto cleanup; }
                case "Wizard's Essence": { result.Quality = 2; goto cleanup; }

                case "Andy's Cum": { result.Quality = 2; goto cleanup; }
                case "Victor's Cum": { result.Quality = 2; goto cleanup; }
                case "Martin's Cum": { result.Quality = 2; goto cleanup; }

                default:
                    result = new sObject(184, 1);
                    break;
            }

            if (who.mailReceived.Contains("MilkingProfQuality")) { result.Quality = 4; }

        cleanup:
            return result;
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
