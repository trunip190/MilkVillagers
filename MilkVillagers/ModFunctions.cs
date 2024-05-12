using StardewModdingAPI;
using StardewValley;
using StardewValley.Network;
using System.Collections.Generic;
using System.IO;
using sObject = StardewValley.Object;

namespace MilkVillagers
{

    public static class ModFunctions
    {
        public static List<string> topics = new List<string>
            {
                "5984GeorgeMed",
                "5984GeorgeMedBJ",
                "5984GeorgeMedHJ",
                "5984GeorgeMedSex",
                "BJ",
                "eat_out",
                "get_eaten",
                "HaleyPanties",
                "HaleyPantiesFinger",
                "HaleyPantiesLick",
                "HaleyPantiesReject",
                "HaleyPantiesSex",
                "milk_fast",
                "milk_start",
                "MTV_BathhouseFun",
                "MTV_BoeBook1",
                "MTV_BoeBook2",
                "MTV_BoeBook3",
                "MTV_BoethiaBook",
                "MTV_BoethiaBookP1",
                "MTV_BoethiaBookP2",
                "MTV_BoethiaBookP3",
                "MTV_Bukkake",
                "mtv_ctcamshow",
                "MTV_GeorgeQ4",
                "QuestStartFail",
                "QuestStartSuccess",
                "sex"
            };

        public static List<string> chars = new List<string> {
            "Abigail",
            "Alex",
            "Caroline",
            "Clint",
            "Demetrius",
            "Dwarf",
            "Elliott",
            "Emily",
            "Evelyn",
            "George",
            "Gil",
            "Gunther",
            "Gus",
            "Haley",
            "Harvey",
            "Jodi",
            "Kent",
            "Krobus",
            "Leah",
            "Lewis",
            "Linus",
            "Magnus",
            "Marlon",
            "Marnie",
            "Maru",
            "Morris",
            "Mr. Qi",
            "Pam",
            "Penny",
            "Pierre",
            "Robin",
            "Sam",
            "Sandy",
            "Sebastian",
            "Shane",
            "Willy",
            "Wizard"
            };

        public static void Log(string message, LogLevel Level = LogLevel.Trace, bool Force = false)
        {
            if (message == null || TempRefs.Monitor == null)
                return;

            if (TempRefs.Verbose || Force) TempRefs.Monitor.Log(message, Level);
        }

        public static bool LookingAtNPC(int[] target, int[] NPC)
        {
            return target[0] == NPC[0] && target[1] == NPC[1];
        }

        public static Farmer FindFarmer(GameLocation location, int[] target, int[] farmerPos)
        {
            int[] NPC = new int[2];

            using (FarmerCollection.Enumerator enumerator = location.farmers.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {

                    Farmer current = enumerator.Current;
                    NPC[0] = (int)current.GetGrabTile().X;
                    NPC[1] = (int)current.GetGrabTile().Y;

                    if (LookingAtNPC(target, NPC) || LookingAtNPC(farmerPos, NPC))
                        return current;
                }
            }

            return null;
        }

        public static NPC FindTarget(GameLocation location, int[] target, int[] farmerPos)
        {
            int[] NPC = new int[2];

            using (List<NPC>.Enumerator enumerator = location.characters.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    NPC current = enumerator.Current;
                    NPC[0] = (int)current.GetGrabTile().X;
                    NPC[1] = (int)current.GetGrabTile().Y;
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
                case "Abigail's Milk":  { result = new sObject("Abigail's Milk", 1, quality: 2); goto cleanup; }
                case "Caroline's Milk": { result = new sObject("Caroline's Milk", 1, quality: 2); goto cleanup; }
                case "Emily's Milk":    { result = new sObject("Emily's Milk", 1, quality: 2); goto cleanup; }
                case "Evelyn's Milk":   { result = new sObject("Evelyn's Milk", 1, quality: 0); goto cleanup; }
                case "Haley's Milk":    { result = new sObject("Haley's Milk", 1, quality: 2); goto cleanup; }
                case "Jodi's Milk":     { result = new sObject("Jodi's Milk", 1, quality: 2); goto cleanup; }
                case "Leah's Milk":     { result = new sObject("Leah's Milk", 1, quality: 2); goto cleanup; }
                case "Marnie's Milk":   { result = new sObject("Marnie's Milk", 1, quality: 2); goto cleanup; }
                case "Maru's Milk":     { result = new sObject("Maru's Milk", 1, quality: 2); goto cleanup; }
                case "Pam's Milk":      { result = new sObject("Pam's Milk", 1, quality: 0); goto cleanup; }
                case "Penny's Milk":    { result = new sObject("Penny's Milk", 1, quality: 2); goto cleanup; }
                case "Robin's Milk":    { result = new sObject("Robin's Milk", 1, quality: 2); goto cleanup; }
                case "Sandy's Milk":    { result = new sObject("Sandy's Milk", 1, quality: 2); goto cleanup; }

                case "Sophia's Milk ":  { result = new sObject("Sophia's Milk", 1, quality: 2); goto cleanup; }
                case "Olivia's Milk":   { result = new sObject("Olivia's Milk", 1, quality: 2); goto cleanup; }
                case "Susan's Milk":    { result = new sObject("Susan's Milk", 1, quality: 2); goto cleanup; }
                case "Claire's Milk":   { result = new sObject("Claire's Milk", 1, quality: 2); goto cleanup; }


                case "Alex's Cum":      { result = new sObject("Alex's Cum", 1, quality: 2); goto cleanup; }
                case "Clint's Cum":     { result = new sObject("Clint's Cum", 1, quality: 1); goto cleanup; }
                case "Demetrius's Cum": { result = new sObject("Demetrius's Cum", 1, quality: 2); goto cleanup; }
                case "Elliott's Cum":   { result = new sObject("Elliott's Cum", 1, quality: 2); goto cleanup; }
                case "George's Cum":    { result = new sObject("George's Cum", 1, quality: 0); goto cleanup; }
                case "Gil's Cum":       { result = new sObject("Gil's Cum", 0 ,quality:0);        goto cleanup; }
                case "Gunther's Cum":   { result = new sObject("Gunther's Cum", 1, quality: 1); goto cleanup; }
                case "Gus's Cum":       { result = new sObject("Gus's Cum", 1, quality: 1); goto cleanup; }
                case "Harvey's Cum":    { result = new sObject("Harvey's Cum", 1, quality: 2); goto cleanup; }
                case "Kent's Cum":      { result = new sObject("Kent's Cum", 1, quality: 2); goto cleanup; }
                case "Lewis's Cum":     { result = new sObject("Lewis's Cum", 1, quality: 0); goto cleanup; }
                case "Linus's Cum":     { result = new sObject("Linus's Cum", 1, quality: 2); goto cleanup; }
                case "Marlon's Cum":    { result = new sObject("Marlon's Cum", 1, quality: 2); goto cleanup; }
                case "Morris's Cum":    { result = new sObject("Morris's Cum", 1, quality: 0); goto cleanup; }
                case "Pierre's Cum":    { result = new sObject("Pierre's Cum", 1, quality: 1); goto cleanup; }
                case "Sam's Cum":       { result = new sObject("Sam's Cum", 1, quality: 2); goto cleanup; }
                case "Sebastian's Cum": { result = new sObject("Sebastian's Cum", 1, quality: 2); goto cleanup; }
                case "Shane's Cum":     { result = new sObject("Shane's Cum", 1, quality: 2); goto cleanup; }
                case "Willy's Cum":     { result = new sObject("Willy's Cum", 1, quality: 1); goto cleanup; }


                case "Dwarf's Essence": { result = new sObject("Dwarf's Essence", 1, quality: 1); goto cleanup; }
                case "Krobus Essence":  { result = new sObject("Krobus Essence", 1, quality: 2); goto cleanup; }
                case "Mr. Qi's Essence":{ result = new sObject("Mr. Qi's Essence", 1, quality: 2); goto cleanup; }
                case "Wizard's Essence":{ result = new sObject("Wizard's Essence", 1, quality: 2); goto cleanup; }

                case "Andy's Cum":      { result = new sObject("Andy's Cum"     , 1, quality: 2); goto cleanup; }
                case "Lance's Cum":     { result = new sObject("Lance's Cum"    , 1,quality: 2);goto cleanup; }
                case "Victor's Cum":    { result = new sObject("Victor's Cum", 1, quality: 2); goto cleanup; }
                case "Martin's Cum":    { result = new sObject("Martin's Cum", 1, quality: 2); goto cleanup; }

                default:
                    result = new sObject("Milk", 1, quality: 1);
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
                    result = new sObject("184", 1);
                    break;
            }

            if (who.mailReceived.Contains("MilkingProfQuality")) { result.Quality = 4; }

        cleanup:
            return result;
        }

        public static Dialogue ProcessDialogue(Dialogue input)
        {
            Dialogue output = new Dialogue(input);

            foreach (var v in output.dialogues)
            {
                while ((v.Text.Contains('[') && v.Text.Contains(']')) || (v.Text.Contains('{') && v.Text.Contains('}')))
                {
                    int first = 0;
                    int last = 0;
                    if (v.Text.Contains('[') && v.Text.Contains(']'))
                    {
                        first = v.Text.IndexOf('[');
                        last = v.Text.IndexOf(']', first);
                        v.Text = v.Text.Remove(first, last - first + 1);
                    }

                    if (v.Text.Contains('{') && v.Text.Contains('}'))
                    {
                        first = v.Text.IndexOf('{');
                        last = v.Text.IndexOf('}', first);
                        v.Text = v.Text.Remove(first, last - first + 1);
                    }
                }

            }

            return output;
        }

        public static List<string> GetDirectories(string root)
        {
            List<string> result = new List<string>();
            result.Add(root);

            foreach (string s in Directory.GetDirectories(root))
            {
                result.AddRange(GetDirectories(s));
            }

            return result;
        }

        //TODO implement this check when getting the farmer being looked at.
        public static Farmer FindTargetFarmer(GameLocation location, int[] target, int[] farmerpos)
        {
            int[] tempPos = new int[2];
            foreach (Farmer who in Game1.getOnlineFarmers())
            {
                tempPos[0] = (int)who.GetGrabTile().X;
                tempPos[1] = (int)who.GetGrabTile().Y;
                if (location == who.currentLocation && (LookingAtNPC(target, tempPos) || LookingAtNPC(farmerpos, tempPos)))
                    return who;
            }
            return null;
        }
    }
}
