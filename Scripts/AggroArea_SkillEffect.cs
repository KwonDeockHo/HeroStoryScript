using UnityEngine;

public class AggroArea_SkillEffect : MonoBehaviour
{
    public SkillEffect owner;
    public bool isTriggerEnter = true;
    public bool isTriggerStay = false;

    public bool useLifeTime = false;
    public float lifeTime = 0f;
    float timer = 0f;

    [HideInInspector] public Vector3 hitPoint;
    [HideInInspector] public Quaternion hitRot;
    void OnTriggerEnter(Collider co)
    {
        if (!isTriggerEnter) return;
        if (co.gameObject.layer.Equals("SkillEffect")) return;
        var entity = co.GetComponentInParent<Entity>();

        if (entity && owner)
        {
            hitPoint = co.ClosestPointOnBounds(transform.position);
            owner.SethitPoint(hitPoint);
            owner.OnAggroEnter(entity);
        }
    }

    void OnTriggerStay(Collider co)
    {
        if (!isTriggerStay) return;
        if (co.gameObject.layer.Equals("SkillEffect")) return;
        var entity = co.GetComponentInParent<Entity>();
        if (entity && owner)
        {
            hitPoint = co.ClosestPointOnBounds(transform.position);

            owner.OnAggroStay(entity);
        }
    }
    private void OnTriggerExit(Collider co)
    {
        if (!isTriggerEnter) return;
        if (co.gameObject.layer.Equals("SkillEffect")) return;
        var entity = co.GetComponentInParent<Entity>();
        if (entity && owner) {
            owner.OnAggroExit(entity);
        }
    }

    private void Update()
    {
        if (!useLifeTime) return;

        if (timer >= lifeTime)
            gameObject.SetActive(false);
        timer += Time.deltaTime;
    }
}