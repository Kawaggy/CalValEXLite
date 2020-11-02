namespace CalValEXLite.Projectiles.Hooks
{
    public class MawHook_Hook : ModHookProjectile
    {
        public MawHook_Hook() : base("Maw Hook", 1, 1, 500f, 17f, 12f, "ExtraTextures/HookChains/MawHook_ExtraTexture") { }

        public override void SafeSetDefaults() => drawOffsetX = -6;
    }
}
