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
            IDictionary<string, string> data = ((IAssetData<IDictionary<string, string>>)asset.AsDictionary<string, string>()).Data;
            data["MilkButton1"] = "@,^^ You can now milk villagers as well as cows using the 'O' button.^To milk a villager they must be an adult woman, they must have more than 8 hearts, and you must have the special milk pail in your inventory.^You can only milk them once a day, but be warned that once you start they may get addicted.^^Anonymous.";
            data["MilkingAbi"] = $"Hey @,^^I have something embarrassing to tell you, but I need to do it in person.^When you have time can you come see me?^^PS. Make sure you bring your milk pail.^^Abigail. %item object {TempRefs.MilkAbig} 1 %%";
            data["AbiEggplant"] = $"Hey @,^^I need an eggplant for my next camshow, and I was hoping it would be one from YOUR farm.^Try to make it a challenge, ok?^ %item quest {TempRefs.QuestID1} %%[#]Abigail's Eggplant";
            data["AbiCarrots"] = $"Hey @,^^The eggplant was a huge hit, but they want something wierd for the next one.^Can you bring me a cave carrot? preferably one that has been cleaned.^ %item quest {TempRefs.QuestID2} %%[#]Abigail's Carrots";
            data["AbiRadishes"] = $"Hey @,^^They really loved the cave carrot, especially when it was scraping up my insides.^This time they want a string of radishes, so make sure there are a bunch of them, k?^ %item quest {TempRefs.QuestID3} %%[#]Abigail's Radishes";
            data["AbiSurprise"] = $"Hey @,^^I've had enough of sticking random veggies up my ass - I want the real thing this time.^Make sure you come to my place tonight, ok? Clothing is optional. ;)^ %item quest {TempRefs.QuestID4} %%[#]Abigail's 'helping hand'";

        }
    }


}
