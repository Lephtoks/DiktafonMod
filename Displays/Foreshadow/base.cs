using BTD_Mod_Helper.Api.Display;

namespace DiktafonMod.Displays.Foreshadow
{
    class ForeshadowBaseDisplay : ModDisplay2D
    {
        public override float Scale => 2f;
        protected override int Order => 1;
        protected override string TextureName => "foreshadowtower";
    }
}