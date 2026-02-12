using CalamityMod;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Items;
using CalamityMod.Rarities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityClickers.Content.Items.Weapons.PostML.DoG
{
    public class ClickerOfGods : ModdedClickerWeapon
    {
        public static string NebulaStars { get; internal set; } = string.Empty;
        public override float Radius => 8f;
        public override Color RadiusColor => Color.Lerp(new Color(39, 151, 171), new Color(252, 109, 202), MathF.Sin(Main.GlobalTimeWrappedHourly * 2) / 2 + 0.5f);
        public override int DustType => Main.rand.NextBool() ? DustID.GemSapphire : DustID.GemAmethyst;
        public override void SetStaticDefaultsExtra()
        {
            NebulaStars = ClickerCompat.RegisterClickEffect(Mod, "NebulaStars", 7, () => RadiusColor, delegate (Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, int type, int damage, float knockBack)
            {
                for (int i = 0; i < 7; i++)
                {
                    Projectile.NewProjectile(source, position + Vector2.UnitX.RotatedBy(MathHelper.TwoPi / 7 * i) * 10, Vector2.UnitX.RotatedBy(MathHelper.TwoPi / 7 * i) * 3, ModContent.ProjectileType<ClickerOfGodsProjectile>(), damage / 2, knockBack, player.whoAmI);
                }
            });
            CalamityClickersUtils.RegisterPostWildMagicClickEffect(NebulaStars);
        }
        public override void SetDefaultsExtra()
        {
            AddEffect(Item, NebulaStars);
            SetDust(Item, DustType);

            Item.damage = 340;
            Item.knockBack = 1f;
            Item.rare = ModContent.RarityType<CosmicPurple>();
            Item.value = CalamityGlobalItem.RarityDarkBlueBuyPrice;
        }
    }
    public class ClickerOfGodsProjectile : ModdedClickerProjectile
    {
        public override bool UseInvisibleProjectile => false;
        public override void SetDefaultsExtra()
        {
            Projectile.width = Projectile.height = 34;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            //Projectile.alpha = 255;
            Projectile.penetrate = 1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 1000;
        }
        public override void AI()
        {
            Projectile.rotation += 0.02f * Projectile.velocity.Length();
            if (Projectile.timeLeft < 900)
                CalamityUtils.HomeInOnNPC(Projectile, true, 1000, 10, 10f);
        }
        public override void PostDraw(Color lightColor)
        {
            Rectangle frame = new Rectangle(0, 0, Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value.Width, Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value.Height);
            Main.EntitySpriteDraw(ModContent.Request<Texture2D>(Texture + "_Glow").Value, Projectile.Center - Main.screenPosition, frame, Color.White * ((255 - Projectile.alpha) / 255f), Projectile.rotation, Projectile.Size / 2, 1f, SpriteEffects.None, 0);
        }
        public override bool? CanHitNPC(NPC target)
        {
            return Projectile.timeLeft < 900;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<GodSlayerInferno>(), 60);
        }
    }
}
