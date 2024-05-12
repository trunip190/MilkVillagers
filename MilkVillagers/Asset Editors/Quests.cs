using StardewModdingAPI;
using System.Collections.Generic;
using StardewValley.GameData;
using StardewValley.Quests;
using StardewValley;
using static System.Net.Mime.MediaTypeNames;
using xTile.Dimensions;
using StardewValley.GameData.SpecialOrders;

namespace MilkVillagers.Asset_Editors
{

    public static class QuestEditor //: IAssetEditor
    {
        private static IDictionary<string, string> QuestData;
        private static IDictionary<string, SpecialOrderData> SOData;

        /// <summary>
        /// Which mail item to send when a quest is completed.
        /// </summary>
        public static Dictionary<int, string> QuestMail = new Dictionary<int, string>()
        {
            [594801] = "MTV_AbigailQ1T",    //Abigail quest 1
            [594802] = "MTV_AbigailQ2T",    //Abigail quest 2
            [594803] = "MTV_AbigailQ3T",    //Abigail quest 3
            //[594804] = "MTV_AbigailQ4T",    //Abigail quest 4

            [594805] = "MTV_ElliottQ1T",    //Elliott quest 1
            [594806] = "MTV_ElliottQ2T",    //Elliott quest 2
            [5948072] = "MTV_ElliottQ3T",    //Elliott quest 3
            [594808] = "MTV_ElliottQ4T",    //Elliott quest 4

            //[594809] = "",                  //Sebastian quest 1 pt 1
            [5948092] = "MTV_SebQ1T",   //Sebastian quest 1 pt 2
            //[594810] = "",              //Sebastian quest 2 pt 1
            [5948102] = "MTV_SebQ2T",    //Sebastian quest 2 pt 2
            [594811] = "MTV_SebQ3T",    //Sebastian quest 3
            [594812] = "MTV_SebQ4T",    //Sebastian quest 4

            [594813] = "MTV_MaruQ1T",       //Maru quest 1
            [594814] = "MTV_MaruQ2T",       //Maru quest 2
            [594815] = "MTV_MaruQ3T",       //Maru quest 3
            [594816] = "MTV_MaruQ4T",       //Maru quest 4

            [594817] = "MTV_EmilyQ1T",      //Emily quest 1
            [594818] = "MTV_EmilyQ2T",      //Emily quest 2
            [594819] = "MTV_EmilyQ3T",      //Emily quest 3
            [594820] = "MTV_EmilyQ4T",      //Emily quest 4

            [594821] = "MTV_HaleyQ1T",      //Haley quest 1
            [594822] = "MTV_HaleyQ2T",      //Haley quest 2
            [594823] = "MTV_HaleyQ3T",      //Haley quest 3
            [594824] = "MTV_HaleyQ4-2",     //Haley quest 4
            //[5948242] = "",

            [5948252] = "MTV_PennyQ1T",      //Penny quest 1
            [594826] = "MTV_PennyQ2T",      //Penny quest 2
            [5948272] = "MTV_PennyQ3T",      //Penny quest 3
            [594828] = "MTV_PennyQ4T",      //Penny quest 4

            [594829] = "MTV_LeahQ1T",       //Leah quest 1
            [594830] = "MTV_LeahQ2T",       //Leah quest 2
            [594831] = "MTV_LeahQ3T",       //Leah quest 3
            [594832] = "MTV_LeahQ4T",       //Leah quest 4

            [594833] = "MTV_GeorgeQ1T",     //George quest 1
            [594834] = "MTV_GeorgeQ2T",     //George quest 2
            [594835] = "MTV_GeorgeQ3T",     //George quest 3
            //[594836] = "",     //George quest 4
            [5948362] = "MTV_GeorgeQ4T",     //George quest 4

            //[594837] = "MTV_HarveyQ1T",     //Harvey quest 1
            [5948372] = "MTV_HarveyQ1T",     //Harvey quest 1
            [594838] = "MTV_HarveyQ2T",     //Harvey quest 2
            [594839] = "MTV_HarveyQ3T",     //Harvey quest 3
            [594840] = "MTV_HarveyQ4T",     //Harvey quest 4
        };

        /// <summary>
        /// questid's from quest names. need to make these explicit references in the code.
        /// </summary>
        public static Dictionary<string, int> QuestIDs = new Dictionary<string, int>()
        {
            ["QuestAbi1"] = 594801,
            ["QuestAbi2"] = 594802,
            ["QuestAbi3"] = 594803,
            ["QuestAbi4"] = 594804,

            ["QuestMaru1"] = 594813,
            ["QuestMaru1-2"] = 5948132,
            ["QuestMaru2"] = 594814,
            ["QuestMaru2-2"] = 5948142,
            ["QuestMaru3"] = 594815,
            ["QuestMaru4"] = 594816,

            ["QuestGeorge1"] = 594833,
            ["QuestGeorge2"] = 594834,
            ["QuestGeorge3"] = 594835,
            ["QuestGeorge4"] = 594836,
            ["QuestGeorge4-2"] = 5948362,

            ["QuestElliott1"] = 594805,
            ["QuestElliott2"] = 594806,
            ["QuestElliott3"] = 594807,
            ["QuestElliott4"] = 594808,

            ["QuestSeb1"] = 594809,
            ["QuestSeb1-2"] = 5948092,
            ["QuestSeb2"] = 594810,
            ["QuestSeb2-2"] = 5948102,
            ["QuestSeb3"] = 594811,
            ["QuestSeb4"] = 594812,

            ["QuestEmily1"] = 594817,
            ["QuestEmily2"] = 594818,
            ["QuestEmily3"] = 594819,
            ["QuestEmily4"] = 594820,

            ["QuestHaley1"] = 594821,
            ["QuestHaley2"] = 594822,
            ["QuestHaley2-2"] = 5948222,
            ["QuestHaley3"] = 594823,
            ["QuestHaley4"] = 594824,
            ["QuestHaley4-2"] = 5948242,

            ["QuestPenny1"] = 594825,
            ["QuestPenny1-2"] = 5948252,
            ["QuestPenny2"] = 594826,
            ["QuestPenny3"] = 594827,
            ["QuestPenny4"] = 594828,

            ["QuestLeah1"] = 594829,
            ["QuestLeah2"] = 594830,
            ["QuestLeah3"] = 594831,
            ["QuestLeah4"] = 594832,

            ["QuestHarvey1"] = 594837,
            ["QuestHarvey1-2"] = 5948372,
            ["QuestHarvey2"] = 594838,
            ["QuestHarvey2-2"] = 5948382,
            ["QuestHarvey3"] = 594839,
            ["QuestHarvey4"] = 594840,
        };

        public static bool CanEdit<T>(IAssetInfo asset)
        {
            bool result = asset.Name.IsEquivalentTo("Data/Quests")
                || asset.Name.IsEquivalentTo("Data/SpecialOrders");

            if (result)
                ModFunctions.Log(asset.Name.Name);

            return result;
        }

        public static bool CanEdit(IAssetName asset)
        {
            bool result = asset.IsEquivalentTo("Data/Quests")
                || asset.IsEquivalentTo("Data/SpecialOrders");

            // if (result) ModFunctions.LogVerbose(asset.Name);

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
                //ModFunctions.LogVerbose("Dumping quests", LogLevel.Trace, Force: true);
                //foreach (KeyValuePair<int, string> kvp in asset.AsDictionary<int, string>().Data)
                //{
                //    ModFunctions.LogVerbose($"{kvp.Key}: {kvp.Value}", LogLevel.Trace, Force: true);
                //}
                QuestData = asset.AsDictionary<string, string>().Data;
                //QuestData[QuestIDs["QuestName"]] = $"Type/Name/Description/Hint/Condition/Next Quest/Gold/Reward Description/Cancellable/Completion Text";

                #region Auto Generated - moved to CP
                //QuestData[594801] = $"ItemDelivery/Abigail's Carrot/Abigail needs a cave carrot to scratch an itch. Bring her one to 'fill' a need/Bring Abigail a cave carrot/Abigail 78/-1/240/-1/FALSE/I hope you washed it! Those caves are wonderful, but the cave carrot needs to be SUPER clean before its going anywhere near my ass.";
                //QuestData[594802] = $"ItemDelivery/Abigail's Radishes/Abigail wants some radishes for a new idea she had/Bring Abigail Radishes/Abigail 264 5/-1/355/-1/FALSE/I'm gonna have so much fun with these! How many do you think I can fit?";
                //QuestData[594803] = $"ItemDelivery/Abigail's Eggplant/Abigail needs an eggplant for her cam show. Make sure it's a good one/Bring Abigail an eggplant./Abigail 272/-1/420/-1/FALSE/Wow, it's so big!. I'll be thinking of you tonight, @. Be sure to watch my show.";
                //QuestData[594804] = $"Location/Abigail's 'helping hand'/Abigail wants you to help her with her show tonight. Go visit her at her house, and bring her an amethyst./Go to Abigail's house with an Amethyst/SeedShop/-1/500/-1/FALSE/I'm so glad you could come over and give me a helping 'hand'. My viewers are going to appreciate it as well...";

                //QuestData[594805] = $"Basic/Writers Block/Elliott needs a distraction from writing. give him a BJ./Go to Elliott's house and see if you can help them relieve some frustration/null/-1/240/-1/FALSE/Thank you for that, @. It may not have cured my writers block, but it definitely helped me get rid of another kind of blockage.";
                //QuestData[594806] = $"Location/Attentive Listener/Elliott wants to hear he someone else read a passage from his book. This may get steamy./Visit Elliott at their seaside cabin/ElliottHouse/-1/355/-1/FALSE/Completion Text";
                //QuestData[594807] = $"Basic/Enter stage right/Elliott wants to try out a new scene from book///-1/420/-1/FALSE/Completion Text";
                //QuestData[594808] = $"Location/Tell me everything you know'/Elliott roleplay 'interrogating the enemy captain'//ElliottHouse/-1/500/-1/FALSE/Completion Text";

                //QuestData[594809] = $"ItemDelivery/Curious tastes pt 1/Sebastian wants to drink some of Abigail's milk, but is nervous of asking her directly/Bring Sebastian some of Abigail's Milk/Sebastian {TempRefs.MilkAbig}/-1/240/-1/FALSE/Wow, She was ok with it? Did you tell her who it was for? Oh...I guess that's only fair. I knew she was kind of into me, but this could be amazing!";
                //QuestData[594810] = $"ItemDelivery/Curious tastes pt 2/Sebastian can have some of Abigail's milk, but only if she gets to taste his cum/Bring Abigail some of Sebastian's Cum/Abigail {TempRefs.MilkSeb}/-1/355/-1/FALSE/Did you suck it out of him, while looking into his eyes? Touching yourself as you brought him closer to orgasm? Did he cum all over your face, or were you tempted to swallow it instead? Anyway, thanks for that, cum buddy.";
                //QuestData[594811] = $"Basic/Coding pipelines need clearing/Sebastian has been stuck in his room too much lately and needs some human interaction./Give Sebastian a pick me up BJ/null/-1/420/-1/FALSE/Completion Text";
                //QuestData[594812] = $"Location/Purple and black desires/Seb special event with Abigail. Abigail and farmer peg Seb/Go visit Sebastian in his basement suite. Abigail will be waiting too./SebastianRoom/-1/500/-1/FALSE/Completion Text";

                //QuestData[594813] = $"ItemDelivery/Scientific sample/Maru wants to test some cum/Bring Maru a sample of cum from a villager/Maru {TempRefs.CumType}/-1/240/-1/FALSE/Wow, I didn't expect you to bring it so quickly! It definitely looks right, and...it has that wonderful heady smell. I might post some request in the future, if you're interested.";
                //QuestData[594814] = $"ItemDelivery/Sharing notes on potions/Maru wants you collect some essence from a magical creature so she can study how it is different./Bring Maru some distilled magical essence/Maru {TempRefs.SpecialType}/-1/355/-1/FALSE/It doesn't look anything like I expected. Did it take a lot of hard work to get this sample, @?";
                //QuestData[594815] = $"Basic/Please keep the doctor occupied/Doctor Harvey has been getting a little touchy with Maru and she wants you to distract him./Visit Dr. Harvey when he's at the clinic./null/-1/420/-1/FALSE/Completion Text";
                //QuestData[594816] = $"Basic/Lacking the human touch/Maru wants you to test out one of her inventions with her/Go visit Maru in the evening when she is at home./null/-1/500/-1/FALSE/Completion Text";

                //QuestData[594817] = $"LostItem/Emily's photoshoot/Emily needs someone to take some photos of her in a dress for an online commission, and she needs Haley's camera for it./Fetch Haley's camera for Emily and help her take some photos./Emily {TempRefs.HaleyCamera} HaleyHouse 1 9/-1/240/-1/FALSE/Thank you so much. I don't really want Haley to see the kind of clothes I have to make sometimes. She'd never let me live it down after the sundress incident last year.";
                //QuestData[594818] = $"Location/Modelling surrogate/Follow up commission but this time  she wants a model because the comments were a little close to home///-1/355/-1/FALSE/Completion Text";
                //QuestData[594819] = $"Basic/Show and Tell/Abigail has requested a special crotchless underwear set, and Emily uses this as an excuse to design a prototype set///-1/420/-1/FALSE/Completion Text";
                //QuestData[594820] = $"Location/Barely contained pleasure/Emily is high and horny and wants your help to push her limits/Go wait at Emily's house before she does something silly/HaleyHouse/-1/500/-1/FALSE/Completion Text";

                //QuestData[594821] = $"Location/Bringing out the inner beauty/Haley photography set - wants to do some artistic nudes of someone///-1/240/-1/FALSE/Completion Text";
                //QuestData[594822] = $"Basic/Can't contain the power/Haley is feeling frustrated and swollen - help her relieve both pressures a the same time.///-1/355/-1/FALSE/Completion Text";
                //QuestData[594823] = $"Basic/Milkers needed/Haley needs a volunteer villager for a milking scene - find a volunteer!///-1/420/-1/FALSE/Completion Text";
                //QuestData[594824] = $"Location/Cover me with your love/bukkake milking scene?///-1/500/-1/FALSE/Completion Text";

                //QuestData[594825] = $"Location/What's the matter with Penny?/Pam wants you to check up on Penny at the museum. Apparently she's been acting weird around Gunther./Go check on Penny at the museum./ArchaeologyHouse/{QuestIDs["QuestPenny2"]}/240/-1/FALSE/No text - just event";
                //QuestData[594826] = $"LostItem/Penny's book/Penny has asked you to bring her the smutty book./Deliver Boethia's pillow book to Penny./Penny {TempRefs.PennyBook} ArchaeologyHouse 21 5/-1/355/-1/FALSE/Thank you for doing that for me. I was so embarrassed. Maybe next time we can explore some of the ideas in the book?";
                //QuestData[594827] = $"Basic/Fifty shades of Penny/Penny wants to explore her sexual side, but needs a firmer hand to guide her///-1/420/-1/FALSE/Completion Text";
                //QuestData[594828] = $"Location/Not all stories are found in books/sex in library, gets caught///-1/500/-1/FALSE/Completion Text";

                //QuestData[594829] = $"Location/Leah pt 1/Leah has asked you to be a model for a painting she is working on. It is for an art competition, and the theme is 'The beautiful body'/Go to Leah's cabin and model for her/LeahHouse/-1/240/-1/FALSE/Thank you, @. I feel so confident about this painting. It really highlights your...I mean the beauty of the human body.";
                //QuestData[594830] = $"Basic/Quest Name/Get special item from Robin? Need to look at addconversationtopic///-1/355/-1/FALSE/Completion Text";
                //QuestData[594831] = $"Location/Tongue Tied/Leah wants you to go to an exhibit with her to get ideas, but it's a sex show/Meet up with Leah at her cabin/LeahHouse/-1/420/-1/FALSE/Completion Text";
                //QuestData[594832] = $"Location/Bound by love/Leah needs a volunteer for an art exhibition, and wants you to model for her./Go to Leah's cabin and model for her/LeahHouse/-1/500/-1/FALSE/Completion Text";

                //QuestData[594833] = $"ItemDelivery/Rejuvenating Milk/George wants to taste some of Haley's and Emily's milk/Bring George some 'Sweet Sibling' milkshake/George {TempRefs.SweetSibling}/-1/240/-1/FALSE/This is exactly what I need. I've been lusting after those two for so long, but Penny keeps getting in the way and blocking me any time I get a good eyeful.";
                //QuestData[594834] = $"LostItem/Ninja mission/George thinks you are a pervert, and wants you to grab him a pair of Haley's underwear 'as a trophy'./Bring George a pair of Haley's underwear/George {TempRefs.HaleyPanties} HaleyHouse 6 5/-1/355/-1/FALSE/I knew you had it in you. Let me just stash these down the side of my chair for later.";
                //QuestData[594835] = $"ItemDelivery/Here cums the aeroplane/Dr Harvey needs your help convincing George to try some 'Super Juice'. This may take some creativity./Convince George to drink Super Juice/George {TempRefs.SuperJuice}/-1/420/-1/FALSE/Completion Text";
                //QuestData[594836] = $"Location/The Hugh Hefner achievement/You and Haley double blowjob for George/Meet up with Haley and go visit George/JoshHouse/-1/500/-1/FALSE/Completion Text";

                //QuestData[594837] = $"Basic/Bitter with age/Harvey wants you to collect a sample of George's cum for testing purposes//Harvey {TempRefs.MilkGeorge}/-1/240/-1/FALSE/Completion Text";
                //QuestData[594838] = $"LostItem/Jodi's medicine/Harvey wants you to take some 'medicine' to Jodi/Bring Jodi the special medicine/Jodi {TempRefs.SuperJuice} Hospital 2 3/-1/355/-1/FALSE/Oh, thank you for bringing that for me, @. Let me just see the instructions on it...#bOh! seems like I might need you to give me a hand with this one. It says that one of the side effects is producing too much milk of my own!";
                //QuestData[594839] = $"Basic/Excuse me, Dr?/Maru wants you to distract Harvey (blowjob)///-1/420/-1/FALSE/Completion Text";
                //QuestData[594840] = $"Location/Doctors & Nurses/Harvey & Maru event///-1/500/-1/FALSE/Completion Text";
                #endregion

            }
            if (asset.Name.IsEquivalentTo("Data/SpecialOrders"))
            {
                SOData = asset.AsDictionary<string, SpecialOrderData>().Data;

                SpecialOrderData SOMilkMale = new SpecialOrderData()
                {
                    Name = "Vitality Collection",
                    Duration = QuestDuration.Week,
                    Requester = "Mister Qi",
                    Repeatable = true,
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
                                { "MailReceived", "MilkingProfCount" },
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
                    Duration = QuestDuration.Week,
                    Requester = "Mister Qi",
                    Repeatable = true,
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
                                { "MailReceived", "MilkingProfQuality" },
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

        public static void UpdateData(Dictionary<string, string> assetdata)
        {
            QuestData = assetdata;
            ModFunctions.Log("Updating QuestEditor: QuestData", LogLevel.Trace);
        }

        public static bool ExportSpecialOrder(string ID)
        {
            if (!SOData.ContainsKey(ID))
                return false;

            SpecialOrderData sso = SOData[ID];

            ModFunctions.Log($"Dumping data now", LogLevel.Alert);
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

            ModFunctions.Log(output);
            return true;
        }

        public static void Report(bool i18n = false, bool raw = false, bool special = false)
        {
            if (!special && QuestData != null)
            {
                ModFunctions.Log($"Dumping quests");
                foreach (KeyValuePair<string, string> d in QuestData)
                {
                    if (d.Key.ToString().Contains("5948"))
                    {
                        if (raw)
                        {
                            string parsed = d.Value.Replace("spacechase0.JsonAssets/ObjectId:", "spacechase0.JsonAssets#ObjectId:");
                            string[] split = parsed.Split('/');

                            if (i18n)
                            {
                                ModFunctions.Log($"\"Quests.{d.Key}.01\": \"{split[1]}\"", Force: true);
                                ModFunctions.Log($"\"Quests.{d.Key}.02\": \"{split[2]}\"", Force: true);
                                ModFunctions.Log($"\"Quests.{d.Key}.03\": \"{split[3]}\"", Force: true);
                                ModFunctions.Log($"\"Quests.{d.Key}.09\": \"{split[9]}\"", Force: true);
                            }

                            ModFunctions.Log($"{d.Key}: {split[0]}/{{{{i18n:Quests.{d.Key}.01}}}}/{{{{i18n:Quests.{d.Key}.02}}}}/{{{{i18n:Quests.{d.Key}.03}}}}/{split[4]}/{split[5]}" +
                                $"/{split[6]}/{split[7]}/{split[8]}/{{{{i18n:Quests.{d.Key}.09}}}}", Force: true);

                        }
                        else
                        {
                            string[] split = d.Value.Split('/');

                            ModFunctions.Log($"{d.Key}: {split[0]}/{split[1]}/{split[2]}/{split[3]}/{split[4]}/{split[5]}" +
                                $"/{split[6]}/{split[7]}/{split[8]}/{split[9]}", Force: true);
                        }
                    }
                }
            }
            if (special & SOData != null)
            {
                ModFunctions.Log($"Dumping Special Orders");
                foreach (KeyValuePair<string, SpecialOrderData> d in SOData)
                {
                    ModFunctions.Log($"{d.Key}: {d.Value.Name}, {d.Value.Text}");
                }
            }
        }

        public static bool CheckAll()
        {
            bool result = true;

            foreach (KeyValuePair<string, int> kvp in QuestIDs)
            {
                if (!QuestData.Keys.Contains(kvp.Value.ToString()))
                {
                    ModFunctions.Log($"Missing quest {kvp.Key}", LogLevel.Alert);
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
