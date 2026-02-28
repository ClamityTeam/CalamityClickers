using CalamityClickers.Content.Projectiles;
using CalamityMod.Items;
using CalamityMod.Items.Materials;
using CalamityMod.Rarities;
using CalamityMod.Tiles.Furniture.CraftingStations;
using ClickerClass;
using ClickerClass.Items.Weapons.Clickers;
using ClickerClass.Projectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityClickers.Content.Items.Weapons.PostML.DoG
{
    public class DarksunClicker : ModdedClickerWeapon
    {
        public static string Totality2 { get; internal set; } = string.Empty;
        public override float Radius => 8f;
        public override Color RadiusColor => new Color(225, 191, 73);
        public override void SetStaticDefaultsExtra()
        {
            Totality2 = ClickerCompat.RegisterClickEffect(Mod, "Totality2", 15, RadiusColor, delegate (Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, int type, int damage, float knockBack)
            {
                Projectile.NewProjectile(source, position, Vector2.Zero, ModContent.ProjectileType<DarksunClickerProjectile>(), damage, knockBack, player.whoAmI);
            });
            CalamityClickersUtils.RegisterPostWildMagicClickEffect(Totality2);
        }
        public override void SetDefaultsExtra()
        {
            AddEffect(Item, Totality2);
            SetDust(Item, DustID.SolarFlare);

            Item.damage = 300;
            Item.knockBack = 1f;
            Item.rare = ModContent.RarityType<CosmicPurple>();
            Item.value = CalamityGlobalItem.RarityDarkBlueBuyPrice;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<EclipticClicker>()
                .AddIngredient<CosmiliteBar>(8)
                .AddIngredient<DarksunFragment>(8)
                .AddTile<CosmicAnvil>()
                .Register();
        }
    }
    public class DarksunClickerProjectile : ModdedClickerProjectile
    {
        public override bool UseInvisibleProjectile => false;

        public static Lazy<Asset<Texture2D>> effect;

        public override void Load()
        {
            effect = new(() => ModContent.Request<Texture2D>(Texture + "_Effect"));
        }

        public override void Unload()
        {
            effect = null;
        }

        public bool shift = false;
        public float pulse = 0f;
        public float rotation = 0f;

        public bool Spawned
        {
            get => Projectile.ai[0] == 1f;
            set => Projectile.ai[0] = value ? 1f : 0f;
        }

        public int Timer
        {
            get => (int)Projectile.ai[1];
            set => Projectile.ai[1] = value;
        }

        public int Timer2
        {
            get => (int)Projectile.ai[2];
            set => Projectile.ai[2] = value;
        }

        public override void SetDefaultsExtra()
        {
            Projectile.width = 50;
            Projectile.height = 50;
            Projectile.aiStyle = -1;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 240;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(255, 255, 255, 255) * (0.1f + 0.005f * Projectile.timeLeft);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            //TextureAssets.Item[1]
            Texture2D texture = effect.Value.Value;
            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, new Color(255, 255, 255, 0) * ((0.65f + pulse) * (0.01f * Projectile.timeLeft)), rotation, new Vector2(58, 58), 1.35f + pulse, SpriteEffects.None, 0);
            return true;
        }

        public override void AI()
        {
            if (!Spawned)
            {
                Spawned = true;
                SoundEngine.PlaySound(SoundID.Item43, Projectile.Center);
            }

            rotation += 0.01f;
            pulse += !shift ? 0.0035f : -0.0035f;
            if (pulse > 0.15f && !shift)
            {
                shift = true;
            }
            if (pulse <= 0f)
            {
                shift = false;
            }

            Timer2++;
            if (Timer2 > 30)
            {
                int index = -1;
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC npc = Main.npc[i];
                    if (npc.CanBeChasedBy() && Projectile.DistanceSQ(npc.Center) < 400f * 400f && Collision.CanHit(Projectile.Center, 1, 1, npc.Center, 1, 1))
                    {
                        index = i;
                    }
                }
                if (index != -1)
                {
                    Vector2 vector = Main.npc[index].Center - Projectile.Center;
                    float speed = 3f;
                    float mag = vector.Length();
                    if (mag > speed)
                    {
                        mag = speed / mag;
                        vector *= mag;
                    }

                    if (Projectile.owner == Main.myPlayer)
                    {
                        float numberProjectiles = 3;
                        float rotation = MathHelper.ToRadians(20);
                        for (int i = 0; i < numberProjectiles; i++)
                        {
                            Vector2 perturbedSpeed = new Vector2(vector.X, vector.Y).RotatedByRandom(MathHelper.TwoPi) * 1f;
                            int p = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, perturbedSpeed, ModContent.ProjectileType<SarosMicrosun>(), (int)(Projectile.damage * 0.25f), Projectile.knockBack, Projectile.owner);
                            Main.projectile[p].DamageType = ModContent.GetInstance<ClickerDamage>();

                            Vector2 perturbedSpeed2 = new Vector2(vector.X, vector.Y).RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1))) * 1f;
                        }
                    }
                }
                Timer2 = 0;
            }

            Timer++;
            if (Timer > 15)
            {
                int index = -1;
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC npc = Main.npc[i];
                    if (npc.CanBeChasedBy() && Projectile.DistanceSQ(npc.Center) < 400f * 400f && Collision.CanHit(Projectile.Center, 1, 1, npc.Center, 1, 1))
                    {
                        index = i;
                    }
                }
                if (index != -1)
                {
                    Vector2 vector = Main.npc[index].Center - Projectile.Center;
                    float speed = 3f;
                    float mag = vector.Length();
                    if (mag > speed)
                    {
                        mag = speed / mag;
                        vector *= mag;
                    }

                    if (Projectile.owner == Main.myPlayer)
                    {
                        float numberProjectiles = 3;
                        float rotation = MathHelper.ToRadians(20);
                        for (int i = 0; i < numberProjectiles; i++)
                        {
                            Vector2 perturbedSpeed = new Vector2(vector.X, vector.Y).RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1))) * 1f;
                            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, perturbedSpeed, ModContent.ProjectileType<TotalityClickerPro2>(), (int)(Projectile.damage * 0.25f), Projectile.knockBack, Projectile.owner);
                        }
                    }
                }
                Timer = 0;
            }
        }
    }
}
