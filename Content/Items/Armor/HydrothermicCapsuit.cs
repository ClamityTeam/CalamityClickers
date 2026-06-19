using CalamityMod;
using CalamityMod.DataStructures;
using CalamityMod.Items.Armor.Hydrothermic;
using CalamityMod.Items.Materials;
using CalamityMod.Systems.Collections;
using ClickerClass;
using ClickerClass.Items;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityClickers.Content.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class HydrothermicCapsuit : ClickerItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Armor.Capsuit";

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 22;
            Item.value = Item.buyPrice(gold: 30);
            Item.rare = ItemRarityID.Yellow;
            Item.defense = 27;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage<ClickerDamage>() += 0.12f;
            player.GetCritChance<ClickerDamage>() += 10;
            player.Clicker().clickerRadius += 0.5f;
            player.lavaImmune = true;
            player.buffImmune[BuffID.OnFire] = true;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<HydrothermicArmor>() && legs.type == ModContent.ItemType<HydrothermicSubligar>();
        }

        public override void ArmorSetShadows(Player player)
        {
            player.armorEffectDrawOutlines = true;
            player.Calamity().hydrothermalSmoke = true;
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = this.GetLocalization("SetBonus").Format(HydrothermicArmor.InfernoHealthThreshold.ToPercent());
            player.Calamity().ataxiaBlaze = true;
            player.GetModPlayer<CalamityClickersPlayer>().setHydrothermicClicker = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<ScoriaBar>(7).
                AddIngredient<EssenceofHavoc>(3).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
    public class HydrothermicCapsuitDebuffData : DebuffData
    {
        public HydrothermicCapsuitDebuffData()
            : base()
        {
            HeatDebuffScaling = 1;
            NPCLifeRegenMethod = HydrothermicDebuffNPCMethod;
        }
        public void HydrothermicDebuffNPCMethod(NPC npc, int buffType, ref int buffIndex, ref int damage)
        {
            EnemyLostRegen = npc.CalClicker().hydrothermicBoilPower;
            BaseUpdateNPCLifeRegen(npc, buffType, ref buffIndex, ref damage);
        }
    }
    public class HydrothermicCapsuitDebuff : ModBuff
    {
        public static DebuffData debuffData = new HydrothermicCapsuitDebuffData();
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
            //BuffID.Sets.LongerExpertDebuff[Type] = true;
            CalamityBuffSets.DebuffDataset[Type] = debuffData;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.CalClicker().hydrothermicBoil = true;
            npc.CalClicker().hydrothermicBoilTime = npc.buffTime[buffIndex];
        }

    }
}
