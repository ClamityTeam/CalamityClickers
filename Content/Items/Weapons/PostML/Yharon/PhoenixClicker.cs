using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Items;
using CalamityMod.Rarities;
using ClickerClass.Projectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityClickers.Content.Items.Weapons.PostML.Yharon
{
    public class PhoenixClicker : ModdedClickerWeapon
    {
        public static string PhoenixWrath { get; internal set; } = string.Empty;
        public override float Radius => 8f;
        public override Color RadiusColor => new Color(255, 213, 75);
        public override int DustType => DustID.GoldFlame;
        public override bool SetBorderTexture => true;
        public override void SetStaticDefaultsExtra()
        {
            PhoenixWrath = ClickerCompat.RegisterClickEffect(Mod, "PhoenixWrath", 12, RadiusColor, delegate (Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, int type, int damage, float knockBack)
            {
                int offSet = player.direction * 350;
                float direction = player.direction * 3f;

                Projectile.NewProjectile(source, new Vector2(position.X - offSet, position.Y - 225), new Vector2(direction, 0f), ModContent.ProjectileType<PhoenixClickerProjectile>(), damage * 2, knockBack, player.whoAmI);
                Projectile.NewProjectile(source, new Vector2(position.X + offSet, position.Y - 200), new Vector2(-direction, 0f), ModContent.ProjectileType<PhoenixClickerProjectile>(), damage * 2, knockBack, player.whoAmI);
            });
            CalamityClickersUtils.RegisterPostWildMagicClickEffect(PhoenixWrath);
            CalamityClickersUtils.RegisterPostNightmareMagicClickEffect(PhoenixWrath);
        }
        public override void SetDefaultsExtra()
        {
            AddEffect(Item, PhoenixWrath);
            SetDust(Item, DustType);

            Item.damage = 620;
            Item.knockBack = 1f;
            Item.rare = ModContent.RarityType<BurnishedAuric>();
            Item.value = CalamityGlobalItem.RarityVioletBuyPrice;
        }
    }
    public class PhoenixClickerProjectile : ModdedClickerProjectile
    {
        public override bool UseInvisibleProjectile => false;
        public float FlightStage
        {
            get => Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }

        public float FlightTimer
        {
            get => Projectile.ai[1];
            set => Projectile.ai[1] = value;
        }

        public int AttackTimer
        {
            get => (int)Projectile.localAI[0];
            set => Projectile.localAI[0] = value;
        }

        public bool Spawned
        {
            get => Projectile.localAI[1] == 1f;
            set => Projectile.localAI[1] = value ? 1f : 0f;
        }

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 20;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            Main.projFrames[Projectile.type] = 4;
        }

        public override void SetDefaultsExtra()
        {
            Projectile.width = 44;
            Projectile.height = 44;
            Projectile.aiStyle = -1;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 260;
            Projectile.alpha = 255;
            Projectile.extraUpdates = 2;
            Projectile.tileCollide = false;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture2D = TextureAssets.Projectile[Projectile.type].Value;
            Vector2 drawOrigin = new Vector2(texture2D.Width * 0.5f, Projectile.height * 0.5f);
            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                Color color = Projectile.GetAlpha(lightColor * 0.25f * Projectile.Opacity) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
                Main.EntitySpriteDraw(texture2D, drawPos, new Rectangle(0, texture2D.Height / 4 * Projectile.frame, texture2D.Width, texture2D.Height / 4), color * 0.1f, Projectile.rotation, drawOrigin, Projectile.scale, Projectile.spriteDirection == -1 ? SpriteEffects.FlipVertically : SpriteEffects.None, 0);
            }
            Main.EntitySpriteDraw(texture2D, Projectile.Center, new Rectangle(0, texture2D.Height / 4 * Projectile.frame, texture2D.Width, texture2D.Height / 4), lightColor, Projectile.rotation, drawOrigin, Projectile.scale, Projectile.spriteDirection == -1 ? SpriteEffects.FlipVertically : SpriteEffects.None, 0);
            return false;
        }

        public override void AI()
        {
            //Projectile.spriteDirection = Projectile.velocity.X > 0f ? 1 : -1;
            Projectile.rotation = Projectile.velocity.ToRotation();
            Projectile.spriteDirection = Projectile.rotation > MathHelper.PiOver2 && Projectile.rotation < MathHelper.Pi + MathHelper.PiOver2 ? -1 : 1;

            //Frames
            Projectile.frameCounter++;
            if (Projectile.frameCounter == 20)
            {
                Projectile.frame++;
                if (Projectile.frame > Main.projFrames[Projectile.type] - 1)
                    Projectile.frame = 0;
                Projectile.frameCounter = 0;
            }

            //Spawn
            if (!Spawned)
            {
                Spawned = true;
                SoundEngine.PlaySound(SoundID.Item74, Projectile.Center);
                for (int k = 0; k < 30; k++)
                {
                    Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Smoke, Main.rand.NextFloat(-9f, 9f), Main.rand.NextFloat(-9f, 9f), 125, default, 2f);
                    dust.noGravity = true;
                }
                for (int k = 0; k < 10; k++)
                {
                    Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.InfernoFork, Main.rand.NextFloat(-4f, 4f), Main.rand.NextFloat(-4f, 4f), 0, default, 1.25f);
                    dust.noGravity = true;
                }
            }

            //Main ai
            if (FlightStage == 0f)
            {
                Projectile.alpha -= 10;
                FlightTimer += 0.05f;
                if (FlightTimer > 3f)
                {
                    FlightStage = 1f;
                }
            }
            else if (FlightStage == 1f)
            {
                FlightTimer -= 0.05f;
                if (FlightTimer < -3f)
                {
                    FlightStage = 2f;
                }
            }
            else
            {
                Projectile.alpha += 10;
                FlightTimer += 0.05f;
            }

            Projectile.velocity.Y = FlightTimer;

            AttackTimer++;
            if (AttackTimer > 5 && Projectile.timeLeft > 20)
            {
                if (Main.myPlayer == Projectile.owner)
                {
                    float direction = Projectile.velocity.X > 0f ? 15f : -15f;
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X, Projectile.Center.Y), new Vector2(direction, 12f), ModContent.ProjectileType<PhoenixClickerProjectile2>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                }
                AttackTimer = 0;
            }
        }
    }
    public class PhoenixClickerProjectile2 : DraconicClickerPro2, ILocalizedModType, IModType
    {
        public new string LocalizationCategory => "Projectiles.Clicker";
        public override string Texture => ModContent.GetInstance<DraconicClickerPro2>().Texture;
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<Dragonfire>(), 300, false);
        }
    }
}
