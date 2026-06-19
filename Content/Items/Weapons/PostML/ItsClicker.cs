using CalamityClickers.Content.Items.Weapons.HM;
using CalamityMod;
using CalamityMod.DataStructures;
using CalamityMod.Items;
using CalamityMod.Items.Materials;
using CalamityMod.Particles;
using CalamityMod.Rarities;
using CalamityMod.Systems.Collections;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityClickers.Content.Items.Weapons.PostML
{
    public class ItsClicker : ModdedClickerWeapon
    {
        public static string ItzClick { get; internal set; } = string.Empty;
        public override float Radius => 8;
        public override Color RadiusColor => Color.LightGray;
        //public override bool SetBorderTexture => true;
        public override void SetStaticDefaultsExtra()
        {
            ItzClick = ClickerCompat.RegisterClickEffect(Mod, "ItzClick", 1, RadiusColor, delegate (Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, int type, int damage, float knockBack)
            {
                NPC npc = CalamityUtils.ClosestNPCAt(position, 500, true, true);
                if (npc != null)
                    npc.AddBuff(ModContent.BuffType<ItsClickerDebuff>(), 30);
            });
            CalamityClickersUtils.RegisterPostWildMagicClickEffect(ItzClick);
            CalamityClickersUtils.RegisterPostNightmareMagicClickEffect(ItzClick);
        }
        public override void SetDefaultsExtra()
        {
            AddEffect(Item, ItzClick);
            SetDust(Item, DustID.Obsidian);

            Item.damage = 600;
            Item.knockBack = 1f;
            Item.rare = ModContent.RarityType<HotPink>();
            Item.value = CalamityGlobalItem.RarityHotPinkBuyPrice;

            Item.Calamity().devItem = true;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<MeldClicker>()
                .AddIngredient(ItemID.BlackPaint, 10)
                .AddIngredient<ShadowspecBar>(5)
                .AddTile(ModContent.TileType<DraedonsForge>())
                .Register();
        }
    }
    public class ItsClickerDebuffData : DebuffData
    {
        public ItsClickerDebuffData()
            : base()
        {
            NPCLifeRegenMethod = ItsClickerDebuffNPCMethod;
        }
        public void ItsClickerDebuffNPCMethod(NPC npc, int buffType, ref int buffIndex, ref int damage)
        {
            EnemyLostRegen = npc.lifeMax / 100;
            BaseUpdateNPCLifeRegen(npc, buffType, ref buffIndex, ref damage);
        }
    }
    public class ItsClickerDebuff : ModBuff
    {
        public static DebuffData debuffData = new ItsClickerDebuffData();
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
            BuffID.Sets.LongerExpertDebuff[Type] = true;
            CalamityBuffSets.DebuffDataset[Type] = debuffData;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.CalClicker().wither = true;
        }

        internal static void DrawEffects(NPC npc, ref Color drawColor)
        {
            if (Main.rand.NextBool())
            {
                Vector2 npcSize = npc.Center + new Vector2(Main.rand.NextFloat(-npc.width / 2, npc.width / 2), Main.rand.NextFloat(-npc.height / 2, npc.height / 2));
                SparkParticle spark = new SparkParticle(npcSize, Main.rand.NextVector2Square(-2.5f, 2.5f), false, Main.rand.Next(11, 13), Main.rand.NextFloat(0.2f, 0.5f), Color.Gray);
                GeneralParticleHandler.SpawnParticle(spark);
            }
        }
    }
}
