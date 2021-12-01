using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameManager : MonoBehaviour
{
    public static InGameManager Instance;

    [HideInInspector] public StageManager stagemanager;
    [HideInInspector] public SceneLoader sceneManager;
    //[HideInInspector] public DungeonCrawlerController mapMaker;

    [Header("Chanpion Information")]
    public Player player;
    public GameObject playerGameObject;


    [Header("ClearRoomInformation")]
    public int totalClearRoomCount = 0;
    public int currClearRoomCount = 0;
    public List<string> chapterStageScore = new List<string>();

    [HideInInspector] public int rewardGold = 0;
    [HideInInspector] public List<string> itemList = new List<string>();
    [HideInInspector] public string currentRoom ="";    
    //[HideInInspector] public int monsterCount = 0;
    [HideInInspector] public int killMonsterCount = 0;
    [HideInInspector] public float adReceivedDamage = 0;
    [HideInInspector] public float apReceivedDamage = 0;
    [HideInInspector] public float attackDamage = 0;
    [HideInInspector] public float abilityPower = 0;
    [HideInInspector] public int totalExperience = 0;

    [Header("Timer")]
    public bool playTimeStart = false;
    public int playTime = 0;
    public int hours;
    public int minutes;
    public int seconds;
    public int days;


    [Header("Stage Score")]
    public int stageScore = 0;

    private int clearCount = 0;
    private int _maxGold = 0;


    public void Awake()
    {
        if (!Instance)
            Instance = this;
        else
            Destroy(this.gameObject);

    }
    protected void Start()
    {

        DontDestroyOnLoad(this.gameObject);

        if (!stagemanager)
            stagemanager = StageManager.Instance;

        if (!sceneManager)
            sceneManager = SceneLoader.Instance;

        TimerStart();
    }

    public void TimerStart()
    {
        Debug.Log("Stage Timer");
        if(playTimeStart)
            StartCoroutine("Playtimer");
        else
            StopCoroutine("Playtimer");
    }


    public void setTimerStatus(bool status)
    {
        playTimeStart = status;
    }

    // 재시작 함수
    public void gameRestartEvent()
    {
        Destroy(Player.player.gameObject);
        Destroy(this.gameObject);

        SceneLoader.Instance.LoadStartScene();
    }
    // 게임 종료 함수
    public void gameQuitEvent()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit(); // 어플리케이션 종료
#endif
    }
    void OnEnable()
    {
        // 씬 매니저의 sceneLoaded에 체인을 건다.
        SceneManager.sceneLoaded += OnSceneLoaded;

    }
    public void monsterKillCountCarc(int monster)
    {
        killMonsterCount += monster;
    }

    // 체인을 걸어서 이 함수는 매 씬마다 호출된다.
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //Debug.Log("OnSceneLoaded: " + scene.name);

        if (!stagemanager)
            stagemanager = StageManager.Instance;

        if (!sceneManager)
            sceneManager = SceneLoader.Instance;

        player = Player.player;

        if (!player && playerGameObject != null
            && scene.name.Contains("PlayStage"))
        {
            GameObject _currentCharacter = Instantiate(playerGameObject, Vector3.zero, transform.rotation);
            _currentCharacter.name = playerGameObject.name;
            player = playerGameObject.GetComponent<Player>();
        }

        currClearRoomCount = 0;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }


    // 게임 스테이지 종료
    public void gameOverEvent()
    {

        StageManager.Instance.gameOverEvent();
    }
    // 보스 방 클리어
    public void bossRoomClear()
    {
        //Debug.Log("BossRoomClear");
        nextStageLoad();
    }
    public void nextStageLoad()
    {
        if(!stagemanager)
            stagemanager = StageManager.Instance;

        if (!sceneManager)
            sceneManager = SceneLoader.Instance;

        Player.player.agent.Warp(Vector3.zero);

        // 생성
        sceneManager.LoadScene(stagemanager.nextStage);
    }
    public void playerDamageCarc()
    {

    }

    public void roomClearCountCarc()
    {
        // 룸 카운트
        totalClearRoomCount++;
        currClearRoomCount++;
    }

    public void playerGetExperience(int rewardExp)
    {
        totalExperience += rewardExp;
    }
    // 타이머 (완)

    private IEnumerator Playtimer()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            playTime += 1;
            seconds = (playTime % 60);
            minutes = (playTime / 60) % 60;
            hours = (playTime / 3600) % 24;
            days = (playTime / 86400) % 365;
        }
    }


    public void playerGetGold(int gold) //획득골드. 몬스터 잡은 reward만
    {
        rewardGold += gold;
    }

    public void GameGetItemList(List<Item> item) //현재 갖고있는 아이템 리스트
    {
        var list = item;
        var nameList = new List<string>();

        for(int i=0; i<list.Count; i++)
        {
            if(list[i].valid)
                nameList.Add(list[i].name);
        }

        itemList = nameList;
    }

    public void GameCurrentRoom(string curRoom) //현재 위치한 방
    {
        currentRoom = curRoom;
    }
    
    public int GameClearRoomCount() //클리어한 방 개수
    {
        return (clearCount = RoomController.Instance.roomClearCount);
    }
    
    public void getPlayerDealDamageAt(float _attackDamage, float _abilityPower, float _trueDamage)
    {
        attackDamage += _attackDamage;
        abilityPower += _abilityPower;
    }
    public void getPlayerBeDamagedBy(float _attackDamage, float _abilityPower, float _trueDamage)
    {
        apReceivedDamage += _abilityPower;
        adReceivedDamage += _attackDamage;
    }
    public int GameGetMaxGold() //재력과시
    {
        Debug.Log("골드"+ _maxGold);
        return _maxGold = _maxGold > Player.player.gold ? _maxGold : Player.player.gold;
    }

    public void SelectAndStartGame(GameObject _character)
    {
        // play
        if (_character != null)
        {
            playerGameObject = _character;
            SceneLoader.Instance.LoadPlayScene();

        }
    }
}
