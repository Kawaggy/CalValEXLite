using CalValEXLite.Projectiles.Pets.Worms.DesertScourge;
using Terraria;
using Terraria.ModLoader;

namespace CalValEXLite.Buffs.Pets.Worms
{
    public class DesertScourgeBuff : ModPetBuff
    {
        public DesertScourgeBuff() : base("Desert Pest", "Blaaaargh", false) { }

        public override void PetFunctionality(Player player)
        {
            player.EX().DesertScourgePet = true;
            player.SpawnPet(ModContent.ProjectileType<DesertScourgeHead>());
        }
    }
}
