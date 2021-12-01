using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class floorMaterialMgr : MonoBehaviour
{
    // Start is called before the first frame update
    public void updateFloorMaterial(Material changeMaterial)
    {
        transform.GetComponentInChildren<MeshRenderer>().material = changeMaterial;
    }
}
