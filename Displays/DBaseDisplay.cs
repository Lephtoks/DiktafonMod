using System.Linq;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Unity.Display;
using BTD_Mod_Helper.Api.Display;
using BTD_Mod_Helper.Extensions;
using UnityEngine;
using UnityEngine.UIElements;

namespace DiktafonMod.Displays
{
    public class DDisplay000 : ModCustomDisplay
    {
        public override string AssetBundleName => "diktafonassets";

        public override string PrefabName => "tower000";
        public override float Scale => Settings.TowerScale;
        
        public void SetSkin(UnityDisplayNode node, string image) {
            Texture2D texture = GetTexture(image);
            texture.filterMode = FilterMode.Point;
            node.GetMeshRenderers().ForEach(renderer => renderer.SetMainTexture(texture));
        }
        public override void ModifyDisplayNode(UnityDisplayNode node) {}
    }
    public abstract class DSkinDisplay : DDisplay000 {
        public abstract string Skin {get;}
        public override void ModifyDisplayNode(UnityDisplayNode node)
        {
            SetSkin(node, Skin);
        }
    }
}