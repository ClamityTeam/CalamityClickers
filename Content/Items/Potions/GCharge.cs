using CalamityMod;
using CalamityMod.Items.Placeables.SunkenSea;
using ClickerClass;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityClickers.Content.Items.Potions
{
    public class GCharge : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Potions";

        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 20;
            ItemID.Sets.DrinkParticleColors[Type] = new Color[3] {
                new Color(185, 245, 245),
                new Color(110, 223, 231),
                new Color(67, 187, 204)
            };
        }

        public override void SetDefaults()
        {
            Item.DefaultToFood(20, 32, ModContent.BuffType<GChargeBuff>(), CalamityUtils.MinutesToFrames(6), true);
            Item.value = Item.sellPrice(silver: 2);
            Item.rare = ItemRarityID.Blue;
        }

        public override void AddRecipes()
        {
            CreateRecipe(10).
                AddIngredient(ItemID.Bottle, 10).
                AddIngredient(ModContent.ItemType<SeaPrism>(), 3).
                AddTile(TileID.Kegs).
                Register();
        }

    }
    public class GChargeBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = false;
            Main.persistentBuff[Type] = true;
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
        }
        public static float ClickDamageBoostPerCPS => 0.01f;
        public static float RadiusNerf => 0.5f;
        public override void Update(Player player, ref int buffIndex)
        {
            player.CalClicker().gchargeBuff = true;
            player.GetDamage<ClickerDamage>() += ClickDamageBoostPerCPS * player.Clicker().clickerPerSecond;
            player.Clicker().clickerBonusPercent += 0.1f;
        }
        //20% increased click damage
        //Reduces your click radius by half
    }
}
