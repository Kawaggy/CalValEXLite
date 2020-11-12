using CalValEXLite.Projectiles.Pets.Walking;
using Terraria;
using Terraria.ModLoader;

namespace CalValEXLite.Buffs.Pets.Walking
{
    public class AndroombaBuff : ModPetBuff
    {
        public AndroombaBuff() : base("Androomba", "Nyoooom!") { }

        public override void PetFunctionality(Player player)
        {
            player.EX().AndroombaPet = true;
            player.SpawnPet(ModContent.ProjectileType<AndroombaPet>());
        }
    }
}
