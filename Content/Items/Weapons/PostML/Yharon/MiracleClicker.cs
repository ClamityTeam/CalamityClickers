using CalamityClickers.Content.Items.Weapons.PostML.Polterghast;
using CalamityClickers.Content.Items.Weapons.PostML.Providance;
using CalamityMod.Items;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Weapons.Rogue;
using CalamityMod.Projectiles.Rogue;
using CalamityMod.Rarities;
using CalamityMod.Tiles.Furniture.CraftingStations;
using ClickerClass;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityClickers.Content.Items.Weapons.PostML.Yharon
{
    public class MiracleClicker : ModdedClickerWeapon
    {
        public static string Exomination { get; internal set; } = string.Empty;
        public override float Radius => 8f;
        public override Color RadiusColor => Color.Lerp(Main.DiscoColor, Color.White, 0.25f);
        public override int DustType => DustID.WhiteTorch;
        public override void SetStaticDefaultsExtra()
        {
            Exomination = ClickerCompat.RegisterClickEffect(Mod, "Exomination", 15, () => RadiusColor, delegate (Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, int type, int damage, float knockBack)
            {
                SoundEngine.PlaySound(Supernova.ExplosionSound, position);
                int p = Projectile.NewProjectile(source, position, Vector2.Zero, ModContent.ProjectileType<SupernovaBoom>(), damage, knockBack, player.whoAmI);
                Main.projectile[p].DamageType = ModContent.GetInstance<ClickerDamage>();
            });
            CalamityClickersUtils.RegisterPostWildMagicClickEffect(Exomination);
            CalamityClickersUtils.RegisterPostNightmareMagicClickEffect(Exomination);
        }
        public override void SetDefaultsExtra()
        {
            AddEffect(Item, Exomination);
            SetDust(Item, DustType);

            Item.damage = 450;
            Item.knockBack = 1f;
            Item.rare = ModContent.RarityType<BurnishedAuric>();
            Item.value = CalamityGlobalItem.RarityVioletBuyPrice;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<ElementalClicker>()
                .AddIngredient<ProfanedClicker>()
                .AddIngredient<StratusClicker>()
                .AddIngredient<MiracleMatter>()
                .AddTile<DraedonsForge>()
                .Register();
        }
    }
}
