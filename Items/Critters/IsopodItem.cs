using Terraria.ID;
using System.Collections.Generic;
using Terraria.ModLoader;
using CalValEXLite.NPCs.Critters;

namespace CalValEXLite.Items.Critters
{
    public class IsopodItem : ModItem
    {
        public override void SetStaticDefaults() => DisplayName.SetDefault("Abyssal Isopod");

        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.GlowingSnail);
            if (CalValEXLite.calamityMod != null)
            {
                Mod clam = CalValEXLite.calamityMod;
                if ((bool)clam.Call("GetBossDowned", "polterghast"))
                    item.bait = 55;
                else
                    item.bait = 1;
            }
            else
                item.bait = 25;
            item.makeNPC = (short)ModContent.NPCType<Isopod>();
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips) => ItemUtils.CheckRarity(13, tooltips);
    }
}
