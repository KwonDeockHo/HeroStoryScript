using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_TooltipMessageSender : MonoBehaviour
{
    public bool useMessageSender = true;
    public string message;

    public void HoverEnter()
    {
        if (!useMessageSender) return;
        if (UI_TooltipMessage.self) UI_TooltipMessage.self.PopUpTooltipMessage(message);
    }

    public void HoverExit()
    {
        if (!useMessageSender) return;
        if (UI_TooltipMessage.self) UI_TooltipMessage.self.CloseTooltipMessage();
    }
}
