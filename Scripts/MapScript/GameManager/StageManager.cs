using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class StageManager : MonoBehaviour
{
    public static StageManager Instance;

    [Header("Monster")]
    public GameObject       StageBoss;
    public Monster[]        monsterPrefabs;
    public double[]         mosnterProbability;

    [Header("Room")]
    public Room             BossRoom;
    public int[]            createdMinDistance;
    public List<int> roomInfo;
    public bool roomCreateDone = false;
    public RoomController roomMaker;

    [Header("Stage")]
    public int currentStageLevel;
    public string nextStage;
    public SceneLoader sceneLoader;
    public bool isStageClear = false;
    public bool isGameOver = false;
    public int stageMaxScore = 0;
    public int stageCurrScore = 0;
    public string stageCurrGrade = "";

    public Sprite[] gradeImage;

    [Header("UI")]
    public UI_InformationBook ui_InformationBook;

    [Header("GameManager")]
    public InGameManager _inGameManager;

    public void Awake()
    {
        if (!Instance)
            Instance = this;
        else
            Destroy(this);

    }
    public void sumStageScoreGrade()
    {
        // 클리어 점수 계산
        // 1. 보스방의 거리 보다 작게 클리어하고 스테이지 완료 시 
        // 2. 각 방의 클리어 시간이 1분보다 작을 때 
        // 최단거리 클리어 조건으로 SSS 부여
        // 최단 거리 + 1 : SS
        // 최단 거리 + 2 : S
        // 최단 거리 + 3 : A
        // 그 외 아래 로직
        int sum = 0;
        int clearCount = InGameManager.Instance.currClearRoomCount;
        int bossRoomDistance = RoomController.Instance.bossRoom.roomDistance;
        int[] roomGrade = {0, 0 , 0 , 0 };
        for (int i=0; i<RoomController.Instance.loadedRooms.Count; i++)
        {
            if (RoomController.Instance.loadedRooms[i].isParentRoom())
            {
                if (RoomController.Instance.loadedRooms[i].isRoomClear)
                {
                    int roomMultiple = 0;
                    clearCount++;

                    switch (RoomController.Instance.loadedRooms[i].roomType)
                    {
                        case RoomType.Single:   roomMultiple = 1; break;
                        case RoomType.Double:   roomMultiple = 2; break;
                        case RoomType.Triple:   roomMultiple = 3; break;
                        case RoomType.Quad:     roomMultiple = 4; break;
                    }

                    switch (RoomController.Instance.loadedRooms[i].roomClearscore)
                    {
                        case "A":
                            roomGrade[0]++;
                            stageMaxScore += (RoomController.Instance.loadedRooms[i].roomMaxscore * roomMultiple);
                            stageCurrScore += (int)((RoomController.Instance.loadedRooms[i].roomMaxscore * roomMultiple) * 1.0f);
                            break;
                        case "B":
                            roomGrade[1]++;
                            stageMaxScore += (RoomController.Instance.loadedRooms[i].roomMaxscore * roomMultiple);
                            stageCurrScore += (int)((RoomController.Instance.loadedRooms[i].roomMaxscore * roomMultiple) * .9f);
                            break;
                        case "C":
                            roomGrade[2]++;
                            stageMaxScore += (RoomController.Instance.loadedRooms[i].roomMaxscore * roomMultiple);
                            stageCurrScore += (int)((RoomController.Instance.loadedRooms[i].roomMaxscore * roomMultiple) * .7f);
                            break;
                        case "D":
                            roomGrade[3]++;
                            stageMaxScore += (RoomController.Instance.loadedRooms[i].roomMaxscore * roomMultiple);
                            stageCurrScore += (int)((RoomController.Instance.loadedRooms[i].roomMaxscore * roomMultiple) * .5f);
                            break;
                    }
                }
            }
        }

        // 모든 방을 1분 이내로 클리어 했을 경우 : SSS
        // 2개 방을 제외한 나머지 방을 1분 이내로 클리어 했을 경우 : SS
        // 3개 방을 제외한 나머지 방을 1분 이내로 클리어 했을 경우 : S
        if (bossRoomDistance == roomGrade[0])
        {
            stageCurrGrade = "SSS";
            return;
        }else if(bossRoomDistance <= roomGrade[0] && roomGrade[0] <= bossRoomDistance + 1 
                    && 1 >= roomGrade[1])
        {
            stageCurrGrade = "SS";
            return;
        }
        else if (bossRoomDistance <= roomGrade[0] && roomGrade[0] <= bossRoomDistance + 2
                    && 2 >= roomGrade[1])
        {
            stageCurrGrade = "S";
            return;
        }

        // 모든 방 클리어
        if(stageMaxScore <= stageCurrScore)                 stageCurrGrade = "SSS"; // 100% : SSS
        else if (stageMaxScore * .97f <= stageCurrScore)    stageCurrGrade = "SS";  // 97% : SS
        else if (stageMaxScore * .95f <= stageCurrScore)    stageCurrGrade = "S";   // 95% : S
        else if (stageMaxScore * .9f  <= stageCurrScore)    stageCurrGrade = "A";   // 90% : A
        else if (stageMaxScore * .8f  <= stageCurrScore)    stageCurrGrade = "B";   // 80% : B
        else if (stageMaxScore * .7f  <= stageCurrScore)    stageCurrGrade = "C";   // 70% : C
        else if (stageMaxScore * .5f  <= stageCurrScore)    stageCurrGrade = "D";   // 50% : D
        else                                                stageCurrGrade = "D";   // 50% : D
    }

    public void Start()
    {
        CreateRoom();

        if (!InGameManager.Instance)
            _inGameManager = InGameManager.Instance;

        //if (!SceneLoader.Instance)
        //    sceneLoader = SceneLoader.Instance;
    }
    // 스테이지 방 생성
    public void CreateRoom()
    {
        RoomController.Instance.newCreatedRoom(roomInfo[0], roomInfo[1], roomInfo[2]);
        roomCreateDone = true;
        Debug.Log("Created Room : " + roomCreateDone);
        Room initRoom = RoomController.Instance.startRoom;
        RoomController.Instance.OnPlayerEnterRoom(initRoom, Vector3.zero);

    }
    public void CaculaterMax(int score)
    {
        stageMaxScore += score;
    }
    

    // 스테이지 새로 시작
    public void gameRestartEvent()
    {
        InGameManager.Instance.gameRestartEvent();
    }

    // 게임 종료
    public void gameQuitEvent()
    {
        InGameManager.Instance.gameQuitEvent();
    }

    public void bossRoomClear()
    {   
        // 보스 방 클리어 되었지만 열려 있을경우 결과창을 닫고 다시 실행
        if (!isStageClear && ui_InformationBook.IsActiveBook())
            ui_InformationBook.SetActiveBook(false);

        Player player = Player.player;

        if (player)
            player.StageClear();

        GameObject nextStageDoor = Instantiate(mapObjSetting.Instance.nextStageObject.gameObject, RoomController.Instance.bossRoom.transform);
        nextStageDoor.transform.SetParent(RoomController.Instance.bossRoom.transform);
        
        sumStageScoreGrade();
    }

    // 게임 종료
    public void gameOverEvent()
    {
          
        // 게임 오버가 되었지만 열려 있을경우 결과창을 닫고 다시 실행
        if(!isGameOver && ui_InformationBook.IsActiveBook())
            ui_InformationBook.SetActiveBook(false);

        if (!ui_InformationBook.IsActiveBook())
        {
            FadeInOut.Instance.Fade(true, 0.55f);

            isGameOver = true;

            ui_InformationBook.SetActiveBook(true);
        }
    }
    public void gameNextStage()
    {
        int currentStageIndex = SceneManager.GetActiveScene().buildIndex;
        string currentStageName = SceneManager.GetActiveScene().name;

        if (nextStage == "End" || nextStage == "")
        {
            FinalBossRoomClear();
        }
        else
        {
            SceneLoader.Instance.LoadScene(currentStageIndex + 1);
        }
    }
    public void FinalBossRoomClear()
    {
        gameOverEvent();
    }

    public void GoToLobbyScene()
    {
        if (Player.player) Destroy(Player.player.gameObject);
        if (InGameManager.Instance) Destroy(InGameManager.Instance.gameObject);
        //sceneLoader.LoadScene("BookLobbyScene");
        SceneManager.LoadScene(0);
    }
}
