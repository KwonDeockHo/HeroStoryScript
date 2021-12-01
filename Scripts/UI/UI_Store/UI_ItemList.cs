using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UI_ItemList : MonoBehaviour
{
    public static UI_ItemList self;
    public List<ItemTemplate> itemsTemplate;
    public List<Item> items;


    void Awake()
    {
        if (!self)
            self = this;
        else
            Destroy(this);
        for(int i=0; i<itemsTemplate.Count; i++)
        {
            items.Add(new Item(itemsTemplate[i]));
        }
    }

    public Item[] GetUpperItems(Item _item)
    {
        return items.Where(item => item.IsNeedThisItemOnMerge(_item.template)).ToArray();
    }
}
