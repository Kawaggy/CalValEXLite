using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace CalValEXLite
{
    public abstract class ModPaintingTile : ModTile
    {
        private readonly Color color;
        private readonly string PaintingName;
        private readonly int PaintingItem;
        public ModPaintingTile(string name, Color worldColor, int paintingItem)
        {
            PaintingName = name;
            color = worldColor;
            PaintingItem = paintingItem;
        }

        public abstract void AddTileData();
        public sealed override void SetDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileLighted[Type] = true;
            Main.tileLavaDeath[Type] = true;
            TileID.Sets.FramesOnKillWall[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3Wall);
            TileObjectData.newTile.CoordinatePadding = 0;
            AddTileData();
            TileObjectData.addTile(Type);
            ModTranslation name = CreateMapEntryName();
            name.SetDefault(PaintingName);
            AddMapEntry(color, name);
        }

        public sealed override void KillMultiTile(int i, int j, int frameX, int frameY) => Item.NewItem(i * 16, j * 16, 48, 48, PaintingItem);
    }
}
