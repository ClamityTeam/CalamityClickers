using CalamityMod;
using CalamityMod.CustomRecipes;
using CalamityMod.Items;
using CalamityMod.Items.Materials;
using CalamityMod.Particles;
using ClickerClass;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityClickers.Content.Items.Weapons.DraedonArsenal
{
    public class HandheldClickerDrill : ModdedClickerWeapon
    {
        public static string DrillShot { get; internal set; } = string.Empty;
        public override float Radius => 3.4f;
        public override Color RadiusColor => CalamityClickersUtils.GetColorFromHex("C929FF");
        public override void SetStaticDefaultsExtra()
        {
            DrillShot = ClickerSystem.RegisterClickEffect(Mod, "DrillShot", 10, RadiusColor, delegate (Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, int type, int damage, float knockBack)
            {
                Vector2 pos = position;

                NPC npc = CalamityUtils.ClosestNPCAt(pos, 1000, false, true);
                if (npc != null)
                {
                    Vector2 vector = npc.Center - pos;
                    float speed = 1f;
                    float mag = vector.Length();
                    if (mag > speed)
                    {
                        mag = speed / mag;
                        vector *= mag;
                    }
                    SoundEngine.PlaySound(SoundID.Item22, pos);
                    Projectile.NewProjectile(source, pos, vector, ModContent.ProjectileType<HandheldClickerDrillPro>(), damage, knockBack, player.whoAmI);
                }
            });
        }
        public override void SetDefaultsExtra()
        {
            AddEffect(Item, DrillShot);
            SetDust(Item, DustID.Electric);

            Item.damage = 35;
            Item.knockBack = 1.5f;
            Item.rare = ItemRarityID.Pink;
            Item.value = CalamityGlobalItem.RarityPinkBuyPrice;
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips) => CalamityGlobalItem.InsertKnowledgeTooltip(tooltips, 2);
        /*         
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            int index = -1;
            index = tooltips.FindLastIndex(tt => tt.Mod.Equals("Terraria") && tt.Name.Equals("Tooltip0"));
            if (index != -1)
            {
                tooltips.Insert(index, new TooltipLine(Mod, "ChemistClass:ChemicalExtraDamageTooltip", LangHelper.GetText("Static.ChemicalExtraDamageTooltip")));
            }
            CalamityGlobalItem.InsertKnowledgeTooltip(tooltips, 2);
        }
         */
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<MysteriousCircuitry>(12).
                AddIngredient<DubiousPlating>(8).
                AddRecipeGroup("AnyMythrilBar", 10).
                AddIngredient(ItemID.SoulofMight, 20).
                AddCondition(ArsenalTierGatedRecipe.ConstructRecipeCondition(2, out Func<bool> condition), condition).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
    public class HandheldClickerDrillPro : ModdedClickerProjectile
    {
        public override bool UseInvisibleProjectile => false;
        public override void SetDefaultsExtra()
        {
            Projectile.width = Projectile.height = 20;
            Projectile.aiStyle = -1;
            AIType = -1;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 400;
            Projectile.friendly = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 20;
        }
        public override void OnSpawn(IEntitySource source)
        {
            Projectile.ai[0] = -1;
        }
        public override void AI()
        {
            if (Projectile.ai[0] == -1)
            {
                if (Math.Abs(Math.Sqrt(Math.Pow(Projectile.velocity.X, 2) + Math.Pow(Projectile.velocity.Y, 2))) < 15f)
                {
                    Projectile.velocity *= 1.035f;
                }
                Projectile.rotation = Projectile.velocity.ToRotation();
            }
            else
            {
                Projectile.Center = Main.npc[(int)Projectile.ai[0]].Center - new Vector2(Projectile.ai[1], Projectile.ai[2]);
            }
            if (Projectile.soundDelay <= 0)
            {
                //Projectile.soundDelay--;
                if (Projectile.soundDelay == 0)
                {
                    SoundEngine.PlaySound(SoundID.Item23, Projectile.Center);
                    Projectile.soundDelay = 30;
                }
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Projectile.ai[0] == -1)
            {
                Projectile.ai[0] = target.whoAmI;
                Vector2 v = target.Center - Projectile.Center;
                Projectile.ai[1] = v.X;
                Projectile.ai[2] = v.Y;
                Projectile.rotation = Projectile.velocity.ToRotation();
                Projectile.velocity = Vector2.Zero;
            }
            for (int i = 0; i < 2; i++)
            {
                SparkParticle spark = new SparkParticle(Projectile.Center, Vector2.UnitX.SafeNormalize(Vector2.Zero).RotatedBy(Projectile.rotation).RotatedByRandom(0.3f) * 5 * Main.rand.NextFloat(0.8f, 1.2f), true, 20, 0.5f, Color.Yellow);
                GeneralParticleHandler.SpawnParticle(spark);
            }
        }
    }
}
