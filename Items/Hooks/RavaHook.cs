using CalValEXLite.Projectiles.Hooks;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace CalValEXLite.Items.Hooks
{
    public class RavaHook : ModHookItem
    {
        public RavaHook() : base("Ravager Claw", "Here to gouge out your eyes", ModContent.ProjectileType<RavaHook_Hook>(), 1000, 8, 18f, new Vector2(46, 38)) { }
    }
}
