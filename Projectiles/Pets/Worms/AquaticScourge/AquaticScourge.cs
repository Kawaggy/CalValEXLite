using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace CalValEXLite.Projectiles.Pets.Worms.AquaticScourge
{
    public class AquaticScourgeHead : ModWormHead
    {
        public AquaticScourgeHead() : base(
            26,
            "Aquatic Scourge Head",
            ModContent.ProjectileType<AquaticScourgeBody>(),
            ModContent.ProjectileType<AquaticScourgeTail>(),
            7)
        { }

        public override void PetFunctionality(Player player)
        {
            if (player.dead)
                player.EX().AquaticScourgePet = false;
            if (player.EX().AquaticScourgePet)
                projectile.timeLeft = 2;
        }
    }

    public class AquaticScourgeBody : ModWormBodyTail
    {
        public AquaticScourgeBody() : base(
            15,
            "Aquatic Scourge Body",
            ModContent.ProjectileType<AquaticScourgeBody>(), 
            ModContent.ProjectileType<AquaticScourgeTail>())
        { }

        public override void PetFunctionality(Player player)
        {
            if (player.dead)
                player.EX().AquaticScourgePet = false;
            if (player.EX().AquaticScourgePet)
                projectile.timeLeft = 2;
        }
    }

    public class AquaticScourgeTail : ModWormBodyTail
    {
        public AquaticScourgeTail() : base(
            12,
            "Aquatic Scourge Tail",
            ModContent.ProjectileType<AquaticScourgeBody>(),
            ModContent.ProjectileType<AquaticScourgeTail>())
        { }

        public override bool? CanCutTiles() => false;

        public override void PetFunctionality(Player player)
        {
            if (player.dead)
                player.EX().AquaticScourgePet = false;
            if (player.EX().AquaticScourgePet)
                projectile.timeLeft = 2;
        }
    }
}
