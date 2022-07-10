using System.Linq;
using StardewModdingAPI;
using System.Collections.Generic;

namespace MilkVillagers
{
    public class RecipeEditor : IAssetEditor
    {
        public IDictionary<string, string> CookingData;
        public IDictionary<string, string> CraftingData;

        public bool CanEdit<T>(IAssetInfo asset)
        {
            return asset.AssetNameEquals("Data/CookingRecipes") || asset.AssetNameEquals("Data/CraftingRecipes");
        }

        public void Edit<T>(IAssetData asset)
        {
            ModFunctions.LogVerbose("Loading recipes", LogLevel.Trace);
            if (asset.AssetNameEquals("Data/CookingRecipes"))
            {
                CookingData = asset.AsDictionary<string, string>().Data;
                SetCooking();
            }
            if (asset.AssetNameEquals("Data/CraftingRecipes"))
            {
                CraftingData = asset.AsDictionary<string, string>().Data;

                SetCrafting();
            }
        }

        public bool SetCooking(bool Male = true, bool Female = true)
        {
            if (CookingData == null)
                return false;

            if (Male)
                CookingData["'Protein' Shake"] = $"{TempRefs.CumType} 1/10 10/{TempRefs.ProteinShake}/null/'Protein' shake";

            if (Female)
                CookingData["Milkshake"] = $"{TempRefs.MilkType} 1/10 10/{TempRefs.MilkShake}/null/Milkshake";

            if (Male && Female)
                CookingData["Super Juice"] = $"{TempRefs.MilkType} 2 {TempRefs.CumType} 2/10 10/{TempRefs.SuperJuice}/default/Super Juice";
            return true;
        }

        public bool SetCrafting(bool Male = true, bool Female = true)
        {
            if (CraftingData == null)
                return false;

            if (Male)
                CraftingData["Special Milk"] = $"{TempRefs.CumType} 1/Field/{TempRefs.MilkSpecial}/false/Special Milk";

            if (Female)
                CraftingData["Woman's Milk"] = $"{TempRefs.MilkType} 1/Field/{TempRefs.MilkGeneric}/false/Woman's Milk";
            return true;
        }

        public void RemoveInvalid(bool Male, bool Female)
        {
            if (!Male)
            {
                CookingData.Remove("'Protein' Shake");
                CookingData.Remove("Super Juice");
                CraftingData.Remove("Special Milk");
            }

            if (!Female)
            {
                CookingData.Remove("Milkshake");
                CookingData.Remove("Super Juice");
                CraftingData.Remove("Woman's Milk");
            }

            SetCooking(Male, Female);
            SetCrafting(Male, Female);
        }

        public bool CheckAll()
        {
            bool result = true;

            if (!CraftingData.Keys.Contains("Special Milk"))
            {
                ModFunctions.LogVerbose("Missing Special Milk Recipe");
                result = false;
            }
            if (!CraftingData.Keys.Contains("Woman's Milk"))
            {
                ModFunctions.LogVerbose("Missing Woman's Milk Recipe");
                result = false;
            }

            if (!CookingData.Keys.Contains("'Protein' Shake"))
            {
                ModFunctions.LogVerbose("Missing 'Protein' Shake Recipe");
                result = false;
            }
            if (!CookingData.Keys.Contains("Milkshake"))
            {
                ModFunctions.LogVerbose("Missing Milkshake Recipe");
                result = false;
            }
            if (!CookingData.Keys.Contains("Super Juice"))
            {
                ModFunctions.LogVerbose("Missing Super Juice Recipe");
                result = false;
            }

            return result;
        }
    }

}
