using BTD_Mod_Helper.Api.Display;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Unity.Display;
using UnityEngine;

namespace DiktafonMod.Displays {
    class TearDisplay : ModDisplay
    {
        public override string BaseDisplay => Generic2dDisplay;
        public override float Scale => 7f;

        public override void ModifyDisplayNode(UnityDisplayNode node)
        {
            Sprite sprite = GetSprite("Tear", PixelsPerUnit);
            sprite.texture.filterMode = FilterMode.Point;
            node.GetRenderer<SpriteRenderer>().sprite = sprite;
            node.IsSprite = true;
        }
    }
}