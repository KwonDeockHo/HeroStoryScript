using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Spell_Indicator_image : MonoBehaviour
{
    // 1. 스킬의 모양
    // 2. 스킬의 색상
    // 3. 스킬의 갯수
    // 4. 스킬의 패턴

    public GameObject indicator_object;
    public SpriteRenderer indicator_image;


    public void SetActive()
    {
        transform.gameObject.SetActive(true);
    }

    public void SetHide()
    {
        transform.gameObject.SetActive(false);
    }

}
