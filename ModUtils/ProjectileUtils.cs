﻿using Microsoft.Xna.Framework;
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

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault(HookName);
        }

        public virtual void SafeSetDefaults() { }
        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.GemHookAmethyst);
            SafeSetDefaults();
        }

        public override bool? CanUseGrapple(Player player)
        {
            int hooksOut = 0;
            for (int i = 0; i < Main.maxProjectiles; i++)
                if (Main.projectile[i].active && Main.projectile[i].owner == Main.myPlayer && Main.projectile[i].type == projectile.type)
                    hooksOut++;

            if (hooksOut > HooksOutMax)
                return false;
            return true;
        }

        public override float GrappleRange() => HookLength;

        public override void NumGrappleHooks(Player player, ref int numHooks) => numHooks = HooksMax;

        public override void GrapplePullSpeed(Player player, ref float speed) => speed = PullSpeed;

        public override void GrappleRetreatSpeed(Player player, ref float speed) => speed = RetreatSpeed;

        public override bool PreDrawExtras(SpriteBatch spriteBatch) => projectile.DrawHook(spriteBatch, TexturePath);
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
        /// Setups the distance for the worm projectile NPC targetting.
        /// </summary>
        /// <returns>The max distance</returns>
        public virtual float DistSetup() => 800f;

        /// <summary>
        /// Setups the max speed for the worm.
        /// </summary>
        /// <returns>The max speed</returns>
        public virtual float MaxSpeedSetup() => 30f;

        /// <summary>
        /// Setups the speed for the worm projectile NPC targetting.
        /// </summary>
        /// <param name="vectorToNPCCenter">Vector to the NPCs' center</param>
        /// <returns>The speed</returns>
        public virtual float SpeedSetup(Vector2 vectorToNPCCenter)
        {
            if (vectorToNPCCenter.Length() > 600f)
                return 0.8f;
            if (vectorToNPCCenter.Length() > 300f)
                return 0.6f;
            return 0.4f;
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
        public virtual float MaxIdleSpeedSetup() => MaxSpeedSetup() / 2f;
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
            spriteBatch.Draw(texture, position, sourceRect, drawColor, projectile.rotation, origin, 1f, SpriteEffects.None, 0);
            return false;
        }
    }
}
