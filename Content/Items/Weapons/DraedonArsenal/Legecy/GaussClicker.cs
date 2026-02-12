using CalamityMod;
using CalamityMod.Items;
using CalamityMod.Rarities;
using ClickerClass;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityClickers.Content.Items.Weapons.DraedonArsenal.Legecy
{
    public class GaussClicker : ModdedClickerWeapon
    {
        public static string GaussFlux { get; internal set; } = string.Empty;
        public override float Radius => 3.4f;
        public override Color RadiusColor => new Color(88, 81, 85);
        public override void SetStaticDefaultsExtra()
        {
            GaussFlux = ClickerSystem.RegisterClickEffect(Mod, "GaussFlux", 8, new Color(206, 255, 31), delegate (Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, int type, int damage, float knockBack)
            {
                Projectile.NewProjectile(source, position, Vector2.Zero, ModContent.ProjectileType<GaussClickerProjectile>(), damage / 2, 0f, player.whoAmI, 0);
            });
            CalamityClickersUtils.RegisterBlacklistedClickEffect(GaussFlux);
        }
        public override void SetDefaultsExtra()
        {
            AddEffect(Item, GaussFlux);

            Item.damage = 35;
            Item.knockBack = 1.5f;
            Item.rare = ModContent.RarityType<DarkOrange>();
            Item.value = CalamityGlobalItem.RarityPinkBuyPrice;

            CalamityGlobalItem modItem = Item.Calamity();
            modItem.UsesCharge = true;
            modItem.MaxCharge = 85;
            modItem.ChargePerUse = 0.01f;
        }
        /*public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<MysteriousCircuitry>(12).
                AddIngredient<DubiousPlating>(8).
                AddRecipeGroup("AnyMythrilBar", 10).
                AddIngredient(ItemID.SoulofMight, 20).
                AddCondition(ArsenalTierGatedRecipe.ConstructRecipeCondition(2, out Func<bool> condition), condition).
                AddTile(TileID.MythrilAnvil).
                Register();
        }*/
    }
    public class GaussClickerProjectile : ModdedClickerProjectile, ILocalizedModType
    {
        //public new string LocalizationCategory => "Projectiles.Clicker";
        //public override string Texture => "CalamityMod/Projectiles/InvisibleProj";
        public override void SetDefaultsExtra()
        {
            Projectile.width = 100;
            Projectile.height = 100;
            Projectile.friendly = true;
            Projectile.penetrate = 5;
            Projectile.timeLeft = 180;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 30;
            //Projectile.DamageType = ModContent.GetInstance<ClickerDamage>();
        }
        public float Time
        {
            get => Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }
        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, Color.Lime.ToVector3());
            if (!Main.dedServ)
            {
                if (Time == 0)
                {
                    for (int i = 0; i < 60; i++)
                    {
                        Dust dust = Dust.NewDustPerfect(Projectile.Center, DustID.AncientLight);
                        dust.color = Utils.SelectRandom(Main.rand, Color.Yellow, Color.YellowGreen);
                        dust.velocity = Main.rand.NextVector2Circular(20f, 20f);
                        dust.scale = 2f;
                        dust.noGravity = true;
                    }
                }
                for (int i = 0; i < 7; i++)
                {
                    for (int arcIndex = 0; arcIndex < 6; arcIndex++)
                    {
                        float offsetAngle = MathHelper.ToRadians(1080f) * i / 18f;
                        offsetAngle += Time / 10f;
                        float scale = 1.4f + (float)Math.Cos(i / 7f * MathHelper.TwoPi + Time / 30f) * 0.3f;
                        scale *= MathHelper.Lerp(1f, 0.4f, arcIndex / 6f);
                        Vector2 offset = new Vector2(10, 10).RotatedBy(offsetAngle);
                        offset += (arcIndex * MathHelper.TwoPi / 6f + Time / 20f).ToRotationVector2() * 6f * arcIndex;
                        //Vector2 offset = (arcIndex * MathHelper.TwoPi / 6f + Time / 20f).ToRotationVector2() * 6f * arcIndex;

                        Dust dust = Dust.NewDustPerfect(Projectile.Center + offset, DustID.AncientLight);
                        dust.color = Utils.SelectRandom(Main.rand, Color.Yellow, Color.YellowGreen);
                        dust.velocity = Vector2.Zero;
                        dust.scale = scale;
                        dust.noGravity = true;
                    }
                }
            }
            Time++;
        }
    }
}
