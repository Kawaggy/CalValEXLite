using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace CalValEXLite.Projectiles.Pets.Worms.Jared
{
    public class JaredHead : ModWormHead
    {
        public JaredHead() : base(
            54, 
            "Jared Head", 
            ModContent.ProjectileType<JaredBody>(), 
            ModContent.ProjectileType<JaredTail>(), 
            20) { }

        public override bool? CanCutTiles() => false;

        public override void PetFunctionality(Player player)
        {
            if (player.dead)
                player.EX().JaredPet = false;
            if (player.EX().JaredPet)
                projectile.timeLeft = 2;
        }

        public override float IdleSpeedSetup(Vector2 vectorToPlayerCenter)
        {
            if (vectorToPlayerCenter.Length() > 200f)
                return 0.3f;
            if (vectorToPlayerCenter.Length() > 140f)
                return 0.175f;
            return 0.095f;
        }
    }

    public class JaredBody : ModWormBodyTail
    {
        public JaredBody() : base(
            44, 
            "Jared Body",
            ModContent.ProjectileType<JaredBody>(), 
            ModContent.ProjectileType<JaredTail>()) { }

        public override bool? CanCutTiles() => false;

        public override void SpawnSegmentLogic(Player player)
        {
            if (projectile.ai[0] == 0)
            {
                if (projectile.localAI[1] > 1)
                {
                    projectile.ai[0] = Projectile.NewProjectile(player.Center, Vector2.Zero, ModContent.ProjectileType<JaredBody>(), projectile.damage, projectile.knockBack, projectile.owner);
                    Main.projectile[(int)projectile.ai[0]].ai[1] = projectile.whoAmI;
                    Main.projectile[(int)projectile.ai[0]].localAI[1] = projectile.localAI[1] - 1f;
                    Main.projectile[(int)projectile.ai[0]].localAI[0] = projectile.localAI[0] == 0 ? 1 : 0;
                    projectile.netUpdate = true;
                }
                else
                {
                    projectile.ai[0] = Projectile.NewProjectile(player.Center, Vector2.Zero, ModContent.ProjectileType<JaredTail>(), projectile.damage, projectile.knockBack, projectile.owner);
                    Main.projectile[(int)projectile.ai[0]].ai[1] = projectile.whoAmI;
                    Main.projectile[(int)projectile.ai[0]].localAI[1] = projectile.localAI[1] - 1f;
                    projectile.netUpdate = true;
                }
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            if (projectile.localAI[0] == 1)
                projectile.DrawWorm(spriteBatch, drawColor, Texture + "2");
            else
                projectile.DrawWorm(spriteBatch, drawColor, Texture);
            return false;
        }

        public override void PetFunctionality(Player player)
        {
            if (player.dead)
                player.EX().JaredPet = false;
            if (player.EX().JaredPet)
                projectile.timeLeft = 2;
        }
    }

    public class JaredTail : ModWormBodyTail
    {
        public JaredTail() : base(
            40, 
            "Jared Tail",
            ModContent.ProjectileType<JaredBody>(),
            ModContent.ProjectileType<JaredTail>()) { }

        public override bool? CanCutTiles() => false;

        public override void PetFunctionality(Player player)
        {
            if (player.dead)
                player.EX().JaredPet = false;
            if (player.EX().JaredPet)
                projectile.timeLeft = 2;
        }
    }
}
