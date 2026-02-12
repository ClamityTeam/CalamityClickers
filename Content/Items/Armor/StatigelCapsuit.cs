using CalamityMod;
using CalamityMod.ExtraJumps;
using CalamityMod.Items;
using CalamityMod.Items.Armor.Statigel;
using CalamityMod.Items.Materials;
using CalamityMod.Tiles.Furniture.CraftingStations;
using ClickerClass;
using ClickerClass.Items;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityClickers.Content.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class StatigelCapsuit : ClickerItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Armor.Capsuit";

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 22;
            Item.value = CalamityGlobalItem.RarityLightRedBuyPrice;
            Item.rare = ItemRarityID.LightRed;
            Item.defense = 5;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage<ClickerDamage>() += 0.1f;
            player.GetCritChance<ClickerDamage>() += 7;
            player.Clicker().clickerRadius += 0.5f;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<StatigelArmor>() && legs.type == ModContent.ItemType<StatigelGreaves>();
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = ILocalizedModTypeExtensions.GetLocalizedValue(this, "SetBonus");

            player.Calamity().statigelSet = true;
            player.GetJumpState<StatigelJump>().Enable();
            Player.jumpHeight += 5;
            player.jumpSpeedBoost += 0.6f;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<PurifiedGel>(5).
                AddIngredient<BlightedGel>(5).
                AddTile(TileID.Solidifier).
                Register();
        }
    }
}
