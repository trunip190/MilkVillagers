using MilkVillagers.Asset_Editors;
using SpaceCore.Events;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Objects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using IGenericModConfigMenuApi = GenericModConfigMenu.IGenericModConfigMenuApi;
using ContentPatcher;
using sObject = StardewValley.Object;
using log = MilkVillagers.ModFunctions;
using HarmonyLib;
using Microsoft.Xna.Framework;
using System.Reflection;
using StardewValley.Quests;
using SpaceShared.APIs;
using StardewValley.GameData.Objects;
using Netcode;
using StardewValley.Network;
using StardewValley.GameData.Characters;
using SpaceCore;
using System.Diagnostics;

namespace MilkVillagers
{
    public class ModEntry : Mod
    {
        #region class variables
        private int[] target;
        private bool loaded;
        private bool running;
        private bool runOnce;
        private bool MessageOnce;
        private NPC currentTarget;
        ContentPatcher.IContentPatcherAPI contentPatcherApi;

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
            {"sex" , 50 },
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

        private bool doneOnce = false; //remove when not testing.

        // Time based stuff
        private float RegenAmount = 0;

        private float TimeFreezeTimer = 0;
        private bool TimeFreeze = false; // Debug time freeze.

        private bool TentacleArmour;
        private bool TentacleCombination = false; // Force-equips the other part of the tentacle outfit.
        private float TentacleCount = 0;
        private float TentacleLimit = 240; // Four ingame hours.

        // Adding item stuff
        sObject AddItem;
        Farmer CurrentFarmer;

        private ModConfig Config;
        List<int> CurrentQuests = new();

        public event ActionNPC OnActionNPC;
        #endregion

        public override void Entry(IModHelper helper)
        {
            Config = helper.ReadConfig<ModConfig>();
            DialogueEditor.ExtraContent = Config.ExtraDialogue;
            TempRefs.Monitor = Monitor;

            #region Harmony setup
            var harmony = new Harmony(this.ModManifest.UniqueID);

            try
            {
                ItemPatches.ApplyPatch(harmony, this.Monitor);
            }
            catch (Exception ex)
            {
                log.Log(ex.Message, LogLevel.Alert, Force: true);
            }
            #endregion

            if (helper == null)
            {
                log.Log("helper is null.", LogLevel.Error);
            }
            else
            {
                TempRefs.Helper = helper;
            }

            #region register in-game events

            Helper.Events.Display.MenuChanged += Display_MenuChanged;
            Helper.Events.GameLoop.DayStarted += GameLoop_DayStarted;
            Helper.Events.GameLoop.GameLaunched += OnGameLaunched;
            Helper.Events.GameLoop.SaveLoaded += GameLoop_SaveLoaded;
            Helper.Events.GameLoop.DayEnding += GameLoop_DayEnding;
            Helper.Events.GameLoop.OneSecondUpdateTicked += GameLoop_OneSecondUpdateTicked;
            Helper.Events.GameLoop.TimeChanged += GameLoop_TimeChanged;
            Helper.Events.Input.ButtonPressed += OnButtonPressed;

            // SpaceCore events
            SpaceEvents.BeforeGiftGiven += SpaceEvents_BeforeGiftGiven;
            SpaceEvents.OnItemEaten += SpaceEvents_OnItemEaten;

            // My events
            this.OnActionNPC += CheckQuestActionOnNPC;

            #endregion

            #region Add in console commands
            helper.ConsoleCommands.Add("mtv_addquests", "Adds all quests to the farmer's questlog\n\tUsage: mtv_addquests <completed> <list>\n\t- completed: auto complete quests as they are added.\n\t- list: prints every vilager changed.", this.AddAllQuests);
            helper.ConsoleCommands.Add("mtv_checkfiles", "checks your computer to see if the modfiles are in the right location\n\nUsage: mtv_checkfiles <all>\n\t- all: Checks all files in folders recursively. Only checks JA folder by default.", this.FileCheck);
            helper.ConsoleCommands.Add("mtv_dumpDialogue", "Output all dialogue for specified villager\n\nUsage: mtv_dumpDialogue <VillagerName> <bare>\n\t- VillagerName: Needs to be the full name of the villager.\n\t- bare: only lists topic names.", this.DumpDialogue);
            helper.ConsoleCommands.Add("mtv_dumpItems", "Outputs all items to log.\n\nUsage: mtv_dumpItems <init>\n\t- init: Set up items, cooking and crafting.", this.CheckItemCodes);
            helper.ConsoleCommands.Add("mtv_dumpquests", "Dumps all quests for translation\n\nmtv_dumpquests [i18n][raw][special]\n\ni18n: append the i18n translations.\nraw: leave data untranslated, with tokens in place.\nspecial: handles Special Orders instead.", this.DumpQuests);
            helper.ConsoleCommands.Add("mtv_friends", "Sets all vanilla NPC's friends to either half or max\n\nmtv_friends <max>\n\t- max: set to 10 hearts. Default is 6 hearts.", this.Friends);
            helper.ConsoleCommands.Add("mtv_forceremovequest", "Remove specified quest from the farmer's questlog\n\nUsage: mtv_frq <value>\n- value: the quest id to remove from the questlog.", this.ForceRemoveQuest);
            helper.ConsoleCommands.Add("mtv_reportRecipes", "Output debug info on all MTV recipes.\n\nUsage: mtv_reportRecipes", this.ReportRecipes);
            helper.ConsoleCommands.Add("mtv_renewItems", "Update items in farmer's inventory to newer versions after upgrade.\n\nUsage: mtv_renewItems", this.RenewItems);
            helper.ConsoleCommands.Add("mtv_resetmail", "Removes all mail flags\n\nUsage: mtv_resetmail <value>\n- value: the farmer to remove mail from.", this.RemoveAllMail);
            helper.ConsoleCommands.Add("mtv_resetmilk", "Resets the list of NPC's that have been milked today, allowing you to collect from them again\n\nUsage: mtv_resetmilk", this.ResetMilk);
            helper.ConsoleCommands.Add("mtv_sendallmail", "Sends every mail item from this mod, even previously sent ones\n\nUsage:mtv_sendallmail", this.SendAllMail);
            helper.ConsoleCommands.Add("mtv_sendmail", "Send the next mail item if conditions are met\n\nUsage: mtv_sendmail <value>\n- value: the farmer to send mail to.", this.SendNewMail);
            helper.ConsoleCommands.Add("mtv_upgrade", "Force the mod to reset quest and mail flags when you have upgraded the mod\n\nUsage: mtv_upgrade", this.Upgrade);
            #endregion
        }

        #region Game OnEvent Triggers
        private void OnGameLaunched(object sender, GameLaunchedEventArgs e)
        {
            UpdateConfig();

            LinkPhone();

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
            Helper.Events.Content.AssetRequested += Content_AssetRequested;
            Helper.Events.Content.AssetReady += Content_AssetReady;

            TempRefs.thirdParty = Config.ThirdParty;
            TempRefs.Verbose = Config.Verbose;
            TempRefs.OverrideGenitals = Config.OverrideGenitals;
            //TempRefs.HasPenis = Config.HasPenis;
            //TempRefs.HasVagina = Config.HasVagina;
            //TempRefs.HasBreasts = Config.HasBreasts;
            TempRefs.IgnoreVillagerGender = Config.IgnoreVillagerGender;
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
                setValue: (SButton var) => this.Config.MilkButton = var
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
            string[] genders = new string[] { "Save default", "Male", "Female", "A-sexual" }; //, "Intersex", "Genderfluid" }; need to work this in.
            string[] genitals = new string[] { "Penis", "Vagina and breasts", "Vagina", "Vagina and Penis", "Penis and breasts", "Penis, Vagina, Breasts", "Breasts", "None" };

            //configMenu.AddBoolOption(
            //    fieldId: "SaveFileGender",
            //    mod: this.ModManifest,
            //    name: () => "Farmer is male?",
            //    tooltip: () => "Does the game think you are male?",
            //    getValue: () => Game1.player.IsMale,
            //    setValue: value => Game1.player.obsolete_isMale = value
            //    );

            //configMenu.AddTextOption(
            //    fieldId: "Gender",
            //    mod: this.ModManifest,
            //    name: () => "Gender",
            //    tooltip: () => "Gender identity of your farmer.",
            //    allowedValues: genders,
            //    getValue: () => this.Config.FarmerGender,
            //    setValue: value => Config.FarmerGender = value
            //    );

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

                case "SaveFileGender": Game1.player.changeGender((bool)arg2); UpdateGenitalConfig(arg1, $"{arg2}"); SendGenitalMail(Game1.player); log.Log($"{Game1.player.displayName} is now male? {Game1.player.IsMale}", LogLevel.Info, Force: true); break;
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

                default: log.Log($"{arg1}", LogLevel.Alert, Force: true); break;
            }

            #region Update token
            string GenderSwitch = "GenderSwitch {{Trunip190.MilkTheVillagers/GenderSwitch}} {{trunip190.CP.MilkTheVillagers/GenderSwitch}} The current time is {{Time}} on {{Season}} {{Day}}, year {{Year}}";
            //if (Game1.player.hasOrWillReceiveMail("MTV_Vagina")) GenderSwitch = "V";
            //if (Game1.player.hasOrWillReceiveMail("MTV_Herm")) GenderSwitch = "H";
            //if (Game1.player.hasOrWillReceiveMail("MTV_Ace")) GenderSwitch = "A";
            //if (Game1.player.hasOrWillReceiveMail("MTV_Penis")) GenderSwitch = "P";

            IManagedTokenString tokenString = contentPatcherApi.ParseTokenString(
               manifest: this.ModManifest,
               rawValue: GenderSwitch,
               formatVersion: new SemanticVersion("2.1.0"),
               assumeModIds: new[] { $"trunip190.CP.MilkTheVillagers" }
            );



            tokenString.UpdateContext();
            string value = tokenString.Value; // The current time is 1430 on Spring 5, year 2.
            #endregion
        }

        private void UpdateGenitalConfig(string option, string value)
        {
            log.Log($"{option} {value}", Force: true);
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
                Game1.addHUDMessage(new HUDMessage(log.GetFarmerString("TimeSlows")));

                TimeFreezeTimer += 200000; // 1 minute timestop.
            }
            else if (who.itemToEat.ItemId == "Trunip190.CP.MilkTheVillagers.Eldritch_Energy")
            {
                Game1.addHUDMessage(new HUDMessage("You feel energised"));
                RegenAmount = 1000; // 100 energy total restoration.

            }
            else if (who.itemToEat.ItemId == "Trunip190.CP.MilkTheVillagers.Shibari_rope_package")
            {
                Game1.addHUDMessage(new HUDMessage("ruh roh"));
                who.isEating = false;
            }
       
        
        }

        private void SpaceEvents_BeforeGiftGiven(object sender, EventArgsBeforeReceiveObject e)
        {
            Farmer who = Game1.player;
            string ItemGiven = e.Gift.ItemId;
            NPC npcTarget = e.Npc;


            if (ItemGiven == "Trunip190.CP.MilkTheVillagers.Readi_Milk")
            {
                e.Cancel = true;
                if (!Config.Quests)
                {
                    Dialogue v = log.ProcessDialogue(new Dialogue(null, "Strings/StringsFromCSFiles:QuestsOff"));
                    Game1.addHUDMessage(new HUDMessage(v.getCurrentDialogue()));
                    DrawDialogue(npcTarget, action: "QuestStartFail");

                    goto cleanup;
                }

                if (!MailEditor.FirstMail.ContainsKey(npcTarget.Name))
                {

                    if (npcTarget.Dialogue.TryGetValue("QuestStartFail", out string FailDialogue))
                        DrawDialogue(npcTarget, action: "QuestStartFail");

                    goto cleanup;
                }

                string FirstQuestMail = MailEditor.FirstMail[npcTarget.Name];
                if (FirstQuestMail != "" && !who.hasOrWillReceiveMail(FirstQuestMail))
                {
                    if (npcTarget.Dialogue.TryGetValue("QuestStartSuccess", out string SuccessDialogue))
                    {
                        DrawDialogue(npcTarget, message: SuccessDialogue, action: "QuestStartSuccess");
                    }
                    else
                    {
                        DrawDialogue(npcTarget, message: "quests not found?", action: "QuestStartFail");
                    }
                    SendNextMail(who, FirstQuestMail, true);

                    goto cleanup;
                }
            }

            if (ItemGiven == "Trunip190.CP.MilkTheVillagers.Discrete_Package")
            {
                if (npcTarget.Name != "Penny")
                {
                    DrawDialogue(npcTarget, "Thats not my package. Maybe ask around the village?", action: "MTV_Parcel");
                    e.Cancel = true;
                    goto cleanup;
                }

                foreach (LostItemQuest qid in who.questLog.Where(o => o.id.Value == $"{594826}"))
                {
                    qid.npcName.Set("Penny");
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

            /// stop debug button working if player ID isn't one of mine.
            if (button == SButton.P && Config.Debug && who.UniqueMultiplayerID != -9150291597868197960 && who.UniqueMultiplayerID != 1404513575115831020 && who.UniqueMultiplayerID != -1887059294522725934)
            {
                log.Log($"{who.UniqueMultiplayerID} tried to use cheats", LogLevel.Alert);
            }
            else if (button == SButton.P && Config.Debug)
            {
                CheckAll();

                log.Log($"{who.currentLocation.Name}, {who.GetGrabTile().X}, {who.GetGrabTile().Y}");

                
                if (TimeFreeze) { TimeFreeze = false; TimeFreezeTimer = 0; Game1.addHUDMessage(new HUDMessage("Time flows again")); }
                else { TimeFreeze = true; TimeFreezeTimer = 1000; Game1.addHUDMessage(new HUDMessage("Time is frozen")); }

                // for release - skip below.
                //return;

                /// Press P on load to trigger. for repeated testing only.
                ///
                ///
                if (!doneOnce)
                {
                    doneOnce = true;

                    //who.addQuest(594827);
                    who.mailbox.Add("MilkButton1");
                    //who.changeFriendship(1750, Game1.getCharacterFromName("Penny"));
                    //who.changeFriendship(1250, Game1.getCharacterFromName("Alex"));
                    string npcToAdd = "Harvey";
                    NetStringDictionary<Friendship, NetRef<Friendship>> data = who.friendshipData;
                    if (data.FieldDict.ContainsKey(npcToAdd)) data.FieldDict[npcToAdd].Value = new Friendship(2000);
                    else
                    {
                        data.FieldDict.Add(npcToAdd, new Netcode.NetRef<Friendship>(new Friendship(2000)));
                    }
                    //who.changeFriendship(2000, Game1.getCharacterFromName("Abigail"));
                    Game1.timeOfDay = 1400;


                    //Game1.warpFarmer("Farm", 69, 16, false);
                    //Clothing shibari = ClothingEditor.getShirt($"Tentacle Armour Torso");
                    //who.addItemByMenuIfNecessary(shibari);

                }
                else
                {
                    //SendGenitalMail(who);

                    //sObject invitation = new(TempRefs.Invitation, 7);
                    //who.addItemToInventory(invitation);

                    //Game1.warpFarmer("Town", 36, 56, false);
                    //Game1.player.activeDialogueEvents.Add("MTV_BoethiaBook", 1);

                    //log.Log($"{who.GetGrabTile().X} {who.GetGrabTile().Y}", LogLevel.Alert);

                    List<Response> choices = new List<Response> {
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

                    SendNewMail("mtv_sendmail", Array.Empty<string>());
                    //Game1.warpFarmer("Forest", 104, 34, false);
                    //Game1.warpFarmer("Mountain", 10, 4, false);
                    foreach (string v in who.mailForTomorrow)
                    {
                        log.Log($"rushing mail item {v}", LogLevel.Trace);
                        who.mailbox.Add(v);
                    }
                    who.mailForTomorrow.Clear();
                }

            }
            else if (button == Config.MilkButton)
            {
                target = GetNewPos(who.FacingDirection, FarmerPos(who)[0], FarmerPos(who)[1]);
                NPC NPCtarget = log.FindTarget(who.currentLocation, this.target, FarmerPos(who));
                Farmer companion = log.FindFarmer(who.currentLocation, this.target, FarmerPos(who));
                if (NPCtarget != null)
                {
                    List<Response> choices = GenerateSexOptions(NPCtarget); // Get list of available options for this character
                    log.Log($"{NPCtarget.Name}: gendercode = {NPCtarget.Gender}, age = {NPCtarget.Age}", LogLevel.Alert);

                    currentTarget = NPCtarget;
                    running = false;

                    Game1.currentLocation.createQuestionDialogue($"What do you want to do with {NPCtarget.Name}?", choices.ToArray(), new GameLocation.afterQuestionBehavior(DialoguesSet));
                }
                else if (companion != null && companion != who)
                {
                    Game1.addHUDMessage(new HUDMessage($"What do you want to do with {companion.displayName}?"));
                }
                else  //Config.Debug)
                {
                    List<Response> options = new();

                    // TODO change to energy based or time based
                    if (GetPenis(who) && who.Stamina > 150)
                    {
                        options.Add(new Response("self_cum", "Collect your own cum")); //collect own cum
                    }

                    if (GetBreasts(who) && who.stamina > 150)// && !TempRefs.SelfMilkedToday)
                    {
                        options.Add(new Response("self_milk", "Milk yourself")); //collect own breastmilk
                    }

                    // TODO not showing up?
                    if (false && who.Items.CountId("Trunip190.CP.MilkTheVillagers.Mr._Qi's_Essence") > 0)
                    {
                        options.Add(new Response("time_freeze", "Freeze time"));
                    }

                    // TODO not showing up?
                    if (false && who.Items.CountId("Trunip190.CP.MilkTheVillagers.Eldritch_Energy") > 0)
                    {
                        options.Add(new Response("stamina_regen", "Drink Eldritch Energy"));
                    }

                    if (options.Count > 0)
                    {
                        options.Add(new Response("abort", "Nothing"));

                        Game1.currentLocation.createQuestionDialogue($"What would you like to do?", options.ToArray(), new GameLocation.afterQuestionBehavior(DialoguesSet));
                    }
                    else
                    {
                        Game1.addHUDMessage(new HUDMessage("You're too tired to do anything else right now"));
                    }
                }
            }
        }

        private void GameLoop_SaveLoaded(object sender, SaveLoadedEventArgs e)
        {
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

            GetItemCodes();

            foreach (Farmer who in Game1.getAllFarmers())
            {
                log.Log($"Sending mail to {who.displayName}", LogLevel.Trace);
                CorrectRecipes();
                AddAllRecipes(who);
                SendGenitalMail(who);
            }

            //log.Log($"{MFM.ContentPack.Skill.Mining}");
            //Game1.player.setSkillLevel("Milking Skill", 2);

            runOnce = true;
        }

        private void GameLoop_DayStarted(object sender, DayStartedEventArgs e)
        {
            if (TempRefs.milkedtoday == null) log.Log("TempRefs not set");
            else
            {
                log.Log($"TempRefs is set. Cleared {TempRefs.milkedtoday.Count}");
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
            }
            //Farmer who = Game1.player;
            //SendNewMail(who);
            TimeFreezeTimer = 0;
            RegenAmount = 0;
        }

        private void GameLoop_OneSecondUpdateTicked(object sender, OneSecondUpdateTickedEventArgs e)
        {
            if (!loaded) return;

            Farmer Who = Game1.player; //TODO change to multiplayer
            if (Game1.dialogueUp) return;

            SendNewMail(Who);
            QuestsCompletedByMail(Who);
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
                    Game1.addHUDMessage(new HUDMessage("Time speeds back up again"));
            }

            //TODO change for multiplayer
            foreach (Farmer who in Game1.getOnlineFarmers())
            {
                //Don't check anything if they haven't received the gender mail
                if (!who.mailReceived.Contains("MTV_Vagina") &&
                    !who.mailReceived.Contains("MTV_Penis") &&
                    !who.mailReceived.Contains("MTV_Ace") &&
                    !who.mailReceived.Contains("MTV_Herm"))
                {
                    log.Log("Skipping quest watching");
                    return;
                }

                QuestChecks(who);
                SendMailCompleted(who);
            }
        }

        private void GameLoop_TimeChanged(object sender, TimeChangedEventArgs e)
        {
            Farmer who = Game1.player;
            int diff;

            if (TentacleArmour ||
                (who.shirtItem.Value != null && who.shirtItem.Value.Name.ToLower() == "tentacle armour torso") ||
                (who.pantsItem.Value != null && who.pantsItem.Value.Name.ToLower() == "tentacle armour lower"))
            {
                if (TentacleCombination) // force equip top/bottom. Should be moved to ontimeupdate.
                {
                    if (who.pantsItem.Value == null || who.pantsItem.Value.Name.ToLower() != "tentacle armour lower")
                    {
                        Clothing tents = new("Tentacle Armour Lower");
                        if (who.pantsItem.Value != null)
                        {
                            Clothing old = who.pantsItem.Value;
                            who.removeItemFromInventory(old);
                            who.addItemToInventory(old);
                        }
                        who.pantsItem.Value = tents;
                    }
                    if (who.shirtItem.Value == null || who.shirtItem.Value.Name.ToLower() != "tentacle armour torso")
                    {
                        Clothing tents = new("Tentacle Armour Torso");
                        if (who.shirtItem.Value != null)
                        {
                            Clothing old = who.shirtItem.Value;
                            who.removeItemFromInventory(old);
                            who.addItemToInventory(old);
                        }
                        who.shirtItem.Value = tents;

                    }
                }

                if (e.NewTime < e.OldTime) //Slept. reset and return;
                {
                    TentacleCount = 0;
                    return;
                }

                diff = e.NewTime - e.OldTime;
                TentacleCount += diff > 10 ? 10 : diff;
                log.Log($"Ten: {TentacleCount}", LogLevel.Info, Force: true);


                if (TentacleCount > TentacleLimit)
                {
                    Game1.addHUDMessage(new HUDMessage($"The tentacles make you climax: {TentacleCount}"));
                    who.movementPause = 400;
                    who.health = who.maxHealth;
                    TentacleCount = 0;
                }
                //if (TentacleCount == (TentacleCount / 4 * 3))
                if (TentacleCount == 180)
                {
                    Game1.addHUDMessage(new HUDMessage($"The tentacles have you right on the edge of cumming: {TentacleCount}"));
                    who.health = who.health + 75 > who.maxHealth ? who.maxHealth : who.health + 75;
                }
                else if (TentacleCount == 120)
                {
                    Game1.addHUDMessage(new HUDMessage($"The tentacles are making it harder to think: {TentacleCount}"));
                    who.health = who.health + 50 > who.maxHealth ? who.maxHealth : who.health + 50;
                }
                else if (TentacleCount == 60)
                {
                    if (Config.HasPenis)
                        Game1.addHUDMessage(new HUDMessage($"The tentacles start rubbing up and down your shaft: {TentacleCount}"));
                    else if (Config.HasVagina)
                        Game1.addHUDMessage(new HUDMessage($"The tentacles start probing your entrance: {TentacleCount}"));
                    else
                    {
                        Game1.addHUDMessage(new HUDMessage($"The tentacles start rubbing your crotch, unsure what to do: {TentacleCount}"));
                    }

                    who.health = who.health + 25 > who.maxHealth ? who.maxHealth : who.health + 25;
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

                QuestChecks(who);
                SendMailCompleted(who);

                log.Log("removing repeatable events from seenEvents", LogLevel.Trace);
                who.eventsSeen.Remove("5948121");
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
                CurrentFarmer.addItemToInventory(AddItem);
                AddItem = null;
                CurrentFarmer = null;
            }
        }

        [EventPriority(EventPriority.Low)]
        private void Content_AssetRequested(object sender, AssetRequestedEventArgs e)
        {
            if (DialogueEditor.CanEdit(e.Name)) { e.Edit(DialogueEditor.Edit); return; }
            if (QuestEditor.CanEdit(e.Name)) { e.Edit(QuestEditor.Edit); return; }
            if (RecipeEditor.CanEdit(e.Name)) { e.Edit(RecipeEditor.Edit); return; }
            if (EventEditor.CanEdit(e.Name)) { e.Edit(EventEditor.Edit); return; }
            if (MailEditor.CanEdit(e.Name)) { e.Edit(MailEditor.Edit); return; }
            if (ItemEditor.CanEdit(e.Name)) { e.Edit(ItemEditor.Edit); return; }
            if (ClothingEditor.CanEdit(e.Name)) { e.Edit(ClothingEditor.Edit); return; }
            if (ObjectEditor.CanEdit(e.Name)) { e.Edit(ObjectEditor.Edit); return; }
        }

        private void Content_AssetReady(object sender, AssetReadyEventArgs e)
        {
            if (e.Name.IsEquivalentTo("Data/mail")) { MailEditor.UpdateData(Helper.GameContent.Load<Dictionary<string, string>>(e.Name)); }
            if (e.Name.IsEquivalentTo("Data/Objects")) { ItemEditor.UpdateData(Helper.GameContent.Load<Dictionary<string, ObjectData>>(e.Name)); }
            if (e.Name.IsEquivalentTo("Data/Quests")) { QuestEditor.UpdateData(Helper.GameContent.Load<Dictionary<string, string>>(e.Name)); }
            if (e.Name.IsEquivalentTo("Data/CookingRecipes")) { RecipeEditor.UpdateCookingData(Helper.GameContent.Load<Dictionary<string, string>>(e.Name)); }
            if (e.Name.IsEquivalentTo("Data/CraftingRecipes")) { RecipeEditor.UpdateCraftingData(Helper.GameContent.Load<Dictionary<string, string>>(e.Name)); }
            if (NPCGiftTastesEditor.CanEdit(e.Name)) { NPCGiftTastesEditor.UpdateData(Helper.GameContent.Load<Dictionary<string, string>>(e.Name)); }
            if (e.Name.IsEquivalentTo("Data/Objects")) { ObjectEditor.UpdateData(Helper.GameContent.Load<Dictionary<string, ObjectData>>(e.Name)); };

        }

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

            //Emily
            CheckCompleteQuest(Who, "594818", "MTV_EmilyQ2P");

            //Penny
            CheckCompleteQuest(Who, "594825", "MTV_PennyQ1P"); //auto leads into part two. standard completion in part 2
            CheckCompleteQuest(Who, "594827", "MTV_PennyQ3P");
            CheckCompleteQuest(Who, "5948272", "MTV_PennyQ3T");

            //Leah
            CheckCompleteQuest(Who, "594829", "MTV_LeahQ1P");
            CheckCompleteQuest(Who, "594831", "MTV_LeahQ3T");
            CheckCompleteQuest(Who, "5948322", "MTV_LeahQ4T");

            //CheckCompleteQuest(Who, "594839", "MTV_HarveyQ3T");
            CheckCompleteQuest(Who, "594840", "MTV_HarveyQ4T");
        }

        /// <summary>
        /// Checks for mail item to determine if quest should be completed.
        /// </summary>
        /// <param name="Who"></param>
        /// <param name="questID"></param>
        /// <param name="MailName"></param>
        private static void CheckCompleteQuest(Farmer Who, string questID, string MailName)
        {
            if (Who == null) { log.Log("ERROR: Who was null"); return; }
            if (Who.hasOrWillReceiveMail(MailName))
                //if (!Who.mailReceived.Contains(MailName)) return;
                Who.completeQuest(questID);
        }

        private void QuestChecks(Farmer who)
        {
            foreach (KeyValuePair<string, int> kvp in QuestEditor.QuestIDs)
            {
                CheckNewQuest(who, kvp.Value);
            }
        }

        private bool CheckNewQuest(Farmer who, int questID)
        {
            if (!CurrentQuests.Contains(questID) && HasQuest(who, questID))
            {
                if (questID == 594824)
                {
                    log.Log("Setting ActiveConversationEvent to MTV_Bukkake", LogLevel.Info);
                    Game1.player.activeDialogueEvents.Add("MTV_Bukkake", 1);
                }
                if (questID == 594834)
                {
                    log.Log("Adding ConversationTopic", LogLevel.Info);
                    Game1.player.activeDialogueEvents.Add("HaleyPanties", 0);

                    // force the game to pick the right dialogue when married to Haley.
                    NPC haley = Game1.getCharacterFromName("Haley");
                    Game1.getCharacterFromName("Haley").resetCurrentDialogue();

                    bool check = haley.Dialogue.TryGetValue("HaleyPanties", out string dialogues);

                    log.Log($"{haley.Name} {check} {dialogues}", LogLevel.Info);
                    haley.setNewDialogue(dialogues);
                }
                if (questID == 594836)
                {
                    log.Log("Setting ActiveConversationEvent to MTV_GeorgeQ4", LogLevel.Info);
                    Game1.player.activeDialogueEvents.Add("MTV_GeorgeQ4", 0);
                }
                if (questID == 594819)
                {
                    if (!who.knowsRecipe("Crotchless Panties"))
                    {
                        who.craftingRecipes["Crotchless Panties"] = 0; // Crafting name, then times crafted (0)
                    }
                }
                if (questID == 594826)
                {
                    log.Log("Setting ActiveConversationEvent to MTV_Parcel", LogLevel.Info);
                    who.activeDialogueEvents.Add("MTV_Parcel", 1);
                }

                CurrentQuests.Add(questID);
                log.Log($"Watching quest ID {questID}", LogLevel.Trace);
                return true;
            }

            return false;
        }

        private static Dictionary<string, int> GetAllIndexes(Farmer who)
        {
            Dictionary<string, int> result = new();

            for (int i = 0; i < who.questLog.Count; i++)
            {
                Quest q = who.questLog[i];

                result[q.id.Value] = i;
            }

            return result;
        }

        private static bool HasQuest(Farmer who, int id)
        {
            foreach (var q in who.questLog)
            {
                if (q.id.Value == $"{id}")
                {
                    if (q.completed.Value)
                    {
                        log.Log($"Quest {q.GetName()} is complete.", LogLevel.Trace);
                        return false;
                    }
                    return true;
                }
            }
            return false;
        }

        private static bool CheckActionQuest(Farmer who, string QuestID, string Recipient, string[] ActionRequired, ActionNPCEventArgs e)
        {
            //if (who.hasQuest(QuestID) && Recipient == e.recipient.Name && ActionRequired.Contains(e.action))
            //{
            //    log.LogVerbose($"Completing quest {QuestID}", LogLevel.Alert);
            //    who.completeQuest(QuestID);
            //    return true;
            //}

            // Specific Name
            if (who.hasQuest(QuestID) &&
                (Recipient == e.Recipient.Name) &&
                ActionRequired.Contains(e.Action))
            {
                log.Log($"Completing quest {QuestID}", LogLevel.Trace);
                who.completeQuest(QuestID);
                return true;
            }

            // Male or Female
            if (who.hasQuest(QuestID) &&
                ((Recipient == "Male" && e.Recipient.Gender == 0) || (Recipient == "Female" && e.Recipient.Gender == Gender.Female)) &&
                ActionRequired.Contains(e.Action))
            {
                log.Log($"Completing quest {QuestID}", LogLevel.Trace);
                who.completeQuest(QuestID);
                return true;
            }

            // Magical
            if (who.hasQuest(QuestID) &&
                (Recipient == "Magical" && (e.Recipient.Name == "Dwarf" || e.Recipient.Name == "Wizard" || e.Recipient.Name == "Mister Qi" || e.Recipient.Name == "Krobus")) &&
                ActionRequired.Contains(e.Action))
            {
                log.Log($"Completing quest {QuestID}", LogLevel.Trace);
                who.completeQuest(QuestID);
                return true;
            }
            return false;
        }

        private static void CheckQuestActionOnNPC(object sender, ActionNPCEventArgs e)
        {
            CheckActionQuest(e.Who, "594805", "Elliott", new string[] { "BJ", "milk_fast" }, e);
            CheckActionQuest(e.Who, "594809", "Abigail", new string[] { "milk_start", "milk_fast" }, e);
            CheckActionQuest(e.Who, "594810", "Sebastian", new string[] { "BJ", "milk_fast" }, e);
            CheckActionQuest(e.Who, "594811", "Sebastian", new string[] { "BJ", "milk_fast" }, e);
            if (CheckActionQuest(e.Who, "594813", "Male", new string[] { "BJ", "milk_fast" }, e)) Game1.addHUDMessage(new HUDMessage("Don't forget to turn it into Special Milk"));
            CheckActionQuest(e.Who, "594814", "Magical", new string[] { "milk_start", "BJ", "milk_fast" }, e);
            CheckActionQuest(e.Who, "594815", "Harvey", new string[] { "BJ", "milk_fast", "sex" }, e);
            CheckActionQuest(e.Who, "594822", "Haley", new string[] { "milk_start", "milk_fast" }, e);
            CheckActionQuest(e.Who, "5948222", "Haley", new string[] { "eat_out" }, e);


            CheckActionQuest(e.Who, "594837", "George", new string[] { "BJ", "milk_fast" }, e);
            CheckActionQuest(e.Who, "5948382", "Jodi", new string[] { "milk_start", "milk_fast" }, e);


            log.Log($"{e.Who.Name} did {e.Action} with {e.Recipient.Name} in {e.Map.Name}", LogLevel.Trace);
        }

        #endregion

        #region Mail
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
                if (Immediate)
                {
                    who.mailbox.Add(NextMail);
                    log.Log($"{who.Name} has read mail {FinishedMail}. Adding {NextMail} to today's mailbox.", LogLevel.Trace);
                }
                else
                {
                    who.mailForTomorrow.Add(NextMail);
                    log.Log($"{who.Name} has read mail {FinishedMail}. Adding {NextMail} to tomorrow's mailbox.", LogLevel.Trace);
                }
            }
        }

        private static void SendNextMail(Farmer who, string FinishedMail, string NextMail, bool Immediate = false)
        {
            if (who.mailReceived.Contains(FinishedMail) && !who.hasOrWillReceiveMail(NextMail))
            {
                if (Immediate)
                {
                    who.mailbox.Add(NextMail);
                    log.Log($"{who.Name} has read mail {FinishedMail}. Adding {NextMail} to today's mailbox.", LogLevel.Trace);
                }
                else
                {
                    who.mailForTomorrow.Add(NextMail);
                    log.Log($"{who.Name} has read mail {FinishedMail}. Adding {NextMail} to tomorrow's mailbox.", LogLevel.Trace);
                }
            }
            if (who.mailForTomorrow.Contains(NextMail) && Immediate)
            {
                who.mailForTomorrow.Remove(NextMail);
                who.mailbox.Add(NextMail);

                log.Log($"Moving mail item {NextMail} to today's mailbox", LogLevel.Trace); ;
            }
        }

        private static void SendNextMail(Farmer who, string NextMail, bool Immediate = false)
        {
            if (!who.mailReceived.Contains(NextMail) && !who.hasOrWillReceiveMail(NextMail))
            {
                if (Immediate)
                {
                    who.mailbox.Add(NextMail);
                    log.Log($"Adding {NextMail} to today's mailbox.", LogLevel.Trace);
                }
                else
                {
                    who.mailForTomorrow.Add(NextMail);
                    log.Log($"Adding {NextMail} to tomorrow's mailbox.", LogLevel.Trace);
                }
            }
        }
        #endregion

        private void RemoveAllMail(string command, string[] args)
        {
            List<Farmer> farmers = Game1.getAllFarmers().ToList();

            Farmer who = args.Length > 0 ? farmers.Where(o => o.displayName.ToLower() == args[0].ToLower()) as Farmer : Game1.player;
            who ??= Game1.player;

            foreach (string s in MailEditor.Mail)
            {
                while (who.hasOrWillReceiveMail(s))
                {
                    who.RemoveMail(s);
                    log.Log($"Removing mail {s} from mailbox");
                }
            }
            SendNewMail(who);
            //ListMailReceived(who);
        }

        private void SendNewMail(Farmer who)
        {
            // Tutorial
            SendNextMail(who, "MilkButton1", Immediate: true);
            SendNextMail(who, "MilkButton1", "MilkButton2", Immediate: true);


            if (Config.Quests) //TODO This section picks the next quest. Need to rewrite/decide how to send next mail/quest.
            {
                //SendNextMail(who, "MilkButton2", "AbiMilking", Immediate: Config.RushMail);

                // TODO change this so that first mail item is sent when farmer uses wand/gives gift to villager.
                if (Config.MilkFemale)
                {
                    #region Abigail
                    SendNextMail(who, "MTV_AbigailQ1T", "MTV_AbigailQ2", "Abigail", 6, Immediate: Config.RushMail);         // Abi Milk Quest 2
                    SendNextMail(who, "MTV_AbigailQ2T", "MTV_AbigailQ3", "Abigail", 7, Immediate: Config.RushMail);         // Abi Milk Quest 3
                    SendNextMail(who, "MTV_AbigailQ3T", "MTV_AbigailQ4", "Abigail", 8, Immediate: Config.RushMail);       // Abi Milk Quest 4
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
                    SendNextMail(who, "MTV_LeahQ1T", "MTV_LeahQ2", "Leah", 6, Immediate: Config.RushMail);                  //Leah Quest 2
                    SendNextMail(who, "MTV_LeahQ2T", "MTV_LeahQ3", "Leah", 7, Immediate: Config.RushMail);                  //Leah Quest 3
                    //SendNextMail(who, "MTV_LeahQ3T", "MTV_LeahQ4", "Leah", 8, Immediate: Config.RushMail); //Leah Quest 4 - Not written yet
                    #endregion

                }

                if (Config.MilkMale)
                {
                    //SendNextQuest(who, "AbiMilking", "MTV_MaruQ1");                    // Maru Cum Quest 1 
                }
            }
        }

        private void SendNewMail(string command, string[] args)
        {
            Farmer who = Game1.player;

            if (args.Length > 0)
            {
                try
                {
                    who.mailbox.Add(args[0]);
                }
                catch (Exception ex)
                {
                    log.Log(ex.Message, Level: LogLevel.Alert);
                }
            }

            SendNewMail(who);

        }

        private void SendMailCompleted(Farmer who)
        {
            if (CurrentQuests.Count < 1)
            {
                log.Log($"Found {CurrentQuests.Count} quests. Skipping SendMailCompleted()");
                return;
            }

            log.Log($"Found {CurrentQuests.Count} quests. Sending completed mail");

            List<int> QuestsToRemove = new();

            // Check if no longer has quest.
            foreach (int q in CurrentQuests)
            {
                log.Log($"Checking if quest {q} is complete yet", LogLevel.Trace);
                if (!HasQuest(who, q)) // OR finished quest
                {
                    log.Log($"Quest ID {q} has finished");

                    // Remove quest items
                    if (q == 594824)
                    {
                        int stack = who.Items.Count(o => o.ItemId == "Invitation");
                        log.Log($"removing Invitation: {stack}", LogLevel.Trace);
                        while (stack > 0)
                        {
                            var v = who.Items.GetById("Invitation").ToList();
                            who.Items.Remove(v[0]);
                            stack--;
                        }
                    }

                    // Send new mail
                    if (QuestEditor.QuestMail.ContainsKey(q))
                    {
                        who.mailbox.Add(QuestEditor.QuestMail[q]);
                        log.Log($"Sending mail {q}: {QuestEditor.QuestMail[q]}", LogLevel.Trace);
                    }
                    else
                    {
                        log.Log($"couldn't find a quest with ID {q}", LogLevel.Trace);
                    }

                    QuestsToRemove.Add(q);
                }
            }

            foreach (int i in QuestsToRemove)
            {
                CurrentQuests.Remove(i);
            }
        }
        #endregion

        #region Farmer genital calls
        public void SendGenitalMail(Farmer who)
        {
            who.RemoveMail("MTV_Vagina");
            who.RemoveMail("MTV_Penis");
            who.RemoveMail("MTV_Ace");
            who.RemoveMail("MTV_Herm");

            log.Log($"o:{Config.OverrideGenitals} a:{Config.AceCharacter} v:{GetVagina(who)} p:{GetPenis(who)}", LogLevel.Trace);

            //Netcode.NetStringList mailbox = Config.Verbose ? who.mailbox : who.mailReceived;

            string newMail = "MTV_null";

            if (Config.AceCharacter) { newMail = "MTV_Ace"; goto sendmail; }
            if (GetPenis(who) && GetVagina(who)) { newMail = "MTV_Herm"; goto sendmail; }
            else if (GetPenis(who) && !GetVagina(who)) { newMail = "MTV_Penis"; goto sendmail; }
            else if (!GetPenis(who) && GetVagina(who)) { newMail = "MTV_Vagina"; goto sendmail; }
            else { newMail = "MTV_Ace"; goto sendmail; }

        sendmail:
            log.Log($"{newMail}: {MailEditor.Mail.Contains(newMail)}", LogLevel.Info);
            //var mailbox = Config.Verbose ? who.mailbox : who.mailReceived;
            //mailbox.Add(newMail);
            if (Config.Verbose)
                who.mailbox.Add(newMail);
            else
                who.mailReceived.Add(newMail);
        }

        public static bool GetVagina(Farmer who)
        {
            //log.LogVerbose($"Override: {TempRefs.OverrideGenitals}. Vagina: {TempRefs.HasVagina}");
            if (TempRefs.OverrideGenitals) return TempRefs.HasVagina;
            return !who.IsMale;
        }

        public static bool GetPenis(Farmer who)
        {
            //log.LogVerbose($"Override: {TempRefs.OverrideGenitals}. Penis: {TempRefs.HasPenis}", LogLevel.Alert);
            if (TempRefs.OverrideGenitals) return TempRefs.HasPenis;
            return who.IsMale;
        }

        public static bool GetBreasts(Farmer who)
        {
            //log.LogVerbose($"Override: {TempRefs.OverrideGenitals}. Breasts: {TempRefs.HasBreasts}", LogLevel.Alert);
            if (TempRefs.OverrideGenitals) return TempRefs.HasBreasts;
            return !who.IsMale;
        }
        #endregion

        #region checks etc.
        private static bool CheckAll()
        {
            bool result = true;

            log.Log("Checking items...");
            if (!ItemEditor.CheckAll())
                result = false;

            CorrectItems();

            return result;
        }

        public void UpdateItemConfig()
        {
            ItemEditor.RemoveInvalid(Config.MilkMale, Config.MilkFemale);
            RecipeEditor.RemoveInvalid(Config.MilkMale, Config.MilkFemale);
        }

        private void AddAllRecipes(Farmer who)
        {
            #region Removing recipes
            log.Log($"Removing recipes", LogLevel.Trace);
            who.cookingRecipes.Remove("Trunip190.CP.MilkTheVillagers.Creamy_Coffee");
            who.cookingRecipes.Remove("Trunip190.CP.MilkTheVillagers.Protein_Shake");
            who.cookingRecipes.Remove("Trunip190.CP.MilkTheVillagers.Secret_Pleasures");
            who.cookingRecipes.Remove("Trunip190.CP.MilkTheVillagers.Special_Milkshake");
            who.cookingRecipes.Remove("Trunip190.CP.MilkTheVillagers.Super_Juice");
            who.cookingRecipes.Remove("Trunip190.CP.MilkTheVillagers.Eldritch_Energy");
            who.cookingRecipes.Remove("Trunip190.CP.MilkTheVillagers.Martini_Kairos");
            who.cookingRecipes.Remove("Trunip190.CP.MilkTheVillagers.Sweet_Sibling");
            who.cookingRecipes.Remove("Trunip190.JA.MilkTheVillagers_Protein_Shake");
            who.cookingRecipes.Remove("Trunip190.CP.MilkTheVillagers.Woman's_Milk");
            who.cookingRecipes.Remove("Trunip190.CP.MilkTheVillagers.Special_Milk");
            who.cookingRecipes.Remove("Trunip190.CP.MilkTheVillagers.Magical_Essence");

            who.craftingRecipes.Remove("Trunip190.CP.MilkTheVillagers.Creamy_Coffee");
            who.craftingRecipes.Remove("Trunip190.CP.MilkTheVillagers.Protein_Shake");
            who.craftingRecipes.Remove("Trunip190.CP.MilkTheVillagers.Secret_Pleasures");
            who.craftingRecipes.Remove("Trunip190.CP.MilkTheVillagers.Special_Milkshake");
            who.craftingRecipes.Remove("Trunip190.CP.MilkTheVillagers.Super_Juice");
            who.craftingRecipes.Remove("Trunip190.CP.MilkTheVillagers.Eldritch_Energy");
            who.craftingRecipes.Remove("Trunip190.CP.MilkTheVillagers.Martini_Kairos");
            who.craftingRecipes.Remove("Trunip190.CP.MilkTheVillagers.Sweet_Sibling");
            who.craftingRecipes.Remove("Trunip190.JA.MilkTheVillagers_Protein_Shake");
            who.craftingRecipes.Remove("Trunip190.CP.MilkTheVillagers.Woman's_Milk");
            who.craftingRecipes.Remove("Trunip190.CP.MilkTheVillagers.Special_Milk");
            who.craftingRecipes.Remove("Trunip190.CP.MilkTheVillagers.Magical_Essence");

            who.craftingRecipes.Remove("Trunip190.CP.MilkTheVillagers.Shibari_rope");
            #endregion

            #region Adding in recipes
            log.Log($"Adding back in {5} recipes", LogLevel.Trace);
            who.cookingRecipes.Add("Trunip190.CP.MilkTheVillagers.Creamy_Coffee", 0);
            who.cookingRecipes.Add("Trunip190.CP.MilkTheVillagers.Protein_Shake", 0);
            who.cookingRecipes.Add("Trunip190.CP.MilkTheVillagers.Secret_Pleasures", 0);
            who.cookingRecipes.Add("Trunip190.CP.MilkTheVillagers.Special_Milkshake", 0);
            who.cookingRecipes.Add("Trunip190.CP.MilkTheVillagers.Super_Juice", 0);
            who.cookingRecipes.Add("Trunip190.CP.MilkTheVillagers.Eldritch_Energy", 0);
            who.cookingRecipes.Add("Trunip190.CP.MilkTheVillagers.Martini_Kairos", 0);
            who.cookingRecipes.Add("Trunip190.CP.MilkTheVillagers.Sweet_Sibling", 0);
            who.cookingRecipes.Add("Trunip190.JA.MilkTheVillagers_Protein_Shake", 0);
            who.cookingRecipes.Add("Trunip190.CP.MilkTheVillagers.Woman's_Milk", 0);
            who.cookingRecipes.Add("Trunip190.CP.MilkTheVillagers.Special_Milk", 0);
            who.cookingRecipes.Add("Trunip190.CP.MilkTheVillagers.Magical_Essence", 0);

            who.craftingRecipes.Add("Trunip190.CP.MilkTheVillagers.Creamy_Coffee", 0);
            who.craftingRecipes.Add("Trunip190.CP.MilkTheVillagers.Protein_Shake", 0);
            who.craftingRecipes.Add("Trunip190.CP.MilkTheVillagers.Secret_Pleasures", 0);
            who.craftingRecipes.Add("Trunip190.CP.MilkTheVillagers.Special_Milkshake", 0);
            who.craftingRecipes.Add("Trunip190.CP.MilkTheVillagers.Super_Juice", 0);
            who.craftingRecipes.Add("Trunip190.CP.MilkTheVillagers.Eldritch_Energy", 0);
            who.craftingRecipes.Add("Trunip190.CP.MilkTheVillagers.Martini_Kairos", 0);
            who.craftingRecipes.Add("Trunip190.CP.MilkTheVillagers.Sweet_Sibling", 0);
            who.craftingRecipes.Add("Trunip190.JA.MilkTheVillagers_Protein_Shake", 0);
            who.craftingRecipes.Add("Trunip190.CP.MilkTheVillagers.Woman's_Milk", 0);
            who.craftingRecipes.Add("Trunip190.CP.MilkTheVillagers.Special_Milk", 0);
            who.craftingRecipes.Add("Trunip190.CP.MilkTheVillagers.Magical_Essence", 0);

            who.craftingRecipes.Add("Trunip190.CP.MilkTheVillagers.Shibari_rope", 0);
            #endregion



            RecipeEditor.RemoveInvalid(Config.MilkMale, Config.MilkFemale);
        }

        private void GetItemCodes()
        {
            log.Log($"Correcting item codes");

            if (!ItemEditor.Initialised) { log.Log("ItemEditor isn't initialised.", LogLevel.Trace); return; }

            TempRefs.loaded = true;

            ItemEditor.GetAllItemIDs(TempRefs.Verbose);

            ItemEditor.RemoveInvalid(Config.MilkMale, Config.MilkFemale);

        }

        private static void CorrectItems()
        {
            ObjectEditor.UpdateData();
        }

        private static void CorrectRecipes()
        {
            RecipeEditor.SetCooking();
            RecipeEditor.SetCrafting();

            //_recipeEditor.CookingData["Protein Shake"] = $"{TempRefs.CumType} 3/10 10/{TempRefs.ProteinShake}/default/Protein shake";
            //_recipeEditor.CookingData["Milkshake"] = $"{TempRefs.MilkType} 3/10 10/{TempRefs.MilkShake}/default/Milkshake";
            //_recipeEditor.CookingData["Super Juice"] = $"{TempRefs.MilkType} 2 {TempRefs.CumType} 2/10 10/{TempRefs.MilkShake}/default/Super Juice";

            //_recipeEditor.data["Woman's Milk"] = $"{TempRefs.MilkType} 1/10 10/{TempRefs.MilkGeneric}/null/Woman's Milk";
            //_recipeEditor.data["Generic Cum"] = $"{TempRefs.CumType} 1/10 10/{TempRefs.MilkSpecial}/null/Special Milk";

        }
        #endregion

        #region Interact with NPC section

        private List<Response> GenerateSexOptions(NPC target)
        {
            List<Response> choices;
            choices = new List<Response>();

            if (target.Age == 2) // 2 is a child - immediate yeet
            {
                log.Log($"{target.Name} is a child.", LogLevel.Trace);
            }
            else
            {
                if ((TempRefs.IgnoreVillagerGender || target.Gender == Gender.Male) && Config.MilkMale) // Male and genderless
                {
                    choices.Add(new Response("milk_fast", "Fast BJ [20E]"));
                    choices.Add(new Response("BJ", "Give them a blowjob [20E]"));
                }
                else if ((TempRefs.IgnoreVillagerGender || target.Gender == Gender.Female) && Config.MilkFemale) // Female and genderless
                {
                    choices.Add(new Response("milk_fast", "Fast Milk [20E]"));
                    choices.Add(new Response("milk_start", "Milk them [20E]"));
                    choices.Add(new Response("eat_out", "Give Cunnilingus [30E] (partially implemented)")); //TODO Not written yet.
                }
                else if (target.Gender == Gender.Undefined)
                {
                    if (target.Name == "Dwarf")
                    {
                        choices.Add(new Response("milk_fast", "Fast Milk [20E]"));
                        choices.Add(new Response("milk_start", "Milk them [20E]"));
                    }
                    if (target.Name == "Krobus")
                    {
                        choices.Add(new Response("milk_fast", "Fast BJ [20E]"));
                        choices.Add(new Response("BJ", "Give them a blowjob [20E]"));
                    }
                }

                //choices.Add(new Response("get_eaten", "Ask them to eat you out [15E] (not implemented)")); //TODO not written yet.
                //choices.Add(new Response("sex", "Ask them for sex [50E](not implemented)")); //TODO not written yet.
            }

            choices.Add(new Response("abort", "Do nothing"));

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
                case "Mines": Game1.warpFarmer("Mountain", 54, 6, 1); break;
                case "Railroad": Game1.warpFarmer("Railroad", 29, 58, 1); break;
            }
        }

        public void DialoguesSet(Farmer who, string action)
        {
            if (who == null || action == null) return;

            #region Get farmer for self dialogue - TODO fix npc not getting loaded.
            NPC npc = Game1.getCharacterFromName("Trunip");

            IDictionary<string, CharacterData> npcs = Game1.characterData;
            //foreach (KeyValuePair<string, CharacterData> kvp in npcs) { }
            //if (candidates.Count > 0) npc = candidates[0];
            #endregion

            if (Config.Verbose)
            {
                if (currentTarget != null)
                    Game1.addHUDMessage(new HUDMessage($"Chose {action}  with {currentTarget.Name}"));
            }

            if (action == null || action == "abort")
                return;
            else if (action == "time_freeze") //pauses in game time progression for 1min IRL
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
            else if (action == "self_milk") // farmer collects their own breast milk.
            {
                //NPC Farmer = Game1.getCharacterFromName("Farmer");
                //Farmer.Dialogue.TryGetValue("FarmerCollectionMilk", out string cumDialogue);
                //Game1.drawObjectDialogue(cumDialogue);

                string dialogues = "Adding milk";
                //if (npc != null) npc.Dialogue.TryGetValue("FarmerCollectionMilk", out dialogues);

                Dialogue v = log.ProcessDialogue(new Dialogue(null, "Strings/StringsFromCSFiles:FarmerCollectionMilk"));
                Game1.DrawDialogue(v);

                //DrawDialogue(npc, message: dialogues, action: "FarmerCollectionMilk");
                //Game1.drawObjectDialogue(dialogues);
                //Game1.drawObjectDialogue(Config.FarmerCollectionMilk);

                who.addItemToInventory(new sObject("Trunip190.CP.MilkTheVillagers.Woman's_Milk", 1, quality: 2));

                who.Stamina -= 150;
                //TempRefs.SelfMilkedToday = true;
            }
            else if (action == "self_cum") // farmer collects their own cum
            {
                //NPC Farmer = Game1.getCharacterFromName("Farmer");
                //Farmer.Dialogue.TryGetValue("FarmerCollectCum", out string cumDialogue);
                //Game1.drawObjectDialogue(cumDialogue);

                string dialogues = "Adding cum";
                //if (npc != null) npc.Dialogue.TryGetValue("FarmerCollectCum", out dialogues);


                Dialogue v = log.ProcessDialogue(new Dialogue(null, "Strings/StringsFromCSFiles:FarmerCollectCum"));
                Game1.DrawDialogue(v);

                //DrawDialogue(npc, message: dialogues, action: "FarmerCollectCum");
                //Game1.drawObjectDialogue(dialogues);
                //Game1.drawObjectDialogue(Config.FarmerCollectCum);

                who.addItemToInventory(new sObject("Trunip190.CP.MilkTheVillagers.Special_Milk", 1, quality: 2));

                who.Stamina -= 150;
                //TempRefs.SelfCummedToday = true;
            }
            else
                ActionOnNPC(currentTarget, who, action);

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

            // Reset Additem
            AddItem = null;
            int HeartCurrent = who.getFriendshipHeartLevelForNPC(npc.Name);
            string chosenString;
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
                DrawDialogue(npc, $"That's flattering, but I don't like you enough for that. ({HeartCurrent}/{heartMin} hearts)", action: "action_rejected");
                log.Log($"{npc.Name} is heart level {HeartCurrent} and needs to be {heartMin}", LogLevel.Trace);
                goto cleanup;
            }

            // Energy level
            if (who.stamina < energyCost)
            {
                Game1.addHUDMessage(new HUDMessage($"You don't have enough energy for that. {who.stamina}/{energyCost}"));
                goto cleanup;
            }

            //milked today
            //TODO rewrite this to base it off of the choice.
            if ((action == "milk_start" || action == "milk_fast") && TempRefs.milkedtoday.Contains(npc))
            {
                if (Config.Verbose)
                    Game1.addHUDMessage(new HUDMessage($"{npc.Name} has already been milked today."));
                goto cleanup;
            }
            if ((action == "sex" || action == "BJ") && TempRefs.SexToday.Contains(npc))
            {
                if (Config.Verbose)
                    Game1.addHUDMessage(new HUDMessage($"{npc.Name} has already had sex with you today."));
                goto cleanup;
            }
            //int ItemCode;
            string ItemCode;
            #endregion

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

            //AddItem = new sObject($"{ItemCode}", Quantity, quality: Quality);

            AddItem = new sObject(ItemCode, Quantity, quality: Quality) { Category = -34 };
            CurrentFarmer = who;

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

            log.Log($"Trying to milk {npc.Name}. Will give item {ItemCode}: {ItemCode}, Category: {AddItem.Category} / {AddItem.getCategoryName()}", LogLevel.Trace);

            // If no male milking, don't give item.
            if ((npc.Gender == 0 & !Config.MilkMale) || !Config.CollectItems)
            {
                //SItemCode = "";
                AddItem = null;
            }
            #endregion

            log.Log($"{AddItem}");

            // Draw Dialogue
            npc.facePlayer(who);
            if (npc.Dialogue.TryGetValue(action, out string dialogues)) //Does npc have milking dialogue?
            {
                chosenString = GetRandomString(dialogues.Split(new string[] { "#split#" }, System.StringSplitOptions.None));

                #region move the whole item selection to a separate method.

                // Get Item code from ChosenString
                if (chosenString.Contains("["))
                {
                    int start = chosenString.IndexOf("[") + 1;
                    int end = chosenString.IndexOf("]", start);
                    string val = chosenString[start..end];
                    log.Log($"{npc.Name} value was {val}", LogLevel.Trace);

                    chosenString = chosenString.Replace($"{val}", "").Replace("[]", "").Trim();
                    if ( !Config.StackMilk) ItemCode = val;
                }

                AddItem = new sObject($"{ItemCode}", 1);
                //string SItemCode = $"[{ItemCode}]";

                // Get Quality from ChosenString
                if (chosenString.Contains("[Quality:"))
                {
                    int start = chosenString.IndexOf("[Quality:");
                    int end = chosenString.IndexOf("]", start);

                    string val = chosenString.Substring(end - 1, 1);
                    log.Log($"{npc.Name} Quality was {val}", LogLevel.Trace);
                    int.TryParse(val, out Quality);

                    AddItem.Quality = Quality;
                    chosenString = chosenString.Replace($"Quality:{val}", "").Replace("[]", "").Trim();
                }

                // Get Quantity from ChosenString
                if (chosenString.Contains("[Quantity:"))
                {
                    //"{{spacechase0.JsonAssets/ObjectId: }}";
                    int start = chosenString.IndexOf("[Quantity:");
                    int end = chosenString.IndexOf("]", start);

                    string val = chosenString.Substring(end - 1, 1);
                    log.Log($"{npc.Name} Quantity was {val}", LogLevel.Trace);
                    int.TryParse(val, out Quantity);

                    AddItem.Stack = Quantity;
                    chosenString = chosenString.Replace($"Quantity:{val}", "").Replace("[]", "");
                }
                #endregion
                //ObjectEditor.UpdateData();

                // HUD messages
                if (who.mailReceived.Contains("MilkingProfQuality") && who.mailReceived.Contains("MilkingProfCount"))
                {
                    if (Config.Verbose && !MessageOnce) Game1.addHUDMessage(new HUDMessage("You manage to improve the quantity and quality of the item through your dextrous hand and tender touch."));
                    AddItem.Stack += 1;
                    AddItem.Quality += 1;
                    MessageOnce = true;
                }
                else if (who.mailReceived.Contains("MilkingProfQuality") && !MessageOnce)
                {
                    if (Config.Verbose) Game1.addHUDMessage(new HUDMessage("You manage to improve the quality of the item through your tender touch."));
                    AddItem.Quality += 1;
                    MessageOnce = true;
                }
                else if (who.mailReceived.Contains("MilkingProfCount") && !MessageOnce)
                {
                    if (Config.Verbose) Game1.addHUDMessage(new HUDMessage("You manage to coax out another batch through your dextrous hands."));
                    AddItem.Stack += 1;
                    MessageOnce = true;
                }


                // TODO split dialogue into list
                //DrawDialogue(npc, chosenString.Split("#$b#", StringSplitOptions.RemoveEmptyEntries));                
                DrawDialogue(npc, chosenString, action);
                success = true;
            }
            else //if (action != "milk_fast")
            {
                log.Log($"{npc.Name} failed to get anything for action {action}. Probably not written yet.", LogLevel.Info);
                switch (action)
                {
                    case "milk_fast":
                        //Game1.drawDialogue(npc, SItemCode);
                        CurrentFarmer.addItemToInventory(AddItem);
                        AddItem = null;
                        CurrentFarmer = null;
                        success = true;
                        break;

                    case "milk_start":
                        if (npc.Gender == Gender.Female)
                        {
                            chosenString = $"I've never been asked that by anyone else. Although, that DOES sound kinda hot.#$b#You spend the next few minutes slowly kneeding their breasts, collecting the milk in a jar you brought with you. [{ItemCode}]";
                        }
                        else if (npc.Gender == 0)
                        {
                            chosenString = $"You want my 'milk'? Erm, You ARE very attractive...#$b#*You quickly unzip their pants and pull out their cock. After a couple of quick licks to get them hard, you start sucking on them*#$b#I think I'm getting close! Here it comes! [{ItemCode}]";
                        }
                        else
                        {
                            chosenString = $"I'm sorry, but I don't produce anything you would want to collect. Perhaps we can do something else instead?";
                        }

                        DrawDialogue(npc, message: chosenString, action: action);
                        success = true;
                        break;

                    case "eat_out":
                        chosenString = $"You want to go down on me? I don't think I've ever had a guy offer to do that without an ulterior motive before.$h" +
                            $"#$b#%*{npc.Name} quickly strips out of her lower garments, and opens her legs wide for you. You're greeted with a heady smell, and notice that her lips are starting to swell*" +
                            $"#$b#%*You lean in and start licking between {npc.Name}'s lips, tasting her sweet nectar as she buries her hands in your hair*" +
                            $"#$b#%*As her moans get louder you use your tongue to flick her clit, and she clenches her legs tightly around your head*" +
                            $"#$b#%*You gently suck on the nub, and then plunge your tongue as deeply into her as you can, shaking your tongue from side to side to stimulate {npc.Name} even more*" +
                            $"#$b#@, I'm cumming!";

                        DrawDialogue(npc, message: chosenString, action: action);
                        success = true;
                        break;

                    case "get_eaten":
                        chosenString = $"I'm sorry, dialogue hasn't been written for that yet.";
                        DrawDialogue(npc, message: chosenString, action: action);
                        success = true;
                        break;

                    case "BJ":
                        chosenString = $"You want my 'milk'? Erm, You ARE very attractive...#$b#*You quickly unzip their pants and pull out their cock. " +
                            $"After a couple of quick licks to get them hard, you start sucking on their penis*#$b#I think I'm getting close! Here it comes! [{ItemCode}]";

                        DrawDialogue(npc, message: chosenString, action: action);
                        success = true;
                        break;

                    case "sex":
                        //TODO write four version of this for each gender configuration.

                        if (npc.Gender == 0 && who.IsMale) // Male player, male NPC.
                            chosenString = $"two dudes going at it";
                        else if (npc.Gender == Gender.Female && who.IsMale) // Male player, female NPC.
                            chosenString = $"{who.Name} buries their cock deep inside {npc.Name}'s pussy";
                        else if (npc.Gender == 0 && !who.IsMale) // Female player, male NPC.
                            chosenString = $"{who.Name} climbs on top of {npc.Name}'s erect cock and plunges it deep inside them until they cum";
                        else // neither is male
                            if (HasStrapon(who))
                            chosenString = $"You put on your strapon and fuck {npc.Name} silly.";
                        else
                            chosenString = $"You and {npc.Name} lick, suck and finger each other into oblivion";

                        DrawDialogue(npc, message: chosenString, action: action);
                        success = true;
                        break;

                    default:
                        chosenString = $"I don't have any dialogue written for that. Sorry.";
                        DrawDialogue(npc, message: chosenString, action: action);
                        Monitor.Log($"{action} for {npc.Name} wasn't found");
                        break;
                }
            }


        cleanup:
            // Cleanup
            if (success)
            {
                if (!Config.CollectItems) AddItem = null;
                who.stamina -= energyCost;
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

                who.changeFriendship(friendGain, npc);

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
                Game1.addHUDMessage(new HUDMessage("didn't succeed"));
            }
        }

        private static string GetRandomString(string[] dialogues)
        {
            int i = dialogues.Length;
            if (i < 1)
                return "";

            Random r = new();
            return dialogues[r.Next(i)];
        }

        private void DrawDialogue(NPC npc, string message = "Placeholder text", string action = "milk_start")
        {
            Dialogue v = log.ProcessDialogue(new Dialogue(npc, $"Characters/Dialogue/{npc.Name}:{action}"));

            var NewMessage = v.dialogues[0];

            if (NewMessage.Text.Trim() == "") NewMessage.Text = message;
            if (NewMessage.Text.Trim() == "") return;
            Game1.DrawDialogue(v);
        }

        private static bool HasStrapon(Farmer who)
        {
            return who.Items.GetById("Strapon").ToList().Count > 0 || true;
        }

        private static int[] GetNewPos(int direction, int x, int y)
        {
            int[] numArray = new int[2] { x, y };
            switch (direction)
            {
                case 0:
                    --numArray[1];
                    break;
                case 1:
                    ++numArray[0];
                    break;
                case 2:
                    ++numArray[1];
                    break;
                case 3:
                    --numArray[0];
                    break;
            }
            return numArray;
        }

        private static int[] FarmerPos(Farmer who)
        {
            return new int[2]
            {
                    (int)who.GetGrabTile().X,
                    (int)who.GetGrabTile().Y
            };
        }

        #endregion

        #region Mobile Phone
        private void LinkPhone()
        {
            //var api = this.Helper.ModRegistry.GetApi<IMobilePhoneApi>("aedenthorn.MobilePhone");
            //if (api != null)
            //{

            //    Texture2D appIcon = Helper.ModContent.Load<Texture2D>(Path.Combine("assets", "app_icon.png"));
            //    bool success = api.AddApp(Helper.ModRegistry.ModID, "MilkTheVillagers", MobileOpenApp, appIcon);
            //    Monitor.Log($"loaded phone app successfully: {success}", LogLevel.Debug);
            //}
        }

        private void MobileOpenApp()
        {

        }
        #endregion

        #region Testing/Console Commands
        private void DumpDialogue(string command, string[] args)
        {
            log.Log("Dumping main custom dialogue", LogLevel.Info, Force: true);

            List<string> chars;
            List<string> sArgs = new();
            List<string> results = new();


            foreach (string s in args)
            {
                if (s != "bare") sArgs.Add(s);
            }

            if (sArgs.Count < 1)
            {
                chars = NPCGiftTastesEditor.Villagers;
            }
            else
            {
                chars = new List<string>();
                chars.AddRange(sArgs);
            }

            foreach (string s in chars)
            {
                var adj = NPCGiftTastesEditor.Villagers.Find(o => o.ToLower().Contains(s.ToLower()));
                if (Game1.getCharacterFromName(adj) != null && Game1.getCharacterFromName(adj).Dialogue.Count > 0)
                {
                    foreach (KeyValuePair<string, string> kvp in Game1.getCharacterFromName(adj).Dialogue)
                    {
                        if (log.topics.Contains(kvp.Key))
                        {
                            if (args.Contains("bare")) results.Add($"{adj}:\t{kvp.Key}");
                            else results.Add($"{adj}:\t{kvp.Key}:\t{kvp.Value}");
                        }
                    }
                }
                else
                {
                    results.Add($"{s} not found");
                }
            }

            results.Sort();

            foreach (string s in results)
            {
                log.Log(s);
            }
        }

        private void ForceRemoveQuest(string command, string[] args)
        {
            Farmer who = Game1.player;
            Dictionary<string, int> quests = GetAllIndexes(who);

            for (int i = 0; i < args.Length; i++)
            {
                _ = args[i];
                if (quests.ContainsKey(args[i]))
                {
                    who.questLog.RemoveAt(quests[args[i]]);
                }
            }
        }

        private void Upgrade(string command, string[] args)
        {
            RemoveAllMail("mtv_resetmail", null);
            SendNewMail("mtv_sendmail", Array.Empty<string>());


        }

        private void RenewItems(string command, string[] args)
        {
            Farmer who = Game1.player;

            //Alternative method
            foreach (Item v in who.Items)
            {
                if (v == null || v.GetType() != typeof(sObject)) continue;

                sObject ve = v as sObject;

                if (ItemEditor.ModItems.ContainsKey(ve.Name))
                {
                    ve = log.NewItem(who, ve);

                    if (ve.Quality == 3) { ve.Quality = 2; }

                    log.Log($"Correcting item {ve.Name}");
                    ve.Category = ve.Name.Contains("Milk") ? -34 : -35;

                    if (ve.Name == "Dwarf's Essence"
                        || ve.Name == "Krobus's Essence"
                        || ve.Name == "Wizard's Essence"
                        || ve.Name == "Mr. Qi's Essence"
                        || ve.Name == "Magical Essence")
                    {
                        v.Category = -36;
                    }
                }
                else
                {
                    log.Log($"Skipping item {v.Name}");

                }
            }

        }

        private void CheckItemCodes(string command, string[] args)
        {
            //log.Log($"Checking items are loaded");
            //if (!ItemEditor.Initialised) { log.Log($"ItemEditor isn't initialised"); return; }

            if (args.Contains("Init"))
            {
                ItemEditor.GetAllItemIDs(report: true);
                RecipeEditor.SetCooking();
                RecipeEditor.SetCrafting();
            }

            ItemEditor.Report();
        }

        private void ReportRecipes(string command, string[] args)
        {
            log.Log("Dumping all MTV recipes.", LogLevel.Info);
            RecipeEditor.ReportAll();
        }

        private void SendAllMail(string command, string[] args)
        {
            Farmer Who = Game1.player;
            foreach (string s in MailEditor.Mail)
            {
                Who.mailbox.Add(s);
            }
        }

        /// <summary>
        /// Adds all quests from Milk The Villagers to the farmer's questslog. (uncompleted)
        /// </summary>
        /// <param name="args">[Completed]</param>
        private void AddAllQuests(string command, string[] args)
        {
            Farmer who = Game1.player;
            List<string> failed = new();
            foreach (KeyValuePair<string, int> kvp in QuestEditor.QuestIDs)
            {
                if (!kvp.Value.ToString().StartsWith("5948")) continue;

                log.Log($"\tAdding quests {kvp.Value}-{kvp.Key}");
                try
                {
                    who.addQuest($"{kvp.Value}");
                    if (args.Length > 0 && args.Contains("completed")) who.completeQuest($"{kvp.Value}");
                }
                catch (Exception ex)
                {
                    log.Log($"{ex.Message}");
                    failed.Add(kvp.Key); ;
                }
            }

            foreach (string s in failed)
            {
                log.Log($"\t\tFailed to add quest {s}");
            }
        }

        private void FileCheck(string command, string[] args)
        {
            log.Log($"Checking files", LogLevel.Trace, Force: true);
            List<string> folders;

            if (args.Contains("all"))
            {
                folders = log.GetDirectories($"{Environment.CurrentDirectory}\\Mods\\Milk the Villagers");
                foreach (string s in folders)
                {
                    foreach (string f in Directory.GetFiles(s))
                    {
                        log.Log(f.Replace(Environment.CurrentDirectory, ".."), LogLevel.Trace, Force: true);
                    }
                }

            }
            else
            {
                folders = log.GetDirectories($"{Environment.CurrentDirectory}\\Mods\\Milk the Villagers\\[JA]MilkVillagers\\");
                foreach (string s in folders)
                {
                    foreach (string f in Directory.GetFiles(s))
                    {
                        log.Log(f.Replace(Environment.CurrentDirectory, ".."), LogLevel.Trace, Force: true);
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

            log.Log($"Setting all villagers to {level / 250} hearts", LogLevel.Info);

            foreach (string s in NPCGiftTastesEditor.Villagers)
            {
                if (args.Contains("list")) log.Log($"{s} set to {level} -> {level / 250}", LogLevel.Info, Force: true);

                int mod = level - Game1.player.getFriendshipLevelForNPC(s);
                Game1.player.changeFriendship(mod, Game1.getCharacterFromName(s));
            }

        }

        private void DumpQuests(string command, string[] args)
        {
            QuestEditor.Report(i18n: args.Length > 0 && args.Contains("i18n"), raw: args.Length > 0 && args.Contains("raw"), special: args.Length > 0 && args.Contains("special"));
            log.Log($"i18n: {args.Contains("i18n")}; raw: {args.Contains("raw")}; special: {args.Contains("special")}", LogLevel.Debug, Force: true);
        }
        #endregion



    }

    public delegate void ActionNPC(object sender, ActionNPCEventArgs e);

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

        public static void CategoryColourPostfix(ref Color __result, ref sObject __instance)
        {
            //log.Log($"mtv trying to edit getCategoryName with Harmony.", LogLevel.Info);

            switch (__instance.Category)
            {
                case -34: __result = Color.DeepPink; return;
                case -35: __result = Color.Blue; return;
                case -36: __result = Color.Purple; return;

                default: return;
            }
        }

        public static void SpriteIndexFromRawPost(ref string item_id, ref string __result)
        {
            switch (item_id)
            {
                case "-34": __result = "Woman's Milk"; break;
                case "-35": __result = "Special Milk"; break;
                case "-36": __result = "Magical Essence"; break;
            };
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

            log.Log("Applying Harmony patch to \"Item.getCategoryName\"", LogLevel.Info);
            log.Log("Applying Harmony patch to \"Item.getCategoryColor\"", LogLevel.Info);
            log.Log("Applying Harmony patch to \"CraftingRecipe.getNameFromIndex\"", LogLevel.Info);

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
}
