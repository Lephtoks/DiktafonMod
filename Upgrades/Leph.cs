using BTD_Mod_Helper;
using BTD_Mod_Helper.Api.Towers;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models;
using Il2CppAssets.Scripts.Models.Towers;
using System.Linq;
using Il2CppAssets.Scripts.Utils;
using Il2CppSystem.Collections.Generic;
using DiktafonMod.Displays;
using BTD_Mod_Helper.Api;
using System;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem.Reflection;

namespace DiktafonMod;
public static class Leph {
    public static void MulRate(TowerModel tower, float mul) {
        tower.GetWeapons().ForEach(weapon => weapon.Rate *= mul);
    }
    public static void print<T>(this LockList<T> enumerator){
        string toprint = $"{enumerator.GetType().Name}:";
        enumerator.ForEach(f => {
            if (f == null) return;
            toprint += $"\n\t{f.GetType().Name} => {f}";
        });
        ModHelper.Msg<DiktafonMod>(toprint);
    }
    public static void print<T>(this LockList<T> enumerator, string tag) where T : Model {
        string toprint = $"{enumerator.GetType().Name}:";
        enumerator.ForEach(f => {
            if (f == null) return;
            toprint += $"\n\t{tag} => {f.name}";
        });
        ModHelper.Msg<DiktafonMod>(toprint);
    }
    public static void log(this object obj) {
        ModHelper.Msg<DiktafonMod>(obj);
    }
    public static readonly Action<Model> printfor = new Action<Model>(beh => beh.GetIl2CppType().Name.log());

    public static void Print(this Model model, int tabulation=0) {
        Il2CppSystem.Type type = model.GetIl2CppType();
        (new string('\t', tabulation) + type.Name).log();
        PropertyInfo? propertyInfo = type.GetProperty("behaviors");
        if (propertyInfo != null) {
            Il2CppSystem.Array? array = (Il2CppSystem.Array?) propertyInfo.GetValue(model);
            if (array != null) {
                foreach (var behModel in array)
                {
                    if (behModel is Model) {
                        ((Model) behModel).Print();
                    }
                }
            }
        }
    }
}