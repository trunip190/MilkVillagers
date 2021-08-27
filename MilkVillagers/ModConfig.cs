using System.Collections.Generic;
using StardewModdingAPI;
using StardewValley;

namespace MilkVillagers
{
    internal class ModConfig
    {
        // Parts of mod
        public bool MilkMale = true;
        public bool MilkFemale = true;
        public bool StackMilk = false;

        // Content
        public bool ExtraDialogue = false;
        public bool ThirdParty = true;
        public bool Quests = true;

        // Debugging
        public bool Debug = true;
        public bool Verbose = false;
    }

    public static class TempRefs
    {
        // Universal access
        public static IModHelper Helper;
        public static IMonitor Monitor;
        public static List<NPC> milkedtoday = new List<NPC>();

        // Config options
        public static bool loaded = false;
        public static bool thirdParty = true;
        public static bool Verbose = false;

        #region Quest ID storage
        public static int QuestID1 = 594801; // Abigail's Eggplant
        public static int QuestID2 = 594802; // Abigail's Carrot
        public static int QuestID3 = 594803; // Abigail's Radishes
        public static int QuestID4 = 594804; // Abigail's 'Helping hand'
        public static int QuestID5 = 594805; // Scientific sample
        public static int QuestID6 = 594806; // Rejuvenating Milk
        public static int QuestID7 = 594807; // Curious tastes pt 1
        public static int QuestID8 = 594808; // Curious tastes pt 2

        public static int QuestIDWait = -1; //594800; // Wait for Abigail.
        #endregion

        #region Items
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
        public static int MilkDwarf = 1214;
        public static int MilkGeneric = 1215;

        // Cum item code storage.
        public static int MilkSpecial = 1216;
        public static int MilkAlex = 1217;
        public static int MilkClint = 1218;
        public static int MilkDemetrius = 1219;
        public static int MilkElliott = 1220;
        public static int MilkGeorge = 1221;
        public static int MilkGil = 1222;
        public static int MilkGunther = 1223;
        public static int MilkGus = 1224;
        public static int MilkHarv = 1225;
        public static int MilkKent = 1226;
        public static int MilkLewis = 1227;
        public static int MilkLinus = 1228;
        public static int MilkMarlon = 1229;
        public static int MilkMorris = 1230;
        public static int MilkQi = 1231;
        public static int MilkPierre = 1232;
        public static int MilkSam = 1233;
        public static int MilkSeb = 1234;
        public static int MilkShane = 1235;
        public static int MilkWilly = 1236;
        public static int MilkWiz = 1237;
        public static int MilkWMarlon = 1238;
        public static int MilkKrobus = 1239;

        // Recipe item code storage
        public static int ProteinShake = 1240;
        public static int MilkShake = 1241;

        // Other mods
        public static int MilkSophia = 1242;
        public static int MilkOlivia = 1243;
        public static int MilkSusan = 1244;
        public static int MilkClaire = 1245;
        public static int MilkAndy = 1246;
        public static int MilkVictor = 1247;
        public static int MilkMartin = 1248;
        #endregion

        // Item types 
        public static int MilkType = -34;
        public static int CumType = -35;

        public static void ReportCodes()
        {
            // Milk item code storage
            ModFunctions.LogVerbose($"MilkAbig is {MilkAbig}", LogLevel.Trace);
            ModFunctions.LogVerbose($"MilkEmil is {MilkEmil}", LogLevel.Trace);
            ModFunctions.LogVerbose($"MilkHale is {MilkHale}", LogLevel.Trace);
            ModFunctions.LogVerbose($"MilkLeah is {MilkLeah}", LogLevel.Trace);
            ModFunctions.LogVerbose($"MilkMaru is {MilkMaru}", LogLevel.Trace);
            ModFunctions.LogVerbose($"MilkPenn is {MilkPenn}", LogLevel.Trace);
            ModFunctions.LogVerbose($"MilkCaro is {MilkCaro}", LogLevel.Trace);
            ModFunctions.LogVerbose($"MilkJodi is {MilkJodi}", LogLevel.Trace);
            ModFunctions.LogVerbose($"MilkMarn is {MilkMarn}", LogLevel.Trace);
            ModFunctions.LogVerbose($"MilkRobi is {MilkRobi}", LogLevel.Trace);
            ModFunctions.LogVerbose($"MilkPam is {MilkPam}", LogLevel.Trace);
            ModFunctions.LogVerbose($"MilkSand is {MilkSand}", LogLevel.Trace);
            ModFunctions.LogVerbose($"MilkEvel is {MilkEvel}", LogLevel.Trace);
            ModFunctions.LogVerbose($"MilkDwarf is {MilkDwarf}", LogLevel.Trace);
            ModFunctions.LogVerbose($"MilkGeneric is {MilkGeneric}", LogLevel.Trace);

            // Cum item code storage.
            ModFunctions.LogVerbose($"MilkSpecial is {MilkSpecial}", LogLevel.Trace);
            ModFunctions.LogVerbose($"MilkAlex is {MilkAlex}", LogLevel.Trace);
            ModFunctions.LogVerbose($"MilkClint is {MilkClint}", LogLevel.Trace);
            ModFunctions.LogVerbose($"MilkDemetrius is {MilkDemetrius}", LogLevel.Trace);
            ModFunctions.LogVerbose($"MilkElliott is {MilkElliott}", LogLevel.Trace);
            ModFunctions.LogVerbose($"MilkGeorge is {MilkGeorge}", LogLevel.Trace);
            ModFunctions.LogVerbose($"MilkGil is {MilkGil}", LogLevel.Trace);
            ModFunctions.LogVerbose($"MilkGunther is {MilkGunther}", LogLevel.Trace);
            ModFunctions.LogVerbose($"MilkGus is {MilkGus}", LogLevel.Trace);
            ModFunctions.LogVerbose($"MilkHarv is {MilkHarv}", LogLevel.Trace);
            ModFunctions.LogVerbose($"MilkKent is {MilkKent}", LogLevel.Trace);
            ModFunctions.LogVerbose($"MilkLewis is {MilkLewis}", LogLevel.Trace);
            ModFunctions.LogVerbose($"MilkLinus is {MilkLinus}", LogLevel.Trace);
            ModFunctions.LogVerbose($"MilkMarlon is {MilkMarlon}", LogLevel.Trace);
            ModFunctions.LogVerbose($"MilkMorris is {MilkMorris}", LogLevel.Trace);
            ModFunctions.LogVerbose($"MilkQi is {MilkQi}", LogLevel.Trace);
            ModFunctions.LogVerbose($"MilkPierre is {MilkPierre}", LogLevel.Trace);
            ModFunctions.LogVerbose($"MilkSam is {MilkSam}", LogLevel.Trace);
            ModFunctions.LogVerbose($"MilkSeb is {MilkSeb}", LogLevel.Trace);
            ModFunctions.LogVerbose($"MilkShane is {MilkShane}", LogLevel.Trace);
            ModFunctions.LogVerbose($"MilkWilly is {MilkWilly}", LogLevel.Trace);
            ModFunctions.LogVerbose($"MilkWiz is {MilkWiz}", LogLevel.Trace);
            ModFunctions.LogVerbose($"MilkMarlon is {MilkMarlon}", LogLevel.Trace);
            ModFunctions.LogVerbose($"MilkKrobus is {MilkKrobus}", LogLevel.Trace);

            // Other Mods
            ModFunctions.LogVerbose($"MilkSophia is {MilkSophia}", LogLevel.Trace);
            ModFunctions.LogVerbose($"MilkOlivia is {MilkOlivia}", LogLevel.Trace);
            ModFunctions.LogVerbose($"MilkSusan is {MilkSusan}", LogLevel.Trace);
            ModFunctions.LogVerbose($"MilkClaire is {MilkClaire}", LogLevel.Trace);
            ModFunctions.LogVerbose($"MilkAndy is {MilkAndy}", LogLevel.Trace);
            ModFunctions.LogVerbose($"MilkVictor is {MilkVictor}", LogLevel.Trace);
            ModFunctions.LogVerbose($"MilkMartin is {MilkMartin}", LogLevel.Trace);

            // Recipe item code storage
            ModFunctions.LogVerbose($"ProteinShake is {ProteinShake}", LogLevel.Trace);
            ModFunctions.LogVerbose($"MilkShake is {MilkShake}", LogLevel.Trace);

            // Item types
            ModFunctions.LogVerbose($"MilkType is {MilkType}", LogLevel.Trace);
            ModFunctions.LogVerbose($"CumType is {CumType}", LogLevel.Trace);
        }
    }
}
