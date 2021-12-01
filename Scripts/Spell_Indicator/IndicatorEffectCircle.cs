using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorEffectCircle : MonoBehaviour
{
    public Entity owner;

    // 캐릭터 위주 스킬 범위 및 스킬 방향 조정
    public float setlifeTime;
    public float lifeTime;
    public bool isShow = false;

    public void ShowEffectIndicator()
    {
        lifeTime = setlifeTime;
        isShow = true;
        transform.gameObject.SetActive(true);
    }

    public void HideEffectIndicator()
    {
        lifeTime = setlifeTime;
        transform.gameObject.SetActive(false);

        transform.SetParent(owner.transform);
        transform.position = Vector3.zero;
    }

    public void deleteEffectIndicator()
    {
        if (owner != null || owner.state == State.Dead)
            Destroy(transform);
    }

    public void Update()
    {
        if (isShow)
        {
            if (lifeTime < 0)
                HideEffectIndicator();
            else
                lifeTime -= Time.deltaTime;
        }
    }
    public void DrawIndicator(Skill _skill, Vector3 start, Vector3 end, float size)
    {

    }
    public void updateIndicatorSize(Vector3 centor, Vector3 destination)
    {

    }

}
