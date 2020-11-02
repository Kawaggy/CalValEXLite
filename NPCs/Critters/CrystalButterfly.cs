using CalValEXLite.Items.Critters;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalValEXLite.NPCs.Critters
{
    public class CrystalButterfly : ModCritter
    {
        public CrystalButterfly() : base("Profaned Butterfly", 20, new Vector2(48, 40), 0.25f, 65, NPCID.GoldButterfly, ModContent.ItemType<CrystalButterflyItem>(), 1) { }

        public override void SafeSetStaticDefaults()
        {
            Main.npcFrameCount[npc.type] = 3;
            Main.npcCatchable[npc.type] = true;
        }

        public override void SafeSetDefaults()
        {
            npc.noGravity = true;
            npc.lavaImmune = true;
            npc.defense = 0;
            animationType = NPCID.GoldButterfly;

            for (int i = 0; i < npc.buffImmune.Length; i++)
                if (CalValEXLite.calamityMod != null)
                    npc.buffImmune[CalValEXLite.calamityMod.BuffType("HolyFlames")] = true;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.playerSafe || !NPC.downedMoonlord)
                return 0f;
            return SpawnCondition.OverworldHallow.Chance * 0.4f;
        }
    }
}
