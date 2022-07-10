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

    public class ItemEditor : IAssetEditor
    {
        public IDictionary<int, string> Data;

        public IDictionary<int, string> Report()
        {
            return Data;
        }

        public bool CanEdit<T>(IAssetInfo asset)
        {
            return asset.AssetNameEquals("Data/ObjectInformation");
        }

        public void Edit<T>(IAssetData asset)
        {
            ModFunctions.LogVerbose("Adding in items");

            if (asset.AssetNameEquals("Data/ObjectInformation"))
                Data = asset.AsDictionary<int, string>().Data;
            if (!TempRefs.loaded)
                return;

            SetItems();

        }

        public void RemoveInvalid(bool Male, bool Female)
        {
            if (!Female)
            {
                //milk items
                Data.Remove(TempRefs.MilkAbig);
                Data.Remove(TempRefs.MilkEmil);
                Data.Remove(TempRefs.MilkHale);
                Data.Remove(TempRefs.MilkLeah);
                Data.Remove(TempRefs.MilkMaru);
                Data.Remove(TempRefs.MilkPenn);
                Data.Remove(TempRefs.MilkCaro);
                Data.Remove(TempRefs.MilkJodi);
                Data.Remove(TempRefs.MilkMarn);
                Data.Remove(TempRefs.MilkRobi);
                Data.Remove(TempRefs.MilkPam);
                Data.Remove(TempRefs.MilkSand);
                Data.Remove(TempRefs.MilkEvel);
                // Other mods
                Data.Remove(TempRefs.MilkSophia);
                Data.Remove(TempRefs.MilkOlivia);
                Data.Remove(TempRefs.MilkSusan);
                Data.Remove(TempRefs.MilkClaire);

                // Recipes
                Data.Remove(TempRefs.MilkGeneric);
                Data.Remove(TempRefs.MilkShake);

            }

            if (!Male)
            {
                //cum items
                Data.Remove(TempRefs.MilkAlex);
                Data.Remove(TempRefs.MilkClint);
                Data.Remove(TempRefs.MilkDemetrius);
                Data.Remove(TempRefs.MilkElliott);
                Data.Remove(TempRefs.MilkGeorge);
                Data.Remove(TempRefs.MilkGil);
                Data.Remove(TempRefs.MilkGunther);
                Data.Remove(TempRefs.MilkGus);
                Data.Remove(TempRefs.MilkHarv);
                Data.Remove(TempRefs.MilkKent);
                Data.Remove(TempRefs.MilkLewis);
                Data.Remove(TempRefs.MilkLinus);
                Data.Remove(TempRefs.MilkMarlon);
                Data.Remove(TempRefs.MilkMorris);
                Data.Remove(TempRefs.MilkPierre);
                Data.Remove(TempRefs.MilkSam);
                Data.Remove(TempRefs.MilkSeb);
                Data.Remove(TempRefs.MilkShane);
                Data.Remove(TempRefs.MilkWilly);
                // Other mods
                Data.Remove(TempRefs.MilkAndy);
                Data.Remove(TempRefs.MilkVictor);
                Data.Remove(TempRefs.MilkMartin);

                // Special materials
                Data.Remove(TempRefs.MilkWiz);
                Data.Remove(TempRefs.MilkQi);

                // Recipes.
                Data.Remove(TempRefs.MilkSpecial);
                Data.Remove(TempRefs.ProteinShake);

            }

            if (!(Male || Female))
                Data.Remove(TempRefs.SuperJuice);

            SetItems(Male, Female);
        }

        public void SetItems(bool Male = true, bool Female = true)
        {
            if (Data == null)
                return;

            if (Female)
            {
                // milk items
                Data[TempRefs.MilkAbig] = $"Abigail's Milk/300/30/Drink {TempRefs.MilkType}/Abigail's Milk/A jug of Abigail's milk./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
                Data[TempRefs.MilkEmil] = $"Emily's Milk/300/30/Drink {TempRefs.MilkType}/Emily's Milk/A jug of Emily's milk./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
                Data[TempRefs.MilkHale] = $"Haley's Milk/300/30/Drink {TempRefs.MilkType}/Haley's Milk/A jug of Haley's milk./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
                Data[TempRefs.MilkLeah] = $"Leah's Milk/300/30/Drink {TempRefs.MilkType}/Leah's Milk/A jug of Leah's milk./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
                Data[TempRefs.MilkMaru] = $"Maru's Milk/300/30/Drink {TempRefs.MilkType}/Maru's Milk/A jug of Maru's milk./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
                Data[TempRefs.MilkPenn] = $"Penny's Milk/300/30/Drink {TempRefs.MilkType}/Penny's Milk/A jug of Penny's milk./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
                Data[TempRefs.MilkCaro] = $"Caroline's Milk/300/30/Drink {TempRefs.MilkType}/Caroline's Milk/A jug of Caroline's milk./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
                Data[TempRefs.MilkJodi] = $"Jodi's Milk/200/20/Drink {TempRefs.MilkType}/Jodi's Milk/A jug of Jodi's milk./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
                Data[TempRefs.MilkMarn] = $"Marnie's Milk/200/20/Drink {TempRefs.MilkType}/Marnie's Milk/A jug of Marnie's milk./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
                Data[TempRefs.MilkRobi] = $"Robin's Milk/200/20/Drink {TempRefs.MilkType}/Robin's Milk/A jug of Robin's milk./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
                Data[TempRefs.MilkPam] = $"Pam's Milk/90/10/Drink {TempRefs.MilkType}/Pam's Milk/A jug of Pam's milk./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
                Data[TempRefs.MilkSand] = $"Sandy's Milk/200/20/Drink {TempRefs.MilkType}/Sandy's Milk/A jug of Sandy's milk./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
                Data[TempRefs.MilkEvel] = $"Evelyn's Milk/90/10/Drink {TempRefs.MilkType}/Evelyn's Milk/A jug of Evelyn's milk./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
                // Other mods
                Data[TempRefs.MilkSophia] = $"Sophia's Milk/300/30/Drink {TempRefs.MilkType}/Sophia's Milk/A jug of Sophia's milk./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
                Data[TempRefs.MilkOlivia] = $"Olivia's Milk/200/20/Drink {TempRefs.MilkType}/Olivia's Milk/A jug of Olivia's milk./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
                Data[TempRefs.MilkSusan] = $"Susan's Milk/300/30/Drink {TempRefs.MilkType}/Susan's Milk/A jug of Susan 's milk./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
                Data[TempRefs.MilkClaire] = $"Claire's Milk/200/20/Drink {TempRefs.MilkType}/Claire's Milk/A jug of Claire's milk./Drink/0 0 0 0 0 0 0 0 0 0 0/0";

                //recipe
                Data[TempRefs.MilkGeneric] = $"Breast Milk/50/15/Cooking {TempRefs.MilkType}/Breast Milk/A jug of breast milk./drink/0 0 0 0 0 0 0 0 0 0 0/0";
                Data[TempRefs.MilkShake] = $"Milkshake/50/15/Cooking -7/Special' Milkshake/Extra milky milkshake./drink/0 0 0 0 0 0 0 25 0 1 0/343";

            }

            if (Male)
            {
                //cum items
                Data[TempRefs.MilkAlex] = $"Alex's Cum/300/15/Drink {TempRefs.CumType}/Alex's Cum /A bottle of Alex's Cum ./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
                Data[TempRefs.MilkClint] = $"Clint's Cum/300/15/Drink {TempRefs.CumType}/Clint's Cum/A bottle of Clint's Cum./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
                Data[TempRefs.MilkDemetrius] = $"Demetrius's Cum/300/15/Drink {TempRefs.CumType}/Demetrius's Cum/A bottle of Demetrius's Cum./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
                Data[TempRefs.MilkElliott] = $"Elliott's Cum/300/15/Drink {TempRefs.CumType}/Elliott's Cum/A bottle of Elliott's Cum./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
                Data[TempRefs.MilkGeorge] = $"George's Cum/300/15/Drink {TempRefs.CumType}/George's Cum /A bottle of George's Cum ./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
                Data[TempRefs.MilkGil] = $"Gil's Cum/300/15/Drink {TempRefs.CumType}/Gil's Cum/A bottle of Gil's Cum./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
                Data[TempRefs.MilkGunther] = $"Gunther's Cum/300/15/Drink {TempRefs.CumType}/Gunther's Cum/A bottle of Gunther's Cum./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
                Data[TempRefs.MilkGus] = $"Gus's Cum/300/15/Drink {TempRefs.CumType}/Gus's Cum/A bottle of Gus's Cum./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
                Data[TempRefs.MilkHarv] = $"Harvey's Cum/300/15/Drink {TempRefs.CumType}/Harvey's Cum /A bottle of Harvey's Cum ./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
                Data[TempRefs.MilkKent] = $"Kent's Cum/300/15/Drink {TempRefs.CumType}/Kent's Cum /A bottle of Kent's Cum ./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
                Data[TempRefs.MilkLewis] = $"Lewis's Cum/300/15/Drink {TempRefs.CumType}/Lewis's Cum/A bottle of Lewis's Cum./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
                Data[TempRefs.MilkLinus] = $"Linus's Cum/300/15/Drink {TempRefs.CumType}/Linus's Cum/A bottle of Linus's Cum./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
                Data[TempRefs.MilkMarlon] = $"Marlon's Cum/300/15/Drink {TempRefs.CumType}/Marlon's Cum /A bottle of Marlon's Cum ./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
                Data[TempRefs.MilkMorris] = $"Morris's Cum/300/15/Drink {TempRefs.CumType}/Morris's Cum /A bottle of Morris's Cum ./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
                Data[TempRefs.MilkPierre] = $"Pierre's Cum/300/15/Drink {TempRefs.CumType}/Pierre's Cum /A bottle of Pierre's Cum ./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
                Data[TempRefs.MilkSam] = $"Sam's Cum/300/15/Drink {TempRefs.CumType}/Sam's Cum/A bottle of Sam's Cum./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
                Data[TempRefs.MilkSeb] = $"Sebastian's Cum/300/15/Drink {TempRefs.CumType}/Sebastian's Cum/A bottle of Sebastian's Cum./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
                Data[TempRefs.MilkShane] = $"Shane's Cum/300/15/Drink {TempRefs.CumType}/Shane's Cum/A bottle of Shane's Cum./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
                Data[TempRefs.MilkWilly] = $"Willy's Cum/300/15/Drink {TempRefs.CumType}/Willy's Cum/A bottle of Willy's Cum./Drink/0 0 0 0 0 0 0 0 0 0 0/0";

                // Other mods
                Data[TempRefs.MilkAndy] = $"Andy's Cum/50/15/Drink {TempRefs.CumType}/Andy's Cum/A bottle of Andy's Cum./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
                Data[TempRefs.MilkVictor] = $"Victor's Cum/50/15/Drink {TempRefs.CumType}/Victor's Cum/A bottle of Victor's Cum./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
                Data[TempRefs.MilkMartin] = $"Martin's Cum/50/15/Drink {TempRefs.CumType}/Martin's Cum/A bottle of Martin's Cum./Drink/0 0 0 0 0 0 0 0 0 0 0/0";

                // Special materials
                Data[TempRefs.MilkDwarf] = $"Dwarf's Essence/300/15/Drink {TempRefs.SpecialType}/Dwarf's Essence/A bottle of Dwarf's Essence./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
                Data[TempRefs.MilkKrobus] = $"Krobus's Essence/300/15/Drink {TempRefs.SpecialType}/Krobus's Essence/A bottle of Krobus's Essence./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
                Data[TempRefs.MilkWiz] = $"Wizard's Essence/300/15/Drink {TempRefs.SpecialType}/Wizard's Essence/A bottle of Wizard's Essence./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
                Data[TempRefs.MilkQi] = $"Mr. Qi's Essence/300/15/Drink {TempRefs.SpecialType}/Mr. Qi's Essence/A bottle of Mr. Qi's Essence./Drink/0 0 0 0 0 0 0 0 0 0 0/0";
                Data[TempRefs.MilkMagic] = $"Magical Essence/300/15/Drink {TempRefs.SpecialType}/Magical Essence/A bottle of Magical Essence./Drink/0 0 0 0 0 0 0 0 0 0 0/0";

                //recipe
                Data[TempRefs.MilkSpecial] = $"Special milk/50/15/Cooking {TempRefs.CumType}/Special Milk/A bottle of 'special' milk./drink/0 0 0 0 0 0 0 0 0 0 0/0";
                Data[TempRefs.ProteinShake] = $"Protein shake/50/15/Cooking -7/Protein' shake/Shake made with extra protein/drink/0 0 0 0 0 0 0 25 0 0 2/343";

            }

            if (Male && Female) Data[TempRefs.SuperJuice] = $"Super Juice/150/125/Cooking -7/Super Juice/The perfect fusion of male and female juices./Drink/0 0 0 0 2 0 0 25 0 3 2/700";


        }

        public bool CheckAll()
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
