using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdgePillar : MonoBehaviour
{
    public enum PillarType
    {
        leftTop, rightTop, leftBottom, rightBottom
    }
    public PillarType pillarType;
    public Transform pillarPos;
    //public bool isUpdate = false;

}
