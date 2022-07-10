using Microsoft.Xna.Framework.Graphics;
using Netcode;
using StardewModdingAPI;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MilkVillagers
{
    public class ToolsLoader : IAssetEditor
    {
        private readonly Texture2D _toolsSpriteSheet;
        private readonly Texture2D _menuTilesSpriteSheet;
        private readonly Texture2D _customLetterBG;

        public ToolsLoader(
          Texture2D toolsSpriteSheet,
          Texture2D menuTilesSpriteSheet,
          Texture2D customLetterBG)
        {
            this._toolsSpriteSheet = toolsSpriteSheet;
            this._menuTilesSpriteSheet = menuTilesSpriteSheet;
            this._customLetterBG = customLetterBG;
        }

        public bool CanEdit<T>(IAssetInfo asset)
        {
            return asset.AssetNameEquals("TileSheets\\tools") || asset.AssetNameEquals("Maps\\MenuTiles");
        }

        public void Edit<T>(IAssetData asset)
        {
        }

    }

    [XmlInclude(typeof(SpellMilk))]
    [XmlInclude(typeof(SpellTeleport))]
    [Serializable]
    public class SpellTouch : StardewValley.Tools.MilkPail
    {
        public static int AttachmentMenuTile = 90;
        public int Range = 1;

        public override bool beginUsing(GameLocation location, int x, int y, Farmer who)
        {
            Game1.addHUDMessage(new HUDMessage(who.displayName + " started casting " + this.Name));

            return base.beginUsing(location, x, y, who);
        }

        private List<int[]> ValidCells(Farmer who)
        {
            int[] numArray = new int[2]
            {
        who.getTileX(),
        who.getTileY()
            };
            List<int[]> numArrayList = new List<int[]>();
            switch (who.FacingDirection)
            {
                case 0:
                    for (int index = 1; index <= this.Range; ++index)
                        numArrayList.Add(new int[2]
                        {
              numArray[0],
              numArray[1] - index
                        });
                    break;
                case 1:
                    for (int index = 1; index <= this.Range; ++index)
                        numArrayList.Add(new int[2]
                        {
              numArray[0] + index,
              numArray[1]
                        });
                    break;
                case 2:
                    for (int index = 1; index <= this.Range; ++index)
                        numArrayList.Add(new int[2]
                        {
              numArray[0],
              numArray[1] + index
                        });
                    break;
                case 3:
                    for (int index = 1; index <= this.Range; ++index)
                        numArrayList.Add(new int[2]
                        {
              numArray[0] - index,
              numArray[1]
                        });
                    break;
            }
            return numArrayList;
        }

        protected NPC CurrentTarget(GameLocation location, Farmer who)
        {
            foreach (int[] validCell in this.ValidCells(who))
            {
                using (List<NPC>.Enumerator enumerator = location.characters.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        NPC current = enumerator.Current;
                        if (validCell[0] == current.getTileX() && validCell[1] == current.getTileY())
                            return current;
                    }
                }
            }
            return (NPC)null;
        }

        protected override string loadDisplayName()
        {
            return this.Name ?? "";
        }

        protected override string loadDescription()
        {
            return string.Format("{0} Range {1}", (object)this.description, (object)this.Range);
        }

        public override Item getOne()
        {
            return (Item)new SpellTouch();
        }
    }

    public class SpellMilk : SpellTouch
    {
        public SpellMilk()
        {
            Name = "Milk";
            description = "Lets you milk villagers.";
            Range = 1;
            initialParentTileIndex.Value = 2;
            indexOfMenuItemView.Value = 2;
            Stackable = false;
            CurrentParentTileIndex = initialParentTileIndex;
            numAttachmentSlots.Value = 1;
            attachments.SetCount(numAttachmentSlots);
            Category = -99;
        }

        public override bool beginUsing(GameLocation location, int x, int y, Farmer who)
        {
            this.Update((int)who.facingDirection, 0, who);
            who.EndUsingTool();
            return true;
        }

        public override void DoFunction(GameLocation location, int x, int y, int power, Farmer who)
        {
            NPC npc = CurrentTarget(location, who);
            if (npc == null)
                return;
            if (Game1.player.getFriendshipHeartLevelForNPC(npc.Name) < 8)
                npc.addExtraDialogues("Hey there @. Wotcha doing with that pail?");
            if (TempRefs.milkedtoday.Contains(npc))
                return;
            npc.facePlayer(Game1.player);

            if (npc.Dialogue.TryGetValue("milk_start", out string dialogues))
            {
                Game1.addHUDMessage(new HUDMessage(string.Format("Starting milking process with {0}", npc.Name)));

                npc.addExtraDialogues(dialogues);
                npc.checkAction(Game1.player, Game1.currentLocation);
                TempRefs.milkedtoday.Add(npc);
            }
            else
            {
                if (npc.Gender != 1)
                    return;
                npc.addExtraDialogues("You want to milk me? Are you crazy...? Although, that DOES sound kinda hot. [174]");
                npc.checkAction(Game1.player, Game1.currentLocation);
                TempRefs.milkedtoday.Add(npc);
            }
        }
    }

    public class SpellTeleport : SpellSelf
    {
        public SpellTeleport()
        {
            this.Name = "Teleport Random";
            this.description = "Teleports you to a random location";
            this.Range = 12;
            this.InitialParentTileIndex = 0;
            this.IndexOfMenuItemView = 2;
            this.Stackable = false;
            this.CurrentParentTileIndex = this.InitialParentTileIndex;
            this.numAttachmentSlots.Value = 1;
            this.attachments.SetCount(this.numAttachmentSlots.Value);
            this.Category = -99;
        }

        public override void DoFunction(GameLocation location, int x, int y, int power, Farmer who)
        {
            base.DoFunction(location, x, y, power, who);
            bool nonWarpFade = Game1.nonWarpFade;
            int[] numArray = NewTarget(location, who, Range);
            Game1.nonWarpFade = true;
            Game1.warpFarmer(location.Name, numArray[0], numArray[1], false);
            Game1.nonWarpFade = nonWarpFade;
        }

        private int[] NewTarget(GameLocation location, Farmer who, int range)
        {
            int[] numArray1 = new int[2]
            {
                who.getTileX(),
                who.getTileY()
            };
            int[] numArray2 = new int[2];
            Random random = new Random();
            int num = 0;
            while (num < 5)
            {
                numArray2[0] = numArray1[0] + random.Next(-range, range);
                numArray2[1] = numArray1[1] + random.Next(-range, range);
                if (location.isTileOnMap(numArray2[0], numArray2[1]))
                    return numArray2;
            }
            return numArray1;
        }
    }

    public class SpellSelf : Tool
    {
        public static int AttachmentMenuTile = 91;
        public string SpellName = nameof(SpellName);
        public int Range;

        protected override string loadDisplayName()
        {
            return this.SpellName ?? "";
        }

        protected override string loadDescription()
        {
            return string.Format("{0} Range {1}", (object)this.description, (object)this.Range);
        }

        public override Item getOne()
        {
            return (Item)new SpellSelf();
        }
    }

}
