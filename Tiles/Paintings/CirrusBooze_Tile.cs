using CalValEXLite.Items.Placeables.Paintings;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace CalValEXLite.Tiles.Paintings
{
    public class CirrusBooze_Tile : ModPaintingTile
    {
        public CirrusBooze_Tile() : base("Fine Chalice", new Color(234, 0, 255), ModContent.ItemType<CirrusBooze_Item>()) { }

        public override void AddTileData() { }
    }
}
