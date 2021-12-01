using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorEffect : MonoBehaviour
{
    public Entity owner;
    public SkillTemplate skill;

    // 캐릭터 위주 스킬 범위 및 스킬 방향 조정
    public float setlifeTime;
    public float lifeTime;
    public bool isShow = false;

    public void Start()
    {
        ShowEffectIndicator();
        HideEffectIndicator();
    }
    public void ShowEffectIndicator()
    {
        lifeTime = setlifeTime;
        isShow = true;
        transform.position = Vector3.zero;
        transform.gameObject.SetActive(true);

    }

    public void HideEffectIndicator()
    {
        lifeTime = setlifeTime;
        transform.gameObject.SetActive(false);
        transform.position = Vector3.zero;
    }

    public void deleteEffectIndicator()
    {
        if (owner != null || owner.state == State.Dead)
            Destroy(transform);
    }

    public void Update()
    {
        //if (isShow)
        //{
        //    if (lifeTime < 0)
        //        HideEffectIndicator();
        //    else
        //        lifeTime -= Time.deltaTime;
        //}
    }
}
