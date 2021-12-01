using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Text.RegularExpressions;
using System.Reflection;

public class UI_ShowToolTip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Skill skill;
    
    [Header("=== Item ===")]
    public Item item;

    [Header("=== Skill ===")]
    public Entity owner;
    public string hotKey;

    [Header("=== Status ===")]
    public GameObject statusType;
    public Text statValue;
    [TextArea(1, 30)] public string headerText = "";
    [TextArea(1, 30)] public string overviewText = "";

    [Header("=== Create Tooltip ===")]
    public GameObject tooltipPrefab;
    public Transform createPos;
    public float tooltipWidth = 250;

    [TextArea(1, 30)] public string text = "";

    GameObject current;

    RectTransform info_panel;
    RectTransform option_panel;

    void CreateToolTip()
    {
        if (current) Destroy(current);
        current = Instantiate(tooltipPrefab, transform.position, Quaternion.identity);
                
        var skillTooltip = current.GetComponent<UI_SkillToolTip>();        
        if (skillTooltip) 
        { 
            if (Player.player.indicatorManager)
                Player.player.indicatorManager.showSkillRange(true, skill);

            skillTooltip.skill = skill;
            skillTooltip.hotkey = "["+hotKey+"] ";
            skillTooltip.ToolTipStatusUpdate(Player.player);

            current.transform.position = createPos.position;
        }


        var itemTooltip = current.GetComponent<UI_ItemToolTip>();
        if (itemTooltip) 
        {
            var Createslot_name = transform.parent.name;

            //툴팁 생성되는 위치에 따라 판매가격 혹은 구매가격 띄움
            if(Createslot_name == "Store_inventory" || Createslot_name == "UI_InventorySlot(Clone)")             
                itemTooltip.price = item.sellPrice.ToString();
            else
                itemTooltip.price = item.buyPrice.ToString();

            info_panel = itemTooltip.tooltip_info_panel.GetComponent<RectTransform>();
            option_panel = itemTooltip.tooltip_option_panel.GetComponent<RectTransform>();

            itemTooltip.item = item;

            if (Createslot_name == "UI_InventorySlot(Clone)")
            {
                itemTooltip.state = 1;
                current.transform.position = createPos.position;        
            }
            else
            {
                itemTooltip.state = 2;
                Tooltip_getPosition(current, info_panel, option_panel);
            }
        }

        var tooltip = current.GetComponent<UI_Tooltip>();
        if (tooltip)
        {
            tooltip.index.text =item.name;
        }


        var statTooltip = current.GetComponent<UI_Stat_Tooltip>();
        if (statTooltip)
        {
            statTooltip.statType.text = headerText;
            if (statTooltip.stat_OverView) statTooltip.statOverView.text = overviewText;

            owner = Player.player;
            var self = UI_Discription_Text.self;
            statTooltip.statDiscription.text = self.Get_StatusName(statusType.name, statValue.text, owner);

            current.transform.position = createPos.position;
        }



        current.transform.SetParent(createPos ? createPos : transform.root, true);
        current.transform.SetAsLastSibling();
    }

    //툴팁 생성 위치 설정
    public void Tooltip_getPosition(GameObject current, RectTransform info_panel, RectTransform option_panel)
    {
        //슬롯 프리팹이 누구인지 스크립트로 판단
        var slot = gameObject.GetComponent<UI_ItemSlot>();

        
        if (!slot)
        {
            //inventorySlot 생성 시 지정된 위치에서 생성
            current.transform.position = createPos.position;
        }
        else
        {
            if(slot.transform.parent.name == "StoreSlots")
            {
                var rectTr = current.GetComponent<RectTransform>();
                
                //마우스보다 우하단에 들어가도록 width 500
                rectTr.localPosition += new Vector3(info_panel.rect.width * 0.5f + info_panel.rect.width * 0.05f, info_panel.rect.height * (-1));               
            }
            else if (slot.transform.parent.name == "Store_inventory")
            {
                var rectTr = current.GetComponent<RectTransform>();

                //마우스보다 우상단에 들어가도록 height 80     
                rectTr.localPosition += new Vector3(info_panel.rect.width * 0.5f + info_panel.rect.width * 0.05f, option_panel.rect.height * 0.5f);  
            }
            else if (slot.transform.parent.name == "upperItemList")
            {
                var rectTr = current.GetComponent<RectTransform>();

                //마우스보다 좌하단에 들어가도록
                rectTr.localPosition += new Vector3(info_panel.rect.width * -0.5f + info_panel.rect.width * -0.05f, info_panel.rect.height * (-1));
            }
            else // 아이템트리에서 하위템들
            {
                var rectTr = current.GetComponent<RectTransform>();
                
                //마우스보다 좌하단에 들어가도록
                rectTr.localPosition += new Vector3(info_panel.rect.width * -0.5f + info_panel.rect.width * -0.05f, info_panel.rect.height * (-1));
            }         
        }
    }


    void ShowToolTip(float delay)
    {
        Invoke("CreateToolTip", delay);
    }

    void DestroyToolTip()
    {
        CancelInvoke("CreateToolTip");

        Destroy(current);
    }

    public void OnPointerEnter(PointerEventData d)
    {
        ShowToolTip(0.1f);
    }

    public void OnPointerExit(PointerEventData d)
    {
        DestroyToolTip();
        //Debug.Log("Show Tool Tip : "+ skill.nam);
        //if (!skill )
        //{
        //    if (Player.player.indicatorManager)
        //        Player.player.indicatorManager.showSkillRange(false, skill);
        //}
    }

    void OnDisable()
    {
        DestroyToolTip();
    }

    void OnDestroy()
    {
        DestroyToolTip();
    }
}
