using CalValEXLite.Projectiles.Hooks;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace CalValEXLite.Items.Hooks
{
    public class ProfanedHook : ModHookItem
    {
        public ProfanedHook() : base("Profaned Energy Hook", "Rattle the holy chains", ModContent.ProjectileType<ProfanedHook_Hook>(), Item.sellPrice(1, 1, 0, 0), 12, 16f, new Vector2(26, 32)) { }
    }
}
