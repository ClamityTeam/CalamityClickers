using CalamityMod.Dusts;
using CalamityMod.Items;
using ClickerClass;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityClickers.Content.Items.Weapons.HM
{
    public class MonolithClicker : ModdedClickerWeapon
    {
        public static string DoubleClick { get; internal set; } = string.Empty;
        public override float Radius => 2.95f;
        public override Color RadiusColor => new Color(152, 152, 155);
        public override void SetStaticDefaultsExtra()
        {
            DoubleClick = ClickerSystem.RegisterClickEffect(Mod, "DoubleClick", 5, RadiusColor, delegate (Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, int type, int damage, float knockBack)
            {
                SoundEngine.PlaySound(SoundID.Item37, position);
                Projectile.NewProjectile(source, position, Vector2.Zero, type, damage, knockBack, player.whoAmI);
                for (int i = 0; i < 2; i++)
                {
                    Dust dust = Dust.NewDustPerfect(position + Main.rand.NextVector2Circular(3, 3), Main.rand.NextBool() ? ModContent.DustType<AstralOrange>() : ModContent.DustType<AstralBlue>(), Vector2.Zero, 0, default, Main.rand.NextFloat(1.2f, 1.6f));
                    dust.noGravity = true;
                }
            });
        }
        public override void SetDefaultsExtra()
        {
            AddEffect(Item, DoubleClick);

            Item.damage = 20;
            Item.knockBack = 2f;
            Item.rare = ItemRarityID.Orange;
            Item.value = CalamityGlobalItem.RarityOrangeBuyPrice;
        }
    }
    public class MonolithClickerProjectile : ModdedClickerProjectile
    {

    }
}
