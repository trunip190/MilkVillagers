using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using GenericModConfigMenu;
using Microsoft.Xna.Framework.Graphics;
using Netcode;
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
        private RecipeEditor _recipeEditor;
        private DialogueEditor _dialogueEditor;
        private ItemEditor _itemEditor;
        private QuestEditor _questEditor;
        private ModConfig Config;

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
            _dialogueEditor = new DialogueEditor();
            _itemEditor = new ItemEditor();
            _questEditor = new QuestEditor();

            if (helper == null)
            {
                Monitor.Log("helper is null.", 0);
            }
            TempRefs.Helper = helper;
            TempRefs.Monitor = Monitor;

            helper.Events.Input.ButtonPressed += new EventHandler<ButtonPressedEventArgs>(OnButtonPressed);
            helper.Events.GameLoop.DayStarted += new EventHandler<DayStartedEventArgs>(GameLoop_DayStarted);
            helper.Events.GameLoop.GameLaunched += new EventHandler<GameLaunchedEventArgs>(OnGameLaunched);
            helper.Events.GameLoop.SaveLoaded += new EventHandler<SaveLoadedEventArgs>(GameLoop_SaveLoaded);

            //helper.Events.Player.Warped += (new EventHandler<WarpedEventArgs>(Player_Warped));
        }

        //TODO can probably remove this
        //private void Player_Warped(object sender, WarpedEventArgs e)
        //{
        //    _ = Game1.player.passedOut ? 1 : 0;
        //}

        private void GameLoop_SaveLoaded(object sender, SaveLoadedEventArgs e)
        {
            Monitor.Log("Loaded savegame.", LogLevel.Trace);
            if (runOnce)
                return;

            GetItemCodes();
            CorrectRecipes();

            //Temporarily add in recipes on savegame load.
            //TODO move this to an event.
            //TODO stop this wiping recipes.
            if (false)
            {
                SerializableDictionary<string, int> newRecipe = new SerializableDictionary<string, int>
                {
                    { "'Protein' Shake", 1 },
                    { "Milkshake", 1 }
                };
                Game1.player.cookingRecipes.Add(newRecipe);
            }

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
                optionDesc: "Enable Abiail's dialogue changes?",
                optionGet: () => this.Config.ExtraDialogue,
                optionSet: value => this.Config.ExtraDialogue = value
            );
            #endregion

            Helper.Content.AssetEditors.Add(_itemEditor);
            Helper.Content.AssetEditors.Add(_dialogueEditor);
            Helper.Content.AssetEditors.Add(_questEditor);
            Helper.Content.AssetEditors.Add(_recipeEditor);
            Helper.Content.AssetEditors.Add(new MyModMail());
            TempRefs.QuestID1 = Config.QuestID1;
            TempRefs.QuestID2 = Config.QuestID2;
            TempRefs.QuestID3 = Config.QuestID3;
        }

        private void GameLoop_DayStarted(object sender, DayStartedEventArgs e)
        {
            if (TempRefs.milkedtoday == null && Config.Verbose)
                Monitor.Log("TempRefs not set", LogLevel.Error);
            else
            {
                if (Config.Verbose)
                    Monitor.Log($"TempRegs is set. Cleared {TempRefs.milkedtoday.Count}", LogLevel.Trace);

                TempRefs.milkedtoday.Clear();
            }

            //Game1.player.mailbox.Add("MilkButton1");
            //Game1.player.mailbox.Add("MilkingAbi");
            //Game1.addHUDMessage(new HUDMessage(string.Format("Adding quest {0}", (object)TempRefs.QuestID1)));
            //Game1.player.addQuest(TempRefs.QuestID1);
        }

        private void GetItemCodes()
        {
            LogLevel Defcon = LogLevel.Trace;
            TempRefs.Monitor.Log($"Correcting item codes", LogLevel.Trace);
            //Defcon = LogLevel.Alert;
            int num1 = 13;
            int num2 = 0;


            foreach (KeyValuePair<int, string> keyValuePair in _itemEditor.Report())
            {
                string[] strArray = keyValuePair.Value.Split('/');
                switch (strArray[0])
                {
                    #region girls
                    case "Abigail's Milk":
                        TempRefs.MilkAbig = keyValuePair.Key;
                        Monitor.Log($"{strArray[0]} added. {TempRefs.MilkAbig}", Defcon);
                        ++num2;
                        continue;

                    case "Caroline's Milk":
                        TempRefs.MilkCaro = keyValuePair.Key;
                        Monitor.Log($"{strArray[0]} added. {TempRefs.MilkCaro}", Defcon);
                        ++num2;
                        continue;

                    case "Emily's Milk":
                        TempRefs.MilkEmil = keyValuePair.Key;
                        Monitor.Log($"{strArray[0]} added. {TempRefs.MilkEmil}", Defcon);
                        ++num2;
                        continue;

                    case "Evelyn's Milk":
                        TempRefs.MilkEvel = keyValuePair.Key;
                        Monitor.Log($"{strArray[0]} added. {TempRefs.MilkEvel}", Defcon);
                        ++num2;
                        continue;

                    case "Haley's Milk":
                        TempRefs.MilkHale = keyValuePair.Key;
                        Monitor.Log($"{strArray[0]} added. {TempRefs.MilkHale}", Defcon);
                        ++num2;
                        continue;

                    case "Jodi's Milk":
                        TempRefs.MilkJodi = keyValuePair.Key;
                        Monitor.Log($"{strArray[0]} added. {TempRefs.MilkJodi}", Defcon);
                        ++num2;
                        continue;

                    case "Leah's Milk":
                        TempRefs.MilkLeah = keyValuePair.Key;
                        Monitor.Log($"{strArray[0]} added. {TempRefs.MilkLeah}", Defcon);
                        ++num2;
                        continue;

                    case "Marnie's Milk":
                        TempRefs.MilkMarn = keyValuePair.Key;
                        Monitor.Log($"{strArray[0]} added. {TempRefs.MilkMarn}", Defcon);
                        ++num2;
                        continue;

                    case "Maru's Milk":
                        TempRefs.MilkMaru = keyValuePair.Key;
                        Monitor.Log($"{strArray[0]} added. {TempRefs.MilkMaru}", Defcon);
                        ++num2;
                        continue;

                    case "Pam's Milk":
                        TempRefs.MilkPam = keyValuePair.Key;
                        Monitor.Log($"{strArray[0]} added. {TempRefs.MilkPam}", Defcon);
                        ++num2;
                        continue;

                    case "Penny's Milk":
                        TempRefs.MilkPenn = keyValuePair.Key;
                        Monitor.Log($"{strArray[0]} added. {TempRefs.MilkPenn}", Defcon);
                        ++num2;
                        continue;

                    case "Robin's Milk":
                        TempRefs.MilkRobi = keyValuePair.Key;
                        Monitor.Log($"{strArray[0]} added. {TempRefs.MilkRobi}", Defcon);
                        ++num2;
                        continue;

                    case "Sandra's Milk":
                        TempRefs.MilkSand = keyValuePair.Key;
                        Monitor.Log($"{strArray[0]} added. {TempRefs.MilkSand}", Defcon);
                        ++num2;
                        continue;
                    #endregion

                    #region guys
                    case "Alex's Cum":
                        TempRefs.MilkAlex = keyValuePair.Key;
                        Monitor.Log($"{strArray[0]} added. {TempRefs.MilkAlex}", Defcon);
                        ++num2;
                        continue;

                    case "Clint's Cum":
                        TempRefs.MilkClint = keyValuePair.Key;
                        Monitor.Log($"{strArray[0]} added. {TempRefs.MilkClint }", Defcon);
                        ++num2;
                        continue;

                    case "Demetrius's Cum":
                        TempRefs.MilkDemetrius = keyValuePair.Key;
                        Monitor.Log($"{strArray[0]} added. {TempRefs.MilkDemetrius }", Defcon);
                        ++num2;
                        continue;

                    case "Elliott's Cum":
                        TempRefs.MilkElliott = keyValuePair.Key;
                        Monitor.Log($"{strArray[0]} added. {TempRefs.MilkElliott }", Defcon);
                        ++num2;
                        continue;

                    case "George's Cum":
                        TempRefs.MilkGeorge = keyValuePair.Key;
                        Monitor.Log($"{strArray[0]} added. {TempRefs.MilkGeorge}", Defcon);
                        ++num2;
                        continue;

                    case "Gil's Cum":
                        TempRefs.MilkGil = keyValuePair.Key;
                        Monitor.Log($"{strArray[0]} added. {TempRefs.MilkGil }", Defcon);
                        ++num2;
                        continue;

                    case "Gunther's Cum":
                        TempRefs.MilkGunther = keyValuePair.Key;
                        Monitor.Log($"{strArray[0]} added. {TempRefs.MilkGunther }", Defcon);
                        ++num2;
                        continue;

                    case "Gus's Cum":
                        TempRefs.MilkGus = keyValuePair.Key;
                        Monitor.Log($"{strArray[0]} added. {TempRefs.MilkGus }", Defcon);
                        ++num2;
                        continue;

                    case "Harvey's Cum":
                        TempRefs.MilkHarv = keyValuePair.Key;
                        Monitor.Log($"{strArray[0]} added. {TempRefs.MilkHarv}", Defcon);
                        ++num2;
                        continue;

                    case "Kent's Cum":
                        TempRefs.MilkKent = keyValuePair.Key;
                        Monitor.Log($"{strArray[0]} added. {TempRefs.MilkKent}", Defcon);
                        ++num2;
                        continue;

                    case "Lewis's Cum":
                        TempRefs.MilkLewis = keyValuePair.Key;
                        Monitor.Log($"{strArray[0]} added. {TempRefs.MilkLewis }", Defcon);
                        ++num2;
                        continue;

                    case "Linus's Cum":
                        TempRefs.MilkLinus = keyValuePair.Key;
                        Monitor.Log($"{strArray[0]} added. {TempRefs.MilkLinus }", Defcon);
                        ++num2;
                        continue;

                    case "Marlon's Cum":
                        TempRefs.MilkMarlon = keyValuePair.Key;
                        Monitor.Log($"{strArray[0]} added. {TempRefs.MilkMarlon}", Defcon);
                        ++num2;
                        continue;

                    case "Morris's Cum":
                        TempRefs.MilkMorris = keyValuePair.Key;
                        Monitor.Log($"{strArray[0]} added. {TempRefs.MilkMorris}", Defcon);
                        ++num2;
                        continue;

                    case "Mr. Qi's Cum":
                        TempRefs.MilkQi = keyValuePair.Key;
                        Monitor.Log($"{strArray[0]} added. {TempRefs.MilkQi}", Defcon);
                        ++num2;
                        continue;

                    case "Pierre's Cum":
                        TempRefs.MilkPierre = keyValuePair.Key;
                        Monitor.Log($"{strArray[0]} added. {TempRefs.MilkPierre}", Defcon);
                        ++num2;
                        continue;

                    case "Sam's Cum":
                        TempRefs.MilkSam = keyValuePair.Key;
                        Monitor.Log($"{strArray[0]} added. {TempRefs.MilkSam }", Defcon);
                        ++num2;
                        continue;

                    case "Sebastian's Cum":
                        TempRefs.MilkSeb = keyValuePair.Key;
                        Monitor.Log($"{strArray[0]} added. {TempRefs.MilkSeb }", Defcon);
                        ++num2;
                        continue;

                    case "Shane's Cum":
                        TempRefs.MilkShane = keyValuePair.Key;
                        Monitor.Log($"{strArray[0]} added. {TempRefs.MilkShane }", Defcon);
                        ++num2;
                        continue;

                    case "Willy's Cum":
                        TempRefs.MilkWilly = keyValuePair.Key;
                        Monitor.Log($"{strArray[0]} added. {TempRefs.MilkWilly }", Defcon);
                        ++num2;
                        continue;

                    case "Wizard's Cum":
                        TempRefs.MilkWiz = keyValuePair.Key;
                        Monitor.Log($"{strArray[0]} added. {TempRefs.MilkWiz }", Defcon);
                        ++num2;
                        continue;
                    #endregion

                    case "Milk":
                        TempRefs.MilkGeneric = keyValuePair.Key;
                        Monitor.Log($"{strArray[0]} added as default. {TempRefs.MilkGeneric}", Defcon);
                        continue;

                    case "Special Milk":
                        TempRefs.MilkSpecial = keyValuePair.Key;
                        Monitor.Log($"{strArray[0]} added. {TempRefs.MilkSpecial}", Defcon);
                        continue;

                    case "Protein Shake":
                        TempRefs.ProteinShake = keyValuePair.Key;
                        Monitor.Log($"{strArray[0]} added. {TempRefs.ProteinShake}", Defcon);
                        continue;

                    case "Milkshake":
                        TempRefs.MilkShake = keyValuePair.Key;
                        Monitor.Log($"{strArray[0]} added. {TempRefs.MilkShake}", Defcon);
                        continue;

                    default:
                        if (strArray[0].ToLower().Contains("milk"))
                        {
                            Monitor.Log($"{strArray[0]} wasn't added.", Defcon);
                        }
                        continue;
                }
            }
            if (Config.StackMilk)
            {
                TempRefs.MilkAbig = TempRefs.MilkGeneric;
                TempRefs.MilkEmil = TempRefs.MilkGeneric;
                TempRefs.MilkHale = TempRefs.MilkGeneric;
                TempRefs.MilkLeah = TempRefs.MilkGeneric;
                TempRefs.MilkMaru = TempRefs.MilkGeneric;
                TempRefs.MilkPenn = TempRefs.MilkGeneric;
                TempRefs.MilkCaro = TempRefs.MilkGeneric;
                TempRefs.MilkJodi = TempRefs.MilkGeneric;
                TempRefs.MilkMarn = TempRefs.MilkGeneric;
                TempRefs.MilkRobi = TempRefs.MilkGeneric;
                TempRefs.MilkPam = TempRefs.MilkGeneric;
                TempRefs.MilkSand = TempRefs.MilkGeneric;
                TempRefs.MilkEvel = TempRefs.MilkGeneric;
            }

            TempRefs.loaded = true;
            Monitor.Log($"Loaded {num2}/{num1} items", Defcon);

            Game1.player.Items[0].getCategoryName();

            #region fix item strings
            IDictionary<int, string> _objectData = _itemEditor._objectData;

            //milk items
            _objectData[TempRefs.MilkAbig] = $"Abigail's Milk/300/15/Basic {TempRefs.MilkType}/Abigail's Milk/A jug of Abigail's milk./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
            _objectData[TempRefs.MilkEmil] = $"Emily's Milk/300/15/Basic {TempRefs.MilkType}/Emily's Milk/A jug of Emily's milk./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
            _objectData[TempRefs.MilkHale] = $"Haley's Milk/300/15/Basic {TempRefs.MilkType}/Haley's Milk/A jug of Haley's milk./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
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
            _objectData[TempRefs.MilkGeneric] = $"Woman's Milk/50/15/Basic {TempRefs.MilkType}/Woman's Milk/A jug of woman's milk./Drink/0 0 0 0 0 0 0 0 0 0 0/0";

            //cum items
            _objectData[TempRefs.MilkSpecial] = $"Special milk/50/15/Basic {TempRefs.CumType}/'Special' Milk/A bottle of 'special' milk./Drink/0 0 0 0 0 0 0 0 0 0 0/0";

            //recipes
            _objectData[TempRefs.ProteinShake] = $"Protein shake/50/15/Basic -6/'Protein' shake/Shake made with extra protein/Drink/0 0 0 0 0 0 0 0 0 0 0/0";
            _objectData[TempRefs.MilkShake] = $"Milkshake/50/15/Basic -6/'Special' Milkshake/Extra milky milkshake./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
            #endregion
        }

        private void CorrectRecipes()
        {
            _recipeEditor.data["'Protein' Shake"] = $"{TempRefs.CumType} 1/10 10/{TempRefs.ProteinShake}/default/'Protein' shake";
            _recipeEditor.data["Milkshake"] = $"{TempRefs.MilkType} 1/10 10/{TempRefs.MilkShake}/default/Milkshake";
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
                        Game1.addHUDMessage(new HUDMessage("no-one found."));

                }
                //}
                //else
                //    Game1.addHUDMessage(new HUDMessage("Wrong tool equipped."));
            }
        }

        private void ActionOnNPC(NPC npc)
        {
            if (Config.NeedTool && Game1.player.CurrentTool.GetType().ToString() != typeof(StardewValley.Tools.MilkPail).ToString())
            {
                Game1.addHUDMessage(new HUDMessage("You need to be holding a pail to milk people."));
                return;
            }

            #region validity checks

            //Age and friendship
            if (npc.age > 1 || Game1.player.getFriendshipHeartLevelForNPC(npc.name) < 8)
            {
                Game1.drawDialogue(npc, "Hey there @. Wotcha doing with that pail?#I don't like you enough for that!");
                Monitor.Log($"{npc.name} is {npc.age} and at heart level {Game1.player.getFriendshipHeartLevelForNPC(npc.name)}", LogLevel.Debug);
                return;
            }

            //gender check 0 is male, 1 is female
            if (npc.gender == 0 & !Config.MilkMale)
            {
                Game1.addHUDMessage(new HUDMessage("You have male character milking turned off."));
                Monitor.Log($"gender is {npc.Gender}", LogLevel.Warn);
                return;
            }
            if (npc.gender == 1 & !Config.MilkFemale)
            {

                Game1.addHUDMessage(new HUDMessage("You have female character milking turned off."));
                Monitor.Log($"gender is {npc.Gender}", LogLevel.Warn);
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

            npc.facePlayer(Game1.player);
            if (npc.Dialogue.TryGetValue("milk_start", out string dialogues)) //Does npc have milking dialogue?
            {
                Game1.drawDialogue(npc, dialogues);
            }
            else
            {
                if (npc.gender == 1)
                    Game1.drawDialogue(npc, $"You want to milk me? Are you crazy...? Although, that DOES sound kinda hot.#$b#You spend the next few minutes slowly kneeding their breasts, collecting the milk in a jar you brought with you. [{TempRefs.MilkGeneric}]");
                else
                    Game1.drawDialogue(npc, $"You want my \"milk\"? Erm, You ARE very attractive...#$b#*You quickly unzip their pants and pull out their cock. After a couple of quick licks to get them hard, you start sucking on them*#$b#I think I'm getting close! Here it comes! [{TempRefs.MilkSpecial}]");
            }

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

    public class ToolsLoader : IAssetEditor
    {
        private readonly Texture2D _toolsSpriteSheet;
        private readonly Texture2D _menuTilesSpriteSheet;
        private readonly Texture2D _customLetterBG;

        public ToolsLoader(
          Texture2D toolsSpriteSheet,
          Texture2D menuTilesSpriteSheet,
          Texture2D customLetterBG)
        {
            this._toolsSpriteSheet = toolsSpriteSheet;
            this._menuTilesSpriteSheet = menuTilesSpriteSheet;
            this._customLetterBG = customLetterBG;
        }

        public bool CanEdit<T>(IAssetInfo asset)
        {
            return asset.AssetNameEquals("TileSheets\\tools") || asset.AssetNameEquals("Maps\\MenuTiles");
        }

        public void Edit<T>(IAssetData asset)
        {
        }

    }

    [XmlInclude(typeof(SpellMilk))]
    [XmlInclude(typeof(SpellTeleport))]
    [Serializable]
    public class SpellTouch : StardewValley.Tools.MilkPail
    {
        public static int AttachmentMenuTile = 90;
        public int Range = 1;

        public override bool beginUsing(GameLocation location, int x, int y, Farmer who)
        {
            Game1.addHUDMessage(new HUDMessage(who.displayName + " started casting " + this.Name));

            return base.beginUsing(location, x, y, who);
        }

        private List<int[]> ValidCells(Farmer who)
        {
            int[] numArray = new int[2]
            {
        who.getTileX(),
        who.getTileY()
            };
            List<int[]> numArrayList = new List<int[]>();
            switch (who.FacingDirection)
            {
                case 0:
                    for (int index = 1; index <= this.Range; ++index)
                        numArrayList.Add(new int[2]
                        {
              numArray[0],
              numArray[1] - index
                        });
                    break;
                case 1:
                    for (int index = 1; index <= this.Range; ++index)
                        numArrayList.Add(new int[2]
                        {
              numArray[0] + index,
              numArray[1]
                        });
                    break;
                case 2:
                    for (int index = 1; index <= this.Range; ++index)
                        numArrayList.Add(new int[2]
                        {
              numArray[0],
              numArray[1] + index
                        });
                    break;
                case 3:
                    for (int index = 1; index <= this.Range; ++index)
                        numArrayList.Add(new int[2]
                        {
              numArray[0] - index,
              numArray[1]
                        });
                    break;
            }
            return numArrayList;
        }

        protected NPC CurrentTarget(GameLocation location, Farmer who)
        {
            foreach (int[] validCell in this.ValidCells(who))
            {
                using (List<NPC>.Enumerator enumerator = location.characters.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        NPC current = enumerator.Current;
                        if (validCell[0] == current.getTileX() && validCell[1] == current.getTileY())
                            return current;
                    }
                }
            }
            return (NPC)null;
        }

        protected override string loadDisplayName()
        {
            return this.Name ?? "";
        }

        protected override string loadDescription()
        {
            return string.Format("{0} Range {1}", (object)this.description, (object)this.Range);
        }

        public override Item getOne()
        {
            return (Item)new SpellTouch();
        }
    }

    public class SpellMilk : SpellTouch
    {
        public SpellMilk()
        {
            Name = "Milk";
            description = "Lets you milk villagers.";
            Range = 1;
            initialParentTileIndex.Value = 2;
            indexOfMenuItemView.Value = 2;
            Stackable = false;
            CurrentParentTileIndex = initialParentTileIndex;
            numAttachmentSlots.Value = 1;
            attachments.SetCount(numAttachmentSlots);
            Category = -99;
        }

        public override bool beginUsing(GameLocation location, int x, int y, Farmer who)
        {
            this.Update((int)who.facingDirection, 0, who);
            who.EndUsingTool();
            return true;
        }

        public override void DoFunction(GameLocation location, int x, int y, int power, Farmer who)
        {
            NPC npc = CurrentTarget(location, who);
            if (npc == null)
                return;
            if (Game1.player.getFriendshipHeartLevelForNPC(npc.name) < 8)
                npc.addExtraDialogues("Hey there @. Wotcha doing with that pail?");
            if (TempRefs.milkedtoday.Contains(npc))
                return;
            npc.facePlayer(Game1.player);

            if (npc.Dialogue.TryGetValue("milk_start", out string dialogues))
            {
                Game1.addHUDMessage(new HUDMessage(string.Format("Starting milking process with {0}", npc.name)));

                npc.addExtraDialogues(dialogues);
                npc.checkAction(Game1.player, Game1.currentLocation);
                TempRefs.milkedtoday.Add(npc);
            }
            else
            {
                if ((int)(NetFieldBase<int, NetInt>)npc.gender != 1)
                    return;
                npc.addExtraDialogues("You want to milk me? Are you crazy...? Although, that DOES sound kinda hot. [174]");
                npc.checkAction(Game1.player, Game1.currentLocation);
                TempRefs.milkedtoday.Add(npc);
            }
        }
    }

    public class SpellTeleport : SpellSelf
    {
        public SpellTeleport()
        {
            this.Name = "Teleport Random";
            this.description = "Teleports you to a random location";
            this.Range = 12;
            this.initialParentTileIndex.Value = 0;
            this.indexOfMenuItemView.Value = 2;
            this.Stackable = false;
            this.CurrentParentTileIndex = (int)(NetFieldBase<int, NetInt>)this.initialParentTileIndex;
            this.numAttachmentSlots.Value = 1;
            this.attachments.SetCount((int)(NetFieldBase<int, NetInt>)this.numAttachmentSlots);
            this.Category = -99;
        }

        public override void DoFunction(GameLocation location, int x, int y, int power, Farmer who)
        {
            base.DoFunction(location, x, y, power, who);
            bool nonWarpFade = Game1.nonWarpFade;
            int[] numArray = this.NewTarget(location, who, this.Range);
            Game1.nonWarpFade = true;
            Game1.warpFarmer((string)(NetFieldBase<string, NetString>)location.name, numArray[0], numArray[1], false);
            Game1.nonWarpFade = nonWarpFade;
        }

        private int[] NewTarget(GameLocation location, Farmer who, int range)
        {
            int[] numArray1 = new int[2]
            {
        who.getTileX(),
        who.getTileY()
            };
            int[] numArray2 = new int[2];
            Random random = new Random();
            int num = 0;
            while (num < 5)
            {
                numArray2[0] = numArray1[0] + random.Next(-range, range);
                numArray2[1] = numArray1[1] + random.Next(-range, range);
                if (location.isTileOnMap(numArray2[0], numArray2[1]))
                    return numArray2;
            }
            return numArray1;
        }
    }

    public class SpellSelf : Tool
    {
        public static int AttachmentMenuTile = 91;
        public string SpellName = nameof(SpellName);
        public int Range;

        protected override string loadDisplayName()
        {
            return this.SpellName ?? "";
        }

        protected override string loadDescription()
        {
            return string.Format("{0} Range {1}", (object)this.description, (object)this.Range);
        }

        public override Item getOne()
        {
            return (Item)new SpellSelf();
        }
    }


}
