using Microsoft.Xna.Framework;
using MonoMod.Cil;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalValEXLite
{
    public abstract class ModCritter : ModNPC
    {
        public virtual bool SafeAutoLoad(ref string name) { return true; }
        public sealed override bool Autoload(ref string name)
        {
            IL.Terraria.Wiring.HitWireSingle += HookStatue;
            return SafeAutoLoad(ref name);
        }

        private void HookStatue(ILContext il)
        {
            var c = new ILCursor(il);
            ILLabel[] targets = null;

            while (c.TryGotoNext(i => i.MatchSwitch(out targets)))
            {
                int offset = 0;
                if (c.Prev.MatchSub() && c.Prev.Previous.MatchLdcI4(out offset))
                {
                    ;
                }

                int case56Index = 56 - offset;
                if (case56Index < 0 || case56Index >= targets.Length || !(targets[case56Index] is ILLabel target))
                {
                    continue;
                }

                c.GotoLabel(target);

                c.GotoNext(i => i.MatchCall(typeof(Utils), nameof(Utils.SelectRandom)));

                c.EmitDelegate<Func<short[], short[]>>(arr =>
                {
                    Array.Resize(ref arr, arr.Length + 1);
                    arr[arr.Length - 1] = (short)npc.type;
                    return arr;
                });

                return;
            }

            throw new Exception("Hook location not found, switch(*) { case 56: ...}");
        }


        private readonly string CritterName;
        private readonly Vector2 Size;
        private readonly float NPCSlots;
        private readonly int AIStyle;
        private readonly int AIType;
        private readonly int CritterItem;
        private readonly int CatchStack;
        private readonly int MaxLife;

        public ModCritter(string name, int maxLife,Vector2 size, float npcSlots, int aiStyle, int aiType, int critterItem, int stack)
        {
            CritterName = name;
            MaxLife = maxLife;
            Size = size;
            NPCSlots = npcSlots;
            AIStyle = aiStyle;
            AIType = aiType;
            CritterItem = critterItem;
            CatchStack = stack;
        }

        public virtual void SafeSetStaticDefaults() { }
        public sealed override void SetStaticDefaults()
        {
            DisplayName.SetDefault(CritterName);
            SafeSetStaticDefaults();
        }

        public virtual void SafeSetDefaults() { }
        public sealed override void SetDefaults()
        {
            npc.lifeMax = MaxLife;
            npc.width = (int)Size.X;
            npc.height = (int)Size.Y;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.npcSlots = NPCSlots;
            npc.aiStyle = AIStyle;
            aiType = AIType;
            npc.catchItem = (short)CritterItem;

            SafeSetDefaults();
        }

        public override void OnCatchNPC(Player player, Item item) => item.stack = CatchStack;

        public override bool? CanBeHitByItem(Player player, Item item) => true;
        public override bool? CanBeHitByProjectile(Projectile projectile) => true;
    }
}
