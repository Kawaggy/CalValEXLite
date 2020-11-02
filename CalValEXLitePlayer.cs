using Terraria.ModLoader;

namespace CalValEXLite
{
    public class CalValEXLitePlayer : ModPlayer
    {
        public bool JaredPet;
        public bool DesertScourgePet;
        public bool AquaticScourgePet;
        public bool AstrumDeusPet;

        public void ResetThings()
        {
            JaredPet = false;
            DesertScourgePet = false;
            AquaticScourgePet = false;
            AstrumDeusPet = false;
        }

        public override void ResetEffects() => ResetThings();

        public override void UpdateDead() => ResetThings();
    }
}
