using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    public enum WallType
    {
        left, top, right, bottom
    }
    public WallType wallType;
    public Transform[] wallPos;
    public bool isSetUp = true;
    public bool isUpdate = false;
    public bool isMinimap = false;
    public bool isAlive = false;

    public List<GameObject> wallPrefabs;
    
    public void Awake()
    {
        if(wallPrefabs.Count > 0)
            createdWall();
    }

    void createdWall()
    {        
        // 기본 벽, 나머지 종류 외 6가지 선택
        double[] persent = { 0.8, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1 };

        for (int i = 0; i < wallPos.Length; i++)
        {
            int thisPatten = (int)Choose(persent);

            GameObject newWall = Instantiate(wallPrefabs[thisPatten], wallPos[i]);
            newWall.gameObject.transform.SetParent(wallPos[i]);
            //newWall.gameObject.transform.position = Vector3.zero;
        }
    }

    public void isAliveVisible(bool visible)
    {
        isAlive = visible;
    }
    // 확률을 계산하여 패턴을 구성
    public double Choose(double[] probs)
    {
        double total = 0;

        foreach (double elem in probs)
            total += elem;

        double randomPoint = Random.value * total;

        for (int i = 0; i < probs.Length; i++)
        {
            if (randomPoint < probs[i])
                return i;
            else
                randomPoint -= probs[i];
        }
        return probs.Length - 1;
    }
}
