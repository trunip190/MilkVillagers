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
        public bool CollectItems = true;
        public bool IgnoreVillagerGender = false;

        // Farmer gender overrides.
        public bool OverrideGenitals = false;
        public bool HasPenis = false;
        public bool HasBreasts = false;

        // Content
        public bool ExtraDialogue = true;
        public bool ThirdParty = true;
        public bool Quests = true;

        // Debugging
        public bool Debug = false;
        public bool Verbose = false;

        // Itemtype Constants
        public readonly int MilkType = -34;
        public readonly int CumType = -35;
        public readonly int SpecialType = -36;

        public SButton MilkButton = SButton.O;
    }

    public static class TempRefs
    {
        // Universal access
        public static IModHelper Helper;
        public static IMonitor Monitor;
        public static SButton ActionKey = SButton.O;

        public static List<NPC> milkedtoday = new List<NPC>();
        public static List<NPC> SexToday = new List<NPC>();
        public static bool SelfMilkedToday = false;
        public static bool SelfCummedToday = false;

        // Extra Dialogue

        public static string FarmerCollectCum = "You free your penis and start using one hand to gently stroke it, and the other to massage your balls. " +
        "You close your eyes as you start to imagine someone leaning down and engulfing your cock between their lips. " +
        "#You pick up your pace as their imaginary mouth starts sucking harder, occasionally flicking your tip with their tongue. " +
        "Your clench your eyes tighter as your cum starts to spurt out, painting the inside of your jar. You seal the lid with a contented sigh. ";

        public static string FarmerCollectionMilk = "You glance around for a mo to see if anyone is watching you, then slip your top over your head. " +
        "You start massaging you breast, and your nipples are soon standing proud, calling for attention. " +
        "#As you start kneading in earnest, a trickle quickly emerges from the sides of your nipple, and quickly becomes a stream. " +
        "The flow increases as you find your rhythm, and the drip-drip of it collecting in your bottle is hypnotising. " +
        "#After several minutes of switching between breasts you are much emptier, and you pull your top back on. " +
        "The fabric rubs against your slightly sore nipples.";

        // Config options
        public static bool loaded = false;
        public static bool thirdParty = true;
        public static bool Verbose = false;
        public static bool MilkMale = true;
        public static bool MilkFemale = true;
        public static bool OverrideGenitals = false;
        public static bool HasPenis = false;
        public static bool HasBreasts = false;
        public static bool IgnoreVillagerGender = false;


        #region Quest ID storage
        public static int QuestAbi1 = 594801;       // Abigail's Eggplant
        public static int QuestAbi2 = 594802;       // Abigail's Carrot
        public static int QuestAbi3 = 594803;       // Abigail's Radishes
        public static int QuestAbi4 = 594804;       // Abigail's 'Helping hand'
        public static int QuestMaru1 = 594805;      // Scientific sample
        public static int QuestGeorge1 = 594806;    // Rejuvenating Milk
        public static int QuestSeb1 = 594807;       // Curious tastes pt 1
        public static int QuestSeb2 = 594808;       // Curious tastes pt 2
        public static int QuestLeah1 = 594809;      // Leah's Muse quest
        public static int QuestPenny1 = 594810;     // Penny check out book quest.

        public static int QuestIDWait = -1;         //594800; // Wait for Abigail.
        #endregion

        #region Event ID storage
        public static int EventAbi = 594801;        //Abigail
        public static int EventHarvey = 594802;     //Harvey
        public static int Event3HarMar = 594803;    //Harvey/Maru
        public static int EventLeah = 594804;       //Leah
        public static int EventPenny = 594805;      //Penny
        public static int Event3HaleyAlex = 594806; //Haley/Alex
        public static int EventCaroline = 594807;   //Caroline
        public static int EventMaru = 594808;       //Maru
        public static int EventEmily = 594809;      //Emily
        public static int EventLeahExhibit = 594810;//Leah Exhibitionism
        //public static int Event                     //
        //public static int Event                     //
        //public static int Event                     //
        //public static int Event                     //
        //public static int Event                     //
        //public static int Event                     //
        #endregion

        #region Items
        // Milk item code storage
        public static int MilkAbig = 803;
        public static int MilkEmil = 803;
        public static int MilkHale = 803;
        public static int MilkLeah = 803;
        public static int MilkMaru = 803;
        public static int MilkPenn = 803;
        public static int MilkCaro = 803;
        public static int MilkJodi = 803;
        public static int MilkMarn = 803;
        public static int MilkRobi = 803;
        public static int MilkPam = 803;
        public static int MilkSand = 803;
        public static int MilkEvel = 803;
        /// <summary>
        /// Generic Breast milk
        /// </summary>
        public static int MilkGeneric = 803;

        // Cum item code storage.
        /// <summary>
        /// Generic cum
        /// </summary>
        public static int MilkSpecial = 803;
        public static int MilkAlex = 803;
        public static int MilkClint = 803;
        public static int MilkDemetrius = 803;
        public static int MilkElliott = 803;
        public static int MilkGeorge = 803;
        public static int MilkGil = 803;
        public static int MilkGunther = 803;
        public static int MilkGus = 803;
        public static int MilkHarv = 803;
        public static int MilkKent = 803;
        public static int MilkLewis = 803;
        public static int MilkLinus = 803;
        public static int MilkMarlon = 803;
        public static int MilkMorris = 803;
        public static int MilkPierre = 803;
        public static int MilkSam = 803;
        public static int MilkSeb = 803;
        public static int MilkShane = 803;
        public static int MilkWilly = 803;

        // Magical
        public static int MilkDwarf = 803;
        public static int MilkKrobus = 803;
        public static int MilkQi = 803;
        public static int MilkWiz = 803;
        public static int MilkMagic = 803;

        // Recipe item code storage
        public static int ProteinShake = 1240;
        public static int MilkShake = 1241;
        public static int SuperJuice = 1249;

        // Other mods
        public static int MilkSophia = 803;
        public static int MilkOlivia = 803;
        public static int MilkSusan = 803;
        public static int MilkClaire = 803;
        public static int MilkScarlett = 803;
        public static int MilkAndy = 803;
        public static int MilkVictor = 803;
        public static int MilkMartin = 803;
        #endregion

        // Item types 
        public static readonly int MilkType = -34;
        public static readonly int CumType = -35;
        public static readonly int SpecialType = -36;



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
