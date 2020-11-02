using CalValEXLite.NPCs.Critters;
using System.Collections.Generic;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalValEXLite.Items.Critters
{
    public class CrystalButterflyItem : ModItem
    {
        public override void SetStaticDefaults() => DisplayName.SetDefault("Crystal Butterfly");
		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.JuliaButterfly);
			item.makeNPC = (short)ModContent.NPCType<CrystalButterfly>();
		}

        public override void ModifyTooltips(List<TooltipLine> tooltips) => ItemUtils.CheckRarity(12, tooltips);
    }
}
