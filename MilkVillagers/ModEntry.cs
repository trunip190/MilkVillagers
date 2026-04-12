using HarmonyLib;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Netcode;
using SpaceCore;
using SpaceCore.Events;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Extensions;
using StardewValley.GameData.Characters;
using StardewValley.GameData.Objects;
using StardewValley.Network;
using StardewValley.Objects;
using StardewValley.Quests;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Framework = Microsoft.Xna.Framework;
using IGenericModConfigMenuApi = GenericModConfigMenu.IGenericModConfigMenuApi;
using Log = MilkVillagers.ModFunctions;
using sObject = StardewValley.Object;

namespace MilkVillagers
{
    public class ModEntry : Mod
    {
        #region class variables
        private Vector2 target;
        private bool loaded;
        private bool running;
        private bool runOnce;
        private bool MessageOnce;
        private NPC currentTarget;
        //private List<NPC> NPCsInRange;
        ContentPatcher.IContentPatcherAPI contentPatcherApi;
        internal static IMobilePhoneApi MobilePhoneApi;
        internal ITranslationHelper i18n;
        internal static ModEntry Instance;
        internal MilkingSkill MSskill;
        const string SkillID = "trunip190.MTV.skill";
        static readonly string[] Magical = new[] { "Dwarf", "Wizard", "Mister Qi", "Krobus" };
        static readonly string[] GenderMail = new[] { "MTV_Vagina", "MTV_Penis", "MTV_Ace", "MTV_Herm" };

        public Dictionary<string, int> LoveRequirement = new()
        {
            {"milk_start" , 8},
            {"milk_fast" , 8},
            {"BJ" , 6},
            {"eat_out" , 6},
            {"get_eaten" , 7},
            {"sex" , 10 },
        };
        public Dictionary<string, int> SexCost = new()
        {
            {"milk_start" , 20},
            {"milk_fast" , 20},
            {"BJ" , 20},
            {"eat_out" , 30},
            {"get_eaten" , 15},
            {"sex" , 50 }
        };
        public Dictionary<string, int> SexLove = new()
        {
            {"milk_start" , 20},
            {"milk_fast" , 20},
            {"BJ" , 20},
            {"eat_out" , 30},
            {"get_eaten" , 15},
            {"sex" , 50 },
        };
        public Dictionary<string, string> SexTopics = new();
        public List<string> BukkakePartyNPCs = new() {
            "Clint",
            "Elliott",
            "Gunther",
            "Gus",
            "Harvey",
            "Lewis",
            "Morris",
            "Pierre",
            "Sam",
            "Sebastian",
            "Shane",
        };

        private bool doneOnce = false; //remove when not testing.
        internal static int QuestId(Quest input) => int.Parse(input.id.Value);
        internal static Dictionary<string, KeyValuePair<string, ObjectData>> ModItems => DataLoader.Objects(Game1.content).Where(o => o.Key.Contains("Trunip190.CP.MilkTheVillagers")).ToDictionary(keySelector: o => o.Key);
        internal static Dictionary<string, KeyValuePair<string, string>> ModMail => DataLoader.Mail(Game1.content).Where(o => o.Key.Contains("MTV")).ToDictionary(keySelector: o => o.Key);

        // Time based stuff
        private float RegenAmount = 0;
        internal static string IncrementString(string raw) => int.TryParse(raw, out int number) ? $"{number++}" : $"{raw}";

        private float TimeFreezeTimer = 0;
        private bool TimeFreeze = false; // Debug time freeze.

        // Adding item stuff
        sObject AddItem;
        Farmer Whom => Game1.player;
        public static bool GetVagina(Farmer who) => TempRefs.OverrideGenitals ? TempRefs.HasVagina : !who.IsMale;
        public static bool GetPenis(Farmer who) => TempRefs.OverrideGenitals ? TempRefs.HasPenis : who.IsMale;
        public static bool GetBreasts(Farmer who) => TempRefs.OverrideGenitals ? TempRefs.HasBreasts : !who.IsMale;

        private ModConfig Config;
        List<int> CurrentQuests = new();

        public event ActionNPC OnActionNPC;
        #endregion

        public override void Entry(IModHelper helper)
        {
            Log.Log("MTV: Entry", LogLevel.Debug);
            Instance = this;
            Config = helper.ReadConfig<ModConfig>();
            TempRefs.Monitor = Monitor;
            i18n = Helper.Translation;
            MSskill = new(SkillID);

            #region Harmony setup
            var harmony = new Harmony(this.ModManifest.UniqueID);

            try
            {
                ItemPatches.ApplyPatch(harmony, this.Monitor);
            }
            catch (Exception ex)
            {
                Log.Log(ex.Message, LogLevel.Alert, Force: true);
            }
            #endregion

                if (helper == null)
                {
                    Log.Log("helper is null.", LogLevel.Error);
                }
                else
                {
                    TempRefs.Helper = helper;
                }

            Dictionary<string, string> data = Game1.content.Load<Dictionary<string, string>>("Data\\Events\\" + "Forest");

            #region register in-game events

            Helper.Events.Display.MenuChanged += Display_MenuChanged;
            Helper.Events.GameLoop.DayStarted += GameLoop_DayStarted;
            Helper.Events.GameLoop.GameLaunched += OnGameLaunched;
            Helper.Events.GameLoop.SaveLoaded += GameLoop_SaveLoaded;
            Helper.Events.GameLoop.DayEnding += GameLoop_DayEnding;
            Helper.Events.GameLoop.OneSecondUpdateTicked += GameLoop_OneSecondUpdateTicked;
            Helper.Events.Input.ButtonPressed += OnButtonPressed;
            //Helper.Events.Player.Warped += ChangedLocation;

            // SpaceCore events
            SpaceEvents.BeforeGiftGiven += SpaceEvents_BeforeGiftGiven;
            SpaceEvents.OnItemEaten += SpaceEvents_OnItemEaten;

            // My events
            this.OnActionNPC += CheckQuestActionOnNPC;

            #endregion

            #region Add in console commands
            helper.ConsoleCommands.Add("mtv_addquests", "Adds all quests to the farmer's questlog\n\tUsage: mtv_addquests <completed> <list>\n\t- completed: auto complete quests as they are added.\n\t- list: prints every vilager changed.", this.AddAllQuests);
            helper.ConsoleCommands.Add("mtv_checkfiles", "checks your computer to see if the modfiles are in the right location\n\nUsage: mtv_checkfiles <all>\n\t- all: Checks all files in folders recursively. Only checks JA folder by default.", this.FileCheck);
            helper.ConsoleCommands.Add("mtv_friends", "Sets all vanilla NPC's friends to either half or max\n\nmtv_friends <max>\n\t- max: set to 10 hearts. Default is 6 hearts.", this.Friends);
            helper.ConsoleCommands.Add("mtv_forceremovequest", "Remove specified quest from the farmer's questlog\n\nUsage: mtv_frq <value>\n- value: the quest id to remove from the questlog.", this.ForceRemoveQuest);
            helper.ConsoleCommands.Add("mtv_renewItems", "Update items in farmer's inventory to newer versions after upgrade.\n\nUsage: mtv_renewItems", this.RenewItems);
            helper.ConsoleCommands.Add("mtv_resetmilk", "Resets the list of NPC's that have been milked today, allowing you to collect from them again\n\nUsage: mtv_resetmilk", this.ResetMilk);
            helper.ConsoleCommands.Add("mtv_sendmail", "Send the next mail item if conditions are met\n\nUsage: mtv_sendmail <value>\n- value: the farmer to send mail to.", this.SendNewMail);
            helper.ConsoleCommands.Add("mtv_upgrade", "Force the mod to reset quest and mail flags when you have upgraded the mod\n\nUsage: mtv_upgrade", this.Upgrade);
            helper.ConsoleCommands.Add("mtv_levelup", "Gain 1 level in the milking skill\n\nUsage: mtv_levelup", this.LevelUp);
            #endregion
            Log.Log("MTV: Entry Done", LogLevel.Debug);
            Skills.RegisterSkill(MSskill = new MilkingSkill(SkillID));
        }

        #region Game OnEvent Triggers
        private void OnGameLaunched(object sender, GameLaunchedEventArgs e)
        {
            Log.Log("MTV: OnGameLaunched", LogLevel.Debug);
            UpdateConfig();

            #region Mobile Phone
            //MobilePhoneApi = Helper.ModRegistry.GetApi<IMobilePhoneApi>("aedenthorn.MobilePhone");
            //if (MobilePhoneApi != null)
            //{
            //    Texture2D appIcon = Helper.ModContent.Load<Texture2D>(Path.Combine("assets", "app_icon.png"));
            //    bool success = MobilePhoneApi.AddApp(Helper.ModRegistry.ModID, "Milk the village", MTV_App.MobileOpenApp, appIcon);
            //    MTV_App.Api = MobilePhoneApi;
            //    MTV_App.Helper = this.Helper;
            //    Monitor.Log($"loaded phone app successfully: {success}", LogLevel.Debug);
            //}
            #endregion

            contentPatcherApi = this.Helper.ModRegistry.GetApi<ContentPatcher.IContentPatcherAPI>("Pathoschild.ContentPatcher");
            contentPatcherApi.RegisterToken(this.ModManifest, "GenderSwitch", () =>
            {
                if (Game1.player.hasOrWillReceiveMail("MTV_Vagina")) return new[] { "V" };
                if (Game1.player.hasOrWillReceiveMail("MTV_Herm")) return new[] { "H" };
                if (Game1.player.hasOrWillReceiveMail("MTV_Ace")) return new[] { "A" };
                if (Game1.player.hasOrWillReceiveMail("MTV_Penis")) return new[] { "P" };

                return new[] { "P" };
            });

            // New style of editing assets.
            //Helper.Events.Content.AssetRequested += Content_AssetRequested;

            TempRefs.thirdParty = Config.ThirdParty;
            TempRefs.Verbose = Config.Verbose;
            TempRefs.OverrideGenitals = Config.OverrideGenitals;
            TempRefs.IgnoreVillagerGender = Config.IgnoreVillagerGender;
            Log.Log("MTV: OnGameLaunched Done", LogLevel.Debug);
        }

        private void UpdateConfig()
        {
            #region Generic Mod Config
            // get Generic Mod Config Menu's API (if it's installed)
            var configMenu = this.Helper.ModRegistry.GetApi<IGenericModConfigMenuApi>("spacechase0.GenericModConfigMenu");
            if (configMenu is null)
                return;

            // register mod
            configMenu.Register(
                mod: this.ModManifest,
                reset: () => this.Config = new ModConfig(),
                save: () => this.Helper.WriteConfig(this.Config)
            );

            // get Generic Mod Config Menu API (if it's installed)
            // add some config options
            #region basic mod options

            #region Set milking button
            configMenu.AddKeybind(
                fieldId: "Milking Button",
                mod: this.ModManifest,
                name: () => "Milking Button",
                tooltip: () => "Set button for milking",
                getValue: () => this.Config.MilkButton,
                setValue: var => this.Config.MilkButton = var
                );
            #endregion

            #region Set gender based milking
            configMenu.AddBoolOption(
                fieldId: "Milk Females",
                mod: this.ModManifest,
                name: () => "Milk Females",
                tooltip: () => "Lets you milk female characters if they like you enough",
                getValue: () => this.Config.MilkFemale,
                setValue: value => this.Config.MilkFemale = value
            );

            configMenu.AddBoolOption(
                fieldId: "Milk Males",
                mod: this.ModManifest,
                name: () => "Milk Males",
                tooltip: () => "Lets you milk male characters if they like you enough",
                getValue: () => this.Config.MilkMale,
                setValue: value => this.Config.MilkMale = value
            );
            #endregion

            #region Set whether items are collected
            configMenu.AddBoolOption(
                fieldId: "Collect Items?",
                mod: this.ModManifest,
                name: () => "Collect Items?",
                tooltip: () => "Collect items, or just sex?",
                getValue: () => this.Config.CollectItems,
                setValue: value => this.Config.CollectItems = value
            );
            #endregion

            #region Set whether generic items are used
            configMenu.AddBoolOption(
                fieldId: "Simple milk/cum",
                mod: this.ModManifest,
                name: () => "Simple milk/cum",
                tooltip: () => "Replace individual items with generic ones.",
                getValue: () => this.Config.StackMilk,
                setValue: value => this.Config.StackMilk = value
            );
            #endregion

            #region Whether to use Abigail's extra dialogue
            configMenu.AddBoolOption(
                fieldId: "ExtraDialogue",
                mod: this.ModManifest,
                name: () => "ExtraDialogue",
                tooltip: () => "Enable Abigail's everyday dialogue changes?",
                getValue: () => this.Config.ExtraDialogue,
                setValue: value => this.Config.ExtraDialogue = value
            );
            #endregion

            #region Enable quests
            configMenu.AddBoolOption(
                fieldId: "Quests",
                mod: this.ModManifest,
                name: () => "Quests",
                tooltip: () => "Enable quest content",
                getValue: () => this.Config.Quests,
                setValue: value => this.Config.Quests = value
            );
            #endregion

            #region Set lower sex option level requirement
            configMenu.AddNumberOption(
                min: 0,
                max: 10,
                interval: 1,
                fieldId: "HeartLevel1",
                mod: this.ModManifest,
                name: () => "HeartLevel1",
                tooltip: () => "Hearts required for basic sexual interactions",
                getValue: () => this.Config.HeartLevel1,
                setValue: value => Config.HeartLevel1 = value
                );
            #endregion

            #region Set upper sex option level requirement
            configMenu.AddNumberOption(
                min: 0,
                max: 10,
                interval: 1,
                fieldId: "HeartLevel2",
                mod: this.ModManifest,
                name: () => "HeartLevel2",
                tooltip: () => "Hearts required for all sexual interactions",
                getValue: () => this.Config.HeartLevel2,
                setValue: value => Config.HeartLevel2 = value
                );
            #endregion

            #region Send mail immediately, instead of next day (quest speed limiter)
            configMenu.AddBoolOption(
                fieldId: "Rush_Mail",
                mod: this.ModManifest,
                name: () => "Rush Mail",
                tooltip: () => "Send quest mail immediately (not next day)",
                getValue: () => this.Config.RushMail,
                setValue: value => this.Config.RushMail = value
            );
            #endregion

            #endregion

            ////////////////////////////////////////////////////////////
            //////////////////   Override Genitals   ///////////////////
            ////////////////////////////////////////////////////////////
            #region Override Genitals
            string[] genders = new[] { "Save default", "Male", "Female", "A-sexual" }; //, "Intersex", "Genderfluid" }; need to work this in.
            string[] genitals = new[] { "Penis", "Vagina and breasts", "Vagina", "Vagina and Penis", "Penis and breasts", "Penis, Vagina, Breasts", "Breasts", "None" };

            configMenu.AddBoolOption(
                fieldId: "Override genitals",
                mod: this.ModManifest,
                name: () => "Override genitals?",
                tooltip: () => "Do you want to override the genitals of your farmer?",
                getValue: () => this.Config.OverrideGenitals,
                setValue: value => this.Config.OverrideGenitals = value
            );

            configMenu.AddTextOption(
                fieldId: "Genitals",
                mod: this.ModManifest,
                name: () => "Genitals",
                tooltip: () => "Genital options for your farmer.",
                allowedValues: genitals,
                getValue: () => this.Config.FarmerGenitals,
                setValue: value => Config.FarmerGenitals = value
                );
            #endregion

            #region A-sexual character
            //configMenu.AddBoolOption(
            //    mod: this.ModManifest,
            //    name: () => "Ace Character",
            //    tooltip: () => "Do you want to play an A-Sexual Character? Ignores genitals.",
            //    getValue: () => this.Config.AceCharacter,
            //    setValue: value => this.Config.AceCharacter = value
            //    );
            #endregion

            #region TODO not current version Farmer genitals
            //api.AddTextOption(
            //    mod: this.ModManifest,
            //    name: () => "Example dropdown",
            //    getValue: () => this.Config.ExampleDropdown,
            //    setValue: value => this.Config.ExampleDropdown = value,
            //    allowedValues: new string[] { "choice A", "choice B", "choice C" }
            //);
            #endregion

            #region Farmer has Penis
            //configMenu.AddBoolOption(
            //    mod: this.ModManifest,
            //    name: () => "Farmer has a penis",
            //    tooltip: () => "Does the farmer have a penis? MUST select override as well",
            //    getValue: () => this.Config.HasPenis,
            //    setValue: value => this.Config.HasPenis = value
            //);
            #endregion

            #region Farmer has Vagina
            //configMenu.AddBoolOption(
            //    mod: this.ModManifest,
            //    name: () => "Farmer has a vagina",
            //    tooltip: () => "Does the farmer have a vagina? MUST select override as well",
            //    getValue: () => this.Config.HasVagina,
            //    setValue: value => this.Config.HasVagina = value
            //);
            #endregion

            #region Farmer has breasts
            //configMenu.AddBoolOption(
            //    mod: this.ModManifest,
            //    name: () => "Farmer has breasts",
            //    tooltip: () => "Does the farmer have breasts? MUST select override as well",
            //    getValue: () => this.Config.HasBreasts,
            //    setValue: value => this.Config.HasBreasts = value
            //);
            #endregion

            #region Ignore villager gender for farming
            configMenu.AddBoolOption(
                fieldId: "Ignore villager gender",
                mod: this.ModManifest,
                name: () => "Ignore villager gender",
                tooltip: () => "Do you want to ignore the gender of the villager when milking?",
                getValue: () => this.Config.IgnoreVillagerGender,
                setValue: value => this.Config.IgnoreVillagerGender = value
                );
            #endregion

            ////////////////////////////////////////////////////////////
            ///////////////// Developer options ////////////////////////
            ////////////////////////////////////////////////////////////
            #region Debug Mode
            configMenu.AddBoolOption(
                fieldId: "Debug mode",
                mod: this.ModManifest,
                name: () => "Debug mode",
                tooltip: () => "Enable debug mode content",
                getValue: () => this.Config.Debug,
                setValue: value => this.Config.Debug = value
            );
            #endregion

            #region Verbose Output
            configMenu.AddBoolOption(
                fieldId: "Verbose Dialogue",
                mod: this.ModManifest,
                name: () => "All Console Output",
                tooltip: () => "Enable verbose dialogue for tracking errors",
                getValue: () => this.Config.Verbose,
                setValue: value => this.Config.Verbose = value
            );
            #endregion

            configMenu.OnFieldChanged(this.ModManifest, UpdateConfig);
            #endregion

            UpdateHeartReq();

            return;

        }

        private void UpdateConfig(string arg1, object arg2) //Updates config when items changed.
        {
            switch (arg1)
            {
                case "Collect Items?": Config.CollectItems = (bool)arg2; break;
                case "Debug mode": Config.Debug = (bool)arg2; break;
                case "ExtraDialogue": Config.ExtraDialogue = ((bool)arg2); break;

                case "SaveFileGender": Game1.player.changeGender((bool)arg2); UpdateGenitalConfig(arg1, $"{arg2}"); SendGenitalMail(Game1.player); Log.Log($"{Game1.player.displayName} is now male? {Game1.player.IsMale}", LogLevel.Info, Force: true); break;
                case "Override genitals": Config.OverrideGenitals = (bool)arg2; UpdateGenitalConfig(arg1, $"{arg2}"); SendGenitalMail(Game1.player); break;
                case "Gender": Config.FarmerGender = (string)arg2; UpdateGenitalConfig(arg1, $"{arg2}"); SendGenitalMail(Game1.player); break;
                case "Genitals": Config.FarmerGenitals = (string)arg2; UpdateGenitalConfig(arg1, $"{arg2}"); SendGenitalMail(Game1.player); break;

                case "HeartLevel1": Config.HeartLevel1 = (int)arg2; UpdateHeartReq(); break;
                case "HeartLevel2": Config.HeartLevel2 = (int)arg2; UpdateHeartReq(); break;

                case "Milk Females": Config.MilkFemale = (bool)arg2; break;
                case "Milk Males": Config.MilkMale = (bool)arg2; break;

                case "Milking Button": Config.MilkButton = (SButton)arg2; break;
                case "Quests": Config.Quests = (bool)arg2; break;

                case "Ignore villager gender": Config.IgnoreVillagerGender = (bool)arg2; break;
                case "Simple milk/cum": Config.StackMilk = (bool)arg2; break;
                case "Verbose Dialogue": Config.Verbose = (bool)arg2; break;
                case "Rush_Mail": Config.RushMail = (bool)arg2; break;

                default: Log.Log($"{arg1}", LogLevel.Alert, Force: true); break;
            }

            #region Update token
            string GenderSwitch = "GenderSwitch {{Trunip190.MilkTheVillagers/GenderSwitch}} {{trunip190.CP.MilkTheVillagers/GenderSwitch}} The current time is {{Time}} on {{Season}} {{Day}}, year {{Year}}";
            //if (Game1.player.hasOrWillReceiveMail("MTV_Vagina")) GenderSwitch = "V";
            //if (Game1.player.hasOrWillReceiveMail("MTV_Herm")) GenderSwitch = "H";
            //if (Game1.player.hasOrWillReceiveMail("MTV_Ace")) GenderSwitch = "A";
            //if (Game1.player.hasOrWillReceiveMail("MTV_Penis")) GenderSwitch = "P";

            ContentPatcher.IManagedTokenString tokenString = contentPatcherApi.ParseTokenString(
               manifest: this.ModManifest,
               rawValue: GenderSwitch,
               formatVersion: new SemanticVersion("2.1.0"),
               assumeModIds: new[] { $"trunip190.CP.MilkTheVillagers" }
            );



            tokenString.UpdateContext();
            //string value = tokenString.Value; // The current time is 1430 on Spring 5, year 2.
            #endregion
        }

        private void UpdateGenitalConfig(string option, string value)
        {
            Log.Log($"{option} {value}", Force: true);
            if (option == "Override genitals") Config.OverrideGenitals = TempRefs.OverrideGenitals = value == "True";
            if (option == "Gender") Config.FarmerGender = TempRefs.FarmerGender = value;
            if (option == "Genitals") Config.FarmerGenitals = TempRefs.FarmerGenitals = value;
            if (option == "SaveFileGender") Game1.player.obsolete_isMale = value == "True";
            SendGenitalMail(Game1.player);
        }

        private void UpdateHeartReq()
        {
            LoveRequirement.Clear();

            foreach (KeyValuePair<string, string> kvp in Config.SexTopics)
            {
                int lvl = kvp.Value == "2" ? Config.HeartLevel2 : Config.HeartLevel1;
                LoveRequirement[kvp.Key] = lvl;

            }
        }

        #region SpaceEvents
        private void SpaceEvents_OnItemEaten(object sender, EventArgs e)
        {
            Farmer who = Game1.player;
            if (who.itemToEat.ItemId == "Trunip190.CP.MilkTheVillagers.Martini_Kairos")
            {
                Log.NewHud(Key: "HUD.Dialogue.TimeSlows", backupmessage: "Time slows...and stops");

                TimeFreezeTimer += 200000; // 1 minute timestop.
            }
            else if (who.itemToEat.ItemId == "Trunip190.CP.MilkTheVillagers.Eldritch_Energy")
            {
                Log.NewHud(Key: "HUD.Dialogue.Energised", backupmessage: "You feel energised");
                RegenAmount = 1000; // 100 energy total restoration.

            }
            else if (who.itemToEat.ItemId == "Trunip190.CP.MilkTheVillagers.Shibari_rope_package")
            {
                Log.NewHud(Key: "HUD.Dialogue.FailedToEat", backupmessage: "You cannot eat that");
                who.isEating = false;
            }


        }

        /// <summary>
        /// Interrupts the item given event so we can keep the item.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SpaceEvents_BeforeGiftGiven(object sender, EventArgsBeforeReceiveObject e)
        {
            Farmer who = Game1.player;
            string ItemGiven = e.Gift.ItemId;
            NPC npcTarget = e.Npc;

            if (ItemGiven == "Trunip190.CP.MilkTheVillagers.Invitation")
            {
                e.Cancel = true;
                if (BukkakePartyNPCs.Contains(e.Npc.Name))
                {
                    if (!Whom.modData.ContainsKey("Trunip190.Invitations"))
                        Whom.modData["Trunip190.Invitations"] = "";
                    if (!Whom.modData["Trunip190.Invitations"].Contains(e.Npc.Name))
                    {
                        e.Npc.Dialogue.TryGetValue("MTV_Bukkake", out string NPCtext);
                        if (NPCtext != $"(no translation:MTV_Bukkake")
                        {
                            Whom.modData["Trunip190.Invitations"] += $"\t{e.Npc.Name}";
                            Game1.DrawDialogue(new Dialogue(e.Npc, "MTV_Bukkake", NPCtext));
                            Whom.reduceActiveItemByOne();
                        }
                    }
                }
                else
                {
                    var NPCtext = i18n.Get(key: "Dialogue.Bukkake");
                    if (NPCtext != $"(no translation:MTV_Bukkake")
                        Game1.DrawDialogue(new Dialogue(e.Npc, "MTV_Bukkake", NPCtext));
                    else
                        Game1.DrawDialogue(new Dialogue(e.Npc, "MTV_Bukkake", i18n.Get("Dialogue.Bukkake")));
                }
            }

            if (ItemGiven == "Trunip190.CP.MilkTheVillagers.Readi_Milk")
            {
                e.Cancel = true;
                if (!Config.Quests)
                {
                    Dialogue v = new(npcTarget, "QuestsOff", i18n.Get("QuestsOff"));
                    Log.NewHud(backupmessage: v.getCurrentDialogue());
                    DrawDialogue(npcTarget, action: "QuestStartFail", FarmerKey: "QuestStartFail");

                    goto cleanup;
                }

                if (!Log.FirstMail.ContainsKey(npcTarget.Name))
                {

                    if (npcTarget.Dialogue.TryGetValue("QuestStartFail", out string FailDialogue))
                        DrawDialogue(npcTarget, action: "QuestStartFail", FarmerKey: "QuestStartFail");

                    goto cleanup;
                }

                string FirstQuestMail = Log.FirstMail[npcTarget.Name];
                if (FirstQuestMail != "" && !who.hasOrWillReceiveMail(FirstQuestMail))
                {
                    if (npcTarget.Dialogue.TryGetValue("QuestStartSuccess", out string SuccessDialogue))
                    {
                        DrawDialogue(npcTarget, BackupMessage: SuccessDialogue, action: "QuestStartSuccess", FarmerKey: "QuestStartSuccess");
                    }
                    else
                    {
                        DrawDialogue(npcTarget, BackupMessage: "quests not found?", action: "QuestStartFail", FarmerKey: "QuestStartSuccess");
                    }
                    SendNextMail(who, FirstQuestMail, true);

                    goto cleanup;
                }
            }

            if (ItemGiven == "Trunip190.CP.MilkTheVillagers.Discrete_Package")
            {
                if (npcTarget.Name != "Penny")
                {
                    var defaultFailed = i18n.Get("Dialogue.MTV_Parcel_Fail");
                    DrawDialogue(npcTarget, defaultFailed, action: "MTV_Parcel", FarmerKey: "MTV_Parcel_Fail");
                    e.Cancel = true;
                    goto cleanup;
                }

                foreach (LostItemQuest qid in who.questLog.Where(o => o.GetType() == typeof(LostItemQuest) && o.id.Value == $"{594826}").Cast<LostItemQuest>())
                {
                    qid.npcName.Set("Penny");
                    goto cleanup;
                }
            }

        cleanup:
            return;
        }
        #endregion

        private void OnButtonPressed(object sender, ButtonPressedEventArgs e)
        {

            if (running || !Context.IsWorldReady || Game1.paused || !Context.IsPlayerFree)
            {
                //log.Log($"running: {running}, Context.IsPlayerFree: {Context.IsPlayerFree}, IsWorldReady {Context.IsWorldReady}, menuUp {Game1.menuUp}", LogLevel.Trace);
                return;
            }

            //switch for multiplayer.
            Farmer who = Game1.player;
            running = false;
            currentTarget = null;

            SButton button = e.Button;
            var keys = Game1.GetKeyboardState().GetPressedKeys();

            if (button == SButton.P && Config.Debug)
            {
                Log.Log($"{who.currentLocation.Name}, {who.GetGrabTile().X}, {who.GetGrabTile().Y}");

                // Press P on load to trigger. for repeated testing only.
                if (!doneOnce)
                {
                    doneOnce = true;
                    //who.addQuest(594827);
                    //who.mailbox.Add("MilkButton1");
                    //who.mailbox.Add("MTV_ShaneQ3");
                    //who.changeFriendship(1750, Game1.getCharacterFromName("Penny"));
                    //who.changeFriendship(1250, Game1.getCharacterFromName("Alex"));
                    string npcToAdd = "Shane";
                    NetStringDictionary<Friendship, NetRef<Friendship>> data = who.friendshipData;
                    if (data.FieldDict.TryGetValue(npcToAdd, out NetRef<Friendship> value)) value.Value = new Friendship(2000);
                    else data.FieldDict.Add(npcToAdd, new Netcode.NetRef<Friendship>(new Friendship(2000)));

                    Game1.timeOfDay = 1400;

                    foreach (string mailitem in who.mailbox)
                        Log.Log($"MTV - {mailitem}");
                }
                else
                {

                    List<Response> choices = new()
                    {
                            new Response("Abigail","Abigail"),
                            new Response("Maru/Sebastian","Maru/Sebastian"),
                            new Response("Haley/Emily","Haley/Emily"),
                            new Response("Penny","Penny"),
                            new Response("Leah","Leah"),
                            new Response("Elliott","Elliott"),
                            new Response("Library","Library"),
                            new Response("Mines","Mines"),
                            new Response("Railroad", "Railroad"),
                            new Response("Farm","Farm"),
                    };

                    Game1.currentLocation.createQuestionDialogue($"Where do you want to warp?",
                        choices.ToArray(),
                        new GameLocation.afterQuestionBehavior(WarpFarmer));



                    SendNewMail("mtv_sendmail", new string[0]);
                    foreach (string v in who.mailForTomorrow)
                    {
                        Log.Log($"rushing mail item {v}", LogLevel.Trace);
                        who.mailbox.Add(v);
                    }
                    who.mailForTomorrow.Clear();
                }

            }
            else if (button == Config.MilkButton)
            {
                target = GetNewPos(who.FacingDirection, who.GetGrabTile());
                NPC NPCtarget = Log.FindTarget(who.currentLocation, this.target, who.GetGrabTile());
                Farmer companion = Log.FindFarmer(who.currentLocation, this.target, who.GetGrabTile());

                if (NPCtarget != null && !NPCtarget.IsMonster)
                {
                    List<Response> choices = GenerateSexOptions(NPCtarget); // Get list of available options for this character
                    Log.Log($"{NPCtarget.Name}: gendercode = {NPCtarget.Gender}, age = {NPCtarget.Age}", LogLevel.Alert);

                    currentTarget = NPCtarget;
                    running = false;


                    //string question = new Dialogue(null, translationKey: "Strings/StringsFromCSFiles:option.menu_dialogue_villager").dialogues[0].Text.Replace("[name]", NPCtarget.displayName);
                    //string question = Game1.content.LoadStringReturnNullIfNotFound("Strings/StringsFromCSFiles:option.menu_dialogue_villager");
                    string question = i18n.Get("Dialogue.option.menu_dialogue_villager", new { name = NPCtarget.displayName });
                    if (question != null)
                    {
                        //Game1.paused = true;
                        question = question.Replace("[name]", NPCtarget.displayName);
                        Game1.currentLocation.createQuestionDialogue(question, choices.ToArray(), new GameLocation.afterQuestionBehavior(DialoguesSet));
                    }
                    else { Log.Log($"Didn't find any dialogue for [Dialogue.option.menu_dialogue_villager]", LogLevel.Alert); }
                }
                else if (companion != null && companion != who)
                {
                    string question = i18n.Get("Dialogue.option.menu_dialogue_villager", new { name = NPCtarget.displayName });
                    Log.NewHud(backupmessage: question);
                }
                else  //Config.Debug)
                {
                    List<Response> options = new();

                    // TODO change to energy based or time based
                    if (GetPenis(who) && who.Stamina > 150)
                    {
                        options.Add(Log.ProcessOption("self_cum"));//collect own cum
                    }

                    if (GetBreasts(who) && who.stamina > 150)// && !TempRefs.SelfMilkedToday)
                    {
                        options.Add(Log.ProcessOption("self_milk")); //collect own breastmilk
                    }

                    // TODO not showing up?
                    if (false && who.Items.CountId("Trunip190.CP.MilkTheVillagers.Mr._Qi's_Essence") > 0)
                    {
                        options.Add(Log.ProcessOption("time_freeze"));
                    }

                    // TODO not showing up?
                    if (false && who.Items.CountId("Trunip190.CP.MilkTheVillagers.Eldritch_Energy") > 0)
                    {
                        options.Add(Log.ProcessOption("stamina_regen"));
                    }

                    if (options.Count > 0)
                    {
                        options.Add(new Response("abort", "Nothing"));

                        string question = i18n.Get("Dialogue.option.menu_dialogue_solo");
                        Game1.currentLocation.createQuestionDialogue(question, options.ToArray(), new GameLocation.afterQuestionBehavior(DialoguesSet));
                    }
                    else
                    {
                        Log.NewHud(Key: "HUD.Dialogue.TooTired", backupmessage: "You're too tired to do anything else right now.");
                    }
                }
            }
        }

        private void GameLoop_SaveLoaded(object sender, SaveLoadedEventArgs e)
        {
            Log.Log("MTV: GameLoop_SaveLoaded", LogLevel.Debug);
            loaded = true;
            doneOnce = false;

            TempRefs.thirdParty = Config.ThirdParty;
            TempRefs.Verbose = Config.Verbose;
            TempRefs.OverrideGenitals = Config.OverrideGenitals;
            TempRefs.FarmerGenitals = Config.FarmerGenitals;
            //TempRefs.HasPenis = Config.HasPenis;
            //TempRefs.HasVagina = Config.HasVagina;
            //TempRefs.HasBreasts = Config.HasBreasts;
            TempRefs.IgnoreVillagerGender = Config.IgnoreVillagerGender;

            if (runOnce)
                return;

            // GetItemCodes(); // Re-enable

            foreach (Farmer who in Game1.getAllFarmers())
            {
                Log.Log($"Sending mail to {who.displayName}", LogLevel.Trace);
                // CorrectRecipes(); // Handled by Content Patcher now
                // AddAllRecipes(who); // Handled by Content Patcher now
                SendGenitalMail(who);
            }

            runOnce = true;
            Log.Log("MTV: GameLoop_SaveLoaded Done", LogLevel.Debug);
        }

        private void GameLoop_DayStarted(object sender, DayStartedEventArgs e)
        {
            Log.Log("MTV: GameLoop_DayStarted", LogLevel.Debug);
            if (TempRefs.milkedtoday == null) Log.Log("TempRefs not set");
            else
            {
                Log.Log($"TempRefs is set. Cleared {TempRefs.milkedtoday.Count}");
                TempRefs.milkedtoday.Clear();
                TempRefs.SexToday.Clear();
                TempRefs.SelfCummedToday = false;
                TempRefs.SelfMilkedToday = false;
                MessageOnce = false;
            }



            //TODO change this for multiplayer
            foreach (Farmer who in Game1.getOnlineFarmers())
            {
                SendNewMail(who);
                who.questLog.OnElementChanged -= QuestLog_OnElementChanged;
                who.questLog.OnElementChanged += QuestLog_OnElementChanged;
            }
            TimeFreezeTimer = 0;
            RegenAmount = 0;
            Log.Log("MTV: GameLoop_DayStarted Done", LogLevel.Debug);
        }

        private void QuestLog_OnElementChanged(NetList<Quest, NetRef<Quest>> list, int index, Quest oldValue, Quest newValue)
        {
            if (newValue != null) CheckNewQuest(Whom, newValue);
            if (oldValue != null) { QuestCompleted(Whom, oldValue); }
        }

        private void GameLoop_OneSecondUpdateTicked(object sender, OneSecondUpdateTickedEventArgs e)
        {
            if (!loaded || Game1.dialogueUp || Game1.paused) return;
            Farmer Who = Game1.player; //TODO change to multiplayer

            SendNewMail(Who);
            QuestsCompletedByMail(Who);

            //CheckAddRecipes(Whom);

            if (RegenAmount > 0)
            {
                float restore = (float)(Game1.gameTimeInterval * 0.001);
                RegenAmount -= restore;
                Game1.player.stamina += restore;
                if (Game1.player.Stamina > Game1.player.MaxStamina) Game1.player.Stamina = Game1.player.MaxStamina;
            }
            if (TimeFreezeTimer > 0 || TimeFreeze)
            {
                if (!TimeFreeze)
                    TimeFreezeTimer -= Game1.gameTimeInterval;

                Game1.gameTimeInterval = 0;
                if (TimeFreezeTimer <= 0)
                    Log.NewHud(Key: "HUD.Dialogue.TimeRestarts", backupmessage: "Time starts moving again");
            }

            //TODO change for multiplayer
            foreach (Farmer who in Game1.getOnlineFarmers())
            {
                SwitchItems(who, "Trunip190.CP.MilkTheVillagers.Shibari_rope", "Trunip190.CP.MilkTheVillagers.Clothing.Shibari_rope");

                // Don't check anything if they haven't received the gender mail
                if (!who.mailReceived.Contains("MTV_Vagina") &&
                    !who.mailReceived.Contains("MTV_Penis") &&
                    !who.mailReceived.Contains("MTV_Ace") &&
                    !who.mailReceived.Contains("MTV_Herm"))
                {
                    Log.Log("Skipping quest watching");
                    return;
                }

            }

        }

        private void GameLoop_DayEnding(object sender, DayEndingEventArgs e)
        {
            //TODO not sure about this
            foreach (Farmer who in Game1.getOnlineFarmers())
            {
                //Don't check anything if they haven't received the first or last quest
                if (!who.mailReceived.Contains("AbiMilking") || who.mailReceived.Contains("MTV_LeahQ1T"))
                    return;

                CheckAddRecipes(who);

                Log.Log("removing repeatable events from seenEvents", LogLevel.Trace);
                who.eventsSeen.Remove("5948121");
            }
        }

        private void CheckAddRecipes(Farmer who)
        {
            // Checking for new recipes
            foreach (var v in Log.recipeRequirement)
            {
                if (v.Value.Key <= who.GetCustomSkillLevel(MSskill) && !who.cookingRecipes.ContainsKey(v.Value.Value))
                    who.cookingRecipes.Add(v.Value.Value, 0);
            }
        }

        private void Display_MenuChanged(object sender, MenuChangedEventArgs e)
        {

            string oldMenu = e.OldMenu != null ? e.OldMenu.ToString() : "";
            string newMenu = e.NewMenu != null ? e.NewMenu.ToString() : "";

            if (oldMenu == "StardewValley.Menus.DialogueBox" && newMenu == "")
            {
                //Monitor.Log($"adding {AddItem.Name} to {CurrentFarmer.Name}", LogLevel.Alert);
                if (AddItem == null) return;
                Whom.addItemToInventory(AddItem);
                AddItem = null;
            }
        }

        //[EventPriority(EventPriority.Low)]
        //private void Content_AssetRequested(object sender, AssetRequestedEventArgs e)
        //{
        //    if (QuestEditor.CanEdit(e.Name)) { e.Edit(QuestEditor.EditAsset); return; }
        //}

        protected virtual void DoActionNPC(object sender, ActionNPCEventArgs e)
        {
            OnActionNPC?.Invoke(this, e);
        }
        #endregion

        #region Quest methods
        /// <summary>
        /// List of quests that are completed by the player seeing a scene.
        /// </summary>
        /// <param name="Who">The player who has the quests.</param>
        private static void QuestsCompletedByMail(Farmer Who)
        {
            //Abigail
            CheckCompleteQuest(Who, "594804", "MTV_AbigailQ4T");

            //Elliott
            CheckCompleteQuest(Who, "594806", "MTV_ElliottQ2T"); // 
                                                                 //CheckCompleteQuest(Who, "594807", "MTV_ElliottQ3T"); // Pt1: kill skeletons
            CheckCompleteQuest(Who, "5948072", "MTV_ElliottQ3T"); // Pt2: speak with Elliott
            CheckCompleteQuest(Who, "594808", "MTV_ElliottQ4T"); // Event. Roleplay interrogation.

            //Sebastian
            //CheckCompleteQuest(Who, "594809", "MTV_SebQ1T"); // Milk Abi, return to Seb
            //CheckCompleteQuest(Who, "594810", "MTV_SebQ2T"); // Milk Seb, return to Abi
            CheckCompleteQuest(Who, "594811", "MTV_SebQ3T"); // Go touch grass. Event.
            CheckCompleteQuest(Who, "594812", "MTV_SebQ4T"); // Purple and Black desires. Event.

            //Emily
            CheckCompleteQuest(Who, "594818", "MTV_EmilyQ2P");

            // Haley
            //CheckCompleteQuest(who, "594821", "MTV_HaleyQ1T"); // Artistic nudes - event quest
            //CheckCompleteQuest(who, "594822", "MTV_HaleyQ2T"); // Milk Haley, lead into part 2
            //CheckCompleteQuest(who, "5948222", "MTV_HaleyQ2T"); // relieve Haley's other needs.
            //CheckCompleteQuest(who, "594823", "MTV_HaleyQ3T"); // Find a volunteer for a milking scene
            //CheckCompleteQuest(who, "594824", "MTV_HaleyQ4T"); // Hand out invitations
            //CheckCompleteQuest(who, "5948242", "MTV_HaleyQ4T"); // Invitation Event

            //Penny
            CheckCompleteQuest(Who, "594825", "MTV_PennyQ1P"); //auto leads into part two. standard completion in part 2
            CheckCompleteQuest(Who, "594827", "MTV_PennyQ3P");
            CheckCompleteQuest(Who, "5948272", "MTV_PennyQ3T");

            //Leah
            CheckCompleteQuest(Who, "594829", "MTV_LeahQ1P");
            CheckCompleteQuest(Who, "594831", "MTV_LeahQ3T");
            CheckCompleteQuest(Who, "5948322", "MTV_LeahQ4T");

            //Harvey
            //CheckCompleteQuest(Who, "594837", "MTV_HarveyQ1T");
            //CheckCompleteQuest(Who, "594838", "MTV_HarveyQ2T");
            //CheckCompleteQuest(Who, "594839", "MTV_HarveyQ3T");
            CheckCompleteQuest(Who, "594840", "MTV_HarveyQ4T");

            //Shane
            //CheckCompleteQuest(Who, "594841", "MTV_ShaneQ1T");
            //CheckCompleteQuest(Who, "594842", "MTV_ShaneQ2T");
            CheckCompleteQuest(Who, "594843", "MTV_ShaneQ3T");
            //CheckCompleteQuest(Who, "594844", "MTV_ShaneQ4T");

        }

        /// <summary>
        /// Checks for mail item to determine if quest should be completed.
        /// </summary>
        /// <param name="Who"></param>
        /// <param name="questID"></param>
        /// <param name="MailName"></param>
        private static void CheckCompleteQuest(Farmer Who, string questID, string MailName)
        {
            if (Who != null && Who.hasOrWillReceiveMail(MailName)) Who.completeQuest(questID);
        }

        private bool CheckNewQuest(Farmer who, Quest NewValue)
        {
            if (!NewValue.id.Contains("5948")) return false;
            if (CurrentQuests.Contains(QuestId(NewValue))) return false;

            //if (who.questLog.Any(o => o.id == NewValue.id)) // HasQuest(who, QuestId(NewValue)))
            //{
                if (QuestId(NewValue) == 594824)
                {
                    Log.Log("Setting ActiveConversationEvent to MTV_Bukkake", LogLevel.Info);
                    if (!who.activeDialogueEvents.ContainsKey("MTV_Bukkake"))
                    {
                        Game1.player.activeDialogueEvents.Add("MTV_Bukkake", 1);
                    }
                        who.addItemToInventory(new sObject("Trunip190.CP.MilkTheVillagers.Invitation", 5));
                }
                if (QuestId(NewValue) == 594841) //Shane quest 1
                {
                    Log.Log("Setting ActiveConversationEvent to MTV_ShaneQ1", LogLevel.Info);
                    if (!who.activeDialogueEvents.ContainsKey("MTV_ShaneQ1"))
                        Game1.player.activeDialogueEvents.Add("MTV_ShaneQ1", 1);
                }
                if (QuestId(NewValue) == 594834)
                {
                    Log.Log("Adding ConversationTopic", LogLevel.Info);
                    if (!who.activeDialogueEvents.ContainsKey("HaleyPanties"))
                        Game1.player.activeDialogueEvents.Add("HaleyPanties", 0);

                    // force the game to pick the right dialogue when married to Haley.
                    NPC haley = Game1.getCharacterFromName("Haley");
                    Game1.getCharacterFromName("Haley").resetCurrentDialogue();

                    bool check = haley.Dialogue.TryGetValue("HaleyPanties", out string dialogues);

                    Log.Log($"{haley.Name} {check} {dialogues}", LogLevel.Info);
                    haley.setNewDialogue(dialogues);
                }
                if (QuestId(NewValue) == 594836)
                {
                    Log.Log("Setting ActiveConversationEvent to MTV_GeorgeQ4", LogLevel.Info);
                    if (!who.activeDialogueEvents.ContainsKey("MTV_GeorgeQ4"))
                        Game1.player.activeDialogueEvents.Add("MTV_GeorgeQ4", 0);
                }
                if (QuestId(NewValue) == 594819)
                {
                    if (!who.knowsRecipe("Trunip190.CP.MilkTheVillagers.Crotchless_Panties"))
                    {
                        who.craftingRecipes["Trunip190.CP.MilkTheVillagers.Crotchless_Panties"] = 0; // Crafting name, then times crafted (0)
                    }
                }
                if (QuestId(NewValue) == 594826)
                {
                    Log.Log("Setting ActiveConversationEvent to MTV_Parcel", LogLevel.Info);
                    if (!who.activeDialogueEvents.ContainsKey("MTV_Parcel"))
                        who.activeDialogueEvents.Add("MTV_Parcel", 1);
                }

                CurrentQuests.Add(QuestId(NewValue));
                Log.Log($"Watching quest ID {QuestId(NewValue)}", LogLevel.Trace);
                return true;
            //}
        }

        private static bool CheckActionQuest(Farmer who, string QuestID, string Recipient, string[] ActionRequired, ActionNPCEventArgs e, Gender NPCGender = Gender.Undefined)
        {
            if (!who.hasQuest(QuestID) || !ActionRequired.Contains(e.Action)) return false;

            // Specific Name
            if (Recipient == e.Recipient.Name)
            {
                who.completeQuest(QuestID);
                return true;
            }

            // Male or Female
            if (Recipient == "Gender" && NPCGender == e.Recipient.Gender)
            {
                who.completeQuest(QuestID);
                return true;
            }

            // Magical
            if (Recipient == "Magical" && Magical.Contains(e.Recipient.Name))
            {
                who.completeQuest(QuestID);
                return true;
            }

            return false;
        }

        /// <summary>
        /// This section checks for the farmer performing actions with npc's for quests.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void CheckQuestActionOnNPC(object sender, ActionNPCEventArgs e)
        {
            switch (e.Recipient.Name)
            {
                case "Elliott":
                    CheckActionQuest(e.Who, "594805", "Elliott", new[] { "BJ", "milk_fast" }, e);
                    break;

                case "Abigail":
                    CheckActionQuest(e.Who, "594809", "Abigail", new[] { "milk_start", "milk_fast" }, e);
                    break;

                case "Sebastian":
                    CheckActionQuest(e.Who, "594810", "Sebastian", new[] { "BJ", "milk_fast" }, e);
                    CheckActionQuest(e.Who, "594811", "Sebastian", new[] { "BJ", "milk_fast" }, e);
                    break;

                case "Harvey":
                    CheckActionQuest(e.Who, "594815", "Harvey", new[] { "BJ", "milk_fast", "sex" }, e);
                    break;

                case "Haley":
                    CheckActionQuest(e.Who, "594822", "Haley", new[] { "milk_start", "milk_fast" }, e);
                    CheckActionQuest(e.Who, "5948222", "Haley", new[] { "eat_out" }, e);
                    break;

                case "George":
                    CheckActionQuest(e.Who, "594837", "George", new[] { "BJ", "milk_fast" }, e);
                    break;

                case "Jodi":
                    CheckActionQuest(e.Who, "5948382", "Jodi", new[] { "milk_start", "milk_fast" }, e);
                    break;

                case "Shane":
                    CheckActionQuest(e.Who, "594841", "Shane", new[] { "BJ", "milk_fast" }, e);
                    break;

                default:
                    if (CheckActionQuest(e.Who, "594813", "Gender", new[] { "BJ", "milk_fast" }, e, NPCGender: Gender.Male))
                        Log.NewHud(Key: "HUD.Quests.5948132", backupmessage: "Don't forget to turn it into Special Milk");

                    CheckActionQuest(e.Who, "594814", "Magical", new[] { "milk_start", "BJ", "milk_fast" }, e);
                    break;
            }

            Log.Log($"{e.Who.Name} did {e.Action} with {e.Recipient.Name} in {e.Map.Name}", LogLevel.Trace);
        }

        private void QuestCompleted(Farmer who, Quest compQuest)
        {
            if (!compQuest.id.Contains("5948")) return; // Check for quests in this pack.
            switch (compQuest.id.Value)
            {
                case "594824":
                    int stack = who.Items.Count(o => o.ItemId == "Invitation"); // Remove unused invitations
                    while (stack > 0)
                    {
                        var v = who.Items.GetById("Invitation").ToList();
                        who.Items.Remove(v[0]);
                        stack--;
                    }
                    goto default;

                default:
                    if (Log.QuestMail.ContainsKey(QuestId(compQuest)))
                        who.mailbox.Add(Log.QuestMail[QuestId(compQuest)]);
                    break;
            }

        }

        #endregion

        #region Mail
        #region SendNextMail
        #region SendNextMail
        private static void SendNextMail(Farmer who, string FinishedMail, string NextMail, string Villager, int Heartlevel, bool Immediate = false)
        {
            bool received = who.mailReceived.Contains(FinishedMail);
            bool willReceive = who.hasOrWillReceiveMail(NextMail);
            int friendshiplevel = who.getFriendshipHeartLevelForNPC(Villager);

            if (received &&
                !willReceive &&
                friendshiplevel >= Heartlevel)
            {
                if (Immediate) who.mailbox.Add(NextMail); else who.mailForTomorrow.Add(NextMail);
                Log.MailChecks.RemoveWhere(o => o.NewMail == NextMail);
            }
        }

        private static void SendNextMail(Farmer who, string FinishedMail, string NextMail, bool Immediate = false)
        {
            if (who.mailReceived.Contains(FinishedMail) && !who.hasOrWillReceiveMail(NextMail))
                if (Immediate) who.mailbox.Add(NextMail); else who.mailForTomorrow.Add(NextMail);

            if (who.mailForTomorrow.Contains(NextMail) && Immediate)
            {
                who.mailForTomorrow.Remove(NextMail);
                who.mailbox.Add(NextMail);
            }
        }

        private static void SendNextMail(Farmer who, string NextMail, bool Immediate = false)
        {
            if (!who.mailReceived.Contains(NextMail) && !who.hasOrWillReceiveMail(NextMail))
                if (Immediate) who.mailbox.Add(NextMail); else who.mailForTomorrow.Add(NextMail);
        }
        #endregion
        #endregion

        // This section handles sending the next mail when a quest is complete.
        private void SendNewMail(string command, string[] args)
        {
            if (args.Length > 0)
                Whom?.mailbox?.Add(args[0]);

            SendNewMail(Whom);
        }

        private void SendNewMail(Farmer who)
        {
            // Tutorial
            SendNextMail(who, "MilkButton1", Immediate: true);
            SendNextMail(who, "MilkButton1", "MilkButton2", Immediate: true);
            List<string> MailToRemove = new();

            if (Config.Quests) //TODO This section picks the next quest. Need to rewrite/decide how to send next mail/quest.
            {
                if (Config.MilkFemale)
                {
                    for (int i = 0; i < Log.MailChecks.Count; i++)
                    {
                        MailCheck mc = Log.MailChecks[i];
                        SendNextMail(who, mc.FinishedMail, mc.NewMail, mc.NPCname, mc.HeartRequired, Config.RushMail);
                        if (who.mailReceived.Contains(mc.NewMail)) MailToRemove.Add(mc.NewMail);
                    }
                    if (false) //trialling the above check to reduce calls. stop checking mail if the item is done.
                    {
                        #region Abigail
                        if (!who.mailReceived.Contains("MTV_AbigailQ2")) SendNextMail(who, "MTV_AbigailQ1T", "MTV_AbigailQ2", "Abigail", 6, Immediate: Config.RushMail);         // Abi Milk Quest 2
                        if (!who.mailReceived.Contains("MTV_AbigailQ3")) SendNextMail(who, "MTV_AbigailQ2T", "MTV_AbigailQ3", "Abigail", 7, Immediate: Config.RushMail);         // Abi Milk Quest 3
                        if (!who.mailReceived.Contains("MTV_AbigailQ4")) SendNextMail(who, "MTV_AbigailQ3T", "MTV_AbigailQ4", "Abigail", 8, Immediate: Config.RushMail);       // Abi Milk Quest 4
                        #endregion

                        #region Elliott - events written, single gender
                        SendNextMail(who, "MTV_ElliottQ1T", "MTV_ElliottQ2", "Elliott", 6, Immediate: Config.RushMail);         // Elliott Quest 2
                        SendNextMail(who, "MTV_ElliottQ2T", "MTV_ElliottQ3", "Elliott", 7, Immediate: Config.RushMail);         // Elliott Quest 3
                        SendNextMail(who, "MTV_ElliottQ3T", "MTV_ElliottQ4", "Elliott", 8, Immediate: Config.RushMail);        // Elliott Quest 4
                        #endregion

                        #region Sebastian
                        SendNextMail(who, "MTV_SebQ1T", "MTV_SebQ2", "Sebastian", 6, Immediate: Config.RushMail);   //Sebastian Quest 2
                        SendNextMail(who, "MTV_SebQ2T", "MTV_SebQ3", "Sebastian", 7, Immediate: Config.RushMail);   //Sebastian Quest 3
                        SendNextMail(who, "MTV_SebQ3T", "MTV_SebQ4", "Sebastian", 8, Immediate: Config.RushMail);  //Sebastian Quest 4
                        #endregion

                        #region Maru
                        SendNextMail(who, "MTV_MaruQ1T", "MTV_MaruQ2", "Maru", 6, Immediate: Config.RushMail);     // Maru Quest 2
                        SendNextMail(who, "MTV_MaruQ2T", "MTV_MaruQ3", "Maru", 7, Immediate: Config.RushMail);     // Maru Quest 3
                        SendNextMail(who, "MTV_MaruQ3T", "MTV_MaruQ4", "Maru", 8, Immediate: Config.RushMail);   // Maru Quest 4
                        #endregion

                        #region Emily
                        SendNextMail(who, "MTV_EmilyQ1T", "MTV_EmilyQ2", "Emily", 6, Immediate: Config.RushMail);               //Emily Quest 2
                                                                                                                                //SendNextMail(who, "MTV_EmilyQ2T", "MTV_EmilyQ3", "Emily", 7, Immediate: Config.RushMail);             //Emily Quest 3 not written yet
                                                                                                                                //SendNextMail(who, "MTV_EmilyQ3T", "MTV_EmilyQ4", "Emily", 8, Immediate: Config.RushMail);              //Emily Quest 4
                        #endregion

                        #region Harvey
                        SendNextMail(who, "MTV_HarveyQ1T", "MTV_HarveyQ2", "Harvey", 6, Immediate: Config.RushMail);            // Harvey Quest 2
                        SendNextMail(who, "MTV_HarveyQ2T", "MTV_HarveyQ3", "Harvey", 7, Immediate: Config.RushMail);            // Harvey Quest 3
                        SendNextMail(who, "MTV_HarveyQ3T", "MTV_HarveyQ4", "Harvey", 8, Immediate: Config.RushMail);           // Harvey Quest 4
                        #endregion

                        #region Penny
                        SendNextMail(who, "MTV_PennyQ1T", "MTV_PennyQ2", "Penny", 6, Immediate: Config.RushMail);               //Penny Quest 2
                        SendNextMail(who, "MTV_PennyQ2T", "MTV_PennyQ3", "Penny", 7, Immediate: Config.RushMail);               //Penny Quest 3
                        /* Not written yet */
                        SendNextMail(who, "MTV_PennyQ3T", "MTV_PennyQ4", "Penny", 8, Immediate: Config.RushMail);              //Penny Quest 4
                        #endregion

                        #region George 
                        SendNextMail(who, "MTV_GeorgeQ1T", "MTV_GeorgeQ2", "George", 6, Immediate: Config.RushMail);            // George Quest 2
                                                                                                                                //SendNextMail(who, "MTV_GeorgeQ2T", "MTV_GeorgeQ3", "George", 7, Immediate: Config.RushMail);            // George Quest 3
                        SendNextMail(who, "MTV_GeorgeQ3T", "MTV_GeorgeQ4", "George", 8, Immediate: Config.RushMail);           // George Quest 4
                        #endregion

                        #region Haley
                        SendNextMail(who, "MTV_HaleyQ1T", "MTV_HaleyQ2", "Haley", 6, Immediate: Config.RushMail);               //Haley Quest 2
                        SendNextMail(who, "MTV_HaleyQ2T", "MTV_HaleyQ3", "Haley", 7, Immediate: Config.RushMail);               //Haley Quest 3
                        SendNextMail(who, "MTV_HaleyQ3T", "MTV_HaleyQ4", "Haley", 8, Immediate: Config.RushMail);              //Haley Quest 4
                        #endregion

                        #region Leah
                        SendNextMail(who, "MTV_LeahQ1T", "MTV_LeahQ2", "Leah", 6, Immediate: Config.RushMail);      //Leah Quest 2
                        SendNextMail(who, "MTV_LeahQ2T", "MTV_LeahQ3", "Leah", 7, Immediate: Config.RushMail);      //Leah Quest 3
                                                                                                                    //SendNextMail(who, "MTV_LeahQ3T", "MTV_LeahQ4", "Leah", 8, Immediate: Config.RushMail);    //Leah Quest 4 - Not written yet
                        #endregion

                        #region Shane
                        SendNextMail(who, "MTV_ShaneQ1T", "MTV_ShaneQ2", "Shane", 6, Immediate: Config.RushMail);   //Leah Quest 2
                        SendNextMail(who, "MTV_ShaneQ2T", "MTV_ShaneQ3", "Shane", 7, Immediate: Config.RushMail);   //Leah Quest 3
                        SendNextMail(who, "MTV_ShaneQ3T", "MTV_ShaneQ4", "Shane", 8, Immediate: Config.RushMail);   //Leah Quest 4 - Not written yet
                        #endregion
                    }


                }
            }
            if (MailToRemove.Count > 0) Log.Log($"MTV Removing {0} mail from quest mail list.");
            foreach (string s in MailToRemove)
            {
                Log.MailChecks.RemoveWhere(o => o.NewMail == s);
            }
        }

        public void SendGenitalMail(Farmer who)
        {
            string newMail;

            if (Config.AceCharacter) newMail = "MTV_Ace";
            else if (GetPenis(who) && GetVagina(who)) newMail = "MTV_Herm";
            else if (GetPenis(who) && !GetVagina(who)) newMail = "MTV_Penis";
            else if (!GetPenis(who) && GetVagina(who)) newMail = "MTV_Vagina";
            else newMail = "MTV_Ace";


            foreach (string s in GenderMail)
                if (s != newMail) who.RemoveMail(s);
        }

        #endregion

        #region checks etc.

        /// <summary>
        /// 
        /// </summary>
        /// <param name="who">The farmer who's inventory you want to search</param>
        /// <param name="BaitItem">The item to remove</param>
        /// <param name="SwitchItem">The item to replace it with</param>
        private static void SwitchItems(Farmer who, string oldValue, string newItem)
        {
            var items = who.Items.GetById(oldValue);
            int count = 0;
            while (items.Any())
            {
                count += items.First().Stack;
                who.Items.Remove(items.First());
            }
            who.Items.Add(new Clothing(newItem) { Stack = count });
        }

        #endregion

        #region Interact with NPC section

        private List<Response> GenerateSexOptions(NPC target)
        {
            List<Response> choices;
            choices = new();

            if (target.Age != 2) // 2 is a child - immediate yeet
            {
                Log.Log($"{target.Name} is a child.", LogLevel.Trace);
                if ((TempRefs.IgnoreVillagerGender || target.Gender == Gender.Male) && Config.MilkMale) // Male and genderless
                {
                    choices.Add(Log.ProcessOption("milk_fast_male"));
                    choices.Add(Log.ProcessOption("BJ"));
                }
                else if ((TempRefs.IgnoreVillagerGender || target.Gender == Gender.Female) && Config.MilkFemale) // Female and genderless
                {
                    choices.Add(Log.ProcessOption("milk_fast_fem"));
                    choices.Add(Log.ProcessOption("milk_start"));
                    choices.Add(Log.ProcessOption("eat_out")); //TODO Not written yet.
                }
                else if (target.Gender == Gender.Undefined) // Genderless
                {
                    switch (target.Name)
                    {
                        case "Dwarf":
                            choices.Add(Log.ProcessOption("milk_fast_fem"));
                            choices.Add(Log.ProcessOption("milk_start"));
                            break;

                        case "Krobus":
                            choices.Add(Log.ProcessOption("milk_fast_male"));
                            choices.Add(Log.ProcessOption("BJ"));
                            break;

                        case "Mister Qi":
                            choices.Add(Log.ProcessOption("milk_fast_male"));
                            choices.Add(Log.ProcessOption("BJ"));
                            break;

                        default:
                            break;
                    }
                }

            }
            choices.Add(Log.ProcessOption("abort"));

            return choices;
        }

        private void WarpFarmer(Farmer who, string ChosenLocation)
        {
            switch (ChosenLocation)
            {
                case "Farm": Game1.warpFarmer("Farm", 67, 17, 00); break;
                case "Abigail": Game1.warpFarmer("SeedShop", 4, 7, 00); break;
                case "Maru/Sebastian": Game1.warpFarmer("ScienceHouse", 7, 8, 00); break;
                case "Haley/Emily": Game1.warpFarmer("HaleyHouse", 8, 20, 00); break;
                case "Penny": Game1.warpFarmer("Trailer", 12, 8, 00); break;
                case "Leah": Game1.warpFarmer("LeahHouse", 7, 8, 1); break;
                case "Elliott": Game1.warpFarmer("ElliottHouse", 3, 8, 1); break;
                case "Library": Game1.warpFarmer("Town", 101, 90, 1); break;
                case "Mines": Game1.warpFarmer("Mine", 18, 8, 1); break;
                case "Railroad": Game1.warpFarmer("Railroad", 29, 58, 1); break;
            }
        }

        public void DialoguesSet(Farmer who, string action)
        {
            if (who == null || action == null || action == "abort") return;

            #region Get farmer for self dialogue - TODO fix npc not getting loaded.

            //IDictionary<string, CharacterData> npcs = Game1.characterData;
            #endregion

            if (Config.Verbose && currentTarget != null)
            {
                string message = i18n.Get("Dialogue.OptionChosen", new { key = action, name = currentTarget.Name });
                Log.NewHud(backupmessage: message);
            }
            if (action == "time_freeze") //pauses in game time progression for 1min IRL
            {
                var v = who.Items.GetById("Trunip190.CP.MilkTheVillagers.Mr._Qi's_Essence").ToList();
                if (v.Count > 0)
                {
                    who.removeItemFromInventory(v[0]);
                }
                TimeFreezeTimer = 576000; // 1 minute timestop.
            }
            else if (action == "stamina_regen")
            {
                var v = who.Items.GetById("Trunip190.CP.MilkTheVillagers.Eldritch_Energy").ToList();
                if (v.Count > 0)
                {
                    who.removeItemFromInventory(v[0]);
                    RegenAmount = 100; // 100hp total restoration.
                }
            }
            else if (action == "self_milk")
            {
                Game1.DrawDialogue(Log.TrimDialogue(new Dialogue(null, "Dialogue.FarmerCollectionMilk", i18n.Get("Dialogue.FarmerCollectionMilk"))));
                who.addItemToInventory(new sObject("Trunip190.CP.MilkTheVillagers.Woman's_Milk", 1, quality: 2));
                who.Stamina -= 150;
            }
            else if (action == "self_cum") // farmer collects their own cum
            {
                Game1.DrawDialogue(Log.TrimDialogue(new Dialogue(null, "Dialogue.FarmerCollectionMilk", i18n.Get("Dialogue.FarmerCollectCum"))));
                who.addItemToInventory(new sObject("Trunip190.CP.MilkTheVillagers.Special_Milk", 1, quality: 2));
                who.Stamina -= 150;
            }
            else if (!currentTarget.IsMonster) ActionOnNPC(currentTarget, who, action);

        }

        /// <summary>
        /// Starts the process of performing a text based user action on the NPC
        /// </summary>
        /// <param name="npc">the NPC you are performing the action with</param>
        /// <param name="who">the farmer performing the action</param>
        /// <param name="action">the text name of the dialogue to perform</param>
        private void ActionOnNPC(NPC npc, Farmer who, string action = "milk_start")
        {
            #region Set up vars for method
            bool success = false;

            if (action.Contains("milk_fast")) action = "milk_fast";

            // Reset Additem
            AddItem = null;
            int HeartCurrent = who.getFriendshipHeartLevelForNPC(npc.Name);
            KeyValuePair<sObject, string> parsedConvo = new();
            string chosenString;
            string chosenKey;
            int Quality;
            int Quantity = 1;

            // This replaces the switch below.
            int heartMin = LoveRequirement[action];
            int energyCost = SexCost[action];
            int friendGain = SexLove[action];
            #endregion

            #region validity checks
            if (npc.Age == 2) // 2 is a child - immediate yeet
            {
                Monitor.Log($"{npc.Name} is {npc.Age}", LogLevel.Debug);
                goto cleanup;
            }

            if (HeartCurrent < heartMin && npc.CanSocialize)//  npc.name != "Mister Qi") // Check if the NPC likes you enough.
            {
                //var rejectionDialogue = new Dialogue(npc, "Strings/StringsFromCSFiles:HeartRejection").dialogues[0].Text.Replace("[HeartCurrent]", $"{HeartCurrent}").Replace("[heartMin]", $"{heartMin}");
                string rejectionDialogue = i18n.Get("Dialogue.HeartRejection", new { HeartHave = HeartCurrent, HeartReq = heartMin });
                DrawDialogue(npc, rejectionDialogue, action: "action_rejected", FarmerKey: "HeartRejection", currentHearts: HeartCurrent, minHearts: heartMin);
                string message = i18n.Get("HUD.LowAffection", new { name = npc.displayName, current = HeartCurrent, min = heartMin });
                Log.NewHud(backupmessage: message);
                Log.Log(message, LogLevel.Trace);
                goto cleanup;
            }

            // Energy level
            if (who.stamina <= energyCost)
            {
                //var lowEnergy = new Dialogue(npc, "Strings/StringsFromCSFiles:EnergyLow").dialogues[0].Text.Replace("[who.stamina]", $"{who.stamina}").Replace("[energyCost]", $"{energyCost}");
                string lowEnergy = i18n.Get("Dialogue.EnergyLow", new { stamina = who.stamina, cost = energyCost });
                Log.NewHud(backupmessage: lowEnergy);
                goto cleanup;
            }

            //milked today
            //TODO rewrite this to base it off of the choice.
            if ((action == "milk_start" || action == "milk_fast") && TempRefs.milkedtoday.Contains(npc))
            {
                if (Config.Verbose)
                {
                    string AlreadyMilked = i18n.Get("Dialogue.AlreadyMilked", new { name = npc.displayName });
                    Log.NewHud(backupmessage: AlreadyMilked);
                }
                goto cleanup;
            }
            if ((action == "sex" || action == "BJ") && TempRefs.SexToday.Contains(npc))
            {
                if (Config.Verbose)
                {
                    string AlreadySex = i18n.Get("Dialogue.AlreadySex", new { name = npc.displayName });
                    Log.NewHud(backupmessage: AlreadySex);
                }
                goto cleanup;
            }
            //int ItemCode;
            string ItemCode; // = "";
            #endregion

            // Draw Dialogue
            npc.facePlayer(who);

            if (npc.Dialogue.TryGetValue(action, out string dialogues)) //Does npc have milking dialogue?
            {
                //chosenString = GetRandomString(dialogues.Split(new string[] { "#split#" }, System.StringSplitOptions.None));
                chosenString = GetRandomString(Log.SplitStrings(dialogues));

                switch (action)
                {
                    case "milk_fast":
                        if (npc.Gender == Gender.Female)
                        {
                            chosenKey = "DefaultFastMilkFem";
                        }
                        else if (npc.Gender == Gender.Male)
                        {
                            chosenKey = "DefaultFastMilkMale";
                        }
                        else
                        {
                            chosenKey = "DefaultFastMilkNull";
                        }
                        break;

                    case "milk_start":
                        if (npc.Gender == Gender.Female)
                        {
                            chosenKey = "DefaultFemaleMilk";
                        }
                        else if (npc.Gender == 0)
                        {
                            chosenKey = "DefaultMaleMilk";
                        }
                        else
                        {
                            chosenKey = "DefaultFailMilk";
                        }

                        DrawDialogue(npc, BackupMessage: chosenString, action: action, FarmerKey: chosenKey);
                        success = true;
                        break;

                    case "eat_out":
                        chosenKey = "DefaultEatOut";
                        break;

                    case "get_eaten":
                        chosenKey = "DefaultGetEaten";
                        break;

                    case "BJ":
                        chosenKey = "DefaultMaleMilk";
                        break;

                    case "sex":
                        if (npc.Gender == 0 && who.IsMale)
                            chosenKey = "DefaultSexMM";
                        else if (npc.Gender == Gender.Female && who.IsMale)
                            chosenKey = "DefaultSexMF";
                        else if (npc.Gender == 0 && !who.IsMale)
                            chosenKey = "DefaultSexFM";
                        else
                        {
                            if (HasStrapon(who)) chosenKey = "DefaultSexFF";
                            else chosenKey = "DefaultSexFF";
                        }
                        break;

                    default:
                        chosenKey = "DefaultCancel";
                        break;
                }

                if (action != "eat_out" && action != "sex" && action != "get_eaten")
                {
                    #region set item to give
                    //ItemCode = npc.Gender == 0 ? TempRefs.MilkSpecial : TempRefs.MilkGeneric;
                    ItemCode = npc.Gender == 0 ? $"{TempRefs.ModItemPrefix}Special_Milk" : $"{TempRefs.ModItemPrefix}Woman's_Milk";
                    Quality = 1;

                    // Don't remove this - It's a good way of speeding up for people.
                    if (Config.StackMilk)
                    {
                        ItemCode = npc.Gender == 0 ? $"{TempRefs.ModItemPrefix}Special_Milk" : $"{TempRefs.ModItemPrefix}Woman's_Milk";
                        if (npc.Name == "Mister Qi"
                            || npc.Name == "Wizard"
                            || npc.Name == "Krobus"
                            || npc.Name == "Dwarf")
                        {
                            ItemCode = $"{TempRefs.ModItemPrefix}Magical_Essence";
                        }
                    }


                    AddItem = new sObject(ItemCode, Quantity, quality: Quality) { Category = -34 };

                    // Trying to fix item category
                    AddItem.Category = (AddItem.Name.Contains("Milk") && AddItem.Name != "Special Milk") ? -34 : -35;
                    if (AddItem.Name == "Dwarf's Essence"
                        || AddItem.Name == "Krobus's Essence"
                        || AddItem.Name == "Wizard's Essence"
                        || AddItem.Name == "Mr. Qi's Essence"
                        || AddItem.Name == "Magical Essence")
                    {
                        AddItem.Category = -36;
                    }

                    Log.Log($"Trying to milk {npc.Name}. Will give item {ItemCode}: {ItemCode}, Category: {AddItem.Category} / {AddItem.getCategoryName()}", LogLevel.Trace);
                    // If no male milking, don't give item.
                    if ((npc.Gender == 0 & !Config.MilkMale) || !Config.CollectItems || action == "eat_out" || action == "sex" || action == "get_eaten")
                    {
                        //SItemCode = "";
                        AddItem = null;
                    }

                    Log.Log($"{AddItem}");

                    parsedConvo = GetQualityCount(who, chosenString, ItemCode);
                    AddItem = parsedConvo.Key;

                    if (AddItem == null)
                    {
                        // Get Item code from ChosenString
                        if (chosenString.Contains('['))
                        {
                            int start = chosenString.IndexOf('[') + 1;
                            int end = chosenString.IndexOf(']', start);
                            string val = chosenString[start..end];
                            Log.Log($"{npc.Name} value was {val}", LogLevel.Trace);

                            chosenString = chosenString.Replace($"{val}", "").Replace("[]", "").Trim();
                            if (!Config.StackMilk) ItemCode = val;
                        }

                        AddItem = new sObject($"{ItemCode}", 1);

                        // Get Quality from ChosenString
                        if (chosenString.Contains("[Quality:"))
                        {
                            int start = chosenString.IndexOf("[Quality:");
                            int end = chosenString.IndexOf(']', start);

                            string val = chosenString.Substring(end - 1, 1);
                            Log.Log($"{npc.Name} Quality was {val}", LogLevel.Trace);
                            _ = int.TryParse(val, out Quality);

                            AddItem.Quality = Quality;
                            chosenString = chosenString.Replace($"Quality:{val}", "").Replace("[]", "").Trim();
                        }

                        // Get Quantity from ChosenString
                        if (chosenString.Contains("[Quantity:"))
                        {
                            //"{{spacechase0.JsonAssets/ObjectId: }}";
                            int start = chosenString.IndexOf("[Quantity:");
                            int end = chosenString.IndexOf(']', start);

                            string val = chosenString.Substring(end - 1, 1);
                            Log.Log($"{npc.Name} Quantity was {val}", LogLevel.Trace);
                            _ = int.TryParse(val, out Quantity);

                            AddItem.Stack = Quantity;
                            chosenString = chosenString.Replace($"Quantity:{val}", "").Replace("[]", "");
                        }
                    }

                    // HUD messages
                    if (who.HasCustomProfession(MilkingSkill.MTV5_1_HigherQuality) && who.HasCustomProfession(MilkingSkill.MTV10_11_MoreItems))
                    {
                        if (Config.Verbose && !MessageOnce) Log.NewHud(Key: "HUD.SkillQualityCount");
                        MessageOnce = true;
                    }
                    else if (who.HasCustomProfession(MilkingSkill.MTV5_1_HigherQuality) && !MessageOnce)
                    {
                        if (Config.Verbose) Log.NewHud(Key: "HUD.SkillQuality");
                        MessageOnce = true;
                    }
                    else if (who.HasCustomProfession(MilkingSkill.MTV10_11_MoreItems) && !MessageOnce)
                    {
                        if (Config.Verbose) Log.NewHud(Key: "HUD.SkillCount");
                        MessageOnce = true;
                    }

                    #endregion
                }

                DrawDialogue(npc, parsedConvo.Value, action, FarmerKey: chosenKey);
                success = true;
            }
            else
            {
                Log.Log($"{npc.Name} failed to get anything for action {action}. Probably not written yet.", LogLevel.Info);
                switch (action)
                {
                    case "milk_fast":
                        if (npc.Gender == Gender.Female)
                        {
                            chosenString = $" ";// [{ItemCode}]";
                            chosenKey = "DefaultFastMilkFem";
                        }
                        else if (npc.Gender == Gender.Male)
                        {
                            chosenString = $" ";
                            chosenKey = "DefaultFastMilkMale";
                        }
                        else
                        {
                            chosenString = $" ";
                            chosenKey = "DefaultFastMilkNull";
                        }
                        DrawDialogue(npc, BackupMessage: chosenString, action: action, FarmerKey: chosenKey);
                        success = true;
                        break;

                    case "milk_start":
                        if (npc.Gender == Gender.Female)
                        {
                            chosenString = $"I've never been asked that by anyone else. Although, that DOES sound kinda hot.#$b#You spend the next few minutes slowly kneeding their breasts, collecting the milk in a jar you brought with you.";// [{ItemCode}]";
                            chosenKey = "DefaultFemaleMilk";
                        }
                        else if (npc.Gender == 0)
                        {
                            chosenString = $"You want my 'milk'? Erm, You ARE very attractive...#$b#*You quickly unzip their pants and pull out their cock. After a couple of quick licks to get them hard, you start sucking on them*#$b#I think I'm getting close! Here it comes!";
                            chosenKey = "DefaultMaleMilk";
                        }
                        else
                        {
                            chosenString = $"I'm sorry, but I don't produce anything you would want to collect. Perhaps we can do something else instead?";
                            chosenKey = "DefaultFailMilk";
                        }

                        DrawDialogue(npc, BackupMessage: chosenString, action: action, FarmerKey: chosenKey);
                        success = true;
                        break;

                    case "eat_out":
                        chosenString = $"You want to go down on me? I don't think I've ever had a guy offer to do that without an ulterior motive before.$h" +
                            $"#$b#%*{npc.Name} quickly strips out of her lower garments, and opens her legs wide for you. You're greeted with a heady smell, and notice that her lips are starting to swell*" +
                            $"#$b#%*You lean in and start licking between {npc.Name}'s lips, tasting her sweet nectar as she buries her hands in your hair*" +
                            $"#$b#%*As her moans get louder you use your tongue to flick her clit, and she clenches her legs tightly around your head*" +
                            $"#$b#%*You gently suck on the nub, and then plunge your tongue as deeply into her as you can, shaking your tongue from side to side to stimulate {npc.Name} even more*" +
                            $"#$b#@, I'm cumming!";
                        chosenKey = "DefaultEatOut";

                        DrawDialogue(npc, BackupMessage: chosenString, action: action, FarmerKey: chosenKey);
                        success = true;
                        break;

                    case "get_eaten":
                        chosenString = $"I'm sorry, dialogue hasn't been written for that yet.";
                        chosenKey = "DefaultGetEaten";
                        DrawDialogue(npc, BackupMessage: chosenString, action: action, FarmerKey: chosenKey);
                        success = true;
                        break;

                    case "BJ":
                        chosenString = $"You want my 'milk'? Erm, You ARE very attractive...#$b#*You quickly unzip their pants and pull out their cock. After a couple of quick licks to get them hard, you start sucking on their penis*#$b#I think I'm getting close! Here it comes!";
                        chosenKey = "DefaultMaleMilk";
                        DrawDialogue(npc, BackupMessage: chosenString, action: action, FarmerKey: chosenKey);
                        success = true;
                        break;

                    case "sex":
                        //TODO write four version of this for each gender configuration.

                        if (npc.Gender == 0 && who.IsMale) // Male player, male NPC.
                        { chosenString = $"two dudes going at it"; chosenKey = "DefaultSexMM"; }
                        else if (npc.Gender == Gender.Female && who.IsMale) // Male player, female NPC.
                        { chosenString = $"{who.Name} buries their cock deep inside {npc.Name}'s pussy"; chosenKey = "DefaultSexMF"; }
                        else if (npc.Gender == 0 && !who.IsMale) // Female player, male NPC.
                        { chosenString = $"{who.Name} climbs on top of {npc.Name}'s erect cock and plunges it deep inside them until they cum"; chosenKey = "DefaultSexFM"; }
                        else // neither is male
                            if (HasStrapon(who))
                            { chosenString = $"You put on your strapon and fuck {npc.Name} silly."; chosenKey = "DefaultSexFF"; }
                            else
                            { chosenString = $"You and {npc.Name} lick, suck and finger each other into oblivion"; chosenKey = "DefaultSexFF"; }

                        DrawDialogue(npc, BackupMessage: chosenString, action: action, FarmerKey: chosenKey);
                        success = true;
                        break;

                    default:
                        chosenString = $"I don't have any dialogue written for that. Sorry.";
                        chosenKey = "DefaultCancel";
                        DrawDialogue(npc, BackupMessage: chosenString, action: action, FarmerKey: chosenKey);
                        Monitor.Log($"{action} for {npc.Name} wasn't found");
                        break;
                }
            }


        cleanup:
            // Cleanup
            if (success)
            {
                if (!Config.CollectItems) AddItem = null;
                // Stamina reduction
                who.stamina -= who.HasCustomProfession(MilkingSkill.MTV10_21_LessStamina) ? (int)(energyCost * 0.9) : energyCost;

                who.AddCustomSkillExperience(MSskill, energyCost);
                switch (action) //Add npc to the correct action performed today list.
                {
                    case "milk_fast":
                        TempRefs.milkedtoday.Add(npc);
                        break;

                    case "milk_start":
                        TempRefs.milkedtoday.Add(npc);
                        break;

                    case "sex":
                        TempRefs.SexToday.Add(npc);
                        break;

                    case "BJ":
                        TempRefs.milkedtoday.Add(npc);
                        TempRefs.SexToday.Add(npc);
                        break;
                }

                // Friendship add
                int FriendBonus = who.HasCustomProfession(MilkingSkill.MTV5_2_LoveGain) ? 10 : 0;
                FriendBonus = who.HasCustomProfession(MilkingSkill.MTV10_22_MaxLoveGain) ? FriendBonus + 10 : FriendBonus;
                who.changeFriendship(friendGain + FriendBonus, npc);

                // Raise new event for other mods to link off of.
                ActionNPCEventArgs args = new()
                {
                    Who = who,
                    Recipient = npc,
                    Action = action,
                    Map = who.currentLocation
                };
                DoActionNPC(this, args);
            }
            else
            {
                string message = i18n.Get("HUD.ActionFailed", new { name = npc.displayName });
                Log.NewHud(backupmessage: message);
            }
        }

        public static KeyValuePair<sObject, string> GetQualityCount(Farmer who, string chosenString, string ItemCode)
        {
            string[] split = chosenString.Split(new[] { "[", "]" }, StringSplitOptions.RemoveEmptyEntries);
            string sItemCode = split.FirstOrDefault(o => o.Contains("Trunip190.CP.MilkTheVillagers."), $"{ItemCode}");
            int iQuantity = Log.StringToInt(split.FirstOrDefault(o => o.Contains("Quantity:"), "Quantity:0")?.Replace("Quantity:", ""));
            int iQuality = Log.StringToInt(split.FirstOrDefault(o => o.Contains("Quality:"), "Quality:0")?.Replace("Quality:", ""));

            if (who.HasCustomProfession(MilkingSkill.MTV5_1_HigherQuality)) iQuality++;
            if (who.HasCustomProfession(MilkingSkill.MTV10_12_HighestQuality)) iQuality++;
            if (who.HasCustomProfession(MilkingSkill.MTV10_11_MoreItems)) iQuantity++;

            sObject newItem = new(sItemCode, iQuantity, quality: iQuality);

            return new(newItem, split[0]);
        }

        private static string GetRandomString(string[] dialogues)
        {
            int i = dialogues.Length;
            return i < 1 ? "" : dialogues[new Random().Next(i)];
        }

        private void DrawDialogue(NPC npc, string BackupMessage = "Placeholder text", string action = "milk_start", string FarmerKey = "option.milk_start", int currentHearts = 0, int minHearts = 0)
        {
            string source = $"Characters/Dialogue/{npc.Name}:{action}";
            if (npc.IsMonster) return;
            Dialogue v = new(npc, source, false);

            foreach (DialogueLine dl in v.dialogues)
            {
                dl.Text = dl.Text.Split(new[] { '[', ']' }, StringSplitOptions.RemoveEmptyEntries)[0];
            }

            var NewMessage = v.dialogues[0];

            if (NewMessage.Text.Contains("Characters/Dialogue/") || NewMessage.Text.Trim() == "")
            {
                v = new Dialogue(npc, "Dialogue.FarmerKey", i18n.Get("Dialogue.FarmerKey"));

                v.dialogues[0].Text = v.dialogues[0].Text.Replace("[HeartCurrent]", $"{currentHearts}").Replace("[heartMin]", $"{minHearts}");
            }

            if (v.dialogues[0].Text.Trim() == "") return;
            Game1.DrawDialogue(v);
        }

        private static bool HasStrapon(Farmer who)
        {
            return who.Items.GetById("Strapon").ToList().Count > 0 || true;
        }

        private static Framework.Vector2 GetNewPos(int direction, Framework.Vector2 position)
        {
            Framework.Vector2 numArray = position;
            switch (direction)
            {
                case 0:
                    --numArray.Y; //--numArray[1];
                    break;
                case 1:
                    ++numArray.X; //++numArray[0];
                    break;
                case 2:
                    ++numArray.Y; //++numArray[1];
                    break;
                case 3:
                    --numArray.X; //--numArray[0];
                    break;
            }
            return numArray;
        }

        //private static Framework.Vector2 FarmerPos(Farmer who) => who.GetGrabTile();

        #endregion



        #region Testing/Console Commands

        private void ForceRemoveQuest(string command, string[] args)
        {
            if (args?.Length < 1) return;
            Whom.removeQuest(args[0]);
        }

        private void Upgrade(string command, string[] args)
        {
            //RemoveAllMail("mtv_resetmail", null);
            //SendNewMail("mtv_sendmail", Array.Empty<string>());
        }

        private void RenewItems(string command, string[] args)
        {
            //Farmer who = Game1.player;

            ////Alternative method
            //foreach (Item v in who.Items)
            //{
            //    if (v == null || v.GetType() != typeof(sObject)) continue;

            //    sObject ve = v as sObject;

            //    if (ItemEditor.ModItems.ContainsKey(ve.Name))
            //    {
            //        ve = log.NewItem(who, ve);

            //        if (ve.Quality == 3) { ve.Quality = 2; }

            //        log.Log($"Correcting item {ve.Name}");
            //        ve.Category = ve.Name.Contains("Milk") ? -34 : -35;

            //        if (ve.Name == "Dwarf's Essence"
            //            || ve.Name == "Krobus's Essence"
            //            || ve.Name == "Wizard's Essence"
            //            || ve.Name == "Mr. Qi's Essence"
            //            || ve.Name == "Magical Essence")
            //        {
            //            v.Category = -36;
            //        }
            //    }
            //    else
            //    {
            //        log.Log($"Skipping item {v.Name}");

            //    }
            //}

        }

        /// <summary>
        /// Adds all quests from Milk The Villagers to the farmer's questslog. (uncompleted)
        /// </summary>
        /// <param name="args">[Completed]</param>
        private void AddAllQuests(string command, string[] args)
        {
            Farmer who = Game1.player;
            List<string> failed = new();
            foreach (KeyValuePair<string, int> kvp in Log.QuestIDs)
            {
                if (!kvp.Value.ToString().StartsWith("5948")) continue;

                Log.Log($"\tAdding quests {kvp.Value}-{kvp.Key}");
                try
                {
                    who.addQuest($"{kvp.Value}");
                    if (args.Length > 0 && args.Contains("completed")) who.completeQuest($"{kvp.Value}");
                }
                catch (Exception ex)
                {
                    Log.Log($"{ex.Message}");
                    failed.Add(kvp.Key); ;
                }
            }

            foreach (string s in failed)
            {
                Log.Log($"\t\tFailed to add quest {s}");
            }
        }

        private void FileCheck(string command, string[] args)
        {
            Log.Log($"Checking files", LogLevel.Trace, Force: true);

            var recipes = DataLoader.CraftingRecipes(Game1.content).Where(o => o.Key.Contains("Trunip190.CP.MilkTheVillagers."));
            var unlocked = Whom.craftingRecipes;

            List<string> folders;

            if (args.Contains("all"))
            {
                folders = Log.GetDirectories($"{Environment.CurrentDirectory}\\Mods\\Milk the Villagers");
                foreach (string s in folders)
                {
                    foreach (string f in Directory.GetFiles(s))
                    {
                        Log.Log(f.Replace(Environment.CurrentDirectory, ".."), LogLevel.Trace, Force: true);
                    }
                }

            }
            else
            {
                folders = Log.GetDirectories($"{Environment.CurrentDirectory}\\Mods\\Milk the Villagers\\[JA]MilkVillagers\\");
                foreach (string s in folders)
                {
                    foreach (string f in Directory.GetFiles(s))
                    {
                        Log.Log(f.Replace(Environment.CurrentDirectory, ".."), LogLevel.Trace, Force: true);
                    }
                }
            }


        }

        private void ResetMilk(string command, string[] args)
        {
            TempRefs.milkedtoday.Clear();
        }

        private void Friends(string command, string[] args)
        {

            int level;
            if (args.Length > 0 && int.TryParse(args[0], out int lv))
                level = lv * 250;
            else
                level = (args.Length > 0 && args.Contains("max")) ? 2500 : 1500;

            Log.Log($"Setting all villagers to {level / 250} hearts", LogLevel.Info);

            foreach (CharacterData cd in DataLoader.Characters(Game1.content).Values.ToList())//NPCGiftTastesEditor.Villagers)
            {
                int mod = level - Game1.player.getFriendshipLevelForNPC(cd.DisplayName);
                Game1.player.changeFriendship(mod, Game1.getCharacterFromName(cd.DisplayName));
            }

        }

        private void LevelUp(string command, string[] args)
        {
            int levels = int.TryParse(args?[0], out levels) ? levels : 1;

            int level = Whom.GetCustomBuffedSkillLevel(MSskill);
            int experience = Whom.GetCustomSkillExperience(MSskill);
            int increase = 0;
            int[] levelmark = MSskill.ExperienceCurve;
            foreach (int i in levelmark)
            {
                if (experience < i)
                {
                    increase = i - experience;
                    Whom.AddCustomSkillExperience(MSskill, increase);
                    experience = Whom.GetCustomSkillExperience(MSskill);
                    levels--;
                }

                if (levels < 1) break;
            }
        }
        #endregion

    }

    [HarmonyPatch(typeof(StardewValley.Item), nameof(StardewValley.Item.getCategoryName))]
    public static class ItemPatches
    {
        public static bool Applied { get; private set; } = false;
        private static IMonitor Monitor { get; set; } = (IMonitor)null;

        public static void CategoryNamePostfix(ref string __result, ref sObject __instance)
        {
            //log.Log($"mtv trying to edit getCategoryName with Harmony.", LogLevel.Info);

            switch (__instance.Category)
            {
                case -34: __result = "Breast Milk"; return;
                case -35: __result = "Human Cum"; return;
                case -36: __result = "Mystical Essence"; return;

                default: return;
            }
        }

        public static void CategoryColourPostfix(ref Framework.Color __result, ref sObject __instance)
        {
            //log.Log($"mtv trying to edit getCategoryName with Harmony.", LogLevel.Info);

            switch (__instance.Category)
            {
                case -34: __result = Framework.Color.DeepPink; return;
                case -35: __result = Framework.Color.Blue; return;
                case -36: __result = Framework.Color.Purple; return;

                default: return;
            }
        }

        public static void SpriteIndexFromRawPost(ref string item_id, ref string __result)
        {
            switch (item_id)
            {
                case "-34": __result = "Trunip190.CP.MilkTheVillagers.Woman's_Milk"; break;
                case "-35": __result = "Trunip190.CP.MilkTheVillagers.Special_Milk"; break; //"Special Milk"; break;
                case "-36": __result = "Trunip190.CP.MilkTheVillagers.Magical_Essence"; break;
            }

        }

        //public static void GetNameFromIndexPostFix(ref int index, ref string __result)
        public static void GetNameFromIndexPostFix(ref string item_id, ref string __result)
        {
            switch (item_id)
            {
                case "-34": __result = "Breast Milk (any)"; break;
                case "-35": __result = "Human Cum (any)"; break;
                case "-36": __result = "Mystical Essence (any)"; break;
            }
        }

        public static void ApplyPatch(Harmony harmony, IMonitor monitor)
        {
            if (Applied || monitor == null)
                return;

            Monitor = monitor;

            Log.Log("Applying Harmony patch to \"Item.getCategoryName\"", LogLevel.Info);
            Log.Log("Applying Harmony patch to \"Item.getCategoryColor\"", LogLevel.Info);
            Log.Log("Applying Harmony patch to \"CraftingRecipe.getNameFromIndex\"", LogLevel.Info);

            // Re-write -34, -35 & -36 category name on items.
            harmony.Patch((MethodBase)AccessTools.Method(typeof(sObject), "getCategoryName"), //Original
                null,
                new HarmonyMethod(typeof(ItemPatches), "CategoryNamePostfix", (Type[])null),
                null,
                null
                );

            // Re-write -34, -35 & -36 category colour on items.
            harmony.Patch((MethodBase)AccessTools.Method(typeof(sObject), "getCategoryColor"), //Original
                null,
                new HarmonyMethod(typeof(ItemPatches), "CategoryColourPostfix", (Type[])null),
                null,
                null
                );

            // Re-write -34, -35 & -36 category name on recipes.
            harmony.Patch((MethodBase)AccessTools.Method(typeof(CraftingRecipe), "getNameFromIndex"), //Original
                null, // Pre-fix
                new HarmonyMethod(typeof(ItemPatches), "GetNameFromIndexPostFix", (Type[])null), // Post-fix
                null, // Transpiler
                null // Injections
                );

            // Re-write -34, -35 & -36 category icon on recipes.
            harmony.Patch((MethodBase)AccessTools.Method(typeof(CraftingRecipe), "getSpriteIndexFromRawIndex"), //Original
                null, // Pre-fix
                new HarmonyMethod(typeof(ItemPatches), "SpriteIndexFromRawPost", (Type[])null), // Post-fix
                null, // Transpiler
                null // Injections
                );

            Applied = true;
        }
    }

    public class ActionNPCEventArgs : EventArgs
    {
        public Farmer Who { get; set; }
        public NPC Recipient { get; set; }
        public string Action { get; set; }
        public GameLocation Map { get; set; }
    }

    public delegate void ActionNPC(object sender, ActionNPCEventArgs e);

    internal class MilkingSkill : Skills.Skill
    {
        public static GenericProfession MTV5_Milking;
        public static GenericProfession MTV5_1_HigherQuality;
        public static GenericProfession MTV5_2_LoveGain;
        public static GenericProfession MTV10_11_MoreItems;
        public static GenericProfession MTV10_12_HighestQuality;
        public static GenericProfession MTV10_21_LessStamina;
        public static GenericProfession MTV10_22_MaxLoveGain;
        public static GenericProfession MTV10_;

        public MilkingSkill(string id) : base(id)
        {
            //this.ExperienceCurve = new[] { 100, 380, 770, 1300, 2150, 3300, 4000, 6900, 10000, 15000 };
            this.ExperienceCurve = new[] { 10, 20, 30, 40, 50, 60, 70, 80, 90, 100 };
            this.ExperienceBarColor = Framework.Color.DeepPink;

            this.Professions.Add(MTV5_Milking = new GenericProfession(this, "MTV5", "trunip190.skill.MTV5_name", "trunip190.skill.MTV5_description") { Icon = this.Icon });
            this.Professions.Add(MTV5_1_HigherQuality = new GenericProfession(this, "MTV5_1", "trunip190.skill.MTV5_1_name", "trunip190.skill.MTV5_1_description") { Icon = this.Icon });
            this.Professions.Add(MTV10_11_MoreItems = new GenericProfession(this, "MTV10_11", "trunip190.skill.MTV10_11_name", "trunip190.skill.MTV10_11_description") { Icon = this.Icon });
            this.Professions.Add(MTV10_12_HighestQuality = new GenericProfession(this, "MTV10_12", "trunip190.skill.MTV10_12_name", "trunip190.skill.MTV10_12_description") { Icon = this.Icon });
            this.Professions.Add(MTV5_2_LoveGain = new GenericProfession(this, "MTV5_2", "trunip190.skill.MTV5_2_name", "trunip190.skill.MTV5_2_description") { Icon = this.Icon });
            this.Professions.Add(MTV10_21_LessStamina = new GenericProfession(this, "MTV10_21", "trunip190.skill.MTV10_21_name", "trunip190.skill.MTV10_21_description") { Icon = this.Icon });
            this.Professions.Add(MTV10_22_MaxLoveGain = new GenericProfession(this, "MTV10_22", "trunip190.skill.MTV10_22_name", "trunip190.skill.MTV10_22_description") { Icon = this.Icon });
            this.Professions.Add(MTV10_ = new GenericProfession(this, "MTV10", "trunip190.skill.MTV10_name", "trunip190.skill.MTV10_description") { Icon = this.Icon });

            ProfessionPair pair1 = new(5, MTV5_1_HigherQuality, MTV5_2_LoveGain);
            ProfessionPair pair1_1 = new(10, MTV10_11_MoreItems, MTV10_12_HighestQuality, MTV5_1_HigherQuality);
            ProfessionPair pair1_2 = new(10, MTV10_21_LessStamina, MTV10_22_MaxLoveGain, MTV5_2_LoveGain);

            this.ProfessionsForLevels.Add(pair1);
            this.ProfessionsForLevels.Add(pair1_1);
            this.ProfessionsForLevels.Add(pair1_2);
        }

        public override string GetName()
        {
            string result = ModEntry.Instance.i18n.Get("trunip190.skill.MTV.name");
            return result;
        }

        public override string GetSkillPageHoverText(int level)
        {
            return base.GetSkillPageHoverText(level);
        }
    }

    /// <summary>Construct an instance.</summary>
    /// <param name="skill">The parent skill.</param>
    /// <param name="id">The unique profession ID.</param>
    /// <param name="name">The translated profession name.</param>
    /// <param name="description">The translated profession description.</param>
    internal class GenericProfession : Skills.Skill.Profession
    {
        /// <summary>Get the translated profession name.</summary>
        private readonly string ProfName;

        /// <summary>Get the translated profession name.</summary>
        private readonly string ProfDesc;

        /// <summary>Enables translations.</summary>
        public readonly ITranslationHelper i18n = ModEntry.Instance.i18n;

        public GenericProfession(Skills.Skill skill, string id, string name, string description)
            : base(skill, id)
        {
            this.ProfName = name;
            this.ProfDesc = description;
        }

        public override string GetName()
        {
            return i18n.Get(this.ProfName);
        }

        public override string GetDescription()
        {
            return i18n.Get(this.ProfDesc);
        }

        public override string ToString()
        {
            return ProfName;
        }
    }

    public static class MTV_App
    {
        public static IModHelper Helper;
        public static IMonitor Monitor;
        public static Texture2D appIcon;
        public static bool dragging;
        public static int yOffset;
        public static int lastMousePositionY;
        public static float listHeight;
        public static bool clicked;
        public static bool Drawn = false;
        public static bool Icons = true;

        public static IMobilePhoneApi Api;
        public static string AppID = "trunip19.MTV.Stream";
        public static List<StreamEvent> Events = new()
        {
            new StreamEvent("5948M01",WatchState.Seen),
            new StreamEvent("5948M02",WatchState.Unlocked),
            new StreamEvent("5948M03",WatchState.Unlocked),
            new StreamEvent("5948M04", WatchState.Locked)
        };
        static List<StreamEvent> CurrentEvents = new();
        internal static Dictionary<Framework.Rectangle, string> ListItemBounds = new();

        public static void MobileOpenApp()
        {
            Log.Log($"opening phone book");
            Api.SetAppRunning(true);
            //Api.phoneAppRunning = true;
            Api.SetRunningApp(AppID);
            Helper.Events.Display.RenderedWorld -= Display_RenderedWorld;
            Helper.Events.Display.RenderedWorld += Display_RenderedWorld;
            //Helper.Events.Display.RenderedWorld -= CreateEventList;
            //Helper.Events.Display.RenderedWorld += CreateEventList;
            Helper.Events.Input.ButtonPressed -= Input_ButtonPressed;
            Helper.Events.Input.ButtonPressed += Input_ButtonPressed;

            //Helper.Events.Input.MouseWheelScrolled += Input_MouseWheelScrolled;

            CreateEventList();
        }

        public static void OpenStreamList()
        {
        }
        private static void Input_ButtonPressed(object sender, StardewModdingAPI.Events.ButtonPressedEventArgs e)
        {

            if (Game1.eventUp || !Api.GetAppRunning() || Api.GetRunningApp() != AppID || !Api.GetScreenRectangle().Contains(Game1.getMousePosition()))
                return;

            if (e.Button == SButton.MouseLeft)
            {
                Helper.Input.Suppress(SButton.MouseLeft);

                clicked = true;

                lastMousePositionY = Game1.getMouseY();
            }
        }

        public static void CreateEventList()
        {
            bool appRunning = Api.GetAppRunning();
            bool phoneOpened = Api.GetPhoneOpened();
            string runningApp = Api.GetRunningApp();
            if (!appRunning || !phoneOpened || runningApp != AppID)
            {
                Api.SetAppRunning(false);
                Api.SetRunningApp(null);
                Helper.Events.Display.RenderedWorld -= Display_RenderedWorld;
                Helper.Events.Input.ButtonPressed -= Input_ButtonPressed;
                //Helper.Events.Input.MouseWheelScrolled -= Input_MouseWheelScrolled;
                return;
            }

            Framework.Vector2 screenPos = Api.GetScreenPosition();
            Framework.Vector2 screenSize = Api.GetScreenSize();
            Framework.Rectangle headerRect = new Framework.Rectangle((int)screenPos.X, (int)screenPos.Y, (int)screenSize.X, (int)screenSize.Y);
            Point mousePos = Game1.getMousePosition();

            CurrentEvents.Clear();
            //int offset = 0; // vertical offset
            foreach (var Eve in Events.Where(o => o.State == WatchState.Unlocked || o.State == WatchState.Seen).OrderBy(o => o.EventID))
            {
                //Game1.spriteBatch.DrawString(Game1.dialogueFont, "sfdsdfsdfsdf", new Framework.Vector2(screenPos.X, screenPos.Y - offset), Color.White);
                //Eve.Location = new Framework.Vector2(screenPos.X, screenPos.Y - offset);
                CurrentEvents.Add(Eve);
                //offset += 30;
            }

            Drawn = false;
        }

        private static void Display_RenderedWorld(object sender, StardewModdingAPI.Events.RenderedWorldEventArgs e)
        {

            if (Game1.eventUp || !Api.GetAppRunning() || Api.GetRunningApp() != AppID)
                return;

            //if (Drawn) return;
            //Drawn = true;

            Framework.Vector2 screenPos = Api.GetScreenPosition();
            Framework.Vector2 screenSize = Api.GetScreenSize();
            int xOffset = Api.GetPhoneRotated() ? 10 : 5;
            int height = Icons ? 64 : 40;
            Framework.Rectangle headerRect = new Framework.Rectangle((int)screenPos.X, (int)screenPos.Y, (int)screenSize.X, (int)screenSize.Y);


            for (int i = 0; i < CurrentEvents.Count; i++)
            {
                var v = CurrentEvents[i];
                v.Area.X = (int)screenPos.X + xOffset;
                v.Area.Y = (int)screenPos.Y + (i * height);
                v.Area.Width = v.Area.Height = height;

                if (Icons)
                {
                    int PerRow = Api.GetPhoneRectangle().Width / height;
                    if (i % PerRow < PerRow)
                    {
                        v.Area.X = (int)screenPos.X + (height * (i % PerRow));
                        v.Area.Y = (int)screenPos.Y + (height * (i / PerRow));

                    }
                    else
                    { // Needs fixing
                        v.Area.X = (int)screenPos.X;
                        v.Area.Y = (int)screenPos.Y;
                    }

                    // Make npc portrait bottom right of icon.
                    Framework.Rectangle StreamIconArea = v.Area;
                    v.Area.X += height / 2;
                    v.Area.Width /= 2;
                    v.Area.Y += height / 2;
                    v.Area.Height /= 2;

                    // Set colour of icon to use
                    string fileLoc = v.State == WatchState.Locked ? "StreamIconB.png" : v.State == WatchState.Unlocked ? "StreamIconG.png" : "StreamIconY.png";
                    Texture2D appIcon = Helper.ModContent.Load<Texture2D>(Path.Combine("assets", fileLoc));

                    Texture2D portraits = Helper.GameContent.Load<Texture2D>("Portraits/Abigail");
                    portraits.SetContentSize(64, 64); // Set area to draw.
                    e.SpriteBatch.Draw(appIcon, StreamIconArea, Color.White);
                    e.SpriteBatch.Draw(portraits, v.Area, Color.White);
                }
                else
                {
                    Color ColourAlt = v.State != WatchState.Seen ? Color.White : Color.Gray;
                    e.SpriteBatch.DrawString(Game1.dialogueFont, v.EventID, v.Location, ColourAlt);
                    v.Area.X = (int)v.Location.X;
                    v.Area.Y = (int)v.Location.Y;
                    v.Area.Width = Api.GetScreenRectangle().Width;
                    v.Area.Height = height;
                }
            }

            if (clicked)
            {
                clicked = false;
                Point mousePos = Game1.getMousePosition();
                if (headerRect.Contains(mousePos))
                {
                    var option = CurrentEvents.Where(o => o.Area.Contains(mousePos));
                    if (option.Count() > 0)
                    {
                        var loc = Game1.locations.Where(o => o.Name == "FarmHouse").First();
                        var eve = loc.findEventById(option.First().EventID);
                        if (eve != null)
                        {
                            loc.startEvent(eve);
                            Api.SetAppRunning(false);
                        }
                    }
                }
            }

        }
    }

    public class StreamEvent
    {
        public string EventID;
        public string EventName = "Name of event";
        public WatchState State = WatchState.Unlocked;
        public Framework.Vector2 Location => new Framework.Vector2(Area.X, Area.Y);
        public Framework.Rectangle Area;

        public StreamEvent(string ID)
        {
            EventID = ID;
        }
        public StreamEvent(string ID, WatchState state)
        {
            EventID = ID;
            State = state;
        }
    }

    public class MailCheck
    {
        public readonly string FinishedMail;
        public readonly string NewMail;
        public readonly string NPCname;
        public readonly int HeartRequired;

        public MailCheck(string finishedMail, string newMail, string nPCname, int heartRequired)
        {
            FinishedMail = finishedMail;
            NewMail = newMail;
            NPCname = nPCname;
            HeartRequired = heartRequired;
        }

        public override string ToString() => NewMail;
    }

    public enum WatchState
    {
        Locked,
        Unlocked,
        Seen
    }
}
