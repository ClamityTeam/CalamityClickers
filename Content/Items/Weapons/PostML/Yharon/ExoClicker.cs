using CalamityMod;
using CalamityMod.Items;
using CalamityMod.Rarities;
using Microsoft.Xna.Framework;
using System;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityClickers.Content.Items.Weapons.PostML.Yharon
{
    public class ExoClicker : ModdedClickerWeapon
    {
        public static string MiniExoTwins { get; internal set; } = string.Empty;
        public override float Radius => 8f;
        public override Color RadiusColor => Color.Lerp(Main.DiscoColor, Color.Gray, 0.5f);
        public override int DustType => DustID.Stone;
        public override void SetStaticDefaultsExtra()
        {
            MiniExoTwins = ClickerCompat.RegisterClickEffect(Mod, "MiniExoTwins", 20, () => RadiusColor, delegate (Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, int type, int damage, float knockBack)
            {
                float rand = Main.rand.NextFloat(MathHelper.TwoPi);
                for (int i = 0; i < 2; i++)
                {
                    int index = Projectile.NewProjectile(source, position, Vector2.Zero, ModContent.ProjectileType<ExoClickerProjectile>(), damage, knockBack, player.whoAmI, i, rand);
                }
            });
            CalamityClickersUtils.RegisterPostWildMagicClickEffect(MiniExoTwins);
            CalamityClickersUtils.RegisterPostNightmareMagicClickEffect(MiniExoTwins);
        }
        public override void SetDefaultsExtra()
        {
            AddEffect(Item, MiniExoTwins);
            SetDust(Item, DustType);

            Item.damage = 420;
            Item.knockBack = 1f;
            Item.rare = ModContent.RarityType<BurnishedAuric>();
            Item.value = CalamityGlobalItem.RarityVioletBuyPrice;
        }
    }
    public class ExoClickerProjectile : ModdedClickerProjectile
    {
        public override bool UseInvisibleProjectile => false;
        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 2;
        }
        public override void SetDefaultsExtra()
        {
            Projectile.width = Projectile.height = 50;
            Projectile.aiStyle = -1;
            Projectile.penetrate = -1;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 300;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 5;
            //Projectile.extraUpdates = 2;
        }
        public Vector2 LastPos = Vector2.Zero;
        public override void OnSpawn(IEntitySource source)
        {
            Projectile.frame = (int)Projectile.ai[0];
            LastPos = Projectile.Center;
        }
        public override void AI()
        {
            int time = 100 - (int)(Projectile.timeLeft) % 100; //Time is up
            if (time < 70)
            {
                Vector2 target = Main.MouseWorld;
                if (Main.player[Projectile.owner].Clicker().HasAimbotModuleTarget)
                    target = Main.npc[Main.player[Projectile.owner].Clicker().accAimbotModuleTarget].Center;

                double perc = Utils.Lerp(0, MathHelper.TwoPi * 2, MathF.Pow(Utils.GetLerpValue(0, 70, time, true), 0.21f));
                Projectile.Center = Vector2.Lerp(LastPos, target, MathHelper.Clamp(0f, 1f, MathF.Pow(time / 25, 0.31f))) + Vector2.UnitX.RotatedBy(perc + MathHelper.Pi * Projectile.frame + Projectile.ai[1]) * 100;
                Projectile.rotation = (target - Projectile.Center).ToRotation() + MathHelper.PiOver2;
            }
            else
            {
                Projectile.Center += Vector2.UnitX.RotatedBy(Projectile.rotation - MathHelper.PiOver2) * 25 * MathF.Pow(1f - Utils.GetLerpValue(70, 99, time, true), 0.45f);
            }
            if (time == 99)
            {
                LastPos = Projectile.Center;
            }
            Projectile.velocity = Vector2.Zero;
        }
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(LastPos.X);
            writer.Write(LastPos.Y);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            LastPos = new Vector2(reader.ReadSingle(), reader.ReadSingle());
        }
        public override bool PreDraw(ref Color lightColor)
        {
            CalamityUtils.DrawAfterimagesCentered(Projectile, -1, lightColor);
            return false;
        }
        public override bool? CanHitNPC(NPC target)
        {
            int time = 100 - (int)(Projectile.timeLeft) % 100; //Time is up
            return time >= 70;
        }
    }
}
