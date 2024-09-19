using Il2CppAssets.Scripts.Models.Towers;
using BTD_Mod_Helper.Api.Towers;
using BTD_Mod_Helper.Extensions;
using DiktafonMod.Displays;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Abilities;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Abilities.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Attack;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppAssets.Scripts.Unity;
using Il2CppAssets.Scripts.Models;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Attack.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Emissions.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2Cpp;
using Il2CppAssets.Scripts.Unity.Towers.Behaviors;
using Il2CppSystem.Dynamic.Utils;
using Il2CppAssets.Scripts.Models.GenericBehaviors;
using Il2CppAssets.Scripts.Simulation.Behaviors;
using Il2CppSystem;
using Il2CppAssets.Scripts.Simulation.Towers.Projectiles.Behaviors;
using Il2CppAssets.Scripts.Simulation.Towers;
using DiktafonMod.Towers;
using Il2CppAssets.Scripts.Models.Towers.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Emissions;
using DiktafonMod.Displays.MoonLord;
using BTD_Mod_Helper.Api;

namespace DiktafonMod.Upgrades.MiddlePath
{
    public class IsaakTears : ModUpgrade<DiktafonTower>
    {
        public override int Path => Middle;

        public override int Tier => 1;

        public override int Cost => 540;

        public override string DisplayName => "Слезы Исаака";

        public override string Description => "Удваивает скорострельность.";

        public override string Icon => "MT1";
        public override string Portrait => "DiktafonTower010";

        public override void ApplyUpgrade(TowerModel tower)
        {
            tower.GetAttackModel().weapons[0].projectile.ApplyDisplay<TearDisplay>();
            Leph.MulRate(tower, 0.5f);
        }
    }
    public class TNT : ModUpgrade<DiktafonTower>
    {
        public override int Path => Middle;

        public override int Tier => 2;

        public override int Cost => 330;

        public override string DisplayName => "Блок динамита";
        public override string Icon => "MT2";

        public override string Description => "Начинает стрелять динамитом, позволяя пробивать свинец и наносить урон по площади.";

        public override string Portrait => "DiktafonTower020";
        public override void ApplyUpgrade(TowerModel tower)
        {
            var cannon = Game.instance.model.GetTowerModel(TowerType.BombShooter).GetAttackModel().weapons[0].projectile;
            
            var behs = tower.GetAttackModel().weapons[0].projectile.childDependants;

            tower.GetAttackModel().weapons[0].projectile = cannon.Duplicate();

            tower.GetAttackModel().weapons[0].projectile.AddChildDependants(behs.ToArray());
        }
    }
    public class EndPoem : ModUpgrade<DiktafonTower>
    {
        public override int Path => Middle;

        public override int Tier => 3;

        public override int Cost => 9800;

        public override string DisplayName => "Хагги вагги";
        public override string Portrait => "DiktafonTower030";
        public override string Icon => "MT3";

        public override string Description => "Обезьяны очень боятся стоять рядом. Их скорострельность увеличивается на 25%. Стакается только 3 раза";

        public override void ApplyUpgrade(TowerModel tower)
        {
            var buff = Game.instance.model.GetTowerModel(TowerType.NinjaMonkey, 0, 3).GetBehavior<SupportShinobiTacticsModel>().Duplicate();
            var HuggyBuffInstance = ModContent.GetInstance<HuggyBuff>();
            buff.buffIconName = HuggyBuffInstance.BuffIconName;
            buff.buffLocsName = HuggyBuffInstance.BuffLocsName;
            /*
            rate = 2
            tears = 0.5
            rate = 1 / (1.25 / rate) = rate / 1.25
            */
            buff.maxStacks = buff.maxStackSize = 3;
            buff.multiplier /= 1.25f;
            buff.SetName("HuggyWaggy");
            buff.mutatorId = "HuggyWaggyBuff";
            buff.filters = null;
            tower.AddBehavior(buff);
        }
    }
    public class MoonLord : ModUpgrade<DiktafonTower>
    {
        public override int Path => Middle;

        public override int Tier => 4;

        public override int Cost => 16700;

        public override string DisplayName => "Террария";
        public override string Portrait => "DiktafonTower040";
        public override string Icon => "MT4";

        public override string Description => "Хах, один раз поиграл в террарию? Ничего! Сюда все равно запихнем!\nСоздает 3 сферы, которые выпускают голубые лучи в самые прочные шары\nДобавляет способность, которая вызывает 3 сферы, наносящие 40 урона + 0.05% MOAB'а в секунду";

        public override void ApplyUpgrade(TowerModel tower)
        {
            var UpgradeAbility = new AbilityModel("MoonLordSpheres", "Moonlord Spheres", "Spawns Moonlord's spheres", 0, 0, GetSpriteReference(Icon), 45, null, false, false, null, 0, 0, 3, false, false);
            var adora = Game.instance.model.GetHeroWithNameAndLevel(TowerType.Adora, 10);
            var spawnBallOfLight = adora.GetAbilities()[2].GetBehavior<AbilityCreateTowerModel>();

            for (int i = 1; i < 4; i++) {
                var Spawn = spawnBallOfLight.Duplicate();
                var ball = Spawn.towerModel;
                var attack = ball.GetAttackModel();
                var weapon = attack.weapons[0];
                var beam = weapon.GetChild<LineProjectileEmissionModel>();

                ball.GetBehavior<OrbitingTowerModel>().radius = 10 + i*5;
                ball.GetBehavior<OrbitingTowerModel>().rotationDegreesPerFrame = i;
                ball.GetBehavior<OrbitingTowerModel>().rotationDegreesPerFrame *= (float) Math.Pow(-1, i);

                ball.ApplyDisplay<MoonLordBall>();
                beam.displayPath.SetName("MoonLordBeam").assetPath = CreatePrefabReference<MoonLordBeam>();
                beam.effectAtEndModel.assetId = CreatePrefabReference<MoonLordParticles>();
                beam.displayLifetime = 0.1f;

                weapon.Rate = 0.025f;
                weapon.projectile.GetDamageModel().damage = 1f;
                weapon.projectile.AddBehavior(new DamagePercentOfMaxModel("DiktafonMod-MoonLordBall", 0.0000125f, new[] {"Moabs"}, false));

                UpgradeAbility.AddBehavior(Spawn);
            }
            
            tower.AddBehavior(UpgradeAbility);
        }
    }
    public class Foreshadow : ModUpgrade<DiktafonTower>
    {
        public override int Path => Middle;

        public override int Tier => 5;

        public override int Cost => 56400;

        public override string DisplayName => "Знамение";
        public override string Icon => "foreshadowtower";
        public override string Portrait => "DiktafonTower050";

        public override string Description => "Хотя бы здесь он поиграет в Mindustry...\nПозволяет размещать рельсотрон \"Знамение\". Знамение раз в 5 секунд выстреливает снарядом из кинетического сплава, наносящий 1350 урона, а также 5% от жизней MOAB'а.";

        public override void ApplyUpgrade(TowerModel tower)
        {
            var SpawnAbility = Game.instance.model.GetParagonTower(TowerType.EngineerMonkey).GetAbility().Duplicate();
            var ForeshadowAbility = new AbilityModel("", "Foreshadow", "Spawns foreshadow", 0, 0, GetSpriteReference(Icon), 60, null, false, false, null, 0, 0, 9999999, false, false);
            ForeshadowAbility.AddBehavior(new ActivateAttackCreateTowerPlacementModel("", new[] {SpawnAbility.GetBehavior<ActivateAttackCreateTowerPlacementModel>().attacks[0]}));
            tower.AddBehavior(ForeshadowAbility);
            var proj = ForeshadowAbility.GetBehavior<ActivateAttackCreateTowerPlacementModel>().attacks[0].weapons[0].projectile;
            proj.RemoveBehavior(proj.GetBehavior<CreateTowersInSequenceModel>());
            proj.AddBehavior(new CreateTowerModel("", GetTowerModel<ForeshadowTower>(), 1, true, true, false, true, false));

        }
    }
}