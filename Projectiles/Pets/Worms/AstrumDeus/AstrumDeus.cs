using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace CalValEXLite.Projectiles.Pets.Worms.AstrumDeus
{
    public class AstrumDeusHead : ModWormHead
    {
        public AstrumDeusHead() : base(
            30,
            "Astrum Deus Head",
            ModContent.ProjectileType<AstrumDeusBody>(),
            ModContent.ProjectileType<AstrumDeusTail>(),
            14)
        { }

        public override bool? CanCutTiles() => false;

        public override void PetFunctionality(Player player)
        {
            if (player.dead)
                player.EX().AstrumDeusPet = false;
            if (player.EX().AstrumDeusPet)
                projectile.timeLeft = 2;
        }

        public override float IdleSpeedSetup(Vector2 vectorToPlayerCenter) => 0.3f;

        public override float MaxIdleSpeedSetup() => 25f;
    }

    public class SmallAstrumDeusHead : ModWormHead
    {
        public override string Texture => "CalValEXLite/Projectiles/Pets/Worms/AstrumDeus/AstrumDeusHead";
        public SmallAstrumDeusHead() : base(
            30,
            "Small Astrum Deus Head",
            ModContent.ProjectileType<AstrumDeusBody>(),
            ModContent.ProjectileType<AstrumDeusTail>(),
            6)
        { }

        public override bool SafePreSetDefaults()
        {
            projectile.scale = 0.75f;
            return base.SafePreSetDefaults();
        }

        public override bool? CanCutTiles() => false;

        public override void PetFunctionality(Player player)
        {
            if (player.dead)
                player.EX().AstrumDeusPet = false;
            if (player.EX().AstrumDeusPet)
                projectile.timeLeft = 2;
        }

        public override float IdleSpeedSetup(Vector2 vectorToPlayerCenter) => 0.15f;

        public override float MaxIdleSpeedSetup() => 10f;
    }

    public class AstrumDeusBody : ModWormBodyTail
    {
        public AstrumDeusBody() : base(
            14,
            "Astrum Deus Body",
            ModContent.ProjectileType<AstrumDeusBody>(),
            ModContent.ProjectileType<AstrumDeusTail>())
        { }

        public override bool? CanCutTiles() => false;

        public override void SpawnSegmentLogic(Player player)
        {
            if (projectile.ai[0] == 0)
            {
                if (projectile.localAI[1] > 1)
                {
                    projectile.ai[0] = Projectile.NewProjectile(player.Center, Vector2.Zero, ModContent.ProjectileType<AstrumDeusBody>(), projectile.damage, projectile.knockBack, projectile.owner);
                    Main.projectile[(int)projectile.ai[0]].ai[1] = projectile.whoAmI;
                    Main.projectile[(int)projectile.ai[0]].localAI[1] = projectile.localAI[1] - 1f;
                    if (projectile.localAI[0] == 0)
                    {
                        Main.projectile[(int)projectile.ai[0]].localAI[0] = 1; //intermediate piece
                    }
                    else if (projectile.localAI[0] == 1)
                    {
                        Main.projectile[(int)projectile.ai[0]].localAI[0] = 2;
                    }
                    else if (projectile.localAI[0] == 2)
                    {
                        Main.projectile[(int)projectile.ai[0]].localAI[0] = 3; //intermediate piece
                    }
                    else if (projectile.localAI[0] == 3)
                    {
                        Main.projectile[(int)projectile.ai[0]].localAI[0] = 0;
                    }
                    projectile.netUpdate = true;
                }
                else
                {
                    projectile.ai[0] = Projectile.NewProjectile(player.Center, Vector2.Zero, ModContent.ProjectileType<AstrumDeusTail>(), projectile.damage, projectile.knockBack, projectile.owner);
                    Main.projectile[(int)projectile.ai[0]].ai[1] = projectile.whoAmI;
                    Main.projectile[(int)projectile.ai[0]].localAI[1] = projectile.localAI[1] - 1f;
                    projectile.netUpdate = true;
                }
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            if (projectile.localAI[0] == 1 || projectile.localAI[0] == 3)
                projectile.DrawWorm(spriteBatch, drawColor, Texture + "2");
            if (projectile.localAI[0] == 0)
                projectile.DrawWorm(spriteBatch, drawColor, Texture);
            if (projectile.localAI[0] == 2)
                projectile.DrawWorm(spriteBatch, drawColor, Texture + "3");
            return false;
        }

        public override void PetFunctionality(Player player)
        {
            if (player.dead)
                player.EX().AstrumDeusPet = false;
            if (player.EX().AstrumDeusPet)
                projectile.timeLeft = 2;
        }
    }

    public class AstrumDeusTail : ModWormBodyTail
    {
        public AstrumDeusTail() : base(
            20,
            "Astrum Deus Tail",
            ModContent.ProjectileType<AstrumDeusBody>(),
            ModContent.ProjectileType<AstrumDeusTail>())
        { }

        public override bool? CanCutTiles() => false;

        public override void PetFunctionality(Player player)
        {
            if (player.dead)
                player.EX().AstrumDeusPet = false;
            if (player.EX().AstrumDeusPet)
                projectile.timeLeft = 2;
        }
    }
}
