using CalValEXLite.Items.Placeables;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace CalValEXLite.Tiles
{
    public class VoidPortal_Tile : ModPaintingTile
    {
        public VoidPortal_Tile() : base("Void Portal", new Color(1, 1, 1), ModContent.ItemType<VoidPortal_Item>()) { }

        public override void AddTileData()
        {
            TileObjectData.newTile.Width = 4;
            TileObjectData.newTile.Height = 4;
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 16, 16 };
            animationFrameHeight = 64;
        }

        public override void AnimateTile(ref int frame, ref int frameCounter)
        {
            frameCounter++;
            if (frameCounter > 8)
            {
                frameCounter = 0;
                frame++;
                if (frame > 3)
                {
                    frame = 0;
                }
            }
        }
    }
}
