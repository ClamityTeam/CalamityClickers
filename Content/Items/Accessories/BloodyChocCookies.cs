using CalamityClickers.Content.Items.Weapons;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Items;
using CalamityMod.Items.Materials;
using ClickerClass;
using ClickerClass.Items;
using ClickerClass.Items.Accessories;
using ClickerClass.Projectiles;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityClickers.Content.Items.Accessories
{
    public class BloodyChocCookies : ClickerItem, ILocalizedModType, IModType
    {
        public new string LocalizationCategory => "Items.Accessories";
        public static string BurnOrBliss { get; internal set; } = string.Empty;
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            BurnOrBliss = ClickerSystem.RegisterClickEffect(Mod, "BurnOrBliss", 15, new Color(165, 110, 60, 0), delegate (Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, int type, int damage, float knockBack)
            {
                bool spawnEffects = true;
                int chocolate = ModContent.ProjectileType<BloodyChocCookiesProjectileChocolate>();
                for (int k = 0; k < 6; k++)
                {
                    float hasSpawnEffects = spawnEffects ? 1f : 0f;
                    int rand = Main.rand.Next(Main.projFrames[chocolate]);
                    Projectile.NewProjectile(source, position.X, position.Y, Main.rand.NextFloat(-10f, 10f), Main.rand.NextFloat(-10f, 10f), chocolate, Math.Max(1, (int)(damage * (0.2f + (rand > 5 ? 0.1f : 0)))), 0f, player.whoAmI, rand, hasSpawnEffects);
                    spawnEffects = false;
                }
            });
        }
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.accessory = true;
            Item.value = CalamityGlobalItem.RarityYellowBuyPrice;
            Item.rare = ItemRarityID.Yellow;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            //player.GetModPlayer<CalamityClickersPlayer>().bloodyChocolate = true;
            player.GetModPlayer<CalamityClickersPlayer>().accBloodyChocolate = true;
            player.GetModPlayer<CalamityClickersPlayer>().accBloodyChocolateItem = Item;
            player.GetModPlayer<ClickerPlayer>().EnableClickEffect(BurnOrBliss);
            player.Clicker().accGlassOfMilk = true;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<ChocolateMilkCookies>()
                .AddIngredient<BloodOrb>(20)
                .AddIngredient<AshesofCalamity>(5)
                .AddIngredient<ScoriaBar>(2)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
    public class BloodyChocCookiesProjectileCookie : CookiePro, ILocalizedModType
    {
        public new string LocalizationCategory => "Projectiles.Clicker";
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            Main.projFrames[Projectile.type] = 4;
        }

        public override void OnSpawn(IEntitySource source)
        {
            if (source is EntitySource_Parent entitySource_Parent && entitySource_Parent.Entity is Player player)
            {
                if (Main.rand.NextFloat() <= 0.1f || player.name == "DebugMan")
                {
                    Frame = 2;
                    if (player.name is "Puka1611" or "Puka")
                        Frame = 3;
                }

                else if (Main.rand.NextFloat() <= 0.1f)
                {
                    Frame = 1;
                }

                //Frame = 2;
            }
        }

    }
    public class BloodyChocCookiesBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            CalamityClickersPlayer clickerPlayer = player.GetModPlayer<CalamityClickersPlayer>();
            clickerPlayer.bloodyCookieBuff = true;
            player.GetDamage<ClickerDamage>() += 0.15f;
            player.GetCritChance<ClickerDamage>() += 0.15f;
            clickerPlayer.rageRegenMult += 0.1f;
        }
    }
    public class BloodyChocCookiesProjectileChocolate : ModdedClickerProjectile
    {
        public override bool UseInvisibleProjectile => false;
        //0-2 - normal
        //3-5 - white
        //6-8 - spicy
        public int Frame
        {
            get => (int)Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }

        public bool HasSpawnEffects
        {
            get => Projectile.ai[1] == 1f;
            set => Projectile.ai[1] = value ? 1f : 0f;
        }
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            Main.projFrames[Projectile.type] = 9;
        }
        public override void SetDefaultsExtra()
        {
            Projectile.width = 22;
            Projectile.height = 22;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 60;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 20;
        }
        public override void AI()
        {
            if (HasSpawnEffects)
            {
                HasSpawnEffects = false;

                SoundEngine.PlaySound(SoundID.Item112, Projectile.Center);
                for (int k = 0; k < 20; k++)
                {
                    Dust dust = Dust.NewDustDirect(Projectile.Center - new Vector2(4), 8, 8, DustID.Pot, Main.rand.NextFloat(-6f, 6f), Main.rand.NextFloat(-6f, 6f), 125, default, 1.5f);
                    dust.noGravity = true;
                    dust.noLight = true;
                }
            }

            Projectile.frame = Frame;
            if (Main.player[Projectile.owner].name is "Puka1611" or "Puka")
                Projectile.frame = Main.rand.Next(6, 9);

            if (Frame is 3 or 4 or 5)
            {
                // Get a target and calculate distance from it
                int target = Player.FindClosest(Projectile.Center, 1, 1);
                Player player = Main.player[target];
                Vector2 distanceFromTarget = player.Center - Projectile.Center;

                if (target == Projectile.owner)
                {
                    if (distanceFromTarget.Length() < 300)
                    {
                        float scaleFactor = Projectile.velocity.Length();
                        float inertia = 6;
                        distanceFromTarget.Normalize();
                        distanceFromTarget *= scaleFactor;
                        Projectile.velocity = (Projectile.velocity * inertia + distanceFromTarget) / (inertia + 1f);
                        Projectile.velocity.Normalize();
                        Projectile.velocity *= scaleFactor;
                    }
                    else
                        Projectile.velocity *= 0.9f;

                    float playerDist = distanceFromTarget.Length();
                    if (playerDist < 50f && Projectile.position.X < player.position.X + player.width && Projectile.position.X + Projectile.width > player.position.X && Projectile.position.Y < player.position.Y + player.height && Projectile.position.Y + Projectile.height > player.position.Y)
                    {
                        if (Projectile.owner == Main.myPlayer && !Main.player[Main.myPlayer].moonLeech)
                        {
                            player.HealEffect(2/*, broadcast: false*/);
                            player.statLife += 2;
                            if (player.statLife > player.statLifeMax2)
                            {
                                player.statLife = player.statLifeMax2;
                            }

                            NetMessage.SendData(MessageID.SpiritHeal, -1, -1, null, target, 4);
                        }
                        Projectile.Kill();
                    }
                }
                else
                    Projectile.velocity *= 0.9f;
            }
            else
                Projectile.velocity *= 0.9f;

            Projectile.rotation += Projectile.velocity.X > 0f ? 0.08f : 0.08f;
            if (Projectile.timeLeft < 20)
            {
                Projectile.alpha += 8;
            }
        }

        public override void OnKill(int timeLeft)
        {
            for (int k = 0; k < 8; k++)
            {
                int dust = Dust.NewDust(Projectile.position, (int)(Projectile.width * 0.5f), (int)(Projectile.height * 0.5f), DustID.Pot, Main.rand.Next((int)-2f, (int)2f), Main.rand.Next((int)-2f, (int)2f), 125, default, 1.25f);
                Main.dust[dust].noGravity = true;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Frame is 6 or 7 or 8)
            {
                target.AddBuff(ModContent.BuffType<BrimstoneFlames>(), 120);
            }
        }
    }
}
