using BTD_Mod_Helper.Api.Display;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Unity.Display;
using UnityEngine;

namespace DiktafonMod.Displays {
    class AdsBuff : ModBuffIcon
    {
        public override int MaxStackSize => 2;
        public override bool GlobalRange => false;
        public override string Icon => "TT3";
    }
}