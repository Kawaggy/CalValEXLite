using System.Linq;
using Terraria;

namespace CalValEXLite
{
    public static class MyUtils
    {
        public static CalValEXLitePlayer EX(this Player player) => player.GetModPlayer<CalValEXLitePlayer>();

        public static bool AnyBossAlive() => Main.npc.Any(n => n.active && n.boss && n.whoAmI < Main.maxNPCs);
        public static void MountNerf(Player player, float reduceDamageBy, float reduceHealthBy)
        {
            if (AnyBossAlive())
            {
                int calculateLife = (int)(player.statLifeMax2 * reduceHealthBy);
                player.statLifeMax2 -= calculateLife;
                player.allDamage -= reduceDamageBy;
            }
        }
    }
}
