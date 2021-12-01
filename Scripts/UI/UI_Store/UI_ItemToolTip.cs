using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UI_ItemToolTip : MonoBehaviour
{
    public UI_ItemToolTip self;

    public Item item;

    public Image backImage;
    public Image item_image;
    public Text item_grade;
    public Text item_name;
    public Text item_Price;
    public Text item_tooltip;

    public GameObject tooltip_contents;
    public GameObject tooltip_info_panel;
    public GameObject tooltip_option_panel;
    public string price;

    public GameObject header;
    [HideInInspector] public int state = 0;
    void Awake()
    {
        if (!self)
            self = this;
        //else
         //   Destroy(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        if (backImage) backImage.sprite = item.backImage;
        if (item_image) item_image.sprite = item.image;
        if (item_grade) item_grade.text = item.itemGrade.ToString()+" ";
        if (item_name) 
        { 
            item_name.text = item.itemname;

            Color color;

            if (item.itemGrade == ItemGrade.Common)
            {
                
                ColorUtility.TryParseHtmlString("#9DA2A5", out color);
                item_name.color = color;
            }
            else if (item.itemGrade == ItemGrade.Rare)
            {
                ColorUtility.TryParseHtmlString("#FB37FB", out color);
                item_name.color = color;
            }
            else if (item.itemGrade == ItemGrade.Legendary)
            {
                ColorUtility.TryParseHtmlString("#FBF74B", out color);
                item_name.color = color;
            }
        }
        if (item_Price) item_Price.text = price;
        if (item_tooltip) item_tooltip.text = item.ToolTip();


        if (state == 1)
            TooltipInventoryLineCount();        
        else if(state == 2)
            ToolTipAreaLineCount();
    }


    public void TooltipInventoryLineCount()
    {
        var rectTr = tooltip_contents.GetComponent<RectTransform>();
        var headerTr = header.GetComponent<RectTransform>();

        rectTr.anchorMin = new Vector2(0.5f, 1f);
        rectTr.anchorMax = new Vector2(0.5f, 1f);
        rectTr.pivot = new Vector2(0.5f, 0.5f);

        var _original_Y = rectTr.rect.y;
        var _before_height = rectTr.rect.height;


        Canvas.ForceUpdateCanvases();
        int cnt = item_tooltip.cachedTextGenerator.lines.Count;

        var _after_height = (cnt * 26);
        rectTr.sizeDelta = new Vector2(rectTr.rect.width, _after_height);
        var _add_Y = (_after_height - _before_height) / 2;
        rectTr.localPosition = new Vector2(0, _original_Y + (_add_Y * (-1)));

        headerTr.localPosition = new Vector2(0, rectTr.rect.height);
    }
    public void ToolTipAreaLineCount()
    {
        var rectTr = tooltip_contents.GetComponent<RectTransform>();

        rectTr.anchorMin = new Vector2(0.5f, 1f);
        rectTr.anchorMax = new Vector2(0.5f, 1f);
        rectTr.pivot = new Vector2(0.5f, 0.5f);

        var _original_Y = rectTr.rect.y;
        var _before_height = rectTr.rect.height;


        Canvas.ForceUpdateCanvases();
        int cnt = item_tooltip.cachedTextGenerator.lines.Count;

        var _after_height = (cnt * 26);
        rectTr.sizeDelta = new Vector2(rectTr.rect.width, _after_height);
        var _add_Y = (_after_height - _before_height) / 2;


        rectTr.localPosition = new Vector2(0, _original_Y + (_add_Y * (-1)));

    }

}
