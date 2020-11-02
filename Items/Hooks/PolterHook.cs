using CalValEXLite.Projectiles.Hooks;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace CalValEXLite.Items.Hooks
{
    public class PolterHook : ModHookItem
    {
        public PolterHook() : base("Ghastly Chains", "Let the eternally suffering souls pull you forward!", ModContent.ProjectileType<PolterHook_Hook>(), Item.sellPrice(1, 1, 0, 0), 13, 18f, new Vector2(32, 46)) { }
    }
}
