using CalValEXLite.Buffs.Pets.Walking.Gem;
using CalValEXLite.Projectiles.Pets.Walking.Gem;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalValEXLite.Items.Pets.Walking.Gem
{
    public abstract class GemItem : PetItem
    {
        public GemItem(string gemName, int petType, int buffType) : base(gemName + " Geode", "May contain a scuttler", new Vector2(22, 22), 1, petType, buffType, Item.sellPrice(gold:1)) { }
        public sealed override void SafeSetDefaults() => item.UseSound = SoundID.NPCHit31;
    }

    public class AmberItem : GemItem
    {
        public AmberItem() : base("Amber", ModContent.ProjectileType<Amber>(), ModContent.BuffType<AmberBuff>()) { }
    }

    public class AmethistItem : GemItem
    {
        public AmethistItem() : base("Amethist", ModContent.ProjectileType<Amethist>(), ModContent.BuffType<AmethistBuff>()) { }
    }

    public class CrystalItem : GemItem
    {
        public CrystalItem() : base("Crystal", ModContent.ProjectileType<Crystal>(), ModContent.BuffType<CrystalBuff>()) { }
    }

    public class DiamondItem : GemItem
    {
        public DiamondItem() : base("Diamond", ModContent.ProjectileType<Diamond>(), ModContent.BuffType<DiamondBuff>()) { }
    }

    public class EmeraldItem : GemItem
    {
        public EmeraldItem() : base("Emerald", ModContent.ProjectileType<Emerald>(), ModContent.BuffType<EmeraldBuff>()) { }
    }

    public class RubyItem : GemItem
    {
        public RubyItem() : base("Ruby", ModContent.ProjectileType<Ruby>(), ModContent.BuffType<RubyBuff>()) { }
    }

    public class SaphireItem : GemItem
    {
        public SaphireItem() : base("Saphire", ModContent.ProjectileType<Saphire>(), ModContent.BuffType<SaphireBuff>()) { }
    }

    public class TopazItem : GemItem
    {
        public TopazItem() : base("Topaz", ModContent.ProjectileType<Topaz>(), ModContent.BuffType<TopazBuff>()) { }
    }
}
