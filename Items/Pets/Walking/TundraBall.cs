using CalValEXLite.Buffs.Pets.Walking;
using CalValEXLite.Projectiles.Pets.Walking;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalValEXLite.Items.Pets.Walking
{
    public class TundraBall : PetItem
    {
        public TundraBall() : base("Tundra Ball", "A chew toy said to have the power to tame the angriest of dogs", new Vector2(32, 26), 3, ModContent.ProjectileType<AngryDoggo>(), ModContent.BuffType<AngryDoggoBuff>(), Item.sellPrice(silver:2, copper:50)) { }

        public override void SafeSetDefaults() => item.UseSound = SoundID.Item47;
    }
}
