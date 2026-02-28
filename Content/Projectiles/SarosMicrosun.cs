using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityMod;

namespace CalamityClickers.Content.Projectiles
{
    public class SarosMicrosun : ModProjectile, ILocalizedModType, IModType
    {
        public override string LocalizationCategory => "Projectiles.Summon";

        public Player Owner => Main.player[this.Projectile.owner];

        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 6;
            ProjectileID.Sets.MinionSacrificable[Type] = true;
            ProjectileID.Sets.MinionTargettingFeature[Type] = true;
            ProjectileID.Sets.CultistIsResistantTo[Type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.timeLeft = 600;
            Projectile.width = Projectile.height = 62;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.netImportant = true;
            Projectile.tileCollide = false;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
        }

        public override void AI()
        {
            NPC npc = Projectile.Center.MinionHoming(5000f, Owner);
            DoAnimation();
            Projectile.rotation += MathHelper.ToRadians(20f);
            if (npc == null)
                return;
            Projectile.velocity = Vector2.Lerp(Projectile.velocity, Utils.SafeNormalize(npc.Center - Projectile.Center, Vector2.Zero) * 35f, 0.1f);
            Projectile.netUpdate = true;
        }

        public void DoAnimation()
        {
            ++Projectile.frameCounter;
            Projectile.frame = Projectile.frameCounter / 6 % Main.projFrames[Type];
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture2D = TextureAssets.Projectile[Type].Value;
            Rectangle rectangle = Utils.Frame(texture2D, 1, Main.projFrames[Type], 0, Projectile.frame, 0, 0);
            Vector2 vector2_1 = Utils.Size(rectangle) * 0.5f;
            SpriteEffects spriteEffects = Projectile.spriteDirection == 1 ? (SpriteEffects)1 : 0;
            for (int index = 0; index < Projectile.oldPos.Length; ++index)
            {
                Color darkOrange = Color.DarkOrange;
                darkOrange.A = 25;
                Color color = (darkOrange * Projectile.Opacity) * (float)(1.0 - index / (double)Projectile.oldPos.Length);
                Vector2 vector2_2 = Projectile.oldPos[index] + Projectile.Size * 0.5f - Main.screenPosition;
                Main.EntitySpriteDraw(texture2D, vector2_2, new Rectangle?(rectangle), color, Projectile.rotation, vector2_1, Projectile.scale, spriteEffects, 0.0f);
            }
            return true;
        }

        public override void OnKill(int timeLeft)
        {
            int num = 180;
            for (int index = 0; index < num; ++index)
            {
                Vector2 vector2 = Utils.ToRotationVector2(6.28318548f / num * index) * Utils.NextFloat(Main.rand, 20f, 30f);
                Dust dust = Dust.NewDustPerfect(Projectile.Center, DustID.CopperCoin, new Vector2?(vector2), 0, new Color(), 1f);
                dust.noGravity = true;
                dust.color = Color.Lerp(Color.White, Color.Yellow, 0.25f);
                dust.scale = vector2.Length() * 0.25f;
                dust.velocity *=  0.5f;
            }
        }
    }
}
