using CalamityMod;
using CalamityMod.CustomRecipes;
using CalamityMod.Items;
using CalamityMod.Items.Materials;
using CalamityMod.Rarities;
using ClickerClass;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;
using static CalamityMod.CalamityUtils;

namespace CalamityClickers.Content.Items.Weapons.DraedonArsenal
{
    public class Bioinjector : ModdedClickerWeapon
    {
        public static string VitalInfusion { get; internal set; } = string.Empty;
        public override float Radius => 6f;
        public override Color RadiusColor => CalamityClickersUtils.GetColorFromHex("C2FEF4");
        public override void SetStaticDefaultsExtra()
        {
            VitalInfusion = ClickerSystem.RegisterClickEffect(Mod, "VitalInfusion", 20, RadiusColor, delegate (Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, int type, int damage, float knockBack)
            {
                int pro = ModContent.ProjectileType<BioinjectorPro>();
                if (player.ownedProjectileCounts[pro] < 3)
                    Projectile.NewProjectile(source, position, Vector2.Zero, pro, 0, 0, player.whoAmI);
            });
        }
        public override void SetDefaultsExtra()
        {
            AddEffect(Item, VitalInfusion);
            SetDust(Item, DustID.Electric);

            Item.damage = 220;
            Item.knockBack = 1.5f;
            Item.rare = ModContent.RarityType<Turquoise>();
            Item.value = CalamityGlobalItem.RarityTurquoiseBuyPrice;
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips) => CalamityGlobalItem.InsertKnowledgeTooltip(tooltips, 4);
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<MysteriousCircuitry>(17).
                AddIngredient<DubiousPlating>(13).
                AddIngredient<UelibloomBar>(8).
                AddIngredient(ItemID.LunarBar, 4).
                AddCondition(ArsenalTierGatedRecipe.ConstructRecipeCondition(4, out Func<bool> condition), condition).
                AddTile(TileID.LunarCraftingStation).
                Register();
            if (ModLoader.TryGetMod("CatalystMod", out var mod))
            {
                CreateRecipe().
                    AddIngredient<MysteriousCircuitry>(17).
                    AddIngredient<DubiousPlating>(13).
                    AddIngredient(mod.Find<ModItem>("MetanovaBar").Type, 4).
                    AddIngredient(ItemID.LunarBar, 4).
                    AddCondition(ArsenalTierGatedRecipe.ConstructRecipeCondition(4, out Func<bool> condition2), condition2).
                    AddTile(TileID.LunarCraftingStation).
                    Register();

            }
        }
    }
    public class BioinjectorPro : ModdedClickerProjectile
    {
        public override bool UseInvisibleProjectile => false;
        public override void SetDefaultsExtra()
        {
            Projectile.width = Projectile.height = 16;
            Projectile.aiStyle = -1;
            AIType = -1;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 1800;
            Projectile.friendly = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;
        }
        public override bool? CanDamage()
        {
            return false;
        }
        /*public int BioNeedleType
        {
            get => (int)Projectile.ai[0];
        }*/
        public Vector2 StartSpot;
        public const float Time = 40;
        public const float ActiveToHealTime = 10;
        public CurveSegment InitialAway = new CurveSegment(SineOutEasing, 0f, 0f, -0.2f, 3);
        public CurveSegment AccelerateTowards = new CurveSegment(PolyInEasing, 0.3f, -0.2f, 1.2f, 3);
        public CurveSegment Bump1Segment = new CurveSegment(SineBumpEasing, 0.5f, 1f, 0.24f);
        public CurveSegment Bump2Segment = new CurveSegment(SineBumpEasing, 0.8f, 1f, -0.1f);
        internal float ProgressionOfNeedle => PiecewiseAnimation(Timer / Time, new CurveSegment[] { InitialAway, AccelerateTowards, Bump1Segment, Bump2Segment });
        public override void OnSpawn(IEntitySource source)
        {
            StartSpot = Projectile.Center;
        }
        public int Timer
        {
            get => (int)Projectile.localAI[1];
            set => Projectile.localAI[1] = value;
        }
        public bool ActiveToHeal
        {
            get => Projectile.ai[0] > 0;
        }
        public int ActiveToHealTimer
        {
            get => (int)Projectile.ai[1];
            set => Projectile.ai[1] = value;
        }
        public override void AI()
        {
            Player player = Projectile.Owner();

            if (player.dead || player.ghost)
            {
                Projectile.Kill();
            }

            Projectile.timeLeft = 1800;
            if (Timer > 0)
                Timer--;
            if (ActiveToHeal && ActiveToHealTimer < ActiveToHealTime)
                ActiveToHealTimer++;
            if (ActiveToHealTimer == ActiveToHealTime)
                Projectile.Kill();



            Vector2 realIdealSpot = player.MountedCenter + player.gfxOffY * Vector2.UnitY;
            Projectile.netUpdate = true;
            int niddlePosition = -1;
            for (int index = 0; index < Projectile.whoAmI; ++index)
            {
                Projectile proj = Main.projectile[index];

                // Short circuits to make the loop as fast as possible
                if (!proj.active || proj.owner != Projectile.owner)
                    continue;

                if (proj.type == Projectile.type)
                    ++niddlePosition;
            }
            float rot = niddlePosition * 0.4f;
            realIdealSpot -= new Vector2(0, 70 + 30 * MathF.Sin(new UnifiedRandom(Projectile.whoAmI).NextFloat(0.9f, 1.1f) * Main.GlobalTimeWrappedHourly)).RotatedBy(rot);
            Projectile.rotation = rot;

            Projectile.Center = Vector2.Lerp(Vector2.Lerp(realIdealSpot, StartSpot, ProgressionOfNeedle), player.Center, ActiveToHealTimer / ActiveToHealTime);
        }
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(StartSpot.X);
            writer.Write(StartSpot.Y);
            writer.Write(Timer);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            StartSpot = new Vector2(reader.ReadSingle(), reader.ReadSingle());
            Timer = reader.ReadInt32();
        }
    }
}
