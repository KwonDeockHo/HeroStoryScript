using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class memo : MonoBehaviour
{
    // Start is called before the first frame update
    //public bool searchDistanceRoom(Vector3Int pos, int distance, string roomName, int maxCount)
    //{
    //    // 특정 거리의 방 List 출력
    //    List<Vector3Int> list = new List<Vector3Int>();

    //    Vector3Int leftTop = pos + new Vector3Int(-distance, 0, -distance);
    //    Vector3Int leftBottom = pos + new Vector3Int(-distance, 0, distance);
    //    Vector3Int rightTop = pos + new Vector3Int(distance, 0, -distance);
    //    Vector3Int rightBottom = pos + new Vector3Int(distance, 0, distance);
    //    // TOP
    //    for (int i = 0; i < Mathf.Abs(rightTop.x - leftTop.x); i++)
    //    {
    //        if (0 <= (leftTop.x + i) && (leftTop.x + i) <= (maxDistance * 2)
    //            && 0 <= leftTop.z && leftTop.z <= (maxDistance * 2))
    //        {
    //            if (posArr[leftTop.x + i, leftTop.z].roomState)
    //            {
    //                Vector3Int listData = new Vector3Int(leftTop.x + i, 0, leftTop.z);
    //                list.Add(listData);
    //            }
    //        }
    //    }
    //    // LEFT
    //    for (int i = 0; i < Mathf.Abs(leftTop.z - leftBottom.z); i++)
    //    {
    //        if (0 <= (leftTop.z + i) && (leftTop.z + i) <= (maxDistance * 2)
    //            && 0 <= (leftTop.x) && (leftTop.x) <= (maxDistance * 2))
    //        {
    //            if (posArr[leftTop.x, leftTop.z + i].roomState)
    //            {
    //                Vector3Int listData = new Vector3Int(leftTop.x, 0, leftTop.z + i);
    //                list.Add(listData);

    //                //list.Add(posArr[leftTop.x, leftTop.z + i].currPos);
    //            }
    //        }
    //    }

    //    // RIGHT
    //    for (int i = 0; i < Mathf.Abs(rightTop.z - rightBottom.z); i++)
    //    {
    //        if (0 <= (rightTop.z + i) && (rightTop.z + i) <= (maxDistance * 2)
    //            && 0 <= rightTop.x && rightTop.x <= (maxDistance * 2))
    //        {
    //            if (posArr[rightTop.x, (rightTop.z + i)].roomState)
    //            {
    //                Vector3Int listData = new Vector3Int(rightTop.x, 0, (rightTop.z + i));
    //                list.Add(listData);

    //                //list.Add(posArr[rightTop.x, (rightTop.z + i)].currPos);
    //            }
    //        }
    //    }

    //    // Bottom
    //    for (int i = 0; i < Mathf.Abs(rightBottom.x - leftBottom.x); i++)
    //    {
    //        if (0 <= leftBottom.z + i && leftBottom.z + i <= (maxDistance * 2)
    //            && 0 <= leftBottom.x && leftBottom.x <= (maxDistance * 2))
    //        {
    //            if (posArr[(leftBottom.x + i), leftBottom.z].roomState)
    //            {
    //                Vector3Int listData = new Vector3Int(leftBottom.x + i, 0, leftBottom.z);
    //                list.Add(listData);

    //                //                    list.Add(posArr[(leftBottom.x + i), leftBottom.z].currPos);
    //            }
    //        }
    //    }
    //    int cnt = 0;
    //    if (list.Count <= 0)
    //    {
    //        return false;
    //    }
    //    else
    //    {
    //        int rand = Random.Range(0, list.Count);

    //        Vector3Int tmp = list[rand];
    //        posArr[tmp.x, tmp.z].roomName = roomName;
    //        cnt++;
    //        return true;


    //    }
    //}
}
