using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class UI_OptionSlot : MonoBehaviour
{
    UI_Option uOption;

    [SerializeField] Image selectFrame;
    [SerializeField] Image tab_arrow;

    // Start is called before the first frame update
    void Start()
    {
        uOption = UI_Option.self;
    }


    public void Selected_Slot() //선택한 옵션 표시 표시하고 해당 패널 열기
    {
        var target = EventSystem.current.currentSelectedGameObject;

        var slots = uOption.slots;
       

        if (target.name == "단축키")
        {
            for (int i = 0; i < slots.Count; i++)
            {
                if(slots[i].gameObject.name == target.name)
                {
                    slots[i].selectFrame.enabled = true;
                    slots[i].tab_arrow.enabled = true;
                }
                else
                {
                    slots[i].selectFrame.enabled = false;
                    slots[i].tab_arrow.enabled = false;
                }                
            }

            uOption.inputkeyPage.SetActive(true);
            uOption.viewPage.SetActive(false);
            uOption.soundPage.SetActive(false);
            uOption.interfacePage.SetActive(false);
            uOption.gamePage.SetActive(false);
        }
        else if (target.name == "화면")
        {
            for (int i = 0; i < slots.Count; i++)
            {
                if (slots[i].gameObject.name == target.name)
                {
                    slots[i].selectFrame.enabled = true;
                    slots[i].tab_arrow.enabled = true;
                }
                else
                {
                    slots[i].selectFrame.enabled = false;
                    slots[i].tab_arrow.enabled = false;
                }
            }

            uOption.inputkeyPage.SetActive(false);
            uOption.viewPage.SetActive(true);
            uOption.soundPage.SetActive(false);
            uOption.interfacePage.SetActive(false);
            uOption.gamePage.SetActive(false);
        }
        else if (target.name == "사운드")
        {
            for (int i = 0; i < slots.Count; i++)
            {
                if (slots[i].gameObject.name == target.name)
                {
                    slots[i].selectFrame.enabled = true;
                    slots[i].tab_arrow.enabled = true;
                }
                else
                {
                    slots[i].selectFrame.enabled = false;
                    slots[i].tab_arrow.enabled = false;
                }
            }

            uOption.inputkeyPage.SetActive(false);
            uOption.viewPage.SetActive(false);
            uOption.soundPage.SetActive(true);
            uOption.interfacePage.SetActive(false);
            uOption.gamePage.SetActive(false);
        }
        else if (target.name == "인터페이스")
        {
            for (int i = 0; i < slots.Count; i++)
            {
                if (slots[i].gameObject.name == target.name)
                {
                    slots[i].selectFrame.enabled = true;
                    slots[i].tab_arrow.enabled = true;
                }
                else
                {
                    slots[i].selectFrame.enabled = false;
                    slots[i].tab_arrow.enabled = false;
                }
            }

            uOption.inputkeyPage.SetActive(false);
            uOption.viewPage.SetActive(false);
            uOption.soundPage.SetActive(false);
            uOption.interfacePage.SetActive(true);
            uOption.gamePage.SetActive(false);
        }
        else if (target.name == "게임")
        {
            for (int i = 0; i < slots.Count; i++)
            {
                if (slots[i].gameObject.name == target.name)
                {
                    slots[i].selectFrame.enabled = true;
                    slots[i].tab_arrow.enabled = true;
                }
                else
                {
                    slots[i].selectFrame.enabled = false;
                    slots[i].tab_arrow.enabled = false;
                }
            }

            uOption.inputkeyPage.SetActive(false);
            uOption.viewPage.SetActive(false);
            uOption.soundPage.SetActive(false);
            uOption.interfacePage.SetActive(false);
            uOption.gamePage.SetActive(true);
        }
    }

}
