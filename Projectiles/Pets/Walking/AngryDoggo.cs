using System;
using Terraria;

namespace CalValEXLite.Projectiles.Pets.Walking
{
    public class AngryDoggo : WalkingPet
    {
        public AngryDoggo() : base(0.2f, 0.1f) { }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Angry Doggo");
            Main.projPet[projectile.type] = true;
            Main.projFrames[projectile.type] = 7;
        }

        public override void SafeSetDefaults()
        {
            projectile.width = 84;
            projectile.height = 40;
            projectile.ignoreWater = true;
            projectile.tileCollide = true;
        }

        public override void PetFunctionality(Player player)
        {
            if (player.dead)
                player.EX().AngryDoggo = false;
            if (player.EX().AngryDoggo)
                projectile.timeLeft = 2;
        }

        public override bool ShouldFlyRotate => false;
        public override bool FacesLeft => true;

        public override void AddAnimation()
        {
            base.AddAnimation();
            projectile.frameCounter++;
            if (projectile.frameCounter > 5)
            {
                if (State == Walking)
                {
                    projectile.rotation = 0;
                    if (Math.Abs(projectile.velocity.X) > 0.1f)
                    {
                        projectile.frame++;
                        if (projectile.frame > Main.projFrames[projectile.type] - 2)
                            projectile.frame = 0;
                    }
                    else
                    {
                        projectile.frame = 0;
                    }
                }
                else if (State == Flying)
                {
                    projectile.frame = Main.projFrames[projectile.type] - 1;
                    projectile.rotation += projectile.spriteDirection / -1.25f;
                }
                projectile.frameCounter = 0;
            }
        }
    }
}
