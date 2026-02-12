using CalamityMod;
using CalamityMod.Buffs.StatDebuffs;
using CalamityMod.Items;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Placeables;
using CalamityMod.Items.Placeables.FurnitureAcidwood;
using ClickerClass;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityClickers.Content.Items.Weapons.PreHM
{
    public class BubbleClicker : ModdedClickerWeapon
    {
        public static string SulphurousBubbles { get; internal set; } = string.Empty;
        public override float Radius => 2.1f;
        public override Color RadiusColor => new Color(140, 234, 87);
        public override void SetStaticDefaultsExtra()
        {
            SulphurousBubbles = ClickerSystem.RegisterClickEffect(Mod, "SulphurousBubbles", 10, RadiusColor, delegate (Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, int type, int damage, float knockBack)
            {
                int rand = Main.rand.Next(4, 5 + 1);
                for (int i = 0; i < 5; i++)
                {
                    Projectile.NewProjectile(source, position + Main.rand.NextVector2CircularEdge(40, 40) * Main.rand.NextFloat(1f, 1.5f), Main.rand.NextVector2Circular(1, 1), ModContent.ProjectileType<BubbleClickerProjectile>(), damage / 2, knockBack, player.whoAmI);
                }
            }, true);
        }
        public override void SetDefaultsExtra()
        {
            AddEffect(Item, SulphurousBubbles);
            SetDust(Item, 261);

            Item.damage = 6;
            Item.knockBack = 1f;
            Item.rare = ItemRarityID.Green;
            Item.value = CalamityGlobalItem.RarityGreenBuyPrice;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<Acidwood>(15)
                .AddIngredient<SulphuricScale>(10)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
    public class BubbleClickerProjectile : ClickableClickerProjectile
    {
        public override bool UseInvisibleProjectile => false;
        public override void SetDefaultsExtra()
        {
            Projectile.width = Projectile.height = 18;
            Projectile.penetrate = 3;
            Projectile.timeLeft = 180;
            Projectile.friendly = true;
            Projectile.aiStyle = -1;
        }
        public override void OnClick()
        {
            Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<BubbleClickerProjectile2>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
            Projectile.Kill();
        }
    }
    public class BubbleClickerProjectile2 : ModdedClickerProjectile
    {
        public override void SetDefaultsExtra()
        {
            Projectile.width = Projectile.height = 110;
            Projectile.friendly = true;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 120;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 8;
        }
        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, 0.5f, 0.95f, 0.4f);

            if (Projectile.localAI[0] == 0f)
            {
                SoundEngine.PlaySound(in SoundID.Item20, Projectile.Center);
                Projectile.localAI[0] += 1f;
            }

            float num = 25f;
            if (Projectile.ai[0] > 180f)
            {
                num -= (Projectile.ai[0] - 180f) / 2f;
            }

            if (num <= 0f)
            {
                num = 0f;
                Projectile.Kill();
            }

            num *= 0.7f;
            Projectile.ai[0] += 4f;
            for (int i = 0; (float)i < num; i++)
            {
                float num2 = Main.rand.Next(-5, 6);
                float num3 = Main.rand.Next(-5, 6);
                float num4 = Main.rand.Next(9, 24);
                float num5 = (float)Math.Sqrt(num2 * num2 + num3 * num3);
                num5 = num4 / num5;
                num2 *= num5;
                num3 *= num5;
                int num6 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.AncientLight, 0f, 0f, 100, default(Color), 2.5f);
                Dust obj = Main.dust[num6];
                obj.noGravity = true;
                obj.position.X = Projectile.Center.X;
                obj.position.Y = Projectile.Center.Y;
                obj.position.X += Main.rand.Next(-10, 11);
                obj.position.Y += Main.rand.Next(-10, 11);
                obj.velocity.X = num2;
                obj.velocity.Y = num3;
                obj.color = Utils.SelectRandom(Main.rand, Color.Yellow, Color.YellowGreen);
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<Irradiated>(), 60);
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            return CalamityUtils.CircularHitboxCollision(Projectile.Center, 95, targetHitbox);
        }
    }
}
