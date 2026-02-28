using CalamityClickers.Commons.CatalystCrossmod;
using CalamityClickers.Content.Cooldowns;
using CalamityClickers.Content.Items.Accessories;
using CalamityClickers.Content.Items.Armor;
using CalamityClickers.Content.Items.Weapons;
using CalamityClickers.Content.Items.Weapons.DraedonArsenal;
using CalamityClickers.Content.Items.Weapons.PostML.Polterghast;
using CalamityClickers.Content.Items.Weapons.PreHM;
using CalamityClickers.Content.Projectiles;
using CalamityMod;
using CalamityMod.CalPlayer;
using CalamityMod.Items.Accessories;
using CalamityMod.Items.Potions.Alcohol;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Particles;
using CalamityMod.Projectiles.Rogue;
using CalamityMod.Projectiles.Typeless;
using CalamityMod.Systems.Collections;
using ClickerClass;
using ClickerClass.Buffs;
using ClickerClass.Core.Netcode.Packets;
using ClickerClass.Items;
using ClickerClass.Items.Accessories;
using ClickerClass.Projectiles;
using ClickerClass.Utilities;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace CalamityClickers
{
    public class CalamityClickersPlayer : ModPlayer
    {
        public float rageRegenMult = 0;

        public bool clickerSelected = false;
        public int clickerTotal = 0;
        public int clickAmount = 0;

        public bool setVictideClicker = false;
        public bool setDaedalusClicker = false;
        public bool setHydrothermicClicker = false;
        public bool setTarragonClicker = false;
        public int setTarragonClickerPower = 0;
        public int setTarragonClickerTime = 0;
        public Item setIntergelacticClicker = null;
        public bool SetIntergelactic => setIntergelacticClicker != null && !setIntergelacticClicker.IsAir;
        public int CurrentRockDamage;
        public bool HideAsteroids;
        public int setIntergelacticTimer = 60;
        public bool setBloodflareClicker = false;
        public bool setGodSlayerClicker = false;
        public int setGodSlayerClickerPower = 0;
        public int setGodSlayerClickerCritCounter = 0;

        public bool accFingerOfBG = false;
        public bool accBeetleClickingGlove = false;
        public bool accBloodyChocolate = false;
        public Item accBloodyChocolateItem = null;
        public int accBloodyChocolateCookieCD = 0;
        public Item accSSMedal = null;
        public Item accPortableParticleAcceleratorUpgrades = null;
        public int accPortableParticleAcceleratorUpgradesPower = 0;

        public bool bloodyCookieBuff = false;
        public bool godSlayerClickerBuff = false;

        public bool enchLecherous;
        public bool wulfrumAutoclick = true;
        public override void ResetEffects()
        {
            rageRegenMult = 0;

            clickerSelected = false;

            setVictideClicker = false;
            setDaedalusClicker = false;
            setHydrothermicClicker = false;
            setTarragonClicker = false;
            setIntergelacticClicker = null;
            setBloodflareClicker = false;
            setGodSlayerClicker = false;

            accFingerOfBG = false;
            accBeetleClickingGlove = false;
            accBloodyChocolate = false;
            accBloodyChocolateItem = null;
            accSSMedal = null;
            accPortableParticleAcceleratorUpgrades = null;
            accPortableParticleAcceleratorUpgradesPower = 0;

            bloodyCookieBuff = false;
            godSlayerClickerBuff = false;

            enchLecherous = false;
        }
        public override void UpdateEquips()
        {
            if (ModLoader.TryGetMod("NoxusBoss", out Mod aoe))
            {
                if (Player.isNearNPC(aoe.Find<ModNPC>("AvatarOfEmptiness").Type))
                {
                    ClickerCompat.EnableClickEffect(Player, "ClickerClass:PhaseReach");
                }
            }
        }
        public override void PostUpdateEquips()
        {
            if (ClickerSystem.IsClickerWeapon(Player.HeldItem))
            {
                List<string> clickerEffects = Player.HeldItem.GetGlobalItem<ClickerItemCore>().itemClickEffects;
                if (Player.GetHeldClickerEffects().Contains(PolterplasmClicker.PhantasmalReach))
                {
                    Player.GetDamage<ClickerDamage>() *= (0.75f + Utils.GetLerpValue(0, MathF.Sqrt(Main.screenWidth * Main.screenWidth + Main.screenHeight * Main.screenHeight), (Main.MouseWorld - Player.Center).Length()));
                }
                if (Player.GetHeldClickerEffects().Contains(WulfrumClicker.BasicAutoclicker))
                {
                    if (PlayerInput.Triggers.JustReleased.MouseRight) wulfrumAutoclick = !wulfrumAutoclick;

                    if (wulfrumAutoclick)
                    {
                        ClickerCompat.SetAutoReuseEffect(Player, 8);
                        Player.AddBuff(ModContent.BuffType<WulfrumClickerBuff>(), 3);
                    }
                }
                if (Player.GetHeldClickerEffects().Contains(Bioinjector.VitalInfusion))
                {
                    int needle = ModContent.ProjectileType<BioinjectorPro>();
                    if (PlayerInput.Triggers.JustReleased.MouseRight && Player.ownedProjectileCounts[needle] > 0)
                    {
                        List<int> indexes = new List<int>();
                        for (int index = 0; index < Main.maxProjectiles; ++index)
                        {
                            ref Projectile proj = ref Main.projectile[index];
                            if (!proj.active) continue;
                            if (proj.type == needle)
                            {
                                indexes.Add(index);
                            }
                        }
                        if (indexes.Count > 0)
                        {
                            int randIndex = Main.rand.Next(indexes);
                            Main.projectile[randIndex].ai[0] = 1;
                            Player.Heal(25);
                        }

                    }
                }
            }

            //Bloodflare Armor
            if (setBloodflareClicker)
            {
                float num1 = (Player.statLife / Player.statLifeMax2);
                Player.Clicker().clickerBonusPercent += 0.2f * (1f - num1);

                //ClickerCompat.SetClickerRadiusAdd(Player, bloodflareRadius);
            }

            //Portable Particle Accelerator Upgrades
            if (accPortableParticleAcceleratorUpgrades != null && !accPortableParticleAcceleratorUpgrades.IsAir)
            {
                float radius = Player.Clicker().ClickerRadiusReal * PortableParticleAccelerator.InnerRadiusRatio / 100f; //No need to check motherboard as that isn't ever relevant with < 100% the radius
                if (Player.DistanceSQ(Player.Clicker().clickerPosition) < radius * radius)
                {
                    if (accPortableParticleAcceleratorUpgrades.ModItem != null)
                    {
                        /*if (accPortableParticleAcceleratorUpgrades.ModItem is LihzahrdParticleAccelerator)
                        {
                            Player.GetDamage<ClickerDamage>().Flat += 10;
                        }
                        if (accPortableParticleAcceleratorUpgrades.ModItem is LunarClickingGlove)
                        {
                            Player.GetDamage<ClickerDamage>().Flat += 25;
                        }
                        if (accPortableParticleAcceleratorUpgrades.ModItem is CosmicClickingGlove)
                        {
                            Player.GetDamage<ClickerDamage>().Flat += 100;
                        }*/

                        Player.GetDamage<ClickerDamage>().Flat += accPortableParticleAcceleratorUpgradesPower;
                    }
                }
            }

            //Tarragon Armor
            if (setTarragonClickerTime > 0)
                setTarragonClickerTime--;
            else
                setTarragonClickerPower = 0;
            if (setTarragonClicker)
            {
                Player.statDefense += setTarragonClickerPower;
                //float power = 1f - 1f / (1f + 0.02f * setTarragonClickerPower);
                //Player.endurance += power;
                Player.endurance += 0.002f * setTarragonClickerPower;
            }

            //Intergelactic or Auric Tesla+ Armor
            if (SetIntergelactic)
            {
                //Summon Asteroids
                CalamityPlayer obj = Player.Calamity();
                //Player.AddBuff(ModContent.BuffType<AstrageldonRocksSetbonus>(), 12);
                int num = ModContent.ProjectileType<AstralRocksProjectile>();
                float num2 = Player.GetTotalDamage(setIntergelacticClicker.DamageType).Multiplicative - 1f;
                int damage = (CurrentRockDamage = (int)(120f * (Player.GetTotalDamage(DamageClass.Generic).Multiplicative + num2)));
                if (Player.ownedProjectileCounts[num] <= 0 && Player.whoAmI == Main.myPlayer)
                {
                    Projectile.NewProjectileDirect(Player.GetSource_Accessory(setIntergelacticClicker), Player.Center, Vector2.One, num, damage, 10f, Player.whoAmI);
                }

                if (!obj.auricSet && Main.netMode != NetmodeID.Server)
                {
                    float amount = Math.Min(Player.velocity.Length() / 30f, 1f);
                    Lighting.AddLight(Player.Center, Vector3.Lerp(new Vector3(0.125f, 0.005f, 0.3f), new Vector3(0.85f, 0.09f, 0.82f), amount) * 3f);
                    /*int consequent = Math.Max((int)(40f - base.Player.velocity.Length() * 6f), 4);
                    if (Main.rand.NextBool(consequent))
                    {
                        LegacyParticleSystem.PostDrawPlayers.NewParticle(new AstrageldonSparkle(base.Player.position + new Vector2(Main.rand.Next(base.Player.width), Main.rand.Next(base.Player.height)), base.Player.velocity * -0.4f, AstrageldonSparkle.RandomColor(Main.rand), Main.rand.NextFloat(0.4f, 0.75f)));
                    }*/
                }
                else
                {
                    Lighting.AddLight(Player.Center, new Vector3(0.2f, 1.5f, 2f));
                }

                //Asteroid regen
                if (setIntergelacticTimer > 0)
                    setIntergelacticTimer--;
                else
                {
                    RenderPlayer rPlayer = Player.GetModPlayer<RenderPlayer>();
                    List<int> indexes = new List<int>();
                    for (int i = 0; i < rPlayer.isRender.Count; i++)
                    {
                        if (!rPlayer.isRender[i])
                            indexes.Add(i);
                    }
                    //Main.NewText(indexes.Count);
                    if (indexes.Count > 0)
                    {
                        rPlayer.isRender[indexes[Main.rand.Next(indexes.Count)]] = true;
                        setIntergelacticTimer = 60;
                    }
                }
            }

            //God Slayer Armor
            if (setGodSlayerClicker)
                Player.GetCritChance<ClickerDamage>() += setGodSlayerClickerPower;
            else
                setGodSlayerClickerPower = 0;

            Item heldItem = Player.HeldItem;
            if (ClickerSystem.IsClickerWeapon(heldItem, out ClickerItemCore clickerItem))
            {
                //EnableClickEffect(clickerItem.itemClickEffects);
                clickerSelected = true;
                if (accBloodyChocolateItem != null && !accBloodyChocolateItem.IsAir && clickerSelected)
                {
                    Player.Clicker().accCookieItem = null;
                    accBloodyChocolateCookieCD++;
                    if (Player.whoAmI == Main.myPlayer && accBloodyChocolateCookieCD > 600)
                    {
                        int radius = (int)(95 * Player.Clicker().clickerRadius);
                        if (radius > 350)
                        {
                            radius = 350;
                        }

                        //Sqrt for bias outwards
                        Vector2 pos = Vector2.UnitY.RotatedByRandom(MathHelper.TwoPi) * radius * (float)Math.Sqrt(Main.rand.NextFloat(0.1f, 1f));

                        Projectile.NewProjectile(Player.GetSource_Accessory(accBloodyChocolateItem), pos + Player.Center, Vector2.Zero, ModContent.ProjectileType<BloodyChocCookiesProjectileCookie>(), 0, 0f, Player.whoAmI);

                        accBloodyChocolateCookieCD = 0;
                    }

                    //Cookie Click
                    if (Player.whoAmI == Main.myPlayer)
                    {
                        int cookieType = ModContent.ProjectileType<BloodyChocCookiesProjectileCookie>();
                        for (int i = 0; i < Main.maxProjectiles; i++)
                        {
                            Projectile proj = Main.projectile[i];

                            if (proj.active && proj.owner == Player.whoAmI && proj.type == cookieType && proj.ModProjectile is BloodyChocCookiesProjectileCookie cookie)
                            {
                                if (Main.mouseLeft && Main.mouseLeftRelease && proj.DistanceSQ(Main.MouseWorld) < 30 * 30)
                                {
                                    if (cookie.Frame == 1)
                                    {
                                        SoundEngine.PlaySound(SoundID.Item4, Player.Center);
                                        Player.AddBuff(ModContent.BuffType<CookieBuff>(), 600);
                                        Player.HealLife(10);
                                        for (int k = 0; k < 10; k++)
                                        {
                                            Dust dust = Dust.NewDustDirect(proj.Center, 20, 20, DustID.GemTopaz, Main.rand.NextFloat(-3f, 3f), Main.rand.NextFloat(-3f, 3f), 0, default, 1.15f);
                                            dust.noGravity = true;
                                        }
                                    }
                                    else if (cookie.Frame == 2 || cookie.Frame == 3)
                                    {
                                        SoundEngine.PlaySound(SoundID.Item4, Player.Center);
                                        Player.AddBuff(ModContent.BuffType<BloodyChocCookiesBuff>(), 600);
                                        //Player.HealLife(-20);

                                        //Anti healing
                                        {
                                            /*if (Player.statLife >= Player.statLifeMax2)
                                            {
                                                return;
                                            }*/

                                            int result = -20;
                                            Player.statLife += result;
                                            Player.HealEffect(result);
                                            /*if (Player.statLife > Player.statLifeMax2)
                                            {
                                                Player.statLife = Player.statLifeMax2;
                                            }*/
                                            if (Player.statLife <= 0)
                                            {
                                                //Player.statLife = 0;
                                                PlayerDeathReason reason = PlayerDeathReason.ByCustomReason(LangHelper.GetText("Mods.CalamityClickers.Misc.Death.BloodyClicker" + Main.rand.Next(2).ToString()));
                                                Player.KillMe(reason, 1000, 0, false);
                                            }
                                            if (Player.whoAmI != Main.myPlayer)
                                            {
                                                NetMessage.SendData(MessageID.SpiritHeal, -1, -1, null, Player.whoAmI, result);
                                            }
                                        }

                                        for (int k = 0; k < 10; k++)
                                        {
                                            Dust dust = Dust.NewDustDirect(proj.Center, 20, 20, DustID.GemRuby, Main.rand.NextFloat(-3f, 3f), Main.rand.NextFloat(-3f, 3f), 0, default, 1.15f);
                                            dust.noGravity = true;
                                        }
                                    }
                                    else
                                    {
                                        SoundEngine.PlaySound(SoundID.Item2, Player.Center);
                                        Player.AddBuff(ModContent.BuffType<CookieBuff>(), 300);
                                        for (int k = 0; k < 10; k++)
                                        {
                                            Dust dust = Dust.NewDustDirect(proj.Center, 20, 20, DustID.Dirt, Main.rand.NextFloat(-4f, 4f), Main.rand.NextFloat(-4f, 4f), 75, default, 1.5f);
                                            dust.noGravity = true;
                                        }
                                    }

                                    Player.Clicker().paintingCondition_ClickedCookiesCount++;
                                    if (Player.Clicker().paintingCondition_ClickedCookiesCount == 100)
                                    {
                                        Player.Clicker().paintingCondition_Clicked100Cookies = true;
                                        if (Main.netMode == NetmodeID.MultiplayerClient)
                                        {
                                            new Clicked100CookiesPacket(Player).Send();
                                        }
                                    }

                                    proj.Kill();
                                }
                            }
                        }

                    }
                }

                //SS medal
                if (Player.whoAmI == Main.myPlayer)
                {
                    if (clickerSelected && accSSMedal != null && !accSSMedal.IsAir)
                    {
                        int ssMedalType = ModContent.ProjectileType<SSMedalProjectile>();
                        int sMedalType1 = ModContent.ProjectileType<SMedalPro>();
                        int sMedalType2 = ModContent.ProjectileType<SMedalPro2>();
                        int sMedalType3 = ModContent.ProjectileType<SMedalPro3>();

                        for (int i = 0; i < Main.maxProjectiles; i++)
                        {
                            Projectile proj = Main.projectile[i];

                            if (proj.active && proj.owner == Player.whoAmI && proj.type == ssMedalType && proj.ModProjectile is SSMedalProjectile sMedal)
                            {
                                float len = (proj.Size / 2f).LengthSquared() * 0.78f; //Circle inside the projectile hitbox
                                if (proj.DistanceSQ(Main.MouseWorld) < len)
                                {
                                    sMedal.MouseoverAlpha = 1f;
                                    if (Player.Clicker().accFMedalAmount < FMedal.ChargeMeterMax)
                                    {
                                        Player.Clicker().accFMedalAmount += 1;

                                        for (int j = 0; j < Main.maxProjectiles; j++)
                                        {
                                            Projectile proj1 = Main.projectile[j];
                                            if (proj1 != null && proj1.active)
                                            {
                                                if (proj1.owner == Main.myPlayer && proj1.type == sMedalType1 && proj1.ModProjectile is SMedalProBase sMedalBase)
                                                {
                                                    sMedalBase.MouseoverAlpha = 1f;
                                                    Vector2 offset = new Vector2(Main.rand.Next(-20, 21), Main.rand.Next(-20, 21));
                                                    Dust dust = Dust.NewDustDirect(proj1.Center + offset, 8, 8, DustID.GemSapphire, Scale: 1.25f);
                                                    dust.noGravity = true;
                                                    dust.velocity = -offset * 0.05f;
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                    if (Player.Clicker().accAMedalAmount < AMedal.ChargeMeterMax)
                                    {
                                        Player.Clicker().accAMedalAmount += 1;
                                        for (int j = 0; j < Main.maxProjectiles; j++)
                                        {
                                            Projectile proj1 = Main.projectile[j];
                                            if (proj1 != null && proj1.active)
                                            {
                                                if (proj1.owner == Main.myPlayer && proj1.type == sMedalType2 && proj1.ModProjectile is SMedalProBase sMedalBase)
                                                {
                                                    sMedalBase.MouseoverAlpha = 1f;
                                                    Vector2 offset = new Vector2(Main.rand.Next(-20, 21), Main.rand.Next(-20, 21));
                                                    Dust dust = Dust.NewDustDirect(proj1.Center + offset, 8, 8, DustID.GemTopaz, Scale: 1.25f);
                                                    dust.noGravity = true;
                                                    dust.velocity = -offset * 0.05f;
                                                    break;
                                                }
                                            }
                                        }
                                        /*
                                        sMedal.MouseoverAlpha = 1f;
                                        Vector2 offset = new Vector2(Main.rand.Next(-20, 21), Main.rand.Next(-20, 21));
                                        Dust dust = Dust.NewDustDirect(Main.MouseWorld + offset, 8, 8, 87, Scale: 1.25f);
                                        dust.noGravity = true;
                                        dust.velocity = -offset * 0.05f;
                                        */
                                    }
                                    if (Player.Clicker().accSMedalAmount < SMedal.ChargeMeterMax)
                                    {
                                        Player.Clicker().accSMedalAmount += 1;
                                        for (int j = 0; j < Main.maxProjectiles; j++)
                                        {
                                            Projectile proj1 = Main.projectile[j];
                                            if (proj1 != null && proj1.active)
                                            {
                                                if (proj1.owner == Main.myPlayer && proj1.type == sMedalType3 && proj1.ModProjectile is SMedalProBase sMedalBase)
                                                {
                                                    sMedalBase.MouseoverAlpha = 1f;
                                                    Vector2 offset = new Vector2(Main.rand.Next(-20, 21), Main.rand.Next(-20, 21));
                                                    Dust dust = Dust.NewDustDirect(proj1.Center + offset, 8, 8, DustID.GemTopaz, Scale: 1.25f);
                                                    dust.noGravity = true;
                                                    dust.velocity = -offset * 0.05f;
                                                    break;
                                                }
                                            }
                                        }
                                        /*
                                        //sMedal.MouseoverAlpha = 1f;
                                        Vector2 offset = new Vector2(Main.rand.Next(-20, 21), Main.rand.Next(-20, 21));
                                        Dust dust = Dust.NewDustDirect(Main.MouseWorld + offset, 8, 8, 89, Scale: 1.25f);
                                        dust.noGravity = true;
                                        dust.velocity = -offset * 0.05f;
                                        */
                                    }
                                    if (Player.Clicker().accSMedalAmount >= SMedal.ChargeMeterMax
                                        && Player.Clicker().accAMedalAmount >= AMedal.ChargeMeterMax
                                        && Player.Clicker().accFMedalAmount >= FMedal.ChargeMeterMax
                                        && PlayerInput.Triggers.JustReleased.MouseRight)
                                    {
                                        Projectile.NewProjectile(Player.GetSource_Accessory(accSSMedal), proj.Center, Vector2.Zero, ModContent.ProjectileType<SSMedalProjectileExplosion>(), 10000, 2f, Player.whoAmI);

                                        Player.Clicker().accSMedalAmount = 0;
                                        Player.Clicker().accAMedalAmount = 0;
                                        Player.Clicker().accFMedalAmount = 0;
                                    }
                                }
                            }
                        }
                    }
                    if (accSSMedal != null && !accSSMedal.IsAir)
                    {
                        int ssMedalType = ModContent.ProjectileType<SSMedalProjectile>();
                        if (Player.ownedProjectileCounts[ssMedalType] == 0)
                        {
                            Projectile.NewProjectile(Player.GetSource_Accessory(accSSMedal), Player.Center, Vector2.Zero, ssMedalType, 0, 0f, Player.whoAmI, 0, 0.5f);
                        }
                    }
                }

                //Clicking on Clickable Projectile
                if (Player.whoAmI == Main.myPlayer)
                {
                    if (Player.Clicker().clickerInRange && Player.Clicker().clickerSelected)
                    {

                        for (int i = 0; i < Main.maxProjectiles; i++)
                        {
                            Projectile proj = Main.projectile[i];
                            if (proj.active && proj.owner == Player.whoAmI &&
                                proj.ModProjectile is ClickableClickerProjectile clickable && !clickable.HasChanged && !clickable.Trigger)
                            {
                                if (Main.mouseLeft && Main.mouseLeftRelease && proj.DistanceSQ(Main.MouseWorld) < 30 * 30)
                                {
                                    clickable.Trigger = true; //Handled in the AI
                                }
                            }
                        }
                    }
                }
            }
        }
        public override void PostUpdateMiscEffects()
        {
            Player.Clicker().clickerBonusPercent = 1f / (2f - Player.Clicker().clickerBonusPercent);

            //UpdateRippers();
        }
        private void UpdateRippers()
        {
            ref float rage = ref Player.Calamity().rage;
            ref float rageMax = ref Player.Calamity().rageMax;


            // This is how much Rage will be changed by this frame.
            float rageDiff = 0;

            // If the player equips multiple rage generation accessories they get the max possible effect without stacking any of them.
            {
                float rageGen = 0f;

                // Shattered Community provides constant rage generation (stronger than Heart of Darkness).
                if (Player.Calamity().shatteredCommunity)
                {
                    float scRageGen = rageMax * ShatteredCommunity.RagePerSecond / 60f;
                    if (rageGen < scRageGen)
                        rageGen = scRageGen;
                }
                // Heart of Darkness grants constant rage generation.
                else if (Player.Calamity().heartOfDarkness)
                {
                    float hodRageGen = rageMax * HeartofDarkness.RagePerSecond / 60f;
                    if (rageGen < hodRageGen)
                        rageGen = hodRageGen;
                }

                rageDiff += rageGen;
            }

            // Holding Gael's Greatsword grants constant rage generation.
            if (Player.Calamity().heldGaelsLastFrame)
                rageDiff += rageMax * GaelsGreatsword.RagePerSecond / 60f;

            // Calculate and grant proximity rage.
            // Regular enemies can give up to 1x proximity rage. Bosses can give up to 3x. Multiple regular enemies don't stack.
            // Proximity rage is maxed out when within 10 blocks (160 pixels) of the enemy's hitbox.
            // Its max range is 50 blocks (800 pixels), at which you get zero proximity rage.
            // Proximity rage does not generate while Rage Mode is active.
            if (!Player.Calamity().rageModeActive)
            {
                float bossProxRageMultiplier = 3f;
                float minProxRageDistance = 160f;
                float maxProxRageDistance = 800f;
                float enemyDistance = maxProxRageDistance + 1f;
                float bossDistance = maxProxRageDistance + 1f;

                foreach (NPC npc in Main.ActiveNPCs)
                {
                    if (npc.type == NPCID.None || !npc.IsAnEnemy() || !npc.Calamity().ProvidesProximityRage)
                        continue;

                    // Take the longer of the two directions for the NPC's hitbox to be generous.
                    float generousHitboxWidth = Math.Max(npc.Hitbox.Width / 2f, npc.Hitbox.Height / 2f);
                    float hitboxEdgeDist = npc.Distance(Player.Center) - generousHitboxWidth;

                    // If this enemy is closer than the previous, reduce the current minimum proximity distance.
                    if (enemyDistance > hitboxEdgeDist)
                    {
                        enemyDistance = hitboxEdgeDist;

                        // If they're a boss, reduce the boss distance.
                        // Boss distance will always be >= enemy distance, so there's no need to do another check.
                        // Worm boss body and tail segments are not counted as bosses for this calculation.
                        if (npc.IsABoss() && !CalamityNPCSets.BossSegmentThatDoesNotGenerateRageFaster[npc.type])
                            bossDistance = hitboxEdgeDist;
                    }
                }

                // Helper function to implement proximity rage formula
                float ProxRageFromDistance(float dist)
                {
                    // Adjusted distance with the 160 grace pixels added in. If you're closer than that it counts as zero.
                    float d = Math.Max(dist - minProxRageDistance, 0f);

                    // The first term is exponential decay which reduces rage gain significantly over distance.
                    // The second term is a linear component which allows a baseline but weak rage generation even at far distances.
                    // This function takes inputs from 0.0 to 640.0 and returns a value from 1.0 to 0.0.
                    float r = 1f / (0.034f * d + 2f) + (590.5f - d) / 1181f;
                    return MathHelper.Clamp(r, 0f, 1f);
                }

                // If anything is close enough then provide proximity rage.
                // You can only get proximity rage from one target at a time. You gain rage from whatever target would give you the most rage.
                if (enemyDistance <= maxProxRageDistance)
                {
                    // If the player is close enough to get proximity rage they are also considered to have rage combat frames.
                    // This prevents proximity rage from fading away unless you run away without attacking for some reason.
                    Player.Calamity().rageCombatFrames = Math.Max(Player.Calamity().rageCombatFrames, 3);

                    float proxRageFromEnemy = ProxRageFromDistance(enemyDistance);
                    float proxRageFromBoss = 0f;
                    if (bossDistance <= maxProxRageDistance)
                        proxRageFromBoss = bossProxRageMultiplier * ProxRageFromDistance(bossDistance);

                    float finalProxRage = Math.Max(proxRageFromEnemy, proxRageFromBoss);

                    // 300% proximity rage (max possible from a boss) will fill the Rage meter in 15 seconds.
                    // 100% proximity rage (max possible from an enemy) will fill the Rage meter in 45 seconds.
                    rageDiff += finalProxRage * rageMax / CalamityUtils.SecondsToFrames(45f);
                }
            }

            bool rageFading = Player.Calamity().rageCombatFrames <= 0 && !Player.Calamity().heartOfDarkness && !Player.Calamity().shatteredCommunity;

            // If Rage Mode is currently active, you smoothly lose all rage over the duration.
            //if (Player.Calamity().rageModeActive)
            //    rageDiff -= rageMax / Player.Calamity().RageDuration;

            // If out of combat and NOT using Heart of Darkness or Shattered Community, Rage fades away.
            //else if (!Player.Calamity().rageModeActive && rageFading)
            //    rageDiff -= rageMax / CalamityUtils.SecondsToFrames(30);

            // Apply the rage change and cap rage in both directions.
            // Changes are only applied if the Rage mechanic is available.
            if (Player.Calamity().RageEnabled)
            {
                rage += rageDiff * rageRegenMult;
                if (rage < 0f)
                    rage = 0f;

                if (rage >= rageMax)
                {
                    // If Rage is not active, it is capped at 100%.
                    if (!Player.Calamity().rageModeActive)
                        rage = rageMax;

                    // If using the Shattered Community, Rage is capped at 200% while it's active.
                    // This prevents infinitely stacking rage before a fight by standing on spikes/lava with a regen build or the Nurse handy.
                    else if (Player.Calamity().shatteredCommunity && rage >= 2f * rageMax)
                        rage = 2f * rageMax;
                }
            }
        }
        public override bool Shoot(Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (ClickerSystem.IsClickerWeapon(item))
            {
                var clickerPlayer = Player.GetModPlayer<ClickerPlayer>();

                bool preventsClickEffects = Player.CanAutoReuseItem(item) && clickerPlayer.ActiveAutoReuseEffect.PreventsClickEffects;
                if (!preventsClickEffects)
                {
                    foreach (var name in ClickerSystem.GetAllEffectNames())
                    {
                        if (clickerPlayer.HasClickEffect(name, out ClickEffect effect))
                        {
                            if (accFingerOfBG && !Player.HasCooldown(FingerOfBloodGodCooldown.ID))
                            {
                                effect.Action?.Invoke(Player, source, position, type, damage, knockback);
                                Player.AddCooldown(FingerOfBloodGodCooldown.ID, 60 * 10);
                            }
                        }
                    }
                }

                if (SetIntergelactic)
                {
                    setIntergelacticTimer -= 12;
                }
                if (setVictideClicker)
                {
                    if (Player.whoAmI == Main.myPlayer && Main.rand.NextBool(10))
                    {
                        // Victide All-class Seashells: 200%, soft cap starts at 46 base damage
                        int seashellDamage = CalamityUtils.DamageSoftCap(damage * 2, 46);
                        seashellDamage = Player.Calamity().oldFashioned ? seashellDamage : (int)((double)seashellDamage * 1.5f);

                        Projectile.NewProjectile(source, Main.MouseWorld, Vector2.Zero, ModContent.ProjectileType<Seashell>(), seashellDamage, 1f, Player.whoAmI);
                    }
                }
                return false;
            }
            return base.Shoot(item, source, position, velocity, type, damage, knockback);
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (SetIntergelactic)
            {
                if (ModLoader.TryGetMod("CatalystMod", out var catalyst))
                    target.AddBuff(catalyst.Find<ModBuff>("AstralBlight").Type, 360);
            }
        }
        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (proj.type == ModContent.ProjectileType<ClickDamage>())
            {
                if (setTarragonClicker && !Player.HasCooldown(TarragonClickerCooldown.ID))
                {
                    setTarragonClickerPower++;
                    if (setTarragonClickerPower > 50)
                        setTarragonClickerPower = 50;
                    setTarragonClickerTime += 60;
                    if (setTarragonClickerTime > 50 * 60)
                        setTarragonClickerTime = 50 * 60;
                }
                if (setBloodflareClicker)
                {
                    Player.lifeRegenTime += 2;
                }
                if (setGodSlayerClicker && !godSlayerClickerBuff)
                {
                    if (!hit.Crit)
                        setGodSlayerClickerPower++;
                    else
                    {
                        setGodSlayerClickerPower = 0;
                        if (!Player.HasCooldown(UltraboostCooldown.ID))
                        {
                            setGodSlayerClickerCritCounter++;
                            if (setGodSlayerClickerCritCounter > 50)
                            {
                                Player.AddBuff(ModContent.BuffType<GodSlayerCapsuitBuff>(), 5 * 60);
                                Player.AddCooldown(UltraboostCooldown.ID, 30 * 60);
                                SoundEngine.PlaySound(new SoundStyle("CalamityMod/Sounds/Custom/AbilitySounds/RageActivate"));
                                for (int i = 0; i < 20; i++)
                                {
                                    SparkParticle spark = new SparkParticle(Player.Center, Main.rand.NextVector2CircularEdge(20, 20) * Main.rand.NextFloat(0.9f, 1.1f), false, 60, 2, Main.rand.NextBool() ? Color.Aqua : Color.Fuchsia);
                                    GeneralParticleHandler.SpawnParticle(spark);
                                }
                                DirectionalPulseRing ring = new DirectionalPulseRing(Player.Center, Vector2.Zero, Main.rand.NextBool() ? Color.Aqua : Color.Fuchsia, Vector2.One / 2, 0, 0, 2f, 10);
                                GeneralParticleHandler.SpawnParticle(ring);
                                setGodSlayerClickerCritCounter = 0;
                            }
                        }
                    }
                }
            }
            if (proj.DamageType is ClickerDamage)
            {
                if (setHydrothermicClicker)
                {
                    target.AddBuff(ModContent.BuffType<HydrothermicCapsuitDebuff>(), (int)MathHelper.Clamp(target.CalClicker().hydrothermicBoil + 6, 5 * 60, 20 * 60));
                    target.CalClicker().hydrothermicBoilPower += 5;
                }
            }
        }
        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref NPC.HitModifiers modifiers)
        {
            if (proj.DamageType is ClickerDamage)
            {
                //if (godSlayerClicker)
                if (godSlayerClickerBuff)
                    modifiers.SetCrit();
                if (accPortableParticleAcceleratorUpgrades != null && !accPortableParticleAcceleratorUpgrades.IsAir && Main.rand.Next(100) < 15)
                {
                    modifiers.CritDamage *= 1.5f;
                }
            }
        }
        public override void OnHurt(Player.HurtInfo info)
        {
            if (!Player.HasCooldown(TarragonClickerCooldown.ID) && setTarragonClicker)
            {
                setTarragonClickerPower = 0;
                setTarragonClickerTime = 0;

                float random = Main.rand.Next(30, 90);
                float spread = random * 0.0174f;
                double startAngle = Main.rand.NextFloat(0, MathHelper.TwoPi) - spread / 2;
                double deltaAngle = spread / 8f;

                int projID = ModContent.ProjectileType<TarraThornRight>();
                int splitDamage = 500;
                float splitKB = 1f;
                float velocityPower = 3f;
                for (int i = 0; i < 4; i++)
                {
                    double offsetAngle = startAngle + deltaAngle * (i + i * i) / 2f + 32f * i;
                    int index = Projectile.NewProjectile(Player.GetSource_None(), Player.Center.X, Player.Center.Y, (float)(Math.Sin(offsetAngle) * 5f) * velocityPower, (float)(Math.Cos(offsetAngle) * 5f) * velocityPower, projID, splitDamage, splitKB, Player.whoAmI);
                    Main.projectile[index].DamageType = ModContent.GetInstance<ClickerDamage>();
                    index = Projectile.NewProjectile(Player.GetSource_None(), Player.Center.X, Player.Center.Y, (float)(-Math.Sin(offsetAngle) * 5f) * velocityPower, (float)(-Math.Cos(offsetAngle) * 5f) * velocityPower, projID, splitDamage, splitKB, Player.whoAmI);
                    Main.projectile[index].DamageType = ModContent.GetInstance<ClickerDamage>();
                }
                DirectionalPulseRing ring = new DirectionalPulseRing(Player.Center, Vector2.Zero, Color.Lime, Vector2.One / 2, 0, 0, 3f, 10);
                GeneralParticleHandler.SpawnParticle(ring);
                Player.AddCooldown(TarragonClickerCooldown.ID, 10 * 60);
            }
        }
        public override void SaveData(TagCompound tag)
        {
            tag.Add("calamityClickerTotal", clickerTotal);

        }
        public override void LoadData(TagCompound tag)
        {
            clickerTotal = tag.GetInt("calamityClickerTotal");

        }
        public override void DrawEffects(PlayerDrawSet drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright)
        {
            if (godSlayerClickerBuff && drawInfo.shadow == 0f)
                GodSlayerCapsuitBuff.DrawEffects(drawInfo);
        }
    }
}
