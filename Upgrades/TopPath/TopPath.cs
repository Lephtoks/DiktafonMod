using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Emissions;
using BTD_Mod_Helper.Api.Towers;
using BTD_Mod_Helper.Extensions;
using System.Linq;
using Il2CppAssets.Scripts.Models.Powers.Mods;
using Il2CppAssets.Scripts.Simulation.Powers;
using Il2CppAssets.Scripts.Unity.Powers;
using Il2CppAssets.Scripts.Models;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2CppAssets.Scripts.Simulation.Towers.Projectiles.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Mods;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Attack;
using Il2CppAssets.Scripts.Unity;
using BTD_Mod_Helper;
using Il2CppAssets.Scripts.Models.Towers.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.TowerFilters;
using DiktafonMod.Displays;
using BTD_Mod_Helper.Api;
using Il2CppAssets.Scripts.Unity.UI_New.InGame;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Abilities;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Abilities.Behaviors;

namespace DiktafonMod.Upgrades.TopPath
{
    public class Donation : ModUpgrade<DiktafonTower>
    {
        public override int Path => TOP;

        public override int Tier => 1;

        public override int Cost => 1030;

        public override string DisplayName => "Донаты";
        public override string Portrait => "DiktafonTower100";

        public override string Description => "Диктафон оставил ссылку на донат в описании! Почти за каждое видео, которое выпускает Диктафон, подписчики донатят 1-3 банана!";
        public override string Icon => "TT1";
        public override void ApplyUpgrade(TowerModel tower)
        {
            tower.IncreaseRange(4);
            var bananas = Game.instance.model.GetTower(TowerType.BananaFarm, 1, 0, 0).GetWeapon().projectile.Duplicate();
            var bananasModel = bananas.GetBehavior<CashModel>();
            bananasModel.minimum = 1;
            bananasModel.maximum = 3;
            var bananaWeapon = tower.GetAttackModel().weapons[0].Duplicate();
            bananaWeapon.projectile = bananas;
            bananaWeapon.SetEmission(tower.GetWeapon().emission.Duplicate());
            var Attack = tower.GetAttackModel().Duplicate();
            Attack.weapons = new[] {bananaWeapon};
            tower.AddBehavior(Attack);
            tower.GetAttackModel().AddWeapon(bananaWeapon);
        }
    }
    public class Charisma : ModUpgrade<DiktafonTower>
    {
        public override int Path => TOP;

        public override int Tier => 2;

        public override int Cost => 1920;

        public override string DisplayName => "Харизма";
        public override string Icon => "TT2";
        public override string Portrait => "DiktafonTower200";

        public override string Description => "Подписчики начинают донатить 2-6 бананов!";

        public override void ApplyUpgrade(TowerModel tower)
        {
            tower.IncreaseRange(4);
            var banana = tower.GetAttackModel(1).weapons[0].projectile.GetBehavior<CashModel>();
            banana.minimum = 2;
            banana.maximum = 6;
        }
    }
    public class Money : ModUpgrade<DiktafonTower>
    {
        public override int Path => TOP;

        public override int Tier => 3;

        public override int Cost => 5290;

        public override string DisplayName => "Монетизация";
        public override string Icon => "TT3";
        public override string Portrait => "DiktafonTower300";

        public override string Description => "Теперь видео диктафона хоть что-то стоят!";

        public override void ApplyUpgrade(TowerModel tower)
        {
            tower.IncreaseRange(6);
            var banana = tower.GetAttackModel(1).weapons[0].projectile.GetBehavior<CashModel>();
            banana.minimum = 3;
            banana.maximum = 7;
            var Banana = Game.instance.model.GetTowerModel(TowerType.BananaFarm).GetWeapon().projectile;
            Banana.behaviors.ForEach(Leph.printfor);
            tower.GetAttackModel().weapons[0].projectile.AddBehavior(Banana.GetBehavior<PickupModel>().Duplicate());
            tower.GetAttackModel().weapons[0].projectile.AddBehavior(Banana.GetBehavior<CreateTextEffectModel>().Duplicate());
            CashModel cash = Banana.GetBehavior<CashModel>().Duplicate();
            cash.minimum = cash.maximum = 6;
            tower.GetAttackModel().weapons[0].projectile.AddBehavior(cash);
        }
    }
    public class Ads : ModUpgrade<DiktafonTower>
    {
        public override int Path => TOP;

        public override int Tier => 4;

        public override int Cost => 15600;

        public override string DisplayName => "Реклама";
        public override string Portrait => "DiktafonTower400";

        public override string Description => "Теперь у Диктафона есть рекламодатель, какой-то сервис по доставке оливье. Улучшает другие каналы диктафона и остальные источники дохода на 20%.";

        public override string Icon => "TT4";
        public override void ApplyUpgrade(TowerModel tower)
        {
            tower.IncreaseRange(4);
            var banana = tower.GetAttackModel(1).weapons[0].projectile.GetBehavior<CashModel>();
            banana.minimum = 8;
            banana.maximum = 13;
            var dart = tower.GetAttackModel().weapons[0].projectile.GetBehavior<CashModel>();
            dart.minimum = dart.maximum += 3;
            var Banana = Game.instance.model.GetTowerModel(TowerType.MonkeyBuccaneer, 0, 0, 3).GetBehavior<PerRoundCashBonusTowerModel>().Duplicate();
            Banana.cashPerRound = 600;
            Banana.SetName("DiktafonCash");
            tower.AddBehavior(Banana);
            tower.AddBehavior(new BananaCashIncreaseSupportModel("DiktafonCashBuff", true, 0.2f, "DiktafonCashBuff", new[] {new FilterInBaseTowerIdModel("", new[] {TowerType.BananaFarm})}, false, 0, 0, 0, "", "").ApplyBuffIcon<AdsBuff>());
            tower.AddBehavior(new BananaCashIncreaseSupportModel("DiktafonCashBuff", true, 0.2f, "DiktafonCashBuff", new[] {new FilterInBaseTowerIdModel("", new[] {GetTowerModel<DiktafonTower>().GetBaseId()})}, true, 1, 0, 0, "", "").ApplyBuffIcon<AdsBuff>());
            tower.AddBehavior(new BananaCashIncreaseSupportModel("DiktafonCashBuff", true, 0.2f, "DiktafonCashBuff", new[] {new FilterInBaseTowerIdModel("", new[] {TowerType.MonkeyBuccaneer})}, true, 0, 0, 3, "", "").ApplyBuffIcon<AdsBuff>());
        }
    }
    public class Community : ModUpgrade<DiktafonTower>
    {
        public override int Path => TOP;

        public override int Tier => 5;

        public override int Cost => 98500;

        public override string DisplayName => "Комьюнити";

        public override string Description => "Только этого и достаточно...";
        public override string Portrait => "DiktafonTower500";
        public override string Icon => "TT5";

        public override void ApplyUpgrade(TowerModel tower)
        {
            tower.IncreaseRange(6);

            tower.RemoveBehaviors<BananaCashIncreaseSupportModel>();

            tower.GetBehavior<PerRoundCashBonusTowerModel>().cashPerRound = 1080;

            var banana = tower.GetAttackModel(1).weapons[0].projectile.GetBehavior<CashModel>();
            banana.minimum = 20;
            banana.maximum = 30;

            var dart = tower.GetAttackModel().weapons[0].projectile.GetBehavior<CashModel>();
            dart.minimum = dart.maximum *= 2;

            tower.AddBehavior(new BananaCashIncreaseSupportModel("DiktafonCash2Buff", true, 0.5f, "DiktafonCash2Buff", new[] {new FilterInBaseTowerIdModel("", new[] {TowerType.BananaFarm})}, false, 0, 0, 0, "", "").ApplyBuffIcon<AdsBuff>());
            tower.AddBehavior(new BananaCashIncreaseSupportModel("DiktafonCash2Buff", true, 0.5f, "DiktafonCash2Buff", new[] {new FilterInBaseTowerIdModel("", new[] {GetTowerModel<DiktafonTower>().GetBaseId()})}, true, 1, 0, 0, "", "").ApplyBuffIcon<AdsBuff>());
            tower.AddBehavior(new BananaCashIncreaseSupportModel("DiktafonCash2Buff", true, 0.5f, "DiktafonCash2Buff", new[] {new FilterInBaseTowerIdModel("", new[] {TowerType.MonkeyBuccaneer})}, true, 0, 0, 3, "", "").ApplyBuffIcon<AdsBuff>());

            var totem = Game.instance.model.GetTowerModel(TowerType.ParagonPowerTotem).Duplicate();
            
            var SpawnAbility = Game.instance.model.GetParagonTower(TowerType.EngineerMonkey).GetAbility().Duplicate();
            var ForeshadowAbility = new AbilityModel("ParagonTotemSpawn", "Paragon totem", "Spawns paragon totem for free", 0, 0, Game.instance.model.GetGeraldoItemWithName(TowerType.ParagonPowerTotem).defaultIcon, 300, null, false, false, null, 0, 0, 1, false, false);
            ForeshadowAbility.AddBehavior(new ActivateAttackCreateTowerPlacementModel("", new[] {SpawnAbility.GetBehavior<ActivateAttackCreateTowerPlacementModel>().attacks[0]}));
            tower.AddBehavior(ForeshadowAbility);
            var proj = ForeshadowAbility.GetBehavior<ActivateAttackCreateTowerPlacementModel>().attacks[0].weapons[0].projectile;
            proj.RemoveBehavior(proj.GetBehavior<CreateTowersInSequenceModel>());
            proj.AddBehavior(new CreateTowerModel("", totem, 1, true, false, false, true, false));
            tower.cost = 0;
            }
    }
}