using CalValEXLite.Items.Placeables.Paintings;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace CalValEXLite.Tiles.Paintings
{
    public class CalamiteaTime_Tile : ModPaintingTile
    {
        public CalamiteaTime_Tile() : base("Calami-tea Time", new Color(124, 192, 222), ModContent.ItemType<CalamiteaTime_Item>()) { }

        public override void AddTileData()
        {
            TileObjectData.newTile.Width = 6;
            TileObjectData.newTile.Height = 4;
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 16, 16 };
        }
    }
}
