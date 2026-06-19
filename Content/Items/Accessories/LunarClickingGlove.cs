using CalamityMod.Items;
using ClickerClass;
using ClickerClass.Items;
using ClickerClass.Items.Accessories;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityClickers.Content.Items.Accessories
{
    [AutoloadEquip(EquipType.HandsOn)]
    public class LunarClickingGlove : ClickerItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Accessories";
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.accessory = true;
            Item.value = CalamityGlobalItem.RarityRedBuyPrice;
            Item.rare = ItemRarityID.Red;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.Clicker().accRegalClickingGlove = true;
            player.GetCritChance<ClickerDamage>() += 10;
            player.GetModPlayer<CalamityClickersPlayer>().accPortableParticleAcceleratorUpgrades = Item;
            player.GetModPlayer<CalamityClickersPlayer>().accPortableParticleAcceleratorUpgradesPower += 25;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<RegalClickingGlove>()
                .AddIngredient<LihzahrdParticleAccelerator>()
                .AddIngredient(ItemID.LunarBar, 5)
                .AddIngredient<MiceFragment>(5)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }
    }
}
