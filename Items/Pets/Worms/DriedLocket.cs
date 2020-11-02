using CalValEXLite.Buffs.Pets.Worms;
using CalValEXLite.Projectiles.Pets.Worms.DesertScourge;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalValEXLite.Items.Pets.Worms
{
    public class DriedLocket : WormItem
    {
        public DriedLocket() : base("Dried Locket", "'There's a worm wriggling in it'", new Vector2(44, 40), 2, ModContent.ProjectileType<DesertScourgeHead>(), ModContent.BuffType<DesertScourgeBuff>(), Item.sellPrice(0, 0, 50, 0)) { }

        public override void SafeSetDefaults() => item.UseSound = SoundID.NPCHit13;
    }
}
