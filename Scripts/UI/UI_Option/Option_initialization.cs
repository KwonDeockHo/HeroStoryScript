using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Option_initialization : MonoBehaviour
{
    //public static Option_initialization self;


    [Header("=== SmartCasting ===")]
    // Toggle
    [HideInInspector] public bool new_smartQ;
    [HideInInspector] public bool new_smartW;
    [HideInInspector] public bool new_smartE;
    [HideInInspector] public bool new_smartR;

    [HideInInspector] public bool new_smartAlpha1;
    [HideInInspector] public bool new_smartAlpha2;
    [HideInInspector] public bool new_smartAlpha3;
    [HideInInspector] public bool new_smartAlpha4;
    [HideInInspector] public bool new_smartAlpha5;
    [HideInInspector] public bool new_smartAlpha6;


    [Header("=== Interface ===")]
    // Slider
    [HideInInspector] public float new_optionSize;        // 옵션창 크기
    [HideInInspector] public float new_mouseCursorSize;   // 마우스 커서 크기 (미)
    [HideInInspector] public float new_storeSize;         // 상점 크기
    [HideInInspector] public float new_miniMapSize;       // 미니맵 크기
    [HideInInspector] public float new_bookSize;          // 북(탭) 크기 (미)

    // Toggle
    [HideInInspector] public bool isStatus;           // 플레이어 체력바 띄울건지
    [HideInInspector] public bool isGetDamage;        // 데미지 입으면 뜨는 UI
    [HideInInspector] public bool isGetCrowdControl;  // 군중제어기 맞으면 뜨는 UI
    [HideInInspector] public bool isCenter;           // 스페이스바 눌렀을때 플레이어 위치에 UI

    [HideInInspector] public bool isAttackRange;      // 공격 사거리 표시할지
    [HideInInspector] public bool isManaCost;         // 마나 소모량 표시할지


    [Header("=== GamePlay ===")]
    // Slider
    [HideInInspector] public float new_mouse_speed;               // 커서 속도 (미)
    [HideInInspector] public float new_screen_moveSpeed_Mouse;    // 화면 이동속도_마우스 (미)
    [HideInInspector] public float new_screen_moveSpeed_Keyboard; // 와면 이동속도_키보드 (미)

    // Toggle
    [HideInInspector] public bool isAutoAttack;       // 자동공격 할건지

    public bool All_True()
    {
        return true;
    }
    public bool All_False()
    {
        return false;
    }

    public void Get_SmartCast_Value()
    {
        new_smartQ = UI_Option_Button.self.skill1_Toggle.isOn;
        new_smartW = UI_Option_Button.self.skill2_Toggle.isOn;
        new_smartE = UI_Option_Button.self.skill3_Toggle.isOn;
        new_smartR = UI_Option_Button.self.skill4_Toggle.isOn;

        new_smartAlpha1 = UI_Option_Button.self.item1_Toggle.isOn;
        new_smartAlpha2 = UI_Option_Button.self.item2_Toggle.isOn;
        new_smartAlpha3 = UI_Option_Button.self.item3_Toggle.isOn;
        new_smartAlpha4 = UI_Option_Button.self.item4_Toggle.isOn;
        new_smartAlpha5 = UI_Option_Button.self.item5_Toggle.isOn;
        new_smartAlpha6 = UI_Option_Button.self.item6_Toggle.isOn;
    }


    // 적용될 때마다 변수들을 최신화 해줌
    public void Get_Interface_Value()
    {
        //new_optionSize = UI_Option_Interface.self.option_Size.value;
        //new_mouseCursorSize = UI_Option_Interface.self.mouseCursor_Size.value;
        //new_storeSize = UI_Option_Interface.self.store_Size.value;
        //new_miniMapSize = UI_Option_Interface.self.minimap_Size.value;
        //new_bookSize = UI_Option_Interface.self.book_Size.value;


        isStatus = UI_Option_Interface.self.playerStatus_toggle.isOn;
        isGetDamage = UI_Option_Interface.self.getDamageUI_toggle.isOn;
        isGetCrowdControl = UI_Option_Interface.self.getCrowdControlFlicker_toggle.isOn;
        isCenter = UI_Option_Interface.self.centerMark_toggle.isOn;
        isAttackRange = UI_Option_Interface.self.attackRange_toggle.isOn;
        isManaCost = UI_Option_Interface.self.manaCost_toggle.isOn;

        //Debug.Log("Confirm Init interface");
    }


    public void Get_GamePlay_Value()
    {
        //new_mouse_speed = UI_Option_Game.self.mouse_Speed.value;
        //new_screen_moveSpeed_Mouse = UI_Option_Game.self.screen_moveSpeed_Mouse.value;
        //new_screen_moveSpeed_Keyboard = UI_Option_Game.self.screen_moveSpeed_Keyboard.value;

        //isAutoAttack = UI_Option_Game.self.autoAttack_toggle.isOn;

        //Debug.Log("Confirm Init GamePlay");
    }




    public void Awake()
    {
        //if (!self)
        //    self = this;
        //else
        //    Destroy(this.gameObject);
    }

    void Start()
    {
        //DontDestroyOnLoad(this.gameObject);
    }

}
