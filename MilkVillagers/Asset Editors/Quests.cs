using StardewModdingAPI;
using System.Collections.Generic;
using StardewValley.GameData;

namespace MilkVillagers.Asset_Editors
{

    public class QuestEditor : IAssetEditor
    {
        public IDictionary<int, string> QuestData;
        public IDictionary<string, SpecialOrderData> SOData;

        public bool CanEdit<T>(IAssetInfo asset)
        {
            bool result = asset.AssetNameEquals("Data/Quests")
                || asset.AssetNameEquals("Data/SpecialOrders");

            if (result)
                ModFunctions.LogVerbose(asset.AssetName, LogLevel.Alert);

            return result;
        }

        public void Edit<T>(IAssetData asset)
        {
            if (asset.AssetNameEquals("Data/Quests"))
            {
                QuestData = asset.AsDictionary<int, string>().Data;
                //data[ID] = $"Type/Name/Description/Hint/Condition/Next Quest/Gold/Reward Description/Cancellable/Completion Text";
                QuestData[TempRefs.QuestAbi1] = $"ItemDelivery/Abigail's Carrot/Abigail needs a cave carrot to scratch an itch. Bring her one to 'fill' a need/Bring Abigail a cave carrot/Abigail 78/-1/410/-1/false/I hope you washed it! Those caves are wonderful, but the cave carrot needs to be SUPER clean before its going anywhere near my ass.";
                QuestData[TempRefs.QuestAbi2] = $"ItemDelivery/Abigail's Radishes/Abigail wants some radishes for a new idea she had/Bring Abigail Radishes/Abigail 264/-1/240/-1/false/I'm gonna have so much fun with these! How many do you think I can fit?";
                QuestData[TempRefs.QuestAbi3] = $"ItemDelivery/Abigail's Eggplant/Abigail needs an eggplant for her cam show. Make sure it's a good one/Bring Abigail an eggplant./Abigail 272/-1/350/-1/false/Wow, it's so big!. I'll be thinking of you tonight, @. Be sure to watch my show.";
                QuestData[TempRefs.QuestAbi4] = $"Location/Abigail's 'helping hand'/Abigail wants you to help her with her show tonight. Go visit her at her house, and bring her an amethyst./Go to Abigail's house with an Amethyst/SeedShop/-1/500/-1/false/I'm so glad you could come over and give me a helping 'hand'. My viewers are going to appreciate it as well...";

                QuestData[TempRefs.QuestMaru1] = $"ItemDelivery/Scientific sample/Maru wants to to test some cum/Bring Maru a sample of cum from a villager/Maru {TempRefs.CumType}/-1/750/-1/FALSE/Wow, I didn't expect you to bring it so quickly! It definitely looks right, and...it has that wonderful heady smell. I might post some request in the future, if you're interested.";
                QuestData[TempRefs.QuestLeah1] = $"Location/Leah pt 1/Leah has asked you to be a model for a painting she is working on. It is for an art competition, and the theme is 'The beautiful body'/Go to Leah's cabin and model for her/LeahHouse/-1/1185/-1/FALSE/Thank you, @. I feel so confident about this painting. It really highlights your...I mean the beauty of the human body.";

                QuestData[TempRefs.QuestGeorge1] = $"ItemDelivery/Rejuvenating Milk/George wants to taste some of Haley's and Emily's milk/Bring George some Special Milkshake/George {TempRefs.MilkHale} George {TempRefs.MilkEmil}/-1/750/-1/FALSE/This is exactly what I need. I've been lusting after those two for so long, but Penny keeps getting in the way and blocking me any time I get a good eyeful.";
                QuestData[TempRefs.QuestSeb1] = $"ItemDelivery/Curious tastes pt 1/Sebastian wants to drink some of Abigail's milk, but is nervous of asking her directly/Bring Sebastian some of Abigail's Milk/Sebastian {TempRefs.MilkAbig}/-1/750/-1/FALSE/Wow, She was ok with it? Did you tell her who it was for? Oh...I guess that's only fair. I knew she was kind of into me, but this could be amazing!";
                QuestData[TempRefs.QuestSeb2] = $"ItemDelivery/Curious tastes pt 2/Sebastian can have some of Abigail's milk, but only if she gets to taste his cum/Bring Abigail some of Sebastian's Cum/Abigail {TempRefs.MilkSeb}/-1/750/-1/FALSE/Did you suck it out of him, while looking into his eyes? Touching yourself as you brought him closer to orgasm? Did he cum all over your face, or were you tempted to swallow it instead? Anyway, thanks for that, cum buddy.";
                QuestData[TempRefs.QuestPenny1] = $"ItemDelivery/Penny's book/Penny wants you to help her by checking out an erotic book for her/Check out Penny's book/Penny {TempRefs.MilkSeb}/-1/750/-1/FALSE/Did you suck it out of him, while looking into his eyes? Touching yourself as you brought him closer to orgasm? Did he cum all over your face, or were you tempted to swallow it instead? Anyway, thanks for that, cum buddy.";

                QuestData[TempRefs.QuestIDWait] = $"Basic/Wait for Abigail/Give Abigail some time to do her show and contact you for the next request/Wait for Abigail/-1/-1/500/-1/false";

            }
            if (asset.AssetNameEquals("Data/SpecialOrders"))
            {
                SOData = asset.AsDictionary<string, SpecialOrderData>().Data;

                SpecialOrderData SOMilkMale = new SpecialOrderData()
                {
                    Name = "Vitality Collection",
                    Duration = "Week",
                    Requester = "Mister Qi",
                    Repeatable = "False",
                    Text = "Please practise your cum collection technique. If you can collect 10 samples for me by the end of the week I'll have special reward for you.",
                    ItemToRemoveOnEnd = "",
                    MailToRemoveOnEnd = "",
                    RandomizedElements = new List<RandomizedElement>()
                    {
                    },
                    OrderType = "",
                    RequiredTags = "island",
                    SpecialRule = "",
                    Rewards = new List<SpecialOrderRewardData>()
                    {
                        new SpecialOrderRewardData()
                        {
                            Type = "Money",
                            Data = new Dictionary<string, string>()
                            {
                                { "Amount", "3000" },
                            }
                        },
                        new SpecialOrderRewardData()
                        {
                            Type = "Mail",
                            Data = new Dictionary<string, string>()
                            {
                                { "MailReceived", "SOQiReward1" },
                                { "NoLetter", "false"},
                            }
                        }
                    },
                    Objectives = new List<SpecialOrderObjectiveData>
                    {
                        new SpecialOrderObjectiveData()
                        {
                            Type = "Collect",
                            Text = "Get 10 samples of cum",
                            RequiredCount = "10", //"10",
                            Data = new Dictionary<string, string>()
                            {
                                {"AcceptedContextTags", "item_special_milk"},
                            }
                        },
                        new SpecialOrderObjectiveData()
                        {
                            Type = "Donate",
                            Text = "Drop off the cum at Mister Qi's island base",
                            RequiredCount = "10", //"10",
                            Data = new Dictionary<string, string>()
                            {
                                {"DropBox", "QiChallengeBox"},
                                {"DropBoxGameLocation", "QiNutRoom"},
                                {"DropBoxIndicatorLocation", "1 3"},
                                {"AcceptedContextTags", "item_special_milk"},
                            }
                        },
                    }
                };
                SpecialOrderData SOMilkFemale = new SpecialOrderData()
                {
                    Name = "Soma Collection",
                    Duration = "Week",
                    Requester = "Mister Qi",
                    Repeatable = "False",
                    Text = "The women of Stardew Valley contain a vitality that is not found elsewhere. Please collect 10 bottles of their milk and deliver it to me.",
                    ItemToRemoveOnEnd = "",
                    MailToRemoveOnEnd = "",
                    RandomizedElements = new List<RandomizedElement>()
                    {
                    },
                    OrderType = "",
                    RequiredTags = "island",
                    SpecialRule = "",
                    Rewards = new List<SpecialOrderRewardData>()
                    {
                        new SpecialOrderRewardData()
                        {
                            Type = "Money",
                            Data = new Dictionary<string, string>()
                            {
                                { "Amount", "3000" },
                            }
                        },
                        new SpecialOrderRewardData()
                        {
                            Type = "Mail",
                            Data = new Dictionary<string, string>()
                            {
                                { "MailReceived", "SOQiReward2" },
                                { "NoLetter", "false"},
                            }
                        }
                    },
                    Objectives = new List<SpecialOrderObjectiveData>
                    {
                        new SpecialOrderObjectiveData()
                        {
                            Type = "Collect",
                            Text = "Get 10 samples of Women's Milk",
                            RequiredCount = "10",
                            Data = new Dictionary<string, string>()
                            {
                                {"AcceptedContextTags", "item_womans_milk"},
                            }
                        },
                        new SpecialOrderObjectiveData()
                        {
                            Type = "Donate",
                            Text = "Drop off the Women's Milk to Mister Qi's island base",
                            RequiredCount = "10",
                            Data = new Dictionary<string, string>()
                            {
                                {"DropBox", "QiChallengeBox"},
                                {"DropBoxGameLocation", "QiNutRoom"},
                                {"DropBoxIndicatorLocation", "1 3"},
                                {"AcceptedContextTags", "item_womans_milk"},
                            }
                        },
                    }
                };

                if (TempRefs.MilkFemale)
                    SOData["TruniMilkFemale"] = SOMilkFemale;

                if (TempRefs.MilkMale)
                    SOData["TruniMilkMale"] = SOMilkMale;
            }
        }

        public bool ExportSpecialOrder(string ID)
        {
            if (!SOData.ContainsKey(ID))
                return false;

            SpecialOrderData sso = SOData[ID];

            ModFunctions.LogVerbose($"Dumping data now", LogLevel.Alert);
            string output = $"\r\nSpecialOrderData SampleSpecialOrder = new SpecialOrderData()\r\n" +
                                "   {\r\n" +
                                $"  Name = \"{sso.Name}\",\r\n" +
                                $"  Duration = \"{sso.Duration}\",\r\n" +
                                $"  Requester = \"{sso.Requester}\",\r\n" +
                                $"  Repeatable = \"{sso.Repeatable}\",\r\n" +
                                $"  Text = \"{sso.Text}\",\r\n" +
                                $"  ItemToRemoveOnEnd = \"{sso.ItemToRemoveOnEnd}\",\r\n" +
                                $"  MailToRemoveOnEnd = \"{sso.MailToRemoveOnEnd}\",\r\n" +

                                // Randomised Elements
                                $"  RandomizedElements = new List<RandomizedElement>()\r\n" +
                                "  {\r\n";

            if (sso.RandomizedElements != null)
            {
                foreach (RandomizedElement ssor in sso.RandomizedElements)
                {
                    output +=
                $"      \"Name\" = {ssor.Name},\r\n" +
                "       {\r\n" +
                "           Data = new Dictionary<string, string>()\r\n" +
                "           {";
                    foreach (RandomizedElementItem v in ssor.Values)
                    {
                        output +=
                $"      \"RequiredTags\" = {v.RequiredTags},\r\n" +
                $"      \"Value\" = {v.Value},\r\n";
                    }
                    output +=
                "           },\r\n" +
                "       },\r\n";
                }
            }

            output +=
            "   },\r\n" +
            $"  OrderType = \"{sso.OrderType}\",\r\n" +
            $"  RequiredTags = \"{sso.RequiredTags}\",\r\n" +
            $"  SpecialRule = \"{sso.SpecialRule}\",\r\n" +
            $"  Rewards = new List<SpecialOrderRewardData>(),\r\n" +
            "   {\r\n";

            // Rewards
            foreach (SpecialOrderRewardData ssor in sso.Rewards)
            {
                output +=
                                $"      new SpecialOrderRewardData()\r\n" +
                                "       {\r\n" +
                                $"          Type = \"{ssor.Type}\",\r\n";
                foreach (KeyValuePair<string, string> kvp in ssor.Data)
                {
                    output +=

    $"          #\"{kvp.Key}\", \"{kvp.Value}\"#,\r\n";
                }
                output +=
    "       }\r\n";
            }

            output += "       }\r\n" +
                                $"  Objectives = new List<SpecialOrderObjectiveData>\r\n" +
                                "   {\r\n";

            foreach (SpecialOrderObjectiveData sood in sso.Objectives)
            {
                output +=
                $"      new SpecialOrderObjectiveData()\r\n" +
                "       {\r\n" +
                $"        Type = \"{sood.Type}\",\r\n" +
                $"        Text = \"{sood.Text}\",\r\n" +
                $"        RequiredCount = \"{sood.RequiredCount}\",\r\n" +

                // Data
                $"        Data = new Dictionary<string, string>()\r\n" +
                "        {\r\n";
                foreach (KeyValuePair<string, string> sod in sood.Data)
                {
                    output +=
                    $"          # \"{sod.Key}\", \"{sod.Value}\"#,\r\n";
                }
                output +=
                "        }\r\n" +
                "      },\r\n";
            }
            output += $"" +
            "  }\r\n" +
            "};";

            ModFunctions.LogVerbose(output);
            return true;
        }

        public void Report()
        {
            if (QuestData != null)
            {
                ModFunctions.LogVerbose($"Dumping quests");
                foreach (KeyValuePair<int, string> d in QuestData)
                {
                    ModFunctions.LogVerbose($"{d.Key}: {d.Value}");
                }
            }
            if (SOData != null)
            {
                ModFunctions.LogVerbose($"Dumping Special Orders");
                foreach (KeyValuePair<string, SpecialOrderData> d in SOData)
                {
                    ModFunctions.LogVerbose($"{d.Key}: {d.Value.Name}, {d.Value.Text}");
                }
            }
        }

        public bool CheckAll()
        {
            bool result = true;

            if (!QuestData.Keys.Contains(TempRefs.QuestAbi1))
            {
                ModFunctions.LogVerbose("Missing Quest 1");
                result = false;
            }
            if (!QuestData.Keys.Contains(TempRefs.QuestAbi2))
            {
                ModFunctions.LogVerbose("Missing Quest 2");
                result = false;
            }
            if (!QuestData.Keys.Contains(TempRefs.QuestAbi3))
            {
                ModFunctions.LogVerbose("Missing Quest 3");
                result = false;
            }
            if (!QuestData.Keys.Contains(TempRefs.QuestAbi4))
            {
                ModFunctions.LogVerbose("Missing Quest 4");
                result = false;
            }
            if (!QuestData.Keys.Contains(TempRefs.QuestMaru1))
            {
                ModFunctions.LogVerbose("Missing Quest 5");
                result = false;
            }
            if (!QuestData.Keys.Contains(TempRefs.QuestGeorge1))
            {
                ModFunctions.LogVerbose("Missing Quest 6");
                result = false;
            }
            if (!QuestData.Keys.Contains(TempRefs.QuestSeb1))
            {
                ModFunctions.LogVerbose("Missing Quest 7");
                result = false;
            }
            if (!QuestData.Keys.Contains(TempRefs.QuestSeb2))
            {
                ModFunctions.LogVerbose("Missing Quest 8");
                result = false;
            }
            return result;

        }
    }

}
