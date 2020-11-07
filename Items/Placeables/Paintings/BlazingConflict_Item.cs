using CalValEXLite.Tiles.Paintings;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace CalValEXLite.Items.Placeables.Paintings
{
    public class BlazingConflict_Item : ModPaintingItem
    {
        public BlazingConflict_Item() : base(
            "Blazing Conflict", 
            12, 
            new Vector2(60, 38), 
            "'Yharex87'", 
            "Fire vs fire", 
            ModContent.TileType<BlazingConflict_Tile>()) { }
    }
}
