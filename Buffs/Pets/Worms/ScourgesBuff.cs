using CalValEXLite.Projectiles.Pets.Worms.AquaticScourge;
using CalValEXLite.Projectiles.Pets.Worms.DesertScourge;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace CalValEXLite.Buffs.Pets.Worms
{
    public class ScourgesBuff : ModPetBuff
    {
        public ScourgesBuff() : base("Pesky Twins", "Blaaaargh^2", false) { }

        public override void PetFunctionality(Player player)
        {
            player.EX().DesertScourgePet = true;
            player.EX().AquaticScourgePet = true;
            if (player.ownedProjectileCounts[ModContent.ProjectileType<DesertScourgeHead>()] <= 0 && player.whoAmI == Main.myPlayer) 
                Projectile.NewProjectile(new Vector2(player.position.X + 50f, player.position.Y), Vector2.Zero, ModContent.ProjectileType<DesertScourgeHead>(), 0, 0f, player.whoAmI);
            if (player.ownedProjectileCounts[ModContent.ProjectileType<AquaticScourgeHead>()] <= 0 && player.whoAmI == Main.myPlayer) 
                Projectile.NewProjectile(new Vector2(player.position.X - 50f, player.position.Y), Vector2.Zero, ModContent.ProjectileType<AquaticScourgeHead>(), 0, 0f, player.whoAmI);
        }
    }
}
