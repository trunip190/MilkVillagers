using System.Collections.Generic;
using System.Linq;
using GenericModConfigMenu;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using MilkVillagers.Asset_Editors;

namespace MilkVillagers
{
    public class ModEntry : Mod
    {
        #region class variables
        private int[] target;
        private bool running;
        private bool runOnce;
        private NPC currentTarget;

        // Time freeze stuff
        private float Countdown = 0;

        // Adding item stuff
        Object AddItem;
        Farmer CurrentFarmer;

        // Asset Editors.
        private RecipeEditor _recipeEditor;
        private DialogueEditor _dialogueEditor;
        private ItemEditor _itemEditor;
        private QuestEditor _questEditor;
        private EventEditor _eventEditor;
        private MailEditor _mailEditor;

        private ModConfig Config;
        public int CurrentQuest = 0; //currently loaded quest id.
        List<int> CurrentQuests = new List<int>();

        #endregion


        public override void Entry(IModHelper helper)
        {
            Config = helper.ReadConfig<ModConfig>();
            _recipeEditor = new RecipeEditor();
            _dialogueEditor = new DialogueEditor { ExtraContent = Config.ExtraDialogue };
            _itemEditor = new ItemEditor();
            _questEditor = new QuestEditor();
            _eventEditor = new EventEditor();
            _mailEditor = new MailEditor();

            if (helper == null)
            {
                ModFunctions.LogVerbose("helper is null.");
            }
            TempRefs.Helper = helper;
            TempRefs.Monitor = Monitor;

            Helper.Events.Display.MenuChanged += Display_MenuChanged;
            helper.Events.GameLoop.DayStarted += GameLoop_DayStarted;
            helper.Events.GameLoop.GameLaunched += OnGameLaunched;
            helper.Events.GameLoop.SaveLoaded += GameLoop_SaveLoaded;
            helper.Events.GameLoop.TimeChanged += GameLoop_TimeChanged;
            helper.Events.GameLoop.DayEnding += GameLoop_DayEnding;
            helper.Events.GameLoop.OneSecondUpdateTicked += GameLoop_OneSecondUpdateTicked;
            helper.Events.Input.ButtonPressed += OnButtonPressed;

            //helper.Events.Player.Warped += (new EventHandler<WarpedEventArgs>(Player_Warped));
        }

        #region Game OnEvent Triggers
        private void OnGameLaunched(object sender, GameLaunchedEventArgs e)
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

                //api.RegisterSimpleOption(
                //    mod: this.ModManifest,
                //    optionName: "Iliress' Dialogue",
                //    optionDesc: "Enable/disable 3rd party dialogue?",
                //    optionGet: () => this.Config.ThirdParty,
                //    optionSet: value => this.Config.ThirdParty = value
                //);

                api.RegisterSimpleOption(
                    mod: this.ModManifest,
                    optionName: "Debug mode",
                    optionDesc: "Enable debug mode content",
                    optionGet: () => this.Config.Debug,
                    optionSet: value => this.Config.Debug = value
                );

                api.RegisterSimpleOption(
                    mod: this.ModManifest,
                    optionName: "Verbose Dialogue",
                    optionDesc: "Enable verbose dialogue for tracking errors",
                    optionGet: () => this.Config.Verbose,
                    optionSet: value => this.Config.Verbose = value
                );
            }
            #endregion

            Helper.Content.AssetEditors.Add(_itemEditor);
            Helper.Content.AssetEditors.Add(_dialogueEditor);
            Helper.Content.AssetEditors.Add(_questEditor); //TODO needs fully writing.
            Helper.Content.AssetEditors.Add(_recipeEditor);
            if (Config.Debug) Helper.Content.AssetEditors.Add(_eventEditor);
            Helper.Content.AssetEditors.Add(_mailEditor);

            TempRefs.thirdParty = Config.ThirdParty;
            TempRefs.Verbose = Config.Verbose;
        }

        private void GameLoop_SaveLoaded(object sender, SaveLoadedEventArgs e)
        {
            ModFunctions.LogVerbose("Loaded savegame.");

            if (runOnce)
                return;

            GetItemCodes();

            // Recipes.
            CorrectRecipes();


            foreach (Farmer who in Game1.getAllFarmers())
                AddAllRecipes(who);

            runOnce = true;
        }

        private void GameLoop_DayStarted(object sender, DayStartedEventArgs e)
        {
            if (TempRefs.milkedtoday == null)
                ModFunctions.LogVerbose("TempRefs not set");
            else
            {
                ModFunctions.LogVerbose($"TempRefs is set. Cleared {TempRefs.milkedtoday.Count}");
                TempRefs.milkedtoday.Clear();
                TempRefs.SexToday.Clear();
            }


            //TODO change this for multiplayer
            Farmer who = Game1.player;

            //Add recipe to stock.
            //Dictionary<ISalable, int[]> stock = Utility.getSaloonStock();
            //foreach (KeyValuePair<ISalable, int[]> kp in stock)
            //{
            //    LogVerbose($"{kp.Key.DisplayName}: price ${kp.Value[0]}, Quantity: {kp.Value[1]}", LogLevel.Trace);
            //}

            //TODO set up preconditions for these.
            if (Config.Debug)
            {

                if (!who.mailReceived.Contains("MilkingAbi"))
                    who.mailbox.Add("MilkingAbi");

                //if (Config.Verbose)
                //    OutputQuests();

            }

            // Tutorial
            if (!who.mailReceived.Contains("MilkButton1"))
                who.mailbox.Add("MilkButton1");
            if (!who.mailReceived.Contains("MilkButton2"))
                who.mailbox.Add("MilkButton2");

            if (Config.Quests) //TODO This section picks the next quest. Need to rewrite/decide how to send next mail/quest.
            {
                if (who.getFriendshipHeartLevelForNPC("Abigail") > 7 && !who.mailReceived.Contains("AbiEggplant"))
                {
                    who.mailForTomorrow.Add("AbiEggplant");

                    //_questEditor.data[TempRefs.QuestID1] = _questEditor.data[TempRefs.QuestID1].Replace($"{TempRefs.QuestID2}", $"{TempRefs.QuestIDWait}");
                    //_questEditor.data[TempRefs.QuestIDWait] = _questEditor.data[TempRefs.QuestIDWait].Replace("594800", $"{TempRefs.QuestID2}");
                }

                // Send new mail checks
                if (who.mailReceived.Contains("AbiEggplantT") && !who.mailReceived.Contains("AbiCarrots") && !who.mailbox.Contains("AbiCarrots"))
                    who.mailForTomorrow.Add("AbiCarrots");
                if (who.mailReceived.Contains("AbiCarrotsT") && !who.mailReceived.Contains("AbiRadishes") && !who.mailbox.Contains("AbiRadishes"))
                    who.mailForTomorrow.Add("AbiRadishes");
                if (who.mailReceived.Contains("AbiRadishesT") && !who.mailReceived.Contains("AbiSurprise") && !who.mailbox.Contains("AbiSurprise"))
                    who.mailForTomorrow.Add("AbiSurpriseT");
                if (who.mailReceived.Contains("AbiSurpriseT") && !who.mailReceived.Contains("MaruSample") && !who.mailbox.Contains("MaruSample"))
                    who.mailForTomorrow.Add("MaruSample");
                if (who.mailReceived.Contains("MaruSampleT") && !who.mailReceived.Contains("GeorgeMilk") && !who.mailbox.Contains("GeorgeMilk"))
                    who.mailForTomorrow.Add("GeorgeMilk");
                if (who.mailReceived.Contains("GeorgeMilkT") && !who.mailReceived.Contains("LeahNudePainting") && !who.mailbox.Contains("LeahNudePainting"))
                    who.mailForTomorrow.Add("LeahNudePainting");

            }
        }

        private void GameLoop_TimeChanged(object sender, TimeChangedEventArgs e)
        {
            //TODO change for multiplayer
            foreach (Farmer who in Game1.getOnlineFarmers())
            {
                if (!who.mailReceived.Contains("AbiEggplant")) // need to add in last quest as well so that it stops checking after all quests are done.
                    return;

                QuestChecks(who);
            }
        }

        private void GameLoop_OneSecondUpdateTicked(object sender, OneSecondUpdateTickedEventArgs e)
        {
            if (Countdown > 0)
            {
                Monitor.Log($"{Countdown} - {Game1.gameTimeInterval}", LogLevel.Alert);
                Countdown -= Game1.gameTimeInterval;
                Game1.gameTimeInterval = 0;
            }
        }

        private void GameLoop_DayEnding(object sender, DayEndingEventArgs e)
        {
            //TODO not sure about this
            foreach (Farmer who in Game1.getOnlineFarmers())
                QuestChecks(who);
        }

        private void Display_MenuChanged(object sender, MenuChangedEventArgs e)
        {
            if (AddItem == null) return;

            string oldMenu = e.OldMenu != null ? e.OldMenu.ToString() : "";
            string newMenu = e.NewMenu != null ? e.NewMenu.ToString() : "";

            if (oldMenu == "StardewValley.Menus.DialogueBox" && newMenu == "")
            {
                Monitor.Log($"adding {AddItem.Name} to {CurrentFarmer.Name}", LogLevel.Alert);
                CurrentFarmer.addItemToInventory(AddItem);
                AddItem = null;
                CurrentFarmer = null;
            }
            else
                Monitor.Log($"was {oldMenu} {newMenu}", LogLevel.Alert);

        }

        #endregion

        #region Quest methods
        private void QuestChecks(Farmer who)
        {
            if (CurrentQuest == 0)
            {
                //TODO rewrite this so it can check multiple quests at once.
                if (HasQuest(who, TempRefs.QuestAbi1)) { CurrentQuest = TempRefs.QuestAbi1; CurrentQuests.Add(TempRefs.QuestAbi1); }
                else if (HasQuest(who, TempRefs.QuestAbi2)) { CurrentQuest = TempRefs.QuestAbi2; CurrentQuests.Add(TempRefs.QuestAbi2); }
                else if (HasQuest(who, TempRefs.QuestAbi3)) { CurrentQuest = TempRefs.QuestAbi3; CurrentQuests.Add(TempRefs.QuestAbi3); }
                else if (HasQuest(who, TempRefs.QuestAbi4)) { CurrentQuest = TempRefs.QuestAbi4; CurrentQuests.Add(TempRefs.QuestAbi4); }
                else if (HasQuest(who, TempRefs.QuestMaru1)) { CurrentQuest = TempRefs.QuestMaru1; CurrentQuests.Add(TempRefs.QuestMaru1); }
                else if (HasQuest(who, TempRefs.QuestGeorge1)) { CurrentQuest = TempRefs.QuestGeorge1; CurrentQuests.Add(TempRefs.QuestGeorge1); }
                else if (HasQuest(who, TempRefs.QuestSeb1)) { CurrentQuest = TempRefs.QuestSeb1; CurrentQuests.Add(TempRefs.QuestSeb1); }
                else if (HasQuest(who, TempRefs.QuestSeb2)) { CurrentQuest = TempRefs.QuestSeb2; CurrentQuests.Add(TempRefs.QuestSeb2); }
                else if (HasQuest(who, TempRefs.QuestLeah1)) { CurrentQuest = TempRefs.QuestLeah1; CurrentQuests.Add(TempRefs.QuestLeah1); }

                #region need to convert over this section
                if (CurrentQuests.Count > 0) // New version
                {
                    foreach (int q in CurrentQuests)
                    {
                        ModFunctions.LogVerbose($"Watching quest ID {q}", LogLevel.Alert);
                    }
                }
                if (CurrentQuest != 0) // Old version
                {
                    ModFunctions.LogVerbose($"Watching quest ID {CurrentQuest}", LogLevel.Alert);

                    _ = RemoveQuest(who, TempRefs.QuestIDWait);
                }
                #endregion
            }
            foreach (int q in CurrentQuests)
            {
                //if (CurrentQuest != 0 && !HasQuest(who, CurrentQuest))
                if (!HasQuest(who, q))
                {
                    ModFunctions.LogVerbose($"Quest ID {CurrentQuest} has finished");

                    //TODO move this to a separate method
                    // Work out what mail completion to send.
                    string NextMail = "";
                    if (!who.mailReceived.Contains("AbiEggplantT")) // Quest 1 complete
                        NextMail = "AbiEggplantT";
                    else if (!who.mailReceived.Contains("AbiCarrotsT"))  // Quest 2 complete
                        NextMail = "AbiCarrotsT";
                    else if (!who.mailReceived.Contains("AbiRadishesT"))  // Quest 3 complete             
                        NextMail = "AbiRadishesT";
                    else if (!who.mailReceived.Contains("AbiSurpriseT"))  // Quest 4 complete            
                        NextMail = "AbiSurpriseT";
                    else if (!who.mailReceived.Contains("MaruSampleT"))  // Quest 5 complete
                        NextMail = "MaruSampleT";
                    else if (!who.mailReceived.Contains("GeorgeMilkT"))  // Quest 6 complete
                        NextMail = "GeorgeMilkT";
                    else if (!who.mailReceived.Contains("LeahNudePaintingT"))  // Quest 6 complete
                        NextMail = "LeahNudePaintingT";
                    else
                    {
                        // No mail item associated with the quest completion.
                    }

                    if (NextMail != "")
                    {
                        ModFunctions.LogVerbose($"adding mail {NextMail} to mailbox", LogLevel.Alert);

                        who.mailForTomorrow.Add(NextMail);
                    }

                    CurrentQuest = 0;
                }
            }
        }

        private bool RemoveQuest(Farmer who, int id)
        {
            foreach (var q in who.questLog)
            {
                if (q.id == id)
                {
                    who.questLog.Remove(q);
                    return true;
                }
            }
            return false;
        }

        private bool HasQuest(Farmer who, int id)
        {
            foreach (var q in who.questLog)
            {
                if (q.id == id)
                {
                    return true;
                }
            }
            return false;
        }

        private void OutputQuests()
        {
            if (Config.Verbose)
            {
                foreach (var d in _questEditor.QuestData)
                {
                    ModFunctions.LogVerbose($"{d.Key}: {d.Value}");
                }
            }
        }
        #endregion

        private bool CheckAll()
        {
            bool result = true;

            ModFunctions.LogVerbose("Checking recipes...");
            if (!_recipeEditor.CheckAll())
                result = false;

            ModFunctions.LogVerbose("Checking items...");
            if (!_itemEditor.CheckAll())
                result = false;

            ModFunctions.LogVerbose("Checking quests...");
            if (!_questEditor.CheckAll())
                result = false;
            else
                _questEditor.Report();

            ModFunctions.LogVerbose("Checking events...");
            if (!_eventEditor.CheckAll())
                result = false;

            ModFunctions.LogVerbose("Checking mail...");
            _mailEditor.Report();

            return result;
        }

        public void UpdateConfig(string key, string value)
        {
            _itemEditor.RemoveInvalid(Config.MilkMale, Config.MilkFemale);
            _recipeEditor.RemoveInvalid(Config.MilkMale, Config.MilkFemale);
        }

        private void AddAllRecipes(Farmer who)
        {
            //TODO move this to be a quest reward or something.
            ModFunctions.LogVerbose($"Adding in {5} recipes", LogLevel.Alert);

            if (!who.cookingRecipes.ContainsKey("Milkshake") && Config.MilkFemale)
                who.cookingRecipes.Add("Milkshake", 0);

            if (!who.cookingRecipes.ContainsKey("'Protein' Shake") && Config.MilkMale)
                who.cookingRecipes.Add("'Protein' Shake", 0);

            if (!who.cookingRecipes.ContainsKey("Special Juice") && Config.MilkFemale && Config.MilkMale)
                who.cookingRecipes.Add("Special Juice", 0);

            if (!who.craftingRecipes.ContainsKey("Generic Milk") && Config.MilkFemale)
                who.craftingRecipes.Add("Generic Milk", 0);

            if (!who.craftingRecipes.ContainsKey("Generic Cum") && Config.MilkMale)
                who.craftingRecipes.Add("Generic Cum", 0);

            _recipeEditor.RemoveInvalid(Config.MilkMale, Config.MilkFemale);
        }

        private void GetItemCodes()
        {
            LogLevel Defcon = LogLevel.Trace;

            ModFunctions.LogVerbose($"Correcting item codes");

            int num1 = 0;
            int num2 = 0;


            foreach (KeyValuePair<int, string> kvp in _itemEditor.Report())
            {
                string[] strArray = kvp.Value.Split('/');
                int ID = 0;

                switch (strArray[0])
                {
                    #region girls
                    case "Abigail's Milk": TempRefs.MilkAbig = kvp.Key; ID = kvp.Key; goto increment;
                    case "Caroline's Milk": TempRefs.MilkCaro = kvp.Key; ID = kvp.Key; goto increment;
                    case "Emily's Milk": TempRefs.MilkEmil = kvp.Key; ID = kvp.Key; goto increment;
                    case "Evelyn's Milk": TempRefs.MilkEvel = kvp.Key; ID = kvp.Key; goto increment;
                    case "Haley's Milk": TempRefs.MilkHale = kvp.Key; ID = kvp.Key; goto increment;
                    case "Jodi's Milk": TempRefs.MilkJodi = kvp.Key; ID = kvp.Key; goto increment;
                    case "Leah's Milk": TempRefs.MilkLeah = kvp.Key; ID = kvp.Key; goto increment;
                    case "Marnie's Milk": TempRefs.MilkMarn = kvp.Key; ID = kvp.Key; goto increment;
                    case "Maru's Milk": TempRefs.MilkMaru = kvp.Key; ID = kvp.Key; goto increment;
                    case "Pam's Milk": TempRefs.MilkPam = kvp.Key; ID = kvp.Key; goto increment;
                    case "Penny's Milk": TempRefs.MilkPenn = kvp.Key; ID = kvp.Key; goto increment;
                    case "Robin's Milk": TempRefs.MilkRobi = kvp.Key; ID = kvp.Key; goto increment;
                    case "Sandy's Milk": TempRefs.MilkSand = kvp.Key; ID = kvp.Key; goto increment;
                    #endregion

                    #region guys
                    case "Alex's Cum": TempRefs.MilkAlex = kvp.Key; ID = kvp.Key; goto increment;
                    case "Clint's Cum": TempRefs.MilkClint = kvp.Key; ID = kvp.Key; goto increment;
                    case "Demetrius's Cum": TempRefs.MilkDemetrius = kvp.Key; ID = kvp.Key; goto increment;
                    case "Elliott's Cum": TempRefs.MilkElliott = kvp.Key; ID = kvp.Key; goto increment;
                    case "George's Cum": TempRefs.MilkGeorge = kvp.Key; ID = kvp.Key; goto increment;
                    case "Gil's Cum": TempRefs.MilkGil = kvp.Key; ID = kvp.Key; goto increment;
                    case "Gunther's Cum": TempRefs.MilkGunther = kvp.Key; ID = kvp.Key; goto increment;
                    case "Gus's Cum": TempRefs.MilkGus = kvp.Key; ID = kvp.Key; goto increment;
                    case "Harvey's Cum": TempRefs.MilkHarv = kvp.Key; ID = kvp.Key; goto increment;
                    case "Kent's Cum": TempRefs.MilkKent = kvp.Key; ID = kvp.Key; goto increment;
                    case "Lewis's Cum": TempRefs.MilkLewis = kvp.Key; ID = kvp.Key; goto increment;
                    case "Linus's Cum": TempRefs.MilkLinus = kvp.Key; ID = kvp.Key; goto increment;
                    case "Marlon's Cum": TempRefs.MilkMarlon = kvp.Key; ID = kvp.Key; goto increment;
                    case "Morris's Cum": TempRefs.MilkMorris = kvp.Key; ID = kvp.Key; goto increment;
                    case "Pierre's Cum": TempRefs.MilkPierre = kvp.Key; ID = kvp.Key; goto increment;
                    case "Sam's Cum": TempRefs.MilkSam = kvp.Key; ID = kvp.Key; goto increment;
                    case "Sebastian's Cum": TempRefs.MilkSeb = kvp.Key; ID = kvp.Key; goto increment;
                    case "Shane's Cum": TempRefs.MilkShane = kvp.Key; ID = kvp.Key; goto increment;
                    case "Willy's Cum": TempRefs.MilkWilly = kvp.Key; ID = kvp.Key; goto increment;
                    #endregion

                    #region Magical
                    case "Dwarf's Milk": TempRefs.MilkDwarf = kvp.Key; ID = kvp.Key; goto increment;
                    case "Krobus's Cum": TempRefs.MilkKrobus = kvp.Key; ID = kvp.Key; goto increment;
                    case "Mr. Qi's Essence": TempRefs.MilkQi = kvp.Key; ID = kvp.Key; goto increment;
                    case "Wizard's Cum": TempRefs.MilkWiz = kvp.Key; ID = kvp.Key; goto increment;
                    case "Magical Essence": TempRefs.MilkMagic = kvp.Key; ID = kvp.Key; goto increment;
                    #endregion

                    #region Extra Items
                    case "Woman's Milk": TempRefs.MilkGeneric = kvp.Key; ID = kvp.Key; goto increment;
                    case "Special Milk": TempRefs.MilkSpecial = kvp.Key; ID = kvp.Key; goto increment;
                    case "Magic Essence": TempRefs.MilkMagic = kvp.Key; ID = kvp.Key; goto increment;
                    case "Protein Shake": TempRefs.ProteinShake = kvp.Key; ID = kvp.Key; goto increment;
                    case "Special Milkshake": TempRefs.MilkShake = kvp.Key; ID = kvp.Key; goto increment;
                    case "Super Juice": TempRefs.SuperJuice = kvp.Key; ID = kvp.Key; goto increment;
                    #endregion

                    #region Other mods
                    case "Sophia's Milk": TempRefs.MilkSophia = kvp.Key; ID = kvp.Key; goto increment;
                    case "Andy's Cum": TempRefs.MilkAndy = kvp.Key; ID = kvp.Key; goto increment;
                    case "Claire's Milk": TempRefs.MilkClaire = kvp.Key; ID = kvp.Key; goto increment;
                    case "Martin's Cum": TempRefs.MilkMartin = kvp.Key; ID = kvp.Key; goto increment;
                    case "Susan's Milk": TempRefs.MilkSusan = kvp.Key; ID = kvp.Key; goto increment;
                    case "Victor's Cum": TempRefs.MilkVictor = kvp.Key; ID = kvp.Key; goto increment;
                    case "Olivia's Milk": TempRefs.MilkOlivia = kvp.Key; ID = kvp.Key; goto increment;
                    #endregion

                    default:
                        if (strArray[0].ToLower().Contains("milk") || strArray[0].ToLower().Contains("cum") || strArray[0].ToLower().Contains("essence"))
                        {
                            num1++;
                            ModFunctions.LogVerbose($"{strArray[0]} wasn't added.", LogLevel.Info);
                        }
                        goto increment;
                }

            increment:
                {
                    if (ID != 0)
                    {
                        ModFunctions.LogVerbose($"{strArray[0]} added. {ID}", Defcon);
                        ++num2;
                    }
                }
            }

            TempRefs.loaded = true;
            ModFunctions.LogVerbose($"Loaded {num2}/{num1 + num2} items", Defcon);

            // Re-fix items for some reason.
            _itemEditor.SetItems();

            _itemEditor.RemoveInvalid(Config.MilkMale, Config.MilkFemale);
        }

        private void CorrectRecipes()
        {
            _recipeEditor.SetCooking();
            _recipeEditor.SetCrafting();

            //_recipeEditor.CookingData["'Protein' Shake"] = $"{TempRefs.CumType} 3/10 10/{TempRefs.ProteinShake}/default/'Protein' shake";
            //_recipeEditor.CookingData["Milkshake"] = $"{TempRefs.MilkType} 3/10 10/{TempRefs.MilkShake}/default/Milkshake";
            //_recipeEditor.CookingData["Super Juice"] = $"{TempRefs.MilkType} 2 {TempRefs.CumType} 2/10 10/{TempRefs.MilkShake}/default/Super Juice";

            //_recipeEditor.data["Woman's Milk"] = $"{TempRefs.MilkType} 1/10 10/{TempRefs.MilkGeneric}/null/Woman's Milk";
            //_recipeEditor.data["Generic Cum"] = $"{TempRefs.CumType} 1/10 10/{TempRefs.MilkSpecial}/null/Special Milk";

        }

        private void OnButtonPressed(object sender, ButtonPressedEventArgs e)
        {
            if (running)
            {
                return;
            }

            //switch for multiplayer.
            Farmer who = Game1.player;
            running = false;

            if (!Context.IsWorldReady)
                return;

            target = GetNewPos(who.FacingDirection, FarmerPos(who)[0], FarmerPos(who)[1]);
            SButton button = e.Button;

            if (Config.Debug && button == SButton.P)
            {
                CheckAll();
                if (!who.mailReceived.Contains("5948MaruStart"))
                    who.mailReceived.Add("5948MaruStart");
                else
                    Game1.warpFarmer("ScienceHouse", 6, 23, false);

                Monitor.Log($"Current item {who.CurrentItem.parentSheetIndex}: {who.CurrentItem.getCategoryName()}/{who.CurrentItem.category}", LogLevel.Alert);
                Monitor.Log($"Farmer is at {who.getTileX()}, {who.getTileY()}");

                //who.mailbox.Add("PennyLibrary");
            }
            else if (button == Config.MilkButton)
            {
                NPC target = ModFunctions.FindTarget(who.currentLocation, this.target, FarmerPos(who));
                if (target != null)
                {
                    //if (Config.Debug)
                    //{
                    //TODO Filter choices by gender?
                    List<Response> choices;
                    if (target.gender == 0)
                    {
                        choices = new List<Response>();
                        if (Config.MilkMale)
                            choices.Add(new Response("milk_fast", "Fast BJ"));

                        choices.Add(new Response("BJ", "Give them a blowjob"));
                    }
                    else
                    {
                        choices = new List<Response>();
                        if (Config.MilkFemale)
                        {
                            choices.Add(new Response("milk_start", "Milk them"));
                            choices.Add(new Response("milk_fast", "Fast Milk"));
                        }
                        //choices.Add(new Response("eat_out", "Give Cunnilingus")); //TODO Not written yet.
                    }

                    //choices.Add(new Response("cunni", "Ask them to eat you out")); //TODO not written yet.
                    //choices.Add(new Response("sex", "Ask them for sex")); //TODO not written yet.
                    choices.Add(new Response("abort", "Do nothing"));

                    currentTarget = target;
                    running = false;

                    Game1.currentLocation.createQuestionDialogue($"What do you want to do with {target.name}?", choices.ToArray(), new GameLocation.afterQuestionBehavior(DialoguesSet));
                    //}
                    //else
                    //{
                    //if (target.gender == 1)
                    //    ActionOnNPC(target, who);
                    //else
                    //    ActionOnNPC(target, who, "BJ");
                    //}
                }
                else if (Config.Debug)
                {
                    List<Response> options = new List<Response>();

                    if (who.IsMale && !TempRefs.SelfMilkedToday)
                    {
                        options.Add(new Response("self_milk", "Collect your own cum"));
                    }
                    else if (!TempRefs.SelfMilkedToday)
                    {

                        options.Add(new Response("self_milk", "Milk yourself"));
                    }

                    if (who.hasItemInInventory(TempRefs.MilkQi, 1))
                    {
                        options.Add(new Response("time_freeze", "Freeze time"));
                    }

                    options.Add(new Response("abort", "Nothing"));

                    Game1.currentLocation.createQuestionDialogue($"What would you like to do?", options.ToArray(), new GameLocation.afterQuestionBehavior(DialoguesSet));
                }
            }
            else
                ModFunctions.LogVerbose($"Button {e.Button} not registered to a function.");
        }

        public void DialoguesSet(Farmer who, string action)
        {
            if (Config.Verbose)
                Game1.addHUDMessage(new HUDMessage($"Chose {action}  with {currentTarget.name}"));

            if (action == null || action == "abort")
                return;
            else if (action == "time_freeze")
            {
                who.removeItemFromInventory(TempRefs.MilkQi);
                Countdown = 576000; // 1 minute.
            }
            else if (action == "self_milk")
            {
                string milkText = who.isMale ? $"You jerk off and collect your own cum in a bottle. [{TempRefs.MilkSpecial}]" : $"You knead your breasts and collect the milk in a bottle. [{TempRefs.MilkGeneric}]";
                Game1.addHUDMessage(new HUDMessage(milkText));
                TempRefs.SelfMilkedToday = true;
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
            // Removed option for requiring a milk pail
            //if (Config.NeedTool && Game1.player.CurrentTool.GetType().ToString() != typeof(StardewValley.Tools.MilkPail).ToString())
            //{
            //    Game1.addHUDMessage(new HUDMessage("You need to be holding a pail to milk people."));
            //    return;
            //}


            bool success = false;
            int heartMin = 0;
            int HeartCurrent = who.getFriendshipHeartLevelForNPC(npc.name);
            string chosenString;
            int Quality;

            //TODO move to static class of actions.
            switch (action)
            {
                case "milk_start":
                    heartMin = 8;
                    break;

                case "milk_fast":
                    heartMin = 8;
                    break;

                case "BJ":
                    heartMin = 6;
                    break;

                case "eat_out":
                    heartMin = 6;
                    break;

                case "get_eaten":
                    heartMin = 7;
                    break;

                case "sex":
                    heartMin = 10;
                    break;
            }

            #region validity checks
            if (npc.age == 2) // 2 is a child - immediate yeet
            {
                Monitor.Log($"{npc.name} is {npc.age}", LogLevel.Debug);
                return;
            }

            if (HeartCurrent < heartMin && npc.CanSocialize)//  npc.name != "Mister Qi") // Check if the NPC likes you enough.
            {
                Game1.drawDialogue(npc, $"That's flattering, but I don't like you enough for that. ({HeartCurrent}/{heartMin})");
                ModFunctions.LogVerbose($"{npc.name} is heart level {HeartCurrent} and needs to be {heartMin}", LogLevel.Alert);
                return;
            }


            //milked today
            //TODO rewrite this to base it off of the choice.
            if ((action == "milk_start" || action == "milk_fast") && TempRefs.milkedtoday.Contains(npc))
            {
                if (Config.Verbose)
                    Game1.addHUDMessage(new HUDMessage($"{npc.name} has already been milked today."));
                return;
            }
            if ((action == "sex" || action == "BJ") && TempRefs.SexToday.Contains(npc))
            {
                if (Config.Verbose)
                    Game1.addHUDMessage(new HUDMessage($"{npc.name} has already had sex with you today."));
                return;
            }
            #endregion

            #region set item to give
            int ItemCode = TempRefs.MilkGeneric;

            switch (npc.Name) //Give items to player
            {
                // Milk
                case "Abigail": ItemCode = TempRefs.MilkAbig; Quality = 3; break;
                case "Caroline": ItemCode = TempRefs.MilkCaro; Quality = 3; break;
                case "Emily": ItemCode = TempRefs.MilkEmil; Quality = 3; break;
                case "Evelyn": ItemCode = TempRefs.MilkEvel; Quality = 0; break;
                case "Haley": ItemCode = TempRefs.MilkHale; Quality = 3; break;
                case "Jodi": ItemCode = TempRefs.MilkJodi; Quality = 2; break;
                case "Leah": ItemCode = TempRefs.MilkLeah; Quality = 3; break;
                case "Marnie": ItemCode = TempRefs.MilkMarn; Quality = 2; break;
                case "Maru": ItemCode = TempRefs.MilkMaru; Quality = 3; break;
                case "Pam": ItemCode = TempRefs.MilkPam; Quality = 0; break;
                case "Penny": ItemCode = TempRefs.MilkPenn; Quality = 3; break;
                case "Robin": ItemCode = TempRefs.MilkRobi; Quality = 2; break;
                case "Sandy": ItemCode = TempRefs.MilkSand; Quality = 3; break;

                case "Sophia": ItemCode = TempRefs.MilkSophia; Quality = 3; break;
                case "Olivia": ItemCode = TempRefs.MilkOlivia; Quality = 2; break;
                case "Susan": ItemCode = TempRefs.MilkSusan; Quality = 3; break;
                case "Claire": ItemCode = TempRefs.MilkClaire; Quality = 2; break;

                // Cum
                case "Alex": ItemCode = TempRefs.MilkAlex; Quality = 3; break;
                case "Clint": ItemCode = TempRefs.MilkClint; Quality = 1; break;
                case "Demetrius": ItemCode = TempRefs.MilkDemetrius; Quality = 3; break;
                case "Elliott": ItemCode = TempRefs.MilkElliott; Quality = 3; break;
                case "George": ItemCode = TempRefs.MilkGeorge; Quality = 0; break;
                case "Gus": ItemCode = TempRefs.MilkGus; Quality = 1; break;
                case "Harvey": ItemCode = TempRefs.MilkHarv; Quality = 3; break;
                case "Kent": ItemCode = TempRefs.MilkKent; Quality = 3; break;
                case "Lewis": ItemCode = TempRefs.MilkLewis; Quality = 0; break;
                case "Linus": ItemCode = TempRefs.MilkLinus; Quality = 2; break;
                case "Pierre": ItemCode = TempRefs.MilkPierre; Quality = 1; break;
                case "Sam": ItemCode = TempRefs.MilkSam; Quality = 3; break;
                case "Sebastian": ItemCode = TempRefs.MilkSeb; Quality = 3; break;
                case "Shane": ItemCode = TempRefs.MilkShane; Quality = 2; break;
                case "Willy": ItemCode = TempRefs.MilkWilly; Quality = 1; break;

                //Magical
                case "Dwarf": ItemCode = TempRefs.MilkDwarf; Quality = 0; break;
                case "Krobus": ItemCode = TempRefs.MilkKrobus; Quality = 2; break;
                case "Mister Qi": ItemCode = TempRefs.MilkQi; Quality = 3; break;
                case "Wizard": ItemCode = TempRefs.MilkWiz; Quality = 3; break;

                case "Andy": ItemCode = TempRefs.MilkAndy; Quality = 2; break;
                case "Victor": ItemCode = TempRefs.MilkVictor; Quality = 3; break;
                case "Martin": ItemCode = TempRefs.MilkMartin; Quality = 2; break;

                default: //NPC's I don't know.
                    ItemCode = npc.gender == 0 ? TempRefs.MilkSpecial : TempRefs.MilkGeneric;
                    Quality = 1;
                    ModFunctions.LogVerbose($"Couldn't find {npc.name} in the list of items", LogLevel.Debug);
                    break;
            }

            // Don't remove this - It's a good way of speeding up for people.
            if (Config.StackMilk)
            {
                ItemCode = npc.gender == 0 ? TempRefs.MilkSpecial : TempRefs.MilkGeneric;
                if (npc.Name == "Mister Qi"
                    || npc.name == "Wizard"
                    || npc.name == "Krobus"
                    || npc.name == "Dwarf")
                {
                    ItemCode = TempRefs.MilkMagic;
                }
            }

            string SItemCode = $"[{ItemCode}]";
            AddItem = new Object(ItemCode, 1, quality: Quality);
            CurrentFarmer = who;

            // If no male milking, don't give item.
            if ((npc.gender == 0 & !Config.MilkMale) || !Config.CollectItems)
            {
                SItemCode = "";
                AddItem = null;
            }
            #endregion

            ModFunctions.LogVerbose($"Trying to milk {npc.name}. Will give item {ItemCode}: {_itemEditor.Data[ItemCode]}", LogLevel.Trace);

            #region Draw Dialogue
            npc.facePlayer(who);
            if (npc.Dialogue.TryGetValue(action, out string dialogues)) //Does npc have milking dialogue?
            {
                chosenString = GetRandomString(dialogues.Split(new string[] { "#split#" }, System.StringSplitOptions.None));
                Game1.drawDialogue(npc, $"{chosenString}");// {SItemCode}");
                success = true;
            }
            else
            {
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
                        if (npc.gender == 1)
                            Game1.drawDialogue(npc, $"I've never been asked that by anyone else. Although, that DOES sound kinda hot.#$b#You spend the next few minutes slowly kneeding their breasts, collecting the milk in a jar you brought with you. {SItemCode}");
                        else
                            Game1.drawDialogue(npc, $"You want my 'milk'? Erm, You ARE very attractive...#$b#*You quickly unzip their pants and pull out their cock. After a couple of quick licks to get them hard, you start sucking on them*#$b#I think I'm getting close! Here it comes! {SItemCode}");
                        success = true;
                        break;

                    case "fellatio":
                        Game1.drawDialogue(npc, $"You want to go down on me? I don't think I've ever had a guy offer to do that without an ulterior motive before.$h" +
                            $"#$b#%*{npc.name} quickly strips out of her lower garments, and opens her legs wide for you. You're greeted with a heady smell, and notice that her lips are starting to swell*" +
                            $"#$b#%*You lean in and start licking between {npc.name}'s lips, tasting her sweet nectar as she buries her hands in your hair*" +
                            $"#$b#%*As her moans get louder you use your tongue to flick her clit, and she clenches her legs tightly around your head*" +
                            $"#$b#%*You gently suck on the nub, and then plunge your tongue as deeply into her as you can, shaking your tongue from side to side to stimulate {npc.name} even more*" +
                            $"#$b#@, I'm cumming!");
                        success = true;
                        break;

                    case "BJ":
                        Game1.drawDialogue(npc, $"You want my 'milk'? Erm, You ARE very attractive...#$b#*You quickly unzip their pants and pull out their cock. After a couple of quick licks to get them hard, you start sucking on them*#$b#I think I'm getting close! Here it comes! {SItemCode}");
                        success = true;
                        break;

                    case "sex":
                        //TODO write four version of this for each gender configuration.

                        if (npc.gender == 0 && who.IsMale) // Male player, male NPC.
                            chosenString = $"two dudes going at it";
                        else if (npc.gender == 1 && who.IsMale) // Male player, female NPC.
                            chosenString = $"{who.name} buries their cock deep inside {npc.name}'s pussy";
                        else if (npc.gender == 0 && !who.IsMale) // Female player, male NPC.
                            chosenString = $"{who.name} climbs on top of {npc.name}'s erect cock and plunges it deep inside them until they cum";
                        else // neither is male
                            if (HasStrapon(who))
                            chosenString = $"You put on your strapon and fuck {npc.name} silly.";
                        else
                            chosenString = $"You and {npc.name} lick, suck and finger each other into oblivion";

                        Game1.drawDialogue(npc, chosenString);

                        success = true;
                        break;

                    default:
                        Game1.drawDialogue(npc, $"I don't have any dialogue written for that. Sorry.");
                        Monitor.Log($"{action} for {npc.name} wasn't found");
                        break;
                }
            }
            #endregion

            ModFunctions.LogVerbose($"ItemCode is [{ItemCode}]");
            if (success)
            {
                switch (action)
                {
                    case "milk_fast":
                        who.changeFriendship(30, npc);
                        TempRefs.milkedtoday.Add(npc);
                        break;

                    case "milk_start":
                        who.changeFriendship(30, npc);
                        TempRefs.milkedtoday.Add(npc);
                        break;

                    case "sex":
                        who.changeFriendship(45, npc);
                        TempRefs.SexToday.Add(npc);
                        break;

                    case "BJ":
                        who.changeFriendship(30, npc);
                        TempRefs.SexToday.Add(npc);
                        break;
                }

                if (AddItem != null
                    && AddItem.Category == TempRefs.SpecialType
                    && !who.mailReceived.Contains("MagicalItem")
                    && !who.mailForTomorrow.Contains("MagicalItem"))
                {
                    //who.mailForTomorrow.Add("MagicalItem");
                }
            }
            //_ = npc.checkAction(who, Game1.currentLocation);
        }

        private string GetRandomString(string[] dialogues)
        {
            int i = dialogues.Length;
            if (i < 1)
                return "";

            System.Random r = new System.Random();
            return dialogues[r.Next(i)];
        }

        private bool HasStrapon(Farmer who)
        {
            return true;
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

        private int[] FarmerPos(Farmer who)
        {
            return new int[2]
            {
                    who.getTileX(),
                    who.getTileY()
            };
        }
    }



}
