using CalValEXLite.Tiles.Paintings;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace CalValEXLite.Items.Placeables.Paintings
{
    public class CirrusBooze_Item : ModPaintingItem
    {
        public CirrusBooze_Item() : base(
            "A Fine Chalice",
            12,
            new Vector2(24, 26),
            "'TimerFun'",
            "A fine chalice indeed",
            ModContent.TileType<CirrusBooze_Tile>())
        { }
    }
}
