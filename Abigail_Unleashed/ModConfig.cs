using System.Collections.Generic;
using StardewModdingAPI;
using StardewValley;

namespace MilkVillagers
{
    internal class ModConfig
    {
        public int CoolDownAmount = 3;
        public int QuestID1 = 300001;
        public int QuestID2 = 300002;
        public int QuestID3 = 300003;
        public int QuestID4 = 300004;
        public bool MilkMale = true;
        public bool MilkFemale = true;
        public bool PailGiven;
        public bool Verbose = false;
        public bool NeedTool = false;
        public bool StackMilk = false;
    }

    public static class TempRefs
    {
        public static IMonitor Monitor;
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
        public static int MilkGeneric = 1214;
        public static bool loaded = false;
        public static List<NPC> milkedtoday = new List<NPC>();
        public static int QuestID1 = 300001;
        public static int QuestID2 = 300002;
        public static int QuestID3 = 300003;
        public static int QuestID4 = 300004;
        public static IModHelper Helper;
    }
}
