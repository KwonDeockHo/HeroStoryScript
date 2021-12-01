using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UI_Tooltip : MonoBehaviour
{
    public GameObject tooltip_content;
    public Text index;    


    void Start()
    {
        ToolTipAreaIineCount();
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
        int cnt = index.cachedTextGenerator.lines.Count;

        var _after_height = (cnt * 26);


        rectTr.sizeDelta = new Vector2(rectTr.rect.width, _after_height);


        var _add_Y = (_after_height - _before_height) / 2;


        rectTr.localPosition = new Vector2(0, _original_Y + (_add_Y * (-1)));



    }
}