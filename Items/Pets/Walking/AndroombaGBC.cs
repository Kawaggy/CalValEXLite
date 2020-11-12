using CalValEXLite.Buffs.Pets.Walking;
using CalValEXLite.Projectiles.Pets.Walking;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalValEXLite.Items.Pets.Walking
{
    public class AndroombaGBC : PetItem
    {
        public AndroombaGBC() : base("Suspicious looking GBC", "What could this mean?", new Vector2(32, 34), 20, ModContent.ProjectileType<AndroombaPet>(), ModContent.BuffType<AndroombaBuff>(), Item.sellPrice(platinum:1, silver:1)) { }

        public override void SafeSetDefaults() => item.UseSound = SoundID.NPCHit4;
    }
}
