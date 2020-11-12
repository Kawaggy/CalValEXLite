using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalValEXLite
{
    public abstract class QuickMat : ModItem
    {
        private readonly string MatName;
        private readonly string MatTooltip;
        private readonly int MaxStack;
        private readonly int Value;
        private readonly int Rare;

        public QuickMat(string name, string tooltip, int maxStack, int rare, int value)
        {
            MatName = name;
            MatTooltip = tooltip;
            MaxStack = maxStack;
            Rare = rare;
            Value = value;
        }

        public virtual void SafeSetStaticDefaults() { }
        public sealed override void SetStaticDefaults()
        {
            DisplayName.SetDefault(MatName);
            Tooltip.SetDefault(MatTooltip);

            SafeSetStaticDefaults();
        }

        public virtual void SafeSetDefaults() { }
        public sealed override void SetDefaults()
        {
            item.width = 16;
            item.height = 16;
            item.maxStack = MaxStack;
            item.rare = Rare;
            item.value = Value;

            SafeSetDefaults();
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips) => ItemUtils.CheckRarity(Rare, tooltips);
    }

    public abstract class ModHookItem : ModItem
    {
        private readonly string HookName;
        private readonly string HookTooltip;
        private readonly int Value;
        private readonly int Rare;
        private readonly int HookType;
        private readonly float HookShootSpeed;
        private readonly Vector2 Size;

        public ModHookItem(string name, string tooltip, int hookType, int value, int rare, float shootSpeed, Vector2 size)
        {
            HookName = name;
            HookTooltip = tooltip;
            HookType = hookType;
            Value = value;
            Rare = rare;
            HookShootSpeed = shootSpeed;
            Size = size;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault(HookName);
            Tooltip.SetDefault(HookTooltip);
        }

        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.AmethystHook);
            item.width = (int)Size.X;
            item.height = (int)Size.Y;
            item.value = Value;
            item.rare = Rare;
            item.shoot = HookType;
            item.shootSpeed = HookShootSpeed;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips) => ItemUtils.CheckRarity(Rare, tooltips);
    }

    public abstract class PetItem : ModItem
    {
        private readonly string ItemName;
        private readonly string TooltipText;
        private readonly Vector2 Size;
        private readonly int Rare;
        private readonly int ProjType;
        private readonly int BuffType;
        private readonly int Value;

        public PetItem(string name, string tooltip, Vector2 size, int rare, int petType, int buffType, int value)
        {
            ItemName = name;
            TooltipText = tooltip;
            Size = size;
            Rare = rare;
            ProjType = petType;
            BuffType = buffType;
            Value = value;
        }

        public virtual void SafeSetStaticDefaults() { }
        public sealed override void SetStaticDefaults()
        {
            SafeSetStaticDefaults();
            DisplayName.SetDefault(ItemName);
            Tooltip.SetDefault(TooltipText);
        }

        public virtual void SafeSetDefaults() { }
        public sealed override void SetDefaults()
        {
            SafeSetDefaults();
            item.damage = 0;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.shoot = ProjType;
            item.buffType = BuffType;
            item.width = (int)Size.X;
            item.height = (int)Size.Y;
            item.useAnimation = 20;
            item.useTime = 20;
            item.rare = Rare;
            item.noMelee = true;
            item.value = Value;
        }

        public virtual void SafeModifyTooltips(List<TooltipLine> tooltips) { }
        public sealed override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            SafeModifyTooltips(tooltips);
            ItemUtils.CheckRarity(Rare, tooltips);
        }

        public sealed override void UseStyle(Player player)
        {
            if (player.whoAmI == Main.myPlayer && player.itemTime == 0)
                player.AddBuff(item.buffType, 3600, true);
        }
    }

    public abstract class WormItem : PetItem
    {
        public WormItem(string name, string tooltip, Vector2 size, int rare, int petType, int buffType, int value) : base(name, tooltip, size, rare, petType, buffType, value) { }

        public sealed override bool CanUseItem(Player player)
        {
            if (player.whoAmI == Main.myPlayer)
            {
                if (player.name == "Fabsol" || player.name == "Wormhell" || player.name == "Astrum Deus")
                    return true;
                if (player.ownedProjectileCounts[item.shoot] > 0)
                    return false;
            }
            return true;
        }
    }

    public abstract class BaseTileItem : ModItem
    {
        private readonly string TileItemName;
        private readonly int TileType;
        private readonly int Rare;
        private readonly Vector2 Size;
        private readonly string TileItemTooltip;
        public BaseTileItem(string name, int rare, Vector2 size, string tooltip, int tileType)
        {
            TileItemName = name;
            TileType = tileType;
            Rare = rare;
            Size = size;
            TileItemTooltip = tooltip;
        }

        public sealed override void SetStaticDefaults()
        {
            DisplayName.SetDefault(TileItemName);
            Tooltip.SetDefault(TileItemTooltip);
        }

        public sealed override void SetDefaults()
        {
            item.width = (int)Size.X;
            item.height = (int)Size.Y;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.useTurn = true;
            item.useAnimation = 15;
            item.useTime = 10;
            item.autoReuse = true;
            item.maxStack = 99;
            item.consumable = true;
            item.createTile = TileType;
            item.rare = Rare;
        }

        public sealed override void ModifyTooltips(List<TooltipLine> tooltips) => ItemUtils.CheckRarity(Rare, tooltips);
    }

    public abstract class ModPaintingItem : BaseTileItem
    {
        public ModPaintingItem(string name, int rare, Vector2 size, string author, string flavourText, int tileType) : base(name, rare, size, author + "\n" + flavourText, tileType) { }
    }

    public static class ItemUtils
    {
        public static void CheckRarity(int Rare, List<TooltipLine> tooltips)
        {
            //rarity 12 (Turquoise)
            //rarity 13 (Pure Green)
            //rarity 14 (Dark Blue)
            //rarity 15 (Violet)
            //rarity 16 (Hot Pink/Developer)
            //rarity 17 rainbow (no expert tag on item)
            //rarity 18 rare variant
            //rarity 19 dedicated(patron items)
            //look at https://calamitymod.gamepedia.com/Rarity to know where to use the colors

            //rarity 20 Our Dev things (dedicated??)
            if (Rare <= 11)
                return;

            Color color;

            switch (Rare)
            {
                case 12:
                    color = new Color(0, 255, 200); //Turquoise
                    break;
                case 13:
                    color = new Color(0, 255, 0); //Pure Green
                    break;
                case 14:
                    color = new Color(43, 96, 222); //Dark Blue
                    break;
                case 15:
                    color = new Color(108, 45, 199); //Violet
                    break;
                case 16:
                    color = new Color(255, 0, 255); //Hot Pink/Developer
                    break;
                case 17:
                    color = new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB); //rainbow (no expert tag on item)
                    break;
                case 18:
                    color = new Color(255, 140, 0); //rare variant
                    break;
                case 19:
                    color = new Color(139, 0, 0); //dedicated(patron items) (calamity)
                    break;
                case 20:
                    color = new Color(107, 240, 255); //dedicated (CalValEX)
                    break;
                default:
                    color = Color.White; //if all else fails give it CUM
                    break;
            }

            foreach (TooltipLine tooltipLine in tooltips) if (tooltipLine.mod == "Terraria" && tooltipLine.Name == "ItemName") tooltipLine.overrideColor = color;
        }
    }
}
