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
            ModFunctions.Log("Updating RecipeEditor: CookingData", LogLevel.Trace);
        }

        public static void UpdateCraftingData(Dictionary<string, string> assetdata)
        {
            CraftingData = assetdata;
            ModFunctions.Log("Updating RecipeEditor: CraftingData", LogLevel.Trace);
        }

        public static bool SetCooking(bool Male = true, bool Female = true)
        {
            if (CookingData == null) return false;
            string ProteinShake = "";
            List<string> dump = CookingData.Keys.Where(o => o.Contains("Trunip190.CP.MilkTheVillagers.Protein_Shake")).ToList();
            if (dump.Count > 0)
            {
                ProteinShake = CookingData.Keys.Where(o => o.Contains("Protein_Shake")).ToList()[0];
            }

            if (ProteinShake != null)
            {
                //if (Male) CookingData["Trunip190.JA.MilkTheVillagers_Protein_Shake"] = "-35 1/10 10/Trunip190.JA.MilkTheVillagers_Protein_Shake 1/null/Protein Shake";
                //if (Male) { CookingData[ProteinShake] = $"-35 1/10 10/Protein Shake//Protein Shake"; }
            }
            else ModFunctions.Log("Protein Shake not found", LogLevel.Alert);

            if (CookingData.Keys.Count(o => o.Contains("Trunip190.CP.MilkTheVillagers.Special_Milkshake")) > 0)
            {
                //if (Female) { CookingData["Special Milkshake"] = $"-34 1/10 10/Special Milkshake//Special Milkshake"; }
            }
            else ModFunctions.Log("Special Milkshake not found", LogLevel.Alert);

            if (CookingData.Keys.Count(o => o.Contains("Trunip190.CP.MilkTheVillagers.Super_Juice")) > 0)
            {
                //if (Male && Female) CookingData["Super Juice"] = $"-34 2 -35 2/10 10/Super Juice//Super Juice";
            }
            else ModFunctions.Log("Super Juice not found", LogLevel.Alert);


            return true;
        }

        public static bool SetCrafting(bool Male = true, bool Female = true)
        {
            //if (CraftingData == null)
            //    return false;
            //List<string> modCrafting = CraftingData.Keys.Where(o => o.Contains("Trunip190.JA.MilkTheVillagers")).ToList();
            //List<string> modRecipes = CraftingData.Values.Where(o => o.Contains("Trunip190.JA.MilkTheVillagers")).ToList();

            if (Male)
            {

                CraftingData["Trunip190.JA.MilkTheVillagers_Special_Milk"] = "-35 1/what is this for?/Trunip190.JA.MilkTheVillagers_Special_Milk 1/false/null/Special Milk";
                CraftingData["Trunip190.JA.MilkTheVillagers_Magical_Essence"] = "-36 1/what is this for?/Trunip190.JA.MilkTheVillagers_Magical_Essence 1/false/null/Magical Essence";
                //CraftingData["Trunip190.JA.MilkTheVillagers_Crotchless_Panties"] = "771 2 428 1/what is this for?/Trunip190.JA.MilkTheVillagers_Crotchless_Panties 1/false/null/Crotchless Panties";
                //CraftingData["Trunip190.JA.MilkTheVillagers_Shibari_rope"] = "-35 1/what is this for?/Trunip190.JA.MilkTheVillagers_Special_Milk 1/false/null/Special Milk";
                ModFunctions.Log($"{CraftingData["Trunip190.JA.MilkTheVillagers_Special_Milk"]}");
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
            foreach (KeyValuePair<string, string> kvp in CookingData)
            {
                ModFunctions.Log($"{kvp.Key}: {kvp.Value}", Force: true);
            }

            foreach (KeyValuePair<string, string> kvp in CraftingData)
            {
                ModFunctions.Log($"{kvp.Key}: {kvp.Value}", Force: true);
            }
        }

        public static bool CheckAll()
        {
            bool result = true;

            if (!CraftingData.Keys.Contains("Special Milk")) { ModFunctions.Log("Missing Special Milk Recipe"); result = false; }
            if (!CraftingData.Keys.Contains("Woman's Milk")) { ModFunctions.Log("Missing Woman's Milk Recipe"); result = false; }

            if (!CookingData.Keys.Contains("'Protein' Shake")) { ModFunctions.Log("Missing 'Protein' Shake Recipe"); result = false; }
            if (!CookingData.Keys.Contains("Milkshake")) { ModFunctions.Log("Missing Milkshake Recipe"); result = false; }
            if (!CookingData.Keys.Contains("Super Juice")) { ModFunctions.Log("Missing Super Juice Recipe"); result = false; }

            return result;
        }
    }

}
