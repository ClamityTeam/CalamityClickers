using CalamityMod.Items;
using CalamityMod.Items.Materials;
using CalamityMod.Rarities;
using CalamityMod.Tiles.Furniture.CraftingStations;
using ClickerClass;
using ClickerClass.Items;
using ClickerClass.Items.Accessories;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityClickers.Content.Items.Accessories
{
    [AutoloadEquip(EquipType.Waist)]
    public class DOG : ClickerItem, ILocalizedModType, IModType
    {
        public new string LocalizationCategory => "Items.Accessories";
        public override void SetDefaults()
        {
            SetDisplayTotalClicks(Item);
            Item.width = 20;
            Item.height = 20;
            Item.accessory = true;
            Item.value = CalamityGlobalItem.RarityDarkBlueBuyPrice;
            Item.rare = ModContent.RarityType<CosmicPurple>();
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            ClickerPlayer clickerPlayer = player.GetModPlayer<ClickerPlayer>();
            clickerPlayer.clickerRadius += 1f;
            player.GetDamage<ClickerDamage>() += 0.3f;
            clickerPlayer.clickerBonusPercent -= 0.2f;
            ClickerCompat.SetAutoReuseEffect(player, 6f, true);
            clickerPlayer.accAimbotModule = true;
            clickerPlayer.accAimbotModule2 = true;

        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<GamerCrate>()
                .AddIngredient<AimbotModule>()
                //.AddIngredient<IcePack>()
                .AddIngredient(ItemID.LunarBar, 8)
                .AddIngredient<GalacticaSingularity>(4)
                .AddIngredient<AscendantSpiritEssence>(4)
                .AddTile<CosmicAnvil>()
                .Register();
        }
    }
}
