using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_RoomTimer : MonoBehaviour
{
    public Text roomMinute;
    public Text roomSecond;
    // Start is called before the first frame update
    public void roomTimerUpdate(string Minute, string Second)
    {
        roomMinute.text = Minute;
        roomSecond.text = Second;
    }
    
}
