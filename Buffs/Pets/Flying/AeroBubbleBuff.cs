using CalValEXLite.Projectiles.Pets.Flying;
using Terraria;
using Terraria.ModLoader;

namespace CalValEXLite.Buffs.Pets.Flying
{
    public class AeroBubbleBuff : ModPetBuff
    {
        public AeroBubbleBuff() : base("Aero Bubble", "We glide together") { }

        public override void PetFunctionality(Player player)
        {
            player.EX().AeroSlimePet = true;
            player.SpawnPet(ModContent.ProjectileType<AeroSlimePet>());
            player.SpawnPet(ModContent.ProjectileType<BabyAeroSlimePet>());
        }
    }
}
