using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalValEXLite.Items.Dyes
{
    public class DraedonHologramDye : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Draedon's Hologram Dye");
        }

        public override void SetDefaults()
		{
			byte dye = item.dye;
			item.CloneDefaults(ItemID.GelDye);
            item.width = 22;
            item.height = 26;
			item.dye = dye;
		}

        private int frameCounter;
        private int frame;

        internal Rectangle GetCurrentFrame(bool frameCounterUp = true)
        {
            int frames = 16;
            if (frameCounter % 5 == 0)
            {
                frame = ((frame == frames - 1) ? 0 : frame + 1);
            }
            if (frameCounterUp)
                frameCounter++;
            return new Rectangle(0, item.height * frame, item.width, item.height);
        }

        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            Texture2D texture = ModContent.GetTexture("CalValEXLite/Items/Dyes/DraedonHologramDye_Animated");
            spriteBatch.Draw(texture, position, new Rectangle?(GetCurrentFrame(true)), Color.White, 0f, origin, scale, SpriteEffects.None, 0f);
            return false;
        }

        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            Texture2D texture = ModContent.GetTexture("CalValEXLite/Items/Dyes/DraedonHologramDye_Animated");
            spriteBatch.Draw(texture, item.position - Main.screenPosition, new Rectangle?(GetCurrentFrame(true)), lightColor, rotation, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            return false;
        }
    }
}
