using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public enum SortType
{
    //   0          1          2        3
    HealthMax, HealthRegen, ManaMax, ManaRegen, 
    //   4             5          6
    AttackDamage, AbilityPower, Armor, 
    //   7              8
    CriticalChance, CriticalDamage, 
    //  9          10          11         12
    MoveSpeed, AttackSpeed, Cooldown, Absorption
}

public class UI_Store : Book
{
    public AudioClip audioClip_Buy;
    public AudioClip audioClip_Sell;

    public Transform tooltipPosition;

    [Header("===Other Script===")]
    public static UI_Store self;
    public UI_ItemInfo itemInfo;
    public UI_ItemList uitemList;


    [Header("===ItemSlot===")]
    //상점창에 띄워줄 slots 리스트
    public List<UI_ItemSlot> slots = new List<UI_ItemSlot>();
    public System.Action<Item> onSlotClick;

    [Header("===Prefab===")]
    public GameObject itemSlot_Prefab;
    public RectTransform itemSlot_Parent;
    public GameObject inventory_Parent;

    public bool canUseStore;

    public Button buyButton;
    public UI_TooltipMessageSender buyButtonSender;
    public Button sellButton;
    public UI_TooltipMessageSender sellButtonSender;
    Color buttonColor;

    // Start is called before the first frame update
    void Start()
    {
        if (self) Destroy(this);
        self = this;
       
        Create_StoreSlot();
        buttonColor = buyButton.image.color;
        book.SetActive(false);
    }

    private void OnEnable()
    {
        if(self)
            itemInfo.ItemUpdate();

        //var storeSize = UI_Option_Interface.self.new_storeSize;        
        //transform.GetComponent<RectTransform>().localScale = new Vector3(storeSize, storeSize, 0);
    }

    public override void SetActiveBook(bool active)
    {
        base.SetActiveBook(active);
        if (RoomController.Instance && RoomController.Instance.currRoom) { 
            if(RoomController.Instance.currRoom.roomName == RoomName.Shop)
            {
                buyButton.image.color = buttonColor;
                sellButton.image.color = buttonColor;
                canUseStore = true;
                buyButtonSender.useMessageSender = !canUseStore;
                sellButtonSender.useMessageSender = !canUseStore;
            }
            else
            {
                buyButton.image.color = Color.gray;
                sellButton.image.color = Color.gray;
                canUseStore = false;
                buyButtonSender.useMessageSender = !canUseStore;
                sellButtonSender.useMessageSender = !canUseStore;
            }
        }
    }

    public void PlaySound_Buy()
    {
        if (audioSource)
        {
            if (SettingManager.self) audioSource.volume = SettingManager.self.CalcFxValue();
            audioSource.clip = audioClip_Buy;
            audioSource.Play();
        }
    }

    public void PlaySound_Sell()
    {
        if (audioSource)
        {
            if (SettingManager.self) audioSource.volume = SettingManager.self.CalcFxValue();
            audioSource.clip = audioClip_Sell;
            audioSource.Play();
        }
    }

    public void Create_StoreSlot() //상점 처음 열었을때 슬롯 생성하고 아이템 보여줌
    {
        //타입별 탐색하기 전에 슬롯을 지워줌
        DeleteItemSlot();

        //아이템 생성되는 Root의 자식(생성될 아이템)의 개수 
        var slot_cnt = itemSlot_Parent.transform.childCount;

        if (slot_cnt < 1)
        {
            int slotCnt = uitemList.items.Count;

            
            for (int i = 0; i < slotCnt; i++)
            {
                //아이템 슬롯 생성
                GameObject item_slot = Instantiate(itemSlot_Prefab);
                item_slot.transform.SetParent(itemSlot_Parent.transform);
                item_slot.transform.SetAsLastSibling();
                item_slot.transform.localScale = itemSlot_Parent.transform.localScale;
                
                var item_slot_Button = item_slot.GetComponent<Button>();
                item_slot_Button.onClick.AddListener(() => OnClickSlot(item_slot.GetComponent<UI_ItemSlot>()));
            }
            //아이템 할당
            Create_ItemSlot(uitemList.items.Count);
        }
    }

    public void Create_ItemSlot(int item_slotCnt) //빈 슬롯에 아이템 할당
    {

        for (int i = 0; i < item_slotCnt; i++)
        {
            //슬롯이 생성되는 slotRoot 하위의 0~slotCnt번째의 오브젝트에 접근
            var slot = itemSlot_Parent.transform.GetChild(i).GetComponent<UI_ItemSlot>();

            //아이템이 있는 slot에는 아이템 이름과 이미지를 담고 리스트에 저장
            slot.ItemSlotSetImage(uitemList.items[i], itemSlot_Parent.transform);
            slots.Add(slot);


            slot.tooltip.item = uitemList.items[i];
            slot.tooltip.createPos = tooltipPosition;
            slot.tooltip.text = uitemList.items[i].ToolTip();
            slot.tooltip.enabled = true;

        }
        //나중에 에셋 바꾸면 값 수정 필요
        ViewPortSize();
    }



    public void Create_StoreSlot(int cnt) //다시 모든 아이템 열려고 할때
    {
        //타입별 탐색하기 전에 스토어의 슬롯을 지워줌
        DeleteItemSlot();

        var slot_cnt = itemSlot_Parent.transform.childCount;

        if (slot_cnt < 1)
        {
            for (int i = 0; i < cnt; i++)
            {
                GameObject item_slot = Instantiate(itemSlot_Prefab);
                item_slot.transform.SetParent(itemSlot_Parent.transform);
                item_slot.transform.SetAsLastSibling();
                item_slot.transform.localScale = itemSlot_Parent.transform.localScale;

                var item_slot_Button = item_slot.GetComponent<Button>();
                item_slot_Button.onClick.AddListener(() => OnClickSlot(item_slot.GetComponent<UI_ItemSlot>()));
            }
        }

        ViewPortSize();
    }

    public void DeleteItemSlot() // 슬롯과 리스트 초기화
    {
        if (itemSlot_Parent.transform.childCount > 0)
        {
            for (int i = 0; i < slots.Count; i++)
            {
                var slot_delete = itemSlot_Parent.transform.GetChild(i).GetComponent<UI_ItemSlot>();

                Destroy(slot_delete.gameObject);
            }
            //DetachChildren -> 모든 child transform으로부터 부모 transform을 떼어냄. (종속관계 해제)
            //gameobject Destroy를 해도 오브젝트가 바로 파괴되지 않음.
            itemSlot_Parent.transform.DetachChildren();
            slots.Clear();
        }
    }

    
    public void Item_Sort_Type(int _type)//빈 슬롯에 선택한 타입에 맞는 아이템 할당
    {
        //타입별 탐색하기 전에 스토어의 슬롯을 지워줌
        DeleteItemSlot();

        int typeCnt = 0;

        //아이템 리스트에서 해당 타입에 맞는 아이템의 갯수를 찾음
        for (int i = 0; i < uitemList.items.Count; i++)
        {
            //ItemTpye enum : Potion->0 , Equipment->1, Size->2
            if ((int)uitemList.items[i].itemType == _type)
            {
                typeCnt++;
            }
        }
        //찾은 타입의 갯수 ex) Potion은 2개
        //Debug.Log("typeCnt : "+typeCnt);

        //갯수만큼 슬롯 생성
        Create_StoreSlot(typeCnt);

        int check_index = 0;

        //생성된 슬롯에 해당 타입의 아이템 적용
        for (int i = 0; i < typeCnt; i++)
        {
            for (int j = check_index; j < uitemList.items.Count; j++)
            {
                if ((int)uitemList.items[j].itemType == _type)
                {
                    //슬롯이 생성되는 slotRoot 하위의 0~slotCnt번째의 오브젝트에 접근
                    var slot = itemSlot_Parent.transform.GetChild(i).GetComponent<UI_ItemSlot>();

                    //아이템이 있는 slot에는 아이템 이름과 이미지를 담고 리스트에 저장
                    slot.ItemSlotSetImage(uitemList.items[j], itemSlot_Parent.transform);
                    slots.Add(slot);

                    slot.tooltip.item = uitemList.items[j];
                    slot.tooltip.createPos = tooltipPosition;
                    slot.tooltip.text = uitemList.items[j].ToolTip();
                    slot.tooltip.enabled = true;

                    check_index = ++j;
                    break;
                }
            }
        }


        ViewPortSize();
    }


    public void Item_Sort_Grage(int _type)//빈 슬롯에 선택한 타입에 맞는 아이템 할당
    {
        //타입별 탐색하기 전에 스토어의 슬롯을 지워줌
        DeleteItemSlot();

        int typeCnt = 0;
        
        //아이템 리스트에서 해당 타입에 맞는 아이템의 갯수를 찾음
        for (int i = 0; i < uitemList.items.Count; i++)
        {
            //ItemTpye enum : Common->0 , Rare->1, Legendary->2
            if ((int)uitemList.items[i].itemGrade == _type)
            {
                typeCnt++;
            }
        }
        //찾은 타입의 갯수 ex) Potion은 2개
        //Debug.Log("typeCnt : "+typeCnt);

        //갯수만큼 슬롯 생성
        Create_StoreSlot(typeCnt);

        int check_index = 0;

        //생성된 슬롯에 해당 타입의 아이템 적용
        for (int i = 0; i < typeCnt; i++)
        {
            for (int j = check_index; j < uitemList.items.Count; j++)
            {
                if ((int)uitemList.items[j].itemGrade == _type)
                {
                    //슬롯이 생성되는 slotRoot 하위의 0~slotCnt번째의 오브젝트에 접근
                    var slot = itemSlot_Parent.transform.GetChild(i).GetComponent<UI_ItemSlot>();

                    //아이템이 있는 slot에는 아이템 이름과 이미지를 담고 리스트에 저장
                    slot.ItemSlotSetImage(uitemList.items[j], itemSlot_Parent.transform);
                    slots.Add(slot);


                    slot.tooltip.item = uitemList.items[j];
                    slot.tooltip.createPos = tooltipPosition;
                    slot.tooltip.text = uitemList.items[j].ToolTip();
                    slot.tooltip.enabled = true;


                    check_index = ++j;
                    break;
                }
            }
        }

        ViewPortSize();
    }


    public void ViewPortSize()//스크롤되는 아이템 리스트의 사이즈 조절
    {
        //한 줄당 n개의 아이템이 들어어가면 때문에 1줄에 150의 height를 가져야함.
        //하지만 500보다 작으면 맨위에서 시작을 못하기때문에 수치 조정 필요
        //아이템이 너무 많으면 스크롤 중간부터 시작해서 content의 pivot을 x=0 y=1로 해결
        
        int itemLineCount = ((itemSlot_Parent.transform.childCount-1)/7);
        itemLineCount = (itemLineCount - 2) >= 0 ? itemLineCount-2 : 0;

        int _rectTransform = (itemLineCount * 150)+500;

        itemSlot_Parent.sizeDelta = new Vector2(itemSlot_Parent.rect.width, _rectTransform);

        //Debug.Log("차일드 : "+ itemSlot_Parent.transform.childCount + ", 줄 : "+itemLineCount);

    }


    public void OnClickSlot(UI_ItemSlot slot)
    {
        //Debug.Log(slot.name);
        if (onSlotClick != null)
        {
            onSlotClick(slot.itemList);
        }
    }

    public void Item_Sorting(int sortTpye)
    {

        //items에 정의한 sortType 옵션이 있는 아이템이 Item[] 안에 저장됨
        var items = ItemSortOption(sortTpye);

        //items 리스트에 담긴 길이만큼 슬롯 생성
        Create_StoreSlot(items.Length);

        //생성된 슬롯에 아이템 정보 할당
        for (int i=0; i<items.Length; i++)
        {
            var slot = itemSlot_Parent.transform.GetChild(i).GetComponent<UI_ItemSlot>();

            slot.ItemSlotSetImage(items[i], itemSlot_Parent.transform);
            slots.Add(slot);


            slot.tooltip.item = items[i];
            slot.tooltip.createPos = tooltipPosition;
            slot.tooltip.text = items[i].ToolTip();
            slot.tooltip.enabled = true;
        }

        ViewPortSize();
    }
    public Item[] ItemSortOption(int sortType)//sortType에 맞는 옵션을 가진 아이템 반환
    {
        /*
                //   0          1          2        3
                HealthMax, HealthRegen, ManaMax, ManaRegen, 
                //   4             5          6
                AttackDamage, AbilityPower, Armor, 
                //   7              8
                CriticalChance, CriticalDamage, 
                //  9          10          11         12
                MoveSpeed, AttackSpeed, Cooldown, Absorption
        */

        if (sortType == (int)SortType.HealthMax)
        {
            return uitemList.items.Where(item => item.equipHealthMaxBonus > 0).ToArray();
        }
        else if (sortType == (int)SortType.HealthRegen)
        {
            return uitemList.items.Where(item => item.equipHealthRegenBonus > 0).ToArray();
        }
        else if (sortType == (int)SortType.ManaMax)
        {
            return uitemList.items.Where(item => item.equipManaMaxBonus > 0).ToArray();
        }
        else if (sortType == (int)SortType.ManaRegen)
        {
            return uitemList.items.Where(item => item.equipManaRegenBonus > 0).ToArray();
        }
        else if (sortType == (int)SortType.AttackDamage)
        {
            return uitemList.items.Where(item => item.equipAttackDamageBonus > 0).ToArray();
        }
        else if (sortType == (int)SortType.AbilityPower)
        {
            return uitemList.items.Where(item => item.equipAbilityPowerBonus > 0).ToArray();
        }
        else if (sortType == (int)SortType.Armor)
        {
            return uitemList.items.Where(item => item.equipArmorBonus > 0).ToArray();
        }
        else if(sortType == (int)SortType.CriticalChance)
        {
            return uitemList.items.Where(item => item.equipCriticalChanceBonus > 0).ToArray();
        }
        else if (sortType == (int)SortType.CriticalDamage)
        {
            return uitemList.items.Where(item => item.equipCriticalDamageBonus > 0).ToArray();
        }
        else if (sortType == (int)SortType.MoveSpeed)
        {
            return uitemList.items.Where(item => item.equipMoveSpeedBonus > 0).ToArray();
        }
        else if (sortType == (int)SortType.AttackSpeed)
        {
            return uitemList.items.Where(item => item.equipAttackSpeedBonus > 0).ToArray();
        }
        else if (sortType == (int)SortType.Cooldown)
        {
            return uitemList.items.Where(item => item.equipCooldownBonus > 0).ToArray();
        }
        else if (sortType == (int)SortType.Absorption)
        {
            return uitemList.items.Where(item => item.equipAbsorptionBonus > 0).ToArray();
        }


        return null;
    }

}
