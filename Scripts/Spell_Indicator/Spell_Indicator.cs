using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell_Indicator : MonoBehaviour
{
    // 캐릭터 위주 스킬 범위 및 스킬 방향 조정
    public Entity owner;

    public float setlifeTime;
    public float lifeTime;

    public void showEffectIndicator()
    {
        lifeTime = setlifeTime;
        transform.gameObject.SetActive(true);
    }
    public void deleteEffectIndicator()
    {
        if (owner != null || owner.state == State.Dead)
            Destroy(transform);
    }

    public void updateIndicatorSize(Vector3 centor, Vector3 destination)
    {

    }

}
