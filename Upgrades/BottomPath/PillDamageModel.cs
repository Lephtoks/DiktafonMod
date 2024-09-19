using Il2Cpp;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;

class PillDamageModel : DamageModel
{
    public PillDamageModel(string name, float damage, float maxDamage, bool distributeToChildren, bool overrideDistributeBlocker, bool createPopEffect, BloonProperties immuneBloonProperties, BloonProperties immuneBloonPropertiesOriginal, bool ignoreImmunityDestroy) : base(name, damage, maxDamage, distributeToChildren, overrideDistributeBlocker, createPopEffect, immuneBloonProperties, immuneBloonPropertiesOriginal, ignoreImmunityDestroy) {}
    public PillDamageModel(DamageModel damageModel) : base(damageModel.name, damageModel.damage, damageModel.maxDamage, damageModel.distributeToChildren, damageModel.overrideDistributeBlocker, damageModel.createPopEffect, damageModel.immuneBloonProperties, damageModel.immuneBloonPropertiesOriginal, damageModel.ignoreImmunityDestroy) {}
    public new float damage 
       {
           get { return 100; }
           set { base.damage = value; }
       }
}