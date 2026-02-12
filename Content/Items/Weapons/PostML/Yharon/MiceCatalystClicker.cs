using CalamityMod.Items;
using CalamityMod.Items.Materials;
using CalamityMod.Rarities;
using CalamityMod.Tiles.Furniture.CraftingStations;
using ClickerClass;
using ClickerClass.Dusts;
using ClickerClass.Items.Weapons.Clickers;
using ClickerClass.Projectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityClickers.Content.Items.Weapons.PostML.Yharon
{
    public class MiceCatalystClicker : ModdedClickerWeapon
    {
        public static string MiceCatalyst { get; internal set; } = string.Empty;
        public override float Radius => 8f;
        public override Color RadiusColor => new Color(150, 150, 225);
        public override int DustType => ModContent.DustType<MiceDust>();
        public override void SetStaticDefaultsExtra()
        {
            MiceCatalyst = ClickerCompat.RegisterClickEffect(Mod, "MiceCatalyst", 25, RadiusColor, delegate (Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, int type, int damage, float knockBack)
            {
                SoundEngine.PlaySound(SoundID.Item117, position);
                Projectile.NewProjectile(source, position, Vector2.Zero, ModContent.ProjectileType<MiceCatalystClickerProjectile>(), damage, knockBack, player.whoAmI);
            });
            CalamityClickersUtils.RegisterPostWildMagicClickEffect(MiceCatalyst);
            CalamityClickersUtils.RegisterPostNightmareMagicClickEffect(MiceCatalyst);
        }
        public override void SetDefaultsExtra()
        {
            AddEffect(Item, MiceCatalyst);
            SetDust(Item, DustType);

            Item.damage = 400;
            Item.knockBack = 1f;
            Item.rare = ModContent.RarityType<BurnishedAuric>();
            Item.value = CalamityGlobalItem.RarityVioletBuyPrice;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<MiceClicker>()
                .AddIngredient<AstralClicker>()
                .AddIngredient<AuricBar>(5)
                .AddTile<CosmicAnvil>()
                .Register();
        }
    }
    public class MiceCatalystClickerProjectile : ClickerProjectileWhichCanShootOnClick
    {
        public override bool UseInvisibleProjectile => false;

        public override void SetStaticDefaults()
        {

        }
        public override void SetDefaultsExtra()
        {
            Projectile.width = Projectile.height = 116;
            Projectile.friendly = true;
            Projectile.aiStyle = -1;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 300;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.DamageType = ModContent.GetInstance<ClickerDamage>();
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 60;
            //base.Projectile.alpha = 255;
        }
        public override void ShootOnClick()
        {
            for (int i = 0; i < 2; i++)
                Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.UnitX.RotatedByRandom(MathHelper.TwoPi) * 3, ModContent.ProjectileType<MiceClickerPro>(), Projectile.damage / 2, Projectile.knockBack, Projectile.owner, Main.rand.Next(100));
        }
        public override void AIExtra()
        {
            Vector2 target = Main.MouseWorld;
            //if (Main.player[Projectile.owner].Clicker().HasAimbotModuleTarget)
            //    target = Main.npc[Main.player[Projectile.owner].Clicker().accAimbotModuleTarget].Center;

            Projectile.velocity = (target - Projectile.Center).SafeNormalize(Vector2.Zero) * 2;
            Projectile.rotation -= 0.104719758f;
            //Projectile.Opacity = (int)MathHelper.Lerp(0, 1, MathHelper.Clamp(1f - Projectile.timeLeft / 30f, 0, 1));
            if (Projectile.timeLeft < 30)
                Projectile.Opacity = Projectile.timeLeft / 30f;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = ModContent.Request<Texture2D>(Texture).Value;
            if (Projectile.timeLeft < 30)
            {
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, Color.Lerp(Color.Black, Color.Gray, Projectile.timeLeft / 30f) * Projectile.Opacity, -Projectile.rotation * 1.1f, texture.Size() / 2, Projectile.scale + 0.1f, SpriteEffects.FlipHorizontally, 1);
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, Color.Lerp(Color.Black, Color.White, Projectile.timeLeft / 30f) * Projectile.Opacity, Projectile.rotation, texture.Size() / 2, Projectile.scale, SpriteEffects.None, 1);
            }
            else
            {
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, Color.Gray, -Projectile.rotation * 1.1f, texture.Size() / 2, Projectile.scale + 0.1f, SpriteEffects.FlipHorizontally, 1);
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation, texture.Size() / 2, Projectile.scale, SpriteEffects.None, 1);
            }
            return base.PreDraw(ref lightColor);
        }
    }
}
