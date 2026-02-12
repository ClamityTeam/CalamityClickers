using CalamityClickers.Content.Projectiles;
using CalamityMod;
using CalamityMod.CalPlayer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityClickers.Commons.CatalystCrossmod
{
    public class RenderPlayer : ModPlayer
    {
        public float intergelacticRenderAlpha;

        public List<AstralRocksProjectile.AstralRockRenderData> astralRockRenderData;

        public List<bool> isRender = new List<bool>() { true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, };

        public override void DrawEffects(PlayerDrawSet drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright)
        {
            if (drawInfo.shadow != 0f)
            {
                return;
            }

            Player drawPlayer = drawInfo.drawPlayer;
            CalamityClickersPlayer modPlayer = drawInfo.drawPlayer.GetModPlayer<CalamityClickersPlayer>();
            if (modPlayer.setIntergelacticClicker != null && Main.myPlayer == drawPlayer.whoAmI && CalamityUtils.Calamity(drawPlayer).wearingRogueArmor)
            {
                IsHoldingAnItemWhichRequiresThemToSeeNearbyTiles(drawPlayer);
                float num = CalamityUtils.Calamity(drawPlayer).rogueStealth / CalamityUtils.Calamity(drawPlayer).rogueStealthMax;
                intergelacticRenderAlpha = 1f - num;
            }
            else if (modPlayer.setIntergelacticClicker != null && Main.myPlayer == drawPlayer.whoAmI && IsHoldingAnItemWhichRequiresThemToSeeNearbyTiles(drawPlayer))
            {
                float num2 = (Main.MouseWorld - drawPlayer.Center).Length();
                float num3 = (float)drawPlayer.width + (float)(drawPlayer.blockRange + drawPlayer.HeldItem.tileBoost) * 16f + 128f;
                if (num2 < num3)
                {
                    intergelacticRenderAlpha = 0.1f;
                }

                if (drawPlayer.CalClicker().HideAsteroids)
                {
                    intergelacticRenderAlpha = 0.1f;
                }
                else if (num2 - num3 < 120f)
                {
                    intergelacticRenderAlpha = (num2 - num3) / 120f;
                    if (intergelacticRenderAlpha < 0.3f)
                    {
                        intergelacticRenderAlpha = 0.3f;
                    }
                }
                else
                {
                    intergelacticRenderAlpha = 1f;
                }
            }
            else if (modPlayer.setIntergelacticClicker != null && Main.myPlayer == drawPlayer.whoAmI && drawPlayer.CalClicker().HideAsteroids)
            {
                IsHoldingAnItemWhichRequiresThemToSeeNearbyTiles(drawPlayer);
                intergelacticRenderAlpha = 0.1f;
            }
            else
            {
                intergelacticRenderAlpha = 1f;
            }
        }

        internal void RenderIntergelacticRocks(CalamityClickersPlayer cat, PlayerDrawSet info, bool drawFront)
        {
            Func<Vector3, bool> func = (drawFront ? ((Func<Vector3, bool>)((Vector3 v) => v.Z < 0f)) : ((Func<Vector3, bool>)((Vector3 v) => v.Z >= 0f)));
            Texture2D value = TextureAssets.Projectile[ModContent.ProjectileType<AstralRocksProjectile>()].Value;
            Vector2 center = info.Position + new Vector2(Player.width / 2, Player.height / 2);
            int num = value.Height / 13;
            Rectangle rectangle = new Rectangle(0, 0, value.Width, num - 2);
            Vector2 origin = rectangle.Size() / 2f;
            //int num2 = 1;

            CalamityPlayer val = CalamityUtils.Calamity(Player);
            Color color = (val.auricSet ? new Color(93, 30, 120, 0) : new Color(135, 25, 150, 0)) * (0.2f + intergelacticRenderAlpha * 0.8f);
            Texture2D value2 = ModContent.Request<Texture2D>(ModContent.GetInstance<AstralRocksProjectile>().Texture + "_Glow").Value;
            for (int i = 0; i < 16; i++)
            {
                AstralRocksProjectile.AstralRockRenderData astralRockRenderData = this.astralRockRenderData[i];
                if (func(astralRockRenderData.offsetFromPlayer) && isRender[i])
                {
                    float drawScale = astralRockRenderData.GetDrawScale(1f);
                    Vector2 drawPosition = astralRockRenderData.GetDrawPosition(center);
                    Color color2 = Lighting.GetColor((int)(drawPosition.X / 16f), (int)(drawPosition.Y / 16f));
                    if (val.auricSet)
                    {
                        color2.R = Math.Max(color2.R, (byte)128);
                        color2.G = Math.Max(color2.G, (byte)128);
                        color2.B = Math.Max(color2.B, (byte)128);
                    }

                    if (color2.R < 90)
                    {
                        color2.R = 90;
                    }

                    if (color2.G < 90)
                    {
                        color2.G = 90;
                    }

                    if (color2.B < 90)
                    {
                        color2.B = 90;
                    }

                    color2 *= 1f + (0f - astralRockRenderData.offsetFromPlayer.Z) * 0.002f;
                    color2.A = byte.MaxValue;
                    color2 *= intergelacticRenderAlpha;
                    rectangle.Y = num * astralRockRenderData.frame;
                    DrawData drawData = new DrawData(value, drawPosition - Main.screenPosition + new Vector2(2f, 0f), rectangle, color, 0f, origin, drawScale, SpriteEffects.None);
                    drawData.shader = base.Player.cHead;
                    drawData.ignorePlayerRotation = true;
                    DrawData item = drawData;
                    info.DrawDataCache.Add(item);
                    item.position = drawPosition - Main.screenPosition + new Vector2(-2f, 0f);
                    info.DrawDataCache.Add(item);
                    item.position = drawPosition - Main.screenPosition + new Vector2(0f, 2f);
                    info.DrawDataCache.Add(item);
                    item.position = drawPosition - Main.screenPosition + new Vector2(0f, -2f);
                    info.DrawDataCache.Add(item);
                    info.DrawDataCache.Add(new DrawData(value, drawPosition - Main.screenPosition, rectangle, color2, 0f, origin, drawScale, SpriteEffects.None)
                    {
                        shader = base.Player.cHead,
                        ignorePlayerRotation = true
                    });
                    info.DrawDataCache.Add(new DrawData(value2, drawPosition - Main.screenPosition, rectangle, Color.White * intergelacticRenderAlpha, 0f, origin, drawScale, SpriteEffects.None)
                    {
                        shader = base.Player.cHead,
                        ignorePlayerRotation = true
                    });
                }
            }
        }

        internal static bool IsHoldingAnItemWhichRequiresThemToSeeNearbyTiles(Player Player)
        {
            Item item = Player.inventory[Player.selectedItem];
            if (item.pick <= 0 && item.axe <= 0 && item.hammer <= 0 && item.tileWand <= 0 && item.createTile < TileID.Dirt)
            {
                return item.createWall >= 0;
            }

            return true;
        }

        public override void OnEnterWorld()
        {
            isRender = new List<bool>();
            for (int i = 0; i < 16; i++)
                isRender.Add(true);
        }

        /*public override void PostUpdateEquips()
        {
            Main.NewText(astralRockRenderData.Count);
        }*/
    }
    public class PostDrawLayer : PlayerDrawLayer
    {
        public override Position GetDefaultPosition()
        {
            return new AfterParent(PlayerDrawLayers.BeetleBuff);
        }

        protected override void Draw(ref PlayerDrawSet info)
        {
            Player drawPlayer = info.drawPlayer;
            CalamityClickersPlayer modPlayer = drawPlayer.GetModPlayer<CalamityClickersPlayer>();
            RenderPlayer modPlayer2 = drawPlayer.GetModPlayer<RenderPlayer>();
            if ((info.shadow == 0f) && modPlayer.setIntergelacticClicker != null && modPlayer2.astralRockRenderData != null)
            {
                modPlayer2.RenderIntergelacticRocks(modPlayer, info, drawFront: true);
            }
        }
    }
    public class PreDrawLayer : PlayerDrawLayer
    {
        public override Position GetDefaultPosition()
        {
            return new BeforeParent(PlayerDrawLayers.JimsCloak);
        }

        protected override void Draw(ref PlayerDrawSet info)
        {
            Player drawPlayer = info.drawPlayer;
            CalamityClickersPlayer modPlayer = drawPlayer.GetModPlayer<CalamityClickersPlayer>();
            RenderPlayer modPlayer2 = drawPlayer.GetModPlayer<RenderPlayer>();
            if ((info.shadow == 0f) && modPlayer.setIntergelacticClicker != null && modPlayer2.astralRockRenderData != null)
            {
                modPlayer2.RenderIntergelacticRocks(modPlayer, info, drawFront: false);
            }
        }
    }
}
