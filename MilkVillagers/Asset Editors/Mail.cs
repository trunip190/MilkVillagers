using System.Collections.Generic;
using StardewModdingAPI;

namespace MilkVillagers
{
    public class MailEditor : IAssetEditor
    {
        IDictionary<string, string> data;
        public bool CanEdit<T>(IAssetInfo asset)
        {
            return asset.AssetNameEquals("Data\\mail");
        }

        public void Edit<T>(IAssetData asset)
        {
            data = asset.AsDictionary<string, string>().Data;

            // Mod tutorial mail.
            data["MilkButton1"] = $"@,^^ You can now milk villagers as well as cows using the 'O' button.^To milk a villager they must be an adult, have more than 8 hearts.^You can only milk them once a day, but be warned that once you start they may get addicted.   ^           -Anonymous.";
            data["MilkButton2"] = $"@,^^ I forgot to tell you that if your inventory is cluttered, there is a recipe that converts the different types of milk and cum into a base type to save space.   ^           -Anonymous.";


            // Quest giving mail.
            data["AbiMilking"] = $"Hey @,^^I have something embarrassing to tell you, but I need to do it in person.^When you have time can you come see me?^^PS. Make sure you bring your milk pail.   ^-Abigail.%item object {TempRefs.MilkAbig} 1 %%";

            // Quest 1
            data["AbiCarrots"] = $"Hey @,^^I need a cave carrot for my show, and Marlon won't let me in the mines yet. Preferably one that has been cleaned.   ^-Abigail%item quest {TempRefs.QuestAbi2} %%[#]Abigail's Carrot";
            data["AbiCarrotsT"] = $"I don't know where you found that cave carrot, but the next time you find one make sure you bring it to me! My viewers loved seeing it tickle me inside and out, and I have to admit I lvoed it too. Not my first choice, but here's hoping the next request is a little more normal.   ^-Your local camgirl"; 

             // Quest 2
             data["AbiRadishes"] = $"Hey @,^^They really loved the cave carrot, especially when it was scraping up my insides.^This time they want a string of radishes, so make sure there are a bunch of them, k?   ^-Abigail%item quest {TempRefs.QuestAbi3} %%[#]Abigail's Radishes";
            data["AbiRadishesT"] = $"I would NEVER have imagined that radishes could feel so good, but when I remembered that they came from your farm, I really got into them. Well, they certainly got into me. I had a whole bunch up there, and feeling them inside me was amazing.   ^-Your favourite camslut"; 

             // Quest 3
             data["AbiEggplant"] = $"Hey @,^^I need an eggplant for my next camshow, and I was hoping it would be one from YOUR farm.^Try to make it a challenge, ok?   ^-Abigail%item quest {TempRefs.QuestAbi1} %%[#]Abigail's Eggplant";
            data["AbiEggplantT"] = $"Hey there, @. You really came through for me, there. That eggplant felt sooo good rubbing up against my pussy, and when I tried to force it into my ass I came so hard. Shame it wouldn't fit. Oh well, I really hope you caught my show. Anyway, next time I need something smooth or bumpy, I know who to call on.   ^-The best camwhore";

            // Quest 4
            data["AbiSurprise"] = $"Hey @,^^I've had enough of sticking random veggies up my ass - I want to see how much of YOU I can fit in my pussy.^Make sure you come to my place tonight, ok? Clothing is optional. ;)   ^-Abigail%item quest {TempRefs.QuestAbi4} %%[#]Abigail's 'helping hand'";
            data["AbiSurpriseT"] = $"I got more views last night than ever before! You would not BELIEVE how many people watched you pounding my ass all night, and I got SO MANY tips. You are the best friend ever, @.   ^-Your friend with sexy benefits";

            // Quest 5
            data["MaruSample"] = $"Hello @,^^I know that you are as inquisitive and curious as I am, and I have some tests that I thought you might be interested in. For this next experiment, I need you to covertly collect a sample of semen from one of the residents of this town. Please bring it to me diretly.   ^-Maru.%item quest {TempRefs.QuestMaru1} %%[#]Maru's Sample";
            data["MaruSampleT"] = $"Hello fellow researcher!^^That sample you got for me was pretty good, but I think it was contaminated. I think I'm going to need to get my samples straight from the source in future, but thank you for your help.   ^-Maru.";

            // Quest 6
            data["GeorgeMilk"] = $"Morning, whippersnapper.^^My bones have been acting up lately, and 'Dr Harvey' said that I should be drinking healthy milk. The best milk is breast milk, but Evelyn is a little old for that, so I was thinking you could 'convince' either Emily or Haley to part with some of theirs and make me a milkshake? I doubt they'd let me get it straight from the source.^   -George.%item quest {TempRefs.QuestGeorge1} %%[#]George's Milk";
            data["GeorgeMilkT"] = $"Morning, whippersnapper.^^Evelyn said I should write you and thank you for the effort you went to, but I think you're just as much of a pervert as I am, so you got your reward by milking a couple of gorgeous women.^Anyway, thank you. I feel amazing and it's all own to you.   ^-George.";
        
            // Harvey final event
            data["HarveyCheckup"] = $"Greetings Farmer @.^^I was going through my records and I need to run some tests on you. It appears you are suffering from elevated levels and I want to make sure you are perfectly healthy. Pleae come see me during clinic hours. (But when Maru is not around).   ^-Your love doctor.";
            data["HarveyCheckupT"] = "Medical report^^Subject: @^Condition: Extremely arousing^Diagnoses:Too hot to handle^Prognosis:Will continue to cause erections and wet pussies wherever they go^Course of action prescribed: Continue visiting your local doctor for 'tests'.   ^-Your love doctor.";

            data["HarveyTrial"] = $"Greetings @.^^I've been contacted by the Ferngill department of health, and they have a new type of ointment they are conducting trials on. It is perfectly safe, but I was hoping you would be up for participating.^Please come by the clinic on a Tuesday or Thursday, as Nurse Maru will be assisting.   ^Dr Harvey, md";
            data["HarveyTrialT"] = $"Good morning @. I've attached a copy of my report.^^Medical trial: new drug report^Subject:@^Response to treatment:Patient experience immediate increase of bloodflow to labia, but no response found in breasts. Subject reported feeling heat in their genitals, and started producing lubricating fluids immediately. After manual stimulation the patient started producing copious amounts of fluid, and found it difficult resist orgasm.^   Dr Harvey, md";

            data["MagicalItem"] = $"Pay special attention, @.^You have come into possession of something very powerful, and need to be aware of this. The 'essence' that you acquired yesterday is from a creature that is not...wholly of this realm, and if concentrated can interrupt the current time. Once crafted, if you press {TempRefs.ActionKey} when you have some space around you, then you can rewind time by releasing the essence.^^Be Warned^   Qi";

            data["LeahNudePainting"] = $"Hey @.^I need a model for an art contest I'm entering. The theme is 'the beautiful body', and I immediately thought of you. Please come by my cottage when you have the time.^   Leah^^^PS. Clothing is optional.%item quest {TempRefs.QuestLeah1} %%[#]Leah's Muse";
            data["LeahNudePaintingT"] = $"Thank you so much for coming by my cottage yesterday. The paint is drying, and I can't wait to send it off to the competition. I'll let you know as soon as I hear back^   Leah";

            data["PennyLibrary"] = $"Wotcha, @.^^I'm not much for writing letters, but I don't want to go punching the wrong person and getting in trouble with Lewis and Gus. Anyway, Penny has always loved books, but yesterday when I got home she was all secretive about that darned book place. Can you go and see if something happened between Gunther and her?^^   Pam";

            // Event triggers
            data[$"5948MaruStart"] = $"Morning @,^^I have a new project that I'm working on, and I could really use your help...and discretion. If you have some time could you come to my house so I can show you what I've been working on?^   Maru";

            // Special order mail
            data["SOQiReward1"] = $"Greeting @,^^I am very impressed with your dedication to the tasks I set you, no matter what they are. I have enclosed a portion of magical essence; many creatures will produce this, and it has many properties you may find useful. Please experiment as you open your mind to new posibilities.^   Qi.%item object {TempRefs.MilkMagic} 1 %%";        
            data["SOQiReward2"] = $"greeting^^message.^   Qi.%item object {TempRefs.MilkMagic} 1 %%";

        }

        public void Report()
        {
            if (data == null) return;

            TempRefs.Monitor.Log("Dumping mail");
            foreach ( KeyValuePair<string, string> mail in data)
            {
                TempRefs.Monitor.Log($"{mail.Key}: {mail.Value}");
            }
        }
    }


}
