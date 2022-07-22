﻿using StardewModdingAPI;
using System.Collections.Generic;
using StardewValley.GameData;

namespace MilkVillagers.Asset_Editors
{

    public static class QuestEditor //: IAssetEditor
    {
        private static IDictionary<int, string> QuestData;
        private static IDictionary<string, SpecialOrderData> SOData;
        public static Dictionary<int, string> QuestMail = new Dictionary<int, string>()
        {
            [594801] = "AbiCarrotsT",   //Abigail quest 1
            [594802] = "AbiRadishesT",  //Abigail quest 2
            [594803] = "AbiEggplantT",  //Abigail quest 3
            [594804] = "AbiSurpriseT",  //Abigail quest 4

            [594805] = "",          //Elliott quest 1
            [594806] = "",          //Elliott quest 2
            [594807] = "",          //Elliott quest 3
            [594808] = "",          //Elliott quest 4

            [594809] = "",          //Sebastian quest 1
            [594810] = "",          //Sebastian quest 2
            [594811] = "",          //Sebastian quest 3
            [594812] = "",          //Sebastian quest 4

            [594813] = "MaruSampleT",          //Maru quest 1
            [594814] = "",          //Maru quest 2
            [594815] = "",          //Maru quest 3
            [594816] = "",          //Maru quest 4

            [594817] = "",          //Emily quest 1
            [594818] = "",          //Emily quest 2
            [594819] = "",          //Emily quest 3
            [594820] = "",          //Emily quest 4

            [594821] = "",          //Haley quest 1
            [594822] = "",          //Haley quest 2
            [594823] = "",          //Haley quest 3
            [594824] = "",          //Haley quest 4

            [594825] = "",          //Penny quest 1
            [594826] = "",          //Penny quest 2
            [594827] = "",          //Penny quest 3
            [594828] = "",          //Penny quest 4

            [594829] = "LeahNudePaintingT",          //Leah quest 1
            [594830] = "",          //Leah quest 2
            [594831] = "",          //Leah quest 3
            [594832] = "",          //Leah quest 4

            [594833] = "GeorgeMilkT",          //George quest 1
            [594834] = "",          //George quest 2
            [594835] = "",          //George quest 3
            [594836] = "",          //George quest 4

            [594837] = "",          //Harvey quest 1
            [594838] = "",          //Harvey quest 2
            [594839] = "HarveyCheckupT",          //Harvey quest 3
            [594840] = "HarveyTrialT",          //Harvey quest 4
        };

        public static Dictionary<string, int> QuestIDs = new Dictionary<string, int>()
        {
            ["QuestAbi1"] = 594801,      // Abigail's Carrot
            ["QuestAbi2"] = 594802,      // Abigail's Radishes
            ["QuestAbi3"] = 594803,      // Abigail's Eggplant
            ["QuestAbi4"] = 594804,      // Abigail's 'Helping hand'

            ["QuestElliott1"] = 594805,
            ["QuestElliott2"] = 594806,
            ["QuestElliott3"] = 594807,
            ["QuestElliott4"] = 594808,

            ["QuestSeb1"] = 594809,          // Curious tastes pt 1
            ["QuestSeb2"] = 594810,          // Curious tastes pt 2
            ["QuestSeb3"] = 594811,
            ["QuestSeb4"] = 594812,

            ["QuestMaru1"] = 594813,         // Scientific sample
            ["QuestMaru2"] = 594814,
            ["QuestMaru3"] = 594815,
            ["QuestMaru4"] = 594816,

            ["QuestEmily1"] = 594817,
            ["QuestEmily2"] = 594818,
            ["QuestEmily3"] = 594819,
            ["QuestEmily4"] = 594820,

            ["QuestHaley1"] = 594821,
            ["QuestHaley2"] = 594822,
            ["QuestHaley3"] = 594823,
            ["QuestHaley4"] = 594824,

            ["QuestPenny1"] = 594825,        // Penny check out book quest.
            ["QuestPenny2"] = 594826,        // Give Penny the book.
            ["QuestPenny3"] = 594827,
            ["QuestPenny4"] = 594828,

            ["QuestLeah1"] = 594829,         // Leah's Muse quest
            ["QuestLeah2"] = 594830,
            ["QuestLeah3"] = 594831,
            ["QuestLeah4"] = 594832,

            ["QuestGeorge1"] = 594833,       // Rejuvenating Milk
            ["QuestGeorge2"] = 594834,
            ["QuestGeorge3"] = 594835,
            ["QuestGeorge4"] = 594836,

            ["QuestHarvey1"] = 594837,
            ["QuestHarvey2"] = 594838,
            ["QuestHarvey3"] = 594839,
            ["QuestHarvey4"] = 594840,
        };

        public static bool CanEdit<T>(IAssetInfo asset)
        {
            bool result = asset.Name.IsEquivalentTo("Data/Quests")
                || asset.Name.IsEquivalentTo("Data/SpecialOrders");

            if (result)
                ModFunctions.LogVerbose(asset.Name.Name);

            return result;
        }

        public static bool CanEdit(IAssetName asset)
        {
            bool result = asset.IsEquivalentTo("Data/Quests")
                || asset.IsEquivalentTo("Data/SpecialOrders");

            if (result)
                ModFunctions.LogVerbose(asset.Name);

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
            if (asset.Name.IsEquivalentTo("Data/Quests"))
            {

                QuestData = asset.AsDictionary<int, string>().Data;
                //QuestData[QuestIDs["QuestName"]] = $"Type/Name/Description/Hint/Condition/Next Quest/Gold/Reward Description/Cancellable/Completion Text";

                #region Auto Generated
                QuestData[QuestIDs["QuestAbi1"]] = $"ItemDelivery/Abigail's Carrot/Abigail needs a cave carrot to scratch an itch. Bring her one to 'fill' a need/Bring Abigail a cave carrot/Abigail 78/-1/240/-1/FALSE/I hope you washed it! Those caves are wonderful, but the cave carrot needs to be SUPER clean before its going anywhere near my ass.";
                QuestData[QuestIDs["QuestAbi2"]] = $"ItemDelivery/Abigail's Radishes/Abigail wants some radishes for a new idea she had/Bring Abigail Radishes/Abigail 264 5/-1/355/-1/FALSE/I'm gonna have so much fun with these! How many do you think I can fit?";
                QuestData[QuestIDs["QuestAbi3"]] = $"ItemDelivery/Abigail's Eggplant/Abigail needs an eggplant for her cam show. Make sure it's a good one/Bring Abigail an eggplant./Abigail 272/-1/420/-1/FALSE/Wow, it's so big!. I'll be thinking of you tonight, @. Be sure to watch my show.";
                QuestData[QuestIDs["QuestAbi4"]] = $"Location/Abigail's 'helping hand'/Abigail wants you to help her with her show tonight. Go visit her at her house, and bring her an amethyst./Go to Abigail's house with an Amethyst/SeedShop/-1/500/-1/FALSE/I'm so glad you could come over and give me a helping 'hand'. My viewers are going to appreciate it as well...";

                QuestData[QuestIDs["QuestElliott1"]] = $"Basic/Writers Block/Elliott needs a distraction from writing. give him a BJ./Go to Elliott's house and see if you can help them relieve some frustration/null/-1/240/-1/FALSE/Thank you for that, @. It may not have cured my writers block, but it definitely helped me get rid of another kind of blockage.";
                QuestData[QuestIDs["QuestElliott2"]] = $"Location/Attentive Listener/Elliott wants to hear he someone else read a passage from his book. This may get steamy./Visit Elliott at their seaside cabin/ElliottHouse/-1/355/-1/FALSE/Completion Text";
                QuestData[QuestIDs["QuestElliott3"]] = $"Basic/Enter stage right/Elliott wants to try out a new scene from book///-1/420/-1/FALSE/Completion Text";
                QuestData[QuestIDs["QuestElliott4"]] = $"Location/Tell me everything you know'/Elliott roleplay 'interrogating the enemy captain'//ElliottHouse/-1/500/-1/FALSE/Completion Text";

                QuestData[QuestIDs["QuestSeb1"]] = $"ItemDelivery/Curious tastes pt 1/Sebastian wants to drink some of Abigail's milk, but is nervous of asking her directly/Bring Sebastian some of Abigail's Milk/Sebastian {TempRefs.MilkAbig}/-1/240/-1/FALSE/Wow, She was ok with it? Did you tell her who it was for? Oh...I guess that's only fair. I knew she was kind of into me, but this could be amazing!";
                QuestData[QuestIDs["QuestSeb2"]] = $"ItemDelivery/Curious tastes pt 2/Sebastian can have some of Abigail's milk, but only if she gets to taste his cum/Bring Abigail some of Sebastian's Cum/Abigail {TempRefs.MilkSeb}/-1/355/-1/FALSE/Did you suck it out of him, while looking into his eyes? Touching yourself as you brought him closer to orgasm? Did he cum all over your face, or were you tempted to swallow it instead? Anyway, thanks for that, cum buddy.";
                QuestData[QuestIDs["QuestSeb3"]] = $"Basic/Coding pipelines need clearing/Sebastian has been stuck in his room too much lately and needs some human interaction./Give Sebastian a pick me up BJ/null/-1/420/-1/FALSE/Completion Text";
                QuestData[QuestIDs["QuestSeb4"]] = $"Location/Purple and black desires/Seb special event with Abigail. Abigail and farmer peg Seb/Go visit Sebastian in his basement suite. Abigail will be waiting too./SebastianRoom/-1/500/-1/FALSE/Completion Text";

                QuestData[QuestIDs["QuestMaru1"]] = $"ItemDelivery/Scientific sample/Maru wants to test some cum/Bring Maru a sample of cum from a villager/Maru {TempRefs.CumType}/-1/240/-1/FALSE/Wow, I didn't expect you to bring it so quickly! It definitely looks right, and...it has that wonderful heady smell. I might post some request in the future, if you're interested.";
                QuestData[QuestIDs["QuestMaru2"]] = $"Basic/Straight from the source/Maru wants some right from the source//null/-1/355/-1/FALSE/Completion Text";
                QuestData[QuestIDs["QuestMaru3"]] = $"ItemDelivery/Sharing notes on potions/Maru wants you collect some essence from a magical creature so she can study how it is different./Bring Maru some distilled magical essence/Maru {TempRefs.SpecialType}/-1/420/-1/FALSE/Completion Text";
                QuestData[QuestIDs["QuestMaru4"]] = $"Basic/Lacking the human touch/Maru wants you to test out one of her inventions with her//null/-1/500/-1/FALSE/Completion Text";

                QuestData[QuestIDs["QuestEmily1"]] = $"LostItem/Emily's photoshoot/Emily needs someone to take some photos of her in a dress for an online commission, and she needs Haley's camera for it./Fetch Haley's camera for Emily and help her take some photos./Emily {TempRefs.HaleyCamera} HaleyHouse 5 4/-1/240/-1/FALSE/Thank you so much. I don't really want Haley to see the kind of clothes I have to make sometimes. She'd never let me live it down after the sundress incident last year.";
                QuestData[QuestIDs["QuestEmily2"]] = $"Location/Modelling surrogate/Follow up commission but this time  she wants a model because the comments were a little close to home///-1/355/-1/FALSE/Completion Text";
                QuestData[QuestIDs["QuestEmily3"]] = $"Basic/Show and Tell/Abigail has requested a special crotchless underwear set, and Emily uses this as an excuse to design a prototype set///-1/420/-1/FALSE/Completion Text";
                QuestData[QuestIDs["QuestEmily4"]] = $"Location/Barely contained pleasure/Emily is high and horny and wants your help to push her limits/Go wait at Emily's house before she does something silly/HaleyHouse/-1/500/-1/FALSE/Completion Text";

                QuestData[QuestIDs["QuestHaley1"]] = $"Location/Bringing out the inner beauty/Haley photography set - wants to do some artistic nudes of someone///-1/240/-1/FALSE/Completion Text";
                QuestData[QuestIDs["QuestHaley2"]] = $"Basic/Can't contain the power/Haley is feeling frustrated and swollen - help her relieve both pressures a the same time.///-1/355/-1/FALSE/Completion Text";
                QuestData[QuestIDs["QuestHaley3"]] = $"Basic/Milkers needed/Haley needs a volunteer villager for a milking scene - find a volunteer!///-1/420/-1/FALSE/Completion Text";
                QuestData[QuestIDs["QuestHaley4"]] = $"Location/Cover me with your love/bukkake milking scene?///-1/500/-1/FALSE/Completion Text";

                QuestData[QuestIDs["QuestPenny1"]] = $"Location/What's the matter with Penny?/Pam wants you to check up on Penny at the museum. Apparently she's been acting weird around Gunther./Go check on Penny at the museum./ArchaeologyHouse/{QuestEditor.QuestIDs["QuestPenny2"]}/240/-1/FALSE/No text - just event";
                QuestData[QuestIDs["QuestPenny2"]] = $"LostItem/Penny's book/Penny has asked you to bring her the smutty book./Delivey Boethia's pillow book to Penny./Penny {TempRefs.PennyBook} ArchaeologyHouse 21 5/-1/355/-1/FALSE/Thank you for doing that for me. I was so embarrassed";
                QuestData[QuestIDs["QuestPenny3"]] = $"Basic/Fifty shades of Penny/Penny wants to explore her sexual side, but needs a firmer hand to guide her///-1/420/-1/FALSE/Completion Text";
                QuestData[QuestIDs["QuestPenny4"]] = $"Location/Not all stories are found in books/sex in library, gets caught///-1/500/-1/FALSE/Completion Text";

                QuestData[QuestIDs["QuestLeah1"]] = $"Location/Leah pt 1/Leah has asked you to be a model for a painting she is working on. It is for an art competition, and the theme is 'The beautiful body'/Go to Leah's cabin and model for her/LeahHouse/-1/240/-1/FALSE/Thank you, @. I feel so confident about this painting. It really highlights your...I mean the beauty of the human body.";
                QuestData[QuestIDs["QuestLeah2"]] = $"Basic/Quest Name/Get special item from Robin? Need to look at addconversationtopic///-1/355/-1/FALSE/Completion Text";
                QuestData[QuestIDs["QuestLeah3"]] = $"Location/Tongue Tied/Leah wants you to go to an exhibit with her to get ideas, but it's a sex show/Meet up with Leah at her cabin/LeahHouse/-1/420/-1/FALSE/Completion Text";
                QuestData[QuestIDs["QuestLeah4"]] = $"Location/Bound by love/Leah needs a volunteer for an art exhibition, and wants you to model for her./Go to Leah's cabin and model for her/LeahHouse/-1/500/-1/FALSE/Completion Text";

                QuestData[QuestIDs["QuestGeorge1"]] = $"ItemDelivery/Rejuvenating Milk/George wants to taste some of Haley's and Emily's milk/Bring George some Special Milkshake/George {TempRefs.MilkShake}/-1/240/-1/FALSE/This is exactly what I need. I've been lusting after those two for so long, but Penny keeps getting in the way and blocking me any time I get a good eyeful.";
                QuestData[QuestIDs["QuestGeorge2"]] = $"LostItem/Ninja mission/George thinks you are a pervert, and wants you to grab him a pair of Haley's underwear 'as a trophy'./Bring George a pair of Haley's underwear/George {TempRefs.HaleyPanties} HaleyHouse 6 5/-1/355/-1/FALSE/Completion Text";
                QuestData[QuestIDs["QuestGeorge3"]] = $"ItemDelivery/Here cums the aeroplane/Dr Harvey needs your help convincing George to try some 'Super Juice'. This may take some creativity./Convince George to drink Super Juice/George {TempRefs.SuperJuice}/-1/420/-1/FALSE/Completion Text";
                QuestData[QuestIDs["QuestGeorge4"]] = $"Location/The Hugh Hefner achievement/You and Haley double blowjob for George/Meet up with Haley and go visit George/JoshHouse/-1/500/-1/FALSE/Completion Text";

                QuestData[QuestIDs["QuestHarvey1"]] = $"Basic/Bitter with age/Harvey wants you to collect a sample of George's cum for testing purposes//Harvey {TempRefs.MilkGeorge}/-1/240/-1/FALSE/Completion Text";
                QuestData[QuestIDs["QuestHarvey2"]] = $"LostItem/Jodi's medicine/Harvey wants you to take some 'medicine' to Jodi/Bring Jodi the special medicine/Jodi {TempRefs.SuperJuice} Hospital 2 3/-1/355/-1/FALSE/Oh, thank you for bringing that for me, @. Let me just see the instructions on it...#bOh! seems like I might need you to give me a hand with this one. It says that one of the side effects is producing too much milk of my own!";
                QuestData[QuestIDs["QuestHarvey3"]] = $"Basic/Excuse me, Dr?/Maru wants you to distract Harvey (blowjob)///-1/420/-1/FALSE/Completion Text";
                QuestData[QuestIDs["QuestHarvey4"]] = $"Location/Doctors & Nurses/Harvey & Maru event///-1/500/-1/FALSE/Completion Text";
                #endregion

            }
            if (asset.Name.IsEquivalentTo("Data/SpecialOrders"))
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
                                {"AcceptedContextTags", "human_cum_item"},
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
                                {"AcceptedContextTags", "item_special_milk/human_cum_item"},
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
                        new SpecialOrderObjectiveData() //this bit isn't working right now.
                        {
                            Type = "Collect",
                            Text = "Get 10 samples of Women's Milk",
                            RequiredCount = "10",
                            Data = new Dictionary<string, string>()
                            {
                                //{"AcceptedContextTags", "item_womans_milk/item_breast_milk/breast_milk_item"},
                                {"AcceptedContextTags", "breast_milk_item"},
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
                                {"AcceptedContextTags", "item_womans_milk/item_breast_milk/breast_milk_item"},
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

        public static bool ExportSpecialOrder(string ID)
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

        public static void Report()
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

        public static bool CheckAll()
        {
            bool result = true;

            foreach (KeyValuePair<string, int> kvp in QuestIDs)
            {
                if (!QuestData.Keys.Contains(kvp.Value))
                {
                    ModFunctions.LogVerbose($"Missing quest {kvp.Key}", LogLevel.Alert);
                    result = false;
                }
            }

            //if (!QuestData.Keys.Contains(TempRefs.QuestAbi1))
            //{
            //    ModFunctions.LogVerbose("Missing Quest 1");
            //    result = false;
            //}
            //if (!QuestData.Keys.Contains(TempRefs.QuestAbi2))
            //{
            //    ModFunctions.LogVerbose("Missing Quest 2");
            //    result = false;
            //}
            //if (!QuestData.Keys.Contains(TempRefs.QuestAbi3))
            //{
            //    ModFunctions.LogVerbose("Missing Quest 3");
            //    result = false;
            //}
            //if (!QuestData.Keys.Contains(TempRefs.QuestAbi4))
            //{
            //    ModFunctions.LogVerbose("Missing Quest 4");
            //    result = false;
            //}
            //if (!QuestData.Keys.Contains(TempRefs.QuestMaru1))
            //{
            //    ModFunctions.LogVerbose("Missing Quest 5");
            //    result = false;
            //}
            //if (!QuestData.Keys.Contains(TempRefs.QuestGeorge1))
            //{
            //    ModFunctions.LogVerbose("Missing Quest 6");
            //    result = false;
            //}
            //if (!QuestData.Keys.Contains(TempRefs.QuestSeb1))
            //{
            //    ModFunctions.LogVerbose("Missing Quest 7");
            //    result = false;
            //}
            //if (!QuestData.Keys.Contains(TempRefs.QuestSeb2))
            //{
            //    ModFunctions.LogVerbose("Missing Quest 8");
            //    result = false;
            //}

            return result;

        }
    }

}
