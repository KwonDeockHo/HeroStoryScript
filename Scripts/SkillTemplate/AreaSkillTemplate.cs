﻿using UnityEngine;
using UnityEditor;
using System.Text;

[CreateAssetMenu(menuName = "Skill/Area Damage", order = 999)]
public class AreaSkillTemplate : SkillTemplate
{
    [Header("Skill Damage")]
    public LevelBasedFloat AttackDamage = new LevelBasedFloat { baseValue = 1 };
    public LevelBasedFloat ADRatio = new LevelBasedFloat { baseValue = 0 };
    public LevelBasedFloat AbilityPower = new LevelBasedFloat { baseValue = 0 };
    public LevelBasedFloat APRatio = new LevelBasedFloat { baseValue = 0 };
    public LevelBasedFloat casterHealthMaxPerDamage = new LevelBasedFloat { baseValue = 0 };
    public LevelBasedFloat casterCurrentHealthPerDamage = new LevelBasedFloat { baseValue = 0 };
    public LevelBasedFloat casterLostHealthPerDamage = new LevelBasedFloat { baseValue = 0 };
    public LevelBasedFloat targetHealthMaxPerDamage = new LevelBasedFloat { baseValue = 0 };
    public LevelBasedFloat targetCurrentHealthPerDamage = new LevelBasedFloat { baseValue = 0 };
    public LevelBasedFloat targetLostHealthPerDamage = new LevelBasedFloat { baseValue = 0 };
    public LevelBasedFloat trueDamage = new LevelBasedFloat { baseValue = 0 };

    public LevelBasedFloat casterManaMaxPerAbilityPower = new LevelBasedFloat { baseValue = 0 };
    public LevelBasedFloat casterCurrentManaPerAbilityPower = new LevelBasedFloat { baseValue = 0 };
    public LevelBasedFloat casterLostManaPerAbilityPower = new LevelBasedFloat { baseValue = 0 };

    [Header("Projectile")]
    public AreaSkillEffect projectile;
    public bool isFollowProjectileIndicator = false;

    public EffectMount projtileEffectMount;

    public override bool CorrectedTarget(Entity caster, Entity target)
    {
        if (target == null) return !haveToTarget;
        if (target.team != caster.team) return true;
        return false;
    }

    public override bool CheckTarget(Entity caster)
    {
        return (caster.target != null || !haveToTarget) &&
               caster.CanAttack(caster.target);
    }

    public override bool CheckDistance(Entity caster, int skillLevel, out Vector3 destination)
    {
        var skillCastRange = addUseCastRange ? castRange.Get(skillLevel) + caster.addCastRange : castRange.Get(skillLevel);

        if (immediatelyCasting)
            destination = caster.transform.position;
        else if(canUseSkillFarAway)
        {
            Vector3 vec = caster.targetPos - caster.transform.position;
            caster.targetPos = caster.transform.position + vec.normalized * skillCastRange;
            destination = caster.targetPos;
        }
        else
            destination = caster.targetPos;
        return Vector3.Distance(caster.transform.position, destination) <= skillCastRange;
    }

    public override void Apply(Entity caster, int skillLevel)
    {
        SpawnVisualEffect(caster, skillLevel);
        Vector3 targetPos;
        Quaternion targetRot;
        if (immediatelyCasting) { targetPos = caster.transform.position; targetRot = Quaternion.identity; }
        else if (projtileEffectMount != EffectMount.None)
        {
            targetPos = caster.GetEffectMount(projtileEffectMount).position;

            targetRot = caster.transform.rotation;

        }
        else { targetPos = caster.targetPos; targetRot = Quaternion.identity; }

            //GameObject go = Instantiate(projectile.gameObject, targetPos, Quaternion.identity);
        GameObject go = Instantiate(projectile.gameObject, targetPos, targetRot);

        AreaSkillEffect effect = go.GetComponent<AreaSkillEffect>();
        if (effect.yisZero) effect.transform.SetYPosition(0);

        effect.target = caster.target;
        effect.caster = caster;
        float ad = AttackDamage.Get(skillLevel) + (caster.attackDamage * ADRatio.Get(skillLevel));
        ad += caster.healthMax * casterHealthMaxPerDamage.Get(skillLevel);
        ad += caster.health * casterCurrentHealthPerDamage.Get(skillLevel);
        ad += (caster.healthMax - caster.health) * casterLostHealthPerDamage.Get(skillLevel);
        float ap = AbilityPower.Get(skillLevel) + (caster.abilityPower * APRatio.Get(skillLevel));
        ap += caster.manaMax * casterManaMaxPerAbilityPower.Get(skillLevel);
        ap += caster.mana * casterCurrentManaPerAbilityPower.Get(skillLevel);
        ap += (caster.manaMax - caster.mana) * casterLostManaPerAbilityPower.Get(skillLevel);

        float td = trueDamage.Get(skillLevel);
        effect.attackDamage = ad;
        effect.abilityPower = ap;
        effect.trueDamage = td;
        effect.currentHealthPerDamage = targetCurrentHealthPerDamage.Get(skillLevel);
        effect.healthMaxPerDamage = targetHealthMaxPerDamage.Get(skillLevel);
        effect.lostHealthPerDamage = targetLostHealthPerDamage.Get(skillLevel);
        
        if (buffSkillTemplateHittedTarget) 
        {
            effect.buffSkillTemplate = buffSkillTemplateHittedTarget;
            effect.buffSkillLevel = skillLevel;
        }
        if (buffSkillTemplateSelf)
        {
            if (caster)
                caster.AddOrRefreshBuff(new Buff(buffSkillTemplateSelf, skillLevel, caster));
        }
    }

    public override void RepeatApply(Entity caster, int skillLevel)
    {
        Apply(caster, skillLevel);
    }
    public override void SpawnVisualEffect(Entity caster, int skillLevel)
    {
        if (visualEffect_Invoke != null)
        {
            GameObject go = Instantiate(visualEffect_Invoke.gameObject, caster.GetEffectMount(effectMount).position, caster.GetEffectMount(effectMount).rotation);
            var _effect = go.GetComponent<VisualSkillEffect>();
            if (_effect.followEffectMount) go.transform.parent = caster.GetEffectMount(effectMount);
            if (effectMount == EffectMount.BothHands)
            {
                GameObject goRight = Instantiate(visualEffect_Invoke.gameObject, caster.GetEffectMount(EffectMount.RightHand).position, caster.GetEffectMount(EffectMount.RightHand).rotation);
                var _effectRight = goRight.GetComponent<VisualSkillEffect>();
                if (_effectRight.followEffectMount) goRight.transform.parent = caster.GetEffectMount(EffectMount.RightHand);
            }
        }
        else
            Debug.LogWarning("visualEffect_Invoke is Null");
    }

    public override string ToolTip(int skillLevel, bool showRequirements = false)
    {
        StringBuilder tip = new StringBuilder(base.ToolTip(skillLevel, showRequirements));

        
        tip.Replace("{ATTACKDAMAGE}", AttackDamage.Get(skillLevel).ToString());
        tip.Replace("{ADRATIO_PER}", (ADRatio.Get(skillLevel) * 100).ToString());

        
        tip.Replace("{ABILITYPOWER}", AbilityPower.Get(skillLevel).ToString());
        tip.Replace("{APRATIO_PER}", (APRatio.Get(skillLevel) * 100).ToString());

        tip.Replace("{TRUEDAMAGE}", trueDamage.Get(skillLevel).ToString());

        tip.Replace("{TARGETCURRENTHEALTHPERDAMAGE}", (targetCurrentHealthPerDamage.Get(skillLevel) * 100).ToString());
        tip.Replace("{TARGETHEALTHMAXPERDAMAGE}", (targetHealthMaxPerDamage.Get(skillLevel) * 100).ToString());
        tip.Replace("{TARGETLOSTHEALTHPERDAMAGE}", (targetLostHealthPerDamage.Get(skillLevel) * 100).ToString());

        tip.Replace("{CASTERHEALTHMAXPERDAMAGE_PER}", (casterHealthMaxPerDamage.Get(skillLevel) * 100).ToString());
        tip.Replace("{CASTERCURRENTHEALTHPERDAMAGE_PER}", (casterCurrentHealthPerDamage.Get(skillLevel) * 100).ToString());
        tip.Replace("{CASTERLOSTHEALTHPERDAMAGE_PER}", (casterLostHealthPerDamage.Get(skillLevel) * 100).ToString());

        tip.Replace("{CASTERMANAMAXPERABILITYPOWER_PER}", (casterManaMaxPerAbilityPower.Get(skillLevel) * 100).ToString());
        tip.Replace("{CASTERCURRENTMANAPERABILITYPOWER_PER}", (casterCurrentManaPerAbilityPower.Get(skillLevel) * 100).ToString());
        tip.Replace("{CASTERLOSTMANAPERABILITYPOWER_PER}", (casterLostManaPerAbilityPower.Get(skillLevel) * 100).ToString());

        return tip.ToString();
    }

    public override string ToolTip(Entity caster, int skillLevel, bool showRequirements = false)
    {
        StringBuilder tip = new StringBuilder(base.ToolTip(caster, skillLevel, showRequirements));

        tip.Replace("{TOTAL_DAMAGE}", (AttackDamage.Get(skillLevel) + (caster.attackDamage * ADRatio.Get(skillLevel)) +
                                     AbilityPower.Get(skillLevel) + (caster.abilityPower * APRatio.Get(skillLevel)) + trueDamage.Get(skillLevel)).ToString());

        tip.Replace("{TOTAL_AD}", (AttackDamage.Get(skillLevel) + (caster.attackDamage * ADRatio.Get(skillLevel)) + trueDamage.Get(skillLevel)).ToString());
        tip.Replace("{ADRATIO}", (caster.attackDamage * ADRatio.Get(skillLevel)).ToString());

        tip.Replace("{TOTAL_AP}", (AbilityPower.Get(skillLevel) + (caster.abilityPower * APRatio.Get(skillLevel)) + trueDamage.Get(skillLevel)).ToString());
        tip.Replace("{APRATIO}", (caster.abilityPower * APRatio.Get(skillLevel)).ToString());

        tip.Replace("{CASTERHEALTHMAXPERDAMAGE}", (caster.healthMax * casterHealthMaxPerDamage.Get(skillLevel)).ToString());
        tip.Replace("{CASTERCURRENTHEALTHPERDAMAGE}", (caster.health * casterCurrentHealthPerDamage.Get(skillLevel)).ToString());
        tip.Replace("{CASTERLOSTHEALTHPERDAMAGE}", ((caster.healthMax - caster.health) * casterLostHealthPerDamage.Get(skillLevel)).ToString());
        
        tip.Replace("{CASTERMANAMAXPERABILITYPOWER_PER}", (casterManaMaxPerAbilityPower.Get(skillLevel)).ToString());
        tip.Replace("{CASTERCURRENTMANAPERABILITYPOWER_PER}", (casterCurrentManaPerAbilityPower.Get(skillLevel)).ToString());
        tip.Replace("{CASTERLOSTMANAPERABILITYPOWER_PER}", (casterLostManaPerAbilityPower.Get(skillLevel)).ToString());
        return tip.ToString();
    }

    public override Team GetTeamThisSkillTarget(Entity caster)
    {
        return caster.team == Team.Enemy ? Team.Player : Team.Enemy;
    }
}