using CalValEXLite.Items;
using CalValEXLite.Items.Dyes;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalValEXLite
{
	public class CalValEXLite : Mod
	{
        public static CalValEXLite Instance;
        public static Mod calamityMod;
        public override void Load()
        {
            foreach (Mod checkMod in ModLoader.Mods)
            {
                if (checkMod.Name == "CalValEX")
                {
                    throw new Exception("Calamities Vanities Lite and Calamities Vanities cannot be loaded at the same time");
                }
            }
            Instance = this;
            if (!Main.dedServ)
            {
                GameShaders.Armor.BindShader(ModContent.ItemType<DraedonHologramDye>(), new ArmorShaderData(new Ref<Effect>(GetEffect("Effects/DraedonHologramDye")), "DraedonHologramDyePass"));
            }
        }

        public override void PostSetupContent()
        {
            calamityMod = ModLoader.GetMod("CalamityMod");
            if (calamityMod != null)
            {
                calamityMod.GetItem("KnowledgeCrabulon").Tooltip.AddTranslation(GameCulture.English, "A crab and its mushrooms, a love story.\nIt's interesting how creatures can adapt given certain circumstances.\nFavorite this item to gain the Mushy buff while underground or in the mushroom biome.\nHowever, your movement speed will be decreased while in these areas due to you being covered in fungi.\nThe great crustacean's mushrooms may also grow angry when attacked, though they will also become harmless.");
            }
        }

        public override void PostAddRecipes()
        {
            AddCalamityRecipes.AddRecipes(this);
        }

        public override void Unload()
        {
            Instance = null;
            calamityMod = null;
        }
    }
}
