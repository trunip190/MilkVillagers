using System.Collections.Generic;
using System.Data;
using System.Linq;
using StardewModdingAPI;

namespace MilkVillagers.Asset_Editors
{
    public enum Emote
    {
        question = 8,
        angry = 12,
        exclamation = 16,
        heart = 20,
        sleep = 24,
        sad = 28,
        happy = 32,
        x = 36,
        pause = 40,
        videogame = 52,
        music = 56,
        blush = 60,
    }

    public static class EventEditor
    {
        private static IDictionary<string, string> data;

        public static bool CanEdit(IAssetName asset)
        {
            //    bool result = asset.IsEquivalentTo("Data/Events/Seedshop")
            //        || asset.IsEquivalentTo("Data/Events/Hospital")
            //        || asset.IsEquivalentTo("Data/Events/LeahHouse")
            //        || asset.IsEquivalentTo("Data/Events/ArchaeologyHouse")
            //        || asset.IsEquivalentTo("Data/Events/Saloon")
            //        || asset.IsEquivalentTo("Data/Events/Sunroom")
            //        || asset.IsEquivalentTo("Data/Events/ScienceHouse")
            //        || asset.IsEquivalentTo("Data/Events/HaleyHouse")
            //        || asset.IsEquivalentTo("Data/Events/BathHouse_Pool");

            bool result = asset.Name.Contains("Data/Events/");

            return result;
        }

        public static void Edit(IAssetData asset)
        {
            EditAsset(asset);
        }

        private static void EditAsset(IAssetData asset)
        {
            data = asset.AsDictionary<string, string>().Data;

            //foreach (KeyValuePair<string, string> kvp in data)
            //{
            //    ModFunctions.Log($"MTV event:\t{kvp.Key}: {kvp.Value}", LogLevel.Trace);
            //}
        }
    }

    public class EventShell
    {
        public bool Has_Vagina = false;
        public bool Has_Penis = false;
        public bool Is_Ace = false;

        public string EventConditions = "";
        public string EventDataV = "";  // Has Vagina
        public string EventDataP = "";  // Has Penis
        public string EventDataA = "";  // Is Ace
        public string EventDataB = "";  // Is Herm

        public EventShell()
        {

        }

        public string GetEventData()
        {
            if (Is_Ace) return EventDataA;
            if (Has_Penis && Has_Vagina) return EventDataB;
            if (Has_Vagina) return EventDataV;
            if (Has_Penis) return EventDataP;

            return "";
        }
    }

}
