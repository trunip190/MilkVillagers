using System.Linq;
using StardewModdingAPI;
using System.Collections.Generic;

namespace MilkVillagers
{
    public static class RecipeEditor //: IAssetEditor
    {
        private static IDictionary<string, string> CookingData;
        private static IDictionary<string, string> CraftingData;

        public static bool CanEdit<T>(IAssetInfo asset)
        {
            return asset.Name.IsEquivalentTo("Data/CookingRecipes") || asset.Name.IsEquivalentTo("Data/CraftingRecipes");
        }

        public static bool CanEdit(IAssetName asset)
        {
            return asset.IsEquivalentTo("Data/CookingRecipes") || asset.IsEquivalentTo("Data/CraftingRecipes");
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
            // ModFunctions.LogVerbose("Loading recipes", LogLevel.Trace);

            if (asset.Name.IsEquivalentTo("Data/CookingRecipes"))
            {
                CookingData = asset.AsDictionary<string, string>().Data;
                //SetCooking();
            }
            if (asset.Name.IsEquivalentTo("Data/CraftingRecipes"))
            {
                CraftingData = asset.AsDictionary<string, string>().Data;
                //SetCrafting();
            }
        }

        public static void UpdateCookingData(Dictionary<string, string> assetdata)
        {
            CookingData = assetdata;
            ModFunctions.LogVerbose("Updating RecipeEditor: CookingData", LogLevel.Trace);
        }

        public static void UpdateCraftingData(Dictionary<string, string> assetdata)
        {
            CraftingData = assetdata;
            ModFunctions.LogVerbose("Updating RecipeEditor: CraftingData", LogLevel.Trace);
        }

        public static bool SetCooking(bool Male = true, bool Female = true)
        {
            if (CookingData == null) return false;

            if (CookingData.ContainsKey("Protein Shake"))
            {
                if (Male) { CookingData["Protein Shake"] = $"{TempRefs.CumType} 1/10 10/{TempRefs.ProteinShake}//Protein Shake"; }
            }
            else ModFunctions.LogVerbose("Protein Shake not found", LogLevel.Alert);

            if (CookingData.ContainsKey("Special Milkshake"))
            {
                if (Female) { CookingData["Special Milkshake"] = $"{TempRefs.MilkType} 1/10 10/{TempRefs.MilkShake}//Special Milkshake"; }
            }
            else ModFunctions.LogVerbose("Special Milkshake not found", LogLevel.Alert);

            if (CookingData.ContainsKey("Super Juice"))
            {
                if (Male && Female) CookingData["Super Juice"] = $"{TempRefs.MilkType} 2 {TempRefs.CumType} 2/10 10/{TempRefs.SuperJuice}//Super Juice";
            }
            else ModFunctions.LogVerbose("Super Juice not found", LogLevel.Alert);

            return true;
        }

        public static bool SetCrafting(bool Male = true, bool Female = true)
        {
            //if (CraftingData == null)
            //    return false;

            if (Male)
            {
                ModFunctions.LogVerbose($"{CraftingData["Special Milk"]}");
                CraftingData["Special Milk"] = $"{TempRefs.CumType} 1/Field/{TempRefs.MilkSpecial}/false/Special Milk";
                ModFunctions.LogVerbose($"{CraftingData["Special Milk"]}");
            }

            if (Female)
            {
                ModFunctions.LogVerbose($"{CraftingData["Woman's Milk"]}");
                CraftingData["Woman's Milk"] = $"{TempRefs.MilkType} 1/Field/{TempRefs.MilkGeneric}/false/Woman's Milk";
                ModFunctions.LogVerbose($"{CraftingData["Woman's Milk"]}");
            }

            return true;
        }

        public static void RemoveInvalid(bool Male, bool Female)
        {
            if (!Male)
            {
                CookingData.Remove("Protein Shake");
                CookingData.Remove("Super Juice");
                CraftingData.Remove("Special Milk");
            }

            if (!Female)
            {
                CookingData.Remove("Milkshake");
                CookingData.Remove("Super Juice");
                CraftingData.Remove("Woman's Milk");
            }

            //SetCooking(Male, Female);
            //SetCrafting(Male, Female);
        }

        public static void ReportAll()
        {
            foreach( KeyValuePair<string, string> kvp in CookingData)
            {
                ModFunctions.LogVerbose($"{kvp.Key}: {kvp.Value}", Force: true);
            }

            foreach (KeyValuePair<string, string> kvp in CraftingData)
            {
                ModFunctions.LogVerbose($"{kvp.Key}: {kvp.Value}", Force: true);
            }
        }

        public static bool CheckAll()
        {
            bool result = true;

            if (!CraftingData.Keys.Contains("Special Milk")) { ModFunctions.LogVerbose("Missing Special Milk Recipe"); result = false; }
            if (!CraftingData.Keys.Contains("Woman's Milk")) { ModFunctions.LogVerbose("Missing Woman's Milk Recipe"); result = false; }

            if (!CookingData.Keys.Contains("'Protein' Shake")) { ModFunctions.LogVerbose("Missing 'Protein' Shake Recipe"); result = false; }
            if (!CookingData.Keys.Contains("Milkshake")) { ModFunctions.LogVerbose("Missing Milkshake Recipe"); result = false; }
            if (!CookingData.Keys.Contains("Super Juice")) { ModFunctions.LogVerbose("Missing Super Juice Recipe"); result = false; }

            return result;
        }
    }

}
