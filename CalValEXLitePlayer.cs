using Terraria.ModLoader;

namespace CalValEXLite
{
    public class CalValEXLitePlayer : ModPlayer
    {
        //WORMS
        public bool JaredPet;
        public bool DesertScourgePet;
        public bool AquaticScourgePet;
        public bool AstrumDeusPet;

        //GEMS
        public bool AmberGem;
        public bool AmethistGem;
        public bool CrystalGem;
        public bool DiamondGem;
        public bool EmeraldGem;
        public bool RubyGem;
        public bool SaphireGem;
        public bool TopazGem;

        //FLYING
        public bool AeroSlimePet;

        //WALKING
        public bool AndroombaPet;

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

            AeroSlimePet = false;

            AndroombaPet = false;
        }

        public override void ResetEffects() => ResetThings();

        public override void UpdateDead() => ResetThings();
    }
}
