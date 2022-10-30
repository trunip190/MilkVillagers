using MilkVillagers.Asset_Editors;
using SpaceCore.Events;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Locations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using IGenericModConfigMenuApi = GenericModConfigMenu.IGenericModConfigMenuApi;
using sObject = StardewValley.Object;

namespace MilkVillagers
{
    public class ModEntry : Mod
    {
        #region class variables
        private int[] target;
        private bool running;
        private bool runOnce;
        private bool MessageOnce;
        private NPC currentTarget;

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

        private bool doneOnce = false; //remove when not testing.

        // Time freeze stuff
        private float TimeFreezeTimer = 0;
        private float RegenAmount = 0;

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
            if (helper == null)
            {
                ModFunctions.LogVerbose("helper is null.", LogLevel.Error);
            }
            else
            {
                TempRefs.Helper = helper;
            }

            #region register events

            Helper.Events.Display.MenuChanged += Display_MenuChanged;
            Helper.Events.GameLoop.DayStarted += GameLoop_DayStarted;
            Helper.Events.GameLoop.GameLaunched += OnGameLaunched;
            Helper.Events.GameLoop.SaveLoaded += GameLoop_SaveLoaded;
            Helper.Events.GameLoop.TimeChanged += GameLoop_TimeChanged;
            Helper.Events.GameLoop.DayEnding += GameLoop_DayEnding;
            Helper.Events.GameLoop.OneSecondUpdateTicked += GameLoop_OneSecondUpdateTicked;
            Helper.Events.Input.ButtonPressed += OnButtonPressed;

            // SpaceCore events
            SpaceEvents.BeforeGiftGiven += SpaceEvents_BeforeGiftGiven;
            SpaceEvents.OnItemEaten += SpaceEvents_OnItemEaten;

            // My events
            OnActionNPC += CheckQuestActionOnNPC;

            #endregion

            #region Add in console commands
            helper.ConsoleCommands.Add("mtv_resetmail", "Removes all mail flags\n\nUsage: mtv_resetmail <value>\n- value: the farmer to remove mail from.", this.RemoveAllMail);
            helper.ConsoleCommands.Add("mtv_sendmail", "Send the next mail item if conditions are met\n\nUsage: mtv_sendmail <value>\n- value: the farmer to send mail to.", this.SendNewMail);
            helper.ConsoleCommands.Add("mtv_forceremovequest", "Remove specified quest from the farmer's questlog\n\nUsage: mtv_frq <value>\n- value: the quest id to remove from their questlog.", this.ForceRemoveQuest);
            helper.ConsoleCommands.Add("mtv_frq", "Remove specified quest from the farmer's questlog\n\nUsage: mtv_frq <value>\n- value: the quest id to remove from their questlog.", this.ForceRemoveQuest);
            helper.ConsoleCommands.Add("mtv_upgrade", "Force the mod to reset quest and mail flags when you have upgraded the mod\n\nUsage: mtv_upgrade", this.Upgrade);
            helper.ConsoleCommands.Add("mtv_renewItems", "Update items in farmer's inventory to newer versions after upgrade.\n\nUsage: mtv_renewItems", this.RenewItems);
            helper.ConsoleCommands.Add("mtv_reportItems", "Ouput debug info on mod items currently in the game.\n\nUsage: mtv_reportItems", this.ReportItems);
            helper.ConsoleCommands.Add("mtv_reportRecipes", "Output debug info on all recipes in game.\n\nUsage: mtv_reportRecipes", this.ReportRecipes);
            helper.ConsoleCommands.Add("mtv_dumpDialogue", "Output all dialogue for specified villager\n\nUsage: mtv_dumpDialogue VillagerName", this.DumpDialogue);
            helper.ConsoleCommands.Add("mtv_reloadconfig", "Forces Milk The Villagers to reload it's config from the file. Useful in case of editing with Generic Mod Config Menu during a session.\n\nUsage: mtv_reloadconfig", this.ReloadConfig);
            helper.ConsoleCommands.Add("mtv_sendallmail", "Sends every mail item from this mod, even previously sent ones\n\nUsage:mtv_sendallmail", this.SendAllMail);
            helper.ConsoleCommands.Add("mtv_addquests", "Adds all quests to the farmer's questlog\n\nUsage: mtv_addquests", this.AddAllQuests);
            #endregion
        }

        #region Game OnEvent Triggers
        private void OnGameLaunched(object sender, GameLaunchedEventArgs e)
        {
            UpdateConfig();

            // New style of editing assets.
            Helper.Events.Content.AssetRequested += Content_AssetRequested;
            Helper.Events.Content.AssetReady += Content_AssetReady;

            TempRefs.thirdParty = Config.ThirdParty;
            TempRefs.Verbose = Config.Verbose;
            TempRefs.OverrideGenitals = Config.OverrideGenitals;
            TempRefs.HasPenis = Config.HasPenis;
            TempRefs.HasVagina = Config.HasVagina;
            TempRefs.HasBreasts = Config.HasBreasts;
            TempRefs.IgnoreVillagerGender = Config.IgnoreVillagerGender;
        }

        private void UpdateConfig()
        {
            #region Generic Mod Config
            // get Generic Mod Config Menu API (if it's installed)
            var api = this.Helper.ModRegistry.GetApi<IGenericModConfigMenuApi>("spacechase0.GenericModConfigMenu");
            if (api != null)
            {
                // register mod configuration
                api.RegisterModConfig(
                    mod: this.ModManifest,
                    revertToDefault: () => this.Config = new ModConfig(),
                    saveToFile: () => this.Helper.WriteConfig(this.Config)
                );

                // let players configure your mod in-game (instead of just from the title screen)
                api.SetDefaultIngameOptinValue(this.ModManifest, true);
                api.SubscribeToChange(this.ModManifest, UpdateConfig);

                // add some config options
                #region basic mod options
                api.RegisterSimpleOption(
                    mod: this.ModManifest,
                    optionName: "Milking Button",
                    optionDesc: "Set button for milking",
                    optionGet: () => this.Config.MilkButton,
                    optionSet: (SButton var) => this.Config.MilkButton = var
                    );

                api.RegisterSimpleOption(
                    mod: this.ModManifest,
                    optionName: "Milk Females",
                    optionDesc: "Lets you milk female characters if they like you enough",
                    optionGet: () => this.Config.MilkFemale,
                    optionSet: value => this.Config.MilkFemale = value
                );

                api.RegisterSimpleOption(
                    mod: this.ModManifest,
                    optionName: "Milk Males",
                    optionDesc: "Lets you milk male characters if they like you enough",
                    optionGet: () => this.Config.MilkMale,
                    optionSet: value => this.Config.MilkMale = value
                );

                api.RegisterSimpleOption(
                    mod: this.ModManifest,
                    optionName: "Collect Items?",
                    optionDesc: "Collect items, or just sex?",
                    optionGet: () => this.Config.CollectItems,
                    optionSet: value => this.Config.CollectItems = value
                );

                api.RegisterSimpleOption(
                    mod: this.ModManifest,
                    optionName: "Simple milk/cum",
                    optionDesc: "Replace individual items with generic ones.",
                    optionGet: () => this.Config.StackMilk,
                    optionSet: value => this.Config.StackMilk = value
                );

                api.RegisterSimpleOption(
                    mod: this.ModManifest,
                    optionName: "ExtraDialogue",
                    optionDesc: "Enable Abigail's everyday dialogue changes?",
                    optionGet: () => this.Config.ExtraDialogue,
                    optionSet: value => this.Config.ExtraDialogue = value
                );

                api.RegisterSimpleOption(
                    mod: this.ModManifest,
                    optionName: "Quests",
                    optionDesc: "Enable quest content",
                    optionGet: () => this.Config.Quests,
                    optionSet: value => this.Config.Quests = value
                );
                #endregion

                ////////////////////////////////////////////////////////////
                //////////////////   Overrite Genitals   ///////////////////
                ////////////////////////////////////////////////////////////
                #region Override Genitals
                api.RegisterSimpleOption(
                    mod: this.ModManifest,
                    optionName: "Override genitals",
                    optionDesc: "Do you want to override the genitals of the farmer?",
                    optionGet: () => this.Config.OverrideGenitals,
                    optionSet: value => this.Config.OverrideGenitals = value
                );
                #endregion

                #region A-sexual character
                api.RegisterSimpleOption(
                    mod: this.ModManifest,
                    optionName: "Ace Character",
                    optionDesc: "Do you want to play an A-Sexual Character? Ignores genitals.",
                    optionGet: () => this.Config.AceCharacter,
                    optionSet: value => this.Config.AceCharacter = value
                    );
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
                api.RegisterSimpleOption(
                    mod: this.ModManifest,
                    optionName: "Farmer has a penis",
                    optionDesc: "Does the farmer have a penis? MUST select override as well",
                    optionGet: () => this.Config.HasPenis,
                    optionSet: value => this.Config.HasPenis = value
                );
                #endregion

                #region Farmer has Vagina
                api.RegisterSimpleOption(
                    mod: this.ModManifest,
                    optionName: "Farmer has a vagina",
                    optionDesc: "Does the farmer have a vagina? MUST select override as well",
                    optionGet: () => this.Config.HasVagina,
                    optionSet: value => this.Config.HasVagina = value
                );
                #endregion

                #region Farmer has breasts
                api.RegisterSimpleOption(
                    mod: this.ModManifest,
                    optionName: "Farmer has breasts",
                    optionDesc: "Does the farmer have breasts? MUST select override as well",
                    optionGet: () => this.Config.HasBreasts,
                    optionSet: value => this.Config.HasBreasts = value
                );
                #endregion

                #region Ignore villager gender for farming
                api.RegisterSimpleOption(
                    mod: this.ModManifest,
                    optionName: "Ignore villager gender",
                    optionDesc: "Do you want to ignore the gender of the villager when milking?",
                    optionGet: () => this.Config.IgnoreVillagerGender,
                    optionSet: value => this.Config.IgnoreVillagerGender = value
                    );
                #endregion

                ////////////////////////////////////////////////////////////
                ///////////////// Developer options ////////////////////////
                ////////////////////////////////////////////////////////////
                #region Debug Mode
                api.RegisterSimpleOption(
                    mod: this.ModManifest,
                    optionName: "Debug mode",
                    optionDesc: "Enable debug mode content",
                    optionGet: () => this.Config.Debug,
                    optionSet: value => this.Config.Debug = value
                );
                #endregion

                #region Verbose Output
                api.RegisterSimpleOption(
                    mod: this.ModManifest,
                    optionName: "Verbose Dialogue",
                    optionDesc: "Enable verbose dialogue for tracking errors",
                    optionGet: () => this.Config.Verbose,
                    optionSet: value => this.Config.Verbose = value
                );
                #endregion
            }
            #endregion
        }

        #region SpaceEvents
        private void SpaceEvents_OnItemEaten(object sender, EventArgs e)
        {
            Farmer who = Game1.player;
            string EatenItem = who.itemToEat.Name;
            if (EatenItem == "Martini Kairos")
            {
                Game1.addHUDMessage(new HUDMessage("Time slows"));

                TimeFreezeTimer = 200000; // 1 minute timestop.
            }
            else if (EatenItem == "Eldritch Energy")
            {
                Game1.addHUDMessage(new HUDMessage("You feel energised"));
                RegenAmount = 1000; // 100 energy total restoration.

            }
        }

        private void SpaceEvents_BeforeGiftGiven(object sender, SpaceCore.Events.EventArgsBeforeReceiveObject e)
        {
            Farmer who = Game1.player;
            string ItemGiven = e.Gift.Name;
            NPC npcTarget = e.Npc;

            if (ItemGiven == "Readi Milk")
            {
                e.Cancel = true;

                if (!MailEditor.FirstMail.ContainsKey(npcTarget.Name))
                    return;

                string FirstQuestMail = MailEditor.FirstMail[npcTarget.Name];
                if (FirstQuestMail != "" && !who.hasOrWillReceiveMail(FirstQuestMail))
                {
                    if (npcTarget.Dialogue.TryGetValue("readi_milk", out string dialogues))
                    {
                        Game1.drawDialogue(npcTarget, dialogues);
                    }
                    else
                    {
                        Game1.drawDialogue(npcTarget, "A drink? Sure, anything that comes from %farm is always tasty.");
                    }
                    SendNextMail(who, FirstQuestMail, true);
                }
            }
        }
        #endregion

        private void OnButtonPressed(object sender, ButtonPressedEventArgs e)
        {
            if (running || Game1.menuUp)
            {
                return;
            }

            //switch for multiplayer.
            Farmer who = Game1.player;
            running = false;

            if (!Context.IsWorldReady)
                return;

            SButton button = e.Button;

            if (button == SButton.P && Config.Debug)
            {
                CheckAll();

                ModFunctions.LogVerbose($"{who.currentLocation.Name}, {who.getTileX()}, {who.getTileY()}");

                // for release - skip below.
                return;

                if (!doneOnce)
                {
                    //ForceRemoveQuest("mtv_frq", new string[] { "594801" });
                    //RemoveAllMail("mtv_resetmail", null);
                    doneOnce = true;

                    //who.addQuest(594815);
                    //who.mailbox.Add("MTV_AbigailQ1");
                    //who.mailbox.Add("MTV_MaruQ1");
                    //who.mailbox.Add("MTV_GeorgeQ1");
                    //who.mailbox.Add("MTV_HarveyQ2");
                    //who.mailbox.Add("MTV_HarveyQ3");
                    //who.mailbox.Add("MTV_MaruQ3");
                    //who.mailbox.Add("MTV_ElliottQ1");
                    //who.mailbox.Add("MTV_EmilyQ1");
                    //who.mailbox.Add("MTV_HaleyQ1");
                    //who.mailbox.Add("MTV_PennyQ2");
                    //who.mailbox.Remove("MTV_PennyQ2T");
                    //who.mailbox.Add("MTV_LeahQ1");
                    who.mailbox.Add("MTV_SebQ3");

                    Game1.warpFarmer("Farm", 69, 16, false);

                    //int ah = who.getFriendshipLevelForNPC("Alex");
                    //int h = 2000 - ah;
                    //ModFunctions.LogVerbose($"Alex was at {ah}, adding {h}, now at {who.getFriendshipLevelForNPC("Alex")}", LogLevel.Debug);
                    //who.changeFriendship(h, Game1.getCharacterFromName("Alex"));
                }
                else
                {
                    //SendGenitalMail(who);

                    //sObject invitation = new(TempRefs.Invitation, 7);
                    //who.addItemToInventory(invitation);

                    //Game1.warpFarmer("Town", 36, 56, false);
                    //Game1.player.activeDialogueEvents.Add("MTV_BoethiaBook", 1);

                    SendNewMail("mtv_sendmail", Array.Empty<string>());
                    //Game1.warpFarmer("Forest", 104, 34, false);
                    //Game1.warpFarmer("Mountain", 10, 4, false);
                    foreach (string v in who.mailForTomorrow)
                    {
                        ModFunctions.LogVerbose($"rushing mail item {v}", LogLevel.Trace);
                        who.mailbox.Add(v);
                    }
                    who.mailForTomorrow.Clear();
                }

            }
            else if (button == Config.MilkButton)
            {
                target = GetNewPos(who.FacingDirection, FarmerPos(who)[0], FarmerPos(who)[1]);
                NPC NPCtarget = ModFunctions.FindTarget(who.currentLocation, this.target, FarmerPos(who));
                if (NPCtarget != null)
                {
                    List<Response> choices = GenerateSexOptions(NPCtarget); // Get list of available options for this character
                    ModFunctions.LogVerbose($"{NPCtarget.Name}: gendercode = {NPCtarget.Gender}, age = {NPCtarget.Age}");

                    currentTarget = NPCtarget;
                    running = false;

                    Game1.currentLocation.createQuestionDialogue($"What do you want to do with {NPCtarget.Name}?", choices.ToArray(), new GameLocation.afterQuestionBehavior(DialoguesSet));
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
                    if (who.hasItemInInventory(TempRefs.MilkQi, 1))
                    {
                        options.Add(new Response("time_freeze", "Freeze time"));
                    }

                    // TODO not showing up?
                    if (who.hasItemInInventory(TempRefs.EldritchEnergy, 1))
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
                        Game1.addHUDMessage(new HUDMessage("You're too tired to do anything else right now", null));
                    }
                }
            }
            else if (Config.Debug && Config.Verbose)
            {
                //ModFunctions.LogVerbose($"Button {e.Button} not registered to a function.");
            }
        }

        private void GameLoop_SaveLoaded(object sender, SaveLoadedEventArgs e)
        {
            doneOnce = false;

            if (runOnce)
                return;

            GetItemCodes();

            foreach (Farmer who in Game1.getAllFarmers())
            {
                ModFunctions.LogVerbose($"Sending mail to {who.displayName}", LogLevel.Trace);
                CorrectRecipes();
                AddAllRecipes(who);
                SendGenitalMail(who);
            }

            runOnce = true;
        }

        private void GameLoop_DayStarted(object sender, DayStartedEventArgs e)
        {
            if (TempRefs.milkedtoday == null) ModFunctions.LogVerbose("TempRefs not set");
            else
            {
                ModFunctions.LogVerbose($"TempRefs is set. Cleared {TempRefs.milkedtoday.Count}");
                TempRefs.milkedtoday.Clear();
                TempRefs.SexToday.Clear();
                TempRefs.SelfCummedToday = false;
                TempRefs.SelfMilkedToday = false;
                MessageOnce = false;
            }

            //TODO change this for multiplayer
            foreach ( Farmer who in Game1.getOnlineFarmers())
            {
                SendNewMail(who);
            }
            //Farmer who = Game1.player;
            //SendNewMail(who);
        }

        private void GameLoop_TimeChanged(object sender, TimeChangedEventArgs e)
        {
            //TODO change for multiplayer
            foreach (Farmer who in Game1.getOnlineFarmers())
            {
                //Don't check anything if they haven't received the gender mail
                if (!who.mailReceived.Contains("MTV_Vagina") &&
                    !who.mailReceived.Contains("MTV_Penis") &&
                    !who.mailReceived.Contains("MTV_Ace") &&
                    !who.mailReceived.Contains("MTV_Herm"))
                {
                    ModFunctions.LogVerbose("Skipping quest watching");
                    return;
                }

                QuestChecks(who);
                SendMailCompleted(who);
            }
        }

        private void GameLoop_OneSecondUpdateTicked(object sender, OneSecondUpdateTickedEventArgs e)
        {
            Farmer Who = Game1.player;
            if (Who.hasMenuOpen.Value) return;

            CheckCompleteQuest(Who, 594825, "MTV_PennyQ1P");
            CheckCompleteQuest(Who, 594829, "MTV_LeahQ1P");
            CheckCompleteQuest(Who, 594818, "MTV_EmilyQ2P");
            CheckCompleteQuest(Who, 594804, "MTV_AbigailQ4T");
            CheckCompleteQuest(Who, 594839, "MTV_HarveyQ3T");
            CheckCompleteQuest(Who, 594840, "MTV_HarveyQ4T");
            CheckCompleteQuest(Who, 594806, "MTV_ElliottQ2T");
            CheckCompleteQuest(Who, 594807, "MTV_ElliottQ3T");
            CheckCompleteQuest(Who, 594808, "MTV_ElliottQ4T");
            CheckCompleteQuest(Who, 594827, "MTV_PennyQ2T");

            if (RegenAmount > 0)
            {
                Monitor.Log($"{RegenAmount} - {Game1.gameTimeInterval}", LogLevel.Trace);
                float restore = (float)(Game1.gameTimeInterval * 0.001);
                RegenAmount -= restore;
                Game1.player.stamina += restore;
                if (Game1.player.Stamina > Game1.player.MaxStamina) Game1.player.Stamina = Game1.player.MaxStamina;
            }
            if (TimeFreezeTimer > 0)
            {
                Monitor.Log($"{TimeFreezeTimer} - {Game1.gameTimeInterval}", LogLevel.Trace);
                TimeFreezeTimer -= Game1.gameTimeInterval;
                Game1.gameTimeInterval = 0;
                if (TimeFreezeTimer <= 0)
                    Game1.addHUDMessage(new HUDMessage("Time speeds back up again"));
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

                ModFunctions.LogVerbose("removing repeatable events from seenEvents", LogLevel.Trace);
                who.eventsSeen.Remove(5948121);
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
            if (DialogueEditor.CanEdit(e.Name)) { e.Edit(DialogueEditor.Edit); }
            if (QuestEditor.CanEdit(e.Name)) { e.Edit(QuestEditor.Edit); }
            if (RecipeEditor.CanEdit(e.Name)) { e.Edit(RecipeEditor.Edit); }
            if (EventEditor.CanEdit(e.Name)) { e.Edit(EventEditor.Edit); }
            if (MailEditor.CanEdit(e.Name)) { e.Edit(MailEditor.Edit); }
            if (ItemEditor.CanEdit(e.Name)) { e.Edit(ItemEditor.Edit); }
        }

        private void Content_AssetReady(object sender, AssetReadyEventArgs e)
        {
            if (e.Name.IsEquivalentTo("Data/mail")) { MailEditor.UpdateData(Helper.GameContent.Load<Dictionary<string, string>>(e.Name)); }
            if (e.Name.IsEquivalentTo("Data/ObjectInformation")) { ItemEditor.UpdateData(Helper.GameContent.Load<Dictionary<int, string>>(e.Name)); }
            if (e.Name.IsEquivalentTo("Data/Quests")) { QuestEditor.UpdateData(Helper.GameContent.Load<Dictionary<int, string>>(e.Name)); }
            if (e.Name.IsEquivalentTo("Data/CookingRecipes")) { RecipeEditor.UpdateCookingData(Helper.GameContent.Load<Dictionary<string, string>>(e.Name)); }
            if (e.Name.IsEquivalentTo("Data/CraftingRecipes")) { RecipeEditor.UpdateCraftingData(Helper.GameContent.Load<Dictionary<string, string>>(e.Name)); }

            //if (e.Name.IsEquivalentTo()) { DialogueEditor.UpdateData(Helper.GameContent.Load<Dictionary<string, string>>(e.Name)); }
            //if (e.Name.IsEquivalentTo()) { EventEditor.UpdateData(Helper.GameContent.Load<Dictionary<string, string>>(e.Name)); }
        }

        protected virtual void DoActionNPC(object sender, ActionNPCEventArgs e)
        {
            OnActionNPC?.Invoke(this, e);
        }
        #endregion

        #region Quest methods
        private void CheckCompleteQuest(Farmer Who, int questID, string MailName)
        {
            if (Who.hasQuest(questID) && Who.hasOrWillReceiveMail(MailName))
            {
                Who.completeQuest(questID);
            }
        }

        private void QuestChecks(Farmer who)
        {
            foreach (KeyValuePair<string, int> kvp in QuestEditor.QuestIDs)
            {
                CheckNewQuest(who, kvp.Value);
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
                    ve = ModFunctions.NewItem(who, ve);

                    if (ve.Quality == 3) { ve.Quality = 2; }

                    ModFunctions.LogVerbose($"Correcting item {ve.Name}");
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
                    ModFunctions.LogVerbose($"Skipping item {v.Name}");

                }
            }

        }

        private void ReportItems(string command, string[] args)
        {
            ItemEditor.GetAllItemIDs(report: true);
            RecipeEditor.SetCooking();
            RecipeEditor.SetCrafting();
            ItemEditor.Report();
        }

        private void ReportRecipes(string command, string[] args)
        {
            ModFunctions.LogVerbose("Dumping all recipes.", Force: true);
            RecipeEditor.ReportAll();
        }

        private bool CheckNewQuest(Farmer who, int questID)
        {
            if (!CurrentQuests.Contains(questID) && HasQuest(who, questID))
            {
                if (questID == 594824)
                {
                    ModFunctions.LogVerbose("Setting ActiveConversationEvent to MTV_Bukkake", LogLevel.Info);
                    Game1.player.activeDialogueEvents.Add("MTV_Bukkake", 1);
                }
                if (questID == 594834)
                {
                    ModFunctions.LogVerbose("Adding ConversationTopic", LogLevel.Info);
                    Game1.player.activeDialogueEvents.Add("HaleyPanties", 0);

                    // force the game to pick the right dialogue when married to Haley.
                    NPC haley = Game1.getCharacterFromName("Haley");
                    Game1.getCharacterFromName("Haley").resetCurrentDialogue();

                    bool check = haley.Dialogue.TryGetValue("HaleyPanties", out string dialogues);

                    ModFunctions.LogVerbose($"{haley.Name} {check} {dialogues}", LogLevel.Info);
                    haley.setNewDialogue(dialogues);
                }
                if ( questID == 594836)
                {
                    ModFunctions.LogVerbose("Setting ActiveConversationEvent to MTV_GeorgeQ4", LogLevel.Info);
                    Game1.player.activeDialogueEvents.Add("MTV_GeorgeQ4", 0);
                }

                CurrentQuests.Add(questID);
                ModFunctions.LogVerbose($"Watching quest ID {questID}", LogLevel.Trace);
                return true;
            }

            return false;
        }

        private void ForceRemoveQuest(string command, string[] args)
        {
            Farmer who = Game1.player;
            Dictionary<int, int> quests = GetAllIndexes(who);

            for (int i = 0; i < args.Length; i++)
            {
                _ = args[i];
                if (int.TryParse(args[0], out int id) && quests.ContainsKey(id))
                {
                    who.questLog.RemoveAt(quests[id]);
                }
            }
        }

        private static Dictionary<int, int> GetAllIndexes(Farmer who)
        {
            Dictionary<int, int> result = new();

            for (int i = 0; i < who.questLog.Count; i++)
            {
                StardewValley.Quests.Quest q = who.questLog[i];

                result[q.id.Value] = i;
            }

            return result;
        }

        private static bool HasQuest(Farmer who, int id)
        {
            foreach (var q in who.questLog)
            {
                if (q.id.Value == id)
                {
                    if (q.completed.Value)
                    {
                        ModFunctions.LogVerbose($"Quest {q.GetName()} is complete.", LogLevel.Trace);
                        return false;
                    }
                    return true;
                }
            }
            return false;
        }

        private static bool CheckActionQuest(Farmer who, int QuestID, string Recipient, string[] ActionRequired, ActionNPCEventArgs e)
        {
            //if (who.hasQuest(QuestID) && Recipient == e.recipient.Name && ActionRequired.Contains(e.action))
            //{
            //    ModFunctions.LogVerbose($"Completing quest {QuestID}", LogLevel.Alert);
            //    who.completeQuest(QuestID);
            //    return true;
            //}

            // Specific Name
            if (who.hasQuest(QuestID) &&
                (Recipient == e.Recipient.Name) &&
                ActionRequired.Contains(e.Action))
            {
                ModFunctions.LogVerbose($"Completing quest {QuestID}", LogLevel.Trace);
                who.completeQuest(QuestID);
                return true;
            }

            // Male or Female
            if (who.hasQuest(QuestID) &&
                ((Recipient == "Male" && e.Recipient.Gender == 0) || (Recipient == "Female" && e.Recipient.Gender == 1)) &&
                ActionRequired.Contains(e.Action))
            {
                ModFunctions.LogVerbose($"Completing quest {QuestID}", LogLevel.Trace);
                who.completeQuest(QuestID);
                return true;
            }

            // Magical
            if (who.hasQuest(QuestID) &&
                (Recipient == "Magical" && (e.Recipient.Name == "Dwarf" || e.Recipient.Name == "Wizard" || e.Recipient.Name == "Mister Qi" || e.Recipient.Name == "Krobus")) &&
                ActionRequired.Contains(e.Action))
            {
                ModFunctions.LogVerbose($"Completing quest {QuestID}", LogLevel.Trace);
                who.completeQuest(QuestID);
                return true;
            }
            return false;
        }

        private static void CheckQuestActionOnNPC(object sender, ActionNPCEventArgs e)
        {
            CheckActionQuest(e.Who, 594805, "Elliott", new string[] { "BJ", "milk_fast" }, e);
            CheckActionQuest(e.Who, 594809, "Abigail", new string[] { "milk_start", "milk_fast" }, e);
            CheckActionQuest(e.Who, 594810, "Sebastian", new string[] { "BJ", "milk_fast" }, e);
            CheckActionQuest(e.Who, 594811, "Sebastian", new string[] { "BJ", "milk_fast" }, e);
            CheckActionQuest(e.Who, 594813, "Male", new string[] { "BJ", "milk_fast" }, e);
            CheckActionQuest(e.Who, 594814, "Magical", new string[] { "milk_start", "BJ", "milk_fast" }, e);
            CheckActionQuest(e.Who, 594815, "Harvey", new string[] { "BJ", "milk_fast", "sex" }, e);
            CheckActionQuest(e.Who, 594822, "Haley", new string[] { "milk_start", "milk_fast" }, e);
            CheckActionQuest(e.Who, 5948222, "Haley", new string[] { "eat_out" }, e);


            CheckActionQuest(e.Who, 594837, "George", new string[] { "BJ", "milk_fast" }, e);
            CheckActionQuest(e.Who, 5948382, "Jodi", new string[] { "milk_start", "milk_fast" }, e);
            //CheckActionQuest(e.Who, 594839, "Harvey", new string[] { "BJ", "milk_fast" }, e);

            ModFunctions.LogVerbose($"{e.Who.Name} did {e.Action} with {e.Recipient.Name} in {e.Map.Name}", LogLevel.Trace);
        }

        #endregion

        #region Mail
        #region SendNextMail
        private static void SendNextMail(Farmer who, string FinishedMail, string NextMail, string Villager, int Heartlevel, bool Immediate = false)
        {
            if (who.mailReceived.Contains(FinishedMail) &&
                !who.hasOrWillReceiveMail(NextMail) &&
                who.getFriendshipHeartLevelForNPC(Villager) >= Heartlevel)
            {
                if (Immediate)
                {
                    who.mailbox.Add(NextMail);
                    ModFunctions.LogVerbose($"{who.Name} has read mail {FinishedMail}. Adding {NextMail} to today's mailbox.", LogLevel.Trace);
                }
                else
                {
                    who.mailForTomorrow.Add(NextMail);
                    ModFunctions.LogVerbose($"{who.Name} has read mail {FinishedMail}. Adding {NextMail} to tomorrow's mailbox.", LogLevel.Trace);
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
                    ModFunctions.LogVerbose($"{who.Name} has read mail {FinishedMail}. Adding {NextMail} to today's mailbox.", LogLevel.Trace);
                }
                else
                {
                    who.mailForTomorrow.Add(NextMail);
                    ModFunctions.LogVerbose($"{who.Name} has read mail {FinishedMail}. Adding {NextMail} to tomorrow's mailbox.", LogLevel.Trace);
                }
            }
        }

        private static void SendNextMail(Farmer who, string NextMail, bool Immediate = false)
        {
            if (!who.mailReceived.Contains(NextMail) && !who.hasOrWillReceiveMail(NextMail))
            {
                if (Immediate)
                {
                    who.mailbox.Add(NextMail);
                    ModFunctions.LogVerbose($"Adding {NextMail} to today's mailbox.", LogLevel.Trace);
                }
                else
                {
                    who.mailForTomorrow.Add(NextMail);
                    ModFunctions.LogVerbose($"Adding {NextMail} to tomorrow's mailbox.", LogLevel.Trace);
                }
            }
        }
        #endregion

        private void RemoveAllMail(string command, string[] args)
        {
            Farmer who = Game1.player;

            foreach (string s in MailEditor.Mail)
            {
                while (who.hasOrWillReceiveMail(s))
                {
                    who.RemoveMail(s);
                    ModFunctions.LogVerbose($"Removing mail {s} from mailbox");
                }
            }
            SendNewMail(who);
            //ListMailReceived(who);
        }

        private void SendNewMail(Farmer who)
        {
            if (Config.Quests) //TODO This section picks the next quest. Need to rewrite/decide how to send next mail/quest.
            {
                // Send new mail checks

                // Tutorial
                SendNextMail(who, "MilkButton1", Immediate: true);
                SendNextMail(who, "MilkButton1", "MilkButton2", Immediate: Config.RushMail);

                SendNextMail(who, "MilkButton2", "AbiMilking", Immediate: Config.RushMail);

                // TODO change this so that first mail item is sent when farmer uses wand/gives gift to villager.
                if (Config.MilkFemale)
                {
                    #region Abigail
                    SendNextMail(who, "MTV_AbigailQ1T", "MTV_AbigailQ2", "Abigail", 7, Immediate: Config.RushMail);         // Abi Milk Quest 2
                    SendNextMail(who, "MTV_AbigailQ2T", "MTV_AbigailQ3", "Abigail", 8, Immediate: Config.RushMail);         // Abi Milk Quest 3
                    SendNextMail(who, "MTV_AbigailQ3T", "MTV_AbigailQ4", "Abigail", 10, Immediate: Config.RushMail);       // Abi Milk Quest 4
                    #endregion

                    #region Maru
                    SendNextMail(who, "MTV_MaruQ1T", "MTV_MaruQ2", "Maru", 7, Immediate: Config.RushMail);                  // Abi Milk Quest 2
                    SendNextMail(who, "MTV_MaruQ2T", "MTV_MaruQ3", "Maru", 8, Immediate: Config.RushMail);                  // Abi Milk Quest 3
                    SendNextMail(who, "MTV_MaruQ3T", "MTV_MaruQ4", "Maru", 10, Immediate: Config.RushMail);                // Abi Milk Quest 4
                    #endregion

                    #region George 
                    SendNextMail(who, "MTV_GeorgeQ1T", "MTV_GeorgeQ2", "George", 7, Immediate: Config.RushMail);            // George Quest 2
                    //SendNextMail(who, "MTV_GeorgeQ2T", "MTV_GeorgeQ3", "George", 8, Immediate: Config.RushMail);            // George Quest 3
                    SendNextMail(who, "MTV_GeorgeQ3T", "MTV_GeorgeQ4", "George", 10, Immediate: Config.RushMail);           // George Quest 4
                    #endregion

                    #region Harvey
                    SendNextMail(who, "MTV_HarveyQ1T", "MTV_HarveyQ2", "Harvey", 7, Immediate: Config.RushMail);            // Harvey Quest 2
                    SendNextMail(who, "MTV_HarveyQ2T", "MTV_HarveyQ3", "Harvey", 8, Immediate: Config.RushMail);            // Harvey Quest 3
                    SendNextMail(who, "MTV_HarveyQ3T", "MTV_HarveyQ4", "Harvey", 10, Immediate: Config.RushMail);           // Harvey Quest 4
                    #endregion

                    #region Elliott - events not written
                    //SendNextMail(who, "MTV_ElliottQ1T", "MTV_ElliottQ2", "Elliott", 7, Immediate: Config.RushMail);         // Elliott Quest 2
                    SendNextMail(who, "MTV_ElliottQ2T", "MTV_ElliottQ3", "Elliott", 8, Immediate: Config.RushMail);         // Elliott Quest 3
                    SendNextMail(who, "MTV_ElliottQ3T", "MTV_ElliottQ4", "Elliott", 10, Immediate: Config.RushMail);        // Elliott Quest 4
                    #endregion

                    #region Emily
                    SendNextMail(who, "MTV_EmilyQ1T", "MTV_EmilyQ2", "Emily", 7, Immediate: Config.RushMail);               //Emily Quest 2
                    //SendNextMail(who, "MTV_EmilyQ2T", "MTV_EmilyQ3", "Emily", 8, Immediate: Config.RushMail);               //Emily Quest 3
                    SendNextMail(who, "MTV_EmilyQ3T", "MTV_EmilyQ4", "Emily", 10, Immediate: Config.RushMail);              //Emily Quest 4
                    #endregion

                    #region Haley
                    SendNextMail(who, "MTV_HaleyQ1T", "MTV_HaleyQ2", "Haley", 7, Immediate: Config.RushMail);               //Haley Quest 2
                    SendNextMail(who, "MTV_HaleyQ2T", "MTV_HaleyQ3", "Haley", 8, Immediate: Config.RushMail);               //Haley Quest 3
                    SendNextMail(who, "MTV_HaleyQ3T", "MTV_HaleyQ4", "Haley", 10, Immediate: Config.RushMail);              //Haley Quest 4
                    #endregion

                    #region Penny
                    SendNextMail(who, "MTV_PennyQ1T", "MTV_PennyQ2", "Penny", 7, Immediate: Config.RushMail);               //Penny Quest 2
                    //SendNextMail(who, "MTV_PennyQ2T", "MTV_PennyQ3", "Penny", 8, Immediate: Config.RushMail);               //Penny Quest 3
                    SendNextMail(who, "MTV_PennyQ3T", "MTV_PennyQ4", "Penny", 10, Immediate: Config.RushMail);              //Penny Quest 4
                    #endregion

                    #region Leah
                    SendNextMail(who, "MTV_LeahQ1T", "MTV_LeahQ2", "Leah", 7, Immediate: Config.RushMail);                  //Leah Quest 2
                    SendNextMail(who, "MTV_LeahQ2T", "MTV_LeahQ3", "Leah", 8, Immediate: Config.RushMail);                  //Leah Quest 3
                    //SendNextMail(who, "MTV_LeahQ3T", "MTV_LeahQ4", "Leah", 10, Immediate: Config.RushMail);                 //Leah Quest 4
                    #endregion

                    #region Sebastian
                    SendNextMail(who, "MTV_SebQ1T", "MTV_SebQ2", "Sebastian", 7, Immediate: Config.RushMail);   //Sebastian Quest 2
                    SendNextMail(who, "MTV_SebQ2T", "MTV_SebQ3", "Sebastian", 8, Immediate: Config.RushMail);   //Sebastian Quest 3
                    //SendNextMail(who, "MTV_SebQ3T", "MTV_SebQ4", "Sebastian", 10, Immediate: Config.RushMail);  //Sebastian Quest 4
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
                    ModFunctions.LogVerbose(ex.Message, Level: LogLevel.Alert);
                }
            }
            else
            {
                SendNewMail(who);
            }
        }

        private void SendMailCompleted(Farmer who)
        {
            if (CurrentQuests.Count < 1)
            {
                ModFunctions.LogVerbose($"Found {CurrentQuests.Count} quests. Skipping SendMailCompleted()");
                return;
            }

            ModFunctions.LogVerbose($"Found {CurrentQuests.Count} quests. Sending completed mail");

            List<int> QuestsToRemove = new();

            // Check if no longer has quest.
            foreach (int q in CurrentQuests)
            {
                ModFunctions.LogVerbose($"Checking if quest {q} is complete yet", LogLevel.Trace);
                if (!HasQuest(who, q)) // OR finished quest
                {
                    ModFunctions.LogVerbose($"Quest ID {q} has finished");

                    // Remove quest items
                    if (q == 594824)
                    {
                        int stack = who.getItemCount(TempRefs.Invitation);
                        ModFunctions.LogVerbose($"removing {TempRefs.Invitation}: {stack}", LogLevel.Trace);
                        who.removeItemsFromInventory(TempRefs.Invitation, stack);
                    }

                    // Send new mail
                    if (QuestEditor.QuestMail.ContainsKey(q))
                    {
                        who.mailbox.Add(QuestEditor.QuestMail[q]);
                        ModFunctions.LogVerbose($"Sending mail {q}: {QuestEditor.QuestMail[q]}", LogLevel.Trace);
                    }
                    else
                    {
                        ModFunctions.LogVerbose($"couldn't find a quest with ID {q}", LogLevel.Trace);
                    }

                    QuestsToRemove.Add(q);
                }
            }

            foreach (int i in QuestsToRemove)
            {
                CurrentQuests.Remove(i);
            }
        }

        //private static void ListMailReceived(Farmer who)
        //{
        //    foreach (string mailID in who.mailReceived)
        //    {
        //        if (MailEditor.ContainsKey(mailID))
        //            ModFunctions.LogVerbose(mailID);

        //    }
        //}
        #endregion

        #region Farmer genital calls
        public void SendGenitalMail(Farmer who)
        {
            who.RemoveMail("MTV_Vagina");
            who.RemoveMail("MTV_Penis");
            who.RemoveMail("MTV_Ace");
            who.RemoveMail("MTV_Herm");

            ModFunctions.LogVerbose($"o:{Config.OverrideGenitals} a:{Config.AceCharacter} v:{GetVagina(who)} p:{GetPenis(who)}", LogLevel.Trace);

            Netcode.NetStringList mailbox = Config.Verbose ? who.mailbox : who.mailReceived;
            string newMail = "MTV_null";

            if (Config.AceCharacter) { newMail = "MTV_Ace"; goto sendmail; }
            if (GetPenis(who) && GetVagina(who)) { newMail = "MTV_Herm"; goto sendmail; }
            if (GetPenis(who) && !GetVagina(who)) { newMail = "MTV_Penis"; goto sendmail; }
            if (!GetPenis(who) && GetVagina(who)) { newMail = "MTV_Vagina"; goto sendmail; }

        sendmail:
            ModFunctions.LogVerbose($"{newMail}: {MailEditor.Mail.Contains(newMail)}", LogLevel.Trace);
            mailbox.Add(newMail);
        }

        public static bool GetVagina(Farmer who)
        {
            //ModFunctions.LogVerbose($"Override: {TempRefs.OverrideGenitals}. Vagina: {TempRefs.HasVagina}");
            if (TempRefs.OverrideGenitals) return TempRefs.HasVagina;
            return !who.IsMale;
        }

        public static bool GetPenis(Farmer who)
        {
            //ModFunctions.LogVerbose($"Override: {TempRefs.OverrideGenitals}. Penis: {TempRefs.HasPenis}", LogLevel.Alert);
            if (TempRefs.OverrideGenitals) return TempRefs.HasPenis;
            return who.IsMale;
        }

        public static bool GetBreasts(Farmer who)
        {
            //ModFunctions.LogVerbose($"Override: {TempRefs.OverrideGenitals}. Breasts: {TempRefs.HasBreasts}", LogLevel.Alert);
            if (TempRefs.OverrideGenitals) return TempRefs.HasBreasts;
            return !who.IsMale;
        }
        #endregion

        #region checks etc.
        private static bool CheckAll()
        {
            bool result = true;

            ModFunctions.LogVerbose("Checking items...");
            if (!ItemEditor.CheckAll())
                result = false;

            return result;
        }

        public void UpdateConfig(string key, string value)
        {
            ItemEditor.RemoveInvalid(Config.MilkMale, Config.MilkFemale);
            RecipeEditor.RemoveInvalid(Config.MilkMale, Config.MilkFemale);
        }

        private void AddAllRecipes(Farmer who)
        {
            //TODO move this to be a quest reward or something.
            ModFunctions.LogVerbose($"Removing recipes", LogLevel.Trace);
            who.cookingRecipes.Remove("Special Milkshake");
            who.cookingRecipes.Remove("Protein Shake");
            who.cookingRecipes.Remove("Super Juice");
            who.craftingRecipes.Remove("Woman's Milk");
            who.craftingRecipes.Remove("Special Milk");

            ModFunctions.LogVerbose($"Adding back in {5} recipes", LogLevel.Trace);
            if (Config.MilkFemale) who.cookingRecipes.Add("Special Milkshake", 0);
            if (Config.MilkMale) who.cookingRecipes.Add("Protein Shake", 0);
            if (Config.MilkFemale && Config.MilkMale) who.cookingRecipes.Add("Super Juice", 0);
            if (Config.MilkFemale) who.craftingRecipes.Add("Woman's Milk", 0);
            if (Config.MilkMale) who.craftingRecipes.Add("Special Milk", 0);

            RecipeEditor.RemoveInvalid(Config.MilkMale, Config.MilkFemale);
        }

        private void GetItemCodes()
        {
            ModFunctions.LogVerbose($"Correcting item codes");

            if (!ItemEditor.Initialised) { ModFunctions.LogVerbose("ItemEditor isn't initialised.", LogLevel.Trace); return; }

            TempRefs.loaded = true;

            ItemEditor.GetAllItemIDs();
            ItemEditor.RemoveInvalid(Config.MilkMale, Config.MilkFemale);
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
        private void DumpDialogue(string command, string[] args)
        {
            if (args.Length < 1) return;

            ModFunctions.LogVerbose($"Dumping {args[0]}'s dialogue", LogLevel.Trace);
            foreach (KeyValuePair<string, string> kvp in Game1.getCharacterFromName(args[0]).Dialogue)
            {
                ModFunctions.LogVerbose($"{kvp.Key}: {kvp.Value}");
            }
        }

        private void ReloadConfig(string command, string[] args)
        {
            UpdateConfig();
        }

        private List<Response> GenerateSexOptions(NPC target)
        {
            List<Response> choices;
            choices = new List<Response>();


            if (target.Age == 2) // 2 is a child - immediate yeet
            {
                ModFunctions.LogVerbose($"{target.Name} is a child.", LogLevel.Trace);
            }
            else
            {
                if ((TempRefs.IgnoreVillagerGender || target.Gender == 0 || target.Gender == 2) && Config.MilkMale) // Male and genderless
                {
                    choices.Add(new Response("milk_fast", "Fast BJ [20E]"));
                    choices.Add(new Response("BJ", "Give them a blowjob [20E]"));
                }

                if (TempRefs.IgnoreVillagerGender || target.Gender == 1 || target.Gender == 2) // Female and genderless
                {
                    if (Config.MilkFemale)
                    {
                        choices.Add(new Response("milk_fast", "Fast Milk [20E]"));
                        choices.Add(new Response("milk_start", "Milk them [20E]"));
                        choices.Add(new Response("eat_out", "Give Cunnilingus [30E] (partially implemented)")); //TODO Not written yet.
                    }
                }

                //choices.Add(new Response("get_eaten", "Ask them to eat you out [15E] (not implemented)")); //TODO not written yet.
                //choices.Add(new Response("sex", "Ask them for sex [50E](not implemented)")); //TODO not written yet.
            }

            choices.Add(new Response("abort", "Do nothing"));

            return choices;
        }

        public void DialoguesSet(Farmer who, string action)
        {
            if (who == null || action == null) return;

            if (Config.Verbose)
            {
                if (currentTarget != null)
                    Game1.addHUDMessage(new HUDMessage($"Chose {action}  with {currentTarget.Name}"));
            }

            if (action == null || action == "abort")
                return;
            else if (action == "time_freeze") //pauses in game time progression for 1min IRL
            {
                who.removeItemFromInventory(TempRefs.MilkQi);
                TimeFreezeTimer = 576000; // 1 minute timestop.
            }
            else if (action == "stamina_regen")
            {
                who.removeItemFromInventory(TempRefs.EldritchEnergy);
                RegenAmount = 100; // 100hp total restoration.

                //Remove this - testing only
                who.Stamina = 100;
            }
            else if (action == "self_milk") // farmer collects their own breast milk.
            {
                Game1.drawObjectDialogue(TempRefs.FarmerCollectionMilk);

                who.addItemToInventory(new sObject(TempRefs.MilkGeneric, 1, quality: 2));

                who.Stamina -= 150;
                //TempRefs.SelfMilkedToday = true;
            }
            else if (action == "self_cum") // farmer collects their own cum
            {
                Game1.drawObjectDialogue(TempRefs.FarmerCollectCum);

                who.addItemToInventory(new sObject(TempRefs.MilkSpecial, 1, quality: 2));

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
                Game1.drawDialogue(npc, $"That's flattering, but I don't like you enough for that. ({HeartCurrent}/{heartMin})");
                ModFunctions.LogVerbose($"{npc.Name} is heart level {HeartCurrent} and needs to be {heartMin}", LogLevel.Trace);
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
            int ItemCode;
            #endregion

            #region set item to give
            switch (npc.Name) //Give items to player
            {
                // Milk
                case "Abigail": ItemCode = TempRefs.MilkAbig; Quality = 2; Quantity = 2; break;
                case "Caroline": ItemCode = TempRefs.MilkCaro; Quality = 2; Quantity = 2; break;
                case "Emily": ItemCode = TempRefs.MilkEmil; Quality = 2; break;
                case "Evelyn": ItemCode = TempRefs.MilkEvel; Quality = 0; break;
                case "Haley": ItemCode = TempRefs.MilkHale; Quality = 2; Quantity = 2; break;
                case "Jodi": ItemCode = TempRefs.MilkJodi; Quality = 1; Quantity = 2; break;
                case "Leah": ItemCode = TempRefs.MilkLeah; Quality = 2; Quantity = 2; break;
                case "Marnie": ItemCode = TempRefs.MilkMarn; Quality = 1; Quantity = 3; break;
                case "Maru": ItemCode = TempRefs.MilkMaru; Quality = 2; Quantity = 2; break;
                case "Pam": ItemCode = TempRefs.MilkPam; Quality = 0; break;
                case "Penny": ItemCode = TempRefs.MilkPenn; Quality = 2; break;
                case "Robin": ItemCode = TempRefs.MilkRobi; Quality = 1; break;
                case "Sandy": ItemCode = TempRefs.MilkSand; Quality = 2; Quantity = 2; break;

                case "Sophia": ItemCode = TempRefs.MilkSophia; Quality = 2; Quantity = 2; break;
                case "Olivia": ItemCode = TempRefs.MilkOlivia; Quality = 1; break;
                case "Susan": ItemCode = TempRefs.MilkSusan; Quality = 2; Quantity = 2; break;
                case "Claire": ItemCode = TempRefs.MilkClaire; Quality = 1; break;

                // Cum
                case "Alex": ItemCode = TempRefs.MilkAlex; Quality = 2; Quantity = 2; break;
                case "Clint": ItemCode = TempRefs.MilkClint; Quality = 1; break;
                case "Demetrius": ItemCode = TempRefs.MilkDemetrius; Quality = 2; Quantity = 2; break;
                case "Elliott": ItemCode = TempRefs.MilkElliott; Quality = 2; break;
                case "George": ItemCode = TempRefs.MilkGeorge; Quality = 0; break;
                case "Gus": ItemCode = TempRefs.MilkGus; Quality = 1; Quantity = 2; break;
                case "Harvey": ItemCode = TempRefs.MilkHarv; Quality = 2; Quantity = 2; break;
                case "Kent": ItemCode = TempRefs.MilkKent; Quality = 2; Quantity = 2; break;
                case "Lewis": ItemCode = TempRefs.MilkLewis; Quality = 0; break;
                case "Linus": ItemCode = TempRefs.MilkLinus; Quality = 1; break;
                case "Pierre": ItemCode = TempRefs.MilkPierre; Quality = 1; break;
                case "Sam": ItemCode = TempRefs.MilkSam; Quality = 2; break;
                case "Sebastian": ItemCode = TempRefs.MilkSeb; Quality = 2; break;
                case "Shane": ItemCode = TempRefs.MilkShane; Quality = 1; break;
                case "Willy": ItemCode = TempRefs.MilkWilly; Quality = 1; break;

                //Magical
                case "Dwarf": ItemCode = TempRefs.MilkDwarf; Quality = 1; break;
                case "Krobus": ItemCode = TempRefs.MilkKrobus; Quality = 1; break;
                case "Mister Qi": ItemCode = TempRefs.MilkQi; Quality = 2; break;
                case "Wizard": ItemCode = TempRefs.MilkWiz; Quality = 2; break;

                case "Andy": ItemCode = TempRefs.MilkAndy; Quality = 1; break;
                case "Victor": ItemCode = TempRefs.MilkVictor; Quality = 2; break;
                case "Martin": ItemCode = TempRefs.MilkMartin; Quality = 1; Quantity = 2; break;

                default: //NPC's I don't know.
                    ItemCode = npc.Gender == 0 ? TempRefs.MilkSpecial : TempRefs.MilkGeneric;
                    Quality = 1;
                    ModFunctions.LogVerbose($"Couldn't find {npc.Name} in the list of items", LogLevel.Debug);
                    break;
            }

            // Don't remove this - It's a good way of speeding up for people.
            if (Config.StackMilk)
            {
                ItemCode = npc.Gender == 0 ? TempRefs.MilkSpecial : TempRefs.MilkGeneric;
                if (npc.Name == "Mister Qi"
                    || npc.Name == "Wizard"
                    || npc.Name == "Krobus"
                    || npc.Name == "Dwarf")
                {
                    ItemCode = TempRefs.MilkMagic;
                }
            }

            string SItemCode = $"[{ItemCode}]";
            AddItem = new sObject(ItemCode, Quantity, quality: Quality);
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

            ModFunctions.LogVerbose($"Trying to milk {npc.Name}. Will give item {ItemCode}: {ItemEditor.GetItemName(ItemCode)}, Category: {AddItem.Category} / {AddItem.getCategoryName()}", LogLevel.Trace);

            // If no male milking, don't give item.
            if ((npc.Gender == 0 & !Config.MilkMale) || !Config.CollectItems)
            {
                SItemCode = "";
                AddItem = null;
            }
            #endregion

            // Draw Dialogue
            npc.facePlayer(who);
            if (npc.Dialogue.TryGetValue(action, out string dialogues)) //Does npc have milking dialogue?
            {
                chosenString = GetRandomString(dialogues.Split(new string[] { "#split#" }, System.StringSplitOptions.None));

                #region move the whole item selection to a separate method.
                // Get Item code from ChosenString
                if (chosenString.Contains("["))
                {
                    //"{{spacechase0.JsonAssets/ObjectId: }}";
                    int start = chosenString.IndexOf("[") + 1;
                    int end = chosenString.LastIndexOf("]");
                    string val = chosenString[start..end];
                    ModFunctions.LogVerbose($"{npc.Name} value was {val}", LogLevel.Trace);
                    int.TryParse(val, out ItemCode);
                    chosenString = chosenString.Replace($"[{val}]", "");

                }

                // Get Quality from ChosenString
                if (chosenString.Contains("{Quality:"))
                {
                    //"{{spacechase0.JsonAssets/ObjectId: }}";
                    int start = chosenString.IndexOf("{Quality:");
                    int end = chosenString.IndexOf("}", start);

                    string val = chosenString.Substring(end - 1, 1);
                    ModFunctions.LogVerbose($"{npc.Name} Quality was {val}", LogLevel.Trace);
                    int.TryParse(val, out Quality);

                    AddItem.Quality = Quality;
                    chosenString = chosenString.Replace($"Quality:{val}", "").Replace("{}", "");
                }

                // Get Quantity from ChosenString
                if (chosenString.Contains("{Quantity:"))
                {
                    //"{{spacechase0.JsonAssets/ObjectId: }}";
                    int start = chosenString.IndexOf("{Quantity:");
                    int end = chosenString.IndexOf("}", start);

                    string val = chosenString.Substring(end - 1, 1);
                    ModFunctions.LogVerbose($"{npc.Name} Quantity was {val}", LogLevel.Trace);
                    int.TryParse(val, out Quantity);

                    AddItem.Stack = Quantity;
                    chosenString = chosenString.Replace($"Quantity:{val}", "").Replace("{}", "");
                }
                #endregion

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

                Game1.drawDialogue(npc, $"{chosenString.Trim()}");
                success = true;
            }
            else //if (action != "milk_fast")
            {
                ModFunctions.LogVerbose($"{npc.Name} failed to get anything for action {action}. Probably not written yet.", LogLevel.Info);
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
                        if (npc.Gender == 1)
                        {
                            chosenString = $"I've never been asked that by anyone else. Although, that DOES sound kinda hot.#$b#You spend the next few minutes slowly kneeding their breasts, collecting the milk in a jar you brought with you. {SItemCode}";
                        }
                        else if (npc.Gender == 0)
                        {
                            chosenString = $"You want my 'milk'? Erm, You ARE very attractive...#$b#*You quickly unzip their pants and pull out their cock. After a couple of quick licks to get them hard, you start sucking on them*#$b#I think I'm getting close! Here it comes! {SItemCode}";
                        }
                        else
                        {
                            chosenString = $"I'm sorry, but I don't produce anything you would want to collect. Perhaps we can do something else instead?";
                        }

                        Game1.drawDialogue(npc, chosenString);
                        success = true;
                        break;

                    case "eat_out":
                        chosenString = $"You want to go down on me? I don't think I've ever had a guy offer to do that without an ulterior motive before.$h" +
                            $"#$b#%*{npc.Name} quickly strips out of her lower garments, and opens her legs wide for you. You're greeted with a heady smell, and notice that her lips are starting to swell*" +
                            $"#$b#%*You lean in and start licking between {npc.Name}'s lips, tasting her sweet nectar as she buries her hands in your hair*" +
                            $"#$b#%*As her moans get louder you use your tongue to flick her clit, and she clenches her legs tightly around your head*" +
                            $"#$b#%*You gently suck on the nub, and then plunge your tongue as deeply into her as you can, shaking your tongue from side to side to stimulate {npc.Name} even more*" +
                            $"#$b#@, I'm cumming!";

                        Game1.drawDialogue(npc, chosenString);
                        success = true;
                        break;

                    case "get_eaten":
                        chosenString = $"I'm sorry, dialogue hasn't been written for that yet.";
                        Game1.drawDialogue(npc, chosenString);
                        success = true;
                        break;

                    case "BJ":
                        chosenString = $"You want my 'milk'? Erm, You ARE very attractive...#$b#*You quickly unzip their pants and pull out their cock. " +
                            $"After a couple of quick licks to get them hard, you start sucking on their penis*#$b#I think I'm getting close! Here it comes! {SItemCode}";
                        Game1.drawDialogue(npc, chosenString);
                        success = true;
                        break;

                    case "sex":
                        //TODO write four version of this for each gender configuration.

                        if (npc.Gender == 0 && who.IsMale) // Male player, male NPC.
                            chosenString = $"two dudes going at it";
                        else if (npc.Gender == 1 && who.IsMale) // Male player, female NPC.
                            chosenString = $"{who.Name} buries their cock deep inside {npc.Name}'s pussy";
                        else if (npc.Gender == 0 && !who.IsMale) // Female player, male NPC.
                            chosenString = $"{who.Name} climbs on top of {npc.Name}'s erect cock and plunges it deep inside them until they cum";
                        else // neither is male
                            if (HasStrapon(who))
                            chosenString = $"You put on your strapon and fuck {npc.Name} silly.";
                        else
                            chosenString = $"You and {npc.Name} lick, suck and finger each other into oblivion";

                        Game1.drawDialogue(npc, chosenString);
                        success = true;
                        break;

                    default:
                        chosenString = $"I don't have any dialogue written for that. Sorry.";
                        Game1.drawDialogue(npc, chosenString);
                        Monitor.Log($"{action} for {npc.Name} wasn't found");
                        break;
                }
            }

        cleanup:
            // Cleanup
            if (success)
            {
                who.stamina -= energyCost;
                switch (action)
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

        private static bool HasStrapon(Farmer who)
        {
            return who.hasItemInInventoryNamed("Strapon") || true;
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
                    who.getTileX(),
                    who.getTileY()
            };
        }

        #endregion

        #region Testing
        private void SendAllMail(string command, string[] args)
        {
            Farmer Who = Game1.player;
            foreach ( string s in MailEditor.Mail)
            {
                Who.mailbox.Add(s);
            }
        }

        private void AddAllQuests(string command, string[] args)
        {
            Farmer who = Game1.player;
            foreach (KeyValuePair<string, int> kvp in QuestEditor.QuestIDs)
            {
                ModFunctions.LogVerbose($"Adding quests {kvp.Value}-{kvp.Key}");
                try
                {
                    who.addQuest(kvp.Value);
                }
                catch (Exception ex)
                {
                    ModFunctions.LogVerbose($"Failed to add quest {kvp.Key} with error {ex.Message}");
                }
            }
        }

        #endregion
    }

    public delegate void ActionNPC(object sender, ActionNPCEventArgs e);

    public class ActionNPCEventArgs : EventArgs
    {
        public Farmer Who { get; set; }
        public NPC Recipient { get; set; }
        public string Action { get; set; }
        public GameLocation Map { get; set; }
    }
}
