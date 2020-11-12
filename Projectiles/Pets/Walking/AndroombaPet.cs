using System;
using Terraria;

namespace CalValEXLite.Projectiles.Pets.Walking
{
    public class AndroombaPet : WalkingPet
    {
        public AndroombaPet() : base(0.2f, 0.1f) { }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Androomba");
            Main.projPet[projectile.type] = true;
            Main.projFrames[projectile.type] = 10;
        }

        public override void SafeSetDefaults()
        {
            projectile.width = 40;
            projectile.height = 30;
            projectile.ignoreWater = true;
            projectile.tileCollide = true;
            drawOriginOffsetY = 2;
        }

        public override void AddAnimation()
        {
            base.AddAnimation();
            projectile.frameCounter++;
            if (projectile.frameCounter > 5)
            {
                if (State == Walking)
                {
                    if (Math.Abs(projectile.velocity.X) > 0.1f)
                    {
                        projectile.frame++;
                        if (projectile.frame > 5)
                            projectile.frame = 0;
                    }
                    else
                    {
                        projectile.frame = 0;
                    }
                }
                else if (State == Flying)
                {
                    projectile.frame++;
                    if (projectile.frame > 9 || projectile.frame < 6)
                        projectile.frame = 6;
                }
                projectile.frameCounter = 0;
            }
        }

        public override void PetFunctionality(Player player)
        {
            if (player.dead)
                player.EX().AndroombaPet = false;
            if (player.EX().AndroombaPet)
                projectile.timeLeft = 2;
        }
    }
}
