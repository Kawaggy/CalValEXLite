using CalValEXLite.Buffs.Pets.Worms;
using CalValEXLite.Projectiles.Pets.Worms.AquaticScourge;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalValEXLite.Items.Pets.Worms
{
    public class MoistLocket : WormItem
    {
        public MoistLocket() : base("Moist Locket", "'Theres a worm wriggling in it'", new Vector2(36, 46), 6, ModContent.ProjectileType<AquaticScourgeHead>(), ModContent.BuffType<AquaticScourgeBuff>(), Item.sellPrice(0, 10, 0, 0)) { }

        public override void SafeSetDefaults() => item.UseSound = SoundID.NPCHit13;
    }
}
