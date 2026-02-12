using CalamityMod.Items;
using CalamityMod.Items.Materials;
using ClickerClass;
using ClickerClass.Items;
using ClickerClass.Items.Accessories;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityClickers.Content.Items.Accessories
{
    public class LihzahrdParticleAccelerator : ClickerItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Accessories";
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
            player.GetCritChance<ClickerDamage>() += 5;
            player.GetModPlayer<CalamityClickersPlayer>().accPortableParticleAcceleratorUpgrades = Item;
            player.GetModPlayer<CalamityClickersPlayer>().accPortableParticleAcceleratorUpgradesPower += 10;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<PortableParticleAccelerator>()
                .AddIngredient(ItemID.EyeoftheGolem)
                .AddIngredient<CoreofCalamity>()
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}
