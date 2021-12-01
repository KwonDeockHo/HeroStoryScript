using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Button : MonoBehaviour
{
    public Text textfield;

    public void SetText(string text)
    {
        textfield.text = text;
    }
}
