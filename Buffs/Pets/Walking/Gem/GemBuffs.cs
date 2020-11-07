using CalValEXLite.Projectiles.Pets.Walking.Gem;
using Terraria;
using Terraria.ModLoader;

namespace CalValEXLite.Buffs.Pets.Walking.Gem
{
    public abstract class GemPetBuff : ModPetBuff
    {
        public GemPetBuff(string gemName) : base(gemName + " Scuttler", "Still won't let go of its gem", false) { }
    }

    public class AmberBuff : GemPetBuff
    {
        public AmberBuff() : base("Amber") { }

        public override void PetFunctionality(Player player)
        {
            player.EX().AmberGem = true;
            player.SpawnPet(ModContent.ProjectileType<Amber>());
        }
    }

    public class AmethistBuff : GemPetBuff
    {
        public AmethistBuff() : base("Amethist") { }

        public override void PetFunctionality(Player player)
        {
            player.EX().AmethistGem = true;
            player.SpawnPet(ModContent.ProjectileType<Amethist>());
        }
    }

    public class CrystalBuff : GemPetBuff
    {
        public CrystalBuff() : base("Crystal") { }

        public override void PetFunctionality(Player player)
        {
            player.EX().CrystalGem = true;
            player.SpawnPet(ModContent.ProjectileType<Crystal>());
        }
    }

    public class DiamondBuff : GemPetBuff
    {
        public DiamondBuff() : base("Diamond") { }

        public override void PetFunctionality(Player player)
        {
            player.EX().DiamondGem = true;
            player.SpawnPet(ModContent.ProjectileType<Diamond>());
        }
    }

    public class EmeraldBuff : GemPetBuff
    {
        public EmeraldBuff() : base("Emerald") { }

        public override void PetFunctionality(Player player)
        {
            player.EX().EmeraldGem = true;
            player.SpawnPet(ModContent.ProjectileType<Emerald>());
        }
    }

    public class RubyBuff : GemPetBuff
    {
        public RubyBuff() : base("Ruby") { }

        public override void PetFunctionality(Player player)
        {
            player.EX().RubyGem = true;
            player.SpawnPet(ModContent.ProjectileType<Ruby>());
        }
    }

    public class SaphireBuff : GemPetBuff
    {
        public SaphireBuff() : base("Sapphire") { }

        public override void PetFunctionality(Player player)
        {
            player.EX().SaphireGem = true;
            player.SpawnPet(ModContent.ProjectileType<Saphire>());
        }
    }

    public class TopazBuff : GemPetBuff
    {
        public TopazBuff() : base("Topaz") { }

        public override void PetFunctionality(Player player)
        {
            player.EX().TopazGem = true;
            player.SpawnPet(ModContent.ProjectileType<Topaz>());
        }
    }
}
