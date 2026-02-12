using CalamityClickers.Content.Dusts;
using CalamityMod.Items;
using CalamityMod.NPCs.Other;
using CalamityMod.Rarities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace CalamityClickers.Content.Items.Weapons.PostML.Yharon
{
    public class CruelClicker : ModdedClickerWeapon
    {
        public static string LecherousOrb { get; internal set; } = string.Empty;
        //public static string ClickerEffect2 { get; internal set; } = string.Empty;
        public override float Radius => 9f;
        public override Color RadiusColor => new Color(176, 28, 80);
        public override int DustType => ModContent.DustType<BrimstoneFlameClickers>();
        public override void SetStaticDefaultsExtra()
        {
            //Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 9));
            //ItemID.Sets.AnimatesAsSoul[Type] = true;

            /*ClickerEffect = ClickerCompat.RegisterClickEffect(Mod, "Hurt", 1, RadiusColor, delegate (Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, int type, int damage, float knockBack)
            {
                player.HealEffect(-2);
                player.statLife -= 2;
            }, postMoonLord: true);
            CalamityClickersUtils.RegisterBlacklistedClickEffect(ClickerEffect);*/

            LecherousOrb = ClickerCompat.RegisterClickEffect(Mod, "LecherousOrb", 20, RadiusColor, delegate (Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, int type, int damage, float knockBack)
            {
                Projectile.NewProjectile(source, position + Main.rand.NextVector2Circular(200, 200), Main.rand.NextVector2CircularEdge(1, 1), ModContent.ProjectileType<CruelClickerProjectile>(), damage, knockBack, player.whoAmI);
                player.Hurt(PlayerDeathReason.ByCustomReason(" has been sacrificed"), 20, 1, dodgeable: false, scalingArmorPenetration: 1f, knockback: 0);
            });
            CalamityClickersUtils.RegisterPostWildMagicClickEffect(LecherousOrb);
            CalamityClickersUtils.RegisterPostNightmareMagicClickEffect(LecherousOrb);
        }
        public override void SetDefaultsExtra()
        {
            AddEffect(Item, LecherousOrb);
            SetDust(Item, DustType);

            Item.damage = 850;
            Item.knockBack = 1f;
            Item.rare = ModContent.RarityType<BurnishedAuric>();
            Item.value = CalamityGlobalItem.RarityVioletBuyPrice;
        }
        /*private void DrawingAnimation(SpriteBatch spriteBatch, Vector2 baseDrawPosition, float baseScale, Color drawColor)
        {
            //if (Item.velocity.X != 0f)
            //    return;
            Texture2D t = ModContent.Request<Texture2D>(Texture + "/" + ((int)(Main.GlobalTimeWrappedHourly * 3) % 9).ToString()).Value;

            spriteBatch.Draw(t, baseDrawPosition, null, Color.White, 0f, Vector2.Zero, baseScale, SpriteEffects.None, 0f);

        }
        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            if (!CalamityClickersConfig.Instance.LegecyClickerTextures)
            {
                //Rectangle frame = TextureAssets.Item[Item.type].Value.Frame();
                DrawingAnimation(spriteBatch, Item.position - Main.screenPosition, scale, lightColor);
                return false;
            }
            return base.PreDrawInWorld(spriteBatch, lightColor, alphaColor, ref rotation, ref scale, whoAmI);
        }
        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            if (!CalamityClickersConfig.Instance.LegecyClickerTextures)
            {
                //Rectangle frame = TextureAssets.Item[Item.type].Value.Frame();
                Item.velocity.X = 0f;
                DrawingAnimation(spriteBatch, Item.position - frame.Size() * 0.25f, scale, drawColor);
                return true;
            }
            return PreDrawInInventory(spriteBatch, position, frame, drawColor, itemColor, origin, scale);
        }*/

        /*public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            if (!CalamityClickersConfig.Instance.LegecyClickerTextures)
            {
                //Item.DrawItemGlowmaskSingleFrame(spriteBatch, Item.rot)
            }


        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            if (!CalamityClickersConfig.Instance.LegecyClickerTextures)
            {
                Item.DrawItemGlowmaskSingleFrame(spriteBatch, rotation, ModContent.Request<Texture2D>(Texture + "/" + ((int)(Main.GlobalTimeWrappedHourly * 3) % 9).ToString()).Value);
            }
        }*/
    }
    public class CruelClickerProjectile : ClickableClickerProjectile
    {
        public override string Texture => ModContent.GetInstance<LecherousOrb>().Texture;
        public override void SetStaticDefaults()
        {

        }
        public override void SetDefaultsExtra()
        {
            Projectile.width = 64; Projectile.height = 90;
            Projectile.aiStyle = -1;
            AIType = -1;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 800;
        }
        public ref float Frame => ref Projectile.localAI[0];
        public ref float Time => ref Projectile.ai[0];
        public override void SafeAI()
        {
            if (Frame != 0f)
            {
                Projectile.frameCounter += 1;
            }
            else
            {
                Projectile.frameCounter = 0;
            }

            if (Frame >= 1f && Frame < 8f && Projectile.frameCounter % 6 == 5)
            {
                Frame += 1f;
                if (Frame >= 8f)
                {
                    Frame = 0f;
                }
            }

            if (Frame >= 8f && Frame < 25f && Projectile.frameCounter % 5 == 4)
            {
                Frame += 1f;
                if (Frame >= 25f)
                {
                    Frame = 0f;
                }
            }

            Projectile.velocity *= 0.98f;
            Time += 1f;

        }
        public override void OnClick()
        {
            int num = (int)MathHelper.Lerp(0f, 4f, Utils.GetLerpValue(15f, 600f, Time, clamped: true));
            for (int i = 0; i < num; i++)
            {
                Item.NewItem(Projectile.GetSource_FromThis(), Projectile.Hitbox, 58);
            }
            Projectile.Kill();
        }
        public override bool PreDraw(ref Color lightColor)
        {
            int num = (int)Frame % 22;
            int num2 = (int)Frame / 22;
            Rectangle rect = new Rectangle(num2 * 64, num * 90, 64, 90);

            Texture2D value = ModContent.Request<Texture2D>(Texture).Value;
            Vector2 position = Projectile.Center - Main.screenPosition;
            SpriteEffects effects = ((Projectile.spriteDirection != 1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None);
            float num3 = Main.GlobalTimeWrappedHourly * 1.9f % 1f;
            float scale = Projectile.scale * (1f + num3 * 0.33f);
            Color color = Projectile.GetAlpha(Color.Red) * (1f - num3) * 0.44f;
            Main.spriteBatch.Draw(value, position, rect, color, Projectile.rotation, rect.Size() * 0.5f, scale, effects, 0f);
            Main.spriteBatch.Draw(value, position, rect, Projectile.GetAlpha(lightColor), Projectile.rotation, rect.Size() * 0.5f, Projectile.scale, effects, 0f);

            return false;
        }
    }
}
