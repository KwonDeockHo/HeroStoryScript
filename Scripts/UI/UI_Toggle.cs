using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_Toggle : MonoBehaviour
{
    public static UI_Toggle self;
    [SerializeField] UI_Store getKey_uiStore;
    [SerializeField] UI_InformationBook getKey_Result;
    //[SerializeField] KeyCode[] toggleUIKeys;
    
    public GameObject miniMap;
    float minimMapScaler = 1f;
    float minimMapScalerSpeed = 2f;

    public GameObject centerPos;
    [HideInInspector]
    public bool isCenter;
    GameObject center_Instantiate;

    public List<KeyCode> inputkeyList = new List<KeyCode>();
    public KeyCode[] inventoryHotkeys = new KeyCode[] { KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5, KeyCode.Alpha6 };
    public KeyCode[] skillsHotkey = new KeyCode[] { KeyCode.A, KeyCode.Q, KeyCode.W, KeyCode.E, KeyCode.R };
    public bool[] skillsSmartCasting = new bool[] { false, false, false, false, false };
    public bool[] itemsSmartCasting = new bool[] { false, false, false, false, false, false };
    public KeyCode skillLearnKey = KeyCode.LeftControl;
    public KeyCode focusKey = KeyCode.Space;
    public KeyCode fixedFocusKey = KeyCode.U;
    public KeyCode storeKey = KeyCode.I;
    public KeyCode stopActionKey = KeyCode.S;
    public KeyCode infomationKey = KeyCode.Tab;
    public KeyCode detailStatus = KeyCode.C;
    public KeyCode settingKey = KeyCode.Escape;
    public KeyCode cancelKey = KeyCode.Escape;
    public KeyCode escapeKey = KeyCode.Escape;

    private bool stopKeyInput;

    private void Start()
    {
        if (self) Destroy(this);
        else self = this;

        isCenter = true;
        CreateCenterPos();//스페이스바 누르면 플레이어 밑에 표시
        ResetKeyValues();
    }

    public void OpenUI_Store()
    {
        if (!getKey_uiStore.IsActiveBook())
        {
            getKey_uiStore.SetActiveBook(true);
            inputkeyList.Add(storeKey);
        }
    }
    void Update()
    {
        if (center_Instantiate)
        {
            if (Input.GetKey(UI_Toggle.self.focusKey) && isCenter == true)
            {
                center_Instantiate.SetActive(true);
            }
            else
            {
                center_Instantiate.SetActive(false);
            }
        }
        else {
            CreateCenterPos();
        }
        if (!stopKeyInput)
        {
            if(Input.GetKeyDown(storeKey))
            {
                if (!getKey_uiStore.IsActiveBook())
                {
                    getKey_uiStore.SetActiveBook(true);
                    inputkeyList.Add(storeKey);
                }
                else if (getKey_uiStore.IsActiveBook())
                {
                    getKey_uiStore.SetActiveBook(false);
                    inputkeyList.Remove(storeKey);
                }


            }

            if (Input.GetKey(infomationKey))
            {
                //if (!StageManager.Instance.isGameOver && !StageManager.Instance.isStageClear)
                //{
                //    if (!getKey_Result.isOpen)
                //    {
                //        InGameManager.Instance.GameGetItemList(Player.player.inventory);
                //        getKey_Result.OpenResutPage();
                //        inputkeyList.Add(infomationKey);
                //    }
                //    else
                //    {
                //        getKey_Result.ui_BookGameObject.setInit();
                //        getKey_Result.CloseResutPage();
                //        inputkeyList.Remove(infomationKey);
                //    }
                //    miniMap.SetActive(!getKey_Result.isOpen); //탭에 미니맵 있으므로 꺼줌
                //}
                minimMapScaler = Mathf.Min(minimMapScaler + (Time.deltaTime * minimMapScalerSpeed), 2f);
                miniMap.transform.localScale = Vector3.one * minimMapScaler;
            }
            else
            {
                minimMapScaler = Mathf.Max(minimMapScaler - (Time.deltaTime * minimMapScalerSpeed), 1f);
                miniMap.transform.localScale = Vector3.one * minimMapScaler;
            }

            if (Input.GetKeyDown(escapeKey))
            {
                if (inputkeyList.Count > 0)
                {
                    var lastKeyCode = inputkeyList[inputkeyList.Count - 1];

                    if(lastKeyCode == storeKey)
                    {
                        getKey_uiStore.SetActiveBook(false);
                        inputkeyList.Remove(storeKey);
                    }
                    else if (lastKeyCode == infomationKey)
                    {
                        getKey_Result.ui_Book.setInit();
                        getKey_Result.SetActiveBook(false);
                        inputkeyList.Remove(infomationKey);
                    }
                    else if (UI_Option.self.IsActiveBook())
                    {
                        UI_Option.self.SetActiveBook(false);
                        ResetKeyValues();
                        inputkeyList.Remove(escapeKey);
                    }
                }
                else if(inputkeyList.Count == 0) //setActive된 ui가 없을때만
                {
                    if (!UI_Option.self.IsActiveBook())
                    {
                        UI_Option.self.SetActiveBook(true);
                        inputkeyList.Add(escapeKey);
                    }
                }
            }
        }
    }


    void CreateCenterPos()
    {
        if (!Player.player) return;
        //Debug.Log("Create ");
        center_Instantiate = Instantiate(centerPos, Player.player.transform);
        center_Instantiate.transform.SetAsLastSibling();
        center_Instantiate.name = "CenterPos";

        center_Instantiate.SetActive(false);
    }

    public void ResetKeyValues()
    {
        var setting = SettingManager.self;
        if(!setting) return;
        inventoryHotkeys = new KeyCode[] { setting.keycode_Item1, setting.keycode_Item2, setting.keycode_Item3, setting.keycode_Item4, setting.keycode_Item5, setting.keycode_Item6 };
        skillsHotkey = new KeyCode[] { setting.keycode_Attack, setting.keycode_Skill1, setting.keycode_Skill2, setting.keycode_Skill3, setting.keycode_Skill4 };
        skillsSmartCasting = new bool[] { false, setting.smartCasting_Skill1, setting.smartCasting_Skill2, setting.smartCasting_Skill3, setting.smartCasting_Skill4 };
        itemsSmartCasting = new bool[] { setting.smartCasting_Item1, setting.smartCasting_Item2, setting.smartCasting_Item3, setting.smartCasting_Item4, setting.smartCasting_Item5, setting.smartCasting_Item6 };
        skillLearnKey = setting.keycode_LearnSkill;
        stopActionKey = setting.keycode_StopAction;
        detailStatus = setting.keycode_DetailStatus;
        storeKey = setting.keycode_Store;
        infomationKey = setting.keycode_Information;
        settingKey = setting.keycode_Setting;
        fixedFocusKey = setting.keycode_FixedCamera;
        focusKey = setting.keycode_FollowCamera;
        
        cancelKey = KeyCode.Escape;
        escapeKey = KeyCode.Escape;
    }
}
