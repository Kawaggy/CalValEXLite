using System;
using Terraria;

namespace CalValEXLite.Projectiles.Pets.Walking.Gem
{
    public abstract class GemScuttler : WalkingPet
    {
        private readonly string GemName;
        public GemScuttler(string gemName) : base(0.2f, 0f)
        {
            GemName = gemName;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault(GemName + " Scuttler");
            Main.projPet[projectile.type] = true;
            Main.projFrames[projectile.type] = 4;
        }

        public override void SafeSetDefaults()
        {
            projectile.width = 16;
            projectile.height = 16;
            projectile.ignoreWater = true;
            projectile.tileCollide = true;
            drawOriginOffsetY = yOffset;
        }

        private void AddDust()
        {
            if (Main.rand.Next(200) == 0)
                Dust.NewDust(projectile.position, projectile.width, projectile.height, 20, Main.rand.Next(-5, 6), Main.rand.Next(-5, 6));
        }

        public sealed override void CustomBehaviour(Player player)
        {
            AddDust();
            base.CustomBehaviour(player);
        }

        public sealed override void AddAnimation()
        {
            projectile.frameCounter++;
            if (projectile.frameCounter > 5)
            {
                if (Math.Abs(projectile.velocity.X) > 0.1f)
                {
                    projectile.frame++;
                    if (projectile.frame > Main.projFrames[projectile.type] - 1)
                        projectile.frame = 0;
                }
                else
                {
                    projectile.frame = 0;
                }
                projectile.frameCounter = 0;
            }
            base.AddAnimation();
        }
        public override bool FacesLeft => true;
        public override bool CanFly => false;
        public virtual int yOffset => 0;
    }

    public class Amber : GemScuttler
    {
        public Amber() : base("Amber") { }

        public override void PetFunctionality(Player player)
        {
            if (player.dead)
                player.EX().AmberGem = false;
            if (player.EX().AmberGem)
                projectile.timeLeft = 2;
        }
    }
    public class Amethist : GemScuttler
    {
        public Amethist() : base("Amethist") { }

        public override void PetFunctionality(Player player)
        {
            if (player.dead)
                player.EX().AmethistGem = false;
            if (player.EX().AmethistGem)
                projectile.timeLeft = 2;
        }

        public override int yOffset => -2;
    }
    public class Crystal : GemScuttler
    {
        public Crystal() : base("Crystal") { }

        public override void PetFunctionality(Player player)
        {
            if (player.dead)
                player.EX().CrystalGem = false;
            if (player.EX().CrystalGem)
                projectile.timeLeft = 2;
        }

        public override int yOffset => -2;
    }
    public class Diamond : GemScuttler
    {
        public Diamond() : base("Diamond") { }

        public override void PetFunctionality(Player player)
        {
            if (player.dead)
                player.EX().DiamondGem = false;
            if (player.EX().DiamondGem)
                projectile.timeLeft = 2;
        }

        public override int yOffset => -2;
    }
    public class Emerald : GemScuttler
    {
        public Emerald() : base("Emerald") { }

        public override void PetFunctionality(Player player)
        {
            if (player.dead)
                player.EX().EmeraldGem = false;
            if (player.EX().EmeraldGem)
                projectile.timeLeft = 2;
        }

        public override int yOffset => -4;
    }
    public class Ruby : GemScuttler
    {
        public Ruby() : base("Ruby") { }

        public override void PetFunctionality(Player player)
        {
            if (player.dead)
                player.EX().RubyGem = false;
            if (player.EX().RubyGem)
                projectile.timeLeft = 2;
        }
    }
    public class Saphire : GemScuttler
    {
        public Saphire() : base("Saphire") { }

        public override void PetFunctionality(Player player)
        {
            if (player.dead)
                player.EX().SaphireGem = false;
            if (player.EX().SaphireGem)
                projectile.timeLeft = 2;
        }

        public override int yOffset => -4;
    }
    public class Topaz : GemScuttler
    {
        public Topaz() : base("Topaz") { }

        public override void PetFunctionality(Player player)
        {
            if (player.dead)
                player.EX().TopazGem = false;
            if (player.EX().TopazGem)
                projectile.timeLeft = 2;
        }
    }
}
