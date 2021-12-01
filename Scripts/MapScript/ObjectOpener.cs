using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectOpener : MonoBehaviour
{
    public RoomName useRoom;
    public GameObject[] subObject;
    public bool isOpen = false;
    public bool isClose = false;
    public bool isItemOpenDone = false;

    public void Start()
    {
        if (useRoom == RoomName.Item)
        {
            subObjectSetting();
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && useRoom == RoomName.Shop)
        {
            var player = other.GetComponent<Player>();
            if (!player) return;
            if (UI_Toggle.self) UI_Toggle.self.OpenUI_Store();
        }

        if (other.tag == "Player" && useRoom == RoomName.Item)
        {

            if (isItemOpenDone)
                return;

            var player = other.GetComponent<Player>();
            if (!player) return;

            subObjectOpen();
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" && useRoom == RoomName.Shop)
        {
            var player = other.GetComponent<Player>();
            if (!player) return;

            // Close Event
            if (UI_Toggle.self) UI_Toggle.self.OpenUI_Store();
        }

        if (other.tag == "Player" && useRoom == RoomName.Item)
        {
            if (isItemOpenDone)
                return;

            var player = other.GetComponent<Player>();
            if (!player) return;

            subObjectClose();
        }
    }
    public void subObjectSetting()
    {
        int itemgrade = Mathf.CeilToInt((float)StageManager.Instance.currentStageLevel / 3);
        ItemGrade selectGrade = ItemGrade.Common;
        List<Item> listItem = new List<Item>();
        switch (itemgrade)
        {
            case 1:
                selectGrade = ItemGrade.Common;
                break;
            case 2:
                selectGrade = ItemGrade.Rare;
                break;
            case 3:
                selectGrade = ItemGrade.Legendary;
                break;
            default:
                selectGrade = (ItemGrade)Random.Range((int)ItemGrade.Common, (int)ItemGrade.Legendary);
                break;
        }
        
        for(int i=0; i<UI_ItemList.self.itemsTemplate.Count; i++)
        {
            if(UI_ItemList.self.itemsTemplate[i].itemType == ItemType.Equipment &&
                UI_ItemList.self.itemsTemplate[i].itemGrade == selectGrade)
                listItem.Add(new Item(UI_ItemList.self.itemsTemplate[i]));
        }

        for (int i = 0; i < subObject.Length; i++) {
            Item selectItem = listItem[Random.Range(0, listItem.Count)];

            subObject[i].GetComponent<ItemDropObject>().DropItemSetting(selectItem);

        }
    }
    public void setItemOpenDone() {
        subObjectClose();
        isItemOpenDone = true;
    }

    public void subObjectOpen()
    {
        if (isItemOpenDone)
            return;

        if (isOpen)
            return;

        for(int i=0; i<subObject.Length; i++)
        {
            subObject[i].GetComponent<ItemDropObject>().DropItemOpen();
        }
        isOpen = true;
        isClose = false;

    }
    public void subObjectClose()
    {
        if (isItemOpenDone)
            return;

        if (isClose)
            return;

        for (int i = 0; i < subObject.Length; i++)
        {
            subObject[i].GetComponent<ItemDropObject>().DropItemClose();
        }
        isClose = true;
        isOpen = false;
    }
}
