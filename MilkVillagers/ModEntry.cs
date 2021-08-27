using System;
using System.Collections.Generic;
using GenericModConfigMenu;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;

namespace MilkVillagers
{
    public class ModEntry : Mod
    {
        private int[] target;
        private bool running;
        private bool runOnce;

        // Asset Editors.
        private RecipeEditor _recipeEditor;
        private DialogueEditor _dialogueEditor;
        private ItemEditor _itemEditor;
        private QuestEditor _questEditor;
        private EventEditor _eventEditor;

        private ModConfig Config;
        public int CurrentQuest = 0; //currently loaded quest id.

        private int[] FarmerPos
        {
            get
            {
                return new int[2]
                {
                    Game1.player.getTileX(),
                    Game1.player.getTileY()
                };
            }
        }

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

            //helper.Events.Player.Warped += (new EventHandler<WarpedEventArgs>(Player_Warped));
        }

        #region Game OnEvent Triggers
        private void GameLoop_DayEnding(object sender, DayEndingEventArgs e)
        {
            QuestChecks();
        }

        private void GameLoop_TimeChanged(object sender, TimeChangedEventArgs e)
        {
            if (!Game1.player.mailReceived.Contains("AbiEggplant")) // need to add in last quest as well so that it stops checking after all quests are done.
                return;

            QuestChecks();
        }

        private void GameLoop_SaveLoaded(object sender, SaveLoadedEventArgs e)
        {
            ModFunctions.LogVerbose("Loaded savegame.");

            if (runOnce)
                return;

            GetItemCodes();

            // Recipes.
            CorrectRecipes();
            AddAllRecipes();

            runOnce = true;
        }

        private void OnGameLaunched(object sender, GameLaunchedEventArgs e)
        {
            #region Generic Mod Config
            // get Generic Mod Config Menu API (if it's installed)
            var api = this.Helper.ModRegistry.GetApi<IGenericModConfigMenuApi>("spacechase0.GenericModConfigMenu");
            if (api is null)
                return;

            // register mod configuration
            api.RegisterModConfig(
                mod: this.ModManifest,
                revertToDefault: () => this.Config = new ModConfig(),
                saveToFile: () => this.Helper.WriteConfig(this.Config)
            );

            // let players configure your mod in-game (instead of just from the title screen)
            api.SetDefaultIngameOptinValue(this.ModManifest, true);

            // add some config options
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
                optionName: "Simple milk/cum",
                optionDesc: "Simplify the milk and cum items?",
                optionGet: () => this.Config.StackMilk,
                optionSet: value => this.Config.StackMilk = value
            );

            api.RegisterSimpleOption(
                mod: this.ModManifest,
                optionName: "ExtraDialogue",
                optionDesc: "Enable Abigail's dialogue changes?",
                optionGet: () => this.Config.ExtraDialogue,
                optionSet: value => this.Config.ExtraDialogue = value
            );

            api.RegisterSimpleOption(
                mod: this.ModManifest,
                optionName: "3rd Party Dialogue",
                optionDesc: "Enable 3rd party dialogue?",
                optionGet: () => this.Config.ThirdParty,
                optionSet: value => this.Config.ThirdParty = value
            );

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

            api.RegisterSimpleOption(
                mod: this.ModManifest,
                optionName: "Quests",
                optionDesc: "Enable quest content",
                optionGet: () => this.Config.Quests,
                optionSet: value => this.Config.Quests = value
            );
            #endregion

            Helper.Content.AssetEditors.Add(_itemEditor);
            Helper.Content.AssetEditors.Add(_dialogueEditor);
            Helper.Content.AssetEditors.Add(_questEditor);
            Helper.Content.AssetEditors.Add(_recipeEditor);
            Helper.Content.AssetEditors.Add(_eventEditor);
            Helper.Content.AssetEditors.Add(new MyModMail());

            TempRefs.thirdParty = Config.ThirdParty;
            TempRefs.Verbose = Config.Verbose;
        }

        private void GameLoop_DayStarted(object sender, DayStartedEventArgs e)
        {
            if (TempRefs.milkedtoday == null)
                ModFunctions.LogVerbose("TempRefs not set");
            else
            {
                ModFunctions.LogVerbose($"TempRefs is set. Cleared {TempRefs.milkedtoday.Count}");
                TempRefs.milkedtoday.Clear();
            }

            //Add recipe to stock.
            //Dictionary<ISalable, int[]> stock = Utility.getSaloonStock();
            //foreach (KeyValuePair<ISalable, int[]> kp in stock)
            //{
            //    LogVerbose($"{kp.Key.DisplayName}: price ${kp.Value[0]}, Quantity: {kp.Value[1]}", LogLevel.Trace);
            //}

            //TODO set up preconditions for these.
            if (Config.Debug)
            {

                if (!Game1.player.mailReceived.Contains("MilkingAbi"))
                    Game1.player.mailbox.Add("MilkingAbi");

                if (Config.Verbose)
                    OutputQuests();

            }

            // Tutorial
            if (!Game1.player.mailReceived.Contains("MilkButton1"))
                Game1.player.mailbox.Add("MilkButton1");
            if (!Game1.player.mailReceived.Contains("MilkButton2"))
                Game1.player.mailbox.Add("MilkButton2");

            if (Config.Quests)
            {
                if (Game1.player.getFriendshipHeartLevelForNPC("Abigail") > 7 && !Game1.player.mailReceived.Contains("AbiEggplant"))
                {
                    Game1.player.mailForTomorrow.Add("AbiEggplant");

                    //_questEditor.data[TempRefs.QuestID1] = _questEditor.data[TempRefs.QuestID1].Replace($"{TempRefs.QuestID2}", $"{TempRefs.QuestIDWait}");
                    _questEditor.data[TempRefs.QuestIDWait] = _questEditor.data[TempRefs.QuestIDWait].Replace("594800", $"{TempRefs.QuestID2}");
                }

                // Send new mail checks
                if (Game1.player.mailbox.Contains("AbiEggplantT") && !Game1.player.mailbox.Contains("AbiCarrots"))
                    Game1.player.mailForTomorrow.Add("AbiCarrots");
                if (Game1.player.mailbox.Contains("AbiCarrotsT") && !Game1.player.mailbox.Contains("AbiRadishes"))
                    Game1.player.mailForTomorrow.Add("AbiRadishes");
                if (Game1.player.mailbox.Contains("AbiRadishesT") && !Game1.player.mailbox.Contains("AbiSurprise"))
                    Game1.player.mailForTomorrow.Add("AbiSurpriseT");
                if (Game1.player.mailbox.Contains("AbiSurpriseT") && !Game1.player.mailbox.Contains("MaruSample"))
                    Game1.player.mailForTomorrow.Add("MaruSample");
                if (Game1.player.mailbox.Contains("MaruSampleT") && !Game1.player.mailbox.Contains("GeorgeMilk"))
                    Game1.player.mailForTomorrow.Add("GeorgeMilk");
            }
        }
        #endregion

        #region Quest methods
        private void QuestChecks()
        {
            if (CurrentQuest == 0)
            {
                if (HasQuest(TempRefs.QuestID1))

                    CurrentQuest = TempRefs.QuestID1;
                else if (HasQuest(TempRefs.QuestID2))
                    CurrentQuest = TempRefs.QuestID2;
                else if (HasQuest(TempRefs.QuestID3))
                    CurrentQuest = TempRefs.QuestID3;
                else if (HasQuest(TempRefs.QuestID4))
                    CurrentQuest = TempRefs.QuestID4;
                else if (HasQuest(TempRefs.QuestID5))
                    CurrentQuest = TempRefs.QuestID5;
                else if (HasQuest(TempRefs.QuestID6))
                    CurrentQuest = TempRefs.QuestID6;
                else if (HasQuest(TempRefs.QuestID7))
                    CurrentQuest = TempRefs.QuestID7;
                else if (HasQuest(TempRefs.QuestID8))
                    CurrentQuest = TempRefs.QuestID8;

                if (CurrentQuest != 0)
                {
                    ModFunctions.LogVerbose($"Watching quest ID {CurrentQuest}", LogLevel.Alert);

                    _ = RemoveQuest(TempRefs.QuestIDWait);
                }
            }
            if (CurrentQuest != 0 && !HasQuest(CurrentQuest))
            {
                ModFunctions.LogVerbose($"Quest ID {CurrentQuest} has finished");

                // load next mail item after a certain amount of time? maybe just next day.
                string NextMail = "";
                if (!Game1.player.mailReceived.Contains("AbiEggplantT")) // Quest 1 complete
                    NextMail = "AbiEggplantT";
                else if (!Game1.player.mailReceived.Contains("AbiCarrotsT"))  // Quest 2 complete
                    NextMail = "AbiCarrotsT";
                else if (!Game1.player.mailReceived.Contains("AbiRadishesT"))  // Quest 3 complete             
                    NextMail = "AbiRadishesT";
                else if (!Game1.player.mailReceived.Contains("AbiSurpriseT"))  // Quest 4 complete            
                    NextMail = "AbiSurpriseT";
                else if (!Game1.player.mailReceived.Contains("MaruSampleT"))  // Quest 5 complete
                    NextMail = "MaruSampleT";
                else if (!Game1.player.mailReceived.Contains("GeorgeMilkT"))  // Quest 6 complete
                    NextMail = "GeorgeMilkT";
                else
                {
                    // No mail item associated with the quest completion.
                }

                //if (!Game1.player.mailReceived.Contains("AbiEggplant")) { } // Quest 5?

                if (NextMail != "")
                {
                    ModFunctions.LogVerbose($"adding mail {NextMail} to mailbox", LogLevel.Alert);

                    Game1.player.mailForTomorrow.Add(NextMail);
                }

                CurrentQuest = 0;
            }

        }

        private bool RemoveQuest(int id)
        {
            foreach (var q in Game1.player.questLog)
            {
                if (q.id == id)
                {
                    Game1.player.questLog.Remove(q);
                    return true;
                }
            }
            return false;
        }

        private bool HasQuest(int id)
        {
            foreach (var q in Game1.player.questLog)
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


        private static void AddAllRecipes()
        {
            //TODO move this to be a quest reward or something.
            ModFunctions.LogVerbose($"Adding in both recipes", LogLevel.Alert);

            if (!Game1.player.cookingRecipes.ContainsKey("Milkshake"))
                Game1.player.cookingRecipes.Add("Milkshake", 0);

            if (!Game1.player.cookingRecipes.ContainsKey("'Protein' Shake"))
                Game1.player.cookingRecipes.Add("'Protein' Shake", 0);

            if (!Game1.player.craftingRecipes.ContainsKey("Generic Milk"))
                Game1.player.craftingRecipes.Add("Generic Milk", 0);

            if (!Game1.player.craftingRecipes.ContainsKey("Generic Cum"))
                Game1.player.craftingRecipes.Add("Generic Cum", 0);

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

                    case "Sandra's Milk":
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

                    #endregion

                    #region Other mods
                    case "Sophia's Milk":
                        TempRefs.MilkSophia = keyValuePair.Key;
                        ID = keyValuePair.Key;
                        continue;
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
                    ModFunctions.LogVerbose($"{strArray[0]} added. {TempRefs.MilkSophia}", Defcon);
                    ++num2;
                }
            }

            TempRefs.loaded = true;
            ModFunctions.LogVerbose($"Loaded {num2}/{num1 + num2} items", Defcon);

            Game1.player.Items[0].getCategoryName();

            #region fix item strings
            //TODO might be able to delete this section
            IDictionary<int, string> _objectData = _itemEditor._objectData;

            //milk items
            _objectData[TempRefs.MilkAbig] = $"Abigail's Milk/300/15/Basic {TempRefs.MilkType}/Abigail's Milk/A jug of Abigail's milk./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
            _objectData[TempRefs.MilkEmil] = $"Emily's Milk/300/15/Basic {TempRefs.MilkType}/Emily's Milk/A jug of Emily's milk./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
            _objectData[TempRefs.MilkHale] = $"Haley's Milk/300/15/Basic {TempRefs.MilkType}/Haley's Milk/A jug of Haley's milk./drink/0 0 0 0 0 0 0 0 0 0 0/0";
            _objectData[TempRefs.MilkLeah] = $"Leah's Milk/300/15/Basic {TempRefs.MilkType}/Leah's Milk/A jug of Leah's milk./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
            _objectData[TempRefs.MilkMaru] = $"Maru's Milk/300/15/Basic {TempRefs.MilkType}/Maru's Milk/A jug of Maru's milk./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
            _objectData[TempRefs.MilkPenn] = $"Penny's Milk/300/15/Basic {TempRefs.MilkType}/Penny's Milk/A jug of Penny's milk./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
            _objectData[TempRefs.MilkCaro] = $"Caroline's Milk/300/15/Basic {TempRefs.MilkType}/Caroline's Milk/A jug of Caroline's milk./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
            _objectData[TempRefs.MilkJodi] = $"Jodi's Milk/300/15/Basic {TempRefs.MilkType}/Jodi's Milk/A jug of Jodi's milk./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
            _objectData[TempRefs.MilkMarn] = $"Marnie's Milk/140/15/Basic {TempRefs.MilkType}/Marnie's Milk/A jug of Marnie's milk./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
            _objectData[TempRefs.MilkRobi] = $"Robin's Milk/300/15/Basic {TempRefs.MilkType}/Robin's Milk/A jug of Robin's milk./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
            _objectData[TempRefs.MilkPam] = $"Pam's Milk/90/15/Basic {TempRefs.MilkType}/Pam's Milk/A jug of Pam's milk./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
            _objectData[TempRefs.MilkSand] = $"Sandy's Milk/350/15/Basic {TempRefs.MilkType}/Sandy's Milk/A jug of Sandy's milk./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
            _objectData[TempRefs.MilkEvel] = $"Evelyn's Milk/50/15/Basic {TempRefs.MilkType}/Evelyn's Milk/A jug of Evelyn's milk./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
            _objectData[TempRefs.MilkDwarf] = $"Dwarf's Milk/300/15/Basic {TempRefs.CumType}/Dwarf's Milk/A jug of Dwarf's milk ./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
            
            //cum items
            _objectData[TempRefs.MilkAlex] = $"Alex's Cum/300/15/Basic {TempRefs.CumType}/Alex's Cum /A bottle of Alex's Cum ./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
            _objectData[TempRefs.MilkClint] = $"Clint's Cum/300/15/Basic {TempRefs.CumType}/Clint's Cum/A bottle of Clint's Cum./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
            _objectData[TempRefs.MilkDemetrius] = $"Demetrius's Cum/300/15/Basic {TempRefs.CumType}/Demetrius's Cum/A bottle of Demetrius's Cum./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
            _objectData[TempRefs.MilkElliott] = $"Elliott's Cum/300/15/Basic {TempRefs.CumType}/Elliott's Cum/A bottle of Elliott's Cum./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
            _objectData[TempRefs.MilkGeorge] = $"George's Cum/300/15/Basic {TempRefs.CumType}/George's Cum /A bottle of George's Cum ./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
            _objectData[TempRefs.MilkGil] = $"Gil's Cum/300/15/Basic {TempRefs.CumType}/Gil's Cum/A bottle of Gil's Cum./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
            _objectData[TempRefs.MilkGunther] = $"Gunther's Cum/300/15/Basic {TempRefs.CumType}/Gunther's Cum/A bottle of Gunther's Cum./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
            _objectData[TempRefs.MilkGus] = $"Gus's Cum/300/15/Basic {TempRefs.CumType}/Gus's Cum/A bottle of Gus's Cum./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
            _objectData[TempRefs.MilkHarv] = $"Harvey's Cum/300/15/Basic {TempRefs.CumType}/Harvey's Cum /A bottle of Harvey's Cum ./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
            _objectData[TempRefs.MilkKent] = $"Kent's Cum/300/15/Basic {TempRefs.CumType}/Kent's Cum /A bottle of Kent's Cum ./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
            _objectData[TempRefs.MilkLewis] = $"Lewis's Cum/300/15/Basic {TempRefs.CumType}/Lewis's Cum/A bottle of Lewis's Cum./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
            _objectData[TempRefs.MilkLinus] = $"Linus's Cum/300/15/Basic {TempRefs.CumType}/Linus's Cum/A bottle of Linus's Cum./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
            _objectData[TempRefs.MilkMarlon] = $"Marlon's Cum/300/15/Basic {TempRefs.CumType}/Marlon's Cum /A bottle of Marlon's Cum ./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
            _objectData[TempRefs.MilkMorris] = $"Morris's Cum/300/15/Basic {TempRefs.CumType}/Morris's Cum /A bottle of Morris's Cum ./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
            _objectData[TempRefs.MilkQi] = $"Mr. Qi's Cum/300/15/Basic {TempRefs.CumType}/Mr. Qi's Cum /A bottle of Mr. Qi's Cum ./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
            _objectData[TempRefs.MilkPierre] = $"Pierre's Cum/300/15/Basic {TempRefs.CumType}/Pierre's Cum /A bottle of Pierre's Cum ./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
            _objectData[TempRefs.MilkSam] = $"Sam's Cum/300/15/Basic {TempRefs.CumType}/Sam's Cum/A bottle of Sam's Cum./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
            _objectData[TempRefs.MilkSeb] = $"Sebastian's Cum/300/15/Basic {TempRefs.CumType}/Sebastian's Cum/A bottle of Sebastian's Cum./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
            _objectData[TempRefs.MilkShane] = $"Shane's Cum/300/15/Basic {TempRefs.CumType}/Shane's Cum/A bottle of Shane's Cum./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
            _objectData[TempRefs.MilkWilly] = $"Willy's Cum/300/15/Basic {TempRefs.CumType}/Willy's Cum/A bottle of Willy's Cum./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
            _objectData[TempRefs.MilkWiz] = $"Wizard's Cum/300/15/Basic {TempRefs.CumType}/Wizard's Cum /A bottle of Wizard's Cum ./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
            _objectData[TempRefs.MilkMarlon] = $"Marlon's Cum/300/15/Basic {TempRefs.CumType}/Marlon's Cum /A bottle of Marlon's Cum ./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
            _objectData[TempRefs.MilkKrobus] = $"Krobus's Cum/300/15/Basic {TempRefs.CumType}/Krobus's Cum /A bottle of Krobus's Cum ./Drink/0 0 0 0 0 0 0 0 0 0 0/0";


            //recipes
            _objectData[TempRefs.ProteinShake] = $"Protein shake/50/15/Cooking -7/'Protein' shake/Shake made with extra protein/drink/0 0 0 0 0 0 0 0 0 0 0/0";
            _objectData[TempRefs.MilkShake] = $"Milkshake/50/15/Cooking -7/'Special' Milkshake/Extra milky milkshake./drink/0 0 0 0 0 0 0 0 0 0 0/0";
            _objectData[TempRefs.MilkSpecial] = $"Special milk/50/15/Crafting {TempRefs.CumType}/'Special' Milk/A bottle of 'special' milk./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
            _objectData[TempRefs.MilkGeneric] = $"Woman's Milk/50/15/Crafting {TempRefs.MilkType}/Woman's Milk/A jug of woman's milk./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
            #endregion


        }

        private void CorrectRecipes()
        {
            _recipeEditor.CookingData["'Protein' Shake"] = $"{TempRefs.CumType} 1/10 10/{TempRefs.ProteinShake}/default/'Protein' shake";
            _recipeEditor.CookingData["Milkshake"] = $"{TempRefs.MilkType} 1/10 10/{TempRefs.MilkShake}/default/Milkshake";

            //_recipeEditor.data["Woman's Milk"] = $"{TempRefs.MilkType} 1/10 10/{TempRefs.MilkGeneric}/null/Woman's Milk";
            //_recipeEditor.data["Generic Cum"] = $"{TempRefs.CumType} 1/10 10/{TempRefs.MilkSpecial}/null/Special Milk";

        }

        private void OnButtonPressed(object sender, ButtonPressedEventArgs e)
        {
            if (running)
            {
                return;
            }


            running = false;

            if (!Context.IsWorldReady)
                return;

            target = GetNewPos(Game1.player.FacingDirection, this.FarmerPos[0], this.FarmerPos[1]);
            SButton button = e.Button;

            //if (button == SButton.P)
            //{
            //    if (!this.Config.PailGiven)
            //    {
            //        //Game1.player.addItemToInventory((Item)new SpellMilk());
            //        //Game1.player.addItemToInventory((Item)new SpellTeleport());
            //        this.Config.PailGiven = true;
            //    }
            //    else
            //        Game1.warpFarmer("SeedShop", 4, 9, false);
            //}
            if (Config.Debug && button == SButton.P)
            {
                Game1.warpFarmer("SeedShop", 4, 9, false);
            }
            if (button == SButton.O)
            {
                //if (Game1.player.CurrentTool == null)
                //    return;

                //if (Game1.player.CurrentTool.GetType().ToString() == typeof(SpellMilk).ToString())
                //{

                NPC target = ModFunctions.FindTarget(this.target, this.FarmerPos);
                if (target != null)
                {
                    if (Config.Verbose)
                        Game1.addHUDMessage(new HUDMessage($"Trying to milk {target.name}"));

                    ActionOnNPC(target);
                    running = false;
                }
                else
                {
                    if (Config.Verbose)
                        Game1.addHUDMessage(new HUDMessage("Couldn't find anyone to milk."));

                }
                //}
                //else
                //    Game1.addHUDMessage(new HUDMessage("Wrong tool equipped."));
            }
        }

        private void ActionOnNPC(NPC npc)
        {
            // Removed option for requiring a milk pail
            //if (Config.NeedTool && Game1.player.CurrentTool.GetType().ToString() != typeof(StardewValley.Tools.MilkPail).ToString())
            //{
            //    Game1.addHUDMessage(new HUDMessage("You need to be holding a pail to milk people."));
            //    return;
            //}

            #region validity checks

            //Age and friendship
            // Child check
            if (npc.age == 2)
            {
                return;
            }
            if (Game1.player.getFriendshipHeartLevelForNPC(npc.name) < 8)
            {
                Game1.drawDialogue(npc, "Hey there @. Wotcha doing with that pail?#I don't like you enough for that!");
                Monitor.Log($"{npc.name} is {npc.age} and at heart level {Game1.player.getFriendshipHeartLevelForNPC(npc.name)}", LogLevel.Debug);
                return;
            }

            //gender check 0 is male, 1 is female
            if (npc.gender == 0 & !Config.MilkMale)
            {
                Game1.addHUDMessage(new HUDMessage("You have male character milking turned off."));
                ModFunctions.LogVerbose($"gender is {npc.Gender}", LogLevel.Warn);
                return;
            }
            if (npc.gender == 1 & !Config.MilkFemale)
            {

                Game1.addHUDMessage(new HUDMessage("You have female character milking turned off."));
                ModFunctions.LogVerbose($"gender is {npc.Gender}", LogLevel.Warn);
                return;
            }

            //milked today
            if (TempRefs.milkedtoday.Contains(npc))
            {
                if (Config.Verbose)
                    Game1.addHUDMessage(new HUDMessage($"{npc.name} has already been milked today."));

                return;
            }
            #endregion

            #region set item to give
            string ItemCode = $"[{TempRefs.MilkGeneric}]";
            if (Config.StackMilk)
            {
                ItemCode = npc.gender == 0 ? $"[{TempRefs.MilkSpecial}]" : $"[{TempRefs.MilkGeneric}]";
            }
            else
            {
                switch (npc.Name) //Give items to player
                {
                    case "Abigail": ItemCode = $"[{TempRefs.MilkAbig}]"; break;
                    case "Caroline": ItemCode = $"[{TempRefs.MilkCaro}]"; break;
                    case "Emily": ItemCode = $"[{TempRefs.MilkEmil}]"; break;
                    case "Evelyn": ItemCode = $"[{TempRefs.MilkEvel}]"; break;
                    case "Haley": ItemCode = $"[{TempRefs.MilkHale}]"; break;
                    case "Jodi": ItemCode = $"[{TempRefs.MilkJodi}]"; break;
                    case "Leah": ItemCode = $"[{TempRefs.MilkLeah}]"; break;
                    case "Marnie": ItemCode = $"[{TempRefs.MilkMarn}]"; break;
                    case "Maru": ItemCode = $"[{TempRefs.MilkMaru}]"; break;
                    case "Pam": ItemCode = $"[{TempRefs.MilkPam}]"; break;
                    case "Penny": ItemCode = $"[{TempRefs.MilkPenn}]"; break;
                    case "Sandy": ItemCode = $"[{TempRefs.MilkSand}]"; break;
                    case "Alex": ItemCode = $"[{TempRefs.MilkAlex}]"; break;
                    case "Clint": ItemCode = $"[{TempRefs.MilkClint}]"; break;
                    case "Demetrius": ItemCode = $"[{TempRefs.MilkDemetrius}]"; break;
                    case "Elliott": ItemCode = $"[{TempRefs.MilkElliott}]"; break;
                    case "George": ItemCode = $"[{TempRefs.MilkGeorge}]"; break;
                    case "Gus": ItemCode = $"[{TempRefs.MilkGus}]"; break;
                    case "Harvey": ItemCode = $"[{TempRefs.MilkHarv}]"; break;
                    case "Kent": ItemCode = $"[{TempRefs.MilkKent}]"; break;
                    case "Lewis": ItemCode = $"[{TempRefs.MilkLewis}]"; break;
                    case "Linus": ItemCode = $"[{TempRefs.MilkLinus}]"; break;
                    case "Pierre": ItemCode = $"[{TempRefs.MilkPierre}]"; break;
                    case "Robin": ItemCode = $"[{TempRefs.MilkRobi}]"; break;
                    case "Sam": ItemCode = $"[{TempRefs.MilkSam}]"; break;
                    case "Sebastian": ItemCode = $"[{TempRefs.MilkSeb}]"; break;
                    case "Shane": ItemCode = $"[{TempRefs.MilkShane}]"; break;
                    case "Willy": ItemCode = $"[{TempRefs.MilkWilly}]"; break;
                    case "Wizard": ItemCode = $"[{TempRefs.MilkWiz}]"; break;
                    case "Marlon": ItemCode = $"[{TempRefs.MilkWMarlon}]"; break;
                    case "Krobus": ItemCode = $"[{TempRefs.MilkKrobus}]"; break;
                    case "Dwarf": ItemCode = $"[{TempRefs.MilkDwarf}]"; break;

                    case "Sophia": ItemCode = $"[{TempRefs.MilkSophia}]"; break;
                    case "Olivia": ItemCode = $"[{TempRefs.MilkOlivia}]"; break;
                    case "Susan": ItemCode = $"[{TempRefs.MilkSusan}]"; break;
                    case "Claire": ItemCode = $"[{TempRefs.MilkClaire}]"; break;
                    case "Andy": ItemCode = $"[{TempRefs.MilkAndy}]"; break;
                    case "Victor": ItemCode = $"[{TempRefs.MilkVictor}]"; break;
                    case "Martin": ItemCode = $"[{TempRefs.MilkMartin}]"; break;

                    default: //NPC's I don't know.
                        ItemCode = npc.gender == 0 ? $"[{TempRefs.MilkSpecial}]" : $"[{TempRefs.MilkGeneric}]";
                        ModFunctions.LogVerbose($"Couldn't find {npc.name} in the list of items", LogLevel.Debug);
                        break;
                }
            }
            #endregion

            npc.facePlayer(Game1.player);
            if (npc.Dialogue.TryGetValue("milk_start", out string dialogues)) //Does npc have milking dialogue?
            {
                Game1.drawDialogue(npc, $"{dialogues} {ItemCode}");
            }
            else
            {
                if (npc.gender == 1)
                    Game1.drawDialogue(npc, $"I've never been asked that by anyone else. Although, that DOES sound kinda hot.#$b#You spend the next few minutes slowly kneeding their breasts, collecting the milk in a jar you brought with you. {ItemCode}");
                else
                    Game1.drawDialogue(npc, $"You want my 'milk'? Erm, You ARE very attractive...#$b#*You quickly unzip their pants and pull out their cock. After a couple of quick licks to get them hard, you start sucking on them*#$b#I think I'm getting close! Here it comes! {ItemCode}");
            }

            ModFunctions.LogVerbose($"ItemCode is {ItemCode}");
            Game1.player.changeFriendship(30, npc);
            TempRefs.milkedtoday.Add(npc);
            //_ = npc.checkAction(Game1.player, Game1.currentLocation);
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

    }



}
