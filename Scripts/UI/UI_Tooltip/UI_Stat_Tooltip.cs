using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Stat_Tooltip : MonoBehaviour
{
    public GameObject tooltip_content;
    public Text statType;
    public Text statOverView;
    public Text statDiscription;

    public GameObject stat_Header;
    public GameObject stat_OverView;
    public GameObject stat_Discription;


    void Start()
    {
        if (stat_Header && stat_OverView && stat_Discription)
            TooltipAreaLineCount();

    }

    public void TooltipAreaLineCount()
    {
        var headerTr = stat_Header.GetComponent<RectTransform>();
        var overviewTr = stat_OverView.GetComponent<RectTransform>();
        var rectTr = stat_Discription.GetComponent<RectTransform>();

        rectTr.anchorMin = new Vector2(0.5f, 1f);
        rectTr.anchorMax = new Vector2(0.5f, 1f);
        rectTr.pivot = new Vector2(0.5f, 0.5f);

        var _original_Y = rectTr.rect.y;
        var _before_height = rectTr.rect.height;


        Canvas.ForceUpdateCanvases();
        int cnt = statDiscription.cachedTextGenerator.lines.Count;

        var _after_height = (cnt * 35);
        rectTr.sizeDelta = new Vector2(rectTr.rect.width, _after_height);
        var _add_Y = (_after_height - _before_height) / 2;
        rectTr.localPosition = new Vector2(0, _original_Y + (_add_Y * -1));

        headerTr.localPosition = new Vector2(0, rectTr.rect.height + 100);
        overviewTr.localPosition = new Vector2(0, rectTr.rect.height + 50);

    }

    public void TooltipArea()
    {
        var headerTr = stat_Header.GetComponent<RectTransform>();
        var rectTr = stat_Discription.GetComponent<RectTransform>();

        rectTr.anchorMin = new Vector2(0.5f, 1f);
        rectTr.anchorMax = new Vector2(0.5f, 1f);
        rectTr.pivot = new Vector2(0.5f, 0.5f);

        var _original_Y = rectTr.rect.y;
        var _before_height = rectTr.rect.height;


        Canvas.ForceUpdateCanvases();
        int cnt = statDiscription.cachedTextGenerator.lines.Count;

        var _after_height = (cnt * 35);
        rectTr.sizeDelta = new Vector2(rectTr.rect.width, _after_height);
        var _add_Y = (_after_height - _before_height) / 2;
        rectTr.localPosition = new Vector2(0, _original_Y + (_add_Y * -1));

        headerTr.localPosition = new Vector2(0, rectTr.rect.height + 50);
    }
}
