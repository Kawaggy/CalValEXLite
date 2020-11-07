using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace CalValEXLite.Projectiles.Pets.Worms.DesertScourge
{
    public class DesertScourgeHead : ModWormHead
    {
        public DesertScourgeHead() : base(
            26, 
            "Desert Scourge Head", 
            ModContent.ProjectileType<DesertScourgeBody>(), 
            ModContent.ProjectileType<DesertScourgeTail>(), 
            7) { }

        public override void PetFunctionality(Player player)
        {
            if (player.dead)
                player.EX().DesertScourgePet = false;
            if (player.EX().DesertScourgePet)
                projectile.timeLeft = 2;
        }
    }

    public class DesertScourgeBody : ModWormBodyTail
    {
        public DesertScourgeBody() : base(
            15, 
            "Desert Scourge Body",
            ModContent.ProjectileType<DesertScourgeBody>(), 
            ModContent.ProjectileType<DesertScourgeTail>()) { }

        public override void PetFunctionality(Player player)
        {
            if (player.dead)
                player.EX().DesertScourgePet = false;
            if (player.EX().DesertScourgePet)
                projectile.timeLeft = 2;
        }
    }

    public class DesertScourgeTail : ModWormBodyTail
    {
        public DesertScourgeTail() : base(
            12, 
            "Desert Scourge Tail",
            ModContent.ProjectileType<DesertScourgeBody>(),
            ModContent.ProjectileType<DesertScourgeTail>()) { }

        public override bool? CanCutTiles() => false;

        public override void PetFunctionality(Player player)
        {
            if (player.dead)
                player.EX().DesertScourgePet = false;
            if (player.EX().DesertScourgePet)
                projectile.timeLeft = 2;
        }
    }
}
