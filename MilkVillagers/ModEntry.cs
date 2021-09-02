using System.Collections.Generic;
using System.Linq;
using GenericModConfigMenu;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;

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

        // Asset Editors.
        private RecipeEditor _recipeEditor;
        private DialogueEditor _dialogueEditor;
        private ItemEditor _itemEditor;
        private QuestEditor _questEditor;
        private EventEditor _eventEditor;

        private ModConfig Config;
        public int CurrentQuest = 0; //currently loaded quest id.

        #endregion


        public override void Entry(IModHelper helper)
        {
            Config = helper.ReadConfig<ModConfig>();
            _recipeEditor = new RecipeEditor();
            _dialogueEditor = new DialogueEditor { ExtraContent = Config.ExtraDialogue };
            _itemEditor = new ItemEditor();
            _questEditor = new QuestEditor();
            _eventEditor = new EventEditor();

            if (helper == null)
            {
                ModFunctions.LogVerbose("helper is null.");
            }
            TempRefs.Helper = helper;
            TempRefs.Monitor = Monitor;

            helper.Events.Input.ButtonPressed += OnButtonPressed;
            helper.Events.GameLoop.DayStarted += GameLoop_DayStarted;
            helper.Events.GameLoop.GameLaunched += OnGameLaunched;
            helper.Events.GameLoop.SaveLoaded += GameLoop_SaveLoaded;
            helper.Events.GameLoop.TimeChanged += GameLoop_TimeChanged;
            helper.Events.GameLoop.DayEnding += GameLoop_DayEnding;
            helper.Events.GameLoop.OneSecondUpdateTicked += GameLoop_OneSecondUpdateTicked;

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
            //Helper.Content.AssetEditors.Add(_eventEditor);
            Helper.Content.AssetEditors.Add(new MyModMail());

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

            if (Config.Quests)
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

        #endregion

        #region Quest methods
        private void QuestChecks(Farmer who)
        {
            if (CurrentQuest == 0)
            {
                if (HasQuest(who, TempRefs.QuestID1))

                    CurrentQuest = TempRefs.QuestID1;
                else if (HasQuest(who, TempRefs.QuestID2))
                    CurrentQuest = TempRefs.QuestID2;
                else if (HasQuest(who, TempRefs.QuestID3))
                    CurrentQuest = TempRefs.QuestID3;
                else if (HasQuest(who, TempRefs.QuestID4))
                    CurrentQuest = TempRefs.QuestID4;
                else if (HasQuest(who, TempRefs.QuestID5))
                    CurrentQuest = TempRefs.QuestID5;
                else if (HasQuest(who, TempRefs.QuestID6))
                    CurrentQuest = TempRefs.QuestID6;
                else if (HasQuest(who, TempRefs.QuestID7))
                    CurrentQuest = TempRefs.QuestID7;
                else if (HasQuest(who, TempRefs.QuestID8))
                    CurrentQuest = TempRefs.QuestID8;

                if (CurrentQuest != 0)
                {
                    ModFunctions.LogVerbose($"Watching quest ID {CurrentQuest}", LogLevel.Alert);

                    _ = RemoveQuest(who, TempRefs.QuestIDWait);
                }
            }
            if (CurrentQuest != 0 && !HasQuest(who, CurrentQuest))
            {
                ModFunctions.LogVerbose($"Quest ID {CurrentQuest} has finished");

                // load next mail item after a certain amount of time? maybe just next day.
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
                else
                {
                    // No mail item associated with the quest completion.
                }

                //if (!who.mailReceived.Contains("AbiEggplant")) { } // Quest 5?

                if (NextMail != "")
                {
                    ModFunctions.LogVerbose($"adding mail {NextMail} to mailbox", LogLevel.Alert);

                    who.mailForTomorrow.Add(NextMail);
                }

                CurrentQuest = 0;
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
                foreach (var d in _questEditor.data)
                {
                    ModFunctions.LogVerbose($"{d.Key}: {d.Value}");
                }
            }
        }
        #endregion

        private bool CheckAll()
        {
            bool result = true;

            if (!_recipeEditor.CheckAll())
                result = false;
            if (!_itemEditor.CheckAll())
                result = false;
            if (!_questEditor.CheckAll())
                result = false;
            if (!_eventEditor.CheckAll())
                result = false;

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


            foreach (KeyValuePair<int, string> keyValuePair in _itemEditor.Report())
            {
                string[] strArray = keyValuePair.Value.Split('/');
                int ID = 0;

                switch (strArray[0])
                {
                    #region girls
                    case "Abigail's Milk":
                        TempRefs.MilkAbig = keyValuePair.Key;
                        ID = keyValuePair.Key;
                        continue;

                    case "Caroline's Milk":
                        TempRefs.MilkCaro = keyValuePair.Key;
                        ID = keyValuePair.Key;
                        continue;

                    case "Emily's Milk":
                        TempRefs.MilkEmil = keyValuePair.Key;
                        ID = keyValuePair.Key;
                        continue;

                    case "Evelyn's Milk":
                        TempRefs.MilkEvel = keyValuePair.Key;
                        ID = keyValuePair.Key;
                        continue;

                    case "Haley's Milk":
                        TempRefs.MilkHale = keyValuePair.Key;
                        ID = keyValuePair.Key;
                        continue;

                    case "Jodi's Milk":
                        TempRefs.MilkJodi = keyValuePair.Key;
                        ID = keyValuePair.Key;
                        continue;

                    case "Leah's Milk":
                        TempRefs.MilkLeah = keyValuePair.Key;
                        ID = keyValuePair.Key;
                        continue;

                    case "Marnie's Milk":
                        TempRefs.MilkMarn = keyValuePair.Key;
                        ID = keyValuePair.Key;
                        continue;

                    case "Maru's Milk":
                        TempRefs.MilkMaru = keyValuePair.Key;
                        ID = keyValuePair.Key;
                        continue;

                    case "Pam's Milk":
                        TempRefs.MilkPam = keyValuePair.Key;
                        ID = keyValuePair.Key;
                        continue;

                    case "Penny's Milk":
                        TempRefs.MilkPenn = keyValuePair.Key;
                        ID = keyValuePair.Key;
                        continue;

                    case "Robin's Milk":
                        TempRefs.MilkRobi = keyValuePair.Key;
                        ID = keyValuePair.Key;
                        continue;

                    case "Sandy's Milk":
                        TempRefs.MilkSand = keyValuePair.Key;
                        ID = keyValuePair.Key;
                        continue;
                    #endregion

                    #region guys
                    case "Alex's Cum":
                        TempRefs.MilkAlex = keyValuePair.Key;
                        ID = keyValuePair.Key;
                        continue;

                    case "Clint's Cum":
                        TempRefs.MilkClint = keyValuePair.Key;
                        ID = keyValuePair.Key;
                        continue;

                    case "Demetrius's Cum":
                        TempRefs.MilkDemetrius = keyValuePair.Key;
                        ID = keyValuePair.Key;
                        continue;

                    case "Elliott's Cum":
                        TempRefs.MilkElliott = keyValuePair.Key;
                        ID = keyValuePair.Key;
                        continue;

                    case "George's Cum":
                        TempRefs.MilkGeorge = keyValuePair.Key;
                        ID = keyValuePair.Key;
                        continue;

                    case "Gil's Cum":
                        TempRefs.MilkGil = keyValuePair.Key;
                        ID = keyValuePair.Key;
                        continue;

                    case "Gunther's Cum":
                        TempRefs.MilkGunther = keyValuePair.Key;
                        ID = keyValuePair.Key;
                        continue;

                    case "Gus's Cum":
                        TempRefs.MilkGus = keyValuePair.Key;
                        ID = keyValuePair.Key;
                        continue;

                    case "Harvey's Cum":
                        TempRefs.MilkHarv = keyValuePair.Key;
                        ID = keyValuePair.Key;
                        continue;

                    case "Kent's Cum":
                        TempRefs.MilkKent = keyValuePair.Key;
                        ID = keyValuePair.Key;
                        continue;

                    case "Lewis's Cum":
                        TempRefs.MilkLewis = keyValuePair.Key;
                        ID = keyValuePair.Key;
                        continue;

                    case "Linus's Cum":
                        TempRefs.MilkLinus = keyValuePair.Key;
                        ID = keyValuePair.Key;
                        continue;

                    case "Marlon's Cum":
                        TempRefs.MilkMarlon = keyValuePair.Key;
                        ID = keyValuePair.Key;
                        continue;

                    case "Morris's Cum":
                        TempRefs.MilkMorris = keyValuePair.Key;
                        ID = keyValuePair.Key;
                        continue;

                    case "Qi's Cum":
                        TempRefs.MilkQi = keyValuePair.Key;
                        ID = keyValuePair.Key;
                        continue;

                    case "Pierre's Cum":
                        TempRefs.MilkPierre = keyValuePair.Key;
                        ID = keyValuePair.Key;
                        continue;

                    case "Sam's Cum":
                        TempRefs.MilkSam = keyValuePair.Key;
                        ID = keyValuePair.Key;
                        continue;

                    case "Sebastian's Cum":
                        TempRefs.MilkSeb = keyValuePair.Key;
                        ID = keyValuePair.Key;
                        continue;

                    case "Shane's Cum":
                        TempRefs.MilkShane = keyValuePair.Key;
                        ID = keyValuePair.Key;
                        continue;

                    case "Willy's Cum":
                        TempRefs.MilkWilly = keyValuePair.Key;
                        ID = keyValuePair.Key;
                        continue;

                    case "Wizard's Cum":
                        TempRefs.MilkWiz = keyValuePair.Key;
                        ID = keyValuePair.Key;
                        continue;
                    #endregion

                    #region Extra Items
                    case "Woman's Milk":
                        TempRefs.MilkGeneric = keyValuePair.Key;
                        ID = keyValuePair.Key;
                        continue;

                    case "Special Milk":
                        TempRefs.MilkSpecial = keyValuePair.Key;
                        ID = keyValuePair.Key;
                        continue;

                    case "Protein Shake":
                        TempRefs.ProteinShake = keyValuePair.Key;
                        ID = keyValuePair.Key;
                        continue;

                    case "Special Milkshake":
                        TempRefs.MilkShake = keyValuePair.Key;
                        ID = keyValuePair.Key;
                        continue;

                    case "Super Juice":
                        TempRefs.SuperJuice = keyValuePair.Key;
                        ID = keyValuePair.Key;
                        continue;

                    #endregion

                    #region Other mods
                    case "Sophia's Milk":
                        TempRefs.MilkSophia = keyValuePair.Key;
                        ID = keyValuePair.Key;
                        continue;

                    case "Andy's Cum":
                        TempRefs.MilkAndy = keyValuePair.Key;
                        ID = keyValuePair.Key;
                        break;

                    case "Claire's Milk":
                        TempRefs.MilkClaire = keyValuePair.Key;
                        ID = keyValuePair.Key;
                        break;

                    case "Martin's Cum":
                        TempRefs.MilkMartin = keyValuePair.Key;
                        ID = keyValuePair.Key;
                        break;

                    case "Susan's Milk":
                        TempRefs.MilkSusan = keyValuePair.Key;
                        ID = keyValuePair.Key;
                        break;

                    case "Victor's Cum":
                        TempRefs.MilkVictor = keyValuePair.Key;
                        ID = keyValuePair.Key;
                        break;

                    case "Olivia's Milk":
                        TempRefs.MilkOlivia = keyValuePair.Key;
                        ID = keyValuePair.Key;
                        break;
                    #endregion

                    default:
                        if (strArray[0].ToLower().Contains("milk") || strArray[0].ToLower().Contains("cum"))
                        {
                            num1++;
                            ModFunctions.LogVerbose($"{strArray[0]} wasn't added.", LogLevel.Info);
                        }

                        continue;
                }

                if (ID != 0)
                {
                    ModFunctions.LogVerbose($"{strArray[0]} added. {ID}", Defcon);
                    ++num2;
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
                ModFunctions.LogVerbose($"skipped button press: {e.Button}, running:{running}");
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
                //Game1.warpFarmer("Hospital", 7, 10, false);
                Monitor.Log($"Current item: {who.CurrentItem.getCategoryName()}/{who.CurrentItem.category}", LogLevel.Alert);
                Monitor.Log($"Farmer is at {who.getTileX()}, {who.getTileY()}");
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
                else if (Config.Debug && who.hasItemInInventory(TempRefs.MilkQi, 1))
                {
                    List<Response> options = new List<Response>()
                    {
                        new Response ("time_freeze", "Yes"),
                        new Response("abort", "No")

                    };

                    Game1.currentLocation.createQuestionDialogue($"Do you want to try and freeze time?", options.ToArray(), new GameLocation.afterQuestionBehavior(DialoguesSet));
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
            else
                ActionOnNPC(currentTarget, who, action);

        }

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

            switch (action)
            {
                case "milk_start":
                    heartMin = 8;
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
                Game1.drawDialogue(npc, $"That's flattering, but I just don't like you enough for that. ({HeartCurrent}/{heartMin}");
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

            // Don't remove this - It's a good way of speeding up for people.
            if (Config.StackMilk)
            {
                ItemCode = npc.gender == 0 ? TempRefs.MilkSpecial : TempRefs.MilkGeneric;
            }
            else
            {
                switch (npc.Name) //Give items to player
                {
                    // Milk
                    case "Abigail": ItemCode = TempRefs.MilkAbig; break;
                    case "Caroline": ItemCode = TempRefs.MilkCaro; break;
                    case "Emily": ItemCode = TempRefs.MilkEmil; break;
                    case "Evelyn": ItemCode = TempRefs.MilkEvel; break;
                    case "Haley": ItemCode = TempRefs.MilkHale; break;
                    case "Jodi": ItemCode = TempRefs.MilkJodi; break;
                    case "Leah": ItemCode = TempRefs.MilkLeah; break;
                    case "Marnie": ItemCode = TempRefs.MilkMarn; break;
                    case "Maru": ItemCode = TempRefs.MilkMaru; break;
                    case "Pam": ItemCode = TempRefs.MilkPam; break;
                    case "Penny": ItemCode = TempRefs.MilkPenn; break;
                    case "Sandy": ItemCode = TempRefs.MilkSand; break;
                    case "Dwarf": ItemCode = TempRefs.MilkDwarf; break;

                    case "Sophia": ItemCode = TempRefs.MilkSophia; break;
                    case "Olivia": ItemCode = TempRefs.MilkOlivia; break;
                    case "Susan": ItemCode = TempRefs.MilkSusan; break;
                    case "Claire": ItemCode = TempRefs.MilkClaire; break;

                    // Cum
                    case "Alex": ItemCode = TempRefs.MilkAlex; break;
                    case "Clint": ItemCode = TempRefs.MilkClint; break;
                    case "Demetrius": ItemCode = TempRefs.MilkDemetrius; break;
                    case "Elliott": ItemCode = TempRefs.MilkElliott; break;
                    case "George": ItemCode = TempRefs.MilkGeorge; break;
                    case "Gus": ItemCode = TempRefs.MilkGus; break;
                    case "Harvey": ItemCode = TempRefs.MilkHarv; break;
                    case "Kent": ItemCode = TempRefs.MilkKent; break;
                    case "Lewis": ItemCode = TempRefs.MilkLewis; break;
                    case "Linus": ItemCode = TempRefs.MilkLinus; break;
                    case "Pierre": ItemCode = TempRefs.MilkPierre; break;
                    case "Robin": ItemCode = TempRefs.MilkRobi; break;
                    case "Sam": ItemCode = TempRefs.MilkSam; break;
                    case "Sebastian": ItemCode = TempRefs.MilkSeb; break;
                    case "Shane": ItemCode = TempRefs.MilkShane; break;
                    case "Willy": ItemCode = TempRefs.MilkWilly; break;

                    //Magical
                    case "Mister Qi": ItemCode = TempRefs.MilkQi; break;
                    case "Wizard": ItemCode = TempRefs.MilkWiz; break;
                    //case "Marlon": ItemCode = TempRefs.MilkWMarlon; break; //WTF? Who is WMarlon?
                    case "Krobus": ItemCode = TempRefs.MilkKrobus; break;

                    case "Andy": ItemCode = TempRefs.MilkAndy; break;
                    case "Victor": ItemCode = TempRefs.MilkVictor; break;
                    case "Martin": ItemCode = TempRefs.MilkMartin; break;

                    default: //NPC's I don't know.
                        ItemCode = npc.gender == 0 ? TempRefs.MilkSpecial : TempRefs.MilkGeneric;
                        ModFunctions.LogVerbose($"Couldn't find {npc.name} in the list of items", LogLevel.Debug);
                        break;
                }
            }

            string SItemCode = $"[{ItemCode}]";

            // If no male milking, don't give item.
            if ((npc.gender == 0 & !Config.MilkMale) || !Config.CollectItems)
            {
                SItemCode = "";
            }
            #endregion

            ModFunctions.LogVerbose($"Trying to milk {npc.name}. Will give item {ItemCode}: {_itemEditor.Data[ItemCode]}", LogLevel.Trace);


            npc.facePlayer(who);
            if (npc.Dialogue.TryGetValue(action, out string dialogues)) //Does npc have milking dialogue?
            {
                chosenString = GetRandomString(dialogues.Split(new string[] { "#split#" }, System.StringSplitOptions.None));
                Game1.drawDialogue(npc, $"{chosenString} {SItemCode}");
                success = true;
            }
            else
            {
                switch (action)
                {
                    case "milk_fast":
                        Game1.drawDialogue(npc, SItemCode);
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
