using BTD_Mod_Helper.Api.Display;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Unity.Display;
using UnityEngine;

namespace DiktafonMod.Displays {
    class HuggyBuff : ModBuffIcon
    {
        public override int MaxStackSize => 4;
        public override bool GlobalRange => false;
        public override string Icon => "MT3";
    }
}