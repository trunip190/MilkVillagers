using System.Collections.Generic;
using StardewValley;
using StardewModdingAPI;
using Microsoft.Xna.Framework.Graphics;
using System;
using SObject = StardewValley.Object;
using SFarmer = StardewValley.Farmer;
using Microsoft.Xna.Framework;
using PyTK;
using PyTK.Extensions;
using PyTK.CustomElementHandler;

namespace MilkVillagers.Asset_Editors
{
    public static class ItemEditor //: IAssetEditor
    {
        private static IDictionary<int, string> ItemData;
        private static IDictionary<string, string> ContextData;
        private static Dictionary<string, int> ModItems = new Dictionary<string, int>()
        {
            ["Abigail's Milk"] = 1,
            ["Emily's Milk"] = 2,
            ["Haley's Milk"] = 3,
            ["Leah's Milk"] = 4,
            ["Maru's Milk"] = 5,
            ["Penny's Milk"] = 6,
            ["Caroline's Milk"] = 7,
            ["Jodi's Milk"] = 8,
            ["Marnie's Milk"] = 9,
            ["Robin's Milk"] = 10,
            ["Pam's Milk"] = 11,
            ["Sandy's Milk"] = 12,
            ["Evelyn's Milk"] = 13,

            // Other mods	
            ["Sophia's Milk"] = 14,
            ["Olivia's Milk"] = 15,
            ["Susan's Milk"] = 16,
            ["Claire's Milk"] = 17,

            //recipe	
            ["Woman's Milk"] = 20,
            ["Special Milkshake"] = 21,

            //cum items	
            ["Alex's Cum"] = 22,
            ["Clint's Cum"] = 23,
            ["Demetrius's Cum"] = 24,
            ["Elliott's Cum"] = 25,
            ["George's Cum"] = 26,
            ["Gil's Cum"] = 27,
            ["Gunther's Cum"] = 28,
            ["Gus's Cum"] = 29,
            ["Harvey's Cum"] = 30,
            ["Kent's Cum"] = 31,
            ["Lewis's Cum"] = 32,
            ["Linus's Cum"] = 33,
            ["Marlon's Cum"] = 34,
            ["Morris's Cum"] = 35,
            ["Pierre's Cum"] = 36,
            ["Sam's Cum"] = 37,
            ["Sebastian's Cum"] = 38,
            ["Shane's Cum"] = 39,
            ["Willy's Cum"] = 40,

            // Other mods	
            ["Andy's Cum"] = 41,
            ["Victor's Cum"] = 42,
            ["Martin's Cum"] = 43,

            // Special materials	
            ["Dwarf's Essence"] = 44,
            ["Krobus's Essence"] = 45,
            ["Wizard's Essence"] = 46,
            ["Mr. Qi's Essence"] = 47,
            ["Magical Essence"] = 48,

            //recipe	
            ["Special Milk"] = 49,
            ["Protein Shake"] = 50,
            ["Super Juice"] = 51,
            ["Eldritch Energy"] = 52,
            ["Boethia's Pillow Book"] = 53,
            ["Haley's Camera"] = 54,
            ["Haley's Panties"] = 55,
            ["Readi Milk"] = 56,
        };
        public static bool Initialised => ItemData != null;

        public static IDictionary<int, string> Report()
        {
            ModFunctions.LogVerbose("ItemEditor.Report()");
            return ItemData;
        }

        public static bool CanEdit<T>(IAssetInfo asset)
        {
            return (asset.Name.IsEquivalentTo("Data/ObjectInformation") || asset.Name.IsEquivalentTo("Data/ObjectContextTags"));
        }

        public static bool CanEdit(IAssetName AssetName)
        {
            bool result = AssetName.IsEquivalentTo("Data/ObjectInformation") || AssetName.IsEquivalentTo("Data/ObjectContextTags");

            if (result)
                ModFunctions.LogVerbose("Asking to edit items");

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
            ModFunctions.LogVerbose("Adding in items");

            if (asset.Name.IsEquivalentTo("Data/ObjectInformation"))
            {
                ItemData = asset.AsDictionary<int, string>().Data;

                if (!TempRefs.loaded)
                {
                    ModFunctions.LogVerbose("TempRefs not loaded.");
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

        private static void ReportContextData()
        {
            ModFunctions.LogVerbose("Dumping context data");
            foreach (KeyValuePair<string, string> kvp in ContextData)
            {
                ModFunctions.LogVerbose($"{kvp.Key} - {kvp.Value}");
            }
        }

        private static void EditContextData()
        {
            ContextData["Abigail's Milk"] = "breast_milk_item";
            ContextData["Caroline's Milk"] = "breast_milk_item";
            ContextData["Claire's Milk"] = "breast_milk_item";
            ContextData["Emily's Milk"] = "breast_milk_item";
            ContextData["Evelyn's Milk"] = "breast_milk_item";
            ContextData["Haley's Milk"] = "breast_milk_item";
            ContextData["Jodi's Milk"] = "breast_milk_item";
            ContextData["Leah's Milk"] = "breast_milk_item";
            ContextData["Marnie's Milk"] = "breast_milk_item";
            ContextData["Maru's Milk"] = "breast_milk_item";
            ContextData["Olivia's Milk"] = "breast_milk_item";
            ContextData["Pam's Milk"] = "breast_milk_item";
            ContextData["Penny's Milk"] = "breast_milk_item";
            ContextData["Robin's Milk"] = "breast_milk_item";
            ContextData["Sandy's Milk"] = "breast_milk_item";
            ContextData["Sophia's Milk"] = "breast_milk_item";
            ContextData["Susan's Milk"] = "breast_milk_item";
            ContextData["Woman's Milk"] = "breast_milk_item";

            ContextData["Alex's Cum"] = "human_cum_item";
            ContextData["Andy's Cum"] = "human_cum_item";
            ContextData["Clint's Cum"] = "human_cum_item";
            ContextData["Demetrius's Cum"] = "human_cum_item";
            ContextData["Elliott's Cum"] = "human_cum_item";
            ContextData["George's Cum"] = "human_cum_item";
            ContextData["Gil's Cum"] = "human_cum_item";
            ContextData["Gunther's Cum"] = "human_cum_item";
            ContextData["Gus's Cum"] = "human_cum_item";
            ContextData["Harvey's Cum"] = "human_cum_item";
            ContextData["Kent's Cum"] = "human_cum_item";
            ContextData["Lewis's Cum"] = "human_cum_item";
            ContextData["Linus's Cum"] = "human_cum_item";
            ContextData["Marlon's Cum"] = "human_cum_item";
            ContextData["Martin's Cum"] = "human_cum_item";
            ContextData["Morris's Cum"] = "human_cum_item";
            ContextData["Pierre's Cum"] = "human_cum_item";
            ContextData["Sam's Cum"] = "human_cum_item";
            ContextData["Sebastian's Cum"] = "human_cum_item";
            ContextData["Shane's Cum"] = "human_cum_item";
            ContextData["Victor's Cum"] = "human_cum_item";
            ContextData["Willy's Cum"] = "human_cum_item";
            ContextData["Special Milk"] = "human_cum_item";

            ContextData["Wizard's Cum"] = "human_cum_item,magic_essence_item";
            ContextData["Dwarf's Milk"] = "breast_milk_item,magic_essence_item";
            ContextData["Mr.Qi's Essence"] = "magic_essence_item";
            ContextData["Magical Essence"] = "magic_essence_item";

            ContextData["Eldritch Energy"] = "cooking_item";
            ContextData["Protein Shake"] = "cooking_item";
            ContextData["Special Milkshake"] = "cooking_item";
            ContextData["Super Juice"] = "cooking_item";

            ContextData["Boethia's Pillow Book"] = "quest_item";
        }

        public static void RemoveInvalid(bool Male, bool Female)
        {
            if (!Female)
            {
                //milk items
                ItemData.Remove(TempRefs.MilkAbig);
                ItemData.Remove(TempRefs.MilkEmil);
                ItemData.Remove(TempRefs.MilkHale);
                ItemData.Remove(TempRefs.MilkLeah);
                ItemData.Remove(TempRefs.MilkMaru);
                ItemData.Remove(TempRefs.MilkPenn);
                ItemData.Remove(TempRefs.MilkCaro);
                ItemData.Remove(TempRefs.MilkJodi);
                ItemData.Remove(TempRefs.MilkMarn);
                ItemData.Remove(TempRefs.MilkRobi);
                ItemData.Remove(TempRefs.MilkPam);
                ItemData.Remove(TempRefs.MilkSand);
                ItemData.Remove(TempRefs.MilkEvel);
                // Other mods
                ItemData.Remove(TempRefs.MilkSophia);
                ItemData.Remove(TempRefs.MilkOlivia);
                ItemData.Remove(TempRefs.MilkSusan);
                ItemData.Remove(TempRefs.MilkClaire);

                // Recipes
                ItemData.Remove(TempRefs.MilkGeneric);
                ItemData.Remove(TempRefs.MilkShake);

            }

            if (!Male)
            {
                //cum items
                ItemData.Remove(TempRefs.MilkAlex);
                ItemData.Remove(TempRefs.MilkClint);
                ItemData.Remove(TempRefs.MilkDemetrius);
                ItemData.Remove(TempRefs.MilkElliott);
                ItemData.Remove(TempRefs.MilkGeorge);
                ItemData.Remove(TempRefs.MilkGil);
                ItemData.Remove(TempRefs.MilkGunther);
                ItemData.Remove(TempRefs.MilkGus);
                ItemData.Remove(TempRefs.MilkHarv);
                ItemData.Remove(TempRefs.MilkKent);
                ItemData.Remove(TempRefs.MilkLewis);
                ItemData.Remove(TempRefs.MilkLinus);
                ItemData.Remove(TempRefs.MilkMarlon);
                ItemData.Remove(TempRefs.MilkMorris);
                ItemData.Remove(TempRefs.MilkPierre);
                ItemData.Remove(TempRefs.MilkSam);
                ItemData.Remove(TempRefs.MilkSeb);
                ItemData.Remove(TempRefs.MilkShane);
                ItemData.Remove(TempRefs.MilkWilly);
                // Other mods
                ItemData.Remove(TempRefs.MilkAndy);
                ItemData.Remove(TempRefs.MilkVictor);
                ItemData.Remove(TempRefs.MilkMartin);

                // Special materials
                ItemData.Remove(TempRefs.MilkWiz);
                ItemData.Remove(TempRefs.MilkQi);

                // Recipes.
                ItemData.Remove(TempRefs.MilkSpecial);
                ItemData.Remove(TempRefs.ProteinShake);

            }

            if (!(Male || Female))
                ItemData.Remove(TempRefs.SuperJuice);

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
                ItemData[TempRefs.MilkAbig] = $"Abigail's Milk/300/30/Drink {TempRefs.MilkType}/Abigail's Milk/A jug of Abigail's milk./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
                ItemData[TempRefs.MilkEmil] = $"Emily's Milk/300/30/Drink {TempRefs.MilkType}/Emily's Milk/A jug of Emily's milk./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
                ItemData[TempRefs.MilkHale] = $"Haley's Milk/300/30/Drink {TempRefs.MilkType}/Haley's Milk/A jug of Haley's milk./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
                ItemData[TempRefs.MilkLeah] = $"Leah's Milk/300/30/Drink {TempRefs.MilkType}/Leah's Milk/A jug of Leah's milk./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
                ItemData[TempRefs.MilkMaru] = $"Maru's Milk/300/30/Drink {TempRefs.MilkType}/Maru's Milk/A jug of Maru's milk./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
                ItemData[TempRefs.MilkPenn] = $"Penny's Milk/300/30/Drink {TempRefs.MilkType}/Penny's Milk/A jug of Penny's milk./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
                ItemData[TempRefs.MilkCaro] = $"Caroline's Milk/300/30/Drink {TempRefs.MilkType}/Caroline's Milk/A jug of Caroline's milk./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
                ItemData[TempRefs.MilkJodi] = $"Jodi's Milk/200/20/Drink {TempRefs.MilkType}/Jodi's Milk/A jug of Jodi's milk./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
                ItemData[TempRefs.MilkMarn] = $"Marnie's Milk/200/20/Drink {TempRefs.MilkType}/Marnie's Milk/A jug of Marnie's milk./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
                ItemData[TempRefs.MilkRobi] = $"Robin's Milk/200/20/Drink {TempRefs.MilkType}/Robin's Milk/A jug of Robin's milk./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
                ItemData[TempRefs.MilkPam] = $"Pam's Milk/90/10/Drink {TempRefs.MilkType}/Pam's Milk/A jug of Pam's milk./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
                ItemData[TempRefs.MilkSand] = $"Sandy's Milk/200/20/Drink {TempRefs.MilkType}/Sandy's Milk/A jug of Sandy's milk./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
                ItemData[TempRefs.MilkEvel] = $"Evelyn's Milk/90/10/Drink {TempRefs.MilkType}/Evelyn's Milk/A jug of Evelyn's milk./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
                // Other mods
                ItemData[TempRefs.MilkSophia] = $"Sophia's Milk/300/30/Drink {TempRefs.MilkType}/Sophia's Milk/A jug of Sophia's milk./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
                ItemData[TempRefs.MilkOlivia] = $"Olivia's Milk/200/20/Drink {TempRefs.MilkType}/Olivia's Milk/A jug of Olivia's milk./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
                ItemData[TempRefs.MilkSusan] = $"Susan's Milk/300/30/Drink {TempRefs.MilkType}/Susan's Milk/A jug of Susan 's milk./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
                ItemData[TempRefs.MilkClaire] = $"Claire's Milk/200/20/Drink {TempRefs.MilkType}/Claire's Milk/A jug of Claire's milk./Drink/0 0 0 0 0 0 0 0 0 0 0/0";

                //recipe
                ItemData[TempRefs.MilkGeneric] = $"Breast Milk/50/15/Cooking {TempRefs.MilkType}/Breast Milk/A jug of breast milk./drink/0 0 0 0 0 0 0 0 0 0 0/0";
                ItemData[TempRefs.MilkShake] = $"Milkshake/50/15/Cooking -7/Special' Milkshake/Extra milky milkshake./drink/0 0 0 0 0 0 0 25 0 1 0/343";

            }

            if (Male)
            {
                //cum items
                ItemData[TempRefs.MilkAlex]         = $"Alex's Cum/300/15/Drink {TempRefs.CumType}/Alex's Cum /A bottle of Alex's Cum./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
                ItemData[TempRefs.MilkClint]        = $"Clint's Cum/300/15/Drink {TempRefs.CumType}/Clint's Cum/A bottle of Clint's Cum./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
                ItemData[TempRefs.MilkDemetrius]    = $"Demetrius's Cum/300/15/Drink {TempRefs.CumType}/Demetrius's Cum/A bottle of Demetrius's Cum./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
                ItemData[TempRefs.MilkElliott]      = $"Elliott's Cum/300/15/Drink {TempRefs.CumType}/Elliott's Cum/A bottle of Elliott's Cum./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
                ItemData[TempRefs.MilkGeorge]       = $"George's Cum/300/15/Drink {TempRefs.CumType}/George's Cum /A bottle of George's Cum./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
                ItemData[TempRefs.MilkGil]          = $"Gil's Cum/300/15/Drink {TempRefs.CumType}/Gil's Cum/A bottle of Gil's Cum./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
                ItemData[TempRefs.MilkGunther]      = $"Gunther's Cum/300/15/Drink {TempRefs.CumType}/Gunther's Cum/A bottle of Gunther's Cum./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
                ItemData[TempRefs.MilkGus]          = $"Gus's Cum/300/15/Drink {TempRefs.CumType}/Gus's Cum/A bottle of Gus's Cum./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
                ItemData[TempRefs.MilkHarv]         = $"Harvey's Cum/300/15/Drink {TempRefs.CumType}/Harvey's Cum /A bottle of Harvey's Cum./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
                ItemData[TempRefs.MilkKent]         = $"Kent's Cum/300/15/Drink {TempRefs.CumType}/Kent's Cum /A bottle of Kent's Cum./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
                ItemData[TempRefs.MilkLewis]        = $"Lewis's Cum/300/15/Drink {TempRefs.CumType}/Lewis's Cum/A bottle of Lewis's Cum./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
                ItemData[TempRefs.MilkLinus]        = $"Linus's Cum/300/15/Drink {TempRefs.CumType}/Linus's Cum/A bottle of Linus's Cum./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
                ItemData[TempRefs.MilkMarlon]       = $"Marlon's Cum/300/15/Drink {TempRefs.CumType}/Marlon's Cum /A bottle of Marlon's Cum./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
                ItemData[TempRefs.MilkMorris]       = $"Morris's Cum/300/15/Drink {TempRefs.CumType}/Morris's Cum /A bottle of Morris's Cum./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
                ItemData[TempRefs.MilkPierre]       = $"Pierre's Cum/300/15/Drink {TempRefs.CumType}/Pierre's Cum /A bottle of Pierre's Cum./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
                ItemData[TempRefs.MilkSam]          = $"Sam's Cum/300/15/Drink {TempRefs.CumType}/Sam's Cum/A bottle of Sam's Cum./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
                ItemData[TempRefs.MilkSeb]          = $"Sebastian's Cum/300/15/Drink {TempRefs.CumType}/Sebastian's Cum/A bottle of Sebastian's Cum./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
                ItemData[TempRefs.MilkShane]        = $"Shane's Cum/300/15/Drink {TempRefs.CumType}/Shane's Cum/A bottle of Shane's Cum./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
                ItemData[TempRefs.MilkWilly]        = $"Willy's Cum/300/15/Drink {TempRefs.CumType}/Willy's Cum/A bottle of Willy's Cum./Drink/0 0 0 0 0 0 0 0 0 0 0/0";

                // Other mods
                ItemData[TempRefs.MilkAndy]         = $"Andy's Cum/50/15/Drink {TempRefs.CumType}/Andy's Cum/A bottle of Andy's Cum./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
                ItemData[TempRefs.MilkVictor]       = $"Victor's Cum/50/15/Drink {TempRefs.CumType}/Victor's Cum/A bottle of Victor's Cum./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
                ItemData[TempRefs.MilkMartin]       = $"Martin's Cum/50/15/Drink {TempRefs.CumType}/Martin's Cum/A bottle of Martin's Cum./Drink/0 0 0 0 0 0 0 0 0 0 0/0";

                // Special materials
                ItemData[TempRefs.MilkDwarf]        = $"Dwarf's Essence/300/15/Drink {TempRefs.SpecialType}/Dwarf's Essence/A bottle of Dwarf's Essence./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
                ItemData[TempRefs.MilkKrobus]       = $"Krobus's Essence/300/15/Drink {TempRefs.SpecialType}/Krobus's Essence/A bottle of Krobus's Essence./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
                ItemData[TempRefs.MilkWiz]          = $"Wizard's Essence/300/15/Drink {TempRefs.SpecialType}/Wizard's Essence/A bottle of Wizard's Essence./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
                ItemData[TempRefs.MilkQi]           = $"Mr. Qi's Essence/300/15/Drink {TempRefs.SpecialType}/Mr. Qi's Essence/A bottle of Mr. Qi's Essence./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
                ItemData[TempRefs.MilkMagic]        = $"Magical Essence/300/15/Drink {TempRefs.SpecialType}/Magical Essence/A bottle of Magical Essence./Drink/0 0 0 0 0 0 0 0 0 0 0/0";

                //recipe
                ItemData[TempRefs.MilkSpecial]      = $"Special Milk/50/15/Cooking {TempRefs.CumType}/Special Milk/A bottle of 'special' milk./drink/0 0 0 0 0 0 0 0 0 0 0/0";
                ItemData[TempRefs.ProteinShake]     = $"Protein shake/50/15/Cooking -7/Protein' shake/Shake made with extra protein/drink/0 0 0 0 0 0 0 25 0 0 2/343";

            }

            if (Male && Female) ItemData[TempRefs.SuperJuice] = $"Super Juice/150/125/Cooking -7/Super Juice/The perfect fusion of male and female juices./Drink/0 0 0 0 2 0 0 25 0 3 2/700";

            //ItemData[TempRefs.PennyBook] = $"Boethia's Pillow Book/1/1/Junk/Boethia's Pillow Book/An erotic book about various sexual positions./";
        }

        public static string GetItemName(int id)
        {
            return ItemData[id];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Name">Name of the item to get</param>
        /// <returns>In game int ID of the item</returns>
        public static int GetItemId(string Name)
        {
            if (ModItems.ContainsKey(Name))
            {
                return ModItems[Name];
            }

            return -1;
        }

        public static void GetAllItemIDs()
        {
            if (ItemData == null)
                return;

            string[] stringSplit;

            foreach (KeyValuePair<int, string> kvp in ItemData)
            {
                stringSplit = kvp.Value.Split('/');

                if (ModItems.ContainsKey(stringSplit[0]))
                {
                    ModItems[stringSplit[0]] = kvp.Key;
                    ModFunctions.LogVerbose($"{stringSplit[0]} = {kvp.Key}");
                }
                else
                {
                    //ModFunctions.LogVerbose($"{stringSplit[0]} not found.");
                }
            }

            #region Items
            // Milk item code storage
            TempRefs.MilkAbig = ModItems["Abigail's Milk"];
            TempRefs.MilkEmil = ModItems["Emily's Milk"];
            TempRefs.MilkHale = ModItems["Haley's Milk"];
            TempRefs.MilkLeah = ModItems["Leah's Milk"];
            TempRefs.MilkMaru = ModItems["Maru's Milk"];
            TempRefs.MilkPenn = ModItems["Penny's Milk"];
            TempRefs.MilkCaro = ModItems["Caroline's Milk"];
            TempRefs.MilkJodi = ModItems["Jodi's Milk"];
            TempRefs.MilkMarn = ModItems["Marnie's Milk"];
            TempRefs.MilkRobi = ModItems["Robin's Milk"];
            TempRefs.MilkPam = ModItems["Pam's Milk"];
            TempRefs.MilkSand = ModItems["Sandy's Milk"];
            TempRefs.MilkEvel = ModItems["Evelyn's Milk"];
            TempRefs.MilkGeneric = ModItems["Woman's Milk"];

            // Cum item code storage.          
            TempRefs.MilkAlex = ModItems["Alex's Cum"];
            TempRefs.MilkClint = ModItems["Clint's Cum"];
            TempRefs.MilkDemetrius = ModItems["Demetrius's Cum"];
            TempRefs.MilkElliott = ModItems["Elliott's Cum"];
            TempRefs.MilkGeorge = ModItems["George's Cum"];
            TempRefs.MilkGil = ModItems["Gil's Cum"];
            TempRefs.MilkGunther = ModItems["Gunther's Cum"];
            TempRefs.MilkGus = ModItems["Gus's Cum"];
            TempRefs.MilkHarv = ModItems["Harvey's Cum"];
            TempRefs.MilkKent = ModItems["Kent's Cum"];
            TempRefs.MilkLewis = ModItems["Lewis's Cum"];
            TempRefs.MilkLinus = ModItems["Linus's Cum"];
            TempRefs.MilkMarlon = ModItems["Marlon's Cum"];
            TempRefs.MilkMorris = ModItems["Morris's Cum"];
            TempRefs.MilkPierre = ModItems["Pierre's Cum"];
            TempRefs.MilkSam = ModItems["Sam's Cum"];
            TempRefs.MilkSeb = ModItems["Sebastian's Cum"];
            TempRefs.MilkShane = ModItems["Shane's Cum"];
            TempRefs.MilkWilly = ModItems["Willy's Cum"];
            TempRefs.MilkSpecial = ModItems["Special Milk"];

            // Magical
            TempRefs.MilkDwarf = ModItems["Dwarf's Essence"];
            TempRefs.MilkKrobus = ModItems["Krobus's Essence"];
            TempRefs.MilkQi = ModItems["Mr. Qi's Essence"];
            TempRefs.MilkWiz = ModItems["Wizard's Essence"];
            TempRefs.MilkMagic = ModItems["Magical Essence"];

            // Recipe item code storage
            TempRefs.ProteinShake = ModItems["Protein Shake"];
            TempRefs.MilkShake = ModItems["Special Milkshake"];
            TempRefs.SuperJuice = ModItems["Super Juice"];
            TempRefs.EldritchEnergy = ModItems["Eldritch Energy"]; // TODO need to make this item.

            // Other mods
            TempRefs.MilkSophia = ModItems["Sophia's Milk"];
            TempRefs.MilkOlivia = ModItems["Olivia's Milk"];
            TempRefs.MilkSusan = ModItems["Susan's Milk"];
            TempRefs.MilkClaire = ModItems["Claire's Milk"];
            TempRefs.MilkScarlett = TempRefs.MilkGeneric; //TODO need to make this item.
            TempRefs.MilkAndy = ModItems["Andy's Cum"];
            TempRefs.MilkVictor = ModItems["Victor's Cum"];
            TempRefs.MilkMartin = ModItems["Martin's Cum"];

            // Quest items
            TempRefs.PennyBook = ModItems["Boethia's Pillow Book"];
            TempRefs.HaleyCamera = ModItems["Haley's Camera"];
            TempRefs.HaleyPanties = ModItems["Haley's Panties"];
            TempRefs.ReadiMilk = ModItems["Readi Milk"];
            #endregion

        }

        public static bool CheckAll()
        {
            bool result = true;

            #region Items
            // Milk item code storage
            if (TempRefs.MilkAbig == 803) { result = false; ModFunctions.LogVerbose($"MilkAbig is not set", LogLevel.Trace); }
            if (TempRefs.MilkEmil == 803) { result = false; ModFunctions.LogVerbose($"MilkEmil is not set", LogLevel.Trace); }
            if (TempRefs.MilkHale == 803) { result = false; ModFunctions.LogVerbose($"MilkHale is not set", LogLevel.Trace); }
            if (TempRefs.MilkLeah == 803) { result = false; ModFunctions.LogVerbose($"MilkLeah is not set", LogLevel.Trace); }
            if (TempRefs.MilkMaru == 803) { result = false; ModFunctions.LogVerbose($"MilkMaru is not set", LogLevel.Trace); }
            if (TempRefs.MilkPenn == 803) { result = false; ModFunctions.LogVerbose($"MilkPenn is not set", LogLevel.Trace); }
            if (TempRefs.MilkCaro == 803) { result = false; ModFunctions.LogVerbose($"MilkCaro is not set", LogLevel.Trace); }
            if (TempRefs.MilkJodi == 803) { result = false; ModFunctions.LogVerbose($"MilkJodi is not set", LogLevel.Trace); }
            if (TempRefs.MilkMarn == 803) { result = false; ModFunctions.LogVerbose($"MilkMarn is not set", LogLevel.Trace); }
            if (TempRefs.MilkRobi == 803) { result = false; ModFunctions.LogVerbose($"MilkRobi is not set", LogLevel.Trace); }
            if (TempRefs.MilkPam == 803) { result = false; ModFunctions.LogVerbose($"MilkPam is not set", LogLevel.Trace); }
            if (TempRefs.MilkSand == 803) { result = false; ModFunctions.LogVerbose($"MilkSand is not set", LogLevel.Trace); }
            if (TempRefs.MilkEvel == 803) { result = false; ModFunctions.LogVerbose($"MilkEvel is not set", LogLevel.Trace); }
            if (TempRefs.MilkDwarf == 803) { result = false; ModFunctions.LogVerbose($"MilkDwarf is not set", LogLevel.Trace); }
            if (TempRefs.MilkGeneric == 803) { result = false; ModFunctions.LogVerbose($"MilkGeneric is not set", LogLevel.Trace); }


            // Cum item code storage.
            if (TempRefs.MilkSpecial == 803) { result = false; ModFunctions.LogVerbose($"MilkSpecial is not set", LogLevel.Trace); }
            if (TempRefs.MilkAlex == 803) { result = false; ModFunctions.LogVerbose($"MilkAlex is not set", LogLevel.Trace); }
            if (TempRefs.MilkClint == 803) { result = false; ModFunctions.LogVerbose($"MilkClint is not set", LogLevel.Trace); }
            if (TempRefs.MilkDemetrius == 803) { result = false; ModFunctions.LogVerbose($"MilkDemetrius is not set", LogLevel.Trace); }
            if (TempRefs.MilkElliott == 803) { result = false; ModFunctions.LogVerbose($"MilkElliott is not set", LogLevel.Trace); }
            if (TempRefs.MilkGeorge == 803) { result = false; ModFunctions.LogVerbose($"MilkGeorge is not set", LogLevel.Trace); }
            if (TempRefs.MilkGil == 803) { result = false; ModFunctions.LogVerbose($"MilkGil is not set", LogLevel.Trace); }
            if (TempRefs.MilkGunther == 803) { result = false; ModFunctions.LogVerbose($"MilkGunther is not set", LogLevel.Trace); }
            if (TempRefs.MilkGus == 803) { result = false; ModFunctions.LogVerbose($"MilkGus is not set", LogLevel.Trace); }
            if (TempRefs.MilkHarv == 803) { result = false; ModFunctions.LogVerbose($"MilkHarv is not set", LogLevel.Trace); }
            if (TempRefs.MilkKent == 803) { result = false; ModFunctions.LogVerbose($"MilkKent is not set", LogLevel.Trace); }
            if (TempRefs.MilkLewis == 803) { result = false; ModFunctions.LogVerbose($"MilkLewis is not set", LogLevel.Trace); }
            if (TempRefs.MilkLinus == 803) { result = false; ModFunctions.LogVerbose($"MilkLinus is not set", LogLevel.Trace); }
            if (TempRefs.MilkMarlon == 803) { result = false; ModFunctions.LogVerbose($"MilkMarlon is not set", LogLevel.Trace); }
            if (TempRefs.MilkMorris == 803) { result = false; ModFunctions.LogVerbose($"MilkMorris is not set", LogLevel.Trace); }
            if (TempRefs.MilkQi == 803) { result = false; ModFunctions.LogVerbose($"MilkQi is not set", LogLevel.Trace); }
            if (TempRefs.MilkPierre == 803) { result = false; ModFunctions.LogVerbose($"MilkPierre is not set", LogLevel.Trace); }
            if (TempRefs.MilkSam == 803) { result = false; ModFunctions.LogVerbose($"MilkSam is not set", LogLevel.Trace); }
            if (TempRefs.MilkSeb == 803) { result = false; ModFunctions.LogVerbose($"MilkSeb is not set", LogLevel.Trace); }
            if (TempRefs.MilkShane == 803) { result = false; ModFunctions.LogVerbose($"MilkShane is not set", LogLevel.Trace); }
            if (TempRefs.MilkWilly == 803) { result = false; ModFunctions.LogVerbose($"MilkWilly is not set", LogLevel.Trace); }
            if (TempRefs.MilkWiz == 803) { result = false; ModFunctions.LogVerbose($"MilkWiz is not set", LogLevel.Trace); }
            //if (TempRefs.MilkWMarlon == 803) { result = false; ModFunctions.LogVerbose($"MilkWMarlon is not set", LogLevel.Trace); }
            if (TempRefs.MilkKrobus == 803) { result = false; ModFunctions.LogVerbose($"MilkKrobus is not set", LogLevel.Trace); }

            // Recipe item code storage
            if (TempRefs.ProteinShake == 1240) { result = false; ModFunctions.LogVerbose($"ProteinShake is not set", LogLevel.Trace); }
            if (TempRefs.MilkShake == 1241) { result = false; ModFunctions.LogVerbose($"MilkShake is not set", LogLevel.Trace); }
            if (TempRefs.SuperJuice == 1249) { result = false; ModFunctions.LogVerbose($"SuperJuice is not set", LogLevel.Trace); }

            // Other mods
            if (TempRefs.MilkSophia == 803) { result = false; ModFunctions.LogVerbose($"MilkSophia is not set", LogLevel.Trace); }
            if (TempRefs.MilkOlivia == 803) { result = false; ModFunctions.LogVerbose($"MilkOlivia is not set", LogLevel.Trace); }
            if (TempRefs.MilkSusan == 803) { result = false; ModFunctions.LogVerbose($"MilkSusan is not set", LogLevel.Trace); }
            if (TempRefs.MilkClaire == 803) { result = false; ModFunctions.LogVerbose($"MilkClaire is not set", LogLevel.Trace); }
            if (TempRefs.MilkAndy == 803) { result = false; ModFunctions.LogVerbose($"MilkAndy is not set", LogLevel.Trace); }
            if (TempRefs.MilkVictor == 803) { result = false; ModFunctions.LogVerbose($"MilkVictor is not set", LogLevel.Trace); }
            if (TempRefs.MilkMartin == 803) { result = false; ModFunctions.LogVerbose($"MilkMartin is not set", LogLevel.Trace); }
            #endregion

            // Item types
            if (TempRefs.MilkType == -34) { result = false; ModFunctions.LogVerbose($"MilkType hasn't changed", LogLevel.Trace); }
            if (TempRefs.CumType == -35) { result = false; ModFunctions.LogVerbose($"CumType hasn't changed", LogLevel.Trace); }
            if (TempRefs.SpecialType == -36) { result = false; ModFunctions.LogVerbose($"SpecialType hasn't changed", LogLevel.Trace); }

            // Quest items
            if ( TempRefs.PennyBook == 804) { result = false; ModFunctions.LogVerbose($"Penny's book hasn't changed", LogLevel.Trace); }
            if ( TempRefs.HaleyCamera == 804) { result = false; ModFunctions.LogVerbose($"Haley's Camera hasn't changed", LogLevel.Trace); }
            if ( TempRefs.HaleyPanties == 804) { result = false; ModFunctions.LogVerbose($"Haley's Panties hasn't changed.", LogLevel.Trace); }
            if (TempRefs.ReadiMilk == 804) { result = false; ModFunctions.LogVerbose($"Readi Milk hasn't changed.", LogLevel.Trace); }

            return result;
        }
    }


    public class MilkItems : SObject
    {

        public override string getCategoryName()
        {
            switch (Category) //TODO work out if this can be softcoded.
            {
                case -34: // MilkType
                    return "Woman's Milk";

                case -35: // CumType
                    return "Man's Milk";

                case -36: //SpecialType
                    return "Magical Essence";

                default: // Default - not found.
                    return base.getCategoryName();
            }

        }

    }

}
