using CalamityMod.DataStructures;
using CalamityMod.Items;
using CalamityMod.Items.Accessories;
using CalamityMod.Systems.Collections;
using ClickerClass;
using ClickerClass.Items;
using ClickerClass.Items.Accessories;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityClickers.Content.Items.Accessories
{
    [AutoloadEquip(EquipType.HandsOn)]
    public class BeetleClickingGlove : ClickerItem, ILocalizedModType, IModType
    {
        public new string LocalizationCategory => "Items.Accessories";
        public override bool IsLoadingEnabled(Mod mod)
        {
            return false;
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
            player.GetModPlayer<ClickerPlayer>().accRegalClickingGlove = true;
            player.CalClicker().accBeetleClickingGlove = true;
            if (player.statLife < player.statLifeMax2 / 10 * 2)
                player.GetDamage<ClickerDamage>() += 0.25f;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<RegalClickingGlove>()
                .AddIngredient<NecklaceofVexation>()
                .AddIngredient(ItemID.BeetleHusk, 5)
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }
    }
    public class BeetleClickingGloveDebuffData : DebuffData
    {
        public BeetleClickingGloveDebuffData()
            : base()
        {
            NPCLifeRegenMethod = HydrothermicDebuffNPCMethod;
        }
        public void HydrothermicDebuffNPCMethod(NPC npc, int buffType, ref int buffIndex, ref int damage)
        {
            Player player = Main.player[npc.CalClicker().clickDebuffOwner];
            EnemyLostRegen = (int)(player.Clicker().clickerPerSecond * 5);
            BaseUpdateNPCLifeRegen(npc, buffType, ref buffIndex, ref damage);
        }
    }
    public class BeetleClickingGloveDebuff : ModBuff
    {
        public static DebuffData debuffData = new BeetleClickingGloveDebuffData();
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
            npc.CalClicker().clickDebuff = true;
        }
    }
}
