using CalValEXLite.Tiles.Paintings;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace CalValEXLite.Items.Placeables.Paintings
{
    public class CalamiteaTime_Item : ModPaintingItem
    {
        public CalamiteaTime_Item() : base(
            "Calami-tea Time", 
            14, new Vector2(48, 32), 
            "Original by 'caligulas'", 
            "Recreation by 'Mathew Maple'", 
            ModContent.TileType<CalamiteaTime_Tile>()) { }
    }
}
