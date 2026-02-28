using CalamityMod;
using CalamityMod.CustomRecipes;
using CalamityMod.Items;
using CalamityMod.Items.Materials;
using CalamityMod.Particles;
using ClickerClass;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityClickers.Content.Items.Weapons.DraedonArsenal
{
    public class FieldPolorizer : ModdedClickerWeapon
    {
        public static string Electrofield { get; internal set; } = string.Empty;
        public override float Radius => 6f;
        public override Color RadiusColor => CalamityClickersUtils.GetColorFromHex("C2FEF4");
        public override void SetStaticDefaultsExtra()
        {
            Electrofield = ClickerSystem.RegisterClickEffect(Mod, "Electrofield", 8, RadiusColor, delegate (Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, int type, int damage, float knockBack)
            {
                Projectile.NewProjectile(source, position, Vector2.Zero, ModContent.ProjectileType<FieldPolorizerPro>(), damage, knockBack, player.whoAmI);
            });
        }
        public override void SetDefaultsExtra()
        {
            AddEffect(Item, Electrofield);
            SetDust(Item, DustID.Electric);

            Item.damage = 86;
            Item.knockBack = 1.5f;
            Item.rare = ItemRarityID.Yellow;
            Item.value = CalamityGlobalItem.RarityYellowBuyPrice;
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips) => CalamityGlobalItem.InsertKnowledgeTooltip(tooltips, 3);
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<MysteriousCircuitry>(14).
                AddIngredient<DubiousPlating>(11).
                AddIngredient<InfectedArmorPlating>(10).
                AddIngredient<LifeAlloy>(5).
                AddCondition(ArsenalTierGatedRecipe.ConstructRecipeCondition(3, out Func<bool> condition), condition).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
    public class FieldPolorizerPro : ModdedClickerProjectile
    {
        public override void SetDefaultsExtra()
        {
            Projectile.width = Projectile.height = 100;
            Projectile.aiStyle = -1;
            AIType = -1;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 180;
            Projectile.friendly = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;
        }
        public override void AI()
        {
            Color c = CalamityClickersUtils.GetColorFromHex("1FFBFF");
            c.A = 128;

            DirectionalPulseRing ring = new DirectionalPulseRing(Projectile.Center, Vector2.Zero, c, Vector2.One / 2, 0, Projectile.width / 90f, Projectile.width / 90f, 2);
            GeneralParticleHandler.SpawnParticle(ring);

            for (int i = 0; i < 3; i++)
            {
                float rand = Main.rand.NextFloat(MathHelper.TwoPi);
                SparkParticle spark = new SparkParticle(Projectile.Center + new Vector2(0, Projectile.width * Main.rand.NextFloat(0.2f, 0.4f)).RotatedBy(rand), Vector2.One.RotatedBy(rand), false, 5, Projectile.width / 100f / 2, c);
                GeneralParticleHandler.SpawnParticle(spark);
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.ExpandHitboxBy(Projectile.width + 10);
        }
    }
}
