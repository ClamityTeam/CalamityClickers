using CalamityMod.Items;
using CalamityMod.Items.Materials;
using CalamityMod.Projectiles.Rogue;
using CalamityMod.Rarities;
using ClickerClass;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityClickers.Content.Items.Weapons.PostML.Providance
{
    public class BloomClicker : ModdedClickerWeapon
    {
        public static string TarragonThorns { get; internal set; } = string.Empty;
        public override float Radius => 7f;
        public override Color RadiusColor => new Color(67, 99, 83);

        public override void SetStaticDefaultsExtra()
        {
            TarragonThorns = ClickerCompat.RegisterClickEffect(Mod, "TarragonThorns", 10, RadiusColor, delegate (Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, int type, int damage, float knockBack)
            {
                float random = Main.rand.Next(30, 90);
                float spread = random * 0.0174f;
                double startAngle = Main.rand.NextFloat(0, MathHelper.TwoPi) - spread / 2;
                double deltaAngle = spread / 8f;

                int projID = ModContent.ProjectileType<TarraThornRight>();
                int splitDamage = damage;
                float splitKB = 1f;
                for (int i = 0; i < 4; i++)
                {
                    double offsetAngle = startAngle + deltaAngle * (i + i * i) / 2f + 32f * i;
                    int index = Projectile.NewProjectile(source, position.X, position.Y, (float)(Math.Sin(offsetAngle) * 5f) * 2f, (float)(Math.Cos(offsetAngle) * 5f) * 2f, projID, splitDamage, splitKB, player.whoAmI);
                    Main.projectile[index].DamageType = ModContent.GetInstance<ClickerDamage>();
                    index = Projectile.NewProjectile(source, position.X, position.Y, (float)(-Math.Sin(offsetAngle) * 5f) * 2f, (float)(-Math.Cos(offsetAngle) * 5f) * 2f, projID, splitDamage, splitKB, player.whoAmI);
                    Main.projectile[index].DamageType = ModContent.GetInstance<ClickerDamage>();
                }
            });
            CalamityClickersUtils.RegisterPostWildMagicClickEffect(TarragonThorns);
        }
        public override void SetDefaultsExtra()
        {
            AddEffect(Item, TarragonThorns);
            SetDust(Item, DustID.CursedTorch);

            Item.damage = 200;
            Item.knockBack = 1f;
            Item.rare = ModContent.RarityType<Turquoise>();
            Item.value = CalamityGlobalItem.RarityTurquoiseBuyPrice;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                //.AddIngredient<PointyClicker>()
                .AddIngredient<UelibloomBar>(8)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}
