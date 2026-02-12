using CalamityMod.Items;
using CalamityMod.Items.Materials;
using CalamityMod.NPCs.SlimeGod;
using CalamityMod.Tiles.Furniture.CraftingStations;
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
    public class GooClicker : ModdedClickerWeapon
    {
        public static string SlimePuppet { get; internal set; } = string.Empty;
        public override float Radius => 2.8f;
        public override Color RadiusColor => Color.Lerp(new Color(91, 123, 205), new Color(195, 70, 124), MathF.Sin(Main.GlobalTimeWrappedHourly * 3) * 0.5f + 0.5f);
        public override void SetStaticDefaultsExtra()
        {
            SlimePuppet = ClickerSystem.RegisterClickEffect(Mod, "SlimePuppet", 6, new Color(243, 79, 174), delegate (Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, int type, int damage, float knockBack)
            {
                SoundEngine.PlaySound(SlimeGodCore.PossessionSound, position);
                Projectile.NewProjectile(source, position, Vector2.UnitX.RotatedByRandom(MathHelper.ToRadians(180)), ModContent.ProjectileType<GooClickerProjectile>(), damage * 2, knockBack, player.whoAmI);
            }, true);
        }
        public override void SetDefaultsExtra()
        {
            AddEffect(Item, SlimePuppet);

            Item.damage = 13;
            Item.knockBack = 1f;
            Item.rare = ItemRarityID.LightRed;
            Item.value = CalamityGlobalItem.RarityLightRedBuyPrice;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<PurifiedGel>(18)
                .AddIngredient<BlightedGel>(18)
                .AddTile(TileID.Solidifier)
                .Register();
        }
    }
    public class GooClickerProjectile : ModdedClickerProjectile
    {
        public override bool UseInvisibleProjectile => false;
        public override void SetDefaultsExtra()
        {
            Projectile.width = 24;
            Projectile.height = 24;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 120;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.aiStyle = -1;
        }

        public override void OnSpawn(IEntitySource source)
        {
            Projectile.localAI[0] = Main.rand.NextFloat(-0.15f, 0.15f);
        }

        public override void AI()
        {
            var direction = Main.MouseWorld - Projectile.Center;
            switch (Projectile.ai[0])
            {
                case 0:
                    /*if (Main.rand.NextBool(3))
                    {
                        var womp = Main.rand.NextVector2Circular(4, 4);
                        var d = Dust.NewDustPerfect(Projectile.Center + womp * 12, DustID.Flare_Blue, womp, 47, default, 2);
                        d.noGravity = true;
                    }*/

                    Projectile.ai[1] += Projectile.localAI[0] + Projectile.ai[2] / 98;
                    if (Projectile.ai[2] <= 4)
                    {
                        Projectile.ai[2] += 0.06f;
                    }
                    else
                    {
                        SoundEngine.PlaySound(SoundID.Item19, Projectile.Center);
                        Projectile.velocity = new Vector2(19, 0).RotatedBy(direction.ToRotation());
                        Projectile.ai[0] = 1f;
                    }

                    Projectile.position.X += (float)Math.Sin(Projectile.ai[1]) * Projectile.ai[2];
                    Projectile.position.Y += (float)Math.Cos(Projectile.ai[1]) * Projectile.ai[2];
                    break;
                case 1:
                    /*if (Main.rand.NextBool(5) && Projectile.alpha <= 180)
                    {
                        var womp = Main.rand.NextVector2Circular(4, 4);
                        var d = Dust.NewDustPerfect(Projectile.Center + womp * 9, DustID.PinkSlime, womp / 2, 47, default, 1.4f);
                        d.noGravity = true;
                    }*/

                    Projectile.velocity /= 1.1f;
                    Projectile.alpha += 9;

                    break;
            }
        }
    }
}
