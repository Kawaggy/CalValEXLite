namespace CalValEXLite.Projectiles.Hooks
{
    public class ScavaHook_Hook : ModHookProjectile
    {
        public ScavaHook_Hook() : base("Scavenger Claw", 2, 2, 800f, 25f, 13f, "ExtraTextures/HookChains/ScavaHook_ExtraTexture") { }

        public override void SafeSetDefaults() => drawOffsetX = -8;

        public override bool OnlyOneHookOut => true;
    }
}
