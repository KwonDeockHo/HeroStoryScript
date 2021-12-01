using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class UI_ItemInfo : MonoBehaviour
{
    public UI_Store uStore;

    public List<UI_ItemSlot> slots = new List<UI_ItemSlot>();
    public Player player;
    public Item info_item;
    public Transform tooltipPosition;


    [Header("=== Upper Item List ===")]
    public GameObject upperSlot_Prefab;
    public RectTransform upper_Parent;

    public List<UI_ItemSlot> upperItemSlotList = new List<UI_ItemSlot>();



    [Header("===Store Inventory===")]
    //public List<Item> store_inventorySlot = new List<Item>();
    public Transform store_inventoryParent;
    public GameObject store_inven_slot;
    public Sprite emptyFrame;

    [Header("===UI===")]
    public Button buyBtn;
    public Button sellBtn;




    [Header("====== Merge Slot Object ======")]
    public List<Item> mergeList = new List<Item>();
    //setActive true false object
    public GameObject commonObject;
    public GameObject mergeOneObject;
    public GameObject mergeTwoObject;
    public GameObject mergeThreeObject;



    [Header("=== Merge Item Slot ===")]

    public GameObject item_slot_Higher;
    public GameObject item_slot;


    [Header("=== Common Item ===")]
    public RectTransform common_higher;

    [Header("=== Rare One Item ===")]
    public RectTransform rare_one_higher;
    public RectTransform[] rare_one_merge;

    [Header("=== Rare Two Item ===")]
    public RectTransform rare_two_higher;
    public RectTransform[] rare_two_merge;

    [Header("=== Legendary Item ===")]
    public RectTransform legendary_top;
    public RectTransform[] legendary_higher;
    public RectTransform[] legendary_Left_merge;
    public RectTransform[] legendary_Right_merge;

    [Header("=== MergeObject List ===")]
    //merge items color init
    public List<UI_ItemSlot> mergeALLobjectList = new List<UI_ItemSlot>();


    [Header("===Target Item Infomation===")]
    public Image item_backImage;
    public Image item_image;
    public Text item_name;
    public Text item_Buyprice;
    public Text item_sellPrice;

    public Text item_tooltip;
    public GameObject tooltip_content;


    private bool isInit = false;
    private int index=0;
    bool[] isHaving = new bool[6];

    // Start is called before the first frame update
    void Start()
    {
        if (!Player.player)
            return;
        player = Player.player;


        //클릭한 슬롯의 정보를 info창에 띄우는 Action 함수
        uStore.onSlotClick = ViewItemInfo;


        //스토어 인벤에 슬롯 생성
        Init_ItemInfo_Inventory();

        //슬롯에 아이템 할당
        for (int i = 0; i < slots.Count; i++)
        {
            var item = player.inventory[i];

            Set_Image_ItemInfo_Inventory(item, i);
        }

        isInit = true;
    }


    void Update() //Start에서 플레이어가 할당안되면 계속 반복
    {
        if (!isInit)
        {
            Start();
            return;
        }

    }


    public void ItemUpdate()//아이템 사고 팔면 인벤토리 업데이트
    {
        Init_ItemInfo_Inventory();

        for (int i = 0; i < slots.Count; i++)
        {
            var item = player.inventory[i];

            Set_Image_ItemInfo_Inventory(item, i);
            Get_ItemPrice(info_item);
        }
    }


    //스토어인벤토리#01
    //플레이어의 인벤토리 정보를 스토어의 인벤토리로 복사.
    public void Init_ItemInfo_Inventory() //스토어 인벤토리 슬롯 생성 
    {
        player = Player.player;
        if (!player) return;

        var invenAmount = player.inventory.Count;

        if (slots.Count < invenAmount)
        {
            for (int i = 0; i < invenAmount; i++)
            {
                GameObject inven_slot = Instantiate(store_inven_slot);
                inven_slot.transform.SetParent(store_inventoryParent);
                inven_slot.transform.SetAsLastSibling();
                inven_slot.transform.localScale = store_inventoryParent.localScale;

                slots.Add(inven_slot.GetComponent<UI_ItemSlot>());
            }
        }
        else if (slots.Count > invenAmount)
        {
            for (int i = invenAmount; i < slots.Count; i++)
            {
                var _slot = slots[i].gameObject;
                slots.RemoveAt(i);
                Destroy(_slot);
            }
        }
    }

    //스토어인벤토리#02
    //스토어 인벤토리에 생성된 슬롯에 이미지 할당 
    public void Set_Image_ItemInfo_Inventory(Item _item, int i)
    {
        if (_item.valid)
        {
            slots[i].frame.sprite = _item.frame;
            slots[i].frame.enabled = true;

            slots[i].backImage.sprite = _item.backImage;
            slots[i].backImage.enabled = true;

            slots[i].image.sprite = _item.image;
            slots[i].image.enabled = true;

            slots[i].name = _item.name;

            //slots[i].frame.rectTransform.sizeDelta = new Vector2(75, 75);
            //slots[i].backImage.rectTransform.sizeDelta = new Vector2(70, 70);
            //slots[i].image.rectTransform.sizeDelta = new Vector2(60, 60);


            var item_slot_Button = slots[i].GetComponent<Button>();
            item_slot_Button.interactable = true;
            item_slot_Button.onClick.AddListener(() => uStore.OnClickSlot(slots[i].GetComponent<UI_ItemSlot>()));


            slots[i].Set_InventoryItem_Infomation_Mini(_item);

            slots[i].tooltip.item = _item;
            slots[i].tooltip.createPos = tooltipPosition;            
            slots[i].tooltip.text = _item.ToolTip();
            slots[i].tooltip.enabled = true;

        }
        else
        {
            slots[i].tooltip.enabled = false;

            slots[i].frame.enabled = true;
            slots[i].backImage.sprite = emptyFrame;
            slots[i].image.enabled = false;

            slots[i].name = "Empty";

            var item_slot_Button = slots[i].GetComponent<Button>();
            item_slot_Button.interactable = false;
        }
    }



    //선택한 아이템 정보 #01
    //스토어에서 클릭한 아이템이 우측하단 Infomation Panel 에 뜨도록
    public void ViewItemInfo(Item _item)
    {
        this.info_item = _item;

        mergeList.Clear(); //UI_ItemSlot에서 
        mergeALLobjectList.Clear();


        item_backImage.sprite = _item.backImage;
        item_backImage.enabled = true;

        item_image.sprite = _item.image;
        item_image.enabled = true;


        if (item_name)
        {
            item_name.text = _item.itemname;

            Color color;

            if (_item.itemGrade == ItemGrade.Common)
            {

                ColorUtility.TryParseHtmlString("#9DA2A5", out color);
                item_name.color = color;
            }
            else if (_item.itemGrade == ItemGrade.Rare)
            {
                ColorUtility.TryParseHtmlString("#FB37FB", out color);
                item_name.color = color;
            }
            else if (_item.itemGrade == ItemGrade.Legendary)
            {
                ColorUtility.TryParseHtmlString("#FBF74B", out color);
                item_name.color = color;
            }
        }

        if (item_Buyprice) item_Buyprice.text = _item.buyPrice.ToString();
        if (item_tooltip) item_tooltip.text = _item.ToolTip();

        View_ItemSlot(_item);
        Select_Item_UpperList(_item);
        ToolTipAreaIineCount();
        Get_ItemPrice(_item);

    } 

    public void Get_ItemPrice(Item _item)
    {
        
       for(int i=0; i<mergeALLobjectList.Count; i++)
       {
            if (i == 0) //최상위 아이템
            {
                mergeALLobjectList[i].price.text = player.GetPriceForBuyItem(_item.template).ToString();
            }
            else //차상위의 아이템
            {
                for(int j=0; j<_item.GetMergeItemCount(); j++)
                {
                    var _template = _item.GetMergeTemplate(j);

                    if (mergeALLobjectList[i].name == _template.name)
                    {
                        mergeALLobjectList[i].price.text = player.GetPriceForBuyItem(_template).ToString();
                    }
                }
            }
       }
    }

    //선택한 아이템 정보 #02
    //선택한 아이템의 등급에 따라 하위 아이템을 전부 보여줌 
    public void View_ItemSlot(Item _item)
    {

        //선택된 아이템의 등급이 Common
        if (_item.itemGrade == ItemGrade.Common)
        {
            Delete_ItemTree_Slot(common_higher);

            Create_HigherItem_Slot(_item, common_higher);
            mergeList.Add(_item);

            commonObject.SetActive(true);
            mergeOneObject.SetActive(false);
            mergeTwoObject.SetActive(false);
            mergeThreeObject.SetActive(false);

        }
        //선택된 아이템의 등급이 Rare
        else if (_item.itemGrade == ItemGrade.Rare)
        {
            if (_item.GetMergeItemCount() == 1)
            {
                Delete_ItemTree_Slot(rare_one_higher);

                Create_HigherItem_Slot(_item, rare_one_higher);
                mergeList.Add(_item);

                for (int i = 0; i < rare_one_merge.Length; i++)
                {
                    Delete_ItemTree_Slot(rare_one_merge[i]);

                    Item merge = new Item(_item.GetMergeTemplate(i));
                    Create_ItemTree_Slot(merge, rare_one_merge[i]);
                    mergeList.Add(merge);
                }

                commonObject.SetActive(false);
                mergeOneObject.SetActive(true);
                mergeTwoObject.SetActive(false);
                mergeThreeObject.SetActive(false);
            }
            else if (_item.GetMergeItemCount() == 2)
            {
                Delete_ItemTree_Slot(rare_two_higher);

                Create_HigherItem_Slot(_item, rare_two_higher);
                mergeList.Add(_item);

                for (int i = 0; i < rare_two_merge.Length; i++)
                {
                    Delete_ItemTree_Slot(rare_two_merge[i]);

                    Item merge = new Item(_item.GetMergeTemplate(i));
                    Create_ItemTree_Slot(merge, rare_two_merge[i]);
                    mergeList.Add(merge);
                }

                commonObject.SetActive(false);
                mergeOneObject.SetActive(false);
                mergeTwoObject.SetActive(true);
                mergeThreeObject.SetActive(false);

            }
        }
        //선택된 아이템의 등급이 Legendary
        else if (_item.itemGrade == ItemGrade.Legendary)
        {
            Delete_ItemTree_Slot(legendary_top);

            Create_HigherItem_Slot(_item, legendary_top);
            mergeList.Add(_item);

            for (int i = 0; i < legendary_higher.Length; i++)
            {
                Delete_ItemTree_Slot(legendary_higher[i]);

                Item merge = new Item(_item.GetMergeTemplate(i));
                Create_ItemTree_Slot(merge, legendary_higher[i]);
                mergeList.Add(merge);
            }


            for (int i = 0; i < legendary_Left_merge.Length; i++)
            {
                Delete_ItemTree_Slot(legendary_Left_merge[i]);

                Item merge = new Item(_item.GetMergeTemplate(0).mergeTemplate[i]);
                Create_ItemTree_Slot(merge, legendary_Left_merge[i]);
                mergeList.Add(merge);
            }

            for (int i = 0; i < legendary_Right_merge.Length; i++)
            {
                Delete_ItemTree_Slot(legendary_Right_merge[i]);

                Item merge = new Item(_item.GetMergeTemplate(1).mergeTemplate[i]);
                Create_ItemTree_Slot(merge, legendary_Right_merge[i]);
                mergeList.Add(merge);
            }

            commonObject.SetActive(false);
            mergeOneObject.SetActive(false);
            mergeTwoObject.SetActive(false);
            mergeThreeObject.SetActive(true);

        }
    }

    //최상위 아이템 생성
    public void Create_HigherItem_Slot(Item _item, RectTransform _parent) //선택된 아이템 슬롯 생성
    {
        GameObject obj = Instantiate(item_slot_Higher);
        obj.transform.SetParent(_parent.transform);
        obj.transform.SetAsLastSibling();

        var  rtobj = obj.GetComponent<RectTransform>();
        rtobj.localPosition = new Vector3(0, 0, 0);
        rtobj.localScale = _parent.transform.localScale;

        var slot = obj.GetComponent<UI_ItemSlot>();
        slot.ItemSlotSetImage(_item);
        mergeALLobjectList.Add(slot);

        slot.tooltip.item = _item;
        slot.tooltip.createPos = tooltipPosition;
        slot.tooltip.text = _item.ToolTip();
        slot.tooltip.enabled = true;



    }

    //차상위 아이템들 생성
    public void Create_ItemTree_Slot(Item _item, RectTransform _parent) //선택된 아이템의 하위 슬롯 생성
    {
        GameObject obj = Instantiate(item_slot);
        obj.transform.SetParent(_parent.transform);
        obj.transform.SetAsLastSibling();

        var rtobj = obj.GetComponent<RectTransform>();
        rtobj.localPosition = new Vector3(0, 0, 0);
        rtobj.localScale = _parent.transform.localScale;

        var slot = obj.GetComponent<UI_ItemSlot>();
        slot.ItemSlotSetImage(_item,_parent.transform);
        mergeALLobjectList.Add(slot);


        slot.tooltip.item = _item;
        slot.tooltip.createPos = tooltipPosition;
        slot.tooltip.text = _item.ToolTip();
        slot.tooltip.enabled = true;


    }


    public void Delete_ItemTree_Slot(RectTransform _parent) //새로운 아이템 선택시 기존 슬롯 제거
    {
        for (int i = 0; i < _parent.transform.childCount; i++)
        {
            Destroy(_parent.transform.GetChild(i).gameObject);
        }
        _parent.transform.DetachChildren();
    }




    public void Select_Item_UpperList(Item _item)//선택한 아이템의 상위 아이템을 보여줌
    {
        DeleteUpperSlot();

        // _item의 상위 아이템을 List로 담음
        var uppers = UI_ItemList.self.GetUpperItems(_item);

        for (int i = 0; i < uppers.Length; i++)
        {
            //InGame upper Item List자리에 슬롯을 생성함
            GameObject upper_slot = Instantiate(upperSlot_Prefab);
            upper_slot.transform.SetParent(upper_Parent);
            upper_slot.transform.SetAsLastSibling();
            upper_slot.transform.localScale = upper_Parent.transform.localScale;

            var up = upper_slot.GetComponent<UI_ItemSlot>();
            up.ItemSlotSetImage(uppers[i], upper_Parent);
            upperItemSlotList.Add(up);

            up.tooltip.item = up.itemList;
            up.tooltip.createPos = tooltipPosition;
            up.tooltip.text = up.itemList.ToolTip();
            up.tooltip.enabled = true;


            //슬롯을 클릭하면 정보를 출력하도록 button을 붙임
            var upper_btn = upper_slot.GetComponent<Button>();
            upper_btn.onClick.AddListener(() => uStore.OnClickSlot(upper_btn.GetComponent<UI_ItemSlot>()));
        }

    }

    public void DeleteUpperSlot()//Upper List 보여주기전에 슬롯 지움
    {
        upperItemSlotList.Clear();

        if (upper_Parent.transform.childCount > 0)
        {
            //upper Parent의 자식 오브젝트를 삭제해야함
            for (int i = 0; i < upper_Parent.transform.childCount; i++)
            {
                var slot_delete = upper_Parent.transform.GetChild(i).GetComponent<UI_ItemSlot>();

                Destroy(slot_delete.gameObject);
            }
            //upper_Parent.transform.DetachChildren();
        }
    }


    public void MergePanelColorClear(UI_ItemSlot target) //Info에서 선택한 아이템이 바뀌면 컬러 초기화
    {
        //greatParent = Panel
        //parent = Panel 아래의 등급별 아이템이 등록되는 부모 오브젝트
        var greatParent = target.transform.parent.transform.parent;
        var parent = target.transform.parent;

        for (int i = 0; i < greatParent.transform.childCount; i++)
        { 
            //레어 2 아이템 선택시 i값에 따라 higher, merge one, merge two 아이템의 게임오브젝트
            var obj = greatParent.GetChild(i).GetChild(0).gameObject.GetComponent<UI_ItemSlot>();

            //선택한 오브젝트가 아니거나 부모의 이름이 다르면
            //나머지 오브젝트는 알파 값 초기화
            if (target.name != obj.name || target.transform.parent.name != obj.transform.parent.name)
            {

                var _wrapper = obj.wrapper;
                var _frame = obj.frame;

                Color wrapper_color = _wrapper.color;
                Color frame_color = _frame.color;

                wrapper_color.a = 0f;
                
                frame_color.r = 70 / 255f;
                frame_color.g = 70 / 255f;
                frame_color.b = 70 / 255f;

                _wrapper.color = wrapper_color;
                _frame.color = frame_color;
            }
        }
    }


    public void ToolTipAreaIineCount() // ToolTip Height 조절
    {
        var rectTr = tooltip_content.GetComponent<RectTransform>();

        rectTr.sizeDelta = new Vector2(rectTr.rect.width, 700);

        Canvas.ForceUpdateCanvases();

        int cnt = item_tooltip.cachedTextGenerator.lines.Count;

        int height = (cnt * 26);

        if(height > 230)
            rectTr.sizeDelta = new Vector2(rectTr.rect.width, height);
        else
            rectTr.sizeDelta = new Vector2(rectTr.rect.width, 230);
    }



    //아이템 구매
    public void ItemBuy()
    {
        if (!player)
            player = Player.player;

        if (!info_item.template) return;
        int price = player.GetPriceForBuyItem(info_item.template);
        if (player.gold >= price)
        {
            if (info_item.valid)
            {
                if (UI_Store.self && UI_Store.self.canUseStore) 
                {
                    player.InventoryAddAmount(info_item.template, 1, true);
                    UI_Store.self.PlaySound_Buy();

                    //player.gold -= info_item.buyPrice;
                    
                    ItemUpdate();
                }
            }

        }
    }


    //아이템 판매
    public void ItemSell()
    {
        if (!player)
            player = Player.player;

        if (!info_item.template) return;
        if (info_item.valid)
        {
            if (UI_Store.self && UI_Store.self.canUseStore)
            {
                player.InventoryDeleteAmount(info_item.template, 1);
                UI_Store.self.PlaySound_Sell();

                player.gold += info_item.sellPrice;

                ItemUpdate();
            }
        }


    }


}
