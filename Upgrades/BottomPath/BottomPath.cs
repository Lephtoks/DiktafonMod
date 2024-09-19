using Il2CppAssets.Scripts.Models.Towers;
using BTD_Mod_Helper.Api.Towers;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2CppAssets.Scripts.Models.ContestedTerritory;
using Il2Cpp;
using Il2CppAssets.Scripts.Models.Towers.Filters;
using Il2CppAssets.Scripts.Unity;
using Il2CppAssets.Scripts.Models.Towers.Behaviors;
using Il2CppAssets.Scripts.Models;
using System;
using Il2CppAssets.Scripts.Models.Towers.TowerFilters;
using Il2CppAssets.Scripts.Unity.Towers.TowerFilters;
using Il2CppAssets.Scripts.Models.TowerSets;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppAssets.Scripts.Unity.Towers.Behaviors;
using DiktafonMod.Displays;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Attack;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Emissions;
using Harmony;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Abilities;

namespace DiktafonMod.Upgrades.BottomPath
{
    public class Symphony : ModUpgrade<DiktafonTower>
    {
        public override int Path => BOTTOM;
        public override int Tier => 1;
        public override int Cost => 90;
        public override string DisplayName => "Симфония";

        public override string Description => "Диктафон может нанести урон по камуфляжу, но все еще его не видит. + 1 пробитие.";

        public override string Portrait => "DiktafonTower001";
        public override string Icon => "T1";

        public override void ApplyUpgrade(TowerModel tower)
        {
            tower.GetAttackModel().weapons[0].GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
            tower.GetAttackModel().weapons[0].projectile.pierce += 1;
        }
    }
    public class Olivier : ModUpgrade<DiktafonTower>
    {
        public override int Path => BOTTOM;
        public override int Tier => 5;
        public override int Cost => 22300;
        public override string DisplayName => "Ты оливье!";

        public override string Description => "Бесконечное (почти) пробитие. Обмазывает большинство видов шаров оливье, замедляя их и нанося урон.";

        public override string Portrait => "DiktafonTower005";
        public override string Icon => "T5";

        public override void ApplyUpgrade(TowerModel tower)
        {   
            var glue = Game.instance.model.GetTowerModel(TowerType.GlueGunner, 5, 0, 0).GetWeapon().projectile;
            
            var dart = tower.GetAttackModel().weapons[0].projectile;

            glue.behaviors.ForEach(b => dart.AddBehavior(b));

            dart.pierce = 999999;
        }
    }
    public class Pills : ModUpgrade<DiktafonTower>
    {
        public override int Path => BOTTOM;
        public override int Tier => 2;
        public override int Cost => 700;
        public override string DisplayName => "Таблетки, сушащие горло";

        public override string Description => "Диктафон наносит больше урона, в зависимости от того, сколько шаров он лопнул.\ndmg = log10(TOTAL_POPS + 10) - 1.5";

        public override string Portrait => "DiktafonTower002";
        public override string Icon => "T2";
        public override void ApplyUpgrade(TowerModel tower)
        {
            tower.GetAttackModel().weapons[0].projectile.AddBehavior(new DamageModel("DiktafonMod-LogarithmicCounter", 0, 9999999, true, false, false, BloonProperties.None, BloonProperties.None, false));   
        }   
    }
    public class Friendship : ModUpgrade<DiktafonTower>
    {
        public override int Path => BOTTOM;
        public override int Tier => 3;
        public override int Cost => 3070;
        public override string DisplayName => "Дружить близко";

        public override string Description => "Шары начинают группироваться в плотные группы. Обезьяны с атакой по площади увеличивают свой лимит поражаемых целей в 5 раз. Остальные обезьяны получают 5 пробивания.";

        public override string Portrait => "DiktafonTower003";
        public override string Icon => "T3";

        public override void ApplyUpgrade(TowerModel tower)
        {
            var UpgradeAbility = new AbilityModel("DiktafonModSwitch", "Switch", "Switch", 0, 0, GetSpriteReference(Icon), 1, null, false, false, null, 0, 0, 999999, false, false, dontShowStacked:true);
            tower.AddBehavior(UpgradeAbility);
            void AddOr(TowerFilterModel filter) {
                PiercePercentageSupportModel beh = new PiercePercentageSupportModel("", false, 500, "DiktafonMod:FriendshipPierce", new[] {filter}, false, "DiktafonModFriendshipPierce", "T1");
                beh.ApplyBuffIcon<FriendshipBuff>();
                tower.AddBehavior(beh);
            }
            void AddOrT(TowerFilterModel filter, int a, int b, int c) {
                PiercePercentageSupportModel beh = new PiercePercentageSupportModel("", false, 500, "DiktafonMod:FriendshipPierce", new[] {filter, new FilterInTowerTiersModel("", a, 5, b, 5, c, 5)}, false, "DiktafonModFriendshipPierce", "T1");
                beh.ApplyBuffIcon<FriendshipBuff>();
                tower.AddBehavior(beh);
            }
            AddOr(new FilterInBaseTowerIdModel("", new[] { TowerType.IceMonkey }));
            AddOr(new FilterInBaseTowerIdModel("", new[] { TowerType.BombShooter }));
            AddOr(new FilterInBaseTowerIdModel("", new[] { TowerType.MortarMonkey }));
            AddOr(new FilterInBaseTowerIdModel("", new[] { TowerType.Alchemist }));
            AddOrT(new FilterInBaseTowerIdModel("", new[] { TowerType.MonkeySub }), 4, 0, 0);
            


            var beh = new PierceSupportModel("", false, 5, "DiktafonMod:FriendshipPierce", null, false, "DiktafonModFriendshipPierce", "Tear");
            tower.AddBehavior(beh);
            beh.ApplyBuffIcon<FriendshipBuff>();
            tower.AddBehavior(new CtEmptyModModel("DiktafonMod-Friendship"));
        }
    }
    public class Monkey : ModUpgrade<DiktafonTower>
    {
        public override int Path => BOTTOM;
        public override int Tier => 4;
        public override int Cost => 4000;
        public override string DisplayName => "Обезьяны";

        public override string Description => "Поддержка смешных обезьянок дает тебе пятерной выстрел, который получает самонаводящиеся свойства, а также очень высокое пробитие!";

        public override string Portrait => "DiktafonTower004";
        public override string Icon => "T4";

        public override void ApplyUpgrade(TowerModel tower)
        {
            tower.IncreaseRange(9);
            var dart = tower.GetWeapon().projectile;
            dart.GetBehavior<TravelStraitModel>().Speed *= 3;
            dart.AddBehavior(Game.instance.model.GetTowerModel(TowerType.NinjaMonkey, 0, 0, 2).GetBehavior<AttackModel>().weapons[0].projectile.GetBehavior<TrackTargetWithinTimeModel>().Duplicate());
            dart.pierce += 50;
            tower.GetWeapons().ForEach(w => {
                var emission = w.emission.Cast<ArcEmissionModel>();
                emission.Count = 5;
                emission.angle = 30;
                });
            Game.instance.model.GetTowerModel(TowerType.NinjaMonkey, 0, 0, 2).GetBehavior<AttackModel>().weapons[0].projectile.GetBehavior<TrackTargetWithinTimeModel>();
        }
    }
}