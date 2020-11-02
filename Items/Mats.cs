using Terraria;
using Terraria.DataStructures;
using Terraria.ID;

namespace CalValEXLite.Items
{
    public class NuclearFumes : QuickMat
    {
        public NuclearFumes() : base("Nuclear Fumes", "Don't inhale them", 999, 13, Item.sellPrice(gold: 10)) { }

        public override void SafeSetStaticDefaults()
        {
            ItemID.Sets.ItemNoGravity[item.type] = true;
        }
    }

    public class SparrowMeat : QuickMat
    {
        public SparrowMeat() : base("Sparrow Meat", "There's a bit of pink cloth in it", 30, 10, Item.sellPrice(gold: 30, silver: 20)) { }

        public override void SafeSetDefaults()
        {
            item.useTime = 15;
            item.useAnimation = 15;
            item.useTurn = true;
            item.UseSound = SoundID.Item2;
            item.consumable = true;
            item.buffType = BuffID.WellFed;
            item.useStyle = ItemUseStyleID.EatingUsing;
            item.buffTime = 3600;
        }
    }

    public class Termipebbles : QuickMat
    {
        public Termipebbles() : base("Termipebbles", "Used to craft Terminus-themed vanitites\n" + "'Do NOT eat.'", 999, 15, Item.sellPrice(silver: 5)) { }

        public override void SafeSetStaticDefaults()
        {
            Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(5, 4));
            ItemID.Sets.ItemNoGravity[item.type] = true;
            ItemID.Sets.ItemIconPulse[item.type] = false;
        }
    }
}
