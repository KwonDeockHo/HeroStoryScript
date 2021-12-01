using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDropObject : MonoBehaviour
{
    public Item dropItem = new Item();
    public SpriteRenderer itemImage;
    public Animator animator;
    public Player player;
    public Camera m_Camera;
    public UI_ToolTip_Object toolTipObject;
    public ObjectOpener parentOpenr;
    public AudioSource audioSource;

    public void Start()
    {
        if (toolTipObject)
            toolTipObject.setHide();
    }
    void LateUpdate()
    {
        if(m_Camera)
        {
            itemImage.transform.LookAt(itemImage.transform.position + m_Camera.transform.rotation * Vector3.back, m_Camera.transform.rotation * Vector3.up);
        }else
            m_Camera = CameraManager.Instance.GetComponent<Camera>();
    }

    private void OnMouseOver()
    {
        if (parentOpenr.isItemOpenDone)
            return;

       // Debug.Log("ItemSelect");
        DropItemSelect();
        if (toolTipObject)
            toolTipObject.setActive();
    }

    private void OnMouseExit()
    {
        DropItemSelectRealse();
       // Debug.Log("DropItemSelectRealse");
        if (toolTipObject)
            toolTipObject.setHide();
    }
    public void DropItemSetting(Item items)
    {
        dropItem = items;

        if (dropItem.image)
            itemImage.sprite = dropItem.image;

    }
    public void PlaySoundChestOpen()
    {
        if (audioSource)
        {
            if (SettingManager.self) audioSource.volume = SettingManager.self.CalcFxValue();
            audioSource.Play();
        }
    }

    private void OnMouseDown()
    {
        if (parentOpenr.isItemOpenDone)
            return;

        DropItemSelectRealse();

        if (!player) {
            player = Player.player;
        }

        if (player.GetInventoryValidCount() < player.inventorySize) {
            player.InventoryAddAmount(dropItem.template, 1);
            itemImage.sprite = null;
            toolTipObject.setHide();
            parentOpenr.setItemOpenDone();
        }
        else
        {
            UI_StageMessage.Instance.RoomMessagePrintOn("인벤토리 창을 확인하세요");
        }
    }
    public void DropItemOpen()
    {
        if (parentOpenr.isItemOpenDone)
            return;

        animator.SetBool("IsOpen", true);
        animator.SetBool("IsClose", false);

    }
    public void DropItemClose()
    {
        if (parentOpenr.isItemOpenDone)
            return;

        animator.SetBool("IsOpen", false);
        animator.SetBool("IsClose", true);
    }

    public void DropItemSelect()
    {
        if (parentOpenr.isItemOpenDone)
            return;

        animator.SetBool("IsSelect", true);

    }
    public void DropItemSelectRealse()
    {
        if (parentOpenr.isItemOpenDone)
            return;

        animator.SetBool("IsSelect", false);
        animator.SetBool("IsClose", false);
        animator.SetBool("IsOpen", false);


    }
}
