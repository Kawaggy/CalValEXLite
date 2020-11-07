using CalValEXLite.Items.Critters;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalValEXLite.NPCs.Critters
{
    public class Isopod : ModCritter
    {
        public Isopod() : base("Abyssal Isopod", 2000, new Vector2(56, 26), 0.5f, 67, NPCID.GlowingSnail, ModContent.ItemType<IsopodItem>(), 1) { }

        public override void SafeSetStaticDefaults()
        {
            Main.npcFrameCount[npc.type] = 8;
            Main.npcCatchable[npc.type] = true;
        }

        public override void SafeSetDefaults()
        {
            animationType = NPCID.GlowingSnail;
            npc.HitSound = SoundID.NPCHit38;

            if (CalValEXLite.calamityMod != null)
            {
                for (int i = 0; i < npc.buffImmune.Length; i++)
                {
                    npc.buffImmune[CalValEXLite.calamityMod.BuffType("CrushDepth")] = true;
                }
            }
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (CalValEXLite.calamityMod == null)
                return 0f;

            Mod clam = CalValEXLite.calamityMod;

            if ((bool)clam.Call("GetInZone", Main.player[Main.myPlayer], "layer4"))
                if (spawnInfo.water)
                    return SpawnCondition.CaveJellyfish.Chance * 1.2f;

            return 0f;
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (npc.life <= 0)
                for (int i = 1; i < 4; i++)
                    Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Isopod/Isopod_Gore" + i), 1f);
        }
    }
}
