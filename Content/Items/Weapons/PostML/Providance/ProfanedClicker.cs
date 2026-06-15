using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Dusts;
using CalamityMod.Items;
using CalamityMod.Particles;
using CalamityMod.Rarities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace CalamityClickers.Content.Items.Weapons.PostML.Providance
{
    public class ProfanedClicker : ModdedClickerWeapon
    {
        public static string ProfanedInferno { get; internal set; } = string.Empty;
        public override float Radius => 7f;
        public override Color RadiusColor => new Color(255, 255, 150);

        public override void SetStaticDefaultsExtra()
        {
            ProfanedInferno = ClickerCompat.RegisterClickEffect(Mod, "ProfanedInferno", 7, RadiusColor, delegate (Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, int type, int damage, float knockBack)
            {
                Projectile.NewProjectile(source, position, Vector2.Zero, ModContent.ProjectileType<ProfanedClickerProjectile>(), damage * 2, knockBack, player.whoAmI);
            });
            CalamityClickersUtils.RegisterPostWildMagicClickEffect(ProfanedInferno);
        }
        public override void SetDefaultsExtra()
        {
            AddEffect(Item, ProfanedInferno);
            SetDust(Item, ModContent.DustType<HolyFireDust>());

            Item.damage = 200;
            Item.knockBack = 1f;
            Item.rare = ModContent.RarityType<Turquoise>();
            Item.value = CalamityGlobalItem.RarityTurquoiseBuyPrice;
        }
    }
    public class ProfanedClickerProjectile : ModdedClickerProjectile
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

                //SoundEngine.PlaySound(SoundID.Item74, Projectile.Center);

                float power = 1f;

                for (int i = 0; i < (int)(30 * power); i++)
                {
                    if (Main.rand.NextBool())
                    {
                        Particle spark = new CustomSpark(Projectile.Center, ((new Vector2(19, 19) * power).RotatedByRandom(100)) * Main.rand.NextFloat(0.2f, 1f), "CalamityMod/Particles/ProvidenceMarkParticle", false, 27, Main.rand.NextFloat(1.15f, 1.3f), Main.rand.NextBool(4) ? Color.Khaki : Color.Orange, new Vector2(1.3f, 0.5f), true, false, 0, false, false, Main.rand.NextFloat(0.1f, 0.2f));
                        GeneralParticleHandler.SpawnParticle(spark);
                    }
                    else
                    {
                        bool isSpark = Main.rand.NextBool(5);
                        Dust dust = Dust.NewDustPerfect(Projectile.Center, isSpark ? 278 : ModContent.DustType<LightDust>(), ((new Vector2(15, 15) * power).RotatedByRandom(100)) * Main.rand.NextFloat(0.2f, 1f));
                        dust.noGravity = true;
                        dust.scale = Main.rand.NextFloat(1.85f, 2.15f) * power * (isSpark ? 0.5f : 1);
                        dust.color = Main.rand.NextBool(5) ? Color.Khaki : Color.Goldenrod;
                        if (isSpark)
                            dust.noGravity = false;
                        else
                            dust.noLightEmittence = true;
                    }
                }

                Particle orb1 = new CustomPulse(Projectile.Center, Vector2.Zero, Color.Goldenrod, "CalamityMod/Particles/SoftRoundExplosion", new Vector2(1, 1), Main.rand.NextFloat(-10, 10), 0, 0.14f * power, 15);
                GeneralParticleHandler.SpawnParticle(orb1);

                Particle orb2 = new CustomPulse(Projectile.Center, Vector2.Zero, Color.Khaki, "CalamityMod/Particles/BloomRing", new Vector2(1, 1), Main.rand.NextFloat(-10, 10), 0, 2.1f * power, 15);
                GeneralParticleHandler.SpawnParticle(orb2);

                SoundStyle explode = new("CalamityMod/Sounds/Custom/Providence/ProvidenceHolyBlastImpact");
                SoundEngine.PlaySound(explode with { Volume = 0.5f, Pitch = 0.3f * power }, Projectile.Center);
                SoundStyle explode2 = new("CalamityMod/Sounds/Item/HeliumFlashReady");
                SoundEngine.PlaySound(explode2 with { Volume = 0.7f, Pitch = 0.6f * power }, Projectile.Center);
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<HolyFlames>(), 180);
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(ModContent.BuffType<HolyFlames>(), 180);
        }
    }
}
