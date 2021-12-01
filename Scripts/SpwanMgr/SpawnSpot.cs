using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSpot : MonoBehaviour
{
    // Start is called before the first frame update

    bool SpawnComplete = false;

    public bool CanUseSpawnPoint()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 1.0f);
        foreach(Collider co in colliders)
        {
            var entity = co.GetComponent<Entity>();
            if (entity)
                return false;
        }
        return true;
    }
}
