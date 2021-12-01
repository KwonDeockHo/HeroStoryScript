using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UI_Buff_Tooltip : MonoBehaviour
{
    public Image buff_Image;
    public Text buff_name;
    public Text buff_tooltip;
    public Text buff_caster;
    public GameObject tooltip_content;

    public Entity target;

    void Start()
    {
        if (target)
        {
            if (target.team == Team.Enemy)
            {
                EnemyToolTipAreaIineCount();
            }
            else
            {
                ToolTipAreaIineCount();
            }
        }
    }


    public void ToolTipAreaIineCount() // ToolTip Height 조절
    {
        var rectTr = tooltip_content.GetComponent<RectTransform>();
        
        rectTr.anchorMin = new Vector2(0.5f, 1f);
        rectTr.anchorMax = new Vector2(0.5f, 1f);
        rectTr.pivot = new Vector2(0.5f, 0.5f);
        
        var _original_Y = rectTr.rect.y;
        var _before_height = rectTr.rect.height;


        Canvas.ForceUpdateCanvases();
        int cnt = buff_tooltip.cachedTextGenerator.lines.Count;

        var _after_height = 20 + (cnt * buff_tooltip.fontSize);


        rectTr.sizeDelta = new Vector2(rectTr.rect.width, _after_height);


        var _add_Y = (_after_height - _before_height) / 2;


        rectTr.localPosition = new Vector2(0, _original_Y + (_add_Y * (-1))+5);

        var obj = gameObject.GetComponent<RectTransform>();
        obj.localPosition = new Vector2(obj.rect.x, rectTr.rect.height+50);

    }


    public void EnemyToolTipAreaIineCount() // ToolTip Height 조절
    {
        var rectTr = tooltip_content.GetComponent<RectTransform>();

        rectTr.anchorMin = new Vector2(0.5f, 1f);
        rectTr.anchorMax = new Vector2(0.5f, 1f);
        rectTr.pivot = new Vector2(0.5f, 0.5f);

        var _original_Y = rectTr.rect.y;
        var _before_height = rectTr.rect.height;


        Canvas.ForceUpdateCanvases();
        int cnt = buff_tooltip.cachedTextGenerator.lines.Count;

        var _after_height = 50 + (cnt * 25);


        rectTr.sizeDelta = new Vector2(rectTr.rect.width, _after_height);


        var _add_Y = (_after_height - _before_height) / 2;


        rectTr.localPosition = new Vector2(0, _original_Y + (_add_Y * (-1)) + 5);

        var obj = gameObject.GetComponent<RectTransform>();
        obj.localPosition = new Vector2(obj.rect.x / 2, (rectTr.rect.height + 100) / 2 * (-1));

    }
}
