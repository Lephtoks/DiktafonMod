using MelonLoader;
using BTD_Mod_Helper;
using DiktafonMod;
using Il2CppAssets.Scripts.Simulation.Towers.Weapons;
using Il2CppAssets.Scripts.Simulation;
using Il2CppAssets.Scripts.Simulation.Towers;
using Il2CppAssets.Scripts.Unity;
using Il2CppAssets.Scripts.Unity.UI_New.InGame;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Simulation.Towers.Projectiles;
using Il2CppAssets.Scripts.Simulation.Objects;
using Il2CppAssets.Scripts.Models;
using BTD_Mod_Helper.Api.Towers;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2CppAssets.Scripts.Models.ContestedTerritory;
using Il2CppAssets.Scripts.Models.Towers.Mods;
using Il2CppAssets.Scripts.Simulation.Towers.Mutators;
using Il2CppAssets.Scripts.Simulation.Towers.Behaviors.Abilities.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Behaviors;
using Il2CppAssets.Scripts.Models.GenericBehaviors;
using Il2CppAssets.Scripts.Simulation.Bloons;
using UnityEngine.Playables;
using System.Threading.Tasks;
using System;
using Il2CppSystem.Collections.Generic;
using BTD_Mod_Helper.Api;
using Il2CppAssets.Scripts.Unity.Audio;
using UnityEngine;
using Il2CppNinjaKiwi.Common.ResourceUtils;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using BTD_Mod_Helper.Api.ModOptions;
using Il2CppAssets.Scripts.Simulation.Towers.Behaviors.Attack;
using Il2CppAssets.Scripts.Simulation.Towers.Behaviors.Abilities;

[assembly: MelonInfo(typeof(DiktafonMod.DiktafonMod), ModHelperData.Name, ModHelperData.Version, ModHelperData.RepoOwner)]
[assembly: MelonGame("Ninja Kiwi", "BloonsTD6")]

namespace DiktafonMod {

    public class DiktafonMod : BloonsTD6Mod
    {
        public static AudioClip? ForeshadowShot;
        public static readonly ModSettingInt ForeshadowShotVolume = new ModSettingInt(50) {slider=true,min=0,max=100};
        public override void OnAudioFactoryStart(AudioFactory audioFactory)
        {
            AssetBundle bundle = ModContent.GetBundle<DiktafonMod>("diktafonassets");
            ForeshadowShot = bundle.LoadAsset("railgun.ogg").Cast<AudioClip>();
            ForeshadowShot.Play();
        }
        public override void OnProjectileCreated(Projectile projectile, Entity entity, Model modelToUse)
        {
            foreach (CtEmptyModModel model in projectile.projectileModel.GetBehaviors<CtEmptyModModel>())
            {
                if (model.name == "DiktafonMod-Foreshadow") {
                    ForeshadowShot.Play(volume: ForeshadowShotVolume / 100);
                    break;
                }
            }
            foreach (DamageModel model in projectile.projectileModel.GetBehaviors<DamageModel>())
            {
                if (model.name == "DamageModel_DiktafonMod-LogarithmicCounter") {
                    long total_pops = 0;
                    InGame.instance.GetTowers().ForEach(tower => {
                        if (tower.model.name.StartsWith("DiktafonMod-DiktafonTower")) {
                            total_pops += tower.damageDealt;
                        }
                    });
                    model.damage = (float)Math.Log10(total_pops + 10) - 1.5f;
                }
            }
        }

        List<Bloon> Bloons = new List<Bloon>();
        int tick = 0;
        bool Friendship = false;
        public override void OnAbilityCast(Ability ability)
        {
            ability.abilityModel.name.log();
            ability.abilityModel.name = "DiktafonModSwitch";
            Friendship = !Friendship;
            if (!Friendship) {
                InGame.instance.GetBloons().ForEach(bloon => bloon.IsPaused = false);
            }
        }
        public override void OnFixedUpdate()
        {
            if (!Friendship) return;
            try {
                if (InGame.instance.GetSimulation().AreRoundsActive()) {
                    tick++;
                    if (tick == 60) {
                        foreach (Bloon bloon in Bloons)
                        {
                            bloon.IsPaused = false;
                        }
                        Bloons.Clear();
                        tick = 0;
                    }
                }
            } catch (NullReferenceException) {}
        }
        public override void OnMatchEnd()
        {
            Friendship = false;
        }
        public override void OnRoundStart()
        {
            Bloons.Clear();
            tick = 0;

        }
        public override void OnRoundEnd()
        {
            Bloons.Clear();
        }

        public override void OnBloonCreated(Bloon bloon)
        {
            if (!Friendship) return;
            Bloons.Add(bloon);
            bloon.IsPaused = true;
        }
    }
}