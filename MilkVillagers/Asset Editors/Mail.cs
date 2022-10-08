using System.Collections.Generic;
using System.Collections;
using StardewModdingAPI;

namespace MilkVillagers.Asset_Editors
{

    public static class MailEditor //: IAssetEditor
    {
        public static List<string> mail = new List<string>()
            {
                "MilkButton1",
                "MilkButton2",
                "AbiMilking",
                "AbiCarrots",
                "AbiCarrotsT",
                "AbiRadishes",
                "AbiRadishesT",
                "AbiEggplant",
                "AbiEggplantT",
                "AbiSurprise",
                "AbiSurpriseT",
                "MaruSample",
                "MaruSampleT",
                "GeorgeMilk",
                "GeorgeMilkT",
                "HarveyCheckup",
                "HarveyCheckupT",
                "HarveyTrial",
                "HarveyTrialT" ,
                "MagicalItem",
                "LeahNudePainting",
                "LeahNudePaintingT",
                "PennyLibrary",
                "PennyLibraryT",
                "MTV_PennyBathhouse",
                "MTV_PennyBathhouseT",
                "5948MaruStart",
                "SOQiReward1",
                "SOQiReward2"
            };

        private static Dictionary<string, string> MTVMail = new Dictionary<string, string>
        {
            // Gender mail
            {"MTV_Vagina" , "Your config has specified that you have a vagina, no penis, and are not Ace. This will be used for events. If you wish to change this, please edit your preferences in the config (this overrides your Stardew option)"},
            {"MTV_Penis" , "Your config has specified that you have a penis, no vagina, and are not Ace. This will be used for events. If you wish to change this, please edit your preferences in the config (this overrides your Stardew option)"},
            {"MTV_Ace" , "Your config has specified that you are Ace, and would prefer not to take part personally in sex. This will be used for events. If you wish to change this, please edit your preferences in the config (this overrides your Stardew option)"},
            {"MTV_Herm" , "Your config has specified that you have both a vagina and a penis, and are not Ace. This will be used for events. If you wish to change this, please edit your preferences in the config (this overrides your Stardew option)"},

             // Mod tutorial mail.
            {"MilkButton1" , $"@,^^ You can now milk villagers using the 'O' button.^To milk a villager they must be an adult and have more than 8 hearts.^You can only milk them once a day, but be warned that once you start they may get addicted.   ^           -Anonymous. %item object {TempRefs.ReadiMilk} 1 %%"},
            {"MilkButton2" , $"@,^^ I forgot to tell you that if your inventory is cluttered, there is a recipe that converts the different types of milk and cum into a base type to save space.^They can no longer be used in some special recipes though.^           -Anonymous."},
                        
            // Quest giving mail.
            {"AbiMilking" , $"Hey @,^^I have something embarrassing to tell you, but I need to do it in person.^When you have time can you come see me?^^PS. Make sure you bring your milk pail.   ^-Abigail. %item object {TempRefs.MilkAbig} 1 %%"},

            #region Abigail Quests
            // Quest 1
            {"AbiCarrots" , $"Hey @,^^I need a cave carrot for my show, and Marlon won't let me in the mines yet. Preferably one that has been cleaned.   ^-Abigail%item quest {QuestEditor.QuestIDs["QuestAbi1"]} %%"},
            {"AbiCarrotsT" , $"I don't know where you found that cave carrot, but the next time you find one make sure you bring it to me! My viewers loved seeing it tickle me inside and out, and I have to admit I loved it too. Not my first choice, but here's hoping the next request is a little more normal.   ^-Your local camgirl"},
            
            // Quest 2
            {"AbiRadishes" , $"Hey @,^^They really loved the cave carrot, especially when it was scraping up my insides.^This time they want a string of radishes, so make sure there are a bunch of them, k?   ^-Abigail %item quest {QuestEditor.QuestIDs["QuestAbi2"]} %%"},
            {"AbiRadishesT" , $"I would NEVER have imagined that radishes could feel so good, but when I remembered that they came from your farm, I really got into them. Well, they certainly got into me. I had a whole bunch up there, and feeling them inside me was amazing.   ^-Your favourite camslut"},
            
             // Quest 3
            {"AbiEggplant" , $"Hey @,^^I need an eggplant for my next camshow, and I was hoping it would be one from YOUR farm.^Try to make it a challenge, ok?   ^-Abigail %item quest {QuestEditor.QuestIDs["QuestAbi3"]} %%"},
            {"AbiEggplantT" , $"Hey there, @. You really came through for me, there. That eggplant felt sooo good rubbing up against my pussy, and when I tried to force it into my ass I came so hard. Shame it wouldn't fit. Oh well, I really hope you caught my show. Anyway, next time I need something smooth or bumpy, I know who to call on.   ^-The best camwhore"},
            
            // Quest 4
            {"AbiSurprise" , $"Hey @,^^I've had enough of sticking random veggies up my ass - I want to see how much of you I can fit in my pussy.^Make sure you come to my place tonight, ok? Clothing is optional. ;)   ^-Abigail%item quest {QuestEditor.QuestIDs["QuestAbi4"]} %%"},
            {"AbiSurpriseT" , $"I got more views last night than ever before! You would not BELIEVE how many people watched you pounding my ass all night, and I got SO MANY tips. You are the best friend ever, @.   ^-Your friend with sexy benefits"},
            #endregion

            #region Maru
            // Quest 1
            {"MaruSample" , $"Hello @,^^I know that you are as inquisitive and curious as I am, and I have some tests that I thought you might be interested in. For this next experiment, I need you to covertly collect a sample of semen from one of the residents of this town. Please bring it to me diretly.   ^-Maru. %item quest {QuestEditor.QuestIDs["QuestMaru1"]} %%"},
            {"MaruSampleT" , $"Hello fellow researcher!^^That sample you got for me was pretty good, but I think it was contaminated. I think I'm going to need to get my samples straight from the source in future, but thank you for your help.   ^-Maru."},

            // Quest 2
            {"MaruMagicEssence",$"That last sample was great, but now I want to study something more exotic. Can you bring me a sample from a magical or mystical creature? I want to run some experiments on it.^^    Maru. %item quest {QuestEditor.QuestIDs["QuestMaru2"]} %%" },
            {"MaruMagicEssenceT",$"I've already written pages and pages in my notebook on this liquid, so if you're interested then please come look through my notes. Anyway, I wanted to say thank you for all you've done. I'd love to hear about any special 'collection techniques' you have.^^     Maru" },
            
            // Quest 3
            {"MaruDrDistract", $"@,^^ I have a problem that I need your help with. Dr Harvey is getting a little obsessive, and I REALLY need someone to distract him from me. I've booked you in for a medical, so if you can help me out and I'd really appreciate it.^  Maru %item quest {QuestEditor.QuestIDs["QuestMaru3"]} %%" },
            {"MaruDrDistractT", $"I'm really sorry about yesterday, @. I know it wasn't the best way of handling Dr. Harvey, but I really appreciate what you did. I have been thinking about my experiments lately, and the doctor was so distracting that I made a bad call as your friend. He's really not so bad, and has been so sweet to me since your 'checkup'.^^    Maru^PS. I think I might have started him down a little bit of a perverted route. I hope this doesn't come back to bite us later." },

            // Quest 4
            {$"5948MaruStart" , $"Morning @,^^I have a new project that I'm working on, and I could really use your help...and discretion. If you have some time could you come to my house so I can show you what I've been working on?^   Maru"},
            #endregion

            #region George Quests
            // Quest 1
            {"GeorgeMilk" , $"Morning, whippersnapper.^^My bones have been acting up lately, and 'Dr Harvey' said that I should be drinking healthy milk. Ever since you gave me that Readi Milk I've been craving the best milk of all, breast milk. Evelyn is a little old for that, so I was thinking you could 'convince' either Emily or Haley to part with some of theirs and make me a healthy shake? I doubt they'd let me get it straight from the source.^   -George. %item quest {QuestEditor.QuestIDs["QuestGeorge1"]} %%"},
            {"GeorgeMilkT" , $"Good morning, whippersnapper.^^Evelyn said I should write you and thank you for the effort you went to, but I think you're just as much of a pervert as I am, so you got your reward by milking a couple of gorgeous women.^Anyway, thank you. I feel amazing and it's all down to you.   ^-George."},

            // Quest 2
            { "GeorgeMail2",$"Psst!^ Ever since you gave me that 'medicine' I've been feeling years younger. ^I'm still stuck in a wheelchair though, so I need you to be my legs. ^I know this is creepy, but I want you to get me pair of Haley's underwear. I don't care how you do it, but bring them here, and don't let Evelyn hear about it!^^    George. %item quest {QuestEditor.QuestIDs["QuestGeorge2"]} %% %item conversationTopic HaleyPanties 10 %%"},
            { "GeorgeMail2T",$"Quest 2 completed"},

            // Quest 3
            { "GeorgeMail3",$"%item quest {QuestEditor.QuestIDs["QuestGeorge3"]} %%"},
            { "GeorgeMail3T",$"Quest 3 completed"},

            // Quest 4
            { "GeorgeMail4",$"%item quest {QuestEditor.QuestIDs["QuestGeorge4"]} %%"},
            { "GeorgeMail4T",$"Quest 4 completed"},

            #endregion

            #region Harvey quests
            // Quest 1


            // Quest 2


            // Quest 3
            {"HarveyCheckup" , $"Greetings Farmer @.^^I was going through my records and I need to run some tests on you. It appears you are suffering from elevated levels and I want to make sure you are perfectly healthy. Pleae come see me during clinic hours. (But when Maru is not around).   ^-Your love doctor. %item quest {QuestEditor.QuestIDs["QuestHarvey3"]} %%"},
            {"HarveyCheckupT" , "Medical report^^Subject: @^Condition: Extremely arousing^Diagnoses:Too hot to handle^Prognosis:Will continue to cause erections and wet pussies wherever they go^Course of action prescribed: Continue visiting your local doctor for 'tests'.   ^-Your love doctor."},
            
            // Quest 4
            {"HarveyTrial" , $"Greetings @.^^I've been contacted by the Ferngill department of health, and they have a new type of ointment they are conducting trials on. It is perfectly safe, but I was hoping you would be up for participating.^Please come by the clinic on a Tuesday or Thursday, as Nurse Maru will be assisting.   ^Dr Harvey, md %item quest {QuestEditor.QuestIDs["QuestHarvey4"]} %%"},
            {"HarveyTrialT" , $"Good morning @. I've attached a copy of my report.^^Medical trial: new drug report^Subject:@^Response to treatment:Patient experience immediate increase of bloodflow to labia, but no response found in breasts. Subject reported feeling heat in their genitals, and started producing lubricating fluids immediately. After manual stimulation the patient started producing copious amounts of fluid, and found it difficult resist orgasm.^   Dr Harvey, MD"},
            #endregion

            {"MagicalItem" , $"Pay special attention, @.^You have come into possession of something very powerful, and need to be aware of this. The 'essence' that you acquired yesterday is from a creature that is not wholly of this realm, and if concentrated can interrupt the current time. Once crafted, if you press {TempRefs.ActionKey} when you have some space around you, then you can rewind time by releasing the essence.^^Be Warned^   Qi %item cookingRecipe %%"},


            #region Emily Quests
            // Quest 1
            {"EmilyPhotoShoot",$"Hi @,^I need someone to help me take some photos of a dress for a client, and Haley is being a brat right now. Can you grab her camera and come help me please?^^     -Emily %item quest {QuestEditor.QuestIDs["QuestEmily1"]} %%" },
            {"EmilyPhotoShootT",$"@,^Thank you so much for your help yesterday. I hope the client likes the photos you took of the dress, and I can send it off for them. I'm a little confused by the mailing address, as it seems to be a redirecting service.^Oh well, I know I did a good job on the dress, so thank you once again.^^    Emily" },

            // Quest 2
            {"EmilyBallgown", "@, ^I hope you can stop by my cottage today if you've got some time? I have a dress for a client that I would like you to try on so I can see how it is.^^      Emily" },
            #endregion

            #region Leah Quests
            // Leah Quest 1
            {"LeahNudePainting" , $"Hey @.^I need a model for an art contest I'm entering. The theme is 'the beautiful body', and I immediately thought of you. Please come by my cottage when you have the time.^   Leah^^^PS. Clothing is optional.%item quest {QuestEditor.QuestIDs["QuestLeah1"]} %%"},
            {"LeahNudePaintingT" , $"Thank you so much for coming by my cottage yesterday. The paint is drying, and I can't wait to send it off to the competition. I'll let you know as soon as I hear back^   Leah"},

            // Leah Quest 3
            {"LeahSexShowPt1", $"Can you come by the cottage when you have some time? There's an event coming up that I want to take you to see.%item quest {QuestEditor.QuestIDs["QuestLeah3"]} %%"},
            {"LeahSexShowPt1T", ""},
            #endregion

            #region Penny Quests
            {"PennyLibrary" , "Wotcha, @.^^I'm not much for writing letters, but I don't want to go punching the wrong person and getting in trouble with Lewis and Gus. Anyway, Penny has always loved books, but yesterday when I got home she was all secretive about that darned book place. Can you go and see if something happened between Gunther and her?^^   Pam"},

            {"MTV_PennyBathhouse","Psst.^Come by the bath house in the evenings. You MIGHT see something...special. (If you look in the windows)^^      -Anonymous.^^PS. Don't get caught!" },
            {"MTV_PennyBathhouseT","Hey there, @.^I don't know if you saw anything at the bath house last night, but I head someone russling around in the bushes out there while I was soaking last night.^   Penny.^^PS. If you want to join me next time, I'll be inside." },
            #endregion

            // Special order mail
            {"SOQiReward1" , $"Greeting @,^^I am very impressed with your dedication to the tasks I set you, no matter what they are. I have enclosed a portion of magical essence, many creatures will produce this, and it has many properties you may find useful. Please experiment as you open your mind to new posibilities.^   Qi.%item object {TempRefs.MilkMagic} 1 %%"},
            { "SOQiReward2" , $"greeting^^message.^   Qi.%item object {TempRefs.MilkMagic} 1 %%"},
        };

        public static Dictionary<string, string> FirstMail = new Dictionary<string, string>()
        {
            ["Abigail"] = "AbiCarrots",         //Abigail quest 1
            ["Elliott"] = "mtv_ElliottQ1",      //Elliott quest 1
            ["Sebastian"] = "",                 //Sebastian quest 1
            ["Maru"] = "MaruSample",            //Maru quest 1
            ["Emily"] = "EmilyPhotoShoot",      //Emily quest 1
            ["Haley"] = "",                     //Haley quest 1
            ["Penny"] = "PennyLibrary",         //Penny quest 1
            ["Leah"] = "LeahNudePainting",      //Leah quest 1
            ["George"] = "GeorgeMilk",          //George quest 1
            ["Harvey"] = "",                    //Harvey quest 1
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

            foreach (KeyValuePair<string, string> kvp in MTVMail)
            {
                if (!data.ContainsKey(kvp.Key))
                {
                    data[kvp.Key] = kvp.Value;

                }
            }
        }

        public static bool ContainsKey(string key)
        {
            return MTVMail.ContainsKey(key);
        }

        public static void Report()
        {
            if (data == null) return;

            TempRefs.Monitor.Log("Dumping mail");
            foreach (KeyValuePair<string, string> mail in data)
            {
                TempRefs.Monitor.Log($"{mail.Key}: {mail.Value}");
            }
        }
    }


}
