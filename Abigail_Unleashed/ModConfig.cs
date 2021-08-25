using System.Collections.Generic;
using StardewModdingAPI;
using StardewValley;

namespace MilkVillagers
{
    internal class ModConfig
    {
        public int CoolDownAmount = 3;
        //public int QuestID1 = 594801; // Abigail's Eggplant
        //public int QuestID2 = 594802; // Abigail's Carrot
        //public int QuestID3 = 594803; // Abigail's Radishes
        //public int QuestID4 = 594804; // Abigail's 'Helping hand'
        //public int QuestID5 = 594805; // Wait for Abigail.
        public bool MilkMale = true;
        public bool MilkFemale = true;
        public bool Verbose = false;
        public bool NeedTool = false;
        public bool StackMilk = false;
        public bool ExtraDialogue = false;
        public bool thirdParty = true;
        public bool debug = true;
    }

    public static class TempRefs
    {
        public static IModHelper Helper;
        public static IMonitor Monitor;
        public static bool loaded = false;
        public static List<NPC> milkedtoday = new List<NPC>();
        public static bool thirdParty = true;

        // Quest ID storage
        public static int QuestID1 = 594801; // Abigail's Eggplant
        public static int QuestID2 = 594802; // Abigail's Carrot
        public static int QuestID3 = 594803; // Abigail's Radishes
        public static int QuestID4 = 594804; // Abigail's 'Helping hand'
        public static int QuestIDWait = 594805; // Wait for Abigail.

        // Milk item code storage
        public static int MilkAbig = 1201;
        public static int MilkEmil = 1202;
        public static int MilkHale = 1203;
        public static int MilkLeah = 1204;
        public static int MilkMaru = 1205;
        public static int MilkPenn = 1206;
        public static int MilkCaro = 1207;
        public static int MilkJodi = 1208;
        public static int MilkMarn = 1209;
        public static int MilkRobi = 1210;
        public static int MilkPam = 1211;
        public static int MilkSand = 1212;
        public static int MilkEvel = 1213;
        public static int MilkDwarf = 1239;
        public static int MilkGeneric = 1214;

        // Cum item code storage.
        public static int MilkSpecial = 1215;
        public static int MilkAlex = 1216;
        public static int MilkClint = 1217;
        public static int MilkDemetrius = 1218;
        public static int MilkElliott = 1219;
        public static int MilkGeorge = 1220;
        public static int MilkGil = 1221;
        public static int MilkGunther = 1222;
        public static int MilkGus = 1223;
        public static int MilkHarv = 1224;
        public static int MilkKent = 1225;
        public static int MilkLewis = 1226;
        public static int MilkLinus = 1227;
        public static int MilkMarlon = 1228;
        public static int MilkMorris = 1229;
        public static int MilkQi = 1230;
        public static int MilkPierre = 1231;
        public static int MilkSam = 1232;
        public static int MilkSeb = 1233;
        public static int MilkShane = 1234;
        public static int MilkWilly = 1235;
        public static int MilkWiz = 1236;
        public static int MilkWMarlon = 1237;
        public static int MilkKrobus = 1238;

        // Recipe item code storage
        public static int ProteinShake = 1216;
        public static int MilkShake = 1217;

        // Item types 
        public static int MilkType = -34;
        public static int CumType = -35;

        public static void ReportCodes()
        {
            // Milk item code storage
            Monitor.Log($"MilkAbig is {MilkAbig}", LogLevel.Trace);
            Monitor.Log($"MilkEmil is {MilkEmil}", LogLevel.Trace);
            Monitor.Log($"MilkHale is {MilkHale}", LogLevel.Trace);
            Monitor.Log($"MilkLeah is {MilkLeah}", LogLevel.Trace);
            Monitor.Log($"MilkMaru is {MilkMaru}", LogLevel.Trace);
            Monitor.Log($"MilkPenn is {MilkPenn}", LogLevel.Trace);
            Monitor.Log($"MilkCaro is {MilkCaro}", LogLevel.Trace);
            Monitor.Log($"MilkJodi is {MilkJodi}", LogLevel.Trace);
            Monitor.Log($"MilkMarn is {MilkMarn}", LogLevel.Trace);
            Monitor.Log($"MilkRobi is {MilkRobi}", LogLevel.Trace);
            Monitor.Log($"MilkPam is {MilkPam}", LogLevel.Trace);
            Monitor.Log($"MilkSand is {MilkSand}", LogLevel.Trace);
            Monitor.Log($"MilkEvel is {MilkEvel}", LogLevel.Trace);
            Monitor.Log($"MilkDwarf is {MilkDwarf}", LogLevel.Trace);
            Monitor.Log($"MilkGeneric is {MilkGeneric}", LogLevel.Trace);

            // Cum item code storage.
            Monitor.Log($"MilkSpecial is {MilkSpecial}", LogLevel.Trace);
            Monitor.Log($"MilkAlex is {MilkAlex}", LogLevel.Trace);
            Monitor.Log($"MilkClint is {MilkClint}", LogLevel.Trace);
            Monitor.Log($"MilkDemetrius is {MilkDemetrius}", LogLevel.Trace);
            Monitor.Log($"MilkElliott is {MilkElliott}", LogLevel.Trace);
            Monitor.Log($"MilkGeorge is {MilkGeorge}", LogLevel.Trace);
            Monitor.Log($"MilkGil is {MilkGil}", LogLevel.Trace);
            Monitor.Log($"MilkGunther is {MilkGunther}", LogLevel.Trace);
            Monitor.Log($"MilkGus is {MilkGus}", LogLevel.Trace);
            Monitor.Log($"MilkHarv is {MilkHarv}", LogLevel.Trace);
            Monitor.Log($"MilkKent is {MilkKent}", LogLevel.Trace);
            Monitor.Log($"MilkLewis is {MilkLewis}", LogLevel.Trace);
            Monitor.Log($"MilkLinus is {MilkLinus}", LogLevel.Trace);
            Monitor.Log($"MilkMarlon is {MilkMarlon}", LogLevel.Trace);
            Monitor.Log($"MilkMorris is {MilkMorris}", LogLevel.Trace);
            Monitor.Log($"MilkQi is {MilkQi}", LogLevel.Trace);
            Monitor.Log($"MilkPierre is {MilkPierre}", LogLevel.Trace);
            Monitor.Log($"MilkSam is {MilkSam}", LogLevel.Trace);
            Monitor.Log($"MilkSeb is {MilkSeb}", LogLevel.Trace);
            Monitor.Log($"MilkShane is {MilkShane}", LogLevel.Trace);
            Monitor.Log($"MilkWilly is {MilkWilly}", LogLevel.Trace);
            Monitor.Log($"MilkWiz is {MilkWiz}", LogLevel.Trace);
            Monitor.Log($"MilkMarlon is {MilkMarlon}", LogLevel.Trace);
            Monitor.Log($"MilkKrobus is {MilkKrobus}", LogLevel.Trace);

            // Recipe item code storage
            Monitor.Log($"ProteinShake is {ProteinShake}", LogLevel.Trace);
            Monitor.Log($"MilkShake is {MilkShake}", LogLevel.Trace);

            // Item types
            Monitor.Log($"MilkType is {MilkType}", LogLevel.Trace);
            Monitor.Log($"CumType is {CumType}", LogLevel.Trace);
        }
    }
}
