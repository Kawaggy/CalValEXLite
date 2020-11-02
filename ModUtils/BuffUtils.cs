using Terraria;
using Terraria.ModLoader;

namespace CalValEXLite
{
    public abstract class ModPetBuff : ModBuff
    {
        private readonly string BuffName;
        private readonly string BuffTooltip;
        private readonly bool LightPet;

        public ModPetBuff(string name, string tooltip, bool lightPet = false)
        {
            BuffName = name;
            BuffTooltip = tooltip;
            LightPet = lightPet;
        }

        public sealed override void SetDefaults()
        {
            DisplayName.SetDefault(BuffName);
            Description.SetDefault(BuffTooltip);
            Main.buffNoTimeDisplay[Type] = true;
            if (!LightPet)
                Main.vanityPet[Type] = true;
            else
                Main.lightPet[Type] = true;
        }

        public virtual void PetFunctionality(Player player) { }

        public sealed override void Update(Player player, ref int buffIndex)
        {
            player.buffTime[buffIndex] = 18000;
            PetFunctionality(player);
        }
    }
}
