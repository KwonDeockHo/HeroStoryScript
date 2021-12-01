using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class UI_Buff_ShowTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Buff buff;

    public GameObject tooltipPrefab;
    public Transform createPos;
    public float tooltipWidth = 200;

    public Entity _targetEntity;

    GameObject current;


    void CreateToolTip()
    {

        if (current) Destroy(current);
        current = Instantiate(tooltipPrefab, createPos.position, Quaternion.identity);

        
        var buffTooltip = current.GetComponent<UI_Buff>();
        if (buffTooltip)
        {
            buffTooltip.buff = buff;
            
            current.transform.position = createPos.position;
        }

        var tooltip = current.GetComponent<UI_Buff_Tooltip>();
        if (tooltip && buff.image)
        {
            tooltip.buff_Image.sprite = buff.image;
            tooltip.buff_name.text = buff.template.skillname;            
            tooltip.buff_tooltip.text = " " + buff.ToolTip(buff.caster);
            tooltip.buff_caster.text = " 시전자 : "+buff.caster.name;

            tooltip.target = _targetEntity;
        }


        var buffenemyTooltip = current.GetComponent<UI_Buff_Enemy>();
        if (buffenemyTooltip)
        {
            buffenemyTooltip.buff = buff;

            current.transform.position = createPos.position;
        }



        current.transform.SetParent(createPos ? createPos : transform.root, true);
        current.transform.SetAsLastSibling();
        
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
