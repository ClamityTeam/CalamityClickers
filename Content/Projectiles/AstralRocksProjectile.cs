using CalamityClickers.Commons.CatalystCrossmod;
using CalamityMod;
using CalamityMod.CalPlayer;
using ClickerClass;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace CalamityClickers.Content.Projectiles
{
    public class AstralRocksProjectile : ModProjectile, ILocalizedModType
    {
        public struct AstralRockRenderData
        {
            public int whoIam;

            public int frame;

            public Vector3 offsetFromPlayer;

            private const float z_view = -20f;

            public Vector2 GetDrawPosition(Vector2 center)
            {
                center += new Vector2(offsetFromPlayer.X, offsetFromPlayer.Y);
                float num = offsetFromPlayer.Z * 0.0157f;
                Vector2 vector = new Vector2(Main.screenPosition.X + (float)Main.screenWidth / 2f, Main.screenPosition.Y + (float)Main.screenHeight / 2f);
                return new Vector2(center.X - (1f - 20f / (num - -20f)) * (center.X - vector.X), center.Y - (1f - 20f / (num - -20f)) * (center.Y - vector.Y));
            }

            public float GetDrawScale(float baseScale)
            {
                float num = offsetFromPlayer.Z * 0.0157f;
                return baseScale * (20f / (num - -20f));
            }

            public AstralRockRenderData(int id, Vector3 pos, UnifiedRandom frameRand)
            {
                whoIam = id;
                offsetFromPlayer = pos;
                frame = frameRand.Next(8);
            }

            public AstralRockRenderData(int id, Vector3 pos, int frame)
            {
                whoIam = id;
                offsetFromPlayer = pos;
                this.frame = frame;
            }
        }

        public const int MaxAstrageldonRocks = 8;

        public const float MinRockRadius = 50f;

        public const float MaxRockRadius = 120f;

        public const int RandFrames = 8;

        public const int SussyFrame = 8;

        public int AuricTeslaFrame = 9;

        public const int FrameCount = 13;

        public int FrameTimer;

        public const float RockX = 1f;

        public const float RockX2 = -1f;

        public const float RockY = -1f;

        public const float RockY2 = -1f;

        public float rotation;

        public float rotation2;

        //public int shotDelay;

        public new string LocalizationCategory => "Projectiles.Clicker";
        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.friendly = true;
            Projectile.hide = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 6;
            Projectile.aiStyle = -1;
            Projectile.manualDirectionChange = true;
        }

        public override bool? CanCutTiles()
        {
            return false;
        }

        public override bool? CanHitNPC(NPC target)
        {
            if (NPCID.Sets.CountsAsCritter[target.type])
            {
                return false;
            }

            return null;
        }

        /*public void ShootRocksRogue(Vector2 velocity)
        {
            Player player = Main.player[Projectile.owner];
            CalamityPlayer val = player.Calamity();
            //player.GetModPlayer<CatalystPlayer>();
            UnifiedRandom unifiedRandom = new UnifiedRandom(player.name.GetHashCode());
            int type = ModContent.ProjectileType<AstralRocksRogueShot>();
            int damage = (int)((float)Projectile.damage * 0.2f);
            Projectile.netUpdate = true;
            for (int i = 0; i < 8; i++)
            {
                Vector3 vector = Vector3.Transform(new Vector3(Projectile.ai[0], 0f, 0f), Matrix.CreateFromYawPitchRoll(1f, -1f, rotation + MathF.PI / 4f * (float)i));
                Vector2 position = player.Center + new Vector2(0f - vector.X, vector.Y);
                int frame = (val.auricSet ? AuricTeslaFrame : unifiedRandom.Next(8));
                int num = Projectile.NewProjectile(Projectile.GetSource_FromAI(), position, velocity.RotatedBy(Main.rand.NextFloat(-0.3f, 0.3f)), type, damage, Projectile.knockBack, Projectile.owner);
                Main.projectile[num].frame = frame;
            }

            for (int j = 0; j < 8; j++)
            {
                Vector3 vector2 = Vector3.Transform(new Vector3(Projectile.ai[1], 0f, 0f), Matrix.CreateFromYawPitchRoll(-1f, -1f, rotation2 + MathF.PI / 4f * (float)j));
                Vector2 position2 = player.Center + new Vector2(0f - vector2.X, vector2.Y);
                int frame2 = (val.auricSet ? AuricTeslaFrame : unifiedRandom.Next(8));
                int num2 = Projectile.NewProjectile(Projectile.GetSource_FromAI(), position2, velocity.RotatedBy(Main.rand.NextFloat(-0.3f, 0.3f)), type, damage, Projectile.knockBack, Projectile.owner);
                Main.projectile[num2].frame = frame2;
            }
        }*/
        public void ShootRocksClicker(Vector2 velocity, int ring, int rock)
        {
            Player player = Main.player[Projectile.owner];
            CalamityPlayer val = player.Calamity();
            //player.GetModPlayer<CatalystPlayer>();
            UnifiedRandom unifiedRandom = new UnifiedRandom(player.name.GetHashCode());
            int type = ModContent.ProjectileType<AstralRocksProjectileFly>();
            int damage = (int)((float)Projectile.damage * 0.2f);

            //List<>
            switch (ring)
            {
                case 0:
                    Vector3 vector = Vector3.Transform(new Vector3(Projectile.ai[0], 0f, 0f), Matrix.CreateFromYawPitchRoll(1f, -1f, rotation + MathF.PI / 4f * (float)rock));
                    Vector2 position = player.Center + new Vector2(0f - vector.X, vector.Y);
                    int frame = (val.auricSet ? AuricTeslaFrame : unifiedRandom.Next(8));
                    int num = Projectile.NewProjectile(Projectile.GetSource_FromAI(), position, velocity.RotatedBy(Main.rand.NextFloat(-0.6f, 0.6f)), type, damage, Projectile.knockBack, Projectile.owner);
                    Main.projectile[num].frame = frame;
                    break;
                case 1:
                    Vector3 vector2 = Vector3.Transform(new Vector3(Projectile.ai[1], 0f, 0f), Matrix.CreateFromYawPitchRoll(-1f, -1f, rotation2 + MathF.PI / 4f * (float)(rock - 1)));
                    Vector2 position2 = player.Center + new Vector2(0f - vector2.X, vector2.Y);
                    int frame2 = (val.auricSet ? AuricTeslaFrame : unifiedRandom.Next(8));
                    int num2 = Projectile.NewProjectile(Projectile.GetSource_FromAI(), position2, velocity.RotatedBy(Main.rand.NextFloat(-0.6f, 0.6f)), type, damage, Projectile.knockBack, Projectile.owner);
                    Main.projectile[num2].frame = frame2;
                    break;
            }
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            CalamityClickersPlayer modPlayer = player.GetModPlayer<CalamityClickersPlayer>();
            if (player.dead)
            {
                modPlayer.setIntergelacticClicker = null;
            }

            if (modPlayer.setIntergelacticClicker == null)
            {
                Projectile.Kill();
                return;
            }

            if (Projectile.damage == modPlayer.CurrentRockDamage)
            {
                Projectile.timeLeft = 2;
            }

            Projectile.Center = player.Center;
            float num = player.velocity.Length();
            if (num > 10f)
            {
                num = 10f;
            }

            rotation += Projectile.ai[1] / 2350f + num * 0.01f;
            rotation2 += Projectile.ai[0] / 2350f + num * 0.007f;
            num = 3f;
            if (player.direction == -1)
            {
                /*Projectile.ai[0] += 3f;
                Projectile.ai[0] -= num;
                num *= 2f;
                Projectile.ai[1] += num;*/
                if (Projectile.ai[0] > 50f)
                {
                    Projectile.ai[0] -= num;
                    if (Projectile.ai[0] < 50f)
                    {
                        Projectile.ai[0] = 50f;
                    }
                }
                else if (Projectile.ai[0] < 50f)
                {
                    Projectile.ai[0] += 3f;
                }

                num *= 2f;
                if (Projectile.ai[1] < 120f)
                {
                    Projectile.ai[1] += num;
                    if (Projectile.ai[1] > 120f)
                    {
                        Projectile.ai[1] = 120f;
                    }
                }
            }
            else
            {
                /*Projectile.ai[1] += 3f;
                Projectile.ai[1] -= num;
                num *= 2f;
                Projectile.ai[0] += num;*/
                if (Projectile.ai[1] > 50f)
                {
                    Projectile.ai[1] -= num;
                    if (Projectile.ai[1] < 50f)
                    {
                        Projectile.ai[1] = 50f;
                    }
                }
                else if (Projectile.ai[1] < 50f)
                {
                    Projectile.ai[1] += 3f;
                }

                num *= 2f;
                if (Projectile.ai[0] < 120f)
                {
                    Projectile.ai[0] += num;
                    if (Projectile.ai[0] > 120f)
                    {
                        Projectile.ai[0] = 120f;
                    }
                }
            }

            CalamityPlayer val = player.Calamity();
            /*if (shotDelay > 0)
            {
                Projectile.ai[0] = 0f;
                Projectile.ai[1] = 0f;
                shotDelay--;
                if (shotDelay < 0)
                {
                    Projectile.Kill();
                }
                else
                {
                    Projectile.friendly = false;
                }

                return;
            }*/

            Projectile.friendly = true;
            if (Main.myPlayer == Projectile.owner)
            {
                Item heldItem = player.HeldItem;
                if (heldItem.DamageType == ModContent.GetInstance<ClickerDamage>() && player.itemTime > 0)
                {
                    //player.GetModPlayer<RenderPlayer>().astralRockRenderData = null;
                    if (Main.myPlayer == player.whoAmI)
                    {
                        num = heldItem.shootSpeed;
                        if (num < 14f)
                        {
                            num = 14f;
                        }

                        if (num > 28f)
                        {
                            num = 28f;
                        }

                        if (player.GetModPlayer<RenderPlayer>().astralRockRenderData != null && player.GetModPlayer<RenderPlayer>().astralRockRenderData.Count > 0)
                        {
                            int ring = Main.rand.Next(2);
                            List<int> rocks = new List<int>();

                            for (int i = ring * 8; i < 8 + ring * 8; i++)
                            {
                                if (player.GetModPlayer<RenderPlayer>().isRender[i])
                                    rocks.Add(i);
                            }

                            if (ring == 0 && rocks.Count == 0)
                            {
                                ring = 1;
                                rocks.Clear();
                                for (int i = ring * 8; i < 8 + ring * 8; i++)
                                {
                                    if (player.GetModPlayer<RenderPlayer>().isRender[i])
                                        rocks.Add(i);
                                }
                            }
                            if (ring == 1 && rocks.Count == 0)
                            {
                                ring = 0;
                                rocks.Clear();
                                for (int i = ring * 8; i < 8 + ring * 8; i++)
                                {
                                    if (player.GetModPlayer<RenderPlayer>().isRender[i])
                                        rocks.Add(i);
                                }
                            }
                            if (rocks.Count > 0)
                            {
                                int randIndex = (int)Main.rand.Next(rocks.Count);
                                //Main.NewText($"ring {ring} rock {rocks[randIndex]}");
                                ShootRocksClicker(Vector2.Normalize(Main.MouseWorld - player.Center) * num, ring, rocks[randIndex]);
                                player.GetModPlayer<RenderPlayer>().isRender[rocks[randIndex]] = false;
                            }
                        }
                    }

                    //shotDelay = 120;
                    return;
                }
            }

            if (Main.netMode == NetmodeID.Server)
            {
                return;
            }

            FrameTimer++;
            if (val.auricSet && FrameTimer == 5)
            {
                if (AuricTeslaFrame != 12)
                {
                    AuricTeslaFrame++;
                }
                else
                {
                    AuricTeslaFrame = 9;
                }

                FrameTimer = 0;
            }

            UnifiedRandom frameRand = new UnifiedRandom(player.name.GetHashCode());
            //bool num2 = modPlayer.intergelacticClicker != null && modPlayer.intergelacticClicker.DamageType == DamageClass.Melee;
            RenderPlayer modPlayer2 = player.GetModPlayer<RenderPlayer>();
            modPlayer2.astralRockRenderData = new List<AstralRockRenderData>();
            //int num3 = 1;

            for (int i = 1; i <= 1; i++)
            {
                int num4 = 8 * i;
                float num5 = rotation / (float)Math.Pow(i, 1.5);
                float num6 = rotation2 / (float)Math.Pow(i, 1.5);
                if (!val.auricSet && (player.name == "Vortex" || player.name == "MarieArk"))
                {
                    for (int j = 0; j < num4; j++)
                    {
                        Vector3 pos = Vector3.Transform(new Vector3(Projectile.ai[0] * (float)i, 0f, 0f), Matrix.CreateFromYawPitchRoll(1f, -1f, num5 + MathF.PI * 2f / (float)num4 * (float)j));
                        pos.X = 0f - pos.X;
                        modPlayer2.astralRockRenderData.Add(new AstralRockRenderData(j, pos, 8));
                    }

                    for (int k = 0; k < num4; k++)
                    {
                        Vector3 pos2 = Vector3.Transform(new Vector3(Projectile.ai[1] * (float)i, 0f, 0f), Matrix.CreateFromYawPitchRoll(-1f, -1f, num6 + MathF.PI * 2f / (float)num4 * (float)k));
                        pos2.X = 0f - pos2.X;
                        modPlayer2.astralRockRenderData.Add(new AstralRockRenderData(k + num4, pos2, 8));
                    }

                    continue;
                }

                for (int l = 0; l < num4; l++)
                {
                    Vector3 pos3 = Vector3.Transform(new Vector3(Projectile.ai[0] * (float)i, 0f, 0f), Matrix.CreateFromYawPitchRoll(1f, -1f, num5 + MathF.PI * 2f / (float)num4 * (float)l));
                    pos3.X = 0f - pos3.X;
                    if (val.auricSet)
                    {
                        modPlayer2.astralRockRenderData.Add(new AstralRockRenderData(l, pos3, AuricTeslaFrame));
                    }
                    else
                    {
                        modPlayer2.astralRockRenderData.Add(new AstralRockRenderData(l, pos3, frameRand));
                    }
                }

                for (int m = 0; m < num4; m++)
                {
                    Vector3 pos4 = Vector3.Transform(new Vector3(Projectile.ai[1] * (float)i, 0f, 0f), Matrix.CreateFromYawPitchRoll(-1f, -1f, num6 + MathF.PI * 2f / (float)num4 * (float)m));
                    pos4.X = 0f - pos4.X;
                    if (val.auricSet)
                    {
                        modPlayer2.astralRockRenderData.Add(new AstralRockRenderData(m + num4, pos4, AuricTeslaFrame));
                    }
                    else
                    {
                        modPlayer2.astralRockRenderData.Add(new AstralRockRenderData(m + num4, pos4, frameRand));
                    }
                }
            }

            modPlayer2.astralRockRenderData.Sort((AstralRockRenderData v, AstralRockRenderData v2) => -v.offsetFromPlayer.Z.CompareTo(v2.offsetFromPlayer.Z));
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(rotation);
            writer.Write(rotation2);
            //writer.Write(shotDelay);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            rotation = reader.ReadInt32();
            rotation2 = reader.ReadInt32();
            //shotDelay = reader.ReadInt32();
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            Player player = Main.player[Projectile.owner];
            CalamityClickersPlayer modPlayer = player.GetModPlayer<CalamityClickersPlayer>();
            RenderPlayer rPlayer = player.GetModPlayer<RenderPlayer>();
            //int num = 1;

            for (int i = 1; i <= 1; i++)
            {
                int num2 = 8 * i;
                float num3 = rotation / (float)Math.Pow(i, 1.5);
                float num4 = rotation2 / (float)Math.Pow(i, 1.5);
                for (int j = 0; j < num2; j++)
                {
                    Vector3 vector = Vector3.Transform(new Vector3(Projectile.ai[0] * (float)i, 0f, 0f), Matrix.CreateFromYawPitchRoll(1f, -1f, num3 + MathF.PI * 2f / (float)num2 * (float)j));
                    if (Utils.CenteredRectangle(player.Center + new Vector2(0f - vector.X, vector.Y), new Vector2(Projectile.width, Projectile.width)).Intersects(targetHitbox))
                    {
                        return rPlayer.isRender[j];
                    }
                }

                for (int k = 0; k < num2; k++)
                {
                    Vector3 vector2 = Vector3.Transform(new Vector3(Projectile.ai[1] * (float)i, 0f, 0f), Matrix.CreateFromYawPitchRoll(-1f, -1f, num4 + MathF.PI * 2f / (float)num2 * (float)k));
                    if (Utils.CenteredRectangle(player.Center + new Vector2(0f - vector2.X, vector2.Y), new Vector2(Projectile.width, Projectile.width)).Intersects(targetHitbox))
                    {
                        return rPlayer.isRender[k + 8];
                    }
                }
            }

            return false;
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            modifiers.HitDirectionOverride = ((!(target.position.X < Main.player[Projectile.owner].position.X)) ? 1 : (-1));
        }
    }
}
