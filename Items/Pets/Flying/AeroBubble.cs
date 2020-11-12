using CalValEXLite.Buffs.Pets.Flying;
using CalValEXLite.Projectiles.Pets.Flying;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalValEXLite.Items.Pets.Flying
{
    public class AeroBubble : PetItem
    {
        public AeroBubble() : base("Aero Bubble", "An odd disc covered in light gel", new Vector2(38, 22), 20, ModContent.ProjectileType<AeroSlimePet>(), ModContent.BuffType<AeroBubbleBuff>(), Item.sellPrice(silver: 20)) { }

        public sealed override void SafeSetDefaults() => item.UseSound = SoundID.NPCHit1;
    }
}
