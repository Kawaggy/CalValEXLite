using Terraria.ModLoader;
using Terraria.ID;
using CalValEXLite.NPCs.Critters;

namespace CalValEXLite.Items.Critters
{
    public class EyedolItem : ModItem
    {
        public override void SetStaticDefaults() => DisplayName.SetDefault("Eyedol");

        public override void SetDefaults()
        {
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.autoReuse = true;
            item.useTime = 10;
            item.useAnimation = 15;
            item.useTurn = true;
            item.maxStack = 999;
            item.consumable = true;
            item.width = 22;
            item.height = 22;
            item.noUseGraphic = true;
            item.rare = ItemRarityID.Orange;
            item.makeNPC = (short)ModContent.NPCType<Eyedol>();
        }
    }
}
