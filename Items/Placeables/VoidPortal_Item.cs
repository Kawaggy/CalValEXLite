using CalValEXLite.Tiles;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace CalValEXLite.Items.Placeables
{
    public class VoidPortal_Item : BaseTileItem
    {
        public VoidPortal_Item() : base("Void Portal", 13, new Vector2(30, 28), "The void calls out", ModContent.TileType<VoidPortal_Tile>()) { }
    }
}
