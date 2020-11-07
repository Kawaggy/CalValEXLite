using CalValEXLite.Tiles.Paintings;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace CalValEXLite.Items.Placeables.Paintings
{
    public class CalamityFriends_Item : ModPaintingItem
    {
        public CalamityFriends_Item() : base(
            "Little Buddies",
            14,
            new Vector2(28, 28),
            "'Mochi'",
            "Friends!",
            ModContent.TileType<CalamityFriends_Tile>())
        { }
    }
}
