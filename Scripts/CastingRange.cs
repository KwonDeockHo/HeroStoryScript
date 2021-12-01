   using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class CastingRange : MonoBehaviour
{
    public static CastingRange self;

    public bool isCasting = false;

    [Range(0, 50)]
    public int segments = 50;
    [Range(0, 20)]
    public float xradius = 20;
    [Range(0, 20)]
    public float yradius = 20;
    LineRenderer line;

    void Start()
    {
        line = transform.GetComponent<LineRenderer>();
        line.positionCount = segments + 1;
        line.useWorldSpace = false;

        line.startWidth = 0;
        line.endWidth = 0;

        transform.SetLossyScale(new Vector3(1,1,1));
    }

    public void SkillCastingRange(bool isCasting)
    {
        if (!isCasting)
        {
            line.startWidth = 0;
            line.endWidth = 0;

            xradius = 0f;
            yradius = 0f;
        }
        CreatePoints();
    }

    public void SkillCastingRange(bool isCasting, Skill skill, float size = 0f)
    {
        if (isCasting == true)
        {
            line.startWidth = 0.1f;
            line.endWidth = 0.1f;

            xradius = size;
            yradius = size;
        }
        else
        {
            line.startWidth = 0;
            line.endWidth = 0;

            xradius = 0f;
            yradius = 0f;
        }
        CreatePoints();
    }

    public void CreatePoints()
    {
        float x;
        float y;

        float angle = 20f;

        for (int i = 0; i < (segments + 1); i++)
        {
            x = Mathf.Sin(Mathf.Deg2Rad * angle) * xradius;
            y = Mathf.Cos(Mathf.Deg2Rad * angle) * yradius;

            line.SetPosition(i, new Vector3(x, y, 0));

            angle += (360f / segments);
        }
    }
}