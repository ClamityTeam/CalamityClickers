using CalamityMod.Items;
using CalamityMod.Items.Materials;
using CalamityMod.Rarities;
using CalamityMod.Tiles.Furniture.CraftingStations;
using ClickerClass;
using ClickerClass.Items;
using Terraria;
using Terraria.ModLoader;

namespace CalamityClickers.Content.Items.Accessories
{
    [AutoloadEquip(EquipType.HandsOn)]
    public class CosmicClickingGlove : ClickerItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Accessories";
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.accessory = true;
            Item.value = CalamityGlobalItem.RarityDarkBlueBuyPrice;
            Item.rare = ModContent.RarityType<CosmicPurple>();
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.Clicker().accRegalClickingGlove = true;
            player.GetCritChance<ClickerDamage>() += 15;
            player.GetModPlayer<CalamityClickersPlayer>().accPortableParticleAcceleratorUpgrades = Item;
            player.GetModPlayer<CalamityClickersPlayer>().accPortableParticleAcceleratorUpgradesPower += 100;
            player.Clicker().accTriggerFinger = true;
            player.GetModPlayer<CalamityClickersPlayer>().accFingerOfBG = true;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<LunarClickingGlove>()
                .AddIngredient<FingerOfBloodGod>()
                .AddIngredient<CosmiliteBar>(8)
                .AddIngredient<AscendantSpiritEssence>(4)
                .AddTile<CosmicAnvil>()
                .Register();
        }
    }
}
