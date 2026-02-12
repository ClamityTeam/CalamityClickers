using CalamityMod;
using CalamityMod.CustomRecipes;
using CalamityMod.Items;
using CalamityMod.Items.Materials;
using CalamityMod.Particles;
using CalamityMod.Rarities;
using CalamityMod.Tiles.Furniture.CraftingStations;
using ClickerClass;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace CalamityClickers.Content.Items.Weapons.DraedonArsenal
{
    public class FusionPointer : ModdedClickerWeapon
    {
        public static string CurrentStrike { get; internal set; } = string.Empty;
        public override float Radius => 8f;
        public override Color RadiusColor => Color.Lerp(CalamityClickersUtils.GetColorFromHex("1FFBFF"), CalamityClickersUtils.GetColorFromHex("FFFFF0"), MathF.Sin(Main.GlobalTimeWrappedHourly));
        public override void SetStaticDefaultsExtra()
        {
            CurrentStrike = ClickerSystem.RegisterClickEffect(Mod, "CurrentStrike", 8, () => RadiusColor, delegate (Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, int type, int damage, float knockBack)
            {
                bool spawnEffects = true;
                for (int k = 0; k < 4; k++)
                {
                    float hasSpawnEffects = spawnEffects ? 1f : 0f;
                    Projectile.NewProjectile(source, position.X, position.Y, Main.rand.NextFloat(-5f, 5f), Main.rand.NextFloat(-5f, 5f), ModContent.ProjectileType<FusionPointerPro>(), damage, 0f, player.whoAmI, (int)DateTime.Now.Ticks, hasSpawnEffects);
                    spawnEffects = false;
                }
            });
        }
        public override void SetDefaultsExtra()
        {
            AddEffect(Item, CurrentStrike);
            SetDust(Item, DustID.Electric);

            Item.damage = 350;
            Item.knockBack = 1.5f;
            Item.rare = ModContent.RarityType<CosmicPurple>();
            Item.value = CalamityGlobalItem.RarityDarkBlueBuyPrice;

            CalamityGlobalItem modItem = Item.Calamity();
            modItem.UsesCharge = true;
            modItem.MaxCharge = 250f;
            modItem.ChargePerUse = 0.03f;

        }
        public override void ModifyTooltips(List<TooltipLine> tooltips) => CalamityGlobalItem.InsertKnowledgeTooltip(tooltips, 5);
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<MysteriousCircuitry>(25).
                AddIngredient<DubiousPlating>(15).
                AddIngredient<CosmiliteBar>(8).
                AddIngredient<AscendantSpiritEssence>(2).
                AddCondition(ArsenalTierGatedRecipe.ConstructRecipeCondition(5, out Func<bool> condition), condition).
                AddTile<CosmicAnvil>().
                Register();
        }
    }
    public class FusionPointerPro : ModdedClickerProjectile
    {
        private readonly HashSet<int> hitTargets = new HashSet<int>();
        private readonly HashSet<int> foundTargets = new HashSet<int>();

        private UnifiedRandom rng;

        public UnifiedRandom Rng
        {
            get
            {
                if (rng == null)
                {
                    rng = new UnifiedRandom(RandomSeed / (1 + Projectile.identity));
                }
                return rng;
            }
        }

        public int RandomSeed
        {
            get => (int)Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }

        public bool HasSpawnEffects
        {
            get => Projectile.ai[1] == 1f;
            set => Projectile.ai[1] = value ? 1f : 0f;
        }

        public int WobbleTimer
        {
            get => (int)Projectile.localAI[0];
            set => Projectile.localAI[0] = value;
        }

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();

            ProjectileID.Sets.CanDistortWater[Projectile.type] = false;
        }

        public override void SetDefaultsExtra()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.aiStyle = -1;
            Projectile.penetrate = -1;
            Projectile.alpha = 255;
            Projectile.friendly = true;
            Projectile.timeLeft = 600;
            Projectile.extraUpdates = 100;
            Projectile.ignoreWater = true;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;
        }

        public override bool? CanHitNPC(NPC target)
        {
            if (hitTargets.Contains(target.whoAmI))
            {
                return false;
            }
            return base.CanHitNPC(target);
        }

        public override void AI()
        {
            bool killed = Projectile.HandleChaining(hitTargets, foundTargets, 8);
            if (killed)
            {
                return;
            }

            /*int index = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 229, 0f, 0f, 0, default(Color), 1.15f);
            Dust dust = Main.dust[index];
            dust.position.X = Projectile.Center.X;
            dust.position.Y = Projectile.Center.Y;
            dust.velocity *= 0f;
            dust.noGravity = true;*/
            if (Main.rand.NextBool(3))
            {
                MediumMistParticle mist = new MediumMistParticle(Projectile.Center, Projectile.velocity * 0.25f, Color.Aqua, Color.White, 1, 50, 0.01f);
                GeneralParticleHandler.SpawnParticle(mist);
            }

            if (HasSpawnEffects)
            {
                HasSpawnEffects = false;

                SoundEngine.PlaySound(SoundID.Item94, Projectile.Center);

                for (int k = 0; k < 20; k++)
                {
                    Dust dust = Dust.NewDustDirect(Projectile.Center - new Vector2(4), 8, 8, DustID.Vortex, Main.rand.NextFloat(-6f, 6f), Main.rand.NextFloat(-6f, 6f), 0, default, 1.25f);
                    dust.noGravity = true;
                }
            }

            if (Projectile.timeLeft < 580)
            {
                if (Projectile.timeLeft >= 600)
                {
                    /*for (int k = 0; k < 6; k++)
                    {
                        index = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 229, Projectile.velocity.X * 0.25f, Projectile.velocity.Y * 0.25f, 125, default(Color), 1.15f);
                        Main.dust[index].noGravity = true;
                    }*/
                    //MediumMistParticle mist = new MediumMistParticle(Projectile.Center, Projectile.velocity * 0.25f, Color.Aqua, Color.White, 1, 50, 0.01f);
                    //GeneralParticleHandler.SpawnParticle(mist);
                }

                WobbleTimer++;
                if (WobbleTimer > 2)
                {
                    Projectile.velocity.Y += Rng.NextFloat(-0.75f, 0.75f);
                    Projectile.velocity.X += Rng.NextFloat(-0.75f, 0.75f);
                    WobbleTimer = 0;
                }

                float x = Projectile.Center.X;
                float y = Projectile.Center.Y;
                float dist = 750;
                bool found = false;

                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC npc = Main.npc[i];
                    if (npc.CanBeChasedBy() && !hitTargets.Contains(npc.whoAmI) && Projectile.DistanceSQ(npc.Center) < dist * dist && Collision.CanHit(Projectile.Center, 1, 1, npc.Center, 1, 1))
                    {
                        float foundX = npc.Center.X;
                        float foundY = npc.Center.Y;
                        float abs = Math.Abs(Projectile.Center.X - foundX) + Math.Abs(Projectile.Center.Y - foundY);
                        if (abs < dist)
                        {
                            dist = abs;
                            x = foundX;
                            y = foundY;
                            found = true;
                        }
                    }
                }

                if (found)
                {
                    float mag = 2.5f;
                    Vector2 center = Projectile.Center;
                    float toX = x - center.X;
                    float toY = y - center.Y;
                    float len = (float)Math.Sqrt((double)(toX * toX + toY * toY));
                    len = mag / len;
                    toX *= len;
                    toY *= len;

                    Projectile.velocity.X = (Projectile.velocity.X * 20f + toX) / 21f;
                    Projectile.velocity.Y = (Projectile.velocity.Y * 20f + toY) / 21f;
                }
                else
                {
                    Projectile.velocity = Vector2.Zero;
                }
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }

        public override void OnKill(int timeLeft)
        {
            for (int k = 0; k < 10; k++)
            {
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Vortex, Main.rand.Next((int)-5f, (int)5f), Main.rand.Next((int)-5f, (int)5f), 75, default(Color), 0.75f);
                Main.dust[dust].noGravity = true;
            }
        }
    }
}
