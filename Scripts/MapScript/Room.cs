using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

[System.Serializable]
public class Room : MonoBehaviour
{
    public int Width;
    public int Height;
    public int roomDistance;

    public Vector3Int currPos;
    public Vector3Int parentPos;
    public Vector3 CenterPos;

    public RoomName roomName;
    public RoomType roomType;
    public string roomId;

    public bool updatedWalls = false;
    public bool visitedRoom = false;
    public bool reVisitedRoom = false;
    public bool isSpecialRoom = false;

    public bool updateRooms = false;

    public bool isRoomClear = false;
    public bool isOpenDoor = false;


    public GameObject _nDoor;
    public GameObject secondDoor;
    public GameObject Wall;
    public Material floor;
    private DoorAnimator[] doorsAnim;
    public NavMeshSurface[] navMesh;

    public List<Monster> monsters = new List<Monster>();
    public SpawnManager spawnManager;

    public SubRoom rooms;
    public int roomMaxscore = 0;
    public int roomClearTime = 0;
    public String roomClearscore = "";
    public void Start()
    {
        RoomTimerInit();
    }
    public void RoomTimerInit()
    {
        roomClearTime = 0;
        roomClearscore = "";
        RoomController.Instance.UI_RoomTimer.roomTimerUpdate("00", "00");
            }
    private IEnumerator Roomtimer()
    {
        yield return new WaitForSeconds(1);
        roomClearTime += 1;

        int miniute = (roomClearTime / 60) % 60;
        int second = (roomClearTime % 60);

        string miniuteTxt = miniute < 10 ? "0" + miniute : miniute.ToString();
        string secondTxt = second < 10 ? "0" + second : second.ToString();

        RoomController.Instance.UI_RoomTimer.roomTimerUpdate(miniuteTxt, secondTxt);

        StartCoroutine("Roomtimer");
    }

    public void TimerRoomStart(bool status)
    {
        if (status)
            StartCoroutine("Roomtimer");
        else
            StopCoroutine("Roomtimer");
    }
    public void currCalculateScore()
    {
        if (roomName == RoomName.Boss) // 1000
        {
            if (roomClearTime <= 300)      { roomClearscore = "A"; }     // 1000
            else if (roomClearTime <= 330) { roomClearscore = "B"; }     // 900
            else if (roomClearTime <= 360) { roomClearscore = "C"; }     // 700  
            else                           { roomClearscore = "D"; }     // 500
            return;
        }

        if (roomName == RoomName.Battle) // 500
        {
            if      (roomClearTime <= 120)      { roomClearscore = "A"; }     // 500
            else if (roomClearTime <= 140)      { roomClearscore = "C"; }     // 450
            else if (roomClearTime <= 180)      { roomClearscore = "B"; }     // 350  
            else                               { roomClearscore = "D"; }     // 250
            return;
        }

        // 각 방 점수 계산
        // roomStageTime <--- 클리어 시간
                // 싱글 방 : 60, 2칸 방 : 100, 3칸 방 : 150, 4칸 방 : 200
        switch (roomType)
        {
            case RoomType.Single :
                if (roomClearTime <= 60)        { roomClearscore = "A"; }     // 150
                else if (roomClearTime <= 70)   { roomClearscore = "B"; }     // 135
                else if (roomClearTime <= 85)   { roomClearscore = "C"; }     // 105  
                else                            { roomClearscore = "D"; }     // 75
            break;
            case RoomType.Double :
                if (roomClearTime <= 100)        { roomClearscore = "A"; }
                else if (roomClearTime <= 110)   { roomClearscore = "B"; }
                else if (roomClearTime <= 115)   { roomClearscore = "C"; }
                else                             { roomClearscore = "D"; }
            break;
            case RoomType.Triple :
                if (roomClearTime <= 150)       { roomClearscore = "A"; }
                else if (roomClearTime <= 160)  { roomClearscore = "B"; }
                else if (roomClearTime <= 175)  { roomClearscore = "C"; }
                else                            { roomClearscore = "D"; }
            break;
            case RoomType.Quad :
                if (roomClearTime <= 200)       { roomClearscore = "A"; }
                else if (roomClearTime <= 210)  { roomClearscore = "B"; }
                else if (roomClearTime <= 230)  { roomClearscore = "C"; }
                else                            { roomClearscore = "D"; }
            break;
        }
    }
    public void visitedRoomUpdateStatus(bool status)
    {
        reVisitedRoom = status;
    }
    public Room(int x, int y, int z)
    {
        currPos.x = x;
        currPos.y = y;
        currPos.z = z;
    }
    
    // Room을 생성 시 초기에 호출(Start)
    public void setUpdateWalls(bool setup)
    {
        updatedWalls = setup;
    }

    public bool roomClearSetup()
    {
        if (monsters.Count <= 0){
            isOpenDoor = isRoomClear = true;
            
            openRoomDoor();
            TimerRoomStart(false);
            currCalculateScore();
            return true;
        }
        return isRoomClear;
    }
    public void openRoomDoor()
    {
        doorsAnim = GetComponentsInChildren<DoorAnimator>();

        if (isRoomClear)
        {
            //Debug.Log("Clear and Open Door");
            for(int i=0; i< doorsAnim.Length; i++)
                doorsAnim[i].OpenDoor();
        }
    }
    public bool isParentRoom()
    {
        return (currPos == parentPos);
    }
    public void initRoom()
    {
        if (RoomController.Instance == null) {
            //Debug.Log("You pressed play in the wrong scene!");
            return;
        }
        int floorIndex = UnityEngine.Random.Range(0, RoomController.Instance.floorMaterial.Length);

        floor = RoomController.Instance.floorMaterial[floorIndex];

        rooms = GetComponentInChildren<SubRoom>();

        if (rooms != null)
        {
            rooms.SubRoomInit();

            rooms.currPos = currPos;
            rooms.roomType = roomType;
            rooms.Width = Width;
            rooms.Height = Height;
            rooms.roomName = roomName;
            rooms.parentPos = parentPos;
            rooms.CenterPos = CenterPos;
            rooms.floor = floor;
        }
            // updatedWalls = false;
    }

    public void RemoveUnconnectedWalls()
    {
        SubRoom[] CheckRooms = GetComponentsInChildren<SubRoom>();
        // 방이 완료될때까지 반복
        bool roomSetupCheck = true;

        for (int i=0; i< CheckRooms.Length; i++)
        {
            if (CheckRooms[i] != null)
            {
                CheckRooms[i].RemoveUnconnectedWalls();
                // 하나라도 업데이트가 안된 것들은 roomSetupCheck = false
                if (!CheckRooms[i].roomPathBool && roomSetupCheck)
                {
                    roomSetupCheck = false;
                }
            }
        }
        updatedWalls = roomSetupCheck;
        spawnManager = GetComponent<SpawnManager>();
    }

    // 현재 방에 몬스터 수 업데이트
    public void monsterDeadCheck()
    {
        //Debug.Log("현재 방 ");

        for (int i = 0; i < monsters.Count; i++)
        {
            if (!monsters[i] || monsters[i].health <= 0) //몬스터가 없을때
            {
                monsters[i] = null;
                monsters.RemoveAt(i);
                i--;

                InGameManager.Instance.monsterKillCountCarc(1);
            }
        }
    }

    public Vector3 GetRoomCenter()
    {
        return new Vector3(currPos.x, 0, currPos.z);
    }   

    public void roomTriggerCheck(Vector3 movePos)
    {
        RoomController.Instance.OnPlayerEnterRoom(this.transform.GetComponent<Room>(), movePos);
    }

}
