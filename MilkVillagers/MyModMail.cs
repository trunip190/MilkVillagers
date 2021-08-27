using System.Collections.Generic;
using StardewModdingAPI;

namespace MilkVillagers
{
    public class MyModMail : IAssetEditor
    {
        public bool CanEdit<T>(IAssetInfo asset)
        {
            return asset.AssetNameEquals("Data\\mail");
        }

        public void Edit<T>(IAssetData asset)
        {
            IDictionary<string, string> data = asset.AsDictionary<string, string>().Data;
            // Mod tutorial mail.
            data["MilkButton1"] = $"@,^^ You can now milk villagers as well as cows using the 'O' button.^To milk a villager they must be an adult, have more than 8 hearts.^You can only milk them once a day, but be warned that once you start they may get addicted.   ^           -Anonymous.";
            data["MilkButton2"] = $"@,^^ I forgot to tell you that if your inventory is cluttered, there is a recipe that converts the different types of milk and cum into a base type to save space.   ^           -Anonymous.";


            // Quest giving mail.
            data["AbiMilking"] = $"Hey @,^^I have something embarrassing to tell you, but I need to do it in person.^When you have time can you come see me?^^PS. Make sure you bring your milk pail.   ^-Abigail.%item object {TempRefs.MilkAbig} 1 %%";

            // Quest 1
            data["AbiEggplant"] = $"Hey @,^^I need an eggplant for my next camshow, and I was hoping it would be one from YOUR farm.^Try to make it a challenge, ok?   ^-Abigail%item quest {TempRefs.QuestID1} %%[#]Abigail's Eggplant";
            data["AbiEggplantT"] = $"Hey there, @. You really came through for me, there. That eggplant felt sooo good rubbing up against my pussy, and when I tried to force it into my ass I came so hard. Shame it wouldn't fit. Oh well, I really hope you caught my show. Anyway, next time I need something smooth or bumpy, I know who to call on.   ^-Your local camgirl";

            // Quest 2
            data["AbiCarrots"] = $"Hey @,^^The eggplant was a huge hit, but they want something wierd for the next one.^Can you bring me a cave carrot? preferably one that has been cleaned.   ^-Abigail%item quest {TempRefs.QuestID2} %%[#]Abigail's Carrots";
            data["AbiCarrotsT"] = $"I don't know where you found that cave carrot, but the next time you find one make sure you bring it to me! My viewers loved seeing it tickle me inside and out, and I have to admit I lvoed it too. Not my first choice, but here's hoping the next request is a little more normal.   ^-Your favourite camslut";

            // Quest 3
            data["AbiRadishes"] = $"Hey @,^^They really loved the cave carrot, especially when it was scraping up my insides.^This time they want a string of radishes, so make sure there are a bunch of them, k?   ^-Abigail%item quest {TempRefs.QuestID3} %%[#]Abigail's Radishes";
            data["AbiRadishesT"] = $"I would NEVER have imagined that radishes could feel so good, but when I remembered that they came from your farm, I really got into them. Well, they certainly got into me. I had a whole bunch up there, and feeling them inside me was amazing.   ^-The best camwhore";

            // Quest 4
            data["AbiSurprise"] = $"Hey @,^^I've had enough of sticking random veggies up my ass - I want the real thing this time.^Make sure you come to my place tonight, ok? Clothing is optional. ;)   ^-Abigail%item quest {TempRefs.QuestID4} %%[#]Abigail's 'helping hand'";
            data["AbiSurpriseT"] = $"I got more views last night than ever before! You would not BELIEVE how many people watched you pounding my ass all night, and I got SO MANY tips. You are the best friend ever, @.   ^-Your friend with sexy benefits";

            // Quest 5
            data["MaruSample"] = $"Hello @,^^I know that you are as inquisitive and curious as I am, and I have some tests that I thought you might be interested in. For this next experiment, I need you to covertly collect a sample of semen from one of the residents of this town. Please bring it to me diretly.   ^-Maru.%item quest {TempRefs.QuestID5} %%[#]Maru's Sample";
            data["MaruSampleT"] = $"Hello fellow researcher!^^That sample you got for me was pretty good, but I think it was contaminated. I think I'm going to need to get my samples straight from the source in future, but thank you for your help.   ^-Maru.";

            // Quest 6
            data["GeorgeMilk"] = $"Morning, whippersnapper.^^My bones have been acting up lately, and 'Dr Harvey' said that I should be drinking healthy milk. The best milk is breast milk, but Evelyn is a little old for that, so I was thinking you could 'convince' either Emily or Haley to part with some of theirs and make me a milkshake? I doubt they'd let me get it straight from the source.   ^-George.%item quest {TempRefs.QuestID6} %%[#]George's Milk";
            data["GeorgeMilkT"] = $"Morning, whippersnapper.^^Evelyn said I should write you and thank you for the effort you went to, but I think you're just as much of a pervert as I am, so you got your reward by milking a couple of gorgeous women.^Anyway, thank you. I feel amazing and it's all own to you.   ^-George.";
        }
    }


}
