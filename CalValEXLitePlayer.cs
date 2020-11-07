using Terraria.ModLoader;

namespace CalValEXLite
{
    public class CalValEXLitePlayer : ModPlayer
    {
        public bool JaredPet;
        public bool DesertScourgePet;
        public bool AquaticScourgePet;
        public bool AstrumDeusPet;

        public bool AmberGem;
        public bool AmethistGem;
        public bool CrystalGem;
        public bool DiamondGem;
        public bool EmeraldGem;
        public bool RubyGem;
        public bool SaphireGem;
        public bool TopazGem;

        public void ResetThings()
        {
            JaredPet = false;
            DesertScourgePet = false;
            AquaticScourgePet = false;
            AstrumDeusPet = false;

            AmberGem = false;
            AmethistGem = false;
            CrystalGem = false;
            DiamondGem = false;
            EmeraldGem = false;
            RubyGem = false;
            SaphireGem = false;
            TopazGem = false;
        }

        public override void ResetEffects() => ResetThings();

        public override void UpdateDead() => ResetThings();
    }
}
