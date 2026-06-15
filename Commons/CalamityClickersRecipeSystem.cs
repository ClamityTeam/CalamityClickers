using CalamityMod.Items.Materials;
using ClickerClass.Items;
using ClickerClass.Items.Accessories;
using ClickerClass.Items.Misc;
using ClickerClass.Items.Tools;
using Terraria;
using Terraria.ModLoader;

namespace CalamityClickers.Commons
{
    public class CalamityClickersRecipeSystem : ModSystem
    {
        public int AnyNormalSFXButton;
        public override void AddRecipeGroups()
        {
            //RecipeGroup firefly = RecipeGroup.recipeGroups[Calamity.Fireflies];
            //firefly.ValidItems.Add(ModContent.ItemType<TwinklerItem>());

            {
                RecipeGroup group = new RecipeGroup(() => LangHelper.GetText("Misc.RecipeGroup.AnyNormalSFXButton"), new int[]
                {
                    ModContent.ItemType<SFXButtonA>(),
                    ModContent.ItemType<SFXButtonB>(),
                    ModContent.ItemType<SFXButtonC>(),
                    ModContent.ItemType<SFXButtonD>(),
                    ModContent.ItemType<SFXButtonE>(),
                    ModContent.ItemType<SFXButtonF>(),
                    ModContent.ItemType<SFXButtonG>(),
                    ModContent.ItemType<SFXButtonH>()
                });
                AnyNormalSFXButton = RecipeGroup.RegisterGroup("AnyNormalSFXButton", group);
            }


            if (RecipeGroup.recipeGroupIDs.ContainsKey("LunarHamaxe"))
            {
                int index = RecipeGroup.recipeGroupIDs["LunarHamaxe"];
                RecipeGroup group = RecipeGroup.recipeGroups[index];
                group.ValidItems.Add(ModContent.ItemType<MiceHamaxe>());
            }
            if (RecipeGroup.recipeGroupIDs.ContainsKey("LunarPickaxe"))
            {
                int index = RecipeGroup.recipeGroupIDs["LunarPickaxe"];
                RecipeGroup group = RecipeGroup.recipeGroups[index];
                group.ValidItems.Add(ModContent.ItemType<MicePickaxe>());
            }
            if (RecipeGroup.recipeGroupIDs.ContainsKey("AnyWings"))
            {
                int index = RecipeGroup.recipeGroupIDs["AnyWings"];
                RecipeGroup group = RecipeGroup.recipeGroups[index];
                group.ValidItems.Add(ModContent.ItemType<TheScroller>());
            }
        }

        public override void AddRecipes()
        {
            /*Recipe.Create(ModContent.ItemType<TheClicker>())
                .AddIngredient<LordsClicker>()
                .AddIngredient(ItemID.WhitePaint, 50)
                .AddIngredient<ShadowspecBar>(5)
                .AddTile<DraedonsForge>()
                .Register();*/
        }

        public override void PostAddRecipes()
        {
            foreach (Recipe recipe in Main.recipe)
            {
                if (recipe.HasResult<GamerCrate>())
                {
                    recipe.AddIngredient<PurifiedGel>(5);
                }

            }
        }
    }
}
