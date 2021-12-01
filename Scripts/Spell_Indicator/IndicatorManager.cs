using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorManager : MonoBehaviour
{
    // 캐릭터 위주 스킬 범위 및 스킬 방향 조정
    public Entity owner;

    // 1. 스킬 가능 범위
    public CastingRange skillRange;

    public List<SkillEffectIndicator> skillsIndicator = new List<SkillEffectIndicator>();
    public List<SkillEffectIndicator> itemsIndicator = new List<SkillEffectIndicator>();
    //public float SkillCastRange = 0f;

    public void Start()
    {
        if (!owner)
            owner = GetComponent<Entity>();


        UpdateSkillIndicator();
        UpdateItemIndicator();
    }
    public void UpdateSkillIndicator()
    {
        int skillIndex = 0;

        foreach (SkillTemplate skills in owner.skillTemplates)
        {
            if (skills.effectIndicator && skills.isUseEffectIndicator)
            {
                SkillEffectIndicator effectIndicator = Instantiate(skills.effectIndicator);
                effectIndicator.transform.SetParent(transform);

                effectIndicator.owner = owner;
                effectIndicator.skill = skills;

                effectIndicator.setSkillIndex(skillIndex);

                effectIndicator.createdEffectIndicator();

                skillsIndicator.Add(effectIndicator);
            }
            skillIndex++;
        }
    }
    public void UpdateItemIndicator()
    {
        if (owner.team == Team.Player)
        {
            Player players = (Player)owner;

            foreach (Item items in players.inventory)
            {
                if (items.skill.template.effectIndicator)
                {
                    SkillEffectIndicator effectIndicator = items.skill.template.effectIndicator;
                    effectIndicator.owner = owner;
                    effectIndicator.skill = items.skill.template;

                    itemsIndicator.Add(effectIndicator);
                }
            }
        }
    }

    public void showSkillRange(bool isCasting)
    {
        if (skillRange)
            skillRange.SkillCastingRange(isCasting);

        if (!isCasting) {
            foreach (SkillEffectIndicator skillEffectIndicator in skillsIndicator) { skillEffectIndicator.HideSkillDirection(); }
            foreach (SkillEffectIndicator skillEffectIndicator in itemsIndicator) { skillEffectIndicator.HideSkillDirection(); }         
        }
    }

    public void showSkillRange(bool isCasting, Skill skill, float size = 0f)
    {
        if (skillRange)
            skillRange.SkillCastingRange(isCasting, skill, size);

       // Debug.Log("Skill : " + skill.name);
        for(int i=0; i< skillsIndicator.Count; i++)
        {
          //  Debug.Log("skillsIndicator : " + skillsIndicator[i].skill);

            if (skillsIndicator[i].skill == skill.template)
            {
                skillsIndicator[i].DrawIndicator(skill, Vector3.one, Vector3.one, size);
            }
        }
    }

    public void UpdateSkillIndicator(Skill skill, Vector3 cursorPos, float size = 0f)
    {
      //  Debug.Log("Indicator Manager Call");
        for(int i = 0; i < skillsIndicator.Count; i++)
        {
            if(skill.template == skillsIndicator[i].skill)
            {
                skillsIndicator[i].DrawIndicator(skill, owner.transform.position, cursorPos, size);
            }
        }
    }
    public void UpdateItemSkillIndicator(Skill skill, Vector3 cursorPos, float size = 0f)
    {
        for (int i = 0; i < skillsIndicator.Count; i++)
        {
            if (skill.template == skillsIndicator[i].skill)
            {
                skillsIndicator[i].DrawIndicator(skill, owner.transform.position, cursorPos, size);
            }
        }
    }
    public void UpdateItemIndicator(Skill skill, Vector3 cursorPos)
    {
        if (owner.team == Team.Player) 
        {
            Player players = (Player)owner;

            foreach (Item items in players.inventory) 
            {
                if (items.skill.template.effectIndicator)
                {
                    SkillEffectIndicator effectIndicator = items.skill.template.effectIndicator;
                    effectIndicator.owner = owner;
                    effectIndicator.skill = items.skill.template;

                    itemsIndicator.Add(effectIndicator);
                }
            }
        }
    }




}
