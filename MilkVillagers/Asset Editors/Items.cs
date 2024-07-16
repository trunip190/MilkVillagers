// Ignore Spelling: Initialised

using SpaceCore.Spawnables;
using StardewModdingAPI;
using StardewValley.GameData.Objects;
using StardewValley.Objects;
using System;
using System.Collections.Generic;
using SObject = StardewValley.Object;
using log = MilkVillagers.ModFunctions;
using StardewValley.GameData.Shirts;
using StardewValley.GameData.Pants;
using System.Linq;

namespace MilkVillagers.Asset_Editors
{
    public static class ItemEditor //: IAssetEditor
    {
        private static IDictionary<string, ObjectData> ItemData; //Game item reference. reflects game items.
        private static IDictionary<string, string> ContextData;
        public static Dictionary<string, ObjectData> ModItems = new()
        {
            // Male Cum
            ["Alex's Cum"] = null,
            ["Clint's Cum"] = null,
            ["Demetrius's Cum"] = null,
            ["Elliott's Cum"] = null,
            ["George's Cum"] = null,
            ["Gil's Cum"] = null,
            ["Gunther's Cum"] = null,
            ["Gus's Cum"] = null,
            ["Harvey's Cum"] = null,
            ["Kent's Cum"] = null,
            ["Lance's Cum"] = null,
            ["Lewis's Cum"] = null,
            ["Linus's Cum"] = null,
            ["Marlon's Cum"] = null,
            ["Morris's Cum"] = null,
            ["Pierre's Cum"] = null,
            ["Qi's Cum"] = null,
            ["Sam's Cum"] = null,
            ["Sebastian's Cum"] = null,
            ["Shane's Cum"] = null,
            ["Willy's Cum"] = null,


            // SDV Male Cum
            ["Andy's Cum"] = null,
            ["Martin's Cum"] = null,
            ["Victor's Cum"] = null,

            // Magical
            ["Wizard's Cum"] = null,
            ["Dwarf's Essence"] = null,
            ["Krobus Essence"] = null,
            ["Magical Essence"] = null,
            ["Wizard's Essence"] = null,
            ["Mr. Qi's Essence"] = null,

            //Female Milk
            ["Abigail's Milk"] = null,
            ["Caroline's Milk"] = null,
            ["Emily's Milk"] = null,
            ["Evelyn's Milk"] = null,
            ["Haley's Milk"] = null,
            ["Jodi's Milk"] = null,
            ["Leah's Milk"] = null,
            ["Marnie's Milk"] = null,
            ["Maru's Milk"] = null,
            ["Pam's Milk"] = null,
            ["Penny's Milk"] = null,
            ["Robin's Milk"] = null,
            ["Sandra's Milk"] = null,
            ["Sandy's Milk"] = null,
            ["Special Milk"] = null,
            ["Woman's Milk"] = null,

            // SVE Female Milk
            ["Claire's Milk"] = null,
            ["Sophia's Milk"] = null,
            ["Olivia's Milk"] = null,
            ["Susan's Milk"] = null,

            // Recipes
            ["Eldritch Energy"] = null,
            ["Martini Kairos"] = null,
            ["Protein Shake"] = null,
            ["Special Milkshake"] = null,
            ["Super Juice"] = null,
            ["Sweet Sibling"] = null,


            // Quest Items
            ["Abigails Panties"] = null,
            ["Boethia's Pillow Book"] = null,
            ["Creamy Coffee"] = null,
            ["Haley's Camera"] = null,
            ["Haley's Panties"] = null,
            ["Invitation"] = null,
            ["Readi Milk"] = null,
            ["Shibari rope"] = null,
            ["Strapon"] = null,

            // Clothing
            ["mtvTeddyu"] = null
        };

        public static bool Initialised => ItemData != null;

        public static void Report()
        {
            List<string> failed = new List<string>();

            //foreach (KeyValuePair<string, ObjectData> kvp in ModItems)
            foreach (KeyValuePair<string, ObjectData> kvp in ItemData)
            {
                //if (ItemData.ContainsKey(kvp.Value.Name))
                //{
                try
                {
                    //output.LoadString(ItemData[kvp.Value]);
                    log.Log($"{kvp.Key}: {ItemData[kvp.Key].DisplayName}", LogLevel.Trace);
                }
                catch
                {
                    failed.Add(kvp.Key);
                }
                //}
            }

            foreach (string s in failed)
            {
                log.Log($"Failed to output {s}", LogLevel.Alert, Force: true);
            }
        }

        public static bool CanEdit<T>(IAssetInfo asset)
        {
            return (asset.Name.IsEquivalentTo("Data/Objects") || asset.Name.IsEquivalentTo("Data/ObjectContextTags"));
        }

        public static bool CanEdit(IAssetName AssetName)
        {
            bool result = AssetName.IsEquivalentTo("Data/Objects") || AssetName.IsEquivalentTo("Data/ObjectContextTags");

            // if (result) log.LogVerbose("Asking to edit items");

            return result;
        }

        public static void Edit<T>(IAssetData asset)
        {
            EditAsset(asset);

        }

        public static void Edit(IAssetData asset)
        {
            EditAsset(asset);

        }

        private static void EditAsset(IAssetData asset)
        {
            // log.LogVerbose("Adding in items");

            if (asset.Name.IsEquivalentTo("Data/Objects"))
            {
                ItemData = asset.AsDictionary<string, ObjectData>().Data;

                Dictionary<string, ObjectData> results = new Dictionary<string, ObjectData>();

                foreach (KeyValuePair<string, ObjectData> kvp in ItemData)
                {
                    try
                    {
                        if (kvp.Value == null || kvp.Value.Name == null) continue;

                        if (kvp.Value.Name.ToLower().Contains("teddy"))
                            results.Add(kvp.Key, kvp.Value);
                    }
                    catch
                    {
                        log.Log("Failed in Items:EditAsset", LogLevel.Alert, Force: true);
                    }
                }

                if (!TempRefs.loaded)
                {
                    log.Log("TempRefs not loaded.");
                    return;
                }

                SetItems();
            }

            if (asset.Name.IsEquivalentTo("Data/ObjectContextTags"))
            {
                ContextData = asset.AsDictionary<string, string>().Data;

                EditContextData();
            }

        }

        private static void EditContextData()
        {
            //ContextData["Abigail's Milk"] = "breast_milk_item";
            //ContextData["Caroline's Milk"] = "breast_milk_item";
            //ContextData["Claire's Milk"] = "breast_milk_item";
            //ContextData["Emily's Milk"] = "breast_milk_item";
            //ContextData["Evelyn's Milk"] = "breast_milk_item";
            //ContextData["Haley's Milk"] = "breast_milk_item";
            //ContextData["Jodi's Milk"] = "breast_milk_item";
            //ContextData["Leah's Milk"] = "breast_milk_item";
            //ContextData["Marnie's Milk"] = "breast_milk_item";
            //ContextData["Maru's Milk"] = "breast_milk_item";
            //ContextData["Olivia's Milk"] = "breast_milk_item";
            //ContextData["Pam's Milk"] = "breast_milk_item";
            //ContextData["Penny's Milk"] = "breast_milk_item";
            //ContextData["Robin's Milk"] = "breast_milk_item";
            //ContextData["Sandy's Milk"] = "breast_milk_item";
            //ContextData["Sophia's Milk"] = "breast_milk_item";
            //ContextData["Susan's Milk"] = "breast_milk_item";
            //ContextData["Woman's Milk"] = "breast_milk_item";

            //ContextData["Alex's Cum"] = "human_cum_item";
            //ContextData["Andy's Cum"] = "human_cum_item";
            //ContextData["Clint's Cum"] = "human_cum_item";
            //ContextData["Demetrius's Cum"] = "human_cum_item";
            //ContextData["Elliott's Cum"] = "human_cum_item";
            //ContextData["George's Cum"] = "human_cum_item";
            //ContextData["Gil's Cum"] = "human_cum_item";
            //ContextData["Gunther's Cum"] = "human_cum_item";
            //ContextData["Gus's Cum"] = "human_cum_item";
            //ContextData["Harvey's Cum"] = "human_cum_item";
            //ContextData["Kent's Cum"] = "human_cum_item";
            //ContextData["Lewis's Cum"] = "human_cum_item";
            //ContextData["Linus's Cum"] = "human_cum_item";
            //ContextData["Marlon's Cum"] = "human_cum_item";
            //ContextData["Martin's Cum"] = "human_cum_item";
            //ContextData["Morris's Cum"] = "human_cum_item";
            //ContextData["Pierre's Cum"] = "human_cum_item";
            //ContextData["Sam's Cum"] = "human_cum_item";
            //ContextData["Sebastian's Cum"] = "human_cum_item";
            //ContextData["Shane's Cum"] = "human_cum_item";
            //ContextData["Victor's Cum"] = "human_cum_item";
            //ContextData["Willy's Cum"] = "human_cum_item";
            //ContextData["Special Milk"] = "human_cum_item";

            //ContextData["Wizard's Cum"] = "human_cum_item,magic_essence_item";
            //ContextData["Dwarf's Milk"] = "breast_milk_item,magic_essence_item";
            //ContextData["Mr.Qi's Essence"] = "magic_essence_item";
            //ContextData["Magical Essence"] = "magic_essence_item";

            //ContextData["Eldritch Energy"] = "cooking_item";
            //ContextData["Protein Shake"] = "cooking_item";
            //ContextData["Special Milkshake"] = "cooking_item";
            //ContextData["Super Juice"] = "cooking_item";

            //ContextData["Boethia's Pillow Book"] = "quest_item";
        }

        public static void UpdateData(Dictionary<string, ObjectData> assetdata)
        {
            ItemData = assetdata;
            SetItems();
        }

        public static void RemoveInvalid(bool Male, bool Female)
        {
            //log.Log("Logging Items");
            //foreach ( string s in ItemData.Keys.Where(o=>o.Contains("Trunip190.CP.MilkTheVillagers")).ToList())
            //{
            //    log.Log($"{s}");
            //}

            //if (!Female)
            //{
            //    //milk items
            //    ItemData.Remove("Trunip190.CP.MilkTheVillagers.Woman's_Milk");
            //    ItemData.Remove("Trunip190.CP.MilkTheVillagers.Abigail's_Milk");
            //    ItemData.Remove("Trunip190.CP.MilkTheVillagers.Caroline's_Milk");
            //    ItemData.Remove("Trunip190.CP.MilkTheVillagers.Claire's_Milk");
            //    ItemData.Remove("Trunip190.CP.MilkTheVillagers.Emily's_Milk");
            //    ItemData.Remove("Trunip190.CP.MilkTheVillagers.Evelyn's_Milk");
            //    ItemData.Remove("Trunip190.CP.MilkTheVillagers.Haley's_Milk");
            //    ItemData.Remove("Trunip190.CP.MilkTheVillagers.Jodi's_Milk");
            //    ItemData.Remove("Trunip190.CP.MilkTheVillagers.Leah's_Milk");
            //    ItemData.Remove("Trunip190.CP.MilkTheVillagers.Marnie's_Milk");
            //    ItemData.Remove("Trunip190.CP.MilkTheVillagers.Maru's_Milk");
            //    ItemData.Remove("Trunip190.CP.MilkTheVillagers.Olivia's_Milk");
            //    ItemData.Remove("Trunip190.CP.MilkTheVillagers.Pam's_Milk");
            //    ItemData.Remove("Trunip190.CP.MilkTheVillagers.Penny's_Milk");
            //    ItemData.Remove("Trunip190.CP.MilkTheVillagers.Robin's_Milk");
            //    ItemData.Remove("Trunip190.CP.MilkTheVillagers.Sandy's_Milk");
            //    ItemData.Remove("Trunip190.CP.MilkTheVillagers.Sophia's_Milk");
            //    ItemData.Remove("Trunip190.CP.MilkTheVillagers.Susan's_Milk");

            //    // Magical
            //    ItemData.Remove("Trunip190.CP.MilkTheVillagers.Dwarf's_Essence");
            //    ItemData.Remove("Trunip190.CP.MilkTheVillagers.Krobus_Essence");

            //    // Recipes
            //    ItemData.Remove("Trunip190.CP.MilkTheVillagers.Woman's_Milk");
            //    ItemData.Remove("Trunip190.CP.MilkTheVillagers.Special_Milkshake");
            //    ItemData.Remove("Trunip190.CP.MilkTheVillagers.Creamy_Coffee");
            //    ItemData.Remove("Trunip190.CP.MilkTheVillagers.Sweet_Sibling");

            //}

            //if (!Male)
            //{
            //    //cum items
            //    ItemData.Remove("Trunip190.CP.MilkTheVillagers.Special_Milk");
            //    ItemData.Remove("Trunip190.CP.MilkTheVillagers.Alex's_Cum");
            //    ItemData.Remove("Trunip190.CP.MilkTheVillagers.Andy's_Cum");
            //    ItemData.Remove("Trunip190.CP.MilkTheVillagers.Clint's_Cum");
            //    ItemData.Remove("Trunip190.CP.MilkTheVillagers.Demetrius's_Cum");
            //    ItemData.Remove("Trunip190.CP.MilkTheVillagers.Elliott's_Cum");
            //    ItemData.Remove("Trunip190.CP.MilkTheVillagers.George's_Cum");
            //    ItemData.Remove("Trunip190.CP.MilkTheVillagers.Gil's_Cum");
            //    ItemData.Remove("Trunip190.CP.MilkTheVillagers.Gunther's_Cum");
            //    ItemData.Remove("Trunip190.CP.MilkTheVillagers.Gus's_Cum");
            //    ItemData.Remove("Trunip190.CP.MilkTheVillagers.Harvey's_Cum");
            //    ItemData.Remove("Trunip190.CP.MilkTheVillagers.Kent's_Cum");
            //    ItemData.Remove("Trunip190.CP.MilkTheVillagers.Lance's_Cum");
            //    ItemData.Remove("Trunip190.CP.MilkTheVillagers.Lewis's_Cum");
            //    ItemData.Remove("Trunip190.CP.MilkTheVillagers.Linus's_Cum");
            //    ItemData.Remove("Trunip190.CP.MilkTheVillagers.Marlon's_Cum");
            //    ItemData.Remove("Trunip190.CP.MilkTheVillagers.Martin's_Cum");
            //    ItemData.Remove("Trunip190.CP.MilkTheVillagers.Morris's_Cum");
            //    ItemData.Remove("Trunip190.CP.MilkTheVillagers.Pierre's_Cum");
            //    ItemData.Remove("Trunip190.CP.MilkTheVillagers.Sam's_Cum");
            //    ItemData.Remove("Trunip190.CP.MilkTheVillagers.Sebastian's_Cum");
            //    ItemData.Remove("Trunip190.CP.MilkTheVillagers.Shane's_Cum");
            //    ItemData.Remove("Trunip190.CP.MilkTheVillagers.Victor's_Cum");
            //    ItemData.Remove("Trunip190.CP.MilkTheVillagers.Willy's_Cum");

            //    // Magical
            //    ItemData.Remove("Trunip190.CP.MilkTheVillagers.Mr._Qi's_Essence");
            //    ItemData.Remove("Trunip190.CP.MilkTheVillagers.Wizard's_Essence");

            //    // Recipes
            //    ItemData.Remove("Trunip190.CP.MilkTheVillagers.Protein_Shake");
            //    ItemData.Remove("Trunip190.CP.MilkTheVillagers.Special_Milk");

            //}

            //// Special materials
            //ItemData.Remove("Trunip190.CP.MilkTheVillagers.Dwarf's_Essence");
            //ItemData.Remove("Trunip190.CP.MilkTheVillagers.Krobus_Essence");
            //ItemData.Remove("Trunip190.CP.MilkTheVillagers.Wizard's_Essence");
            //ItemData.Remove("Trunip190.CP.MilkTheVillagers.Mr._Qi's_Essence");

            //ItemData.Remove("Trunip190.CP.MilkTheVillagers.Eldritch_Energy");
            //ItemData.Remove("Trunip190.CP.MilkTheVillagers.Martini_Kairos");

            //if (!(Male || Female))
            //    ItemData.Remove("Trunip190.CP.MilkTheVillagers.Super_Juice");

            SetItems(Male, Female);
        }

        public static void SetItems(bool Male = true, bool Female = true)
        {
            if (ItemData == null)
                return;

            GetAllItemIDs();

            if (Female)
            {
                // milk items
                //if (ItemData.ContainsKey($"{TempRefs.ModItemPrefix}Abigail's_Milk")) ItemData[$"{TempRefs.ModItemPrefix}Abigail's_Milk"].Category = -34;  // $"Abigail's Milk/300/30/Drink {TempRefs.MilkType}/Abigail's Milk/A jug of Abigail's milk./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
                //if (ItemData.ContainsKey(TempRefs.MilkEmil)) ItemData[TempRefs.MilkEmil] = ItemData[TempRefs.MilkEmil].Replace("Milk -6", $"Drink {TempRefs.MilkType}"); // $"Emily's Milk/300/30/Drink {TempRefs.MilkType}/Emily's Milk/A jug of Emily's milk./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
                //if (ItemData.ContainsKey(TempRefs.MilkHale)) ItemData[TempRefs.MilkHale] = ItemData[TempRefs.MilkHale].Replace("Milk -6", $"Drink {TempRefs.MilkType}"); // $"Haley's Milk/300/30/Drink {TempRefs.MilkType}/Haley's Milk/A jug of Haley's milk./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
                //if (ItemData.ContainsKey(TempRefs.MilkLeah)) ItemData[TempRefs.MilkLeah] = ItemData[TempRefs.MilkLeah].Replace("Milk -6", $"Drink {TempRefs.MilkType}"); // $"Leah's Milk/300/30/Drink {TempRefs.MilkType}/Leah's Milk/A jug of Leah's milk./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
                //if (ItemData.ContainsKey(TempRefs.MilkMaru)) ItemData[TempRefs.MilkMaru] = ItemData[TempRefs.MilkMaru].Replace("Milk -6", $"Drink {TempRefs.MilkType}"); // $"Maru's Milk/300/30/Drink {TempRefs.MilkType}/Maru's Milk/A jug of Maru's milk./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
                //if (ItemData.ContainsKey(TempRefs.MilkPenn)) ItemData[TempRefs.MilkPenn] = ItemData[TempRefs.MilkPenn].Replace("Milk -6", $"Drink {TempRefs.MilkType}"); // $"Penny's Milk/300/30/Drink {TempRefs.MilkType}/Penny's Milk/A jug of Penny's milk./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
                //if (ItemData.ContainsKey(TempRefs.MilkCaro)) ItemData[TempRefs.MilkCaro] = ItemData[TempRefs.MilkCaro].Replace("Milk -6", $"Drink {TempRefs.MilkType}"); // $"Caroline's Milk/300/30/Drink {TempRefs.MilkType}/Caroline's Milk/A jug of Caroline's milk./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
                //if (ItemData.ContainsKey(TempRefs.MilkJodi)) ItemData[TempRefs.MilkJodi] = ItemData[TempRefs.MilkJodi].Replace("Milk -6", $"Drink {TempRefs.MilkType}"); // $"Jodi's Milk/200/20/Drink {TempRefs.MilkType}/Jodi's Milk/A jug of Jodi's milk./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
                //if (ItemData.ContainsKey(TempRefs.MilkMarn)) ItemData[TempRefs.MilkMarn] = ItemData[TempRefs.MilkMarn].Replace("Milk -6", $"Drink {TempRefs.MilkType}"); // $"Marnie's Milk/200/20/Drink {TempRefs.MilkType}/Marnie's Milk/A jug of Marnie's milk./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
                //if (ItemData.ContainsKey(TempRefs.MilkRobi)) ItemData[TempRefs.MilkRobi] = ItemData[TempRefs.MilkRobi].Replace("Milk -6", $"Drink {TempRefs.MilkType}"); // $"Robin's Milk/200/20/Drink {TempRefs.MilkType}/Robin's Milk/A jug of Robin's milk./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
                //if (ItemData.ContainsKey(TempRefs.MilkPam)) ItemData[TempRefs.MilkPam] = ItemData[TempRefs.MilkPam].Replace("Milk -6", $"Drink {TempRefs.MilkType}"); //$"Pam's Milk/90/10/Drink {TempRefs.MilkType}/Pam's Milk/A jug of Pam's milk./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
                //if (ItemData.ContainsKey(TempRefs.MilkSand)) ItemData[TempRefs.MilkSand] = ItemData[TempRefs.MilkSand].Replace("Milk -6", $"Drink {TempRefs.MilkType}"); // $"Sandy's Milk/200/20/Drink {TempRefs.MilkType}/Sandy's Milk/A jug of Sandy's milk./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
                //if (ItemData.ContainsKey(TempRefs.MilkEvel)) ItemData[TempRefs.MilkEvel] = ItemData[TempRefs.MilkEvel].Replace("Milk -6", $"Drink {TempRefs.MilkType}"); // $"Evelyn's Milk/90/10/Drink {TempRefs.MilkType}/Evelyn's Milk/A jug of Evelyn's milk./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
                //// Other mods
                //if (ItemData.ContainsKey(TempRefs.MilkSophia)) ItemData[TempRefs.MilkSophia] = ItemData[TempRefs.MilkSophia].Replace("Milk -6", $"Drink {TempRefs.MilkType}"); // $"Sophia's Milk/300/30/Drink {TempRefs.MilkType}/Sophia's Milk/A jug of Sophia's milk./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
                //if (ItemData.ContainsKey(TempRefs.MilkOlivia)) ItemData[TempRefs.MilkOlivia] = ItemData[TempRefs.MilkOlivia].Replace("Milk -6", $"Drink {TempRefs.MilkType}"); // $"Olivia's Milk/200/20/Drink {TempRefs.MilkType}/Olivia's Milk/A jug of Olivia's milk./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
                //if (ItemData.ContainsKey(TempRefs.MilkSusan)) ItemData[TempRefs.MilkSusan] = ItemData[TempRefs.MilkSusan].Replace("Milk -6", $"Drink {TempRefs.MilkType}"); //$"Susan's Milk/300/30/Drink {TempRefs.MilkType}/Susan's Milk/A jug of Susan 's milk./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
                //if (ItemData.ContainsKey(TempRefs.MilkClaire)) ItemData[TempRefs.MilkClaire] = ItemData[TempRefs.MilkClaire].Replace("Milk -6", $"Drink {TempRefs.MilkType}"); // $"Claire's Milk/200/20/Drink {TempRefs.MilkType}/Claire's Milk/A jug of Claire's milk./Drink/0 0 0 0 0 0 0 0 0 0 0/0";

                ////recipe
                //if (ItemData.ContainsKey(TempRefs.MilkGeneric)) ItemData[TempRefs.MilkGeneric] = ItemData[TempRefs.MilkGeneric].Replace("Milk -6", $"Drink {TempRefs.MilkType}");  //$"Breast Milk/50/15/Cooking {TempRefs.MilkType}/Breast Milk/A jug of breast milk./drink/0 0 0 0 0 0 0 0 0 0 0/0";

            }

            //if (Male)
            //{
            //    //cum items
            //    if (ItemData.ContainsKey(TempRefs.MilkAlex)) ItemData[TempRefs.MilkAlex] = ItemData[TempRefs.MilkAlex].Replace("Milk -6", $"Drink {TempRefs.CumType}");      //$"Alex's Cum/300/15/Drink {TempRefs.CumType}/Alex's Cum /A bottle of Alex's Cum./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
            //    if (ItemData.ContainsKey(TempRefs.MilkClint)) ItemData[TempRefs.MilkClint] = ItemData[TempRefs.MilkClint].Replace("Milk -6", $"Drink {TempRefs.CumType}");     //$"Clint's Cum/300/15/Drink {TempRefs.CumType}/Clint's Cum/A bottle of Clint's Cum./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
            //    if (ItemData.ContainsKey(TempRefs.MilkDemetrius)) ItemData[TempRefs.MilkDemetrius] = ItemData[TempRefs.MilkDemetrius].Replace("Milk -6", $"Drink {TempRefs.CumType}"); //$"Demetrius's Cum/300/15/Drink {TempRefs.CumType}/Demetrius's Cum/A bottle of Demetrius's Cum./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
            //    if (ItemData.ContainsKey(TempRefs.MilkElliott)) ItemData[TempRefs.MilkElliott] = ItemData[TempRefs.MilkElliott].Replace("Milk -6", $"Drink {TempRefs.CumType}");   //$"Elliott's Cum/300/15/Drink {TempRefs.CumType}/Elliott's Cum/A bottle of Elliott's Cum./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
            //    if (ItemData.ContainsKey(TempRefs.MilkGeorge)) ItemData[TempRefs.MilkGeorge] = ItemData[TempRefs.MilkGeorge].Replace("Milk -6", $"Drink {TempRefs.CumType}");  //$"George's Cum/300/15/Drink {TempRefs.CumType}/George's Cum /A bottle of George's Cum./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
            //    if (ItemData.ContainsKey(TempRefs.MilkGil)) ItemData[TempRefs.MilkGil] = ItemData[TempRefs.MilkGil].Replace("Milk -6", $"Drink {TempRefs.CumType}");  //$"Gil's Cum/300/15/Drink {TempRefs.CumType}/Gil's Cum/A bottle of Gil's Cum./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
            //    if (ItemData.ContainsKey(TempRefs.MilkGunther)) ItemData[TempRefs.MilkGunther] = ItemData[TempRefs.MilkGunther].Replace("Milk -6", $"Drink {TempRefs.CumType}");  //$"Gunther's Cum/300/15/Drink {TempRefs.CumType}/Gunther's Cum/A bottle of Gunther's Cum./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
            //    if (ItemData.ContainsKey(TempRefs.MilkGus)) ItemData[TempRefs.MilkGus] = ItemData[TempRefs.MilkGus].Replace("Milk -6", $"Drink {TempRefs.CumType}");  //$"Gus's Cum/300/15/Drink {TempRefs.CumType}/Gus's Cum/A bottle of Gus's Cum./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
            //    if (ItemData.ContainsKey(TempRefs.MilkHarv)) ItemData[TempRefs.MilkHarv] = ItemData[TempRefs.MilkHarv].Replace("Milk -6", $"Drink {TempRefs.CumType}");  //$"Harvey's Cum/300/15/Drink {TempRefs.CumType}/Harvey's Cum /A bottle of Harvey's Cum./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
            //    if (ItemData.ContainsKey(TempRefs.MilkKent)) ItemData[TempRefs.MilkKent] = ItemData[TempRefs.MilkKent].Replace("Milk -6", $"Drink {TempRefs.CumType}");  //$"Kent's Cum/300/15/Drink {TempRefs.CumType}/Kent's Cum /A bottle of Kent's Cum./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
            //    if (ItemData.ContainsKey(TempRefs.MilkLewis)) ItemData[TempRefs.MilkLewis] = ItemData[TempRefs.MilkLewis].Replace("Milk -6", $"Drink {TempRefs.CumType}");  //$"Lewis's Cum/300/15/Drink {TempRefs.CumType}/Lewis's Cum/A bottle of Lewis's Cum./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
            //    if (ItemData.ContainsKey(TempRefs.MilkLinus)) ItemData[TempRefs.MilkLinus] = ItemData[TempRefs.MilkLinus].Replace("Milk -6", $"Drink {TempRefs.CumType}");  //$"Linus's Cum/300/15/Drink {TempRefs.CumType}/Linus's Cum/A bottle of Linus's Cum./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
            //    if (ItemData.ContainsKey(TempRefs.MilkMarlon)) ItemData[TempRefs.MilkMarlon] = ItemData[TempRefs.MilkMarlon].Replace("Milk -6", $"Drink {TempRefs.CumType}");  //$"Marlon's Cum/300/15/Drink {TempRefs.CumType}/Marlon's Cum /A bottle of Marlon's Cum./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
            //    if (ItemData.ContainsKey(TempRefs.MilkMorris)) ItemData[TempRefs.MilkMorris] = ItemData[TempRefs.MilkMorris].Replace("Milk -6", $"Drink {TempRefs.CumType}");  //$"Morris's Cum/300/15/Drink {TempRefs.CumType}/Morris's Cum /A bottle of Morris's Cum./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
            //    if (ItemData.ContainsKey(TempRefs.MilkPierre)) ItemData[TempRefs.MilkPierre] = ItemData[TempRefs.MilkPierre].Replace("Milk -6", $"Drink {TempRefs.CumType}");  //$"Pierre's Cum/300/15/Drink {TempRefs.CumType}/Pierre's Cum /A bottle of Pierre's Cum./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
            //    if (ItemData.ContainsKey(TempRefs.MilkSam)) ItemData[TempRefs.MilkSam] = ItemData[TempRefs.MilkSam].Replace("Milk -6", $"Drink {TempRefs.CumType}");  //$"Sam's Cum/300/15/Drink {TempRefs.CumType}/Sam's Cum/A bottle of Sam's Cum./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
            //    if (ItemData.ContainsKey(TempRefs.MilkSeb)) ItemData[TempRefs.MilkSeb] = ItemData[TempRefs.MilkSeb].Replace("Milk -6", $"Drink {TempRefs.CumType}");  //$"Sebastian's Cum/300/15/Drink {TempRefs.CumType}/Sebastian's Cum/A bottle of Sebastian's Cum./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
            //    if (ItemData.ContainsKey(TempRefs.MilkShane)) ItemData[TempRefs.MilkShane] = ItemData[TempRefs.MilkShane].Replace("Milk -6", $"Drink {TempRefs.CumType}");  //$"Shane's Cum/300/15/Drink {TempRefs.CumType}/Shane's Cum/A bottle of Shane's Cum./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
            //    if (ItemData.ContainsKey(TempRefs.MilkWilly)) ItemData[TempRefs.MilkWilly] = ItemData[TempRefs.MilkWilly].Replace("Milk -6", $"Drink {TempRefs.CumType}");//$"Willy's Cum/300/15/Drink {TempRefs.CumType}/Willy's Cum/A bottle of Willy's Cum./Drink/0 0 0 0 0 0 0 0 0 0 0/0";

            //    // Other mods
            //    if (ItemData.ContainsKey(TempRefs.MilkAndy)) ItemData[TempRefs.MilkAndy] = ItemData[TempRefs.MilkAndy].Replace("Milk -6", $"Drink {TempRefs.CumType}");  //$"Andy's Cum/50/15/Drink {TempRefs.CumType}/Andy's Cum/A bottle of Andy's Cum./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
            //    if (ItemData.ContainsKey(TempRefs.MilkVictor)) ItemData[TempRefs.MilkVictor] = ItemData[TempRefs.MilkVictor].Replace("Milk -6", $"Drink {TempRefs.CumType}");//$"Victor's Cum/50/15/Drink {TempRefs.CumType}/Victor's Cum/A bottle of Victor's Cum./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
            //    if (ItemData.ContainsKey(TempRefs.MilkMartin)) ItemData[TempRefs.MilkMartin] = ItemData[TempRefs.MilkMartin].Replace("Milk -6", $"Drink {TempRefs.CumType}");//$"Martin's Cum/50/15/Drink {TempRefs.CumType}/Martin's Cum/A bottle of Martin's Cum./Drink/0 0 0 0 0 0 0 0 0 0 0/0";

            //    // Special materials
            //    if (ItemData.ContainsKey(TempRefs.MilkDwarf)) ItemData[TempRefs.MilkDwarf] = ItemData[TempRefs.MilkDwarf].Replace("Milk -6", $"Drink {TempRefs.SpecialType}"); //$"Dwarf's Essence/300/15/Drink {TempRefs.SpecialType}/Dwarf's Essence/A bottle of Dwarf's Essence./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
            //    if (ItemData.ContainsKey(TempRefs.MilkKrobus)) ItemData[TempRefs.MilkKrobus] = ItemData[TempRefs.MilkKrobus].Replace("Milk -6", $"Drink {TempRefs.SpecialType}"); //$"Krobus's Essence/300/15/Drink {TempRefs.SpecialType}/Krobus's Essence/A bottle of Krobus's Essence./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
            //    if (ItemData.ContainsKey(TempRefs.MilkWiz)) ItemData[TempRefs.MilkWiz] = ItemData[TempRefs.MilkWiz].Replace("Milk -6", $"Drink {TempRefs.SpecialType}");  //$"Wizard's Essence/300/15/Drink {TempRefs.SpecialType}/Wizard's Essence/A bottle of Wizard's Essence./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
            //    if (ItemData.ContainsKey(TempRefs.MilkQi)) ItemData[TempRefs.MilkQi] = ItemData[TempRefs.MilkQi].Replace("Milk -6", $"Drink {TempRefs.SpecialType}");    //$"Mr. Qi's Essence/300/15/Drink {TempRefs.SpecialType}/Mr. Qi's Essence/A bottle of Mr. Qi's Essence./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
            //    if (ItemData.ContainsKey(TempRefs.MilkMagic)) ItemData[TempRefs.MilkMagic] = ItemData[TempRefs.MilkMagic].Replace("Milk -6", $"Drink {TempRefs.SpecialType}");  //$"Magical Essence/300/15/Drink {TempRefs.SpecialType}/Magical Essence/A bottle of Magical Essence./Drink/0 0 0 0 0 0 0 0 0 0 0/0";

            //    //recipe
            //    if (ItemData.ContainsKey(TempRefs.MilkSpecial)) ItemData[TempRefs.MilkSpecial] = ItemData[TempRefs.MilkSpecial].Replace("Milk -6", $"Drink {TempRefs.CumType}"); //$"Special Milk/50/15/Cooking {TempRefs.CumType}/Special Milk/A bottle of 'special' milk./drink/0 0 0 0 0 0 0 0 0 0 0/0";

            //}

            //Report(); // After change.
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="Name">Name of the item to get</param>
        /// <returns>In game int ID of the item</returns>
        public static string GetItemId(string Name)
        {
            if (ModItems.ContainsKey(Name))
            {
                return ModItems[Name].ToString();
            }

            return null;
        }

        public static void GetAllItemIDs(bool report = false)
        {
            if (ItemData == null)
                return;

            string[] stringSplit;

            foreach (KeyValuePair<string, ObjectData> kvp in ItemData)
            {
                if (kvp.Value == null) continue;

                try
                {
                    stringSplit = kvp.Value.Name.Split('/');

                    if (ModItems.ContainsKey(stringSplit[0]) && ItemData.ContainsKey(kvp.Key))
                    {
                        ModItems[stringSplit[0]] = new ObjectData() { Name = kvp.Key };
                    }
                }
                catch
                {
                    log.Log("Failed in GetAllItemIDs", LogLevel.Alert, Force: true);
                }
            }

            #region Items
            // Milk item code storage
            //TempRefs.MilkAbig = ModItems["Abigail's Milk"];
            //TempRefs.MilkEmil = ModItems["Emily's Milk"];
            //TempRefs.MilkHale = ModItems["Haley's Milk"];
            //TempRefs.MilkLeah = ModItems["Leah's Milk"];
            //TempRefs.MilkMaru = ModItems["Maru's Milk"];
            //TempRefs.MilkPenn = ModItems["Penny's Milk"];
            //TempRefs.MilkCaro = ModItems["Caroline's Milk"];
            //TempRefs.MilkJodi = ModItems["Jodi's Milk"];
            //TempRefs.MilkMarn = ModItems["Marnie's Milk"];
            //TempRefs.MilkRobi = ModItems["Robin's Milk"];
            //TempRefs.MilkPam = ModItems["Pam's Milk"];
            //TempRefs.MilkSand = ModItems["Sandy's Milk"];
            //TempRefs.MilkEvel = ModItems["Evelyn's Milk"];
            //TempRefs.MilkGeneric = ModItems["Woman's Milk"];

            // Cum item code storage.          
            //TempRefs.MilkAlex = ModItems["Alex's Cum"];
            //TempRefs.MilkClint = ModItems["Clint's Cum"];
            //TempRefs.MilkDemetrius = ModItems["Demetrius's Cum"];
            //TempRefs.MilkElliott = ModItems["Elliott's Cum"];
            //TempRefs.MilkGeorge = ModItems["George's Cum"];
            //TempRefs.MilkGil = ModItems["Gil's Cum"];
            //TempRefs.MilkGunther = ModItems["Gunther's Cum"];
            //TempRefs.MilkGus = ModItems["Gus's Cum"];
            //TempRefs.MilkHarv = ModItems["Harvey's Cum"];
            //TempRefs.MilkKent = ModItems["Kent's Cum"];
            //TempRefs.MilkLewis = ModItems["Lewis's Cum"];
            //TempRefs.MilkLinus = ModItems["Linus's Cum"];
            //TempRefs.MilkMarlon = ModItems["Marlon's Cum"];
            //TempRefs.MilkMorris = ModItems["Morris's Cum"];
            //TempRefs.MilkPierre = ModItems["Pierre's Cum"];
            //TempRefs.MilkSam = ModItems["Sam's Cum"];
            //TempRefs.MilkSeb = ModItems["Sebastian's Cum"];
            //TempRefs.MilkShane = ModItems["Shane's Cum"];
            //TempRefs.MilkWilly = ModItems["Willy's Cum"];
            //TempRefs.MilkSpecial = ModItems["Special Milk"];

            // Magical
            //TempRefs.MilkDwarf = ModItems["Dwarf's Essence"];
            //TempRefs.MilkKrobus = ModItems["Magical Essence"];
            //TempRefs.MilkQi = ModItems["Mr. Qi's Essence"];
            //TempRefs.MilkWiz = ModItems["Wizard's Essence"];
            //TempRefs.MilkMagic = ModItems["Magical Essence"];

            // Recipe item code storage
            //TempRefs.ProteinShake = ModItems["Protein Shake"];
            //TempRefs.MilkShake = ModItems["Special Milkshake"];
            //TempRefs.SuperJuice = ModItems["Super Juice"];
            //TempRefs.EldritchEnergy = ModItems["Eldritch Energy"];

            // Other mods
            //TempRefs.MilkSophia = ModItems["Sophia's Milk"];
            //TempRefs.MilkOlivia = ModItems["Olivia's Milk"];
            //TempRefs.MilkSusan = ModItems["Susan's Milk"];
            //TempRefs.MilkClaire = ModItems["Claire's Milk"];
            //TempRefs.MilkScarlett = TempRefs.MilkGeneric;
            //TempRefs.MilkAndy = ModItems["Andy's Cum"];
            //TempRefs.MilkVictor = ModItems["Victor's Cum"];
            //TempRefs.MilkMartin = ModItems["Martin's Cum"];

            // Quest items
            //TempRefs.PennyBook = ModItems["Boethia's Pillow Book"];
            //TempRefs.HaleyCamera = ModItems["Haley's Camera"];
            //TempRefs.HaleyPanties = ModItems["Haley's Panties"];
            //TempRefs.ReadiMilk = ModItems["Readi Milk"];
            //TempRefs.SweetSibling = ModItems["Sweet Sibling"];
            #endregion

        }

        public static void CorrectCategories()
        {
            foreach (KeyValuePair<string, ObjectData> kvp in ItemData)
            {

            }
        }

        public static bool CheckAll()
        {
            bool result = true;

            log.Log("CheckAll() does nothing now", LogLevel.Trace);

            #region Items
            // Milk item code storage
            //if (TempRefs.MilkAbig == 803) { result = false; log.Log($"MilkAbig is not set", LogLevel.Trace); }
            //if (TempRefs.MilkEmil == 803) { result = false; log.Log($"MilkEmil is not set", LogLevel.Trace); }
            //if (TempRefs.MilkHale == 803) { result = false; log.Log($"MilkHale is not set", LogLevel.Trace); }
            //if (TempRefs.MilkLeah == 803) { result = false; log.Log($"MilkLeah is not set", LogLevel.Trace); }
            //if (TempRefs.MilkMaru == 803) { result = false; log.Log($"MilkMaru is not set", LogLevel.Trace); }
            //if (TempRefs.MilkPenn == 803) { result = false; log.Log($"MilkPenn is not set", LogLevel.Trace); }
            //if (TempRefs.MilkCaro == 803) { result = false; log.Log($"MilkCaro is not set", LogLevel.Trace); }
            //if (TempRefs.MilkJodi == 803) { result = false; log.Log($"MilkJodi is not set", LogLevel.Trace); }
            //if (TempRefs.MilkMarn == 803) { result = false; log.Log($"MilkMarn is not set", LogLevel.Trace); }
            //if (TempRefs.MilkRobi == 803) { result = false; log.Log($"MilkRobi is not set", LogLevel.Trace); }
            //if (TempRefs.MilkPam == 803) { result = false; log.Log($"MilkPam is not set", LogLevel.Trace); }
            //if (TempRefs.MilkSand == 803) { result = false; log.Log($"MilkSand is not set", LogLevel.Trace); }
            //if (TempRefs.MilkEvel == 803) { result = false; log.Log($"MilkEvel is not set", LogLevel.Trace); }
            //if (TempRefs.MilkDwarf == 803) { result = false; log.Log($"MilkDwarf is not set", LogLevel.Trace); }
            //if (TempRefs.MilkGeneric == 803) { result = false; log.Log($"MilkGeneric is not set", LogLevel.Trace); }


            // Cum item code storage.
            //if (TempRefs.MilkSpecial == 803) { result = false; log.Log($"MilkSpecial is not set", LogLevel.Trace); }
            //if (TempRefs.MilkAlex == 803) { result = false; log.Log($"MilkAlex is not set", LogLevel.Trace); }
            //if (TempRefs.MilkClint == 803) { result = false; log.Log($"MilkClint is not set", LogLevel.Trace); }
            //if (TempRefs.MilkDemetrius == 803) { result = false; log.Log($"MilkDemetrius is not set", LogLevel.Trace); }
            //if (TempRefs.MilkElliott == 803) { result = false; log.Log($"MilkElliott is not set", LogLevel.Trace); }
            //if (TempRefs.MilkGeorge == 803) { result = false; log.Log($"MilkGeorge is not set", LogLevel.Trace); }
            //if (TempRefs.MilkGil == 803) { result = false; log.Log($"MilkGil is not set", LogLevel.Trace); }
            //if (TempRefs.MilkGunther == 803) { result = false; log.Log($"MilkGunther is not set", LogLevel.Trace); }
            //if (TempRefs.MilkGus == 803) { result = false; log.Log($"MilkGus is not set", LogLevel.Trace); }
            //if (TempRefs.MilkHarv == 803) { result = false; log.Log($"MilkHarv is not set", LogLevel.Trace); }
            //if (TempRefs.MilkKent == 803) { result = false; log.Log($"MilkKent is not set", LogLevel.Trace); }
            //if (TempRefs.MilkLewis == 803) { result = false; log.Log($"MilkLewis is not set", LogLevel.Trace); }
            //if (TempRefs.MilkLinus == 803) { result = false; log.Log($"MilkLinus is not set", LogLevel.Trace); }
            //if (TempRefs.MilkMarlon == 803) { result = false; log.Log($"MilkMarlon is not set", LogLevel.Trace); }
            //if (TempRefs.MilkMorris == 803) { result = false; log.Log($"MilkMorris is not set", LogLevel.Trace); }
            //if (TempRefs.MilkQi == 803) { result = false; log.Log($"MilkQi is not set", LogLevel.Trace); }
            //if (TempRefs.MilkPierre == 803) { result = false; log.Log($"MilkPierre is not set", LogLevel.Trace); }
            //if (TempRefs.MilkSam == 803) { result = false; log.Log($"MilkSam is not set", LogLevel.Trace); }
            //if (TempRefs.MilkSeb == 803) { result = false; log.Log($"MilkSeb is not set", LogLevel.Trace); }
            //if (TempRefs.MilkShane == 803) { result = false; log.Log($"MilkShane is not set", LogLevel.Trace); }
            //if (TempRefs.MilkWilly == 803) { result = false; log.Log($"MilkWilly is not set", LogLevel.Trace); }
            //if (TempRefs.MilkWiz == 803) { result = false; log.Log($"MilkWiz is not set", LogLevel.Trace); }
            ///if (TempRefs.MilkWMarlon == 803) { result = false; log.LogVerbose($"MilkWMarlon is not set", LogLevel.Trace); }
            //if (TempRefs.MilkKrobus == 803) { result = false; log.Log($"MilkKrobus is not set", LogLevel.Trace); }

            // Recipe item code storage
            //if (TempRefs.ProteinShake == 1240) { result = false; log.Log($"ProteinShake is not set", LogLevel.Trace); }
            //if (TempRefs.MilkShake == 1241) { result = false; log.Log($"MilkShake is not set", LogLevel.Trace); }
            //if (TempRefs.SuperJuice == 1249) { result = false; log.Log($"SuperJuice is not set", LogLevel.Trace); }

            //if (TempRefs.EldritchEnergy == 1241) { result = false; log.Log($"EldritchEnergy is not set", LogLevel.Trace); }
            //if (TempRefs.MartiniKairos == 1249) { result = false; log.Log($"MartiniKairos is not set", LogLevel.Trace); }
            //if (TempRefs.SweetSibling == 1249) { result = false; log.Log($"SweetSibling is not set", LogLevel.Trace); }

            // Other mods
            //if (TempRefs.MilkSophia == 803) { result = false; log.Log($"MilkSophia is not set", LogLevel.Trace); }
            //if (TempRefs.MilkOlivia == 803) { result = false; log.Log($"MilkOlivia is not set", LogLevel.Trace); }
            //if (TempRefs.MilkSusan == 803) { result = false; log.Log($"MilkSusan is not set", LogLevel.Trace); }
            //if (TempRefs.MilkClaire == 803) { result = false; log.Log($"MilkClaire is not set", LogLevel.Trace); }
            //if (TempRefs.MilkAndy == 803) { result = false; log.Log($"MilkAndy is not set", LogLevel.Trace); }
            //if (TempRefs.MilkVictor == 803) { result = false; log.Log($"MilkVictor is not set", LogLevel.Trace); }
            //if (TempRefs.MilkMartin == 803) { result = false; log.Log($"MilkMartin is not set", LogLevel.Trace); }
            #endregion

            // Item types
            //if (TempRefs.MilkType == -34) { result = false; log.Log($"MilkType hasn't changed", LogLevel.Trace); }
            //if (TempRefs.CumType == -35) { result = false; log.Log($"CumType hasn't changed", LogLevel.Trace); }
            //if (TempRefs.SpecialType == -36) { result = false; log.Log($"SpecialType hasn't changed", LogLevel.Trace); }

            // Quest items
            //if (TempRefs.PennyBook == 804) { result = false; log.Log($"Penny's book hasn't changed", LogLevel.Trace); }
            //if (TempRefs.HaleyCamera == 804) { result = false; log.Log($"Haley's Camera hasn't changed", LogLevel.Trace); }
            //if (TempRefs.HaleyPanties == 804) { result = false; log.Log($"Haley's Panties hasn't changed.", LogLevel.Trace); }
            //if (TempRefs.ReadiMilk == 804) { result = false; log.Log($"Readi Milk hasn't changed.", LogLevel.Trace); }

            return result;
        }
    }

    public static class ClothingEditor
    {
        private static IDictionary<string, ShirtData> ShirtData;
        private static IDictionary<string, PantsData> PantsData;

        public static bool CanEdit(IAssetName AssetName)
        {
            bool result = AssetName.IsEquivalentTo("Data/Shirts") || AssetName.IsEquivalentTo("Data/Pants");

            return result;
        }

        public static void Edit(IAssetData asset)
        {
            if (asset.Name.IsEquivalentTo("Data/Shirts") || asset.Name.IsEquivalentTo("Data/Pants"))
            {
                EditClothingData(asset);
            }
        }

        private static void EditClothingData(IAssetData asset)
        {
            if (asset.Name.IsEquivalentTo("Data/Shirts"))
            {
                ShirtData = (IDictionary<string, ShirtData>)asset.Data;
            }
            else if (asset.Name.IsEquivalentTo("Data/Pants"))
            {
                PantsData = (IDictionary<string, PantsData>)asset.Data;
            }

        }

        public static Clothing getShirt(string Name)
        {
            if (!ShirtData.ContainsKey($"{TempRefs.ModItemPrefix}{Name.Replace(" ", "_")}") || ShirtData.Values.Where(o => o.Name.Contains(Name)).ToList().Count < 1)
                return null;

            Clothing result = new Clothing($"{TempRefs.ModItemPrefix}{Name.Replace(" ", "_")}");
            return result;
        }
    }

    public static class ObjectEditor
    {
        static Dictionary<string, ObjectData> ItemData;

        public static bool CanEdit(IAssetName AssetName)
        {
            bool result = AssetName.IsEquivalentTo("Data/Objects");

            // if (result) log.LogVerbose("Asking to edit items");
            //if (!result && AssetName.Name.Contains("Object")) log.Log($"{AssetName.Name}", LogLevel.Info, Force: true);

            return result;
        }

        public static void Edit(IAssetData asset)
        {
            UpdateData(asset.Data as Dictionary<string, ObjectData>);

        }

        public static void UpdateData(Dictionary<string, ObjectData> assetdata)
        {
            ItemData = assetdata;
            UpdateData();
        }

        public static void UpdateData()
        {
            return;

            log.Log("Dumping items", LogLevel.Info, Force: true);
            foreach (KeyValuePair<string, ObjectData> kvp in ItemData)
            {
                switch (kvp.Key)
                {
                    case "Trunip190.CP.MilkTheVillagers.Abigail's_Milk": /*kvp.Value.Category = -34; */break;
                    case "Trunip190.CP.MilkTheVillagers.Caroline's_Milk": /*kvp.Value.Category = -34; */break;
                    case "Trunip190.CP.MilkTheVillagers.Claire's_Milk": /*kvp.Value.Category = -34; */break;
                    case "Trunip190.CP.MilkTheVillagers.Emily's_Milk": /*kvp.Value.Category = -34; */break;
                    case "Trunip190.CP.MilkTheVillagers.Evelyn's_Milk": /*kvp.Value.Category = -34; */break;
                    case "Trunip190.CP.MilkTheVillagers.Haley's_Milk": /*kvp.Value.Category = -34; */break;
                    case "Trunip190.CP.MilkTheVillagers.Jodi's_Milk": /*kvp.Value.Category = -34; */break;
                    case "Trunip190.CP.MilkTheVillagers.Leah's_Milk": /*kvp.Value.Category = -34; */break;
                    case "Trunip190.CP.MilkTheVillagers.Marnie's_Milk": /*kvp.Value.Category = -34; */break;
                    case "Trunip190.CP.MilkTheVillagers.Maru's_Milk": /*kvp.Value.Category = -34; */break;
                    case "Trunip190.CP.MilkTheVillagers.Olivia's_Milk": /*kvp.Value.Category = -34; */break;
                    case "Trunip190.CP.MilkTheVillagers.Pam's_Milk": /*kvp.Value.Category = -34; */break;
                    case "Trunip190.CP.MilkTheVillagers.Penny's_Milk": /*kvp.Value.Category = -34; */break;
                    case "Trunip190.CP.MilkTheVillagers.Readi_Milk": /*kvp.Value.Category = -34;*/ break;
                    case "Trunip190.CP.MilkTheVillagers.Robin's_Milk": /*kvp.Value.Category = -34; */break;
                    case "Trunip190.CP.MilkTheVillagers.Sandy's_Milk": /*kvp.Value.Category = -34; */break;
                    case "Trunip190.CP.MilkTheVillagers.Sophia's_Milk": /*kvp.Value.Category = -34; */break;
                    case "Trunip190.CP.MilkTheVillagers.Susan's_Milk": /*kvp.Value.Category = -34; */break;
                    case "Trunip190.CP.MilkTheVillagers.Woman's_Milk": /*kvp.Value.Category = -34; */break;

                    case "Trunip190.CP.MilkTheVillagers.Alex's_Cum": /*kvp.Value.Category = -35; */break;
                    case "Trunip190.CP.MilkTheVillagers.Andy's_Cum": /*kvp.Value.Category = -35; */break;
                    case "Trunip190.CP.MilkTheVillagers.Clint's_Cum": /*kvp.Value.Category = -35; */break;
                    case "Trunip190.CP.MilkTheVillagers.Demetrius's_Cum": /*kvp.Value.Category = -35; */break;
                    case "Trunip190.CP.MilkTheVillagers.Elliott's_Cum": /*kvp.Value.Category = -35; */break;
                    case "Trunip190.CP.MilkTheVillagers.George's_Cum": /*kvp.Value.Category = -35; */break;
                    case "Trunip190.CP.MilkTheVillagers.Gil's_Cum": /*kvp.Value.Category = -35; */break;
                    case "Trunip190.CP.MilkTheVillagers.Gunther's_Cum": /*kvp.Value.Category = -35; */break;
                    case "Trunip190.CP.MilkTheVillagers.Gus's_Cum": /*kvp.Value.Category = -35; */break;
                    case "Trunip190.CP.MilkTheVillagers.Harvey's_Cum": /*kvp.Value.Category = -35; */break;
                    case "Trunip190.CP.MilkTheVillagers.Kent's_Cum": /*kvp.Value.Category = -35; */break;
                    case "Trunip190.CP.MilkTheVillagers.Lance's_Cum": /*kvp.Value.Category = -35; */break;
                    case "Trunip190.CP.MilkTheVillagers.Lewis's_Cum": /*kvp.Value.Category = -35; */break;
                    case "Trunip190.CP.MilkTheVillagers.Linus's_Cum": /*kvp.Value.Category = -35; */break;
                    case "Trunip190.CP.MilkTheVillagers.Marlon's_Cum": /*kvp.Value.Category = -35; */break;
                    case "Trunip190.CP.MilkTheVillagers.Martin's_Cum": /*kvp.Value.Category = -35; */break;
                    case "Trunip190.CP.MilkTheVillagers.Morris's_Cum": /*kvp.Value.Category = -35; */break;
                    case "Trunip190.CP.MilkTheVillagers.Pierre's_Cum": /*kvp.Value.Category = -35; */break;
                    case "Trunip190.CP.MilkTheVillagers.Sam's_Cum": /*kvp.Value.Category = -35; */break;
                    case "Trunip190.CP.MilkTheVillagers.Sebastian's_Cum": /*kvp.Value.Category = -35; */break;
                    case "Trunip190.CP.MilkTheVillagers.Shane's_Cum": /*kvp.Value.Category = -35; */break;
                    case "Trunip190.CP.MilkTheVillagers.Special Milk": /*kvp.Value.Category = -35; */break;
                    case "Trunip190.CP.MilkTheVillagers.Victor's_Cum": /*kvp.Value.Category = -35; */break;
                    case "Trunip190.CP.MilkTheVillagers.Willy's_Cum": /*kvp.Value.Category = -35; */break;

                    case "Trunip190.CP.MilkTheVillagers.Dwarf's_Essence":/* kvp.Value.Category = -36;*/ break;
                    case "Trunip190.CP.MilkTheVillagers.Krobus_Essence": /*kvp.Value.Category = -36; */break;
                    case "Trunip190.CP.MilkTheVillagers.Magical_Essence": /*kvp.Value.Category = -36; */break;
                    case "Trunip190.CP.MilkTheVillagers.Wizard's_Essence": /*kvp.Value.Category = -36; */break;

                    //case "Special Milkshake": kvp.Value.Category = -36; break;
                    //case "Protein Shake": kvp.Value.Category = -36; break;
                    //case "Super Juice": kvp.Value.Category = -36; break;
                    //case "Creamy Coffee": kvp.Value.Category = -36; break;
                    //case "Sweet Sibling": kvp.Value.Category = -36; break;
                    //case "Eldritch Energy": kvp.Value.Category = -36; break;
                    //case "Martini Kairos": kvp.Value.Category = -36; break;
                    //case "Mr. Qi's Essence": kvp.Value.Category = -36; break;

                    default:
                        if (kvp.Key.Contains("Trunip190") || kvp.Value.Name.Contains("Milk")) log.Log(kvp.Value.Name, LogLevel.Trace); break;
                }
            }
            //SetItems();
        }
    }

    public static class NPCGiftTastesEditor
    {
        private static IDictionary<string, string> data;
        public static List<string> Villagers = new List<string>();


        public static bool CanEdit(IAssetName AssetName)
        {
            return AssetName.IsEquivalentTo("Data/NPCGiftTastes");
        }

        public static void Edit(IAssetData asset)
        {
            EditAsset(asset);
        }

        private static void EditAsset(IAssetData asset) { }

        public static void UpdateData(Dictionary<string, string> assetdata)
        {
            data = assetdata;

            string[] banned = new string[] {
                "Universal_Love",
                "Universal_Like",
                "Universal_Neutral",
                "Universal_Dislike",
                "Universal_Hate"
            };

            foreach (KeyValuePair<string, string> kvp in data)
                if (!Villagers.Contains(kvp.Key))
                {
                    if (!banned.Contains(kvp.Key))
                        Villagers.Add(kvp.Key);
                }

        }

    }
}
