using CalamityClickers.Content.Items.Weapons.HM;
using CalamityMod;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Items;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Placeables.Ores;
using CalamityMod.Particles;
using CalamityMod.Projectiles.Melee;
using CalamityMod.Rarities;
using CalamityMod.Systems.Mechanic;
using ClickerClass;
using ClickerClass.Core;
using Microsoft.Xna.Framework;
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
        public static string StratusMoon { get; internal set; } = string.Empty;
        public override float Radius => 7.5f;
        public override Color RadiusColor => new Color(123, 228, 234);
        public static bool hasFired = false;
        public static StarburstEntity starburst1 = null;
        public override void SetStaticDefaultsExtra()
        {
            StratusMoon = ClickerCompat.RegisterClickEffect(Mod, "StratusMoon", 1, RadiusColor, delegate (Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, int type, int damage, float knockBack)
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

                if (player.DistanceSQ(position) < 128)
                {
                    player.Calamity().StratusStarburst++;
                }
                else if (player.Calamity().AvaliableStarburst > 20)
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
            });
            CalamityClickersUtils.RegisterPostWildMagicClickEffect(StratusMoon);
        }
        public override void SetDefaultsExtra()
        {
            AddEffect(Item, StratusMoon);
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
