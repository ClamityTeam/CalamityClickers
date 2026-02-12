using CalamityMod;
using CalamityMod.CalPlayer.Dashes;
using CalamityMod.Items;
using CalamityMod.Items.Armor.Auric;
using CalamityMod.Items.Materials;
using CalamityMod.Rarities;
using CalamityMod.Tiles.Furniture.CraftingStations;
using ClickerClass;
using ClickerClass.Items;
using Terraria;
using Terraria.ModLoader;

namespace CalamityClickers.Content.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class AuricTeslaRadarCapsuit : ClickerItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Armor.Capsuit";

        public override void SetDefaults()
        {
            Item.width = 27;
            Item.height = 22;
            Item.value = CalamityGlobalItem.RarityVioletBuyPrice;
            Item.rare = ModContent.RarityType<BurnishedAuric>();
            Item.defense = 38;
        }

        public override void UpdateEquip(Player player)
        {
            //ClickerCompat.SetDamageAdd(player, 0.13f);
            //ClickerCompat.SetClickerCritAdd(player, 13);
            //ClickerCompat.SetClickerRadiusAdd(player, WitherTacoLib.Math.Radius(0.5f));
            //ClickerCompat.SetClickerBonusPercentAdd(player, 0.1f);

            player.GetDamage<ClickerDamage>() += 0.3f;
            player.GetCritChance<ClickerDamage>() += 30;
            player.Clicker().clickerBonusPercent -= 0.1f;
            player.Clicker().clickerRadius += 1f;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            Mod calamity = ModLoader.GetMod("CalamityMod");
            return body.type == ModContent.ItemType<AuricTeslaBodyArmor>() && legs.type == ModContent.ItemType<AuricTeslaCuisses>();
        }

        public override void UpdateArmorSet(Player player)
        {
            //player.setBonus = "Clicker Tarragon, Bloodflare, God Slayer, and Silva armor effects\n30% increased clicker damage and crit";
            player.setBonus = ILocalizedModTypeExtensions.GetLocalizedValue(this, "SetBonus");

            //ClickerCompat.SetDamageAdd(player, 0.30f);
            //ClickerCompat.SetClickerCritAdd(player, 30);
            //player.GetDamage<ClickerDamage>() += 0.3f;
            //player.GetCritChance<ClickerDamage>() += 30;

            //Tarragon
            player.Calamity().tarraSet = true;
            //player.GetCritChance<ClickerDamage>() += 10;
            player.GetModPlayer<CalamityClickersPlayer>().setTarragonClicker = true;

            //Bloodflare
            player.Calamity().bloodflareSet = true;
            player.crimsonRegen = true;
            player.GetModPlayer<CalamityClickersPlayer>().setBloodflareClicker = true;

            //God Slayer
            player.Calamity().godSlayer = true;
            player.GetModPlayer<CalamityClickersPlayer>().setGodSlayerClicker = true;
            if (player.Calamity().godSlayerDashHotKeyPressed || (player.dashDelay != 0 && player.Calamity().LastUsedDashID == GodslayerArmorDash.ID))
            {
                player.Calamity().DeferredDashID = GodslayerArmorDash.ID;
                //player.dash = 0;
            }

            //Auric
            player.Calamity().auricSet = true;
            player.thorns += 3f;
        }

        public override void ArmorSetShadows(Player player)
        {
            player.armorEffectDrawOutlines = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<TarragonCapsuit>()
                .AddIngredient<BloodflareCrimeraCapsuit>()
                .AddIngredient<GodSlayerCapsuit>()
                .AddIngredient<AuricBar>(12)
                .AddTile<CosmicAnvil>()
                .Register();
        }
    }
}