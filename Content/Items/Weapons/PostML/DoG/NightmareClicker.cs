using CalamityMod.Items;
using CalamityMod.Items.Materials;
using CalamityMod.Rarities;
using CalamityMod.Tiles.Furniture.CraftingStations;
using ClickerClass;
using ClickerClass.Items.Weapons.Clickers;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityClickers.Content.Items.Weapons.PostML.DoG
{
    public class NightmareClicker : ModdedClickerWeapon
    {
        public static string NightmareMagic { get; internal set; } = string.Empty;
        public override float Radius => 8f;
        public override Color RadiusColor => new Color(225, 165, 53);
        public override bool SetBorderTexture => true;
        public override void SetStaticDefaultsExtra()
        {
            NightmareMagic = ClickerCompat.RegisterClickEffect(Mod, "NightmareMagic", 6, RadiusColor, delegate (Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, int type, int damage, float knockBack)
            {
                List<ClickEffect> allowed = new List<ClickEffect>();

                foreach (var name in ClickerSystem.GetAllEffectNames())
                {
                    if (!(CalamityClickers.extraAPI.Call("GetBlacklistedRandomClickerEffectList") as List<string>).Contains(name) && !CalamityClickersSystem.PostNightmareClickerEffects.Contains(name) && ClickerSystem.IsClickEffect(name, out ClickEffect effect))
                    {
                        allowed.Add(effect);
                    }
                }

                if (allowed.Count == 0) return;

                ClickEffect random = Main.rand.Next(allowed);
                random.Action?.Invoke(player, source, position, type, damage, knockBack);
            });
            CalamityClickersUtils.RegisterBlacklistedClickEffect(NightmareMagic);
        }
        public override void SetDefaultsExtra()
        {
            AddEffect(Item, NightmareMagic);
            SetDust(Item, DustID.SolarFlare);

            Item.damage = 300;
            Item.knockBack = 1f;
            Item.rare = ModContent.RarityType<CosmicPurple>();
            Item.value = CalamityGlobalItem.RarityDarkBlueBuyPrice;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<WitchClicker>()
                .AddIngredient<BurningSuperDeathClicker>()
                .AddIngredient<CosmiliteBar>(8)
                .AddIngredient<NightmareFuel>(8)
                .AddTile<CosmicAnvil>()
                .Register();
        }
    }
}
