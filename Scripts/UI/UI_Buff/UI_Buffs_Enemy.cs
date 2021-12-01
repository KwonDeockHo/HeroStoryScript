using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Buffs_Enemy : MonoBehaviour
{
    public Entity owner;

    public GameObject buffPrefab;
    public List<UI_Buff_Enemy> buffSlot = new List<UI_Buff_Enemy>();

    public Transform tooltipPosition;


    void OnEnable()
    {
        owner = gameObject.transform.parent.GetComponent<UI_targetEnemyStatus>().owner;
    }

    void Update()
    {
        if (!owner) return;

        BalancePrefabs();

        for (int i = 0; i < owner.buffs.Count; i++)
        {
            var buff = owner.buffs[i];
            var slot = buffSlot[i];

            slot.buffImage.sprite = buff.image;
            slot.buffImageFill.sprite = buff.image;
            slot.buffImageFill.fillAmount = buff.BuffTimeRemaining() / (float)buff.buffTime;
            slot.tooltip.buff = buff;
            slot.tooltip._targetEntity = owner;
        }
    }
    
    void BalancePrefabs()
    {
        if (!owner) return;

        int amount = owner.buffs.Count;

        if (buffSlot.Count < amount)
        {
            for (int i = buffSlot.Count; i < amount; i++)
            {
                var buff = Instantiate(buffPrefab);
                buff.transform.parent = tooltipPosition;
                buffSlot.Add(buff.GetComponent<UI_Buff_Enemy>());
            }
        }
        for (int i = 0; i < buffSlot.Count; i++)
        {
            buffSlot[i].gameObject.SetActive(i < amount);
        }
    }


}
