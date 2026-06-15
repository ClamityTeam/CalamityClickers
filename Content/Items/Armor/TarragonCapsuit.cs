using CalamityMod;
using CalamityMod.Items;
using CalamityMod.Items.Armor.Tarragon;
using CalamityMod.Items.Materials;
using CalamityMod.Rarities;
using ClickerClass;
using ClickerClass.Items;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityClickers.Content.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class TarragonCapsuit : ClickerItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Armor.Capsuit";

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = CalamityGlobalItem.RarityTurquoiseBuyPrice;
            Item.defense = 24;
            Item.rare = ModContent.RarityType<Turquoise>();
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage<ClickerDamage>() += 0.1f;
            player.GetCritChance<ClickerDamage>() += 5;
            player.Clicker().clickerRadius += 0.5f;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<TarragonBreastplate>() && legs.type == ModContent.ItemType<TarragonLeggings>();
        }

        public override void ArmorSetShadows(Player player)
        {
            player.armorEffectDrawShadowSubtle = true;
            player.armorEffectDrawOutlines = true;
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = this.GetLocalizedValue("SetBonus");

            var modPlayer = player.Calamity();
            modPlayer.tarraSet = true;
            player.GetCritChance<ClickerDamage>() += 10;
            player.CalClicker().setTarragonClicker = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<UelibloomBar>(7).
                AddIngredient<DivineGeode>(6).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
