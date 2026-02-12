using CalamityMod;
using CalamityMod.CalPlayer.Dashes;
using CalamityMod.Items;
using CalamityMod.Items.Armor.Auric;
using CalamityMod.Items.Materials;
using CalamityMod.Rarities;
using CalamityMod.Tiles.Furniture.CraftingStations;
using ClickerClass;
using ClickerClass.Items;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityClickers.Content.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class AuricTeslaPlusRadarCapsuit : ClickerItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Armor.Capsuit";
        public override bool IsLoadingEnabled(Mod mod)
        {
            return ModLoader.HasMod("CatalystMod");
        }
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
            player.statDefense += 6;
            player.GetDamage<ClickerDamage>() += 0.3f;
            player.GetCritChance<ClickerDamage>() += 30;
            player.Clicker().clickerBonusPercent -= 0.1f;
            player.Clicker().clickerRadius += 1f;
        }
        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            Mod catalyst = ModLoader.GetMod("CatalystMod");
            Player player = Main.player[Main.myPlayer];
            /*return (body.type == ModContent.ItemType<AuricTeslaBodyArmor>() && player.body == EquipLoader.GetEquipSlot(catalyst, "AuricBody", EquipType.Body))
                && (legs.type == ModContent.ItemType<AuricTeslaCuisses>() && player.legs == EquipLoader.GetEquipSlot(catalyst, "AuricLegs", EquipType.Legs));*/

            return (body.type == ModContent.ItemType<AuricTeslaBodyArmor>())
                && (legs.type == ModContent.ItemType<AuricTeslaCuisses>());
        }
        public override void UpdateArmorSet(Player player)
        {
            //player.setBonus = "Clicker Tarragon, Bloodflare, God Slayer, and Silva armor effects\n30% increased clicker damage and crit";
            player.setBonus = ILocalizedModTypeExtensions.GetLocalizedValue(this, "SetBonus");

            //ClickerCompat.SetDamageAdd(player, 0.30f);
            //ClickerCompat.SetClickerCritAdd(player, 30);
            //player.GetDamage<ClickerDamage>() += 0.3f;
            //player.GetCritChance<ClickerDamage>() += 30;

            //Intergelcatic
            player.CalClicker().setIntergelacticClicker = Item;

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
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            string text = Colors.AlphaDarken(new Color(112, 244, 244, 255)).Hex3();
            for (int i = 0; i < tooltips.Count; i++)
            {
                if (!(tooltips[i].Mod == "Terraria"))
                {
                    continue;
                }

                if (tooltips[i].Name == "ItemName")
                {
                    string value2;
                    string text2 = (value2 = Lang.GetItemName(Item.type).Value);
                    value2 = value2 + "[c/" + text + ":+]";
                    for (int j = text2.Length; j < tooltips[i].Text.Length; j++)
                    {
                        value2 += tooltips[i].Text[j];
                    }

                    tooltips[i].Text = value2;
                    if (Item.social)
                    {
                        break;
                    }
                }
                else if (tooltips[i].Name == "Defense")
                {
                    tooltips[i].Text = Item.defense + "[c/" + text + ":(+" + 6 + ")]" + Lang.tip[25];
                }
            }
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<IntergelacticCapsuit>()
                .AddIngredient<BloodflareCrimeraCapsuit>()
                .AddIngredient<GodSlayerCapsuit>()
                .AddIngredient<AuricBar>(12)
                .AddTile<CosmicAnvil>()
                .Register();
        }

    }
}
