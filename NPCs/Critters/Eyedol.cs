using CalValEXLite.Items.Critters;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalValEXLite.NPCs.Critters
{
    public class Eyedol : ModCritter
    {
        public Eyedol() : base("Eyedol", 250, new Vector2(28, 24), 0.25f, -1, -1, ModContent.ItemType<EyedolItem>(), 1) { }

        public override void SafeSetStaticDefaults()
        {
            Main.npcFrameCount[npc.type] = 4;
            Main.npcCatchable[npc.type] = true;
        }

        public override void SafeSetDefaults()
        {
            npc.damage = 0;
            npc.defense = 0;
            if (CalValEXLite.calamityMod != null)
            {
                npc.lifeMax = 100;
                if ((bool)CalValEXLite.calamityMod.Call("GetBossDowned", "providence"))
                {
                    npc.lifeMax = 500;
                }
            }
            npc.HitSound = SoundID.NPCHit33;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.lavaImmune = true;
        }

        private enum Frames : int
        {
            Up = 0,
            Rightup,
            Rightdown,
            Down
        }

        public override void FindFrame(int frameHeight)
        {
            npc.spriteDirection = npc.direction;
            npc.TargetClosest(true);
            Vector2 targetPosition = Main.player[npc.target].position;

            if (targetPosition.Y - npc.position.Y <= 0f)
            {
                npc.frame.Y = (int)Frames.Rightup * frameHeight;

                if (targetPosition.X - npc.position.X < 25 && targetPosition.X - npc.position.X > -25)
                {
                    npc.frame.Y = (int)Frames.Up * frameHeight;
                }
            }
            if (targetPosition.Y - npc.position.Y > 0f)
            {
                npc.frame.Y = (int)Frames.Rightdown * frameHeight;

                if (targetPosition.X - npc.position.X < 25 && targetPosition.X - npc.position.X > -25)
                {
                    npc.frame.Y = (int)Frames.Down * frameHeight;
                }
            }
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (CalValEXLite.calamityMod == null)
                return 0f;

            Mod clam = CalValEXLite.calamityMod;

            if ((bool)clam.Call("GetInZone", Main.player[Main.myPlayer], "crags"))
            {
                return 0.35f;
            }

            return 0f;
        }
    }
}
