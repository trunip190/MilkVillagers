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
            data["MilkshakeCooking"] = "Hey @.^I've got a fun recipe for you try out. Just take some of my milk, add in crushed ice and blend it really well. I guarantee it will taste better than store bought icecream.   ^   -Emily %item cookingRecipe %%[#]Emily's Milkshake";
            data["CumshakeCooking"] = $"Hey @.^If you want a way of toning your muscles, this super secret recipe that gym's don't want you to know will really let you pile on the muscle.   ^   -Gus %item cookingRecipe %%[#]Gus's Protein Shake";
        }
    }


}
