using CalamityMod;
using CalamityMod.CalPlayer.Dashes;
using CalamityMod.Items;
using CalamityMod.Items.Armor.GodSlayer;
using CalamityMod.Items.Materials;
using CalamityMod.Particles;
using CalamityMod.Rarities;
using CalamityMod.Tiles.Furniture.CraftingStations;
using ClickerClass;
using ClickerClass.Items;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace CalamityClickers.Content.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class GodSlayerCapsuit : ClickerItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Armor.Capsuit";

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = CalamityGlobalItem.RarityDarkBlueBuyPrice;
            Item.defense = 34;
            Item.rare = ModContent.RarityType<CosmicPurple>();
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage<ClickerDamage>() += 0.14f;
            player.GetCritChance<ClickerDamage>() += 14;
            player.Clicker().clickerRadius += 0.9f;
            player.Clicker().clickerBonusPercent -= 0.1f;

        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<GodSlayerChestplate>() && legs.type == ModContent.ItemType<GodSlayerLeggings>();
        }

        public override void ArmorSetShadows(Player player)
        {
            player.armorEffectDrawShadow = true;
        }

        public override void UpdateArmorSet(Player player)
        {
            var hotkey2 = CalamityKeybinds.GodSlayerDashHotKey.TooltipHotkeyString();
            var hotkey = CalamityKeybinds.ArmorSetBonusHotKey.TooltipHotkeyString();
            player.setBonus = this.GetLocalization("SetBonus").Format(hotkey) + "\n" + CalamityUtils.GetTextFromModItem<GodSlayerChestplate>("CommonSetBonus").Format(hotkey2, GodslayerArmorDash.GodslayerCooldown);

            var modPlayer = player.Calamity();
            modPlayer.godSlayer = true;
            player.GetModPlayer<CalamityClickersPlayer>().setGodSlayerClicker = true;
            if (modPlayer.godSlayerDashHotKeyPressed || (player.dashDelay != 0 && modPlayer.LastUsedDashID == GodslayerArmorDash.ID))
            {
                modPlayer.DeferredDashID = GodslayerArmorDash.ID;
                player.dash = 0;
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<CosmiliteBar>(7).
                AddIngredient<AscendantSpiritEssence>(2).
                AddTile<CosmicAnvil>().
                Register();
        }
    }
    public class GodSlayerCapsuitBuff : ModBuff
    {
        //public override LocalizedText Description => base.Description.WithFormatArgs(OverclockHelmet.SetBonusEffectDecrease);

        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.CalClicker().godSlayerClickerBuff = true;
        }
        internal static void DrawEffects(PlayerDrawSet drawInfo)
        {
            Player player = drawInfo.drawPlayer;

            if (Main.GlobalTimeWrappedHourly % 10 == 0)
            {
                for (int i = 0; i < 10; i++)
                {
                    SparkParticle spark = new SparkParticle(player.Center, Main.rand.NextVector2CircularEdge(10, 10) * Main.rand.NextFloat(0.9f, 1.1f), false, 30, 1, Main.rand.NextBool() ? Color.Aqua : Color.Fuchsia);
                    GeneralParticleHandler.SpawnParticle(spark);
                }
            }
        }
    }
}
