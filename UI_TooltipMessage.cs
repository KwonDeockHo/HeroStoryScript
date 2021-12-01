using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_TooltipMessage : MonoBehaviour
{
    public static UI_TooltipMessage self;
    public RectTransform panel;
    public Text text;
    // Start is called before the first frame update
    void Start()
    {
        if (self) Destroy(gameObject);
        else self = this;
    }

    public void PopUpTooltipMessage(string str)
    {
        panel.transform.position = Input.mousePosition + new Vector3(panel.rect.width * 0.5f, panel.rect.height * 0.5f, 0);
        text.text = str;
        Invoke("ShowTooltop", 0.5f);
    }

    void ShowTooltop()
    { panel.gameObject.SetActive(true); }
    void HideTooltop()
    { panel.gameObject.SetActive(false); }

    public void CloseTooltipMessage()
    {
        Invoke("HideTooltop", 0.5f);
    }
}
