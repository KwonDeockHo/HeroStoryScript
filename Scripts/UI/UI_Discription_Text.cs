using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Discription_Text : MonoBehaviour
{
    public static UI_Discription_Text self;
    public string discription;
    Entity player;


    void Start()
    {
        if (self) Destroy(this);
        self = this;

    }

    public string Get_StatusName(string name, string value, Entity _player)
    {
        this.player = _player;
        discription = "";
        //Debug.Log("이름 : "+name+" 값 : "+value);

        if(name == "AttackDamage")
        {
            discription = Status_AttackDamage(value, player);
        }
        else if (name == "AbilityPower")
        {
            discription = Status_AbilityPower(value, player);
        }
        else if (name == "AttackSpeed")
        {
            discription = Status_AttackSpeed(value, player);
        }
        else if (name == "Armor")
        {
            discription = Status_Armor(value, player);
        }
        else if (name == "CriticalChance")
        {
            discription = Status_CriticalChance(value);
        }
        else if (name == "CriticalDamage")
        {
            discription = Status_CriticalDamage(value);
        }
        else if (name == "CoolDown")
        {
            discription = Status_CoolDown(value);
        }
        else if (name == "MoveSpeed")
        {
            discription = Status_MoveSpeed(value, player);
        }
        else if (name == "Health")
        {
            discription = Status_Health(value);
        }
        else if (name == "Mana")
        {
            discription = Status_Mana(value);
        }
        else if (name == "Exp")
        {
            discription = Status_Exp(value);
        }


        return discription;
    }

    public string Status_AttackDamage(string value, Entity player)
    {
        var level = player.level;
        string index = 
            "현재 공격력 : <color=#ED7D31>"+value+"<b></b></color> (기본"+player.attackDamageOrigin(level)+
            " + 추가 "+(int.Parse(value) - player.attackDamageOrigin(level)+") \n \n"
            + "기본 공격 시 <color=#ED7D31>" + value + "<b></b></color>의 물리 피해를 입힙니다.");

        return index;
    }

    public string Status_AbilityPower(string value, Entity player)
    {
        var level = player.level;
        string index =
            "현재 주문력 : <color=#7030A0>" + value + "<b></b></color> (기본" + player.abilityPowerOrigin(level) +
            " + 추가 " + (int.Parse(value) - player.abilityPowerOrigin(level) + ") \n \n"
            + "스킬 공격 시 <color=#7030A0>" + value + "<b></b></color>의 마법 피해를 입힙니다.");

        return index;
    }

    public string Status_AttackSpeed(string value, Entity player)
    {
        Debug.Log("status");
        var level = player.level;
        string index =
            "현재 공격 속도 : <color=#FFD966>" + value + "<b></b></color>";

        return index;
    }

    public string Status_Armor(string value, Entity player)
    {
        var level = player.level;
        string index =
            "현재 방어력 : <color=#21DAFF>" + value+ " <b></b></color>( 기본 " + player.armorOrigin(level)+
            " + 추가 "+(int.Parse(value) - player.armorOrigin(level)+") \n \n"
            +"물리 피해를 x% 만큼 덜 받습니다.");
        
        return index;
    }

    public string Status_CriticalChance(string value)
    {
        string index =
            "현재 치명타 확률 : <color=#F5530B>" + value + "<b></b></color>";

        return index;
    }

    public string Status_CriticalDamage(string value)
    {
        string index =
            "현재 치명타 추가 데미지 : <color=#F5530B>" + value + "<b></b></color>";

        return index;
    }

    public string Status_CoolDown(string value)
    {
        string index =
            "현재 스킬 재사용 시간 단축 : <color=#CFB855>" + value + "<b></b></color>" +
            "\n * 최대 재사용 시간 단축 : 80%";

        return index;
    }

    public string Status_MoveSpeed(string value, Entity player)
    {
        var level = player.level;
        string index =
            "현재 이동 속도 : <color=#ECFA32>" + value + "<b></b></color>";

        return index;
    }

    public string Status_Health(string value)
    {
        var level = player.level;
        string index =
            "현재 체력 : " + value;

        return index;
    }

    public string Status_Mana(string value)
    {
        var level = player.level;
        string index =
            "현재 마나 : " + value;

        return index;
    }

    public string Status_Exp(string value)
    {
        string index =
            "현재 경험치 : " + value;

        return index;
    }
}
