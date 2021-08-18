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
            data["MilkingAbi"] = string.Format("Hey @,^^I have something embarrassing to tell you, but I need to do it in person.^When you have time can you come see me?^^PS. Make sure you bring your milk pail.^^Abigail. %item object {0} 1 %%", (object)TempRefs.MilkAbig);
        }
    }


}
