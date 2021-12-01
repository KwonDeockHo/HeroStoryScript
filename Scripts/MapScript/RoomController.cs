using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;
using TMPro;

public class RoomController : MonoBehaviour
{
    public static RoomController Instance;

    public string currentWorldName = "Basement";

  //  public RoomInfo currentLoadRoomData;
    public Room currRoom;

    public Room startRoom;
    public Room bossRoom;
    public Room ItemRoom;
    public Room ShopRoom;
    public List<Room> HiddenRoom = new List<Room>();
    public Room BattleRoom;

    public List<Room> loadedRooms = new List<Room>();
    public int roomClearCount = 0;
    public GameObject roomPrefabs;
    public bool isLoadingRoom = false;
    public bool spawnedBossRoom = false;
    public bool spawnedMergeRoom = false;
    public bool updatedRooms = false;
    public bool createRoom = false;
    public Material DefaultBackground;
    public Material VisitedBack;
    public Material currMaterial;
    public Material[] floorMaterial;
    public Player player;

    public bool BakeNavComplete = false;
    public UI_StageMessage UI_RoomMessage;
    public UI_RoomTimer UI_RoomTimer;


    public void Awake()
    {
        if (!Instance)
            Instance = this;
        else
            Destroy(this);
    }

    public void newCreatedRoom(int nMinRoomCnt, int nMaxRoomCnt, int nMaxDistance)
    {

        isLoadingRoom = false;
        spawnedBossRoom = false;
        spawnedMergeRoom = false;
        updatedRooms = false;
        createRoom = false;

        //// navmesh delete
        //foreach (var surface in GetComponentsInChildren<NavMeshSurface>())
        //{
        //    surface.RemoveData();
        //}

        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
        loadedRooms.Clear();

        // 생성
        DungeonCrawlerController.Instance.CreatedRoom(nMinRoomCnt, nMaxRoomCnt, nMaxDistance);


        // 방 보정
        UpdateRoomQueue();


        if (player == null)
            player = Player.player;

        // 방 입장 시 Player 시점에서 발동하는 트리거
        if (player)
        {
            player.StageStart();

            player.agent.Warp(Vector3.zero);
        }

        //Vector3 updateVector = new Vector3(loadedRooms[0].Width * nMaxDistance, 20, loadedRooms[0].Height * nMaxDistance);
        LocalNavMeshBuilder.Instance.settingOnNavMeshSize(nMaxDistance);

    }
    public void HiddenRoomOpenDoor(Room isHiddenRoom)
    {
        if (isHiddenRoom.isRoomClear || isHiddenRoom.rooms.hiddenUpdate)
            return;

        foreach (Door d in isHiddenRoom.rooms.doors)
        {
            switch (d.doorType)
            {
                case DoorType.right:
                    if (d.nextRoom != null)
                    {
                        if (d.nextRoom.rooms.leftDoor.transform.childCount >= 2)
                        {
                            Destroy(d.nextRoom.rooms.leftDoor.transform.GetChild(1).gameObject);

                            d.nextRoom.rooms.leftDoor.transform.GetChild(0).gameObject.SetActive(true);
                        }
                        d.nextRoom.rooms.leftDoor.setHiddenCheck(true);

                    }
                    break;
                case DoorType.left:
                    if (d.nextRoom != null)
                    {
                        if (d.nextRoom.rooms.rightDoor.transform.childCount >= 2)
                        {
                            Destroy(d.nextRoom.rooms.rightDoor.transform.GetChild(1).gameObject);

                            d.nextRoom.rooms.rightDoor.transform.GetChild(0).gameObject.SetActive(true);
                        }
                        d.nextRoom.rooms.rightDoor.setHiddenCheck(true);

                    }
                    break;
                case DoorType.top:
                    if (d.nextRoom != null)
                    {
                        if (d.nextRoom.rooms.bottomDoor.transform.childCount >= 2)
                        {
                            Destroy(d.nextRoom.rooms.bottomDoor.transform.GetChild(1).gameObject);

                            d.nextRoom.rooms.bottomDoor.transform.GetChild(0).gameObject.SetActive(true);
                        }
                        d.nextRoom.rooms.bottomDoor.setHiddenCheck(true);

                    }
                    break;
                case DoorType.bottom:
                    if (d.nextRoom != null)
                    {
                        if (d.nextRoom.rooms.topDoor.transform.childCount >= 2)
                        {
                            Destroy(d.nextRoom.rooms.topDoor.transform.GetChild(1).gameObject);

                            d.nextRoom.rooms.topDoor.transform.GetChild(0).gameObject.SetActive(true);
                        }
                        d.nextRoom.rooms.topDoor.setHiddenCheck(true);

                    }
                    break;
            }
        }
        currRoom.openRoomDoor();
    }
    void UpdateRoomQueue()
    {
        if (isLoadingRoom) {
            return;
        }

        if (loadedRooms.Count > 0)
        {
            while (true)
            {
                bool removeCompleteState = true;

                foreach (Room room in loadedRooms)
                {
                    room.RemoveUnconnectedWalls();

                    if (!room.updatedWalls && removeCompleteState)
                        removeCompleteState = false;
                }

                if (removeCompleteState)
                    break;
            }


            // SpawnManager Find 
            foreach (Room room in loadedRooms)
            {
                if (room.spawnManager)
                    room.spawnManager.findSpawnPoint();

                if ((room.currPos.z * minimapCam.Instance.roomSize) > minimapCam.Instance.maxUpPos.z )
                    minimapCam.Instance.maxUpPos = new Vector3(0, 0, room.currPos.z) * minimapCam.Instance.roomSize;

                if ((room.currPos.x * minimapCam.Instance.roomSize )> minimapCam.Instance.maxUpPos.x)
                    minimapCam.Instance.maxRightPos = new Vector3(room.currPos.x, 0, 0) * minimapCam.Instance.roomSize;
            }
            // RoomBake
            // BakeNavMeshSurface();

            isLoadingRoom = true;
        }
    }
    //public void BakeNavMeshSurface()
    //{
    //    foreach (var surface in GetComponentsInChildren<NavMeshSurface>())
    //    {
    //        surface.RemoveData();
    //        surface.BuildNavMesh();
    //    }
    //}

    public void LoadRoom(RoomInfo oldRoom)
    {
        if (DoesRoomExist(oldRoom.currPos.x, oldRoom.currPos.y, oldRoom.currPos.z))
        {
            return;
        }

        GameObject room = Instantiate(RoomPrefabsSet.Instance.roomPrefabs[oldRoom.roomName]);
        room.transform.position = new Vector3(
                    (oldRoom.currPos.x * room.transform.GetComponent<Room>().Width),
                     oldRoom.currPos.y,
                    (oldRoom.currPos.z * room.transform.GetComponent<Room>().Height)
        );

        room.transform.GetComponent<Room>().currPos = oldRoom.currPos;
        room.name = currentWorldName + "-" + oldRoom.roomName + " " + oldRoom.currPos.x + ", " + oldRoom.currPos.z;

        room.transform.GetComponent<Room>().roomName     = oldRoom.roomName;
        room.transform.GetComponent<Room>().roomType     = oldRoom.roomType;
        room.transform.GetComponent<Room>().roomId       = oldRoom.roomID;
        room.transform.GetComponent<Room>().parentPos    = oldRoom.parentPos;
        room.transform.GetComponent<Room>().CenterPos    = oldRoom.CenterPos;
        room.transform.GetComponent<Room>().roomDistance = oldRoom.weightCnt;
        room.transform.GetComponent<Room>().roomMaxscore = oldRoom.roomScore;

        //// Minimap Set
        //RoomMinimap minimap = GetComponentInChildren<RoomMinimap>();
        //minimap.minimapRoomPos = oldRoom.currPos;

        if (oldRoom.roomName != RoomName.Start || oldRoom.roomName != RoomName.Normal)
            room.transform.GetComponent<Room>().isSpecialRoom = false;
        else
            room.transform.GetComponent<Room>().isSpecialRoom = true;

        switch (oldRoom.roomName)
        {
            case RoomName.Shop:
                ShopRoom = room.GetComponent<Room>();
                room.transform.GetComponent<Room>().isSpecialRoom = true;
                break;
            case RoomName.Hidden:
                HiddenRoom.Add(room.GetComponent<Room>());
                room.transform.GetComponent<Room>().isSpecialRoom = true;
                break;
            case RoomName.Battle:
                BattleRoom = room.GetComponent<Room>();
                room.transform.GetComponent<Room>().isSpecialRoom = true;
                break;
            case RoomName.Item:
                ItemRoom = room.GetComponent<Room>();
                room.transform.GetComponent<Room>().isSpecialRoom = true;
                break;
            case RoomName.Boss:
                bossRoom = room.GetComponent<Room>();
                room.transform.GetComponent<Room>().isSpecialRoom = true;
                break;
            case RoomName.Start:
                startRoom = room.GetComponent<Room>();
                break;
            default:
                break;
        }

        StageManager.Instance.CaculaterMax(oldRoom.roomScore);
        room.transform.parent = transform;
        room.GetComponent<Room>().initRoom();

        loadedRooms.Add(room.GetComponent<Room>());

    }

    public void ShopRoomClearEvent()
    {
        UI_RoomMessage.RoomMessagePrintOn("상점방 입장");
        currRoom.roomClearSetup();
    }
    public void HiddenRoomClearEvent()
    {
        UI_RoomMessage.RoomMessagePrintOn("비밀방 입장");
        currRoom.roomClearSetup();
    }
    public void itemRoomClearEvent()
    {
        UI_RoomMessage.RoomMessagePrintOn("아이템방 입장");
        // 아이템 상자 오픈(최초 1번만 실행)
        currRoom.roomClearSetup();
    }
    public void BattleRoomClearEvent()
    {
        UI_RoomMessage.RoomMessagePrintOn("클리어!!");

        haveMonsterRoomClear();        
    }
    public void BossRoomClear()
    {
        UI_RoomMessage.RoomMessagePrintOn("스테이지 완료");
        haveMonsterRoomClear();
        StageManager.Instance.bossRoomClear();
    }
    public void haveMonsterRoomClear()
    {
        roomClearCount++;
        currRoom.roomClearSetup();
        InGameManager.Instance.roomClearCountCarc();

        Player player = Player.player;

        if (player)
            player.RoomClear();

    }
    public void Update()
    {
        if (currRoom)
        {
            if (currRoom.updatedWalls && currRoom.visitedRoom && !currRoom.isRoomClear)
            {
                if (currRoom.isSpecialRoom)
                {
                    switch (currRoom.roomName)
                    {
                        case RoomName.Shop:
                            ShopRoomClearEvent();
                            break;
                        case RoomName.Hidden:
                            HiddenRoomClearEvent();
                            break;
                        case RoomName.Battle:
                            if (roomClearCheck())
                                BattleRoomClearEvent();
                            break;
                        case RoomName.Item:
                            itemRoomClearEvent();
                            break;
                        case RoomName.Boss:
                            if (currRoom.spawnManager.state == SpawnManager.SpawnStatus.END)
                                BossRoomClear();
                            break;
                        default:

                            break;
                    }                    
                }
                else
                {
                    if (roomClearCheck())
                    {
                        UI_RoomMessage.RoomMessagePrintOn("클리어!!");
                        haveMonsterRoomClear();
                    }
                }
            }
        }
    }
    

    public void tmpRoomClear()
    {
        for (int i = 0; i < currRoom.monsters.Count; i++)
        {
            Destroy(currRoom.monsters[i].gameObject);
            currRoom.monsters[i] = null;
        }
        currRoom.monsters.Clear();
    }

    public bool roomClearCheck()
    {
        currRoom.monsterDeadCheck();

        if (currRoom.monsters.Count <= 0 && currRoom.spawnManager.state == SpawnManager.SpawnStatus.END)
            return true;

        return false;
    }

    public bool DoesRoomExist(int x, int y, int z)
    {
        return loadedRooms.Find(item => item.currPos.x == x && item.currPos.y == y && item.currPos.z == z) != null;
    }

    public Room FindRoom(int x, int y, int z)
    {
        // List.Find : item 변수 조건에 맞는 Room을 찾아 반환
        return loadedRooms.Find(item => item.currPos.x == x && item.currPos.y == y && item.currPos.z == z);
    }
    public Room FindParentRoom(Room searchRoom)
    {
        foreach(Room rooms in loadedRooms)
        {
            if (rooms.parentPos == searchRoom.parentPos && rooms.currPos == rooms.parentPos)
            {
                return rooms;
            }
        }
        return searchRoom;
    }
    // 해당 Room에서 Player가 있는 방을 반환
    public void OnPlayerEnterRoom(Room room, Vector3 movePos)
    {
        if (!room)
            return;

        currRoom = FindParentRoom(room);

        // navmesh delete
        if (StageManager.Instance.roomCreateDone)
        {
            FadeInOut.Instance.Fade(true, 0.5f);
            for (int i = 0; i < loadedRooms.Count; i++)
            {
                if (currRoom.parentPos == loadedRooms[i].parentPos) {
                    // Rooms 배열형태 추후 수정 필요.
                    loadedRooms[i].visitedRoomUpdateStatus(true);
                    loadedRooms[i].rooms.gameObject.SetActive(true);
                    loadedRooms[i].rooms.minimapUpdate();

                    // 처음 방을 방문했을 때 실행
                    if (loadedRooms[i].spawnManager)
                    {
                        loadedRooms[i].spawnManager.enabled = true;
                        
                        if (loadedRooms[i].spawnManager.state == SpawnManager.SpawnStatus.WAITING){
                            loadedRooms[i].spawnManager.setStateChange(SpawnManager.SpawnStatus.START);
                        }
                    }
                    //loadedRooms[i].rooms.ReplaceHiddenRoomDoor();
                    // 비밀방 Open 체크
                }
                else
                {
                    loadedRooms[i].rooms.gameObject.SetActive(false);

                    if (loadedRooms[i].spawnManager)
                        loadedRooms[i].spawnManager.enabled = false;
                }
            }
            FadeInOut.Instance.Fade(false, 0.5f);

        }

        // 방 입장 시 Player 시점에서 발동하는 트리거
        if (player == null)
            player = Player.player;

        if (player)
            player.RoomEnter();

        minimapCam.Instance.moveMiniCamCamera(currRoom);
        InGameManager.Instance.GameCurrentRoom(currRoom.name);
    }
}
