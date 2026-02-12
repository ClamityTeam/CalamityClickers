using CalamityMod.Items;
using CalamityMod.Items.Materials;
using CalamityMod.Rarities;
using CalamityMod.Tiles.Furniture.CraftingStations;
using ClickerClass;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace CalamityClickers.Content.Items.Weapons.PostML.DoG
{
    public class CosmiliteClicker : ModdedClickerWeapon
    {
        public static string Distort { get; internal set; } = string.Empty;
        public override float Radius => 8f;
        public override Color RadiusColor => Color.Lerp(new Color(38, 148, 237), new Color(217, 46, 223), MathF.Sin(Main.GlobalTimeWrappedHourly) / 2 + 0.5f);

        public override void SetStaticDefaultsExtra()
        {
            Distort = ClickerCompat.RegisterClickEffect(Mod, "Distort", 12, RadiusColor, delegate (Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, int type, int damage, float knockBack)
            {
                for (int i = 0; i < 5; i++)
                {
                    Projectile.NewProjectile(source, position, Vector2.UnitX.RotatedByRandom(MathHelper.TwoPi) * 5, ModContent.ProjectileType<CosmiliteClickerProjectile>(), damage, knockBack, player.whoAmI);
                }
            });
            CalamityClickersUtils.RegisterPostWildMagicClickEffect(Distort);
        }
        public override void SetDefaultsExtra()
        {
            AddEffect(Item, Distort);
            SetDust(Item, DustType);

            Item.damage = 350;
            Item.knockBack = 1f;
            Item.rare = ModContent.RarityType<CosmicPurple>();
            Item.value = CalamityGlobalItem.RarityDarkBlueBuyPrice;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<CosmiliteBar>(12)
                .AddTile<CosmicAnvil>()
                .Register();
        }
    }
    public class CosmiliteClickerProjectile : ModdedClickerProjectile
    {
        public static int MaxTimeleft = 600;
        public static int AfterClickFly = 60;
        public static int SlowDawn = 30;
        public override bool UseInvisibleProjectile => false;
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            Main.projFrames[Type] = 2;
        }
        public override void SetDefaultsExtra()
        {
            Projectile.width = Projectile.height = 14;
            Projectile.aiStyle = -1;
            AIType = -1;
            Projectile.timeLeft = MaxTimeleft;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 60;
            Projectile.penetrate = -1;
            Projectile.DamageType = ModContent.GetInstance<ClickerDamage>();
            Projectile.scale = 1.2f;
        }
        public override void OnSpawn(IEntitySource source)
        {
            Projectile.ai[0] = Main.rand.NextFloat(0.9f, 1.1f);
            Projectile.ai[0] *= Main.rand.NextBool() ? 1 : -1;
            Projectile.frame = Main.rand.Next(2);
        }
        public override void AI()
        {
            /*if (Projectile.timeLeft < MaxTimeleft - PreClickFly)
            {
                if (Projectile.ai[1] != 1)
                {

                }
                else
                    Projectile.velocity *= 0.97f;
                if (Projectile.wet)
                {

                }
            }
            else
            {
            }*/

            if (Projectile.ai[0] > 0)
            {
                Projectile.ai[0]--;
            }
            else
            {
                if (Projectile.Owner().ItemAnimationActive)
                {
                    Projectile.velocity = (Main.MouseWorld - Projectile.Center).SafeNormalize(Vector2.UnitX) * 15;
                    Projectile.rotation = Projectile.velocity.ToRotation();
                    Projectile.ai[0] = AfterClickFly;
                }
            }
            if (Projectile.ai[0] < SlowDawn)
            {
                Projectile.rotation += (1f - (MaxTimeleft - Projectile.timeLeft) / MaxTimeleft) * MathHelper.PiOver4 * Projectile.ai[0] * Utils.GetLerpValue(MaxTimeleft - AfterClickFly, MaxTimeleft, Projectile.timeLeft);
                Projectile.velocity *= 0.92f;
            }
        }
    }
}
