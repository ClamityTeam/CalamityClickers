using CalamityMod;
using CalamityMod.Items;
using CalamityMod.Items.Materials;
using CalamityMod.Rarities;
using CalamityMod.Tiles.Furniture.CraftingStations;
using ClickerClass.Items.Weapons.Clickers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityClickers.Content.Items.Weapons.PostML.Yharon
{
    public class ClickerNova : ModdedClickerWeapon
    {
        public static string Popstar { get; internal set; } = string.Empty;
        public override float Radius => 8f;
        public override Color RadiusColor => Color.Yellow;
        public override int DustType => DustID.GoldCoin;
        public const int StarOffset = 1000;
        //public const float StarVelocity = 50;
        public override void SetStaticDefaultsExtra()
        {
            Popstar = ClickerCompat.RegisterClickEffect(Mod, "Popstar", 25, RadiusColor, delegate (Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, int type, int damage, float knockBack)
            {
                SoundEngine.PlaySound(new SoundStyle("CalamityClickers/Assets/Sounds/Custom/asriel_star"), position);
                Projectile.NewProjectile(source, position - new Vector2(0, StarOffset), new Vector2(0, StarOffset / ClickerNovaProjectile.TimeLeft), ModContent.ProjectileType<ClickerNovaProjectile>(), damage * 3, knockBack, player.whoAmI);
            });
            CalamityClickersUtils.RegisterPostWildMagicClickEffect(Popstar);
            CalamityClickersUtils.RegisterPostNightmareMagicClickEffect(Popstar);
        }
        public override void SetDefaultsExtra()
        {
            AddEffect(Item, Popstar);
            SetDust(Item, DustType);

            Item.damage = 400;
            Item.knockBack = 1f;
            Item.rare = ModContent.RarityType<BurnishedAuric>();
            Item.value = CalamityGlobalItem.RarityVioletBuyPrice;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<DreamClicker>()
                .AddIngredient(ItemID.Cog, 300)
                .AddIngredient<CoreofCalamity>()
                .AddIngredient<AuricBar>(5)
                .AddTile<CosmicAnvil>()
                .Register();
        }
    }
    public class ClickerNovaProjectile : ModdedClickerProjectile
    {
        public override bool UseInvisibleProjectile => false;
        public const int TimeLeft = 20;
        public const int MaxSmallStarCount = 10;
        public override void SetDefaultsExtra()
        {
            Projectile.width = Projectile.height = 128;
            Projectile.timeLeft = TimeLeft;
            Projectile.penetrate = -1;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.aiStyle = -1;
            AIType = -1;
        }
        public override void AI()
        {
            Projectile.rotation += 0.2f;
        }
        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(new SoundStyle("CalamityClickers/Assets/Sounds/Custom/asriel_star_explosion"), Projectile.Center);
            int rand = Main.rand.NextBool() ? 1 : -1;
            for (int i = 0; i < MaxSmallStarCount; i++)
            {
                int damage = Projectile.damage / 3;
                Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, new Vector2(0, 30).RotatedBy(MathHelper.TwoPi / MaxSmallStarCount * i), ModContent.ProjectileType<ClickerNovaProjectileSmall>(), damage, Projectile.knockBack, Projectile.owner, rand);
                Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, new Vector2(0, 20).RotatedBy(MathHelper.TwoPi / MaxSmallStarCount * i), ModContent.ProjectileType<ClickerNovaProjectileSmall>(), damage, Projectile.knockBack, Projectile.owner, rand);
                Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, new Vector2(0, 10).RotatedBy(MathHelper.TwoPi / MaxSmallStarCount * i), ModContent.ProjectileType<ClickerNovaProjectileSmall>(), damage, Projectile.knockBack, Projectile.owner, rand);
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            //Projectile.DrawStarTrail(Color.Magenta, Color.Purple);

            Texture2D value = ModContent.Request<Texture2D>("CalamityMod/Projectiles/StarTrail").Value;
            Vector2 vector = new Vector2(0f, Projectile.gfxOffY) - Main.screenPosition;
            Rectangle value2 = value.Frame();
            float rotation = Projectile.velocity.ToRotation() + MathF.PI / 2f;
            Vector2 origin = new Vector2((float)value2.Width / 2f, 10);
            Vector2 vector2 = vector + Projectile.Center + Projectile.velocity;
            Vector2 spinningpoint = -Vector2.UnitY * 10;
            float num = (float)Main.player[Projectile.owner].miscCounter % 216000f / 60f;
            Color color = Color.Magenta * 0.2f;
            color.A = 0;
            float num2 = MathF.PI * 2f * num;
            for (int i = 0; i < 6; i += 2)
            {
                Vector2 position = vector2 + spinningpoint.RotatedBy(num2 - MathF.PI * (float)i / 3f);
                float scale = 1.5f - (float)i * 0.1f;
                Main.EntitySpriteDraw(value, position, value2, color, rotation, origin, scale, SpriteEffects.None);
            }

            Vector2 position2 = vector + Projectile.Center - Projectile.velocity * 0.5f;
            Color color2 = Color.Purple * 0.5f;
            color2.A = 0;
            for (float num3 = 0f; num3 < 1f; num3 += 0.5f)
            {
                float num4 = num % 0.5f / 0.5f;
                num4 = (num4 + num3) % 1f;
                float num5 = num4 * 2f;
                if (num5 > 1f)
                {
                    num5 = 2f - num5;
                }

                Main.EntitySpriteDraw(value, position2, value2, color2 * num5, rotation, origin, 10 + num4 * 0.5f, SpriteEffects.None);
            }

            return base.PreDraw(ref lightColor);
        }
    }
    public class ClickerNovaProjectileSmall : ModdedClickerProjectile
    {
        public override bool UseInvisibleProjectile => false;
        public const int TimeLeft = 600;
        public override void SetDefaultsExtra()
        {
            Projectile.width = Projectile.height = 32;
            Projectile.timeLeft = TimeLeft;
            Projectile.penetrate = -1;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.aiStyle = -1;
            AIType = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }
        public override void AI()
        {
            Projectile.rotation += 0.4f;
            if (Projectile.timeLeft == TimeLeft - 200)
            {
                Projectile.penetrate = 1;
            }

            if (Projectile.timeLeft > TimeLeft - 200)
            {
                Projectile.velocity = Projectile.velocity.RotatedBy(0.02f * Projectile.ai[0]);
            }
            else
            {
                CalamityUtils.HomeInOnNPC(Projectile, true, 2000, 100, 50);
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Projectile.DrawStarTrail(Color.Magenta, Color.Purple);
            return base.PreDraw(ref lightColor);
        }
    }
}
