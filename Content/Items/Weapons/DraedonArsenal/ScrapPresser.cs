using CalamityMod;
using CalamityMod.CustomRecipes;
using CalamityMod.Items;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Placeables;
using CalamityMod.Items.Placeables.SunkenSea;
using CalamityMod.Particles;
using ClickerClass;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityClickers.Content.Items.Weapons.DraedonArsenal
{
    public class ScrapPresser : ModdedClickerWeapon
    {
        public static string ScrapMissile { get; internal set; } = string.Empty;
        public override float Radius => 2.7f;
        public override Color RadiusColor => new Color(255, 64, 31);
        public override void SetStaticDefaultsExtra()
        {
            ScrapMissile = ClickerSystem.RegisterClickEffect(Mod, "ScrapMissile", 10, RadiusColor, delegate (Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, int type, int damage, float knockBack)
            {
                Vector2 pos = position;

                NPC npc = CalamityUtils.ClosestNPCAt(pos, 1000, false, true);
                if (npc != null)
                {
                    SoundEngine.PlaySound(new SoundStyle("CalamityMod/Sounds/Custom/Codebreaker/LongRangeSensorArrayInstall"), pos);
                    Vector2 vec1 = Vector2.UnitY.RotateRandom(0.1f);
                    Projectile.NewProjectile(source, npc.Center - vec1 * 500, vec1, ModContent.ProjectileType<ScrapPresserPro>(), damage * 2, knockBack, player.whoAmI);
                }
            }, true);
        }
        public override void SetDefaultsExtra()
        {
            AddEffect(Item, ScrapMissile);
            SetDust(Item, DustID.Electric);

            Item.damage = 13;
            Item.knockBack = 1.5f;
            Item.rare = ItemRarityID.Orange;
            Item.value = CalamityGlobalItem.RarityOrangeBuyPrice;

            CalamityGlobalItem modItem = Item.Calamity();
            modItem.UsesCharge = true;
            modItem.MaxCharge = 50f;
            modItem.ChargePerUse = 0.02f;

        }
        public override void ModifyTooltips(List<TooltipLine> tooltips) => CalamityGlobalItem.InsertKnowledgeTooltip(tooltips, 1);
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<MysteriousCircuitry>(6).
                AddIngredient<DubiousPlating>(5).
                AddIngredient<AerialiteBar>(4).
                AddIngredient<SeaPrism>(7).
                AddCondition(ArsenalTierGatedRecipe.ConstructRecipeCondition(1, out Func<bool> condition), condition).
                AddTile(TileID.Anvils).
                Register();
        }
    }
    public class ScrapPresserPro : ModdedClickerProjectile
    {
        public override bool UseInvisibleProjectile => false;
        public override void SetDefaultsExtra()
        {
            Projectile.width = Projectile.height = 16;
            Projectile.aiStyle = -1;
            AIType = -1;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 300;
            Projectile.friendly = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;
        }
        public override void AI()
        {
            if (Projectile.ai[1] == 0)
            {
                CalamityUtils.HomeInOnNPC(Projectile, false, 500, 10, 20);
                if (Main.rand.NextBool(2))
                {
                    Dust.NewDustPerfect(Projectile.Center, DustID.Flare, -Projectile.velocity.SafeNormalize(Vector2.Zero), Scale: 0.5f);
                }
            }
            Projectile.rotation = Projectile.velocity.ToRotation();
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Projectile.ai[1] == 0)
            {
                DirectionalPulseRing ring = new DirectionalPulseRing(Projectile.Center, Vector2.Zero, Color.Orange, Vector2.One / 2, 0, 0, 0.75f, 10);
                GeneralParticleHandler.SpawnParticle(ring);
                ring = new DirectionalPulseRing(Projectile.Center, Vector2.Zero, Color.Red, Vector2.One / 2, 0, 0, 1f, 10);
                GeneralParticleHandler.SpawnParticle(ring);
                for (int i = 0; i < 6; i++)
                {
                    SparkParticle spark = new SparkParticle(Projectile.Center, Vector2.UnitX.RotatedBy(MathHelper.TwoPi / 6 * i), false, 5, 0.5f, Color.Orange);
                    GeneralParticleHandler.SpawnParticle(spark);
                }

                SoundEngine.PlaySound(SoundID.Item14, Projectile.Center);
                Projectile.ExpandHitboxBy(100);
                Projectile.timeLeft = 10;
                Projectile.velocity = Vector2.Zero;
                Projectile.alpha = 255;
                Projectile.ai[1] = 1;
            }
        }
    }
}
