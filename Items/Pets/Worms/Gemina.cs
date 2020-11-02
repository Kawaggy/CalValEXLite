using CalValEXLite.Buffs.Pets.Worms;
using CalValEXLite.Projectiles.Pets.Worms.AstrumDeus;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalValEXLite.Items.Pets.Worms
{
    public class Gemina : WormItem
    {
        public Gemina() : base("Gemina", "A highly condensed star", new Vector2(26, 26), 9, ModContent.ProjectileType<AstrumDeusHead>(), ModContent.BuffType<AstrumDeusBuff>(), Item.sellPrice(1, 0, 0, 0)) { }

        public override void SafeSetDefaults() => item.UseSound = SoundID.NPCHit4;
    }
}
