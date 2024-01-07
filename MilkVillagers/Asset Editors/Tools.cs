using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Netcode;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MilkVillagers
{
    public class ToolsLoader //: IAssetEditor
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
            return asset.Name.IsEquivalentTo("TileSheets\\tools") || asset.Name.IsEquivalentTo("Maps\\MenuTiles");
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

    public class DebugWand : Tool
    {
        public bool charged;

        public DebugWand() : base("Return Scepter", 0, 2, 2, false, 0)
        {
            this.UpgradeLevel = 0;
            this.CurrentParentTileIndex = this.IndexOfMenuItemView;
            this.InstantUse = true;
        }

        protected override string loadDisplayName()
        {
            return Game1.content.LoadString("Strings\\StringsFromCSFiles:Wand.cs.14318"); //Just...no
        }

        protected override string loadDescription()
        {
            return Game1.content.LoadString("Strings\\StringsFromCSFiles:Wand.cs.14319"); //Also no.
        }

        public override void DoFunction(GameLocation location, int x, int y, int power, Farmer who)
        {
            if (who.bathingClothes.Value)
                return; //Can't warp while bathing.

            this.IndexOfMenuItemView = 2;
            this.CurrentParentTileIndex = 2;

            if (who.IsMainPlayer) //only main player can use the wand?
            {
                for (int index = 0; index < 12; ++index)
                    who.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(354, (float)Game1.random.Next(25, 75), 6, 1, new Vector2((float)Game1.random.Next((int)who.position.X - Game1.tileSize * 4, (int)who.position.X + Game1.tileSize * 3), (float)Game1.random.Next((int)who.position.Y - Game1.tileSize * 4, (int)who.position.Y + Game1.tileSize * 3)), false, Game1.random.NextDouble() < 0.5));
                Game1.playSound("wand");
                Game1.displayFarmer = false;
                Game1.player.Halt();
                Game1.player.faceDirection(2);
                Game1.player.freezePause = 1000;
                Game1.flashAlpha = 1f;
                DelayedAction.fadeAfterDelay(new Game1.afterFadeFunction(this.wandWarpForReal), 1000);
                new Rectangle(who.GetBoundingBox().X, who.GetBoundingBox().Y, Game1.tileSize, Game1.tileSize).Inflate(Game1.tileSize * 3, Game1.tileSize * 3);
                int num1 = 0;
                for (int index = who.getTileX() + 8; index >= who.getTileX() - 8; --index)
                {
                    List<TemporaryAnimatedSprite> temporarySprites = who.currentLocation.temporarySprites;
                    TemporaryAnimatedSprite temporaryAnimatedSprite = new TemporaryAnimatedSprite(6, new Vector2((float)index, (float)who.getTileY()) * (float)Game1.tileSize, Color.White, 8, false, 50f, 0, -1, -1f, -1, 0);
                    temporaryAnimatedSprite.layerDepth = 1f;
                    int num2 = num1 * 25;
                    temporaryAnimatedSprite.delayBeforeAnimationStart = num2;
                    Vector2 vector2 = new Vector2(-0.25f, 0.0f);
                    temporaryAnimatedSprite.motion = vector2;
                    temporarySprites.Add(temporaryAnimatedSprite);
                    ++num1;
                }
            }
            this.CurrentParentTileIndex = this.IndexOfMenuItemView;
        }

        public override bool actionWhenPurchased()
        {
            Game1.player.mailReceived.Add("ReturnScepter"); //Change this. :)
            return base.actionWhenPurchased();
        }

        private void wandWarpForReal()
        {
            Game1.warpFarmer("Farm", 64, 15, false);
            if (!Game1.isStartingToGetDarkOut())
                Game1.playMorningSong();
            else
                Game1.changeMusicTrack("none");
            Game1.fadeToBlackAlpha = 0.99f;
            Game1.screenGlow = false;
            Game1.player.temporarilyInvincible = false;
            Game1.player.temporaryInvincibilityTimer = 0;
            Game1.displayFarmer = true;
        }

        public override Item getOne()
        {
            throw new NotImplementedException();
        }


    }

    public class uObject : StardewValley.Object
    {
        [XmlIgnore]
        private readonly NetRef<LightSource> netLightSource = new NetRef<LightSource>();


        protected override void initNetFields()
        {
            base.NetFields.AddFields(tileLocation, owner, type, canBeSetDown, canBeGrabbed, isHoedirt, isSpawnedObject, questItem, questId, isOn, fragility, price, edibility, stack, quality, uses, bigCraftable, setOutdoors, setIndoors, readyForHarvest, showNextIndex, flipped, hasBeenPickedUpByFarmer, isRecipe, isLamp, heldObject, minutesUntilReady, boundingBox, preserve, preservedParentSheetIndex, honeyType, netLightSource, orderData, _destroyOvernight);
        }


    }

}
