using CalValEXLite.Projectiles.Hooks;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace CalValEXLite.Items.Hooks
{
    public class MawHook : ModHookItem
    {
        public MawHook() : base ("Maw Hook", "Prehistoric pull power!", ModContent.ProjectileType<MawHook_Hook>(), 1000, 8, 18f, new Vector2(46, 38)) { }
    }
}
