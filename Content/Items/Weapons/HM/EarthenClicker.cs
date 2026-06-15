using CalamityMod.Buffs.StatDebuffs;
using CalamityMod.Items;
using ClickerClass;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityClickers.Content.Items.Weapons.HM
{
    public class EarthenClicker : ModdedClickerWeapon
    {
        public static string Crunch { get; internal set; } = string.Empty;
        public override float Radius => 2.95f;
        public override Color RadiusColor => new Color(152, 152, 155);
        public override void SetStaticDefaultsExtra()
        {
            Crunch = ClickerSystem.RegisterClickEffect(Mod, "Crunch", 10, RadiusColor, delegate (Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, int type, int damage, float knockBack)
            {
                Projectile.NewProjectile(source, position, Vector2.Zero, ModContent.ProjectileType<EarthenClickerProjectile>(), damage, knockBack, player.whoAmI);
            });
        }
        public override void SetDefaultsExtra()
        {
            AddEffect(Item, Crunch);

            Item.damage = 22;
            Item.knockBack = 2f;
            Item.rare = ItemRarityID.Pink;
            Item.value = CalamityGlobalItem.RarityPinkBuyPrice;
        }
    }
    public class EarthenClickerProjectile : ModdedClickerProjectile
    {
        public bool Spawned
        {
            get => Projectile.ai[0] == 1f;
            set => Projectile.ai[0] = value ? 1f : 0f;
        }
        public override void SetDefaultsExtra()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 10;
            Projectile.alpha = 255;
            Projectile.extraUpdates = 3;
        }
        public override void AI()
        {
            if (!Spawned)
            {
                Spawned = true;

                SoundEngine.PlaySound(SoundID.Item10, Projectile.Center);

                for (int u = 0; u < Main.maxNPCs; u++)
                {
                    NPC target = Main.npc[u];
                    if (target.CanBeChasedBy() && target.DistanceSQ(Projectile.Center) < 158 * 158)
                    {
                        target.AddBuff(ModContent.BuffType<ArmorCrunch>(), 180);
                        //target.Hitbox.Intersects

                        for (int i = 0; i < 15; i++)
                        {
                            int index = Dust.NewDust(target.position, target.width, target.height, DustID.Stone, 0f, 0f, 100, default(Color), 1f);
                            Dust dust = Main.dust[index];
                            dust.noGravity = true;
                            dust.velocity *= 0.75f;
                            int x = Main.rand.Next(-50, 51);
                            int y = Main.rand.Next(-50, 51);
                            dust.position.X += x;
                            dust.position.Y += y;
                            dust.velocity.X = -x * 0.075f;
                            dust.velocity.Y = -y * 0.075f;
                        }
                    }
                }
            }
        }
    }
}
