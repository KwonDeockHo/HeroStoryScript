using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SubRoom : MonoBehaviour
{
    public int Width;
    public int Height;

    public RoomName roomName;
    public RoomType roomType;

    // 각 방의 문을 세팅
    public List<Door> doors;
    public Door leftDoor;
    public Door rightDoor;
    public Door topDoor;
    public Door bottomDoor;

    public List<Wall> walls;
    public Wall leftWall;
    public Wall rightWall;
    public Wall topWall;
    public Wall bottomWall;

    public List<EdgePillar> pillars;
    public EdgePillar leftTopPillar;
    public EdgePillar rightTopPillar;
    public EdgePillar leftBottomPillar;
    public EdgePillar rightBottomPillar;

    // 현재 방 위치
    public Vector3Int currPos;
    public Vector3Int parentPos;
    public Vector3 CenterPos;
    public string wallType;
    public floorMaterialMgr floors;
    public Material floor;

    public Room parentRoom;

    public bool roomPathBool = false;

    // 현재 방이 히든 방과 연결된 문이 오픈되어 있다면 true, 아니라면 false
    public bool hiddenUpdate = false;

    public RoomMinimap minimapRoom;


    // Start is called before the first frame update
    public void SubRoomInit()
    {
        Door[] ds = GetComponentsInChildren<Door>();

        foreach (Door d in ds)
        {
            // Door 리스트에 Door를 삽입(
            doors.Add(d);

            switch (d.doorType)
            {
                case DoorType.right:
                    rightDoor = d;
                    break;
                case DoorType.left:
                    leftDoor = d;
                    break;
                case DoorType.top:
                    topDoor = d;
                    break;
                case DoorType.bottom:
                    bottomDoor = d;
                    break;
            }
        }

        Wall[] ws = GetComponentsInChildren<Wall>();

        foreach (Wall w in ws)
        {
            // Door 리스트에 Door를 삽입(
            walls.Add(w);

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

        EdgePillar[] ps = GetComponentsInChildren<EdgePillar>();

        foreach (EdgePillar p in ps)
        {
            // Door 리스트에 Door를 삽입(
            pillars.Add(p);

            switch (p.pillarType)
            {
                case EdgePillar.PillarType.leftTop:
                    leftTopPillar = p;
                    break;
                case EdgePillar.PillarType.rightTop:
                    rightTopPillar = p;
                    break;
                case EdgePillar.PillarType.leftBottom:
                    leftBottomPillar = p;
                    break;
                case EdgePillar.PillarType.rightBottom:
                    rightBottomPillar = p;
                    break;
            }
        }

        floors = GetComponentInChildren<floorMaterialMgr>();

    }
    public void setRoomPath(string wallStr)
    {
        if (roomPathBool)
            return;

        List<MapObj> typeMapObj = new List<MapObj>();
        RoomObj thisroom = this.transform.GetComponentInChildren<RoomObj>();

        string roomPatten = "";

        thisroom.roomPatten.TryGetValue(wallStr, out roomPatten);

        //for (int i = 0; i < mapObjSetting.Instance.randMapObj.Count; i++)
        //{
        //    //Debug.Log("WallName : " + wallStr + ",  RoomType - name : " + mapObjSetting.Instance.randMapObj[i].roomtype + ", roomPatten : " + roomPatten);

        //    /// 방의 패턴을 roomPatten을 반환(None, Curve, Straight, Triple, Quad)
        //    if (mapObjSetting.Instance.randMapObj[i].roomtype == roomPatten)
        //    {
        //        typeMapObj.Add(mapObjSetting.Instance.randMapObj[i]);
        //    }
        //}

        int objPrefabIdx = Random.Range(0, mapObjSetting.Instance.objTemplete.Length);

        if (!roomPathBool)
        {
            GameObject thisPatten = mapObjSetting.Instance.objTemplete[objPrefabIdx];
            GameObject newObj = Instantiate(thisPatten, thisroom.transform.position, this.transform.rotation);
            newObj.transform.SetParent(this.transform.GetComponentInChildren<RoomObj>().gameObject.transform);

        }

        Vector3 objRotate = new Vector3(0, 1, 0);
        int rotateValue = 0;

        if (thisroom.doorPatten.TryGetValue(wallType, out rotateValue))
        {
            this.transform.GetComponentInChildren<RoomObj>().gameObject.transform.rotation
                    = Quaternion.Euler(objRotate * rotateValue);
        }
        //Debug.Log("Room Rotate : " + rotateValue);

        roomPathBool = true;
    }


    public void updateRoomSetup()
    {
        if (!roomType.Equals("Single"))
        {
            parentRoom = RoomController.Instance.FindRoom(parentPos.x, parentPos.y, parentPos.z);
            if (parentRoom != null)
            {
                GameObject subRoom = this.gameObject;
                subRoom.transform.SetParent(parentRoom.transform);
                //subRoom.transform.parent.GetComponent<Room>().setUpdateWalls(false);
                subRoom.transform.GetComponent<SubRoom>().floors.updateFloorMaterial(parentRoom.rooms.floor);

                GameObject miniRoom = minimapRoom.gameObject;
                miniRoom.transform.SetParent(parentRoom.transform);

            }
        }

        switch (roomName)
        {
            case RoomName.Shop:
                ShopRoomSetup();
                break;
            case RoomName.Hidden:
                HiddenRoomSetup();
                break;
            case RoomName.Battle:
                //BattleRoomSetup();
                setRoomPath(wallType);
                break;
            case RoomName.Item:
                ItemRoomSetup();
                break;
            default:
                setRoomPath(wallType);
                break;
        }
    }

    public void ShopRoomSetup()
    {
        if (roomPathBool) // 시작
            return;

        GameObject go = Instantiate(mapObjSetting.Instance.shopObject);
        go.transform.SetParent(transform.GetComponentInChildren<RoomObj>().gameObject.transform);
        go.transform.localPosition = Vector3.zero;


        roomPathBool = true; // 끝ㄴ
    }

    public void HiddenRoomSetup()
    {
        if (roomPathBool) // 시작
            return;

        GameObject go = Instantiate(mapObjSetting.Instance.chests[Random.Range(0, mapObjSetting.Instance.chests.Length)].gameObject);
        go.transform.SetParent(transform.GetComponentInChildren<RoomObj>().gameObject.transform);
        go.transform.localPosition = Vector3.zero;

        roomPathBool = true; // 끝
    }

    public void BattleRoomSetup()
    {
        if (roomPathBool) // 시작
            return;







        roomPathBool = true; // 끝
    }

    public void ItemRoomSetup()
    {
        if (roomPathBool) // 시작
            return;


        GameObject go = Instantiate(mapObjSetting.Instance.itemObject);
        go.transform.SetParent(transform.GetComponentInChildren<RoomObj>().gameObject.transform);
        go.transform.localPosition = Vector3.zero;


        roomPathBool = true; // 끝
    }
    public void RemoveUnconnectedEdge()
    {
        foreach (EdgePillar pillar in pillars)
        {
            switch (pillar.pillarType)
            {
                case EdgePillar.PillarType.leftTop:
                    if (GetLeft() != null && GetTop() != null)
                    {
                        if (GetLeft().parentPos == parentPos && GetTop().parentPos == parentPos)
                        {
                            leftTopPillar.gameObject.SetActive(false);
                        }
                    }
                    break;
                case EdgePillar.PillarType.rightTop:
                    if (GetRight() != null && GetTop() != null)
                    {
                        if (GetRight().parentPos == parentPos && GetTop().parentPos == parentPos)
                        {
                            rightTopPillar.gameObject.SetActive(false);
                        }
                    }
                    break;
                case EdgePillar.PillarType.leftBottom:
                    if (GetLeft() != null && GetBottom() != null)
                    {
                        if (GetLeft().parentPos == parentPos && GetBottom().parentPos == parentPos)
                        {
                            leftBottomPillar.gameObject.SetActive(false);
                        }
                    }
                    break;
                case EdgePillar.PillarType.rightBottom:
                    if (GetRight() != null && GetBottom() != null)
                    {
                        if (GetRight().parentPos == parentPos && GetBottom().parentPos == parentPos)
                        {
                            rightBottomPillar.gameObject.SetActive(false);
                        }
                    }
                    break;
            }
        }
    }
    public void minimapUpdate()
    {
        // 0. 현재 맵을 setActive = true
        //visitedRoom = true;

        for (int i = 0; i < RoomController.Instance.loadedRooms.Count; i++)
        {
            if (parentPos == RoomController.Instance.loadedRooms[i].parentPos)
            {
                RoomController.Instance.loadedRooms[i].visitedRoom = true;
                RoomController.Instance.loadedRooms[i].rooms.minimapRoom.VisitiedRoom(true, true);
                RoomController.Instance.loadedRooms[i].rooms.minimapRoom.VisitiedCurrRoom(true);
            }
            else
            {
                RoomController.Instance.loadedRooms[i].rooms.minimapRoom.VisitiedCurrRoom(false);
            }
        }

        // 2. 해당 인접한 Room에 대해서 visible 
        if (GetRight() != null)
            minimapUpdateSide(GetRight());

        if (GetLeft() != null)
            minimapUpdateSide(GetLeft());

        if (GetTop() != null)
            minimapUpdateSide(GetTop());

        if (GetBottom() != null)
            minimapUpdateSide(GetBottom());
    }
    public void minimapUpdateSide(Room room)
    {
        for (int i = 0; i < RoomController.Instance.loadedRooms.Count; i++)
        {
            if (room.parentPos == RoomController.Instance.loadedRooms[i].parentPos)
            {
                // 3. Hidden방 처리 : 직접 방문해야 minimap에 표시
                if (room.roomName != RoomName.Hidden)
                {
                    RoomController.Instance.loadedRooms[i].rooms.minimapRoom.VisitiedRoom(true, false);
                    //RoomController.Instance.loadedRooms[i].rooms.minimapRoom.VisitiedRoom(true, false);
                    //   minimapCam.Instance.moveMiniCamCamera(parentPos);
                }
            }
        }
    }
    // 벽, 미니맵 수정
    // 벽, 미니맵 수정
    public void RemoveUnconnectedWalls()
    {
        Vector3 tmpCenterPos = transform.parent.gameObject.GetComponent<Room>().parentPos;
        string wallStr = "";
        bool[] wallIndex = { false, false, false, false };
        string[] wallname = { "L", "T", "R", "B" };

        foreach (Wall wall in walls)
        {
            switch (wall.wallType)
            {
                case Wall.WallType.left:
                    if (GetLeft() != null)
                    {
                        Room leftRoom = GetLeft();

                        if (leftRoom.parentPos == tmpCenterPos)
                        {
                            leftDoor.gameObject.SetActive(false);
                            leftWall.gameObject.SetActive(false);

                            minimapRoom.leftWall.gameObject.SetActive(true);
                            minimapRoom.leftWall.isAliveVisible(true);
                            minimapRoom.leftWall.isSetUp = false;

                        }
                        else
                        {
                            wallIndex[0] = true;
                            if (!leftDoor.isUpdate)
                            {
                                GameObject roomDoor = Instantiate(leftRoom._nDoor, leftDoor.transform);
                                roomDoor.gameObject.transform.SetParent(leftDoor.gameObject.transform);

                                if (leftRoom.roomName == RoomName.Hidden)
                                {
                                    GameObject secRoomDoor = Instantiate(leftRoom.secondDoor, leftDoor.transform);
                                    secRoomDoor.gameObject.transform.SetParent(leftDoor.gameObject.transform);
                                    secRoomDoor.gameObject.transform.SetAsFirstSibling();
                                    secRoomDoor.gameObject.SetActive(false);
                                }


                                leftDoor.setNextRoom(leftRoom);
                                leftDoor.setThisRoom(this);
                                leftDoor.SideDoor = leftRoom.rooms.rightDoor;
                                leftDoor.setSideDoorPos();

                                leftDoor.isUpdate = true;
                            }
                            minimapRoom.leftWall.gameObject.SetActive(false);
                            minimapRoom.leftWall.isSetUp = false;
                        }
                    }
                    else
                    {
                        if (!leftWall.isUpdate)
                        {
                            GameObject newWall = transform.parent.GetComponent<Room>().Wall.gameObject;
                            GameObject roomWall = Instantiate(newWall, leftWall.transform);
                            leftWall.isUpdate = true;
                        }
                        minimapRoom.leftWall.gameObject.SetActive(false);
                        leftDoor.gameObject.SetActive(false);
                        minimapRoom.leftWall.isSetUp = false;
                    }
                    break;

                case Wall.WallType.top:

                    if (GetTop() != null)
                    {
                        Room topRoom = GetTop();

                        if (topRoom.parentPos == tmpCenterPos)
                        {
                            topDoor.gameObject.SetActive(false);
                            topWall.gameObject.SetActive(false);
                            minimapRoom.topWall.gameObject.SetActive(true);
                            minimapRoom.topWall.isAliveVisible(true);

                            minimapRoom.topWall.isSetUp = false;
                        }
                        else
                        {
                            wallIndex[1] = true;
                            if (!topDoor.isUpdate)
                            {
                                GameObject roomDoor = Instantiate(topRoom._nDoor, topDoor.transform);
                                roomDoor.gameObject.transform.SetParent(topDoor.gameObject.transform);


                                if (topRoom.roomName == RoomName.Hidden)
                                {
                                    GameObject secRoomDoor = Instantiate(topRoom.secondDoor, topDoor.transform);
                                    secRoomDoor.gameObject.transform.SetParent(topDoor.gameObject.transform);
                                    secRoomDoor.gameObject.transform.SetAsFirstSibling();
                                    secRoomDoor.gameObject.SetActive(false);
                                }

                                topDoor.setNextRoom(topRoom);
                                topDoor.setThisRoom(this);
                                topDoor.SideDoor = topRoom.rooms.bottomDoor;
                                topDoor.setSideDoorPos();

                                topDoor.isUpdate = true;
                            }
                            minimapRoom.topWall.gameObject.SetActive(false);
                            minimapRoom.topWall.isSetUp = false;
                        }
                    }
                    else
                    {
                        if (!topWall.isUpdate)
                        {
                            GameObject newWall = transform.parent.GetComponent<Room>().Wall.gameObject;
                            GameObject roomWall = Instantiate(newWall, topWall.transform);
                            topWall.isUpdate = true;
                        }
                        minimapRoom.topWall.gameObject.SetActive(false);
                        minimapRoom.topWall.isSetUp = false;
                        topDoor.gameObject.SetActive(false);
                    }
                    break;

                case Wall.WallType.right:
                    if (GetRight() != null)
                    {
                        Room rightRoom = GetRight();
                        if (rightRoom.parentPos == tmpCenterPos)
                        {
                            rightDoor.gameObject.SetActive(false);
                            rightWall.gameObject.SetActive(false);

                            minimapRoom.rightWall.gameObject.SetActive(true);
                            minimapRoom.rightWall.isAliveVisible(true);
                            minimapRoom.rightWall.isSetUp = false;

                        }
                        else
                        {
                            wallIndex[2] = true;
                            if (!rightDoor.isUpdate)
                            {
                                GameObject roomDoor = Instantiate(rightRoom._nDoor, rightDoor.transform);
                                roomDoor.gameObject.transform.SetParent(rightDoor.gameObject.transform);

                                if (rightRoom.roomName == RoomName.Hidden)
                                {
                                    GameObject secRoomDoor = Instantiate(rightRoom.secondDoor, rightDoor.transform);
                                    secRoomDoor.gameObject.transform.SetParent(rightDoor.gameObject.transform);
                                    secRoomDoor.gameObject.transform.SetAsFirstSibling();
                                    secRoomDoor.gameObject.SetActive(false);
                                }

                                rightDoor.setThisRoom(this);
                                rightDoor.setNextRoom(rightRoom);
                                rightDoor.SideDoor = rightRoom.rooms.leftDoor;
                                rightDoor.setSideDoorPos();

                                rightDoor.isUpdate = true;
                            }
                            minimapRoom.rightWall.gameObject.SetActive(false);
                            minimapRoom.rightWall.isSetUp = false;
                        }
                    }
                    else
                    {
                        if (!rightWall.isUpdate)
                        {
                            GameObject newWall = transform.parent.GetComponent<Room>().Wall.gameObject;
                            GameObject roomWall = Instantiate(newWall, rightWall.transform);
                            rightWall.isUpdate = true;
                        }
                        rightDoor.gameObject.SetActive(false);
                        minimapRoom.rightWall.gameObject.SetActive(false);
                        minimapRoom.rightWall.isSetUp = false;
                    }
                    break;

                case Wall.WallType.bottom:
                    if (GetBottom() != null)
                    {
                        Room bottomRoom = GetBottom();

                        if (bottomRoom.parentPos == tmpCenterPos)
                        {
                            // 방이 뚫려 있다.
                            bottomDoor.gameObject.SetActive(false);
                            bottomWall.gameObject.SetActive(false);

                            minimapRoom.bottomWall.gameObject.SetActive(true);
                            minimapRoom.bottomWall.isAliveVisible(true);

                            minimapRoom.bottomWall.isSetUp = false;

                        }
                        else
                        {
                            wallIndex[3] = true;
                            if (!bottomDoor.isUpdate)
                            {
                                GameObject roomDoor = Instantiate(bottomRoom._nDoor, bottomDoor.transform);
                                roomDoor.gameObject.transform.SetParent(bottomDoor.gameObject.transform);

                                if (bottomRoom.roomName == RoomName.Hidden)
                                {
                                    GameObject secRoomDoor = Instantiate(bottomRoom.secondDoor, bottomDoor.transform);
                                    secRoomDoor.gameObject.transform.SetParent(bottomDoor.gameObject.transform);
                                    secRoomDoor.gameObject.transform.SetAsFirstSibling();
                                    secRoomDoor.gameObject.SetActive(false);
                                }

                                bottomDoor.setThisRoom(this);
                                bottomDoor.setNextRoom(bottomRoom);
                                bottomDoor.SideDoor = bottomRoom.rooms.topDoor;
                                bottomDoor.setSideDoorPos();

                                bottomDoor.isUpdate = true;
                            }
                            minimapRoom.bottomWall.gameObject.SetActive(false);
                            minimapRoom.bottomWall.isSetUp = false;
                        }
                    }
                    else
                    {

                        if (!bottomWall.isUpdate)
                        {
                            GameObject newWall = transform.parent.GetComponent<Room>().Wall.gameObject;
                            GameObject roomWall = Instantiate(newWall, bottomWall.transform);
                            bottomWall.isUpdate = true;
                        }
                        bottomDoor.gameObject.SetActive(false);
                        minimapRoom.bottomWall.gameObject.SetActive(false);
                        minimapRoom.bottomWall.isSetUp = false;
                    }
                    break;

            }
        }
        for (int i = 0; i < wallIndex.Length; i++)
        {
            if (wallIndex[i])
                wallStr += wallname[i];
        }

        if (wallStr != "")
            wallType = wallStr;
        else
            wallType = "None";

        if (roomType == RoomType.Quad)
        {
            RemoveUnconnectedEdge();
        }

        minimapChangeTexture(roomType);

        //Debug.Log("Rooms Subroom Init : " + parentRoom.name + " , parent ");
        updateRoomSetup();
    }
    public void minimapChangeTexture(RoomType wallType)
    {
        if (minimapRoom.topWall.isAlive && minimapRoom.leftWall.isAlive)  {
            minimapRoom.MaterialTextureChange(wallType);
        }
    }

    //  // 현재 방이 히든 방과 연결된 문이 오픈되어 있다면 true, 아니라면 false
    public void setHiddenUpdate(bool status)
    {
        hiddenUpdate = status;
    }
    public Room GetRight()
    {
        if (RoomController.Instance.DoesRoomExist(currPos.x + 1, currPos.y, currPos.z))
        {
            return RoomController.Instance.FindRoom(currPos.x + 1, currPos.y, currPos.z);
        }
        return null;
    }
    public Room GetLeft()
    {
        if (RoomController.Instance.DoesRoomExist(currPos.x - 1, currPos.y, currPos.z))
        {
            return RoomController.Instance.FindRoom(currPos.x - 1, currPos.y, currPos.z);
        }
        return null;
    }
    public Room GetTop()
    {
        if (RoomController.Instance.DoesRoomExist(currPos.x, currPos.y, currPos.z + 1))
        {
            return RoomController.Instance.FindRoom(currPos.x, currPos.y, currPos.z + 1);
        }
        return null;
    }
    public Room GetBottom()
    {
        if (RoomController.Instance.DoesRoomExist(currPos.x, currPos.y, currPos.z - 1))
        {
            return RoomController.Instance.FindRoom(currPos.x, currPos.y, currPos.z - 1);
        }
        return null;
    }

    //private void OnTriggerEnter(Collider collision)
    //{
    //    if (collision.tag == "Player" && RoomController.Instance.isLoadingRoom)
    //    {
    //        this.transform.parent.GetComponent<Room>().roomTriggerCheck();
    //    }
    //}
}
