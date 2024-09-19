using System;
using System.Reflection;
using BTD_Mod_Helper;
using BTD_Mod_Helper.Api;
using BTD_Mod_Helper.Api.Towers;
using BTD_Mod_Helper.Extensions;
using DiktafonMod.Displays;
using Il2CppAssets.Scripts.Models.GenericBehaviors;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Emissions;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Weapons;
using Il2CppAssets.Scripts.Models.TowerSets;
using Il2CppAssets.Scripts.Unity;
using Il2CppNinjaKiwi.Common.ResourceUtils;

namespace DiktafonMod {
    public class DiktafonTower : ModTower
    {
        public override TowerSet TowerSet => TowerSet.Primary;

        public override string BaseTower => TowerType.DartMonkey;
        
        public override int Cost => 650;
        public override string DisplayName => "Диктафон";
        public override int TopPathUpgrades => 5;
        public override int MiddlePathUpgrades => 5;
        public override int BottomPathUpgrades => 5;
        public override string Description => "Башня Диктафона";
        public override string Icon => "diktafon";
        public override string Portrait => "DiktafonTower000";

        public override void ModifyBaseTowerModel(TowerModel towerModel)
        {
            string displayTypeName = $"DDisplay{towerModel.tiers[0]}{towerModel.tiers[1]}{towerModel.tiers[2]}";
            Type? displayType = Type.GetType($"DiktafonMod.Displays.{displayTypeName}");
            if (displayType != null) {
                object display = GetInstance(displayType);
                MethodInfo? applyMethod = displayType.GetMethod("Apply", [typeof(TowerModel)]);
                applyMethod?.Invoke(display, [towerModel]);
            } else {
                $"Unknown tower upgrade {towerModel.tiers[0]}-{towerModel.tiers[1]}-{towerModel.tiers[2]}".log();
            }
            towerModel.range += 10;
            var attackModel = towerModel.GetAttackModel();
            attackModel.range += 10;
            

            attackModel.weapons[0].SetEmission(Game.instance.model.GetTowerModel(TowerType.DartMonkey, 0, 3, 0).GetWeapon().emission.Duplicate());
            attackModel.weapons[0].emission.Cast<ArcEmissionModel>().Count = 1;

            var projectile = attackModel.weapons[0].projectile;
            projectile.GetBehavior<TravelStraitModel>().Speed /= 3;
            projectile.pierce = 1;
            projectile.GetDamageModel().damage = 1;
        }
    }
}