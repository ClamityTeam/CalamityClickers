using CalamityClickers.Content.Items.Weapons.HM;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Items;
using CalamityMod.Items.Materials;
using CalamityMod.Particles;
using ClickerClass;
using ClickerClass.Items;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityClickers.Content.Items.Weapons.PostML
{
    public class ElementalClicker : ModdedClickerWeapon
    {
        public static string ElementalAura { get; internal set; } = string.Empty;
        public override float Radius => 6f;
        //public override Color RadiusColor => Color.White;
        public override Color RadiusColor
        {
            get
            {
                float a = Main.GlobalTimeWrappedHourly % 4;
                if (a < 1)
                    return Color.Lerp(new Color(255, 215, 51), new Color(85, 237, 194), a);
                if (a >= 1 && a < 2)
                    return Color.Lerp(new Color(85, 237, 194), new Color(255, 138, 232), a - 1);
                if (a >= 2 && a < 3)
                    return Color.Lerp(new Color(255, 138, 232), new Color(108, 199, 235), a - 2);
                else
                    return Color.Lerp(new Color(108, 199, 235), new Color(255, 215, 51), a - 3);
            }
        }
        public override void SetStaticDefaultsExtra()
        {
            ElementalAura = ClickerCompat.RegisterClickEffect(Mod, "ElementalAura", 10, () => RadiusColor, delegate (Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, int type, int damage, float knockBack)
            {
                Projectile.NewProjectile(source, position, Vector2.Zero, ModContent.ProjectileType<ElementalClickerProjectile>(), damage * 2, knockBack, player.whoAmI, Main.rand.Next(4));
            });
            CalamityClickersUtils.RegisterPostWildMagicClickEffect(ElementalAura);
        }
        public override void SetDefaultsExtra()
        {
            AddEffect(Item, ElementalAura);
            //SetColor(Item, color());

            Item.damage = 110;
            Item.knockBack = 1f;
            Item.rare = ItemRarityID.Red;
            Item.value = CalamityGlobalItem.RarityRedBuyPrice;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<FloraClicker>()
                .AddIngredient(ItemID.LunarBar, 5)
                .AddIngredient<LifeAlloy>(5)
                .AddIngredient<MiceFragment>(5)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }
    }
    public class ElementalClickerProjectile : ModdedClickerProjectile
    {
        public override void SetDefaultsExtra()
        {
            Projectile.width = 375;
            Projectile.height = 375;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 120;
            Projectile.alpha = 255;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;
            Projectile.DamageType = ModContent.GetInstance<ClickerDamage>();
        }
        public int ShinkGrow = 0;
        public int Framecounter = 0;
        public int PulseOnce = 1;
        public int PulseOnce2 = 1;
        public int PulseOnce3 = 1;
        public override void AI()
        {
            Framecounter++;

            Color color = Color.White;
            switch ((int)Projectile.ai[0])
            {
                case 0:
                    color = new Color(255, 215, 51);
                    break;
                case 1:
                    color = new Color(85, 237, 194);
                    break;
                case 2:
                    color = new Color(255, 138, 232);
                    break;
                case 3:
                    color = new Color(76, 138, 273);
                    break;
            }

            if (ShinkGrow == 0)
            {
                if (PulseOnce == 1)
                {
                    Particle pulse = new StaticPulseRing(Projectile.Center, Vector2.Zero, color, new Vector2(1f, 1f), 0f, 0f, 0.228f, 10);
                    GeneralParticleHandler.SpawnParticle(pulse);
                    //SoundEngine.PlaySound(Spawnsound with { Pitch = -0.9f }, Projectile.Center);
                    PulseOnce = 0;
                }

                if (Framecounter == 10)
                {
                    ShinkGrow = 1;
                }
            }
            if (ShinkGrow == 1)
            {
                if (PulseOnce2 == 1)
                {
                    Particle pulse2 = new StaticPulseRing(Projectile.Center, Vector2.Zero, color, new Vector2(1f, 1f), 0f, 0.228f, 0.228f, 100);
                    GeneralParticleHandler.SpawnParticle(pulse2);
                    PulseOnce2 = 0;
                }

                if (Framecounter == 110)
                {
                    ShinkGrow = 2;
                }
            }
            if (ShinkGrow == 2)
            {
                if (PulseOnce3 == 1)
                {
                    Particle pulse3 = new StaticPulseRing(Projectile.Center, Vector2.Zero, color, new Vector2(1f, 1f), 0f, 0.228f, 0f, 10);
                    GeneralParticleHandler.SpawnParticle(pulse3);
                    PulseOnce3 = 0;
                }
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<ElementalMix>(), 60);
        }
    }
}
