using CalamityMod;
using CalamityMod.Items;
using CalamityMod.Items.Armor.Bloodflare;
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
    public class BloodflareCrimeraCapsuit : ClickerItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Armor.Capsuit";
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = CalamityGlobalItem.RarityPureGreenBuyPrice;
            Item.defense = 16;
            Item.rare = ModContent.RarityType<PureGreen>();
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage<ClickerDamage>() += 0.1f;
            player.GetCritChance<ClickerDamage>() += 10;
            player.Clicker().clickerRadius += 0.7f;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<BloodflareBodyArmor>() && legs.type == ModContent.ItemType<BloodflareCuisses>();
        }

        public override void ArmorSetShadows(Player player)
        {
            player.armorEffectDrawShadowSubtle = true;
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = this.GetLocalizedValue("SetBonus");

            var modPlayer = player.Calamity();
            modPlayer.bloodflareSet = true;
            player.GetModPlayer<CalamityClickersPlayer>().setBloodflareClicker = true;
            player.crimsonRegen = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<BloodstoneCore>(11).
                AddIngredient<RuinousSoul>(2).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
