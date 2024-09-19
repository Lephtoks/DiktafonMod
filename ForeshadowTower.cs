using BTD_Mod_Helper;
using BTD_Mod_Helper.Api.Display;
using BTD_Mod_Helper.Api.Towers;
using BTD_Mod_Helper.Extensions;
using DiktafonMod.Displays;
using DiktafonMod.Displays.Foreshadow;
using Il2Cpp;
using Il2CppAssets.Scripts.Models.Audio;
using Il2CppAssets.Scripts.Models.ContestedTerritory;
using Il2CppAssets.Scripts.Models.GenericBehaviors;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Abilities.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Attack;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Attack.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Emissions;
using Il2CppAssets.Scripts.Models.Towers.Projectiles;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Weapons;
using Il2CppAssets.Scripts.Models.Towers.Weapons.Behaviors;
using Il2CppAssets.Scripts.Models.TowerSets;
using Il2CppAssets.Scripts.Unity;
using Il2CppAssets.Scripts.Unity.Display;
using Il2CppNinjaKiwi.Common.ResourceUtils;
using UnityEngine;

namespace DiktafonMod.Towers
{
    class ForeshadowTower : ModSubTower
    {

        public static Vector3 EjectOffset => new(0, 0, 5);
        public override TowerSet TowerSet => TowerSet.Military;

        public override string BaseTower => TowerType.SniperMonkey;
        public override string Portrait => "foreshadowtower";

        public override void ModifyBaseTowerModel(TowerModel towerModel)
        {

            towerModel.ApplyDisplay<ForeshadowBaseDisplay>();
            var mainWeapon = towerModel.GetAttackModel().weapons[0];
            var mainProj = mainWeapon.projectile;

            mainWeapon.Rate = 5f;
            mainProj.AddBehavior(new CtEmptyModModel("DiktafonMod-Foreshadow"));
            mainProj.AddBehavior(new DamagePercentOfMaxModel("DiktafonMod-ForeshadowDamagePercent", 0.05f, new[] {"Moabs"}, false));
            mainProj.GetDamageModel().damage = 1350f;
            towerModel.RemoveBehavior<AttackModel>();



            var adora = Game.instance.model.GetHeroWithNameAndLevel(TowerType.Adora, 10);
            var ballOfLight = adora.GetDescendant<AbilityCreateTowerModel>().towerModel;
            var phaserAttack = ballOfLight.GetAttackModel().Duplicate("ForeshadowBolt");
            var phaserWeapon = phaserAttack.GetChild<WeaponModel>().SetName("ForeshadowBolt");
            var phaserProj = phaserWeapon.projectile.SetName("ForeshadowBolt");
            var phaserDamage = phaserProj.GetDamageModel();
            var phaserBeam = phaserWeapon.GetChild<LineProjectileEmissionModel>();

            phaserAttack.RemoveBehaviors<CirclePatternModel>();
            phaserAttack.targetProvider = new TargetStrongPrioCamoModel("", false, true);
            phaserAttack.AddBehavior(phaserAttack.targetProvider);
            phaserAttack.AddBehavior(new RotateToTargetModel("", false, false, false, 0, true, false));
            phaserAttack.offsetY = 0;
            phaserAttack.attackThroughWalls = true;

            phaserWeapon.SetEject(EjectOffset);
            phaserWeapon.Rate = mainWeapon.Rate;

            phaserBeam.dontUseTowerPosition = true;
            phaserBeam.displayPath.SetName("ForeshadowBolt").assetPath = CreatePrefabReference<PhaserBeam>();
            phaserBeam.effectAtEndModel.assetId = CreatePrefabReference<PhaserParticles>();
            phaserBeam.displayLifetime = .2f;
            
            phaserProj.pierce = 100;
            phaserProj.RemoveBehaviors<DamageModifierForTagModel>();
            phaserDamage.damage = 500;
            phaserDamage.immuneBloonPropertiesOriginal = phaserDamage.immuneBloonProperties = BloonProperties.Purple;

            phaserAttack.AddWeapon(mainWeapon);
            towerModel.AddBehavior(phaserAttack);
            mainProj.GetBehavior<DamagePercentOfMaxModel>().percent.log();
        }
    }

    public class PhaserParticles : ModDisplay
    {
        private static readonly int TintColor = Shader.PropertyToID("_TintColor");
        
        public override string BaseDisplay => "3c445c563b7fc3f44aacf5cb2a79e424"; // BeamHitParticlesLvl10

        public override void ModifyDisplayNode(UnityDisplayNode node)
        {
            // foreach (var renderer in node.genericRenderers)
            // {
            //     // renderer.material.SetColor(TintColor, new Color(1, .1f, 0));
            // }
        }
}
}