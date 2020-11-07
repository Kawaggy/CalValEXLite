using CalValEXLite.Items.Placeables.Paintings;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace CalValEXLite.Tiles.Paintings
{
    public class BlazingConflict_Tile : ModPaintingTile
    {
        public BlazingConflict_Tile() : base("Blazing Conflict", new Color(139, 0, 0), ModContent.ItemType<BlazingConflict_Item>()) { }

        public override void AddTileData()
        {
            TileObjectData.newTile.Width = 7;
            TileObjectData.newTile.Height = 5;
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 16, 16, 16 };
        }
    }
}
