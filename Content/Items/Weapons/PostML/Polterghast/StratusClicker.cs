using CalamityClickers.Content.Items.Weapons.HM;
using CalamityMod;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Items;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Placeables.Ores;
using CalamityMod.Particles;
using CalamityMod.Rarities;
using CalamityMod.Systems.Mechanic;
using ClickerClass.Core;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityClickers.Content.Items.Weapons.PostML.Polterghast
{
    public class StratusClicker : ModdedClickerWeapon
    {
        public static string Constellation { get; internal set; } = string.Empty;
        public override float Radius => 7.5f;
        public override Color RadiusColor => new Color(123, 228, 234);
        public static bool hasFired = false;
        public static StarburstEntity starburst1 = null;
        public override void SetStaticDefaultsExtra()
        {
            Constellation = ClickerCompat.RegisterClickEffect(Mod, "Constellation", 1, RadiusColor, delegate (Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, int type, int damage, float knockBack)
            {
                if (!hasFired)
                {
                    hasFired = true;
                    if (player.Calamity().AvaliableStarburst >= 20)
                    {
                        var star1 = player.Calamity().StarburstEntities.FirstOrDefault(x => x.AICooldown <= 0 && x.value == 10, null);
                        if (star1 != null)
                        {
                            star1.AICooldown = 1;
                            starburst1 = star1;
                        }
                    }
                }

                /*void SpawnStar(Vector2 offset, float intensity, int flashOffset = 0, int flashMod = 60)
                {
                    offset += new Vector2(5, 49.25f); //this centers the constellation
                    offset.X *= Projectile.spriteDirection;
                    var star = new BloomParticle(DrawCenter + offset.RotatedBy(Projectile.rotation) * Projectile.scale - (Owner.oldVelocity * Math.Clamp(offset.Length() * 0.001f, 0, 1)), Vector2.Zero, Color.SkyBlue * Projectile.Opacity * ((Owner.miscCounter + flashOffset) % flashMod < 5 ? 0.75f : 1f), StarScale * intensity, StarScale * intensity, 2, false);
                    var star2 = new CustomSpark(DrawCenter + offset.RotatedBy(Projectile.rotation) * Projectile.scale - (Owner.oldVelocity * Math.Clamp(offset.Length() * 0.001f, 0, 1)), Vector2.UnitX.RotatedBy(MathHelper.Pi * ((Owner.miscCounter + flashOffset) / 300f)) * 0.1f, "CalamityMod/Particles/Sparkle", false, 2, 4 * StarScale * intensity, Color.White * Projectile.Opacity, Vector2.One);
                    GeneralParticleHandler.SpawnParticle(star);
                    GeneralParticleHandler.SpawnParticle(star2);
                }*/

                if (player.Clicker().clickerTotal % 10 == 0 && player.Calamity().AvaliableStarburst >= 10)
                {
                    Projectile.NewProjectile(source, position, Vector2.Zero, ModContent.ProjectileType<StratusClickerProjectile>(), damage, knockBack, player.whoAmI, Main.rand.NextFloat(0.75f, 1.5f));
                    player.Calamity().StratusStarburst -= 10;
                }
                player.Calamity().StratusStarburst += Main.rand.Next(1, 3);





                /*if (player.Calamity().AvaliableStarburst > 20)
                {
                    SoundEngine.PlaySound(SoundID.DD2_WitherBeastDeath);

                    int proj = Projectile.NewProjectile(source, position, Vector2.Zero, ModContent.ProjectileType<ScorpiusConstellation>(), (int)(damage * 3), 0f, player.whoAmI, 0f, 0f);
                    Main.projectile[proj].DamageType = ModContent.GetInstance<ClickerDamage>();

                    GeneralParticleHandler.SpawnParticle(new CustomPulse(position, Vector2.Zero, Color.SkyBlue, "CalamityMod/Particles/ShatteredExplosion", Vector2.One, Main.rand.NextFloat(-15f, 15f), 0f, 0.25f, 12));
                    GeneralParticleHandler.SpawnParticle(new CustomPulse(position, Vector2.Zero, Color.DeepSkyBlue, "CalamityMod/Particles/ShatteredExplosion", Vector2.One, Main.rand.NextFloat(-15f, 15f), 0f, 0.2f, 12));
                    for (int i = 0; i < 30; i++)
                    {
                        int dustType = Utils.SelectRandom(Main.rand, new int[]
                        {
                            109,
                            111,
                            132
                        });

                        int dust = Dust.NewDust(position, 0, 0, dustType);
                        Main.dust[dust].noGravity = true;
                        Main.dust[dust].velocity *= 7;
                    }
                    player.Calamity().StratusStarburst -= 20;
                    if (starburst1 != null)
                        player.Calamity().StarburstEntities.Remove(starburst1);
                }
                else player.Calamity().StratusStarburst++;*/
            });
            CalamityClickersUtils.RegisterPostWildMagicClickEffect(Constellation);
        }
        public override void SetDefaultsExtra()
        {
            AddEffect(Item, Constellation);
            SetDust(Item, 176);

            Item.damage = 230;
            Item.knockBack = 1f;
            Item.rare = ModContent.RarityType<PureGreen>();
            Item.value = CalamityGlobalItem.RarityPureGreenBuyPrice;
        }
        public override void HoldItem(Player player)
        {
            player.Calamity().StratusStarburstResetTimer = (int)MathHelper.Max(player.Calamity().StratusStarburstResetTimer, 600);
        }
        public override bool AltFunctionUse(Player player) => player.Calamity().StratusStarburst > 50;
        public override bool? UseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                foreach (var p in Main.projectile)
                {
                    if (p.active && p.ModProjectile is StratusClickerProjectile mp)
                    {
                        mp.ExplosionTimer = Main.rand.Next(2, 30);
                    }
                }
                player.Calamity().StratusStarburst -= 50;
            }
            return base.UseItem(player);
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<StarblightClicker>()
                .AddIngredient<Lumenyl>(6)
                .AddIngredient<RuinousSoul>(4)
                .AddIngredient<ExodiumCluster>(16)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
    public class StratusClickerProjectile : ModdedClickerProjectile
    {
        public override string Texture => "CalamityMod/Projectiles/InvisibleProj";
        public int MaxTimeLeft = 3600;
        public const int AnimationTimeLeft = 30;
        public override void SetDefaultsExtra()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = MaxTimeLeft;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 60;
            //Projectile.scale = 0;
            MaxTimeLeft = 3600;
        }
        public int connectedStars1 = -1;
        public int connectedStars2 = -1;
        //public int whoAmI;
        public ref float Scale => ref Projectile.ai[0];
        public ref float ExplosionTimer => ref Projectile.ai[1];
        public override void AI()
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                //whoAmI = Projectile.whoAmI
                if (connectedStars1 == -1 || connectedStars2 == -1)
                {
                    List<int> list1 = new List<int>();
                    List<int> list2 = new List<int>();
                    for (int i = 0; i < Main.projectile.Length; i++)
                    {
                        Projectile p = Main.projectile[i];
                        if (p.active && p.owner == Projectile.owner && /*p.whoAmI != Projectile.whoAmI &&*/ p.ModProjectile is StratusClickerProjectile mp)
                        {
                            if (mp.connectedStars1 == -1)
                                list1.Add(i);
                            if (mp.connectedStars2 == -1)
                                list2.Add(i);
                        }
                    }
                    //Main.NewText(list1.Count);
                    if (list1.Count > 0 && connectedStars1 == -1)
                    {
                        //int secondClosest = -1;
                        int secondClosest = -1, closest = -1;
                        float minDistance = float.MaxValue;
                        foreach (int i in list1)
                        {
                            float d = (Main.projectile[i].Center - Projectile.Center).Length();
                            if (d < minDistance)
                            {
                                secondClosest = closest;
                                closest = i;
                                minDistance = d;
                            }
                        }
                        if (minDistance < 500 && closest > -1)
                            connectedStars1 = closest;
                        if (secondClosest > -1 && Main.rand.NextBool(3))
                            connectedStars1 = secondClosest;
                    }
                    if (list2.Count > 0 && connectedStars2 == -1)
                    {
                        if (connectedStars1 != -1) list2.Remove(connectedStars1);
                        int secondClosest = -1, closest = -1;
                        float minDistance = float.MaxValue;
                        foreach (int i in list2)
                        {
                            float d = (Main.projectile[i].Center - Projectile.Center).Length();
                            if (d < minDistance)
                            {
                                secondClosest = closest;
                                closest = i;
                                minDistance = d;
                            }
                        }
                        if (minDistance < 500 && closest > -1)
                            connectedStars2 = closest;
                        if (secondClosest > -1 && Main.rand.NextBool(3))
                            connectedStars1 = secondClosest;
                    }
                    //Main.NewText(connectedStars1.ToString() + " " + connectedStars2.ToString());
                }
                if (connectedStars1 != -1 && !Main.projectile[connectedStars1].active) connectedStars1 = -1;
                if (connectedStars2 != -1 && !Main.projectile[connectedStars2].active) connectedStars2 = -1;
                if (connectedStars2 == connectedStars1) connectedStars2 = -1;
            }
            Player Owner = Main.player[Projectile.owner];
            if (ExplosionTimer != 0)
            {
                ExplosionTimer--;
                if (ExplosionTimer == 1) Projectile.Kill();
            }
            else if (Owner.HeldItem.ModItem is not StratusClicker)
            {
                Projectile.Kill();
            }
            float percent = MathHelper.Clamp(MaxTimeLeft - Projectile.timeLeft, 0, AnimationTimeLeft) / AnimationTimeLeft;
            Projectile.scale = .75f + MathF.Cos(2 * percent * MathF.PI) / 2f;
            float percentHalf = MathHelper.Clamp(MaxTimeLeft / 2 - Projectile.timeLeft, 0, AnimationTimeLeft / 2) / AnimationTimeLeft;
            float a = (1 - MathF.Pow(MathF.Cos(percentHalf * MathF.PI), 2));
            var bloom = new BloomParticle(Projectile.Center, Vector2.Zero, Color.SkyBlue, 0.2f * Scale * Projectile.scale, 0.2f * Scale * Projectile.scale, 2, false);
            var star = new CustomSpark(Projectile.Center, Vector2.UnitX.RotatedBy(MathHelper.Pi * (Owner.miscCounter / 300f + a)) * 0.1f, "CalamityMod/Particles/Sparkle", false, 2, 4 * 0.2f * Scale * Projectile.scale, Color.White, Vector2.One);
            GeneralParticleHandler.SpawnParticle(bloom);
            GeneralParticleHandler.SpawnParticle(star);

            Projectile.velocity = Owner.velocity;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            void ConnectStars(Vector2 point1, Vector2 point2)
            {
                Player Owner = Main.player[Projectile.owner];
                point1 += new Vector2(5, 49.25f); //this centers the constellation
                point2 += new Vector2(5, 49.25f);
                point1.X *= Projectile.spriteDirection;
                point2.X *= Projectile.spriteDirection;
                var color = Color.SkyBlue * 0.75f * ((MathF.Sin(Main.GlobalTimeWrappedHourly) + 1) * 0.25f + 0.5f);
                CalamityUtils.DrawLineBetter(Main.spriteBatch, Projectile.Center + point1 - new Vector2(8, 48) - Owner.velocity, Projectile.Center + point2 - new Vector2(8, 48) - Owner.velocity, color * Projectile.Opacity, 3);
            }
            //if (Main.spriteBatch.HasBeginBeenCalled()) Main.spriteBatch.End();
            if (connectedStars1 > -1 || connectedStars2 > -1)
            {
                //Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, SamplerState.PointClamp, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
                if (connectedStars1 > -1) ConnectStars(Vector2.Zero, Main.projectile[connectedStars1].Center - Projectile.Center);
                if (connectedStars2 > -1) ConnectStars(Vector2.Zero, Main.projectile[connectedStars2].Center - Projectile.Center);
                //Main.spriteBatch.End();
                //Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
            }

            return false;
        }
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(connectedStars1);
            writer.Write(connectedStars2);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            connectedStars1 = reader.ReadInt32();
            connectedStars2 = reader.ReadInt32();
        }
        public override void OnKill(int timeLeft)
        {
            if (ExplosionTimer != 0)
            {
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<StratusClickerProjectileExplosion>(), Projectile.damage * 2, Projectile.knockBack, Projectile.owner);
            }
            for (int i = 0; i < 30; i++)
            {
                int dustType = Utils.SelectRandom(Main.rand, new int[]
                {
                        109,
                        111,
                        132
                });

                int dust = Dust.NewDust(Projectile.Center, 0, 0, dustType);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity *= 7;
            }
        }

    }
    public class StratusClickerProjectileExplosion : ModdedClickerProjectile
    {
        public bool Spawned
        {
            get => Projectile.ai[0] == 1f;
            set => Projectile.ai[0] = value ? 1f : 0f;
        }
        public override void SetDefaultsExtra()
        {
            Projectile.width = Projectile.height = 300;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 10;
            Projectile.alpha = 255;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.extraUpdates = 3;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;
        }
        public override void AI()
        {
            if (!Spawned)
            {
                Spawned = true;

                SoundEngine.PlaySound(SoundID.DD2_WitherBeastDeath, Projectile.Center);

                GeneralParticleHandler.SpawnParticle(new CustomPulse(Projectile.Center, Vector2.Zero, Color.SkyBlue, "CalamityMod/Particles/ShatteredExplosion", Vector2.One, Main.rand.NextFloat(-15f, 15f), 0f, 0.25f, 12));
                GeneralParticleHandler.SpawnParticle(new CustomPulse(Projectile.Center, Vector2.Zero, Color.DeepSkyBlue, "CalamityMod/Particles/ShatteredExplosion", Vector2.One, Main.rand.NextFloat(-15f, 15f), 0f, 0.2f, 12));
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<Voidfrost>(), 180);
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(ModContent.BuffType<Voidfrost>(), 180);
        }

    }
    public class StratusClickerProjectileOld : ModdedClickerProjectile
    {
        public override string Texture => "CalamityMod/Projectiles/Melee/CrescentMoonProj";

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaultsExtra()
        {
            Projectile.width = 50;
            Projectile.height = 50;
            Projectile.alpha = 100;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.extraUpdates = 2;
            Projectile.timeLeft = 220 * Projectile.MaxUpdates;
            Projectile.ignoreWater = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = Projectile.MaxUpdates * 13;
        }

        public override void AI()
        {
            Projectile.rotation += 0.2f;
            if (Projectile.timeLeft > 60)
                Projectile.velocity *= 0.95f;
            if (Projectile.timeLeft == 60)
            {
                MousePlayer mousePlayer = Main.player[Projectile.owner].GetModPlayer<MousePlayer>();
                if (mousePlayer.TryGetMousePosition(out Vector2 mouseWorld))
                {
                    if (Main.player[Projectile.owner].Clicker().HasAimbotModuleTarget)
                        mouseWorld = Main.npc[Main.player[Projectile.owner].Clicker().accAimbotModuleTarget].Center;
                    Vector2 vector = mouseWorld - Projectile.Center;
                    float speed = 13f;
                    float mag = vector.Length();
                    if (mag > speed)
                    {
                        mag = speed / mag;
                        vector *= mag;
                    }
                    Projectile.velocity = vector;
                    Projectile.netUpdate = true;
                }
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 1);
            return false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<Nightwither>(), 180);
        }
    }
}
