using CalamityClickers.Content.Items.Armor;
using CalamityClickers.Content.Items.Misc.SFXButton;
using CalamityClickers.Content.Items.Weapons.DLC;
using CalamityClickers.Content.Items.Weapons.HM;
using CalamityClickers.Content.Items.Weapons.PostML;
using CalamityClickers.Content.Items.Weapons.PostML.DoG;
using CalamityClickers.Content.Items.Weapons.PostML.Polterghast;
using CalamityClickers.Content.Items.Weapons.PostML.Providance;
using CalamityClickers.Content.Items.Weapons.PostML.Yharon;
using CalamityClickers.Content.Items.Weapons.PreHM;
using CalamityMod;
using CalamityMod.NPCs.AquaticScourge;
using CalamityMod.NPCs.Astral;
using CalamityMod.NPCs.AstrumAureus;
using CalamityMod.NPCs.AstrumDeus;
using CalamityMod.NPCs.Bumblebirb;
using CalamityMod.NPCs.CalClone;
using CalamityMod.NPCs.Crabulon;
using CalamityMod.NPCs.Cryogen;
using CalamityMod.NPCs.DesertScourge;
using CalamityMod.NPCs.DevourerofGods;
using CalamityMod.NPCs.ExoMechs.Ares;
using CalamityMod.NPCs.HiveMind;
using CalamityMod.NPCs.Leviathan;
using CalamityMod.NPCs.NormalNPCs;
using CalamityMod.NPCs.Perforator;
using CalamityMod.NPCs.PlaguebringerGoliath;
using CalamityMod.NPCs.Polterghast;
using CalamityMod.NPCs.Providence;
using CalamityMod.NPCs.Ravager;
using CalamityMod.NPCs.SupremeCalamitas;
using CalamityMod.NPCs.TownNPCs;
using CalamityMod.NPCs.Yharon;
using CalamityMod.Systems.Collections;
using ClickerClass.Items;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityClickers
{
    public class CalamityClickersGlobalNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;
        public bool wither = false;
        public bool hydrothermicBoil = false;
        public int hydrothermicBoilTime = 0;
        public int hydrothermicBoilPower = 50;
        public bool clickDebuff = false;
        public int clickDebuffOwner = -1;

        public override GlobalNPC Clone(NPC from, NPC to)
        {
            CalamityClickersGlobalNPC myClone = (CalamityClickersGlobalNPC)base.Clone(from, to);
            myClone.wither = wither;
            myClone.hydrothermicBoil = hydrothermicBoil;
            myClone.hydrothermicBoilPower = hydrothermicBoilPower;
            myClone.clickDebuff = clickDebuff;
            return myClone;
        }

        public override void UpdateLifeRegen(NPC npc, ref int damage)
        {
            // Debuff vulnerabilities and resistances.
            // Damage multiplier calcs.
            // Worms that are vulnerable to debuffs and Slime God slimes take reduced damage from vulnerabilities.
            bool wormBoss = CalamityNPCTypeSets.DesertScourge.Contains(npc.type) || CalamityNPCTypeSets.EaterOfWorlds.Contains(npc.type) || CalamityNPCTypeSets.Perforators.Contains(npc.type) ||
                CalamityNPCTypeSets.AquaticScourge.Contains(npc.type) || CalamityNPCTypeSets.AstrumDeus.Contains(npc.type) || CalamityNPCTypeSets.StormWeaver.Contains(npc.type);
            bool slimeGod = CalamityNPCTypeSets.SlimeGod.Contains(npc.type);



            // Subtract 1 for the vanilla damage multiplier because it's already dealing DoT in the vanilla regen code.
            //double vanillaHeatDamageMult = heatDamageMult - CalamityGlobalNPC.BaseDoTDamageMult;
            //double vanillaColdDamageMult = coldDamageMult - CalamityGlobalNPC.BaseDoTDamageMult;
            //double vanillaSicknessDamageMult = sicknessDamageMult - CalamityGlobalNPC.BaseDoTDamageMult;


            /*if (wither > 0)
            {
                int baseBrimstoneFlamesDoTValue = npc.lifeMax / 100;
                ApplyDPSDebuff(baseBrimstoneFlamesDoTValue, baseBrimstoneFlamesDoTValue / 5, ref npc.lifeRegen, ref damage);
            }
            if (hydrothermicBoil > 0)
            {
                int baseCrushDepthDoTValue = (int)(hydrothermicBoilPower * heatDamageMult);
                if (hydrothermicBoilPower > (int)(500 * heatDamageMult))
                    hydrothermicBoilPower = (int)(500 * heatDamageMult);
                ApplyDPSDebuff(baseCrushDepthDoTValue, baseCrushDepthDoTValue / 2, ref npc.lifeRegen, ref damage);
            }
            if (clickDebuff > 0 && clickDebuffOwner != -1)
            {
                Player player = Main.player[clickDebuffOwner];
                int baseBrimstoneFlamesDoTValue = (int)(player.Clicker().clickerPerSecond * 5);
                ApplyDPSDebuff(baseBrimstoneFlamesDoTValue, baseBrimstoneFlamesDoTValue / 5, ref npc.lifeRegen, ref damage);
            }*/
        }

        public void ApplyDPSDebuff(int lifeRegenValue, int damageValue, ref int lifeRegen, ref int damage)
        {
            if (lifeRegen > 0)
                lifeRegen = 0;

            lifeRegen -= lifeRegenValue;

            if (damage < damageValue)
                damage = damageValue;
        }
        public override void ResetEffects(NPC npc)
        {
            wither = false;
            hydrothermicBoil = false;
            clickDebuff = false;
        }

        public override void PostAI(NPC npc)
        {
            if (!hydrothermicBoil) hydrothermicBoilPower = 50;
        }
        public override void DrawEffects(NPC npc, ref Color drawColor)
        {
            if (wither)
                ItsClickerDebuff.DrawEffects(npc, ref drawColor);
        }

        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            LeadingConditionRule mainRule = npcLoot.DefineNormalOnlyDropSet();

            if (npc.type == ModContent.NPCType<EarthElemental>())
            {
                mainRule.Add(ModContent.ItemType<EarthenClicker>(), 4);
            }


            //Any lunar event enemies now drops Mice fragments
            switch (npc.type)
            {
                case NPCID.SolarSpearman: // Drakanian
                case NPCID.SolarSolenian: // Selenian
                case NPCID.SolarCorite:
                case NPCID.SolarSroller:
                case NPCID.SolarDrakomireRider:
                case NPCID.SolarDrakomire:
                case NPCID.SolarCrawltipedeHead:

                case NPCID.VortexSoldier:     // Vortexian
                case NPCID.VortexLarva:       // Alien Larva
                case NPCID.VortexHornet:      // Alien Hornet
                case NPCID.VortexHornetQueen: // Alien Queen
                case NPCID.VortexRifleman:    // Storm Diver

                case NPCID.NebulaBrain:    // Nebula Floater
                case NPCID.NebulaSoldier:  // Predictor
                case NPCID.NebulaHeadcrab: // Brain Suckler
                case NPCID.NebulaBeast:    // Evolution Beast

                case NPCID.StardustSoldier:      // Stargazer
                case NPCID.StardustSpiderBig:    // Twinkle Popper
                case NPCID.StardustJellyfishBig: // Flow Invader
                case NPCID.StardustCellBig:      // Star Cell
                case NPCID.StardustWormHead:     // Milkyway Weaver
                    npcLoot.Add(ItemDropRule.NormalvsExpert(ModContent.ItemType<MiceFragment>(), 8, 7));
                    break;
            }
            if (npc.type == ModContent.NPCType<StellarCulex>())
            {
                //mainRule.Add(ModContent.ItemType<StellarClicker>(), 7);
                npcLoot.AddIf(() => DownedBossSystem.downedAstrumAureus, ModContent.ItemType<StellarClicker>(), 7);
            }


            if (npc.type == ModContent.NPCType<DesertScourgeHead>())
            {
                mainRule.Add(ModContent.ItemType<ScourgeClicker>(), 4);
            }
            if (npc.type == ModContent.NPCType<Crabulon>())
            {
                mainRule.Add(ModContent.ItemType<MushyClicker>(), 4);
            }
            if (npc.type == ModContent.NPCType<HiveMind>())
            {
                mainRule.Add(ModContent.ItemType<RottenClicker>(), 4);
            }
            if (npc.type == ModContent.NPCType<PerforatorHive>())
            {
                mainRule.Add(ModContent.ItemType<FleshyClicker>(), 4);
            }

            if (npc.type == ModContent.NPCType<Cryogen>())
            {
                mainRule.Add(ModContent.ItemType<CryoClicker>(), 4);
            }
            if (npc.type == ModContent.NPCType<AquaticScourgeHead>())
            {
                mainRule.Add(ModContent.ItemType<AquaticClicker>(), 4);
            }
            if (npc.type == ModContent.NPCType<CalamitasClone>())
            {
                mainRule.Add(ModContent.ItemType<ClickerOfCalamity>(), 4);
            }
            if (npc.type == ModContent.NPCType<Leviathan>() || npc.type == ModContent.NPCType<Anahita>())
            {
                var lastStanding = npcLoot.DefineConditionalDropSet(Leviathan.LastAnLStanding);
                LeadingConditionRule normalOnly = new LeadingConditionRule(new Conditions.NotExpert());
                lastStanding.Add(normalOnly);
                normalOnly.Add(ModContent.ItemType<AnahitasClicker>(), 4);
            }
            if (npc.type == ModContent.NPCType<AstrumAureus>())
            {
                mainRule.Add(ModContent.ItemType<AureusClicker>(), 4);
            }
            if (npc.type == ModContent.NPCType<RavagerBody>())
            {
                mainRule.Add(ModContent.ItemType<BloodGodsClicker>(), 4);
            }
            if (npc.type == ModContent.NPCType<PlaguebringerGoliath>())
            {
                mainRule.Add(ModContent.ItemType<PlagueClicker>(), 4);
            }
            if (npc.type == ModContent.NPCType<AstrumDeusHead>())
            {
                var lastWorm = npcLoot.DefineConditionalDropSet(info => !AstrumDeusHead.ShouldNotDropThings(info.npc));

                lastWorm.Add(DropHelper.NormalVsExpertQuantity(ModContent.ItemType<MiceFragment>(), 1, 16, 24, 20, 32));

                var normalOnly = new LeadingConditionRule(new Conditions.NotExpert());
                lastWorm.Add(normalOnly);
                normalOnly.Add(ModContent.ItemType<CosmicStarClicker>(), 4);
            }
            if (ModLoader.TryGetMod("InfernumMode", out var infernum) && npc.type == infernum.Find<ModNPC>("BereftVassal").Type)
            {
                mainRule.Add(ModContent.ItemType<BereftVassalsClicker>(), 4);
            }


            if (npc.type == ModContent.NPCType<Dragonfolly>())
            {
                mainRule.Add(ModContent.ItemType<RedLightningClicker>(), 4);
            }
            if (npc.type == ModContent.NPCType<Providence>())
            {
                mainRule.Add(ModContent.ItemType<ProfanedClicker>(), 4);
            }
            if (npc.type == ModContent.NPCType<Polterghast>())
            {
                mainRule.Add(ModContent.ItemType<RuinousClicker>(), 4);
            }
            if (npc.type == ModContent.NPCType<DevourerofGodsHead>())
            {
                mainRule.Add(ModContent.ItemType<ClickerOfGods>(), 4);
            }
            if (npc.type == ModContent.NPCType<Yharon>())
            {
                mainRule.Add(ModContent.ItemType<PhoenixClicker>(), 4);
            }
            if (npc.type == ModContent.NPCType<AresBody>())
            {
                bool AresLoot(DropAttemptInfo info) => info.npc.type == ModContent.NPCType<AresBody>() || DownedBossSystem.downedAres;
                mainRule.Add(ItemDropRule.ByCondition(DropHelper.If(AresLoot), ModContent.ItemType<ExoClicker>()));
            }
            if (npc.type == ModContent.NPCType<SupremeCalamitas>())
            {
                mainRule.Add(ModContent.ItemType<CruelClicker>(), 4);
            }

        }
        public override void ModifyShop(NPCShop shop)
        {
            int type = shop.NpcType;

            if (type == ModContent.NPCType<Archmage>())
            {
                shop.Add(ModContent.ItemType<FrostysClicker>(), Condition.DownedEverscream, Condition.DownedSantaNK1, Condition.DownedIceQueen);
            }
            if (type == NPCID.Clothier)
            {
                shop.AddWithCustomValue(ModContent.ItemType<SilvaCapsuit>(), Item.buyPrice(gold: 8), new Condition(CalamityUtils.GetText("Condition.PostDoG"), () => DownedBossSystem.downedDoG));
                shop.AddWithCustomValue(ModContent.ItemType<ReaverCapsuit>(), Item.buyPrice(gold: 4), new Condition(LangHelper.GetLocalizedText("Condition.PostPlant"), () => NPC.downedPlantBoss));
            }
            if (type == NPCID.PartyGirl)
            {
                shop.Add<SFXButtonCalamity>(Condition.BloodMoon);
                shop.Add<SFXButtonAstral>(CalamityConditions.InAstral);
                shop.Add<SFXButtonAuric>(CalamityConditions.DownedYharon);
            }
        }
    }
}
