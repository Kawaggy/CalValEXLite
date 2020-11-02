namespace CalValEXLite.Projectiles.Hooks
{
    public class RavaHook_Hook : ModHookProjectile
    {
        public RavaHook_Hook() : base("Ravager Claw", 2, 2, 700f, 20f, 11f, "ExtraTextures/HookChains/RavaHook_ExtraTexture") { }

        public override void SafeSetDefaults() => drawOffsetX = -8;
    }
}
