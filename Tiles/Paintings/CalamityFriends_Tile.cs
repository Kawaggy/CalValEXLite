using CalValEXLite.Items.Placeables.Paintings;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace CalValEXLite.Tiles.Paintings
{
    public class CalamityFriends_Tile : ModPaintingTile
    {
        public CalamityFriends_Tile() : base("Friends!", new Color(139, 0, 0), ModContent.ItemType<CalamityFriends_Item>()) { }

        public override void AddTileData()
        {
            TileObjectData.newTile.Width = 5;
            TileObjectData.newTile.Height = 5;
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 16, 16, 16 };
        }
    }
}
