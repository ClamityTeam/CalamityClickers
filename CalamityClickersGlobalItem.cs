using CalamityClickers.Content.Items.Weapons.DLC;
using CalamityClickers.Content.Items.Weapons.HM;
using CalamityClickers.Content.Items.Weapons.PostML;
using CalamityClickers.Content.Items.Weapons.PostML.DoG;
using CalamityClickers.Content.Items.Weapons.PostML.Polterghast;
using CalamityClickers.Content.Items.Weapons.PostML.Providance;
using CalamityClickers.Content.Items.Weapons.PostML.Yharon;
using CalamityClickers.Content.Items.Weapons.PreHM;
using CalamityMod;
using CalamityMod.Items.TreasureBags;
using ClickerClass;
using ClickerClass.Items.Accessories;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace CalamityClickers
{
    public class CalamityClickersGlobalItem : GlobalItem
    {
        public override void UpdateAccessory(Item item, Player player, bool hideVisual)
        {
            ClickerPlayer clickPlayer = player.Clicker();
            CalamityClickersPlayer modPlayer = player.CalClicker();
            if (item.ModItem is Soda)
            {
                clickPlayer.clickerBonus -= Soda.ClickAmountDecreaseFlat;
                if (modPlayer.crateLevel < 1)
                    modPlayer.crateLevel = 1;

            }
            if (item.ModItem is GamerCrate)
            {
                clickPlayer.clickerBonusPercent += (float)GamerCrate.ClickAmountDecrease / 100f;
                if (modPlayer.crateLevel < 2)
                    modPlayer.crateLevel = 2;
            }
        }
        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Mod calamity = ModLoader.GetMod("CalamityMod");
            if (ClickerCompat.IsClickerItem(item))
            {
                /*if (player.GetModPlayer<CalamityClickersPlayer>().daedalusClicker && Main.rand.NextBool(5))
                {
                    for (int i = 0; i < 2; i++)
                    {
                        int damageA = (int)(damage * 0.5f);
                        Vector2 vec1 = Main.MouseWorld;
                        Vector2 vec2 = new Vector2(Main.rand.NextFloat(-5f, 5f), Main.rand.NextFloat(-5f, 5f));
                        //Projectile.NewProjectile(vec1, vec2, ProjectileID.CrystalShard, damageA, knockback, player.whoAmI);
                        Projectile.NewProjectile(source, Main.MouseWorld, Main.rand.NextVector2Circular(5f, 5f), ProjectileID.CrystalShard, damage / 10, knockback / 2, player.whoAmI);
                    }
                }
                if (player.GetModPlayer<CalamityClickersPlayer>().godSlayerClicker && Main.rand.NextBool(5))
                {
                    int damageA = (int)(damage * 0.15f);
                    for (int i = 0; i < 4; i++)
                    {
                        int numA = 50;
                        Vector2 vec1 = new Vector2(Main.MouseWorld.X + Main.rand.Next(-numA, numA), Main.MouseWorld.Y + Main.rand.Next(-numA, numA));
                        Vector2 vec2 = new Vector2(0f, 0f);
                        Projectile.NewProjectile(vec1, vec2, ModContent.ProjectileType<NebulaStar>(), damageA, knockBack, player.whoAmI);
                    }
                }*/
                return false;
            }
            return base.Shoot(item, player, source, position, velocity, type, damage, knockback);
        }
        public override void ModifyItemLoot(Item item, ItemLoot itemLoot)
        {
            if (item.type == ModContent.ItemType<DesertScourgeBag>())
            {
                itemLoot.Add(ModContent.ItemType<ScourgeClicker>(), 4);
            }
            if (item.type == ModContent.ItemType<CrabulonBag>())
            {
                itemLoot.Add(ModContent.ItemType<MushyClicker>(), 4);
            }
            if (item.type == ModContent.ItemType<HiveMindBag>())
            {
                itemLoot.Add(ModContent.ItemType<RottenClicker>(), 4);
            }
            if (item.type == ModContent.ItemType<PerforatorBag>())
            {
                itemLoot.Add(ModContent.ItemType<FleshyClicker>(), 4);
            }


            if (item.type == ModContent.ItemType<CryogenBag>())
            {
                itemLoot.Add(ModContent.ItemType<CryoClicker>(), 4);
            }
            if (item.type == ModContent.ItemType<AquaticScourgeBag>())
            {
                itemLoot.Add(ModContent.ItemType<AquaticClicker>(), 4);
            }
            if (item.type == ModContent.ItemType<CalamitasCloneBag>())
            {
                itemLoot.Add(ModContent.ItemType<ClickerOfCalamity>(), 4);
            }
            if (item.type == ModContent.ItemType<LeviathanBag>())
            {
                itemLoot.Add(ModContent.ItemType<AnahitasClicker>(), 4);
            }
            if (item.type == ModContent.ItemType<AstrumAureusBag>())
            {
                itemLoot.Add(ModContent.ItemType<AureusClicker>(), 4);
            }
            if (item.type == ModContent.ItemType<RavagerBag>())
            {
                itemLoot.Add(ModContent.ItemType<BloodGodsClicker>(), 4);
            }
            if (item.type == ModContent.ItemType<PlaguebringerGoliathBag>())
            {
                itemLoot.Add(ModContent.ItemType<PlagueClicker>(), 4);
            }
            if (item.type == ModContent.ItemType<AstrumDeusBag>())
            {
                itemLoot.Add(ModContent.ItemType<CosmicStarClicker>(), 4);
            }
            if (ModLoader.TryGetMod("InfernumMode", out var infernum) && item.type == infernum.Find<ModItem>("BereftVassalBossBag").Type)
            {
                itemLoot.Add(ModContent.ItemType<BereftVassalsClicker>(), 4);
            }


            if (item.type == ModContent.ItemType<DragonfollyBag>())
            {
                itemLoot.Add(ModContent.ItemType<RedLightningClicker>(), 4);
            }
            if (item.type == ModContent.ItemType<ProvidenceBag>())
            {
                itemLoot.Add(ModContent.ItemType<ProfanedClicker>(), 4);
            }
            if (item.type == ModContent.ItemType<PolterghastBag>())
            {
                itemLoot.Add(ModContent.ItemType<RuinousClicker>(), 4);
            }
            if (item.type == ModContent.ItemType<DevourerofGodsBag>())
            {
                itemLoot.Add(ModContent.ItemType<ClickerOfGods>(), 4);
            }
            if (item.type == ModContent.ItemType<YharonBag>())
            {
                itemLoot.Add(ModContent.ItemType<PhoenixClicker>(), 4);
            }
            if (item.type == ModContent.ItemType<DraedonBag>())
            {
                var ares = itemLoot.DefineConditionalDropSet(() => DownedBossSystem.downedAres);
                ares.Add(ModContent.ItemType<ExoClicker>());
            }
            if (item.type == ModContent.ItemType<CalamitasCoffer>())
            {
                itemLoot.Add(ModContent.ItemType<CruelClicker>(), 4);
            }
        }
    }
}
