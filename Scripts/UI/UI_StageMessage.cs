using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_StageMessage : MonoBehaviour
{
    public static UI_StageMessage Instance;

    public Text stageNoticeMessage;

    public void Awake()
    {
        if (!Instance)
            Instance = this;
        else
            Destroy(this);
    }

    // Start is called before the first frame update
    public void Start()
    {
        if (!stageNoticeMessage)
            stageNoticeMessage = transform.GetComponentInChildren<Text>();
    }

    public void RoomMessagePrintOn(string messageText)
    {
        stageNoticeMessage.text = messageText;
        stageNoticeMessage.gameObject.SetActive(true);

        StartCoroutine("RoomMessagePrintOff");
    }
    IEnumerator RoomMessagePrintOff()
    {
        yield return new WaitForSeconds(2.0f);
        stageNoticeMessage.gameObject.SetActive(false);
    }

}
