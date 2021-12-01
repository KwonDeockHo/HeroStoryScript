using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomPrefabsSet : MonoBehaviour
{
    public static RoomPrefabsSet Instance;

    [SerializeField]
    public Dictionary<RoomName, GameObject> roomPrefabs = new Dictionary<RoomName, GameObject>();
    public List<RoomName> roomPrefabsName;
    public List<GameObject> roomPrefabsList;


    // Start is called before the first frame update
    void Awake()
    {
        if (!Instance)
            Instance = this;
        else
            Destroy(this);


        for (int i = 0; i < roomPrefabsName.Count; i++)
        {
            roomPrefabs.Add(roomPrefabsName[i], roomPrefabsList[i]);
        }
        
    }
    
}
