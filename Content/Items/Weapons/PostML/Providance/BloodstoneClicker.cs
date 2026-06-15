using CalamityMod;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Buffs.StatDebuffs;
using CalamityMod.Cooldowns;
using CalamityMod.Items;
using CalamityMod.Items.Materials;
using CalamityMod.Rarities;
using Microsoft.Xna.Framework;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityClickers.Content.Items.Weapons.PostML.Providance
{
    internal class BloodstoneClicker : ModdedClickerWeapon
    {
        public static string Lifestealing { get; internal set; } = string.Empty;
        public static string BloodyKnives { get; internal set; } = string.Empty;
        public override float Radius => 7f;
        public override Color RadiusColor => new Color(204, 42, 60);
        public override void SetStaticDefaultsExtra()
        {
            Lifestealing = ClickerCompat.RegisterClickEffect(Mod, "Lifestealing", 1, RadiusColor, delegate (Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, int type, int damage, float knockBack)
            {
                //
                if (player.moonLeech || player.HasCooldown(LifeSteal.ID) /*|| !player*/)
                    return;
                /*NPC npc = CalamityUtils.ClosestNPCAt(position, 32, true);
                if (npc != null || npc.active)
                    if (npc.HasBuff<MarkedforDeath>())
                    {
                        player.Heal(2);
                        player.AddCooldown(LifeSteal.ID, 20);
                    }*/
                foreach (var npc in Main.npc)
                {
                    if (npc == null || !npc.active) continue;
                    if ((position - npc.Center).Length() < 32)
                    {
                        if (npc.HasBuff<MarkedforDeath>())
                        {
                            player.Heal(2);
                            player.AddCooldown(LifeSteal.ID, 20);
                            break;
                        }
                    }
                }

            });
            CalamityClickersUtils.RegisterPostWildMagicClickEffect(Lifestealing);
            CalamityClickersUtils.RegisterBlacklistedClickEffect(Lifestealing);
            BloodyKnives = ClickerCompat.RegisterClickEffect(Mod, "BloodyKnives", 20, RadiusColor, delegate (Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, int type, int damage, float knockBack)
            {
                for (int i = 0; i < 8; i++)
                    Projectile.NewProjectile(source, position, Vector2.UnitX.RotatedBy(MathHelper.TwoPi / 8 * i) * 10, ModContent.ProjectileType<BloodstoneClickerProjectile>(), damage, knockBack, player.whoAmI);
            });
            CalamityClickersUtils.RegisterPostWildMagicClickEffect(BloodyKnives);
        }
        public override void SetDefaultsExtra()
        {
            AddEffect(Item, Lifestealing);
            AddEffect(Item, BloodyKnives);
            //AddEffect(Item, ClickEffect.Linger);
            SetDust(Item, DustID.Blood);

            Item.damage = 200;
            Item.knockBack = 1f;
            Item.rare = ModContent.RarityType<Turquoise>();
            Item.value = CalamityGlobalItem.RarityTurquoiseBuyPrice;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                //.AddIngredient<HemoClicker>()
                .AddIngredient<BloodstoneCore>(5)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
    public class BloodstoneClickerProjectile : ModdedClickerProjectile
    {
        public override bool UseInvisibleProjectile => false;
        public static int MaxTimeLeft = 300;
        public override void SetDefaultsExtra()
        {
            Projectile.width = Projectile.height = 30;
            Projectile.aiStyle = -1;
            Projectile.penetrate = -1;
            Projectile.timeLeft = MaxTimeLeft;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.friendly = true;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 5;
        }
        public Vector2 EnemyOffset = Vector2.Zero;
        public override void OnSpawn(IEntitySource source)
        {
            Projectile.ai[2] = -1;
        }
        public override void AI()
        {
            if (Projectile.ai[2] != -1)
            {
                NPC npc = Main.npc[(int)Projectile.ai[2]];
                if (npc is null || !npc.active)
                {
                    Projectile.ai[2] = -1;
                    return;
                }
                Projectile.Center = npc.Center + EnemyOffset;
                npc.AddBuff(ModContent.BuffType<BurningBlood>(), 30);
                npc.AddBuff(ModContent.BuffType<MarkedforDeath>(), 30);
            }
            else
            {
                CalamityUtils.HomeInOnNPC(Projectile, true, 1200, 5, 40f);
            }
            Projectile.rotation = Projectile.velocity.ToRotation();
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            //target.AddBuff()
            Projectile.ai[2] = target.whoAmI;
            EnemyOffset = Projectile.Center - target.Center;
        }
        public override bool? CanHitNPC(NPC target)
        {
            //return base.CanHitNPC(target);
            return /*target.Hitbox.Intersects(Projectile.Hitbox) &&*/ Projectile.ai[2] == -1 && Projectile.timeLeft < MaxTimeLeft - 30;
        }
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(EnemyOffset.X);
            writer.Write(EnemyOffset.Y);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            EnemyOffset = new Vector2(reader.ReadSingle(), reader.ReadSingle());
        }
    }
}
