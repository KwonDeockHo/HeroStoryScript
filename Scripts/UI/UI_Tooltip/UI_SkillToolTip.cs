using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UI_SkillToolTip : MonoBehaviour
{
    public Skill skill;
    Entity owner;

    public Image skill_image;
    public string hotkey;
    public Text skill_name;
    public Text skill_cooltime;
    public Text skill_manaCost;
    public Text skill_tooltip;

    public GameObject tooltip_Header;
    public GameObject tooltip_contents;

    // Start is called before the first frame update
    void Start()
    {
        owner = Player.player;

        skill_image.sprite = skill.image;
        skill_name.text = hotkey+ skill.skillname;
        
        skill_cooltime.text = skill.cooldown.ToString()+"초";
        skill_manaCost.text = skill.manaCosts.ToString();
        skill_tooltip.text = skill.ToolTip(owner);

        ToolTipStatusUpdate(owner);
        ToolTipAreaLineCount();
    }
    public void ToolTipStatusUpdate(Entity owner)
    {
        var skillCoolTime = skill.cooldown - (skill.cooldown * owner.cooldown);
        var skillManaCost = skill.manaCosts - (skill.manaCosts * owner.manaConsumptions) ;

        skill_cooltime.text = skillCoolTime.ToString() + "초";
        skill_manaCost.text = skillManaCost.ToString();
    }
    public void ToolTipAreaLineCount() // ToolTip Height 조절
    {
        var rectTr = tooltip_contents.GetComponent<RectTransform>();
        var headerTr = tooltip_Header.GetComponent<RectTransform>();


        rectTr.anchorMin = new Vector2(0.5f, 1f);
        rectTr.anchorMax = new Vector2(0.5f, 1f);
        rectTr.pivot = new Vector2(0.5f, 0.5f);

        var _original_Y = rectTr.rect.y;
        var _before_height = rectTr.rect.height;


        Canvas.ForceUpdateCanvases();
        int cnt = skill_tooltip.cachedTextGenerator.lines.Count;

        var _after_height = 20 + (cnt * skill_tooltip.fontSize);
        rectTr.sizeDelta = new Vector2(rectTr.rect.width, _after_height);
        var _add_Y = (_after_height - _before_height) / 2;
        rectTr.localPosition = new Vector2(0, _original_Y + (_add_Y * (-1)));

        headerTr.localPosition = new Vector2(0, rectTr.rect.height + 80f);
    }

}
