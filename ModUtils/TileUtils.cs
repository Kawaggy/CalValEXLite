using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace CalValEXLite
{
    public abstract class FurnitureBase : ModTile
    {
        public readonly string FurnitureName;
        public readonly int FurnitureItem;
        public readonly Color FurnitureWorldColor;

        public FurnitureBase(string name, Color worldColor, int item)
        {
            FurnitureName = name;
            FurnitureItem = item;
            FurnitureWorldColor = worldColor;
        }

        public abstract void AddDefaultTileData();

        public sealed override void SetDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileLighted[Type] = true;
            Main.tileLavaDeath[Type] = true;

            AddDefaultTileData();
            TileObjectData.newTile.CoordinatePadding = 0;
            TileObjectData.addTile(Type);

            ModTranslation name = CreateMapEntryName();
            name.SetDefault(FurnitureName);
            AddMapEntry(FurnitureWorldColor, name);
        }

        public sealed override void KillMultiTile(int i, int j, int frameX, int frameY) => Item.NewItem(i * 16, j * 16, 48, 48, FurnitureItem);
        public sealed override void NumDust(int i, int j, bool fail, ref int num) => num = fail ? 1 : 3;
    }

    public abstract class ModPaintingTile : FurnitureBase
    {
        public ModPaintingTile(string paintingName, Color worldColor, int paintingItem) : base(paintingName, worldColor, paintingItem) { }

        public abstract void AddTileData();

        public sealed override void AddDefaultTileData()
        {
            TileID.Sets.FramesOnKillWall[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3Wall);
            AddTileData();
        }
    }

    public abstract class ModChair : FurnitureBase
    {
        public ModChair(string chairName, Color worldColor, int chairItem) : base(chairName, worldColor, chairItem) { }

        public virtual void AddTileData() { }

        public sealed override void AddDefaultTileData()
        {
            Main.tileNoAttach[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2);
            TileObjectData.newTile.CoordinateHeights = new[] { 16, 18 };
            TileObjectData.newTile.Direction = TileObjectDirection.PlaceLeft;
            TileObjectData.newTile.StyleWrapLimit = 2;
            TileObjectData.newTile.StyleMultiplier = 2;
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
            TileObjectData.newAlternate.Direction = TileObjectDirection.PlaceRight;
            TileObjectData.addAlternate(1);
            AddTileData();
            AddToArray(ref TileID.Sets.RoomNeeds.CountsAsChair);
            disableSmartCursor = true;
            adjTiles = new int[] { TileID.Chairs };
        }
    }

    public abstract class ModTableTile : FurnitureBase
    {
        public ModTableTile(string tableName, Color worldColor, int tableItem) : base(tableName, worldColor, tableItem) { }

        public virtual void AddTileData() { }

        public sealed override void AddDefaultTileData()
        {
            Main.tileSolidTop[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileTable[Type] = true;
            Main.tileWaterDeath[Type] = false;
            Main.tileLavaDeath[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
            TileObjectData.newTile.LavaDeath = true;
            AddTileData();
            AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTable);
            adjTiles = new int[] { TileID.Tables };
        }
    }

    public abstract class ModBookcaseTile : FurnitureBase
    {
        public ModBookcaseTile(string bookcaseName, Color worldColor, int bookcaseItem) : base(bookcaseName, worldColor, bookcaseItem) { }
        
        public virtual void AddTileData() { }
        public sealed override void AddDefaultTileData()
        {
            Main.tileSolidTop[Type] = SolidTop;
            Main.tileNoAttach[Type] = true;
            Main.tileTable[Type] = SolidTop;
            Main.tileWaterDeath[Type] = false;
            Main.tileLavaDeath[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x4);
            TileObjectData.newTile.LavaDeath = true;
            AddTileData();
            AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTable);
            adjTiles = new int[] { TileID.Bookcases };
        }

        public virtual bool SolidTop => true;
    }

    public abstract class WideTileBase : FurnitureBase
    {
        public WideTileBase(string name, Color worldColor, int item) : base(name, worldColor, item) { }

        public abstract void AddDefaultTileData2();
        public sealed override void AddDefaultTileData()
        {
            TileObjectData.newTile.CopyFrom(TileObjectData.Style4x2);
            TileObjectData.newTile.CoordinateHeights = new[] { 16, 18 };
            disableSmartCursor = true;
            AddDefaultTileData2();
        }
    }

    public abstract class ModBedTile : WideTileBase
    {
        public ModBedTile(string bedName, Color worldColor, int bedItem) : base(bedName, worldColor, bedItem) { }

        public virtual void AddTileData() { }
        public override void AddDefaultTileData2()
        {
            Main.tileLighted[Type] = false;
            TileID.Sets.HasOutlines[Type] = true;
            adjTiles = new int[] { TileID.Beds };
            bed = true;
            AddToArray(ref TileID.Sets.RoomNeeds.CountsAsChair);
            AddTileData();
        }

        public sealed override bool HasSmartInteract() => true;

        public virtual bool CustomBedBehaviour(int i, int j) => true;
        public sealed override bool NewRightClick(int i, int j)
        {
            Player player = Main.LocalPlayer;
            Tile tile = Main.tile[i, j];
            int spawnX = i - tile.frameX / 18;
            int spawnY = j + 2;
            spawnX += tile.frameX >= 72 ? 5 : 2;
            if(tile.frameY % 38 != 0)
            {
                spawnY--;
            }
            player.FindSpawn();
            if (player.SpawnX == spawnX && player.SpawnY == spawnY)
            {
                player.RemoveSpawn();
                Main.NewText("Spawn point removed!", 255, 240, 20, false);
            }
            else if(Player.CheckSpawn(spawnX, spawnY))
            {
                player.ChangeSpawn(spawnX, spawnY);
                Main.NewText("Spawn point set!", 255, 240, 20, false);
            }
            return CustomBedBehaviour(i, j);
        }

        public sealed override void MouseOver(int i, int j)
        {
            Player player = Main.LocalPlayer;
            player.noThrow = 2;
            player.showItemIcon = true;
            player.showItemIcon2 = FurnitureItem;
        }
    }

    public abstract class ModBathtubTile : WideTileBase
    {
        public ModBathtubTile(string bathtubName, Color worldColor, int bathtubItem) : base(bathtubName, worldColor, bathtubItem) { }

        public virtual void AddTileData() { }
        public override void AddDefaultTileData2()
        {
            adjTiles = new int[] { TileID.Bathtubs };
            AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTable);
            AddTileData();
        }
    }

    public abstract class ModCandelabraTile : FurnitureBase
    {
        private readonly string FlamePath;
        public ModCandelabraTile(string candelabraName, Color worldColor, int candelabraItem, string flamePath) : base(candelabraName, worldColor, candelabraItem)
        {
            FlamePath = flamePath;
        }

        public virtual void AddTileData() { }
        public override void AddDefaultTileData()
        {
            Main.tileLavaDeath[Type] = true;
            Main.tileWaterDeath[Type] = false;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
            TileObjectData.newTile.LavaDeath = false;
            TileObjectData.addTile(Type);
            disableSmartCursor = true;
            adjTiles = new int[] { TileID.Candelabras };
            AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTorch);
            AddTileData();
        }

        /// <summary>
        /// Called before the flame is drawn
        /// </summary>
        public virtual void SafePostDraw(int i, int j, SpriteBatch spriteBatch, Color drawColor, Vector2 position, Rectangle sourceRectangle) { }
        public sealed override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            int frameX = Main.tile[i, j].frameX;
            int frameY = Main.tile[i, j].frameY;
            Texture2D flameTexture = ModContent.GetTexture(FlamePath);
            Color drawColor = DrawColor(i, j, new Color(100, 100, 100));
            Vector2 offset = Main.drawToScreen ? Vector2.Zero : new Vector2(Main.offScreenRange);
            Vector2 position = new Vector2(i * 16, j * 16) - Main.screenPosition + offset;
            Rectangle sourceRectangle = new Rectangle(frameX, frameY, 18, 18);
            SafePostDraw(i, j, spriteBatch, drawColor, position, sourceRectangle);
            spriteBatch.Draw(flameTexture, position, sourceRectangle, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        }

        internal Color DrawColor(int i, int j, Color color)
        {
            int tileColor = Main.tile[i, j].color();
            Color colour = WorldGen.paintColor(tileColor);
            if (tileColor >= 13 && tileColor <= 24)
            {
                color.R = (byte)(colour.R / 255f * color.R);
                color.G = (byte)(colour.G / 255f * color.G);
                color.B = (byte)(colour.B / 255f * color.B);
            }
            return color;
        }

        public virtual Color LightColor => new Color(100, 100, 100);

        public sealed override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            if (Main.tile[i, j].frameX < 18)
            {
                r = LightColor.R / 255;
                g = LightColor.G / 255;
                b = LightColor.B / 255;
                return;
            }
            r = 0;
            g = 0;
            b = 0;
        }

        public sealed override void HitWire(int i, int j) => TileUtils.LightHitWire(i, j, LightHitWireID.Candelabra);
    }

    public abstract class ModChest : ModTile
    {
        private readonly string ChestName;
        private readonly int ChestItem;
        private readonly Color WorldColor;
        private readonly bool HasLockedState;
        public ModChest(string chestName, Color worldColor, int chestItem, bool hasLockedState = false)
        {
            ChestName = chestName;
            ChestItem = chestItem;
            WorldColor = worldColor;
            HasLockedState = hasLockedState;
        }

        public virtual short TileValue => 500;
        public virtual int TileShine => 1200;
        public virtual int ChestKey => ItemID.GoldenKey;

        public virtual void AddTileData() { }
        public sealed override void SetDefaults()
        {
            Main.tileSpelunker[Type] = true;
            Main.tileContainer[Type] = true;
            Main.tileShine2[Type] = true;
            Main.tileShine[Type] = TileShine;
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileValue[Type] = TileValue;
            TileID.Sets.HasOutlines[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
            TileObjectData.newTile.Origin = new Point16(0, 1);
            TileObjectData.newTile.CoordinateHeights = new[] { 16, 18 };
            TileObjectData.newTile.HookCheck = new PlacementHook(new Func<int, int, int, int, int, int>(Chest.FindEmptyChest), -1, 0, true);
            TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(new Func<int, int, int, int, int, int>(Chest.AfterPlacement_Hook), -1, 0, false);
            TileObjectData.newTile.AnchorInvalidTiles = new[] { 127 };
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.LavaDeath = false;
            TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
            TileObjectData.newTile.CoordinatePadding = 0;
            AddTileData();
            TileObjectData.addTile(Type);
            ModTranslation name = CreateMapEntryName();
            name.SetDefault(ChestName);
            AddMapEntry(WorldColor, name, MapChestName);
            name = CreateMapEntryName(Name + "_Locked");
            name.SetDefault("Locked " + ChestName);
            AddMapEntry(new Color(WorldColor.R / 2f, WorldColor.G / 2f, WorldColor.B / 2f), name, MapChestName);
            disableSmartCursor = true;
            adjTiles = new int[] { TileID.Containers };
            chest = ChestName;
            chestDrop = ChestItem;
        }

        public sealed override ushort GetMapOption(int i, int j)
        {
            if (HasLockedState)
                return (ushort)(Main.tile[i, j].frameX / 36);
            return 0;
        }

        public sealed override bool HasSmartInteract() => true;

        public sealed override bool IsLockedChest(int i, int j)
        {
            if (HasLockedState)
                return Main.tile[i, j].frameX / 36 == 1;
            return false;
        }

        public abstract bool CanUnlockChest();
        public sealed override bool UnlockChest(int i, int j, ref short frameXAdjustment, ref int dustType, ref bool manual) => CanUnlockChest();

        private string MapChestName(string name, int i, int j)
        {
            int left = i;
            int top = j;
            Tile tile = Main.tile[i, j];
            if (tile.frameX % 36 != 0)
            {
                left--;
            }
            if (tile.frameY != 0)
            {
                top--;
            }
            int chest = Chest.FindChest(left, top);
            if (chest < 0)
            {
                return Language.GetTextValue("LegacyChestType.0");
            }
            else if (Main.chest[chest].name == "")
            {
                return name;
            }
            else
            {
                return name + ": " + Main.chest[chest].name;
            }
        }

        public sealed override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Item.NewItem(i * 16, j * 16, 32, 32, chestDrop);
            Chest.DestroyChest(i, j);
        }

        public sealed override bool NewRightClick(int i, int j)
        {
            Player player = Main.LocalPlayer;
            Tile tile = Main.tile[i, j];
            Main.mouseRightRelease = false;
            int left = i;
            int top = j;
            if (tile.frameX % 36 != 0)
            {
                left--;
            }
            if (tile.frameY != 0)
            {
                top--;
            }
            if (player.sign >= 0)
            {
                Main.PlaySound(SoundID.MenuClose);
                player.sign = -1;
                Main.editSign = false;
                Main.npcChatText = "";
            }
            if (Main.editChest)
            {
                Main.PlaySound(SoundID.MenuTick);
                Main.editChest = false;
                Main.npcChatText = "";
            }
            if (player.editedChestName)
            {
                NetMessage.SendData(MessageID.SyncPlayerChest, -1, -1, NetworkText.FromLiteral(Main.chest[player.chest].name), player.chest, 1f, 0f, 0f, 0, 0, 0);
                player.editedChestName = false;
            }
            bool isLocked = IsLockedChest(left, top);
            if (Main.netMode == NetmodeID.MultiplayerClient && !isLocked)
            {
                if (left == player.chestX && top == player.chestY && player.chest >= 0)
                {
                    player.chest = -1;
                    Recipe.FindRecipes();
                    Main.PlaySound(SoundID.MenuClose);
                }
                else
                {
                    NetMessage.SendData(MessageID.RequestChestOpen, -1, -1, null, left, (float)top, 0f, 0f, 0, 0, 0);
                    Main.stackSplit = 600;
                }
            }
            else
            {
                if (isLocked)
                {
                    int key = ChestKey;
                    if (player.ConsumeItem(key) && Chest.Unlock(left, top))
                    {
                        if (Main.netMode == NetmodeID.MultiplayerClient)
                        {
                            NetMessage.SendData(MessageID.Unlock, -1, -1, null, player.whoAmI, 1f, (float)left, (float)top);
                        }
                    }
                }
                else
                {
                    int chest = Chest.FindChest(left, top);
                    if (chest >= 0)
                    {
                        Main.stackSplit = 600;
                        if (chest == player.chest)
                        {
                            player.chest = -1;
                            Main.PlaySound(SoundID.MenuClose);
                        }
                        else
                        {
                            player.chest = chest;
                            Main.playerInventory = true;
                            Main.recBigList = false;
                            player.chestX = left;
                            player.chestY = top;
                            Main.PlaySound(player.chest < 0 ? SoundID.MenuOpen : SoundID.MenuTick);
                        }
                        Recipe.FindRecipes();
                    }
                }
            }
            return true;
        }

        public sealed override void MouseOver(int i, int j)
        {
            Player player = Main.LocalPlayer;
            Tile tile = Main.tile[i, j];
            int left = i;
            int top = j;
            if (tile.frameX % 36 != 0)
            {
                left--;
            }
            if (tile.frameY != 0)
            {
                top--;
            }
            int chest = Chest.FindChest(left, top);
            player.showItemIcon2 = -1;
            if (chest < 0)
            {
                player.showItemIconText = Language.GetTextValue("LegacyChestType.0");
            }
            else
            {
                player.showItemIconText = Main.chest[chest].name.Length > 0 ? Main.chest[chest].name : ChestName;
                if (player.showItemIconText == ChestName)
                {
                    player.showItemIcon2 = ChestItem;
                    if (Main.tile[left, top].frameX / 36 == 1)
                        player.showItemIcon2 = ChestKey;
                    player.showItemIconText = "";
                }
            }
            player.noThrow = 2;
            player.showItemIcon = true;
        }

        public sealed override void MouseOverFar(int i, int j)
        {
            MouseOver(i, j);
            Player player = Main.LocalPlayer;
            if (player.showItemIconText == "")
            {
                player.showItemIcon = false;
                player.showItemIcon2 = 0;
            }
        }
    }
    
    public abstract class ModDresser : ModTile
    {
        private readonly string DresserName;
        private readonly int DresserItem;
        private readonly Color WorldColor;
        public ModDresser(string dresserName, Color worldColor, int dresserItem)
        {
            DresserName = dresserName;
            DresserItem = dresserItem;
            WorldColor = worldColor;
        }

        public virtual void AddTileData() { }
        public sealed override void SetDefaults()
        {
            Main.tileSolidTop[Type] = true;
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileTable[Type] = true;
            Main.tileContainer[Type] = true;
            Main.tileLavaDeath[Type] = true;
            TileID.Sets.HasOutlines[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
            TileObjectData.newTile.Origin = new Point16(1, 1);
            TileObjectData.newTile.CoordinateHeights = new[] { 16, 16 };
            TileObjectData.newTile.HookCheck = new PlacementHook(new Func<int, int, int, int, int, int>(Chest.FindEmptyChest), -1, 0, true);
            TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(new Func<int, int, int, int, int, int>(Chest.AfterPlacement_Hook), -1, 0, false);
            TileObjectData.newTile.AnchorInvalidTiles = new[] { 127 };
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.LavaDeath = false;
            TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
            AddTileData();
            TileObjectData.addTile(Type);
            AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTable);
            ModTranslation name = CreateMapEntryName();
            name.SetDefault(DresserName);
            AddMapEntry(WorldColor, name);
            disableSmartCursor = true;
            adjTiles = new int[] { TileID.Dressers };
            dresser = DresserName;
            dresserDrop = DresserItem;
        }

        public override bool HasSmartInteract() => true;

        public override bool NewRightClick(int i, int j)
        {
            Player player = Main.LocalPlayer;
            if (Main.tile[Player.tileTargetX, Player.tileTargetY].frameY == 0)
            {
                Main.CancelClothesWindow(true);
                Main.mouseRightRelease = false;
                int left = (int)(Main.tile[Player.tileTargetX, Player.tileTargetY].frameX / 18);
                left %= 3;
                left = Player.tileTargetX - left;
                int top = Player.tileTargetY - (int)(Main.tile[Player.tileTargetX, Player.tileTargetY].frameY / 18);
                if (player.sign > -1)
                {
                    Main.PlaySound(SoundID.MenuClose);
                    player.sign = -1;
                    Main.editSign = false;
                    Main.npcChatText = string.Empty;
                }
                if (Main.editChest)
                {
                    Main.PlaySound(SoundID.MenuTick);
                    Main.editChest = false;
                    Main.npcChatText = string.Empty;
                }
                if (player.editedChestName)
                {
                    NetMessage.SendData(MessageID.SyncPlayerChest, -1, -1, NetworkText.FromLiteral(Main.chest[player.chest].name), player.chest, 1f, 0f, 0f, 0, 0, 0);
                    player.editedChestName = false;
                }
                if (Main.netMode == NetmodeID.MultiplayerClient)
                {
                    if (left == player.chestX && top == player.chestY && player.chest != -1)
                    {
                        player.chest = -1;
                        Recipe.FindRecipes();
                        Main.PlaySound(SoundID.MenuClose);
                    }
                    else
                    {
                        NetMessage.SendData(MessageID.RequestChestOpen, -1, -1, null, left, (float)top, 0f, 0f, 0, 0, 0);
                        Main.stackSplit = 600;
                    }
                }
                else
                {
                    player.flyingPigChest = -1;
                    int num213 = Chest.FindChest(left, top);
                    if (num213 != -1)
                    {
                        Main.stackSplit = 600;
                        if (num213 == player.chest)
                        {
                            player.chest = -1;
                            Recipe.FindRecipes();
                            Main.PlaySound(SoundID.MenuClose);
                        }
                        else if (num213 != player.chest && player.chest == -1)
                        {
                            player.chest = num213;
                            Main.playerInventory = true;
                            Main.recBigList = false;
                            Main.PlaySound(SoundID.MenuOpen);
                            player.chestX = left;
                            player.chestY = top;
                        }
                        else
                        {
                            player.chest = num213;
                            Main.playerInventory = true;
                            Main.recBigList = false;
                            Main.PlaySound(SoundID.MenuTick);
                            player.chestX = left;
                            player.chestY = top;
                        }
                        Recipe.FindRecipes();
                    }
                }
            }
            else
            {
                Main.playerInventory = false;
                player.chest = -1;
                Recipe.FindRecipes();
                Main.dresserX = Player.tileTargetX;
                Main.dresserY = Player.tileTargetY;
                Main.OpenClothesWindow();
            }
            return true;
        }

        public override void MouseOverFar(int i, int j)
        {
            Player player = Main.LocalPlayer;
            Tile tile = Main.tile[Player.tileTargetX, Player.tileTargetY];
            int left = Player.tileTargetX;
            int top = Player.tileTargetY;
            left -= (int)(tile.frameX % 54 / 18);
            if (tile.frameY % 36 != 0)
            {
                top--;
            }
            int chestIndex = Chest.FindChest(left, top);
            player.showItemIcon2 = -1;
            if (chestIndex < 0)
            {
                player.showItemIconText = Language.GetTextValue("LegacyDresserType.0");
            }
            else
            {
                if (Main.chest[chestIndex].name != "")
                {
                    player.showItemIconText = Main.chest[chestIndex].name;
                }
                else
                {
                    player.showItemIconText = chest;
                }
                if (player.showItemIconText == chest)
                {
                    player.showItemIcon2 = DresserItem;
                    player.showItemIconText = "";
                }
            }
            player.noThrow = 2;
            player.showItemIcon = true;
            if (player.showItemIconText == "")
            {
                player.showItemIcon = false;
                player.showItemIcon2 = 0;
            }
        }

        public override void MouseOver(int i, int j)
        {
            Player player = Main.LocalPlayer;
            Tile tile = Main.tile[Player.tileTargetX, Player.tileTargetY];
            int left = Player.tileTargetX;
            int top = Player.tileTargetY;
            left -= (int)(tile.frameX % 54 / 18);
            if (tile.frameY % 36 != 0)
            {
                top--;
            }
            int num138 = Chest.FindChest(left, top);
            player.showItemIcon2 = -1;
            if (num138 < 0)
            {
                player.showItemIconText = Language.GetTextValue("LegacyDresserType.0");
            }
            else
            {
                if (Main.chest[num138].name != "")
                {
                    player.showItemIconText = Main.chest[num138].name;
                }
                else
                {
                    player.showItemIconText = chest;
                }
                if (player.showItemIconText == chest)
                {
                    player.showItemIcon2 = DresserItem;
                    player.showItemIconText = "";
                }
            }
            player.noThrow = 2;
            player.showItemIcon = true;
            if (Main.tile[Player.tileTargetX, Player.tileTargetY].frameY > 0)
            {
                player.showItemIcon2 = ItemID.FamiliarShirt;
            }
        }
    }

    public abstract class ModClock : FurnitureBase
    {
        public ModClock(string clockName, Color worldColor, int clockItem) : base(clockName, worldColor, clockItem) { }

        public virtual void AddTileData() { }
        public sealed override void AddDefaultTileData()
        {
            Main.tileNoAttach[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style2xX);
            TileObjectData.newTile.Height = 5;
            TileObjectData.newTile.CoordinateHeights = new[]
            {
                16,
                16,
                16,
                16,
                16
            };
            AddTileData();
            adjTiles = new int[] { TileID.GrandfatherClocks };
        }

        public sealed override void NearbyEffects(int i, int j, bool closer)
        {
            if (closer)
                Main.clock = true;
        }

        public sealed override bool NewRightClick(int x, int y)
        {
            string text = "AM";
            double time = Main.time;
            if (!Main.dayTime)
                time += 54000.0;

            time = time / 86400.0 * 24.0;
            time = time - 7.5 - 12.0;

            if (time < 0.0)
                time += 24.0;
            if (time >= 12.0)
                text = "PM";

            int intTime = (int)time;
            double deltaTime = time - intTime;
            deltaTime = (int)(deltaTime * 60.0);
            string text2 = string.Concat(deltaTime);

            if (deltaTime < 10.0)
                text2 = "0" + text2;

            if (intTime > 12)
                intTime -= 12;
            if (intTime == 0)
                intTime = 12;

            var newText = string.Concat("Time: ", intTime, ":", text2, " ", text);
            Main.NewText(newText, 255, 240, 20);
            return true;
        }
    }

    public abstract class ModPiano : FurnitureBase
    {
        public ModPiano(string pianoName, Color worldColor, int pianoItem) : base(pianoName, worldColor, pianoItem) { }

        public virtual void AddTileData() { }
        public sealed override void AddDefaultTileData()
        {
            Main.tileWaterDeath[Type] = false;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
            AddTileData();
            AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTable);
        }
    }

    public abstract class ModSofa : FurnitureBase
    {
        public ModSofa(string sofaName, Color worldColor, int sofaItem) : base(sofaName, worldColor, sofaItem) { }

        public virtual void AddTileData() { }
        public sealed override void AddDefaultTileData()
        {
            Main.tileWaterDeath[Type] = false;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
            AddTileData();
            AddToArray(ref TileID.Sets.RoomNeeds.CountsAsChair);
        }
    }

    public abstract class ModCandle : FurnitureBase
    {
        private readonly string FlamePath;
        public ModCandle(string candleName, Color worldColor, int candleItem, string flamePath) : base(candleName, worldColor, candleItem)
        {
            FlamePath = flamePath;
        }

        public virtual void AddTileData() { }
        public sealed override void AddDefaultTileData()
        {
            Main.tileWaterDeath[Type] = false;
            TileObjectData.newTile.CopyFrom(TileObjectData.StyleOnTable1x1);
            TileObjectData.newTile.CoordinateHeights = new int[]
            {
                20
            };
            TileObjectData.newTile.DrawYOffset = -4;
            AddTileData();
            disableSmartCursor = true;
            adjTiles = new int[]
            {
                TileID.Torches,
                TileID.Candles
            };
            AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTorch);
        }

        /// <summary>
        /// Called before the flame is drawn
        /// </summary>
        public virtual void SafePostDraw(int i, int j, SpriteBatch spriteBatch, Color drawColor, Vector2 position, Rectangle sourceRectangle) { }
        public sealed override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            int frameX = Main.tile[i, j].frameX;
            int frameY = Main.tile[i, j].frameY;
            Texture2D flameTexture = ModContent.GetTexture(FlamePath);
            Color drawColor = DrawColor(i, j, new Color(100, 100, 100));
            Vector2 offset = Main.drawToScreen ? Vector2.Zero : new Vector2(Main.offScreenRange);
            Vector2 position = new Vector2(i * 16, j * 16) - Main.screenPosition + offset;
            Rectangle sourceRectangle = new Rectangle(frameX, frameY, 18, 18);
            SafePostDraw(i, j, spriteBatch, drawColor, position, sourceRectangle);
            spriteBatch.Draw(flameTexture, position, sourceRectangle, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        }

        internal Color DrawColor(int i, int j, Color color)
        {
            int tileColor = Main.tile[i, j].color();
            Color colour = WorldGen.paintColor(tileColor);
            if (tileColor >= 13 && tileColor <= 24)
            {
                color.R = (byte)(colour.R / 255f * color.R);
                color.G = (byte)(colour.G / 255f * color.G);
                color.B = (byte)(colour.B / 255f * color.B);
            }
            return color;
        }

        public virtual Color LightColor => new Color(100, 100, 100);

        public sealed override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            if (Main.tile[i, j].frameX < 18)
            {
                r = LightColor.R / 255;
                g = LightColor.G / 255;
                b = LightColor.B / 255;
                return;
            }
            r = 0;
            g = 0;
            b = 0;
        }

        public sealed override void MouseOver(int i, int j)
        {
            Player localPlayer = Main.LocalPlayer;
            localPlayer.noThrow = 2;
            localPlayer.showItemIcon = true;
            localPlayer.showItemIcon2 = FurnitureItem;
        }

        public sealed override bool NewRightClick(int i, int j)
        {
            if (Main.tile[i, j] != null && Main.tile[i, j].active())
            {
                WorldGen.KillTile(i, j, false, false, false);
                if (!Main.tile[i, j].active() && Main.netMode != NetmodeID.SinglePlayer)
                    NetMessage.SendData(MessageID.TileChange, -1, -1, null, 0, i, j, 0f, 0, 0, 0);
            }
            return true;
        }

        public sealed override void HitWire(int i, int j) => TileUtils.LightHitWire(i, j, LightHitWireID.Candle);
    }

    public static class TileUtils
    {
        public static void LightHitWire(int i, int j, int lightID) //todo
        {
            switch(lightID)
            {
                case LightHitWireID.Candelabra:
                    break;
                case LightHitWireID.Candle:
                    break;
                case LightHitWireID.Chandelier:
                    break;
                case LightHitWireID.Lamp:
                    break;
                case LightHitWireID.Lantern:
                    break;
            }
        }
    }

    public class LightHitWireID
    {
        public const int Candelabra = 0;
        public const int Candle = 1;
        public const int Chandelier = 2;
        public const int Lamp = 3;
        public const int Lantern = 4;
    }
}
