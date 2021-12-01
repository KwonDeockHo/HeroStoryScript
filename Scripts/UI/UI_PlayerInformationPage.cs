using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_PlayerInformationPage : MonoBehaviour
{
    [Header("LEFT PAGE")]
    public Text championNameText;               // Player
    public Text championLevelText;               // Player
    public Text totalGoldText;                  // GameManager
    public Text killMonsterText;                // RoomController -> GameManager
    public Text totalClearRoomText;             // RoomController -> GameManager

    public Text damageReceiveText;
    public Text apDamageReceivedText;             // Player
    public Text adDamageReceivedText;             // Player

    public Text damageDealtText;               // Player
    public Text abilityPowerText;               // Player
    public Text attackDamageText;               // Player
    
    public Text playTimeText;
    //[Header("RIGHT PAGE")]
    //public bool minimapSetVisible = false;

    private void OnEnable()
    {
        UpdateTextValues();
    }

    public void UpdateTextValues()
    {
        championNameText.text = Player.player.name;
        championLevelText.text = Player.player.level.ToString();
        totalGoldText.text = InGameManager.Instance.rewardGold.ToString();
        killMonsterText.text = InGameManager.Instance.killMonsterCount.ToString();
        totalClearRoomText.text = InGameManager.Instance.totalClearRoomCount.ToString();
        damageReceiveText.text = ((int)InGameManager.Instance.apReceivedDamage + (int)InGameManager.Instance.adReceivedDamage).ToString();
        apDamageReceivedText.text = ((int)InGameManager.Instance.apReceivedDamage).ToString();
        adDamageReceivedText.text = ((int)InGameManager.Instance.adReceivedDamage).ToString();
        damageDealtText.text = ((int)InGameManager.Instance.abilityPower + (int)InGameManager.Instance.attackDamage).ToString();
        abilityPowerText.text = ((int)InGameManager.Instance.abilityPower).ToString();
        attackDamageText.text = ((int)InGameManager.Instance.attackDamage).ToString();

        playTimeText.text = (((int)Time.time / 3600) % 24).ToString("00") + " : " +
                            (((int)Time.time / 60) % 60).ToString("00") + " : " +
                            (((int)Time.time % 60)).ToString("00");
    }
}
