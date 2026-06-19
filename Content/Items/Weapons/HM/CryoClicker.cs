using CalamityMod;
using CalamityMod.Items;
using CalamityMod.Projectiles.Rogue;
using ClickerClass;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityClickers.Content.Items.Weapons.HM
{
    public class CryoClicker : ModdedClickerWeapon
    {
        public static string CryoBomb { get; internal set; } = string.Empty;
        public override float Radius => 3f;
        public override Color RadiusColor => new Color(90, 90, 203);
        public override void SetStaticDefaultsExtra()
        {
            CryoBomb = ClickerSystem.RegisterClickEffect(Mod, "CryoBomb", 5, RadiusColor, delegate (Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, int type, int damage, float knockBack)
            {
                Projectile.NewProjectile(source, position + Main.rand.NextVector2Circular(200, 200), Main.rand.NextVector2CircularEdge(1, 1), ModContent.ProjectileType<CryoClickerProjectile>(), damage, knockBack, player.whoAmI);
            });
        }
        public override void SetDefaultsExtra()
        {
            AddEffect(Item, CryoBomb);
            SetDust(Item, DustID.IceRod);

            Item.damage = 23;
            Item.knockBack = 1f;
            Item.rare = ItemRarityID.Pink;
            Item.value = CalamityGlobalItem.RarityPinkBuyPrice;
        }
    }
    public class CryoClickerProjectile : ClickableClickerProjectile
    {
        public override bool UseInvisibleProjectile => false;
        public override string Texture => "CalamityMod/Projectiles/Boss/IceBomb";
        public override void SetDefaultsExtra()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            //Projectile.scale = 0.5f;
            Projectile.friendly = true;
            Projectile.DamageType = ModContent.GetInstance<ClickerDamage>();
            Projectile.penetrate = 1;
            Projectile.timeLeft = 180;
        }
        public override void SafePreAI()
        {
            Projectile.velocity *= 0.98f;

            if (Projectile.localAI[0] == 0f)
            {
                Projectile.scale += 0.01f;
                Projectile.alpha -= 50;
                if (Projectile.alpha <= 0)
                {
                    Projectile.localAI[0] = 1f;
                    Projectile.alpha = 0;
                }
            }
            else
            {
                Projectile.scale -= 0.01f;
                Projectile.alpha += 50;
                if (Projectile.alpha >= 255)
                {
                    Projectile.localAI[0] = 0f;
                    Projectile.alpha = 255;
                }
            }
        }

        public override void OnClick()
        {
            SoundEngine.PlaySound(SoundID.Item27, Projectile.position);
            float spread = 90f * 0.0174f;
            double startAngle = Math.Atan2(Projectile.velocity.X, Projectile.velocity.Y) - spread / 2;
            double deltaAngle = spread / 8f;
            double offsetAngle;
            int i;
            if (Projectile.owner == Main.myPlayer)
            {
                for (i = 0; i < 2; i++)
                {
                    offsetAngle = startAngle + deltaAngle * (i + i * i) / 2f + 32f * i;
                    int projectile1 = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, (float)(Math.Sin(offsetAngle) * 5f), (float)(Math.Cos(offsetAngle) * 5f), ModContent.ProjectileType<FrostShardFriendly>(), (int)(Projectile.damage * 0.5), 0f, Projectile.owner, 0f, 0f);
                    if (projectile1.WithinBounds(Main.maxProjectiles))
                    {
                        Main.projectile[projectile1].DamageType = ModContent.GetInstance<ClickerDamage>();
                        Main.projectile[projectile1].penetrate = 2;
                    }
                    int projectile2 = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, (float)(-Math.Sin(offsetAngle) * 5f), (float)(-Math.Cos(offsetAngle) * 5f), ModContent.ProjectileType<FrostShardFriendly>(), (int)(Projectile.damage * 0.5), 0f, Projectile.owner, 0f, 0f);
                    if (projectile2.WithinBounds(Main.maxProjectiles))
                    {
                        Main.projectile[projectile2].DamageType = ModContent.GetInstance<ClickerDamage>();
                        Main.projectile[projectile2].penetrate = 2;
                    }
                }
            }

            for (int k = 0; k < 3; k++)
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.IceRod, Projectile.oldVelocity.X * 0.5f, Projectile.oldVelocity.Y * 0.5f);

            Projectile.Kill();
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Main.dayTime ? new Color(50, 50, 255, Projectile.alpha) : new Color(255, 255, 255, Projectile.alpha);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Frozen, 30);
        }
    }
}
