using CalValEXLite.Buffs.Pets.Worms;
using CalValEXLite.Projectiles.Pets.Worms.Jared;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalValEXLite.Items.Pets.Worms
{
    public class SoulShard : WormItem
    {
        public SoulShard() : base(
            "Soul Shard", 
            "The madness of the abyss echoes through your flesh", 
            new Vector2(16, 30), 
            16, //hot pink/dev
            ModContent.ProjectileType<JaredHead>(), 
            ModContent.BuffType<JaredBuff>(), 
            Item.sellPrice(3, 50, 0, 0)) { }

        public override void SafeSetDefaults() => item.UseSound = SoundID.NPCHit13;
    }
}
