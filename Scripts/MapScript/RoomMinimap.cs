using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomMinimap : MonoBehaviour
{
    //public List<GameObject> minimapRoomWall;
    public GameObject floorMap;
    public Texture noneWallTexture;
    public int nonWallFixedLength = 10;

    public List<Wall> walls;
    public Wall leftWall;
    public Wall rightWall;
    public Wall topWall;
    public Wall bottomWall;

    public Vector3 minimapRoomPos;

    public GameObject currRoom;
    public bool visited = false;
    public Material minimapMaterial;
    public Material minimapWallMaterial;

    public bool minimapAlive = false;
    public Wall[] ws;

    // Start is called before the first frame update
    public void Awake()
    {
        transform.gameObject.SetActive(false);


        ws = GetComponentsInChildren<Wall>();


        minimapMaterial = RoomController.Instance.DefaultBackground;
        floorMap.GetComponentInChildren<MeshRenderer>().material = minimapMaterial;

        foreach (Wall w in ws)
        {
            // Door 리스트에 Door를 삽입(
            walls.Add(w);
            w.GetComponentInChildren<MeshRenderer>().material.color = minimapMaterial.color;

            switch (w.wallType)
            {
                case Wall.WallType.left:
                    leftWall = w;
                    break;
                case Wall.WallType.top:
                    topWall = w;
                    break;
                case Wall.WallType.right:
                    rightWall = w;
                    break;
                case Wall.WallType.bottom:
                    bottomWall = w;
                    break;
            }
        }

        minimapWallset(false);
    }

    // Update is called once per frame
    
    public void VisitiedRoom(bool boolean, bool currBool)
    {

        transform.gameObject.SetActive(boolean);
        if (currBool)
        {
            visited = true;
        }
        minimapAlive = boolean;
        minimapWallset(true);
    }
    public void MaterialTextureChange(RoomType wallType)
    {
        if(noneWallTexture != null && wallType == RoomType.Triple)
        {
            topWall.GetComponentInChildren<MeshRenderer>().material.mainTexture = noneWallTexture;
        }
        else if(noneWallTexture != null && wallType == RoomType.Quad)
        {
            topWall.GetComponentInChildren<MeshRenderer>().material.mainTexture = noneWallTexture;
            GameObject changeWall = topWall.GetComponentInChildren<MeshRenderer>().gameObject;
            //// scale += 10
            // position -5
            Vector3 newPos = new Vector3(changeWall.transform.position.x - (nonWallFixedLength / 2), changeWall.transform.position.y + 0.1f, changeWall.transform.position.z);
            changeWall.transform.position = newPos;

            Vector3 newScale = new Vector3(changeWall.transform.localScale.x + nonWallFixedLength, changeWall.transform.localScale.y, changeWall.transform.localScale.z);
            changeWall.transform.localScale = newScale;



            
        }
    }

    public void VisitiedCurrRoom(bool boolean)
    {

        // 4. 현재 위치 밝게 처리
        if (boolean)
        {
            minimapMaterial = RoomController.Instance.currMaterial;
        }
        else
        {
            if (visited)
            {
                minimapMaterial  = RoomController.Instance.VisitedBack;
            }
            else
            {
                minimapMaterial = RoomController.Instance.DefaultBackground;
            }
        }
        floorMap.GetComponentInChildren<MeshRenderer>().material = minimapMaterial;

        foreach (Wall w in ws){
            w.GetComponentInChildren<MeshRenderer>().material.color = minimapMaterial.color;
        }
    }

    public void minimapWallset(bool boolean)
    {
        if (visited || boolean)
        {

            for (int i = 0; i < walls.Count; i++)
            {
                if (walls[i].isSetUp)
                {
                    walls[i].transform.gameObject.SetActive(boolean);
                }
            }
        }
        else
            for (int i = 0; i < walls.Count; i++)
            {
                if (walls[i].isSetUp)
                    walls[i].transform.gameObject.SetActive(boolean);
            }
    }
}
