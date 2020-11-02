using CalValEXLite.Projectiles.Pets.Worms.AquaticScourge;
using Terraria;
using Terraria.ModLoader;

namespace CalValEXLite.Buffs.Pets.Worms
{
    public class AquaticScourgeBuff : ModPetBuff
    {
        public AquaticScourgeBuff() : base("Aquatic Pest", "Blaaaargh", false) { }

        public override void PetFunctionality(Player player)
        {
            player.EX().AquaticScourgePet = true;
            player.SpawnPet(ModContent.ProjectileType<AquaticScourgeHead>());
        }
    }
}
