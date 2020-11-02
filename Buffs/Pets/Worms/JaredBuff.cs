using CalValEXLite.Projectiles.Pets.Worms.Jared;
using Terraria;
using Terraria.ModLoader;

namespace CalValEXLite.Buffs.Pets.Worms
{
    public class JaredBuff : ModPetBuff
    {
        public JaredBuff() : base("Eidolon Wyrm", "Up from the depths", false) { }

        public override void PetFunctionality(Player player)
        {
            player.EX().JaredPet = true;
            player.SpawnPet(ModContent.ProjectileType<JaredHead>());
        }
    }
}
