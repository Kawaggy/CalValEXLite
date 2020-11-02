using CalValEXLite.Projectiles.Pets.Worms.AstrumDeus;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace CalValEXLite.Buffs.Pets.Worms
{
    public class AstrumDeusBuff : ModPetBuff
    {
        public AstrumDeusBuff() : base("Astrum Demus", "Wormhell", false) { }

        public override void PetFunctionality(Player player)
        {
            player.EX().AstrumDeusPet = true;
            player.SpawnPet(ModContent.ProjectileType<AstrumDeusHead>());
            bool swarmSpawned = player.ownedProjectileCounts[ModContent.ProjectileType<SmallAstrumDeusHead>()] <= 0;
            if (swarmSpawned && player.whoAmI == Main.myPlayer)
            {
                if (player.name == "Fabsol" || player.name == "Wormhell" || player.name == "Astrum Deus")
                {
                    for (int i = 0; i < 100; i++)
                    {
                        Projectile.NewProjectile(new Vector2(player.position.X + Main.rand.Next(-50, 51), player.position.Y + Main.rand.Next(-50, 51)), Vector2.Zero, ModContent.ProjectileType<SmallAstrumDeusHead>(), 0, 0f, player.whoAmI);
                    }
                }
                else
                {
                    for (int i = 0; i < 10; i++)
                    {
                        Projectile.NewProjectile(new Vector2(player.position.X + Main.rand.Next(-50, 51), player.position.Y + Main.rand.Next(-50, 51)), Vector2.Zero, ModContent.ProjectileType<SmallAstrumDeusHead>(), 0, 0f, player.whoAmI);
                    }
                }
            }
        }
    }
}
