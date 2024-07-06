using System.Collections.Generic;
using System.Collections;
using StardewModdingAPI;
using Microsoft.VisualBasic;

namespace MilkVillagers.Asset_Editors
{

    public static class MailEditor //: IAssetEditor
    {
        public static List<string> Mail = new List<string>()
            {
                "MTV_InvitationID",
                "MTV_Vagina",
                "MTV_Penis",
                "MTV_Ace",
                "MTV_Herm",
                "MilkButton1",
                "MilkButton2",
                "MilkingProfQuality",
                "MilkingProfCount",
                "AbiMilking",
                "MTV_AbigailQ1",
                "MTV_AbigailQ1T",
                "MTV_AbigailQ2",
                "MTV_AbigailQ2T",
                "MTV_AbigailQ3",
                "MTV_AbigailQ3T",
                "MTV_AbigailQ4",
                "MTV_AbigailQ4T",
                "MTV_MaruQ1",
                "MTV_MaruQ1T",
                "MTV_MaruQ2",
                "MTV_MaruQ2T",
                "MTV_MaruQ3",
                "MTV_MaruQ3T",
                "MTV_MaruQ4",
                "MTV_MaruQ4T",
                "MTV_GeorgeQ1",
                "MTV_GeorgeQ1T",
                "MTV_GeorgeQ2",
                "MTV_GeorgeQ2T",
                "MTV_GeorgeQ3",
                "MTV_GeorgeQ3T",
                "MTV_GeorgeQ4",
                "MTV_GeorgeQ4T",
                "MTV_HarveyQ1",
                "MTV_HarveyQ1T",
                "MTV_HarveyQ2",
                "MTV_HarveyQ2T",
                "MTV_HarveyQ3",
                "MTV_HarveyQ3T",
                "MTV_HarveyQ4",
                "MTV_HarveyQ4T",
                "MTV_ElliottQ1",
                "MTV_ElliottQ1T",
                "MTV_ElliottQ2",
                "MTV_ElliottQ2T",
                "MTV_ElliottQ3",
                "MTV_ElliottQ3T",
                "MTV_ElliottQ4",
                "MTV_ElliottQ4T",
                "MTV_EmilyQ1",
                "MTV_EmilyQ1T",
                "MTV_EmilyQ2",
                "MTV_EmilyQ2P",
                "MTV_EmilyQ2T",
                "MTV_EmilyQ3",
                "MTV_EmilyQ3T",
                "MTV_EmilyQ4",
                "MTV_EmilyQ4T",
                "MTV_HaleyQ1",
                "MTV_HaleyQ1T",
                "MTV_HaleyQ2",
                "MTV_HaleyQ2T",
                "MTV_HaleyQ3",
                "MTV_HaleyQ3T",
                "MTV_HaleyQ4",
                "MTV_HaleyQ4-2",
                "MTV_PennyQ1",
                "MTV_PennyQ1P",
                "MTV_PennyQ1T",
                "MTV_PennyQ2",
                "MTV_PennyQ2T",
                "MTV_PennyQ3",
                "MTV_PennyQ3T",
                "MTV_PennyQ4",
                "MTV_PennyQ4T",
                "MTV_LeahQ1",
                "MTV_LeahQ1P",
                "MTV_LeahQ1T",
                "MTV_LeahQ2",
                "MTV_LeahQ2T",
                "MTV_LeahQ3",
                "MTV_LeahQ3T",
                "MTV_LeahQ4",
                "MTV_LeahQ4T",
                "MTV_SebQ1",
                "MTV_SebQ1T",
                "MTV_SebQ2",
                "MTV_SebQ2T",
                "MTV_SebQ3",
                "MTV_SebQ3T",
                "MTV_SebQ4",
                "MTV_SebQ4T",
                "MagicalItem",
                "SOQiReward1",
                "SOQiReward2"
            };

        public static Dictionary<string, string> FirstMail = new Dictionary<string, string>()
        {
            ["Abigail"] = "MTV_AbigailQ1",      //Abigail quest 1
            ["Maru"] = "MTV_MaruQ1",            //Maru quest 1
            ["George"] = "MTV_GeorgeQ1",        //George quest 1
            ["Harvey"] = "MTV_HarveyQ1",        //Harvey quest 1
            ["Elliott"] = "MTV_ElliottQ1",      //Elliott quest 1
            ["Emily"] = "MTV_EmilyQ1",          //Emily quest 1
            //["Haley"] = "MTV_HaleyQ1",          //Haley quest 1
            ["Penny"] = "MTV_PennyQ1",          //Penny quest 1
            ["Leah"] = "MTV_LeahQ1",            //Leah quest 1
            ["Sebastian"] = "MTV_SebQ1",    //Sebastian quest 1
        };


        static IDictionary<string, string> data;

        public static bool CanEdit<T>(IAssetInfo asset)
        {
            return asset.Name.IsEquivalentTo("Data\\mail");
        }

        public static bool CanEdit(IAssetName asset)
        {
            return asset.IsEquivalentTo("Data\\mail");
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
            data = asset.AsDictionary<string, string>().Data;

            return;

            //foreach (KeyValuePair<string, string> kvp in MTVMail)
            //{
            //    if (!ContainsKey(kvp.Key))
            //    {
            //        ModFunctions.LogVerbose($"Injecting mail item {kvp.Key}", LogLevel.Alert);
            //        data[kvp.Key] = kvp.Value;
            //    }
            //    else
            //    {
            //        ModFunctions.LogVerbose($"Mail already contains {kvp.Key}", LogLevel.Alert);
            //    }
            //}
        }

        public static void UpdateData(Dictionary<string, string> assetdata)
        {
            data = assetdata;
            if (!data.ContainsKey("MTV_InvitationID")) return;

            string s = int.TryParse(data["MTV_InvitationID"], out TempRefs.InvitationsSent) ? $"Invitation ItemID is {TempRefs.InvitationsSent}" : "Failed to get Invitation ItemID from mail";
            ModFunctions.Log(s, LogLevel.Trace);
        }

        //public static bool ContainsKey(string key)
        //{
        //    return MTVMail.ContainsKey(key);
        //}

        public static void Report()
        {
            if (data == null) { ModFunctions.Log("data is null"); return; }

            ModFunctions.Log("Dumping mail");
            foreach (KeyValuePair<string, string> mail in data)
            {
                ModFunctions.Log($"{mail.Key}: {mail.Value}");
            }
        }
    }


}
