using CalValEXLite.Items.Dyes;
using CalValEXLite.Items.Pets.Worms;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalValEXLite.Items
{
    public static class AddCalamityRecipes
    {
        public static void AddRecipes(Mod mod)
        {
            if (CalValEXLite.calamityMod != null)
            {
                ModRecipe recipe = new ModRecipe(mod);
                Mod calamityMod = CalValEXLite.calamityMod;

                recipe.AddIngredient(calamityMod.ItemType("CosmicDischarge"));
                recipe.AddIngredient(calamityMod.ItemType("HolyCollider"));
                recipe.AddIngredient(calamityMod.ItemType("BansheeHook"));
                recipe.AddIngredient(calamityMod.ItemType("StreamGouge"));
                recipe.AddIngredient(calamityMod.ItemType("SoulEdge"));
                recipe.AddIngredient(calamityMod.ItemType("Valediction"));
                recipe.SetResult(ModContent.ItemType<SparrowMeat>());
                recipe.AddRecipe();

                recipe = new ModRecipe(mod);
                recipe.AddIngredient(ItemID.BottledWater);
                recipe.AddIngredient(calamityMod.ItemType("PowerCell"), 5);
                recipe.AddTile(TileID.DyeVat);
                recipe.SetResult(ModContent.ItemType<DraedonHologramDye>());
                recipe.AddRecipe();

                recipe = new ModRecipe(mod);
                recipe.AddIngredient(ItemID.LunarBar, 500);
                recipe.AddIngredient(calamityMod.ItemType("SoulEdge"), 5);
                recipe.AddIngredient(calamityMod.ItemType("EidolicWail"), 5);
                recipe.AddIngredient(calamityMod.ItemType("CalamitousEssence"), 20);
                recipe.AddTile(calamityMod.TileType("DraedonsForge"));
                recipe.SetResult(ModContent.ItemType<SoulShard>());
                recipe.AddRecipe();
            }
        }
    }
}
