using CalValEXLite.Projectiles.Hooks;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace CalValEXLite.Items.Hooks
{
    public class ScavaHook : ModHookItem
    {
        public ScavaHook() : base("Ancient Scavenger Claw", "Here to gouge out your eyes, runic style!", ModContent.ProjectileType<ScavaHook_Hook>(), Item.sellPrice(1, 1, 0, 0), 18, 19f, new Vector2(30, 48)) { }
    }
}
