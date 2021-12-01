using UnityEngine;

public class OneTimeTargetSkillEffect : SkillEffect
{
    float lifeTime = 3;

    void Update()
    {
        if (target != null)
            transform.position = target.collider.bounds.center;

        if (target == null || lifeTime <= 0)
            Destroy(gameObject);
        lifeTime -= Time.deltaTime;
    }
}
