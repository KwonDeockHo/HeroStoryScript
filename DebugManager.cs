using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class DebugManager : MonoBehaviour
{
    public GameObject panel;
    public RectTransform rect;
    public Text text;

    void OnEnable() { Application.logMessageReceived += Log; }
    void OnDisable() { Application.logMessageReceived -= Log; }
    public void Log(string logString, string stackTrace, LogType type)
    {
        if (type == LogType.Warning) return;
        if (type == LogType.Error || type == LogType.Exception)
        {
            text.text += "<color=red>" + logString + "</color>" + "\n";
            text.text += "<color=red>" + stackTrace + "</color>" + "\n";
        }
        else
            text.text += logString + "\n";
        ActivePanel(panel.activeSelf);

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.BackQuote))
            ActivePanel(!panel.activeSelf);
        if (!panel.activeSelf) return;
    }

    public void ActivePanel(bool active)
    {
        panel.SetActive(active);
        if (!panel.activeSelf) return;
        int lineCount = text.text.Split('\n').Length;
        rect.sizeDelta = new Vector2(rect.sizeDelta.x, lineCount * 29);
        rect.anchoredPosition = new Vector2(0, rect.sizeDelta.y * 0.5f);
    }
}

