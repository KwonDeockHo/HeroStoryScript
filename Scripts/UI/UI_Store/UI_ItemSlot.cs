using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_ItemSlot : MonoBehaviour
{
    [HideInInspector]
    public Item itemList;

    public Image image;
    public Image wrapper;
    public Image backImage;
    public Image frame;
    public Text price;
    public Image emptyFrame;

    public UI_ShowToolTip tooltip;
    //from ItemInfo
    private Button buyBtn;
    private Button sellBtn;
    //from ItemInfo changeColor

    //from Store
    private GameObject itemSlot_Parent;
    private GameObject inventory_Parent;

    private Image _wrapper;
    private Image _frame;
    private Color wrapper_color;
    private Color frame_color;

    //UI_ItemInfo에서 Store Inventory 아이템 클릭 시 버튼 활성 유무 파악
    public void Set_InventoryItem_Infomation_Mini(Item _item)
    {
        //인벤토리에서 클릭한 아이템 정보 저장
        this.itemList = _item;


        //구매,판매버튼 등등 활성화
        OtherScripUI_Setting();
    }

    //UI_Store Slot 이미지 할당
    public void ItemSlotSetImage(Item _item, Transform targetTr)//슬롯에 아이템 이미지 할당
    {
        this.itemList = _item;

        // 아이템이 존재하면 이미지 활성화
        if (itemList.valid)
        {

            if (image) image.enabled = true;
            if (backImage) backImage.enabled = true;
            if (frame) frame.enabled = true;
            if (price) price.enabled = true;

            gameObject.name = _item.name;



            if (targetTr.name == "StoreSlots")
            {
                if (wrapper) wrapper.sprite = _item.wrapper;
                //if (wrapper) wrapper.rectTransform.sizeDelta = new Vector2(85, 105);

                if (frame) frame.sprite = _item.frame;
                //if (frame) frame.rectTransform.sizeDelta = new Vector2(75, 75);

                if (backImage) backImage.sprite = _item.backImage;
                //if (backImage) backImage.rectTransform.sizeDelta = new Vector2(70, 70);

                if (image) image.sprite = _item.image;
                //if (image) image.rectTransform.sizeDelta = new Vector2(60, 60);

                if (price) price.text = _item.buyPrice.ToString();

            }
            else if (targetTr.name == "Store_inventory")
            {
                if (backImage) backImage.sprite = _item.backImage;
                //if (backImage) backImage.rectTransform.sizeDelta = new Vector2(70, 70);

                if (image) image.sprite = _item.image;
                //if (image) image.rectTransform.sizeDelta = new Vector2(60, 60);
            }
            else if (targetTr.name == "upperItemList")
            {
                if (backImage) backImage.sprite = _item.backImage;
                //if (backImage) backImage.rectTransform.sizeDelta = new Vector2(40, 40);

                if (image) image.sprite = _item.image;
                //if (image) image.rectTransform.sizeDelta = new Vector2(30, 30);
            }
            else if (targetTr.name == "Higher")
            {
                if (wrapper) wrapper.sprite = _item.wrapper;
                //if (wrapper) wrapper.rectTransform.sizeDelta = new Vector2(95, 110);

                if (frame) frame.sprite = _item.frame;
                //if (frame) frame.rectTransform.sizeDelta = new Vector2(85, 85);

                if (backImage) backImage.sprite = _item.backImage;
                //if (backImage) backImage.rectTransform.sizeDelta = new Vector2(80, 80);

                if (image) image.sprite = _item.image;
                //if (image) image.rectTransform.sizeDelta = new Vector2(70, 70);

                if (price) price.text = _item.buyPrice.ToString();
            }
            else
            {
                if (backImage) backImage.sprite = _item.backImage;
                //if (backImage) backImage.rectTransform.sizeDelta = new Vector2(75, 75);

                if (image) image.sprite = _item.image;
                //if (image) image.rectTransform.sizeDelta = new Vector2(60, 60);

                if (price) price.text = _item.buyPrice.ToString();
            }


            OtherScripUI_Setting();


            var this_slot = gameObject.GetComponent<Button>();
            this_slot.interactable = true;

        }
        // 그렇지 않으면 이미지 비활성화
        else
        {
            if (image) image.enabled = false;
            if (backImage) backImage.enabled = false;

            gameObject.name = "Empty";
            
            
            
            OtherScripUI_Setting();


            
            var this_slot = gameObject.GetComponent<Button>();
            this_slot.interactable = false;
        }
    }

    //UI_ItemInfo 아이템트리(higher item, itemtree slot) 이미지 할당
    public void ItemSlotSetImage(Item _item)
    {
        this.itemList = _item;

        // 아이템이 존재하면 이미지 활성화
        if (itemList.valid)
        {
            if (wrapper) wrapper.enabled = true;
            if (wrapper) wrapper.sprite = _item.wrapper;
            
            if (frame) frame.enabled = true;
            if (frame) frame.sprite = _item.frame;

            if (backImage) backImage.enabled = true;
            if (backImage) backImage.sprite = _item.backImage;

            if (image) image.enabled = true;
            if (image) image.sprite = _item.image;

            if (price) price.enabled = true;
            if (price) price.text = _item.buyPrice.ToString();

            gameObject.name = _item.name;            
        }
        else
        {
            if (wrapper) wrapper.enabled = false;
            if (frame) frame.enabled = false;
            if (backImage) backImage.enabled = false;
            if (image) image.enabled = false;
            if (price) price.enabled = false;

            gameObject.name = "Empty";
        }
    }

   

    //UI_ItemInfo
    //아이템 클릭시 오브젝트 종류 확인하여 판매버튼 or 구매버튼 하나만 활성화
    public void ItemSellBuyCheck() //상점에서 오브젝트의 부모가 누구인지 확인
    {
        var target = EventSystem.current.currentSelectedGameObject;
        string targetParent_name = target.transform.parent.gameObject.name;


        if (targetParent_name == "StoreSlots") //상점 슬롯 클릭하면 구매 버튼만 활성화
        {

            buyBtn.GetComponent<Button>().interactable = true;
            sellBtn.GetComponent<Button>().interactable = false;
        }
        else if (targetParent_name == "Store_inventory") //인벤 슬롯 클릭하면 판매 버튼만 활성화
        {

            buyBtn.GetComponent<Button>().interactable = false;
            sellBtn.GetComponent<Button>().interactable = true;
        }
        else// if (targetParent_name == "Panel")
        {
            OtherScripUI_Setting();

            buyBtn.GetComponent<Button>().interactable = true;
            sellBtn.GetComponent<Button>().interactable = false;

            var item_info = UI_Store.self.itemInfo;
            var mergeList_Count = item_info.mergeList.Count;

            //mergeList = 최상위 아이템에서 최하위 아이템들의 리스트
            for(int i=0; i< mergeList_Count; i++)
            {
                //타겟 오브젝트의 이름이 mergeList에 있으면
                if(item_info.mergeList[i].name.Contains(target.name))
                {
                    //해당 아이템들의 정보를 info 창에 출력
                    item_info.item_name.text = item_info.mergeList[i].name;                    
                    item_info.item_Buyprice.text = item_info.mergeList[i].buyPrice.ToString();
                    item_info.item_tooltip.text = item_info.mergeList[i].ToolTip();

                    UI_Store.self.itemInfo.Select_Item_UpperList(item_info.mergeList[i]);
                }
            }
        }
    }

    
    public void OtherScripUI_Setting()//ItemInfo에 있는 버튼과 이미지를 슬롯과 연결해줌
    {
        //슬롯에 아이템이 장착된 경우에만 스토어의 구매/판매 버튼 정보를 가져옴
        var store = UI_Store.self;

        buyBtn = store.itemInfo.buyBtn;
        sellBtn = store.itemInfo.sellBtn;

        itemSlot_Parent = store.itemSlot_Parent.gameObject;
        inventory_Parent = store.inventory_Parent;
    }



    public void SelectedItemSlot() //상점에서 선택한 아이템 표시
    {
        //클릭한 오브젝트
        var target = EventSystem.current.currentSelectedGameObject;
        //클릭한 오브젝트의 UI_Slot 컴포넌트를 가져와서 wrapper와 frame 컬러를 변경
        var target_slot = target.GetComponent<UI_ItemSlot>();
        var targetParent = target.transform.parent.gameObject;

 
        if(target_slot.wrapper)
            _wrapper = target_slot.wrapper;
        if (target_slot.frame)
            _frame = target_slot.frame;

        InventorySlotColorClear(target_slot);

        if (targetParent.transform.childCount > 0)
        {
            if(target_slot._wrapper) 
                wrapper_color = _wrapper.GetComponent<Image>().color;

            if (target_slot._frame)
                frame_color = _frame.GetComponent<Image>().color;

            if (target_slot._wrapper)
                wrapper_color.a = 0f;

            if (target_slot._frame)
            {
                frame_color.r = 70 / 255f;
                frame_color.g = 70 / 255f;
                frame_color.b = 70 / 255f;
            }

            //모든 슬롯의 컬러 초기화
            for (int i = 0; i < targetParent.transform.childCount; i++)
            {
                if (target_slot._wrapper)
                    targetParent.transform.GetChild(i).
                        GetComponent<UI_ItemSlot>().wrapper.GetComponent<Image>().color = wrapper_color;

                if (target_slot.frame)
                    targetParent.transform.GetChild(i).
                        GetComponent<UI_ItemSlot>().frame.GetComponent<Image>().color = frame_color;
            }

            if (target_slot._wrapper)
                wrapper_color.a = 255f;

            if (target_slot._frame)
            {
                frame_color.r = 255 / 255f;
                frame_color.g = 255 / 255f;
                frame_color.b = 255 / 255f;
            }

            //타겟 오브젝트만 컬러 변경
            if (target_slot._wrapper)
                _wrapper.GetComponent<Image>().color = wrapper_color;

            if (target_slot._frame)
                _frame.GetComponent<Image>().color = frame_color;
            

        }
    }

    //상점 슬롯을 클릭하면 인벤토리 컬러 초기화
    //인벤토리 슬롯을 클릭하면 상점 컬러 초기화
    public void InventorySlotColorClear(UI_ItemSlot target) 
    {
        if (target.transform.parent.name == "StoreSlots") //self.slots
        {
            //상점 아이템 선택하면 인벤토리 아이템 컬러를 초기화
            for (int i = 0; i < inventory_Parent.transform.childCount; i++)
            {
                var child = inventory_Parent.transform.GetChild(i).gameObject.GetComponent<UI_ItemSlot>();

                var _frame = child.frame;

                Color frame_color = _frame.GetComponent<Image>().color;

                frame_color.r = 70 / 255f;
                frame_color.g = 70 / 255f;
                frame_color.b = 70 / 255f;

                _frame.GetComponent<Image>().color = frame_color;
            }
        }
        else if (target.transform.parent.name == "Store_inventory") //itemInfo.store_inventorySlot
        {
            //인벤토리 아이템 선택하면 상점 아이템 컬러 초기화
            for (int i = 0; i < itemSlot_Parent.transform.childCount; i++)
            {
                var child = itemSlot_Parent.transform.GetChild(i).gameObject.GetComponent<UI_ItemSlot>();

                var _wrapper = child.wrapper;
                var _frame = child.frame;

                Color wrapper_color = _wrapper.GetComponent<Image>().color;
                Color frame_color = _frame.GetComponent<Image>().color;

                wrapper_color.a = 0f;

                frame_color.r = 70 / 255f;
                frame_color.g = 70 / 255f;
                frame_color.b = 70 / 255f;

                _wrapper.GetComponent<Image>().color = wrapper_color;
                _frame.GetComponent<Image>().color = frame_color;
            }
        }
        else
        {
            UI_Store.self.itemInfo.MergePanelColorClear(target);

            //Infomation 아이템 클릭하면 상점과 인벤토리 클릭한 컬러 초기화

            //상점 아이템 선택하면 인벤토리 아이템 컬러를 초기화
            for (int i = 0; i < inventory_Parent.transform.childCount; i++)
            {
                var child = inventory_Parent.transform.GetChild(i).gameObject.GetComponent<UI_ItemSlot>();

                var _frame = child.frame;

                Color frame_color = _frame.GetComponent<Image>().color;

                frame_color.r = 70 / 255f;
                frame_color.g = 70 / 255f;
                frame_color.b = 70 / 255f;

                _frame.GetComponent<Image>().color = frame_color;
            }
            //인벤토리 아이템 선택하면 상점 아이템 컬러 초기화
            for (int i = 0; i < itemSlot_Parent.transform.childCount; i++)
            {
                var child = itemSlot_Parent.transform.GetChild(i).gameObject.GetComponent<UI_ItemSlot>();

                var _wrapper = child.wrapper;
                var _frame = child.frame;

                Color wrapper_color = _wrapper.GetComponent<Image>().color;
                Color frame_color = _frame.GetComponent<Image>().color;

                wrapper_color.a = 0f;

                frame_color.r = 70 / 255f;
                frame_color.g = 70 / 255f;
                frame_color.b = 70 / 255f;

                _wrapper.GetComponent<Image>().color = wrapper_color;
                _frame.GetComponent<Image>().color = frame_color;

            }
        }

    }
}
