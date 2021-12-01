using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_ClickBuySell : MonoBehaviour, IPointerClickHandler
{
    Player player;
    UI_ItemSlot slot;
    public PointerEventData.InputButton btn2 = PointerEventData.InputButton.Right;

    void Start()
    {
        player = Player.player;
        if (!player) return;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == btn2)
        {
            slot = GetComponent<UI_ItemSlot>();

            if (transform.parent.name == "Store_inventory") //Sell
            {
                ItemSell();
            }
            else if (transform.parent.name == "StoreSlots") //Buy
            {
                ItemBuy();
            }
            else if (transform.parent.name == "upperItemList") //Buy
            {
                ItemBuy();
            }
            else //Buy -> TreeSlot
            {
                ItemBuy();
            }

        }
    }


    
    //아이템 구매
    public void ItemBuy()
    {
        if (!player)
            player = Player.player;
        if (!slot.itemList.template) return;
        int price = player.GetPriceForBuyItem(slot.itemList.template);
        if (player.gold >= price)
        {
            if (slot.itemList.valid) {
                if (UI_Store.self && UI_Store.self.canUseStore)
                {
                    player.InventoryAddAmount(slot.itemList.template, 1, true);
                    UI_Store.self.PlaySound_Buy();

                    //player.gold -= slot.itemList.buyPrice;

                    UI_Store.self.itemInfo.ItemUpdate();
                }
            }


        }
        
    }


    //아이템 판매
    public void ItemSell()
    {
        if (!player)
            player = Player.player;
        if (!slot.itemList.template) return;
        if (slot.itemList.valid) {
            if (UI_Store.self && UI_Store.self.canUseStore)
            {
                player.InventoryDeleteAmount(slot.itemList.template, 1);
                UI_Store.self.PlaySound_Sell();

                player.gold += slot.itemList.buyPrice;

                UI_Store.self.itemInfo.ItemUpdate();
            }
        }


    }

    


}
