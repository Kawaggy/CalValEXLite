using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalValEXLite
{
    public abstract class ModHookProjectile : ModProjectile
    {
        private readonly string HookName;
        private readonly int HooksMax;
        private readonly int HooksOutMax;
        private readonly float HookLength;
        private readonly float RetreatSpeed;
        private readonly float PullSpeed;
        private readonly string TexturePath;

        public ModHookProjectile(string name, int maxHooks, int maxHooksOut, float hookLength, float retreatSpeed, float pullSpeed, string texturePath)
        {
            HookName = name;
            HooksMax = maxHooks;
            HooksOutMax = maxHooksOut;
            HookLength = hookLength;
            RetreatSpeed = retreatSpeed;
            PullSpeed = pullSpeed;
            TexturePath = texturePath;
        }

        public sealed override void SetStaticDefaults()
        {
            DisplayName.SetDefault(HookName);
        }

        public virtual void SafeSetDefaults() { }
        public sealed override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.GemHookAmethyst);
            SafeSetDefaults();
        }

        public sealed override bool? CanUseGrapple(Player player)
        {
            int hooksOut = 0;
            int hooksOnTile = 0;
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                if (Main.projectile[i].active && Main.projectile[i].owner == Main.myPlayer && Main.projectile[i].type == projectile.type)
                {
                    if (Main.projectile[i].ai[0] != 2)
                    {
                        if (OnlyOneHookOut)
                            return false;
                    }
                    else
                        hooksOnTile++;
                    hooksOut++;
                }
            }
            if (hooksOnTile == HooksOutMax && hooksOut <= HooksOutMax)
                return true;
            if (hooksOut > HooksOutMax - 1)
                return false;
            return true;
        }

        public virtual bool OnlyOneHookOut => false;

        public sealed override float GrappleRange() => HookLength;

        public sealed override void NumGrappleHooks(Player player, ref int numHooks) => numHooks = HooksMax;

        public sealed override void GrapplePullSpeed(Player player, ref float speed) => speed = PullSpeed;

        public sealed override void GrappleRetreatSpeed(Player player, ref float speed) => speed = RetreatSpeed;

        public sealed override bool PreDrawExtras(SpriteBatch spriteBatch) => projectile.DrawHook(spriteBatch, TexturePath);
    }

    public abstract class ModWorm : ModProjectile
    {
        public readonly int Size;
        public readonly string PartName;

        /// <summary>
        /// Makes a ModProjectile that has common Worm Part things
        /// </summary>
        /// <param name="size">This projectiles' size. It sets both width and height</param>
        /// <param name="name">This parts' name</param>
        /// <param name="pet">If this is part of a pet</param>
        public ModWorm(int size, string name)
        {
            Size = size;
            PartName = name;
        }

        public virtual bool SafePreSetStaticDefaults() { return true; }
        public virtual void SafePostSetStaticDefaults() { }
        public sealed override void SetStaticDefaults()
        {
            if (!SafePreSetStaticDefaults())
                return;
            DisplayName.SetDefault(PartName);
            SafePostSetStaticDefaults();
        }

        public virtual bool SafePreSetDefaults() { return true; }
        public virtual void SafePostSetDefaults() { }
        public sealed override void SetDefaults()
        {
            if (SafePreSetDefaults())
                return;
            projectile.netImportant = true;
            projectile.width = projectile.height = Size;
            projectile.aiStyle = -1;
            projectile.penetrate = -1;
            projectile.timeLeft *= 5;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.ignoreWater = true;
            projectile.tileCollide = false;
            projectile.alpha = 255;
            SafePostSetDefaults();
        }

        /// <summary>
        /// This projectiles' localAI array is already getting synqued
        /// </summary>
        /// <param name="writer"></param>
        public virtual void SafeSendExtraAI(BinaryWriter writer) { }
        public sealed override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(projectile.localAI[0]);
            writer.Write(projectile.localAI[1]);
            SafeSendExtraAI(writer);
        }

        /// <summary>
        /// This projectiles' localAI array is already getting synqued
        /// </summary>
        /// <param name="reader"></param>
        public virtual void SafeReceiveExtraAI(BinaryReader reader) { }
        public sealed override void ReceiveExtraAI(BinaryReader reader)
        {
            projectile.localAI[0] = reader.ReadSingle();
            projectile.localAI[1] = reader.ReadSingle();
            SafeReceiveExtraAI(reader);
        }
    }

    public abstract class ModWormHead : ModWorm
    {
        private readonly int BodyType;
        private readonly int TailType;
        private readonly int SegmentCount;

        /// <summary>
        /// Makes a new Worm Head
        /// </summary>
        /// <param name="size">This projectiles' size. It sets both width and height</param>
        /// <param name="name">This parts' name</param>
        /// <param name="bodyType">The type of this worms' body</param>
        /// <param name="tailType">The type of this worms' tail</param>
        /// <param name="pet">If this is part of a pet</param>
        /// <param name="segmentCount">How many segments it spawns, if its -1 it'll spawn the minimum amount (1 head, 1 body and 1 tail)</param>
        public ModWormHead(int size, string name, int bodyType, int tailType, int segmentCount = -1) : base(size, name)
        {
            BodyType = bodyType;
            TailType = tailType;
            SegmentCount = segmentCount;
        }

        /// <summary>
        /// For buff things
        /// </summary>
        /// <param name="player">The Projectiles' player owner</param>
        public virtual void PetFunctionality(Player player)
        {
            projectile.timeLeft = 30;
        }
        public virtual void AddDust() { }

        /// <summary>
        /// For segment spawning logic
        /// </summary>
        /// <param name="player">The player owner</param>
        public virtual void SpawnSegmentsLogic(Player player)
        {
            if (projectile.ai[0] == 0)
            {
                if (SegmentCount != -1)
                {
                    projectile.localAI[1] = SegmentCount;
                    projectile.ai[0] = Projectile.NewProjectile(player.Center, Vector2.Zero, BodyType, projectile.damage, projectile.knockBack, projectile.owner);
                    Main.projectile[(int)projectile.ai[0]].ai[1] = projectile.whoAmI;
                    Main.projectile[(int)projectile.ai[0]].localAI[1] = projectile.localAI[1] - 1f;
                    projectile.netUpdate = true;
                }
                else
                {
                    projectile.ai[0] = Projectile.NewProjectile(player.Center, Vector2.Zero, BodyType, projectile.damage, projectile.knockBack, projectile.owner);
                    Main.projectile[(int)projectile.ai[0]].ai[1] = projectile.whoAmI;
                    projectile.netUpdate = true;
                    Main.projectile[(int)projectile.ai[0]].ai[0] = Projectile.NewProjectile(player.Center, Vector2.Zero, TailType, projectile.damage, projectile.knockBack, projectile.owner);
                    Main.projectile[(int)Main.projectile[(int)projectile.ai[0]].ai[0]].ai[1] = Main.projectile[(int)projectile.ai[0]].whoAmI;
                    Main.projectile[(int)projectile.ai[0]].netUpdate = true;
                }
            }
        }

        public virtual bool DespawnLogic()
        {
            if (!Main.projectile[(int)projectile.ai[0]].active)
            {
                projectile.active = false;
                return false;
            }
            return true;
        }

        public sealed override void AI()
        {
            projectile.tileCollide = false;
            Player player = Main.player[projectile.owner];

            if ((int)Main.time % 120 == 0) projectile.netUpdate = true;

            if (!player.active) { projectile.active = false; return; }

            AddDust();
            PetFunctionality(player);
            SpawnSegmentsLogic(player);

            if (!DespawnLogic())
                return;

            Vector2 center = player.Center;

            if (Main.myPlayer == player.whoAmI && projectile.Distance(player.Center) > 2000f)
            {
                projectile.position = center;
                projectile.velocity *= 0.1f;
                projectile.netUpdate = true;
            }

            projectile.DontOverlap(0.04f);

            IdleAI(center);

            projectile.rotation = projectile.velocity.ToRotation() + (float)Math.PI / 2f;
            int direction = projectile.direction;
            projectile.direction = projectile.spriteDirection = projectile.velocity.X > 0f ? 1 : -1;
            if (direction != projectile.direction)
                projectile.netUpdate = true;

            projectile.position = projectile.Center;
            projectile.width = projectile.height = (int)(Size * projectile.scale);
            projectile.Center = projectile.position;
            if (projectile.alpha > 0)
            {
                projectile.alpha -= 42;
                if (projectile.alpha < 0)
                    projectile.alpha = 0;
            }
        }

        /// <summary>
        /// Setups the speed for the worm projectile idle movement.
        /// </summary>
        /// <param name="vectorToPlayerCenter">Vector to the Players' center</param>
        /// <returns>The idle speed</returns>
        public virtual float IdleSpeedSetup(Vector2 vectorToPlayerCenter)
        {
            if (vectorToPlayerCenter.Length() > 200f)
                return 0.2f;
            if (vectorToPlayerCenter.Length() > 140f)
                return 0.12f;
            return 0.06f;
        }

        /// <summary>
        /// Max speed for idle movement. Default is half of max speed
        /// </summary>
        /// <returns>Max idle speed</returns>
        public virtual float MaxIdleSpeedSetup() => 15f;
        public virtual float MinIdleMovementDist() => 100f;
        private void IdleAI(Vector2 center)
        {
            Vector2 vectorToPlayerCenter = center - projectile.Center;
            float speed = IdleSpeedSetup(vectorToPlayerCenter);
            if (vectorToPlayerCenter.Length() > MinIdleMovementDist())
            {
                if (Math.Abs(center.X - projectile.Center.X) > 20f)
                    projectile.velocity.X += speed * Math.Sign(center.X - projectile.Center.X);
                if (Math.Abs(center.Y - projectile.Center.Y) > 20f)
                    projectile.velocity.Y += speed * Math.Sign(center.Y - projectile.Center.Y);
            }
            else if (projectile.velocity.Length() > 2f)
            {
                projectile.velocity *= 0.96f;
            }

            if (Math.Abs(projectile.velocity.Y) < 1f)
                projectile.velocity.Y -= 0.1f;

            float maxSpeed = MaxIdleSpeedSetup();
            if (projectile.velocity.Length() > maxSpeed)
                projectile.velocity = Vector2.Normalize(projectile.velocity) * maxSpeed;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor) => projectile.DrawWorm(spriteBatch, drawColor, Texture);
    }

    public abstract class ModWormBodyTail : ModWorm
    {
        private readonly int BodyType;
        private readonly int TailType;
        /// <summary>
        /// Makes a new Worm Head or Tail
        /// </summary>
        /// <param name="size">This projectiles' size. It sets both width and height</param>
        /// <param name="name">This parts' name</param>
        /// <param name="bodyType">The type of this worms' body</param>
        /// <param name="tailType">The type of this worms' tail</param>
        /// <param name="pet">If this is part of a pet</param>
        public ModWormBodyTail(int size, string name, int bodyType, int tailType) : base(size, name)
        {
            BodyType = bodyType;
            TailType = tailType;
        }

        /// <summary>
        /// For buff things
        /// </summary>
        /// <param name="player">This projectiles' Player owner</param>
        public virtual void PetFunctionality(Player player)
        {
            projectile.timeLeft = 30;
        }
        public virtual void AddDust() { }

        public virtual bool DespawnLogic()
        {
            if (projectile.type == BodyType)
            {
                if (!Main.projectile[(int)projectile.ai[0]].active || !Main.projectile[(int)projectile.ai[1]].active)
                {
                    projectile.active = false;
                    return false;
                }
            }
            else if (projectile.type == TailType)
            {
                if (!Main.projectile[(int)projectile.ai[1]].active)
                {
                    projectile.active = false;
                    return false;
                }
            }
            return true;
        }

        public virtual void SpawnSegmentLogic(Player player)
        {
            if (projectile.ai[0] == 0)
            {
                if (projectile.type == BodyType)
                {
                    if (projectile.localAI[1] > 1)
                    {
                        projectile.ai[0] = Projectile.NewProjectile(player.Center, Vector2.Zero, BodyType, projectile.damage, projectile.knockBack, projectile.owner);
                        Main.projectile[(int)projectile.ai[0]].ai[1] = projectile.whoAmI;
                        Main.projectile[(int)projectile.ai[0]].localAI[1] = projectile.localAI[1] - 1f;
                        projectile.netUpdate = true;
                    }
                    else
                    {
                        projectile.ai[0] = Projectile.NewProjectile(player.Center, Vector2.Zero, TailType, projectile.damage, projectile.knockBack, projectile.owner);
                        Main.projectile[(int)projectile.ai[0]].ai[1] = projectile.whoAmI;
                        Main.projectile[(int)projectile.ai[0]].localAI[1] = projectile.localAI[1] - 1f;
                        projectile.netUpdate = true;
                    }
                }
            }
        }

        public sealed override void AI()
        {
            projectile.tileCollide = false;
            Player player = Main.player[projectile.owner];

            if ((int)Main.time % 120 == 0) projectile.netUpdate = true;

            if (!player.active) { projectile.active = false; return; }

            AddDust();
            PetFunctionality(player);
            SpawnSegmentLogic(player);

            if (!DespawnLogic())
                return;

            Vector2 center = Main.projectile[(int)projectile.ai[1]].Center;
            float rotation = Main.projectile[(int)projectile.ai[1]].rotation;
            projectile.friendly = Main.projectile[(int)projectile.ai[1]].friendly;

            if (projectile.alpha > 0)
            {
                projectile.alpha -= 42;
                if (projectile.alpha < 0)
                    projectile.alpha = 0;
            }

            projectile.velocity = Vector2.Zero;
            Vector2 vectorToSegmentAheadCenter = center - projectile.Center;
            if (rotation != projectile.rotation)
            {
                float angle = MathHelper.WrapAngle(rotation - projectile.rotation);
                vectorToSegmentAheadCenter = vectorToSegmentAheadCenter.RotatedBy(angle * 0.1f);
            }

            projectile.rotation = vectorToSegmentAheadCenter.ToRotation() + (float)Math.PI / 2f;
            projectile.position = projectile.Center;
            projectile.scale = Main.projectile[(int)projectile.ai[0]].scale;
            projectile.width = projectile.height = (int)(Size * projectile.scale);
            projectile.Center = projectile.position;
            if (vectorToSegmentAheadCenter != Vector2.Zero)
                projectile.Center = center - Vector2.Normalize(vectorToSegmentAheadCenter) * Size * projectile.scale;
            projectile.spriteDirection = vectorToSegmentAheadCenter.X > 0f ? 1 : -1;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor) => projectile.DrawWorm(spriteBatch, drawColor, Texture);
    }

    public abstract class PetBase : ModProjectile
    {
        public readonly float Speed;
        public readonly float Inertia;

        public PetBase(float speed, float inertia)
        {
            Speed = speed;
            Inertia = inertia;
        }

        public virtual void SafeSetDefaults() { }
        public sealed override void SetDefaults()
        {
            SafeSetDefaults();
            projectile.penetrate = -1;
            projectile.netImportant = true;
            projectile.timeLeft *= 5;
            projectile.friendly = true;
            projectile.tileCollide = false;
            projectile.aiStyle = -1;
        }

        public virtual bool FacesLeft => false;
        public virtual bool ShouldFlip => true;
        public virtual string AuraTexture => null;
        public virtual string AuraGlowTexture => null;
        public virtual string Glowmask => null;
        public virtual float AuraRotation => 0f;
        public float rotation;
        public virtual Vector2 Offset => default;
        public virtual bool ShouldFlyRotate => true;
        public virtual float TeleportDistance => 2000f;
        public virtual void AddLight() { }
        public abstract void PetFunctionality(Player player);
        public virtual void AddAnimation() { }
        public int State { get => (int)projectile.localAI[1]; set => projectile.localAI[1] = value; }

        /// <summary>
        /// Gets called before normal behaviour happens. Use State (int) to know what State it is.
        /// </summary>
        /// <param name="player">This projectiles' owner</param>
        public virtual void CustomBehaviour(Player player) { }
        public virtual bool SafePreDraw(SpriteBatch spriteBatch, Color lightColor) { return true; }
        public sealed override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            if (!SafePreDraw(spriteBatch, lightColor))
                return false;
            if (AuraTexture != null)
            {
                Texture2D auraTexture = ModContent.GetTexture(AuraTexture);
                Rectangle sourceRectangle = new Rectangle(0, 0, auraTexture.Width, auraTexture.Height);
                Vector2 origin = new Vector2(auraTexture.Width, auraTexture.Height);
                origin /= 2;
                spriteBatch.Draw(auraTexture, projectile.Center - Main.screenPosition, sourceRectangle, lightColor, rotation, origin, projectile.scale, SpriteEffects.None, 0);
                if (AuraGlowTexture != null)
                {
                    Texture2D auraGlowmaskTexture = ModContent.GetTexture(AuraGlowTexture);
                    Rectangle auraSourceRectangle = new Rectangle(0, 0, auraGlowmaskTexture.Width, auraGlowmaskTexture.Height);
                    Vector2 auraOrigin = new Vector2(auraGlowmaskTexture.Width, auraGlowmaskTexture.Height);
                    auraOrigin /= 2;
                    spriteBatch.Draw(auraGlowmaskTexture, projectile.Center - Main.screenPosition, auraSourceRectangle, lightColor, rotation, auraOrigin, projectile.scale, SpriteEffects.None, 0);
                }
            }
            return base.PreDraw(spriteBatch, lightColor);
        }

        public virtual void SafePostDraw(SpriteBatch spriteBatch, Color lightColor) { }
        public sealed override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            if (Glowmask != null)
            {
                Texture2D glowmaskTexture = ModContent.GetTexture(Glowmask);
                Rectangle rectangle = new Rectangle(0, 0, glowmaskTexture.Width, glowmaskTexture.Height);
                spriteBatch.Draw(glowmaskTexture, projectile.Center - Main.screenPosition, rectangle, Color.White, projectile.rotation, projectile.Size / 2f, projectile.scale, SpriteEffects.None, 0f);
            }
            SafePostDraw(spriteBatch, lightColor);
            base.PostDraw(spriteBatch, lightColor);
        }

        public virtual void SafeSendExtraAI(BinaryWriter writer) { }
        public sealed override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(projectile.localAI[0]);
            writer.Write(projectile.localAI[1]);

            SafeSendExtraAI(writer);
        }

        public virtual void SafeReceiveExtraAI(BinaryReader reader) { }
        public sealed override void ReceiveExtraAI(BinaryReader reader)
        {
            projectile.localAI[0] = reader.ReadSingle();
            projectile.localAI[1] = reader.ReadSingle();

            SafeReceiveExtraAI(reader);
        }
    }

    public abstract class FlyingPet : PetBase
    {
        public FlyingPet(float speed, float inertia) : base(speed, inertia) { }

        public override Vector2 Offset => new Vector2(48f * -Main.player[projectile.owner].direction, -50f);

        public const int Flying = 0;
        public sealed override void AI()
        {
            Player player = Main.player[projectile.owner];
            PetFunctionality(player);

            if (!player.active)
            {
                projectile.active = false;
                return;
            }

            Vector2 vectorToPlayer = player.Center + Offset - projectile.Center;
            float distanceToPlayer = vectorToPlayer.Length();

            if(ShouldFlip)
            {
                if (FacesLeft)
                    projectile.spriteDirection = projectile.velocity.X > 0 ? -1 : 1;
                else
                    projectile.spriteDirection = projectile.velocity.X > 0 ? 1 : -1;
            }

            if (projectile.Distance(player.Center) > TeleportDistance)
            {
                projectile.position = player.Center;
                projectile.velocity *= 0.1f;
                projectile.netUpdate = true;
            }

            CustomBehaviour(player);
            switch(State)
            {
                case Flying:
                    projectile.tileCollide = false;

                    if (distanceToPlayer > 20f)
                    {
                        vectorToPlayer.Normalize();
                        vectorToPlayer *= Speed;
                        projectile.velocity = (projectile.velocity * (Inertia - 1) + vectorToPlayer) / Inertia;
                    }
                    else if(projectile.velocity == Vector2.Zero)
                    {
                        projectile.velocity.X = Main.rand.NextBool().ToDirectionInt() * 0.15f;
                        projectile.velocity.Y = Main.rand.NextBool().ToDirectionInt() * 0.15f;
                        projectile.netUpdate = true;
                    }

                    if (ShouldFlyRotate)
                        projectile.rotation = projectile.velocity.X * 0.1f;
                    break;
            }
            AddLight();
            rotation += AuraRotation;

            AddAnimation();
        }
    }

    public abstract class WalkingPet : PetBase
    {
        public WalkingPet(float walkingSpeed, float flyingSpeed) : base(0f, 0f)
        {
            WalkingSpeed = walkingSpeed;
            FlyingSpeed = flyingSpeed;
        }
        private readonly float FlyingSpeed;
        private readonly float WalkingSpeed;

        /// <summary>
        /// The range that this pet will start to walk. It'll be used as the value for FlyingRange and BackToWalkingRange if their values is not set (multiplied by a constant value).
        /// </summary>
        public virtual int WalkingRange => 85;
        public virtual int FlyingRange => -1;
        public virtual int BackToWalkingRange => -1;
        public virtual float FlyingSpeedMult => 1.5f;
        public virtual float WalkingSpeedMult => 0.25f;
        public virtual float Gravity => 0.4f;
        public virtual bool CanFly => true;
        public virtual bool ConstantJump => false;

        public const float flyingRangeMultiplier = 5.88235294f;
        public const float backToWalkingRangeMultiplier = 2.35294118f;
        public const int Walking = 0;
        public const int Flying = 1;
        public sealed override void AI()
        {
            Player player = Main.player[projectile.owner];
            PetFunctionality(player);

            if (!player.active)
            {
                projectile.active = false;
                return;
            }

            bool left = false;
            bool right = false;
            bool flag = false;
            bool jump = false;
            int range = WalkingRange;
            if (player.position.X + (player.width / 2) < projectile.position.X + (projectile.width / 2) - range)
            {
                left = true;
            }
            else if (player.position.X + (player.width / 2) > projectile.position.X + (projectile.width / 2) + range)
            {
                right = true;
            }

            int flyingRange = (int)(WalkingRange * flyingRangeMultiplier);
            if (FlyingRange != -1)
                flyingRange = FlyingRange;
            if (State == Flying || State == Walking)
            {
                if (CanFly)
                {
                    if (player.rocketDelay2 > 0)
                        State = Flying;
                }
            }

            Vector2 projectileCenter = new Vector2(projectile.position.X + (projectile.width * 0.5f), projectile.position.Y + (projectile.height * 0.5f));
            float playerX = player.position.X + (player.width / 2) - projectileCenter.X;
            float playerY = player.position.Y + (player.height / 2) - projectile.Center.Y;
            float sqrt = (float)Math.Sqrt(playerX * playerX + playerY * playerY);

            if (projectile.Distance(player.Center) > TeleportDistance)
            {
                projectile.position = player.Center;
                projectile.velocity *= 0.1f;
                projectile.netUpdate = true;
            }

            if (State == Flying || State == Walking)
            {
                if (CanFly)
                {
                    if (sqrt > flyingRange && sqrt < TeleportDistance)
                    {
                        if (playerY > 0f && projectile.velocity.Y < 0f)
                            projectile.velocity.Y = 0f;
                        if (playerY < 0f && projectile.velocity.Y > 0f)
                            projectile.velocity.Y = 0f;

                        State = Flying;
                    }
                }
            }
            CustomBehaviour(player);
            switch(State)
            {
                case Walking:
                    projectile.tileCollide = true;

                    if (ShouldFlyRotate)
                        projectile.rotation = 0;

                    if (left)
                    {
                        if (projectile.velocity.X > -3.5)
                            projectile.velocity.X -= WalkingSpeed;
                        else
                            projectile.velocity.X -= WalkingSpeed * WalkingSpeedMult;
                    }
                    else if (right)
                    {
                        if (projectile.velocity.X < 3.5)
                            projectile.velocity.X += WalkingSpeed;
                        else
                            projectile.velocity.X += WalkingSpeed * WalkingSpeedMult;
                    }
                    else
                    {
                        projectile.velocity.X *= 0.9f;
                        if (projectile.velocity.X >= 0f - WalkingSpeed && projectile.velocity.X <= WalkingSpeed)
                            projectile.velocity.X = 0f;
                    }

                    if (left || right)
                    {
                        if (ShouldFlip)
                        {
                            if (FacesLeft)
                                projectile.spriteDirection = projectile.velocity.X > 0 ? -1 : 1;
                            else
                                projectile.spriteDirection = projectile.velocity.X > 0 ? 1 : -1;
                        }

                        int i = (int)(projectile.position.X + (projectile.width / 2)) / 16;
                        int j = (int)(projectile.position.Y + (projectile.height / 2)) / 16;

                        if (left)
                            i--;

                        if (right)
                            i++;

                        i += (int)projectile.velocity.X;
                        if (WorldGen.SolidTile(i, j))
                            jump = true;
                    }

                    if (player.position.Y + player.head - 8f > projectile.position.Y + projectile.height)
                        flag = true;
                    Collision.StepUp(ref projectile.position, ref projectile.velocity, projectile.width, projectile.height, ref projectile.stepSpeed, ref projectile.gfxOffY);
                    if (projectile.velocity.Y == 0)
                    {
                        if (!flag && (projectile.velocity.X < 0f || projectile.velocity.X > 0f))
                        {
                            int i = (int)(projectile.position.X + (projectile.width / 2)) / 16;
                            int j = (int)(projectile.position.Y + (projectile.height / 2)) / 16 + 1;

                            if (left)
                                i--;
                            if (right)
                                i++;

                            WorldGen.SolidTile(i, j);
                        }

                        if (jump)
                        {
                            int i = (int)(projectile.position.X + (projectile.width / 2)) / 16;
                            int j = (int)(projectile.position.Y + projectile.height) / 16 + 1;
                            if (WorldGen.SolidTile(i, j) || Main.tile[i, j].halfBrick() || Main.tile[i, j].slope() > 0)
                            {
                                try
                                {
                                    i = (int)(projectile.position.X + (projectile.width / 2)) / 16;
                                    j = (int)(projectile.position.Y + (projectile.height / 2)) / 16;
                                    if (left)
                                        i--;
                                    if (right)
                                        i++;

                                    i += (int)projectile.velocity.X;

                                    if (!WorldGen.SolidTile(i, j - 1) && !WorldGen.SolidTile(i, j - 2))
                                        projectile.velocity.Y = -5.1f;
                                    else if (!WorldGen.SolidTile(i, j - 2))
                                        projectile.velocity.Y = -7.1f;
                                    else if (WorldGen.SolidTile(i, j - 5))
                                        projectile.velocity.Y = -11.1f;
                                    else if (WorldGen.SolidTile(i, j - 4))
                                        projectile.velocity.Y = -10.1f;
                                    else
                                        projectile.velocity.Y = -9.1f;
                                }
                                catch
                                {
                                    projectile.velocity.Y = -9.1f;
                                }
                            }
                        }
                        else if (ConstantJump)
                        {
                            projectile.velocity.Y = -9.1f;
                        }
                    }

                    float speedLimit = 6.5f;
                    if (projectile.velocity.X > speedLimit)
                        projectile.velocity.X = speedLimit;

                    if (projectile.velocity.X < 0f - speedLimit)
                        projectile.velocity.X = 0f - speedLimit;
                    if (projectile.velocity.X < 0f)
                        projectile.direction = -1;

                    if (projectile.velocity.X > 0f)
                        projectile.direction = 1;

                    if (projectile.velocity.X > WalkingSpeed && right)
                        projectile.direction = 1;

                    if (projectile.velocity.X < 0f - WalkingSpeed && left)
                        projectile.direction = -1;

                    projectile.velocity.Y += Gravity;

                    if (projectile.velocity.Y > 10f)
                        projectile.velocity.Y = 10f;
                    break;
                case Flying:
                    if (!CanFly)
                    {
                        State = Walking;
                    }
                    projectile.tileCollide = false;
                    int backToWalkingRange = (int)(WalkingRange * backToWalkingRangeMultiplier);
                    if (BackToWalkingRange != -1)
                        backToWalkingRange = BackToWalkingRange;

                    projectileCenter = new Vector2(projectile.position.X + (projectile.width * 0.5f), projectile.position.Y + (projectile.height * 0.5f));
                    playerX = player.position.X + (player.width / 2) - projectileCenter.X;
                    playerY = player.position.Y + (player.height / 2) - projectileCenter.Y;
                    sqrt = (float)Math.Sqrt(playerX * playerX + playerY * playerY);

                    if (sqrt < backToWalkingRange && player.velocity.Y == 0f && projectile.position.Y + projectile.height <= player.position.Y + player.height && !Collision.SolidCollision(projectile.position, projectile.width, projectile.height))
                    {
                        State = Walking;
                        if (projectile.velocity.Y < -6f)
                            projectile.velocity.Y = -6f;
                    }

                    if (sqrt < 60f)
                    {
                        playerX = projectile.velocity.X;
                        playerY = projectile.velocity.Y;
                    }
                    else
                    {
                        sqrt = 10f / sqrt;
                        playerX *= sqrt;
                        playerY *= sqrt;
                    }

                    if (projectile.velocity.X < playerX)
                    {
                        projectile.velocity.X += FlyingSpeed;
                        if (projectile.velocity.X < 0f)
                            projectile.velocity.X += FlyingSpeed * FlyingSpeedMult;
                    }

                    if (projectile.velocity.X > playerX)
                    {
                        projectile.velocity.X -= FlyingSpeed;
                        if (projectile.velocity.X > 0f)
                            projectile.velocity.X -= FlyingSpeed * FlyingSpeedMult;
                    }

                    if (projectile.velocity.Y < playerY)
                    {
                        projectile.velocity.Y += FlyingSpeed;
                        if (projectile.velocity.Y < 0f)
                            projectile.velocity.Y += FlyingSpeed * FlyingSpeedMult;
                    }

                    if (projectile.velocity.Y > playerY)
                    {
                        projectile.velocity.Y -= FlyingSpeed;
                        if (projectile.velocity.Y > 0f)
                            projectile.velocity.Y -= FlyingSpeed * FlyingSpeedMult;
                    }

                    if (ShouldFlip)
                    {
                        if (FacesLeft)
                            projectile.spriteDirection = projectile.velocity.X > 0 ? -1 : 1;
                        else
                            projectile.spriteDirection = projectile.velocity.X > 0 ? 1 : -1;
                    }

                    if (ShouldFlyRotate)
                        projectile.rotation = projectile.velocity.X * 0.1f;
                    break;
            }
            AddLight();
            rotation += AuraRotation;

            AddAnimation();
        }
    }

    public static class ProjectileUtils
    {
        public static void SpawnPet(this Player player, int projType) { if (player.ownedProjectileCounts[projType] <= 0 && player.whoAmI == Main.myPlayer) Projectile.NewProjectile(player.position, Vector2.Zero, projType, 0, 0f, player.whoAmI); }

        public static bool DrawHook(this Projectile projectile, SpriteBatch spriteBatch, string texturePath)
        {
            Player player = Main.player[projectile.owner];
            Vector2 distToProj = projectile.Center;
            float projRotation = projectile.AngleTo(player.MountedCenter) - 1.57f;
            bool doIDraw = true;
            Texture2D texture = CalValEXLite.Instance.GetTexture(texturePath);

            while (doIDraw)
            {
                float distance = (player.MountedCenter - distToProj).Length();
                if (distance < (texture.Height + 1))
                {
                    doIDraw = false;
                }
                else if (!float.IsNaN(distance))
                {
                    Color drawColor = Lighting.GetColor((int)distToProj.X / 16, (int)(distToProj.Y / 16f));
                    distToProj += projectile.DirectionTo(player.MountedCenter) * texture.Height;
                    spriteBatch.Draw(texture, distToProj - Main.screenPosition,
                        new Rectangle(0, 0, texture.Width, texture.Height), drawColor, projRotation,
                        Utils.Size(texture) / 2f, 1f, SpriteEffects.None, 0f);
                }

            }
            return false;
        }

        public static bool DrawWorm(this Projectile projectile, SpriteBatch spriteBatch, Color drawColor, string texturePath)
        {
            Texture2D texture = ModContent.GetTexture(texturePath);
            Rectangle sourceRect = new Rectangle(0, 0, texture.Width, texture.Height);
            Vector2 origin = new Vector2(texture.Width, texture.Height);
            origin /= 2;
            Vector2 position = projectile.Center;
            position -= Main.screenPosition;
            spriteBatch.Draw(texture, position, sourceRect, drawColor, projectile.rotation, origin, projectile.scale, SpriteEffects.None, 0);
            return false;
        }

        public static void DontOverlap(this Projectile projectile, float overlapVelocity)
        {
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile other = Main.projectile[i];
                if (i != projectile.whoAmI && other.active && other.owner == projectile.owner && Math.Abs(projectile.position.X - other.position.X) + Math.Abs(projectile.position.Y - other.position.Y) < projectile.width)
                {
                    if (projectile.position.X < other.position.X) projectile.velocity.X -= overlapVelocity; else projectile.velocity.X += overlapVelocity;
                    if (projectile.position.Y < other.position.Y) projectile.velocity.Y -= overlapVelocity; else projectile.velocity.Y += overlapVelocity;
                }
            }
        }

        public static void SpawnSegment(this Projectile projectile, Player player, int BodyType, int TailType)
        {
            if (projectile.ai[0] == 0)
            {
                if (projectile.type == BodyType)
                {
                    if (projectile.localAI[1] > 1)
                    {
                        projectile.ai[0] = Projectile.NewProjectile(player.Center, Vector2.Zero, BodyType, projectile.damage, projectile.knockBack, projectile.owner);
                        Main.projectile[(int)projectile.ai[0]].ai[1] = projectile.whoAmI;
                        Main.projectile[(int)projectile.ai[0]].localAI[1] = projectile.localAI[1] - 1f;
                        projectile.netUpdate = true;
                    }
                    else
                    {
                        projectile.ai[0] = Projectile.NewProjectile(player.Center, Vector2.Zero, TailType, projectile.damage, projectile.knockBack, projectile.owner);
                        Main.projectile[(int)projectile.ai[0]].ai[1] = projectile.whoAmI;
                        Main.projectile[(int)projectile.ai[0]].localAI[1] = projectile.localAI[1] - 1f;
                        projectile.netUpdate = true;
                    }
                }
            }
        }

        public static bool DespawnSegment(this Projectile projectile, int BodyType, int TailType)
        {
            if (projectile.type == BodyType)
            {
                if (!Main.projectile[(int)projectile.ai[0]].active || !Main.projectile[(int)projectile.ai[1]].active)
                {
                    projectile.active = false;
                    return false;
                }
            }
            else if (projectile.type == TailType)
            {
                if (!Main.projectile[(int)projectile.ai[1]].active)
                {
                    projectile.active = false;
                    return false;
                }
            }
            return true;
        }
    }
}
