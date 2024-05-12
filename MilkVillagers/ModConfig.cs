using System.Collections.Generic;
using System.Linq;
using SpaceCore;
using StardewModdingAPI;
using StardewValley;
using log = MilkVillagers.ModFunctions;

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
        public bool FarmerSavefileMale = true;
        private string _faarmerSavefileGender => FarmerSavefileMale ? "Male" : "Female";
        public bool OverrideGenitals = false;
        public string FarmerGender = "Female";
        public  string FarmerGenderMod => FarmerGender == "Save default" ? _faarmerSavefileGender : FarmerGender;
        public string FarmerGenitals = "Vagina and breasts";
        public bool HasPenis => (FarmerGenitals.ToLower().Contains("penis") || FarmerGenderMod == "Male" && !OverrideGenitals);
        public bool HasVagina => (FarmerGenitals.ToLower().Contains("vagina") || FarmerGenderMod == "Female" && !OverrideGenitals);
        public bool HasBreasts => (FarmerGenitals.ToLower().Contains("breasts") || FarmerGenderMod == "Female" && !OverrideGenitals);
        public bool AceCharacter => (FarmerGender == "A-sexual");

        public int HeartLevel1 = 6;
        public int HeartLevel2 = 8;

        // Content
        public bool ExtraDialogue = true;
        public bool ThirdParty = true;
        public bool Quests = true;
        public bool RushMail = false;

        /// <summary>
        /// List of sex topics to hook into. You can add your own topics here, and add entries under dialogue for the NPC
        /// </summary>
        public Dictionary<string, string> SexTopics = new Dictionary<string, string>()
        {
            ["milk_start"] = "Female 1",
            ["milk_fast"] = "Both 1",
            ["BJ"] = "Male 1",
            ["eat_out"] = "Female 1",
            ["get_eaten"] = "Both 2",
            ["sex"] = "Both 2"
        };

        // Debugging
        public bool Debug = false;
        public bool Verbose = false;

        // Itemtype Constants
        public readonly int MilkType = -34;
        public readonly int CumType = -35;
        public readonly int SpecialType = -36;

        public SButton MilkButton = SButton.O;

        // Extra Dialogue
        //public readonly string FarmerCollectCum = "You free your penis and start using one hand to gently stroke it, and the other to massage your balls. " +
        //"You close your eyes as you start to imagine someone leaning down and engulfing your cock between their lips. " +
        //"#You pick up your pace as their imaginary mouth starts sucking harder, occasionally flicking your tip with their tongue. " +
        //"Your clench your eyes tighter as your cum starts to spurt out, painting the inside of your jar. You seal the lid with a contented sigh. ";

        //public readonly string FarmerCollectionMilk = "You glance around for a mo to see if anyone is watching you, then slip your top over your head. " +
        //"You start massaging you breast, and your nipples are soon standing proud, calling for attention. " +
        //"#As you start kneading in earnest, a trickle quickly emerges from the sides of your nipple, and quickly becomes a stream. " +
        //"The flow increases as you find your rhythm, and the drip-drip of it collecting in your bottle is hypnotising. " +
        //"#After several minutes of switching between breasts you are much emptier, and you pull your top back on. " +
        //"The fabric rubs against your slightly sore nipples.";
    }

    public static class TempRefs
    {
        // Universal access
        public static IModHelper Helper;
        public static IMonitor Monitor;
        public static SButton ActionKey = SButton.O;
        public static string ModItemPrefix = "Trunip190.CP.MilkTheVillagers.";

        public static List<NPC> milkedtoday = new();
        public static List<NPC> SexToday = new();
        public static bool SelfMilkedToday = false;
        public static bool SelfCummedToday = false;
        public static int InvitationsSent = 0;

        // Config options
        public static bool loaded = false;
        public static bool thirdParty = true;
        public static bool Verbose = false;
        public static bool MilkMale = true;
        public static bool MilkFemale = true;

        #region Genitals
        public static bool OverrideGenitals = false;
        public static string FarmerGender = "Female";
        public static string FarmerGenitals = "penis";
        public static bool HasPenis => FarmerGenitals.ToLower().Contains("penis");
        public static bool HasVagina => FarmerGenitals.ToLower().Contains("vagina");
        public static bool HasBreasts => FarmerGenitals.ToLower().Contains("breasts");
        public static bool AceCharacter => (FarmerGender == "A-sexual");
        #endregion

        public static bool IgnoreVillagerGender = false;

        #region Event ID storage
        #region Abigail 5948 0X
        public static int EventAbi01 = 594801;
        public static int EventAbi02 = 594802;
        public static int EventAbi03 = 594803;
        public static int EventAbi04 = 594804;
        #endregion

        #region Elliott 5948 1X
        public static int EventElliott01 = 594811; // unused random BJ
        public static int EventElliott02 = 594812; // challenging sex repression in book
        public static int EventElliott03 = 594813; // unused Skeleton slaying
        public static int EventElliott04 = 594814; // placeholder (unwritten) - Roleplay interrogation
        #endregion

        #region Sebastian 5948 2X
        public static int EventSeb01 = 594821;
        public static int EventSeb02 = 594822;
        public static int EventSeb03 = 594823;
        public static int EventSeb04 = 594824;
        #endregion

        #region Maru 5948 3X
        public static int EventMaru01 = 594831;
        public static int EventMaru02 = 594832;
        public static int EventMaru03 = 594833;
        public static int EventMaru04 = 594834;
        #endregion

        #region Emily 5948 4X
        public static int EventEmily01 = 594841;
        public static int EventEmily02 = 594842;
        public static int EventEmily03 = 594843;
        public static int EventEmily04 = 594844;
        #endregion

        #region Haley 5948 5X
        public static int EventHaley01 = 594851;
        public static int EventHaley02 = 594852;
        public static int EventHaley03 = 594853;
        public static int EventHaley04 = 594854;
        #endregion

        #region Penny 5948 6X
        public static int EventPenny01 = 594861;
        public static int EventPenny02 = 594862;
        public static int EventPenny03 = 594863;
        public static int EventPenny04 = 594864;
        #endregion

        #region Leah 5948 7X
        public static int EventLeah01 = 594865;
        public static int EventLeah02 = 594866;
        public static int EventLeah03 = 594867;
        public static int EventLeah04 = 594868;
        #endregion

        public static int EventHarvey = 594802;         //Harvey
        public static int Event3HarMar = 594803;        //Harvey/Maru
        public static int EventLeah = 594804;           //Leah
        public static int EventPenny = 594805;          //Penny
        public static int Event3HaleyAlex = 594806;     //Haley/Alex
        public static int EventCaroline = 594807;       //Caroline
        public static int EventEmily = 594809;          //Emily
        public static int EventLeahExhibitOld = 5948101;
        public static int EventLeahExhibitA = 5948101;  //Leah Exhibitionism Ace, Need to switch to 5948101
        public static int EventLeahExhibitV = 5948102;  //Leah Exhibitionism Vagina. Need to switch to 5948102

        public static int UnusedEvent = 594811;

        public static int EventBathHouse = 594863;      //BathHouse Scene
        public static int EventBathHouseRepeat = 594864;//BathHouse Scene
        public static int EventBlairKeahi1NA = 594815;  //Blair and farmer hide in bushes
        public static int EventElliottScene1 = 594816;  //Elliott Q4, Quest 2 roleplay event
        public static int EventSebTouchGrass = 594817;  //Seb Q3, Sebastian go touch grass event (quest 3)
        public static int EventSebAbi = 594818;         //Seb Q4, Abigail, Sebastian, farmer threeway stream.
        //public static int Event                       //
        #endregion

        #region Items
        // Milk item code storage
        //public static int MilkAbig = 803;
        //public static int MilkEmil = 803;
        //public static int MilkHale = 803;
        //public static int MilkLeah = 803;
        //public static int MilkMaru = 803;
        //public static int MilkPenn = 803;
        //public static int MilkCaro = 803;
        //public static int MilkJodi = 803;
        //public static int MilkMarn = 803;
        //public static int MilkRobi = 803;
        //public static int MilkPam = 803;
        //public static int MilkSand = 803;
        //public static int MilkEvel = 803;

        /// <summary>
        /// Generic Breast milk
        /// </summary>
        //public static int MilkGeneric = 803;

        // Cum item code storage.
        /// <summary>
        /// Generic cum
        /// </summary>
        //public static int MilkSpecial = 803;
        //public static int MilkAlex = 803;
        //public static int MilkClint = 803;
        //public static int MilkDemetrius = 803;
        //public static int MilkElliott = 803;
        //public static int MilkGeorge = 803;
        //public static int MilkGil = 803;
        //public static int MilkGunther = 803;
        //public static int MilkGus = 803;
        //public static int MilkHarv = 803;
        //public static int MilkKent = 803;
        //public static int MilkLewis = 803;
        //public static int MilkLinus = 803;
        //public static int MilkMarlon = 803;
        //public static int MilkMorris = 803;
        //public static int MilkPierre = 803;
        //public static int MilkSam = 803;
        //public static int MilkSeb = 803;
        //public static int MilkShane = 803;
        //public static int MilkWilly = 803;

        // Magical
        //public static int MilkDwarf = 803;
        //public static int MilkKrobus = 803;
        //public static int MilkQi = 803;
        //public static int MilkWiz = 803;
        //public static int MilkMagic = 803;

        // Recipe item code storage
        //public static int ProteinShake = 1240;
        //public static int MilkShake = 1241;
        //public static int SuperJuice = 1249;
        //public static int EldritchEnergy = 1250;
        //public static int MartiniKairos = 1251;
        //public static int SweetSibling = 1252;

        // Other mods
        //public static int MilkSophia = 803;
        //public static int MilkOlivia = 803;
        //public static int MilkSusan = 803;
        //public static int MilkClaire = 803;
        //public static int MilkScarlett = 803;
        //public static int MilkAndy = 803;
        //public static int MilkVictor = 803;
        //public static int MilkMartin = 803;

        // Item types 
        //public const int MilkType = -34;
        //public const int CumType = -35;
        //public const int SpecialType = -36;

        // Quest items
        //public static int PennyBook = 804;
        //public static int HaleyCamera = 804;
        //public static int HaleyPanties = 804;
        //public static int ReadiMilk = 804;
        //public static int Invitation = 804;

        // Clothing Items
        //public static int mtvTeddyl = 806;
        //public static int mtvTeddy = 806;
        //public static int TentacleTop = 806;
        //public static int TentacleLeg = 806;
        #endregion

        public static void ReportCodes()
        {
            log.Log("ReportCodes() does nothing now", LogLevel.Trace);
            // Milk item code storage
            //Log($"MilkAbig is {MilkAbig}", LogLevel.Trace);
            //Log($"MilkEmil is {MilkEmil}", LogLevel.Trace);
            //Log($"MilkHale is {MilkHale}", LogLevel.Trace);
            //Log($"MilkLeah is {MilkLeah}", LogLevel.Trace);
            //Log($"MilkMaru is {MilkMaru}", LogLevel.Trace);
            //Log($"MilkPenn is {MilkPenn}", LogLevel.Trace);
            //Log($"MilkCaro is {MilkCaro}", LogLevel.Trace);
            //Log($"MilkJodi is {MilkJodi}", LogLevel.Trace);
            //Log($"MilkMarn is {MilkMarn}", LogLevel.Trace);
            //Log($"MilkRobi is {MilkRobi}", LogLevel.Trace);
            //Log($"MilkPam is {MilkPam}", LogLevel.Trace);
            //Log($"MilkSand is {MilkSand}", LogLevel.Trace);
            //Log($"MilkEvel is {MilkEvel}", LogLevel.Trace);
            //Log($"MilkDwarf is {MilkDwarf}", LogLevel.Trace);
            //Log($"MilkGeneric is {MilkGeneric}", LogLevel.Trace);

            // Cum item code storage.
            //Log($"MilkSpecial is {MilkSpecial}", LogLevel.Trace);
            //Log($"MilkAlex is {MilkAlex}", LogLevel.Trace);
            //Log($"MilkClint is {MilkClint}", LogLevel.Trace);
            //Log($"MilkDemetrius is {MilkDemetrius}", LogLevel.Trace);
            //Log($"MilkElliott is {MilkElliott}", LogLevel.Trace);
            //Log($"MilkGeorge is {MilkGeorge}", LogLevel.Trace);
            //Log($"MilkGil is {MilkGil}", LogLevel.Trace);
            //Log($"MilkGunther is {MilkGunther}", LogLevel.Trace);
            //Log($"MilkGus is {MilkGus}", LogLevel.Trace);
            //Log($"MilkHarv is {MilkHarv}", LogLevel.Trace);
            //Log($"MilkKent is {MilkKent}", LogLevel.Trace);
            //Log($"MilkLewis is {MilkLewis}", LogLevel.Trace);
            //Log($"MilkLinus is {MilkLinus}", LogLevel.Trace);
            //Log($"MilkMarlon is {MilkMarlon}", LogLevel.Trace);
            //Log($"MilkMorris is {MilkMorris}", LogLevel.Trace);
            //Log($"MilkQi is {MilkQi}", LogLevel.Trace);
            //Log($"MilkPierre is {MilkPierre}", LogLevel.Trace);
            //Log($"MilkSam is {MilkSam}", LogLevel.Trace);
            //Log($"MilkSeb is {MilkSeb}", LogLevel.Trace);
            //Log($"MilkShane is {MilkShane}", LogLevel.Trace);
            //Log($"MilkWilly is {MilkWilly}", LogLevel.Trace);
            //Log($"MilkWiz is {MilkWiz}", LogLevel.Trace);
            //Log($"MilkMarlon is {MilkMarlon}", LogLevel.Trace);
            //Log($"MilkKrobus is {MilkKrobus}", LogLevel.Trace);

            // Other Mods
            //Log($"MilkSophia is {MilkSophia}", LogLevel.Trace);
            //Log($"MilkOlivia is {MilkOlivia}", LogLevel.Trace);
            //Log($"MilkSusan is {MilkSusan}", LogLevel.Trace);
            //Log($"MilkClaire is {MilkClaire}", LogLevel.Trace);
            //Log($"MilkAndy is {MilkAndy}", LogLevel.Trace);
            //Log($"MilkVictor is {MilkVictor}", LogLevel.Trace);
            //Log($"MilkMartin is {MilkMartin}", LogLevel.Trace);

            // Recipe item code storage
            //Log($"ProteinShake is {ProteinShake}", LogLevel.Trace);
            //Log($"MilkShake is {MilkShake}", LogLevel.Trace);

            // Item types
            //Log($"MilkType is {MilkType}", LogLevel.Trace);
            //Log($"CumType is {CumType}", LogLevel.Trace);

            // Quest items
            //Log($"PennyBook is {PennyBook}", LogLevel.Trace);
            //Log($"HaleyCamera is {HaleyCamera}", LogLevel.Trace);
            //Log($"HaleyPanties is {HaleyPanties}", LogLevel.Trace);
            //Log($"ReadiMilk is {ReadiMilk}", LogLevel.Trace);
        }


    }
}
