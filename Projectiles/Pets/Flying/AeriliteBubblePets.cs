using Microsoft.Xna.Framework;
using Terraria;

namespace CalValEXLite.Projectiles.Pets.Flying
{
    public abstract class AeroPet : FlyingPet
    {
        private readonly string AeroName;
        public AeroPet(string name, float speed, float inertia) : base(speed, inertia)
        {
            AeroName = name;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault(AeroName + "Aero Slime");
            Main.projFrames[projectile.type] = 4;
            Main.projPet[projectile.type] = true;
        }

        public override void SafeSetDefaults()
        {
            projectile.width = 14;
            projectile.height = 14;
            projectile.ignoreWater = true;
            projectile.tileCollide = false;
        }

        public sealed override void AddAnimation()
        {
            base.AddAnimation();
            projectile.frameCounter++;
            if (projectile.frameCounter >= (10 + Main.rand.Next(-4, 5)))
            {
                projectile.frameCounter = 0;
                if (++projectile.frame > Main.projFrames[projectile.type] - 1)
                    projectile.frame = 0;
            }
        }

        public sealed override void PetFunctionality(Player player)
        {
            if (player.dead)
                player.EX().AeroSlimePet = false;
            if (player.EX().AeroSlimePet)
                projectile.timeLeft = 2;
        }

        public sealed override bool FacesLeft => true;
    }

    public class AeroSlimePet : AeroPet
    {
        public AeroSlimePet() : base("", 6f, 12f) { }

        public override Vector2 Offset => new Vector2(72f * -Main.player[projectile.owner].direction, -25f);
    }

    public class BabyAeroSlimePet : AeroPet
    {
        public BabyAeroSlimePet() : base("Baby ", 3f, 6f) { }
    }
}
