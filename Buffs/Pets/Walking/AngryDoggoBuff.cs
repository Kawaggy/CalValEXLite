using CalValEXLite.Projectiles.Pets.Walking;
using Terraria;
using Terraria.ModLoader;

namespace CalValEXLite.Buffs.Pets.Walking
{
    public class AngryDoggoBuff : ModPetBuff
    {
        public AngryDoggoBuff() : base("Upset Pupper", "Like, the fifth best dog in Calamity") { }

        public override void PetFunctionality(Player player)
        {
            player.EX().AngryDoggo = true;
            player.SpawnPet(ModContent.ProjectileType<AngryDoggo>());
        }
    }
}
