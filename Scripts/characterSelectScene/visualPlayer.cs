using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class visualPlayer : MonoBehaviour
{
    public Player champion;
    public List<Skill> skills = new List<Skill>();
    public SkillTemplate[] skillTemplates;
    public Animator anim;
    // Start is called before the first frame update
    void Awake()
    {
        for(int i=0; i<champion.skills.Count; i++)
        {
            skills.Add(champion.skills[i]);
            Debug.Log("Skill : " + champion.skills[i]);
        }
    }

    public void UseSkill(int skillIndex)
    {
        Debug.Log("use Skill : " + skillIndex);
        anim.SetBool(skillTemplates[skillIndex].name, true);
    }
}
