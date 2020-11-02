using CalValEXLite.Buffs.Pets.Worms;
using CalValEXLite.Projectiles.Pets.Worms.AquaticScourge;
using CalValEXLite.Projectiles.Pets.Worms.DesertScourge;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalValEXLite.Items.Pets.Worms
{
    public class TheSeaKingsCoin : PetItem
    {
        public TheSeaKingsCoin() : base("The Sea King's Coin", "'Blessed with pest power'", new Vector2(36, 46), 6, ModContent.ProjectileType<AquaticScourgeHead>(), ModContent.BuffType<ScourgesBuff>(), Item.sellPrice(0, 10, 0, 0)) { }

        public override void SafeSetDefaults() => item.UseSound = SoundID.NPCHit13;

        public override bool CanUseItem(Player player)
        {
            if (player.whoAmI == Main.myPlayer)
            {
                if (player.name == "Fabsol" || player.name == "Wormhell" || player.name == "Astrum Deus")
                    return true;
                if (player.ownedProjectileCounts[ModContent.ProjectileType<AquaticScourgeHead>()] > 0 || player.ownedProjectileCounts[ModContent.ProjectileType<DesertScourgeHead>()] > 0)
                    return false;
            }
            return true;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Projectile.NewProjectile(new Vector2(position.X - 50f, position.Y), new Vector2(speedX, speedY), ModContent.ProjectileType<AquaticScourgeHead>(), damage, knockBack, player.whoAmI);
            Projectile.NewProjectile(new Vector2(position.X + 50f, position.Y), new Vector2(speedX, speedY), ModContent.ProjectileType<DesertScourgeHead>(), damage, knockBack, player.whoAmI);
            return false;
        }
    }
}
