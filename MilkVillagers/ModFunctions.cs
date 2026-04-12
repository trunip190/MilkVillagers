using MailFrameworkMod.ContentPack;
using Microsoft.Xna.Framework;
using SpaceCore;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Characters;
using StardewValley.Network;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Xml;
using sObject = StardewValley.Object;

namespace MilkVillagers
{
    public static class ModFunctions
    {
        public static Dictionary<string, string> FirstMail = new()
        {
            ["Abigail"] = "MTV_AbigailQ1",      //Abigail quest 1
            ["Maru"] = "MTV_MaruQ1",            //Maru quest 1
            ["George"] = "MTV_GeorgeQ1",        //George quest 1
            ["Harvey"] = "MTV_HarveyQ1",        //Harvey quest 1
            ["Elliott"] = "MTV_ElliottQ1",      //Elliott quest 1
            ["Emily"] = "MTV_EmilyQ1",          //Emily quest 1
            ["Haley"] = "MTV_HaleyQ1",          //Haley quest 1
            ["Penny"] = "MTV_PennyQ1",          //Penny quest 1
            ["Leah"] = "MTV_LeahQ1",            //Leah quest 1
            ["Sebastian"] = "MTV_SebQ1",        //Sebastian quest 1
            ["Shane"] = "MTV_ShaneQ1",          //Shane quest 1
        };
        public static Dictionary<Skills.Skill, KeyValuePair<int, string>> recipeRequirement = new()
        {
            [ModEntry.Instance.MSskill] = new KeyValuePair<int, string>(1, "Trunip190.CP.MilkTheVillagers.CookiesnCream"),
        };
        public static int StringToInt(string s) => int.TryParse(s, out int result) ? result : 0;

        public static List<string> topics = new()
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

        public static List<string> chars = new() {
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
        public static List<MailCheck> MailChecks = new() {
            new MailCheck ("MTV_AbigailQ1T", "MTV_AbigailQ2", "Abigail", 6), // Abi Milk Quest 2
            new MailCheck ("MTV_AbigailQ2T", "MTV_AbigailQ3", "Abigail", 7), // Abi Milk Quest 3
            new MailCheck ("MTV_AbigailQ3T", "MTV_AbigailQ4", "Abigail", 8), // Abi Milk Quest 4
            new MailCheck("MTV_ElliottQ1T", "MTV_ElliottQ2", "Elliott", 6),         // Elliott Quest 2
            new MailCheck("MTV_ElliottQ2T", "MTV_ElliottQ3", "Elliott", 7),         // Elliott Quest 3
            new MailCheck("MTV_ElliottQ3T", "MTV_ElliottQ4", "Elliott", 8),        // Elliott Quest 4
            new MailCheck("MTV_SebQ1T", "MTV_SebQ2", "Sebastian", 6),   //Sebastian Quest 2
            new MailCheck("MTV_SebQ2T", "MTV_SebQ3", "Sebastian", 7), //Sebastian Quest 3
            new MailCheck("MTV_SebQ3T", "MTV_SebQ4", "Sebastian", 8), //Sebastian Quest 4
            new MailCheck("MTV_MaruQ1T", "MTV_MaruQ2", "Maru", 6), // Maru Quest 2
            new MailCheck("MTV_MaruQ2T", "MTV_MaruQ3", "Maru", 7), // Maru Quest 3
            new MailCheck("MTV_MaruQ3T", "MTV_MaruQ4", "Maru", 8), // Maru Quest 4
            new MailCheck("MTV_EmilyQ1T", "MTV_EmilyQ2", "Emily", 6), //Emily Quest 2
            //new MailCheck("MTV_EmilyQ2T", "MTV_EmilyQ3", "Emily", 7), //Emily Quest 3 not written yet
            //new MailCheck("MTV_EmilyQ3T", "MTV_EmilyQ4", "Emily", 8), //Emily Quest 4
            new MailCheck("MTV_HarveyQ1T", "MTV_HarveyQ2", "Harvey", 6), // Harvey Quest 2
            new MailCheck("MTV_HarveyQ2T", "MTV_HarveyQ3", "Harvey", 7), // Harvey Quest 3
            new MailCheck("MTV_HarveyQ3T", "MTV_HarveyQ4", "Harvey", 8), // Harvey Quest 4
            new MailCheck("MTV_PennyQ1T", "MTV_PennyQ2", "Penny", 6), //Penny Quest 2
            new MailCheck("MTV_PennyQ2T", "MTV_PennyQ3", "Penny", 7), //Penny Quest 3
            new MailCheck("MTV_PennyQ3T", "MTV_PennyQ4", "Penny", 8), //Penny Quest 4
            new MailCheck("MTV_GeorgeQ1T", "MTV_GeorgeQ2", "George", 6), // George Quest 2
            //new MailCheck("MTV_GeorgeQ2T", "MTV_GeorgeQ3", "George", 7), // George Quest 3
            new MailCheck("MTV_GeorgeQ3T", "MTV_GeorgeQ4", "George", 8), // George Quest 4
            new MailCheck("MTV_HaleyQ1T", "MTV_HaleyQ2", "Haley", 6), //Haley Quest 2
            new MailCheck("MTV_HaleyQ2T", "MTV_HaleyQ3", "Haley", 7), //Haley Quest 3
            new MailCheck("MTV_HaleyQ3T", "MTV_HaleyQ4", "Haley", 8), //Haley Quest 4
            new MailCheck("MTV_LeahQ1T", "MTV_LeahQ2", "Leah", 6), //Leah Quest 2
            new MailCheck("MTV_LeahQ2T", "MTV_LeahQ3", "Leah", 7), //Leah Quest 3
            //new MailCheck("MTV_LeahQ3T", "MTV_LeahQ4", "Leah", 8), //Leah Quest 4 - Not written yet
            new MailCheck("MTV_ShaneQ1T", "MTV_ShaneQ2", "Shane", 6), //Leah Quest 2
            new MailCheck("MTV_ShaneQ2T", "MTV_ShaneQ3", "Shane", 7), //Leah Quest 3
            new MailCheck("MTV_ShaneQ3T", "MTV_ShaneQ4", "Shane", 8), //Leah Quest 4 - Not written yet
        };

        /// <summary>
        /// Which mail item to send when a quest is completed.
        /// </summary>
        public static Dictionary<int, string> QuestMail = new Dictionary<int, string>()
        {
            [594801] = "MTV_AbigailQ1T",    //Abigail quest 1
            [594802] = "MTV_AbigailQ2T",    //Abigail quest 2
            [594803] = "MTV_AbigailQ3T",    //Abigail quest 3
            //[594804] = "MTV_AbigailQ4T",  //Abigail quest 4

            [594805] = "MTV_ElliottQ1T",    //Elliott quest 1
            [594806] = "MTV_ElliottQ2T",    //Elliott quest 2
            [594807] = "MTV_ElliottQ3P",    //Elliott quest 3
            [5948072] = "MTV_ElliottQ3T",   //Elliott quest 3
            [594808] = "MTV_ElliottQ4T",    //Elliott quest 4

            //[594809] = "",                //Sebastian quest 1 pt 1
            [5948092] = "MTV_SebQ1T",       //Sebastian quest 1 pt 2
            //[594810] = "",                //Sebastian quest 2 pt 1
            [5948102] = "MTV_SebQ2T",       //Sebastian quest 2 pt 2
            [594811] = "MTV_SebQ3T",        //Sebastian quest 3
            [594812] = "MTV_SebQ4T",        //Sebastian quest 4

            [594813] = "MTV_MaruQ1T",       //Maru quest 1
            [594814] = "MTV_MaruQ2T",       //Maru quest 2
            [594815] = "MTV_MaruQ3T",       //Maru quest 3
            [594816] = "MTV_MaruQ4T",       //Maru quest 4

            [594817] = "MTV_EmilyQ1T",      //Emily quest 1
            [594818] = "MTV_EmilyQ2T",      //Emily quest 2
            [594819] = "MTV_EmilyQ3T",      //Emily quest 3
            [594820] = "MTV_EmilyQ4T",      //Emily quest 4

            [594821] = "MTV_HaleyQ1T",      //Haley quest 1
            [594822] = "MTV_HaleyQ2T",      //Haley quest 2
            [594823] = "MTV_HaleyQ3T",      //Haley quest 3
            [594824] = "MTV_HaleyQ4-2",     //Haley quest 4
            //[5948242] = "",

            [5948252] = "MTV_PennyQ1T",     //Penny quest 1
            [594826] = "MTV_PennyQ2T",      //Penny quest 2
            [5948272] = "MTV_PennyQ3T",     //Penny quest 3
            [594828] = "MTV_PennyQ4T",      //Penny quest 4

            [594829] = "MTV_LeahQ1T",       //Leah quest 1
            [594830] = "MTV_LeahQ2T",       //Leah quest 2
            [594831] = "MTV_LeahQ3T",       //Leah quest 3
            [594832] = "MTV_LeahQ4T",       //Leah quest 4

            [594833] = "MTV_GeorgeQ1T",     //George quest 1
            [594834] = "MTV_GeorgeQ2T",     //George quest 2
            [594835] = "MTV_GeorgeQ3T",     //George quest 3
            //[594836] = "",                //George quest 4 pt 1
            [5948362] = "MTV_GeorgeQ4T",    //George quest 4 pt 2

            //[594837] = "MTV_HarveyQ1T",   //Harvey quest 1
            [5948372] = "MTV_HarveyQ1T",    //Harvey quest 1
            [594838] = "MTV_HarveyQ2T",     //Harvey quest 2
            [594839] = "MTV_HarveyQ3T",     //Harvey quest 3
            [594840] = "MTV_HarveyQ4T",     //Harvey quest 4

            [594841] = "MTV_ShaneQ1T",      //Shane quest 1
            [594842] = "MTV_ShaneQ2T",      //Shane quest 2
            [594843] = "MTV_ShaneQ3T",      //Shane quest 3
            [594844] = "MTV_ShaneQ4T",      //Shane quest 4
        };

        /// <summary>
        /// questid's from quest names. need to make these explicit references in the code.
        /// </summary>
        public static Dictionary<string, int> QuestIDs = new Dictionary<string, int>()
        {
            ["QuestAbi1"] = 594801,
            ["QuestAbi2"] = 594802,
            ["QuestAbi3"] = 594803,
            ["QuestAbi4"] = 594804,

            ["QuestElliott1"] = 594805,
            ["QuestElliott2"] = 594806,
            ["QuestElliott3"] = 594807,
            ["QuestElliott4"] = 594808,

            ["QuestSeb1"] = 594809,
            ["QuestSeb1-2"] = 5948092,
            ["QuestSeb2"] = 594810,
            ["QuestSeb2-2"] = 5948102,
            ["QuestSeb3"] = 594811,
            ["QuestSeb4"] = 594812,

            ["QuestMaru1"] = 594813,
            ["QuestMaru1-2"] = 5948132,
            ["QuestMaru2"] = 594814,
            ["QuestMaru2-2"] = 5948142,
            ["QuestMaru3"] = 594815,
            ["QuestMaru4"] = 594816,

            ["QuestEmily1"] = 594817,
            ["QuestEmily2"] = 594818,
            ["QuestEmily3"] = 594819,
            ["QuestEmily4"] = 594820,

            ["QuestHaley1"] = 594821,
            ["QuestHaley2"] = 594822,
            ["QuestHaley2-2"] = 5948222,
            ["QuestHaley3"] = 594823,
            ["QuestHaley4"] = 594824,
            ["QuestHaley4-2"] = 5948242,

            ["QuestPenny1"] = 594825,
            ["QuestPenny1-2"] = 5948252,
            ["QuestPenny2"] = 594826,
            ["QuestPenny3"] = 594827,
            ["QuestPenny4"] = 594828,

            ["QuestLeah1"] = 594829,
            ["QuestLeah2"] = 594830,
            ["QuestLeah3"] = 594831,
            ["QuestLeah4"] = 594832,

            ["QuestGeorge1"] = 594833,
            ["QuestGeorge2"] = 594834,
            ["QuestGeorge3"] = 594835,
            ["QuestGeorge4"] = 594836,
            ["QuestGeorge4-2"] = 5948362,

            ["QuestHarvey1"] = 594837,
            ["QuestHarvey1-2"] = 5948372,
            ["QuestHarvey2"] = 594838,
            ["QuestHarvey2-2"] = 5948382,
            ["QuestHarvey3"] = 594839,
            ["QuestHarvey4"] = 594840,

            ["QuestShane1"] = 594841,
            ["QuestShane2"] = 594842,
            ["QuestShane3"] = 594843,
            ["QuestShane4"] = 594844,
        };


        public static void Log(string message, LogLevel Level = LogLevel.Trace, bool Force = false)
        {
            if (message == null || TempRefs.Monitor == null) return;

            if (TempRefs.Verbose || Force) TempRefs.Monitor.Log(message, Level);
        }

        public static bool LookingAtNPC(Microsoft.Xna.Framework.Vector2 target, Microsoft.Xna.Framework.Vector2 NPC) =>
            target.X == NPC.X && target.Y == NPC.Y;

        public static Farmer FindFarmer(GameLocation location, Microsoft.Xna.Framework.Vector2 target, Microsoft.Xna.Framework.Vector2 farmerPos) =>
            location.farmers.First(o => LookingAtNPC(target, o.GetGrabTile()) || LookingAtNPC(farmerPos, o.GetGrabTile()));

        public static void NewHud(string Key = "", string backupmessage = "") =>
            Game1.addHUDMessage(new HUDMessage(Key != "" ? ModEntry.Instance.i18n.Get(Key) : backupmessage != "" ? backupmessage : ""));

        public static NPC FindTarget(GameLocation location, Microsoft.Xna.Framework.Vector2 target, Microsoft.Xna.Framework.Vector2 farmerPos)
        {
            int[] NPC = new int[2];

            using List<NPC>.Enumerator enumerator = location.characters.GetEnumerator();
            while (enumerator.MoveNext())
            {
                NPC current = enumerator.Current;
                if (LookingAtNPC(target, current.GetGrabTile()) || LookingAtNPC(farmerPos, current.GetGrabTile()))
                    return current;
            }
            return null;
        }

        public static Dialogue TrimDialogue(Dialogue input)
        {
            Dialogue output = new(input);

            foreach (var v in output.dialogues)
            {
                while ((v.Text.Contains('[') && v.Text.Contains(']')) || (v.Text.Contains('{') && v.Text.Contains('}')))
                {
                    int first = 0;
                    int last = 0;
                    string dumpText = "";
                    if (v.Text.Contains('[') && v.Text.Contains(']'))
                    {
                        first = v.Text.IndexOf('[');
                        last = v.Text.IndexOf(']', first) + 1;
                        dumpText = v.Text[first..last];
                        v.Text = v.Text.Replace(dumpText, "");
                    }

                    if (v.Text.Contains('{') && v.Text.Contains('}'))
                    {
                        first = v.Text.IndexOf('{');
                        last = v.Text.IndexOf('}', first) + 1;
                        dumpText = v.Text[first..last];
                        v.Text = v.Text.Replace(dumpText, "");
                    }
                    v.Text = v.Text.Trim();
                }
            }

            return output;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="DialogueKey">Key to pull. eg 'milk_fast_fem'</param>
        /// <returns></returns>
        public static Response ProcessOption(string DialogueKey)
        {
            Dialogue output = new Dialogue(null, $"Strings/StringsFromCSFiles:option.{DialogueKey}");

            Response newResponse = new Response(DialogueKey, output.dialogues[0].Text);

            return newResponse;
        }

        public static string[] SplitStrings(string str)
        {
            string[] result = new string[] { };

            int ItemIndex = str.IndexOf('[');
            string DialogueAdjusted = str;
            string ItemCodes = "";
            if (ItemIndex != -1)
            {
                ItemCodes = str.Substring(ItemIndex, str.Length - ItemIndex);
                DialogueAdjusted = str.Substring(0, ItemIndex);
            }

            result = DialogueAdjusted.Split(new string[] { "#split#" }, System.StringSplitOptions.None);

            for (int i = 0; i < result.Length; i++)
            {
                result[i] += ItemCodes;
            }

            return result;
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
        public static Farmer FindTargetFarmer(GameLocation location, Microsoft.Xna.Framework.Vector2 target, Microsoft.Xna.Framework.Vector2 farmerpos)
        {
            foreach (Farmer who in Game1.getOnlineFarmers())
            {
                if (location == who.currentLocation && (LookingAtNPC(target, who.GetGrabTile()) || LookingAtNPC(farmerpos, who.GetGrabTile())))
                    return who;
            }
            return null;
        }
    }

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
        public string FarmerGenderMod => FarmerGender == "Save default" ? _faarmerSavefileGender : FarmerGender;
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
        /// <summary>
        /// Female, Male, Herm, Ace
        /// </summary>
        public static string FarmerGender = "Female";
        public static string FarmerGenitals = "penis";
        public static bool HasPenis => FarmerGenitals.ToLower().Contains("penis");
        public static bool HasVagina => FarmerGenitals.ToLower().Contains("vagina");
        public static bool HasBreasts => FarmerGenitals.ToLower().Contains("breasts");
        public static bool AceCharacter => (FarmerGender == "A-sexual" || FarmerGender == "Ace");
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

    }

}
