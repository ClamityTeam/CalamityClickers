using CalamityMod.Items;
using CalamityMod.Items.Materials;
using ClickerClass.Items.Weapons.Clickers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityClickers.Content.Items.Weapons.PostML.Polterghast
{
    public class PolterplasmClicker : ModdedClickerWeapon
    {
        public static string PhantasmalReach { get; internal set; } = string.Empty;
        public override float Radius => 6f;
        public override Color RadiusColor => new Color(255, 80, 128);
        public override void SetStaticDefaultsExtra()
        {
            PhantasmalReach = ClickerCompat.RegisterClickEffect(Mod, "PhantasmalReach", 1, RadiusColor, delegate (Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, int type, int damage, float knockBack)
            {

            });
            CalamityClickersUtils.RegisterPostWildMagicClickEffect(PhantasmalReach);
            CalamityClickersUtils.RegisterBlacklistedClickEffect(PhantasmalReach);
        }
        public override void SetDefaultsExtra()
        {
            AddEffect(Item, PhantasmalReach);
            AddEffect(Item, "ClickerClass:PhaseReach");
            SetDust(Item, 88);

            Item.damage = 240;
            Item.knockBack = 1f;
            Item.rare = ItemRarityID.Red;
            Item.value = CalamityGlobalItem.RarityRedBuyPrice;
        }
        private void DrawingAnimation(SpriteBatch spriteBatch, Vector2 baseDrawPosition, Rectangle frame, float baseScale, Color drawColor)
        {
            //if (Item.velocity.X != 0f)
            //    return;
            Texture2D t = ModContent.Request<Texture2D>(Texture + "/" + ((int)Main.GlobalTimeWrappedHourly % 5).ToString()).Value;

            spriteBatch.Draw(t, baseDrawPosition, frame, drawColor, 0f, Vector2.Zero, baseScale, SpriteEffects.None, 0f);

        }
        /*public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            if (!CalamityClickersConfig.Instance.LegecyClickerTextures)
            {
                Rectangle frame = TextureAssets.Item[Item.type].Value.Frame();
                DrawingAnimation(spriteBatch, Item.position - Main.screenPosition, frame, scale, lightColor);
                return true;
            }
            return base.PreDrawInWorld(spriteBatch, lightColor, alphaColor, ref rotation, ref scale, whoAmI);
        }
        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            if (!CalamityClickersConfig.Instance.LegecyClickerTextures)
            {
                //Rectangle frame = TextureAssets.Item[Item.type].Value.Frame();
                Item.velocity.X = 0f;
                DrawingAnimation(spriteBatch, Item.position - frame.Size() * 0.25f, frame, scale, drawColor);
                return true;
            }
            return PreDrawInInventory(spriteBatch, position, frame, drawColor, itemColor, origin, scale);
        }*/
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<SpectreClicker>()
                .AddIngredient<Necroplasm>(12)
                .AddIngredient<RuinousSoul>(2)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}
