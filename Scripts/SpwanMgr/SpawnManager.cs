using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class SpawnManager : MonoBehaviour
{    
    // WAITING : 대기 상태
    // START : 몬스터 생성 시작
    // SPAWING : 몬스터 웨이브 시작
    // COUNTING : 몬스터 웨이브 후 모든 몬스터 처치(웨이브 잔여)
    // END : 해당 방 클리어 시 종료

    public enum SpawnStatus { WAITING, START, BATTLE, SPAWING, END };

    // Wave 정보
    [System.Serializable]
    public class Wave
    {
        public int monsterCount;
        public int rate;
    }

    [Header ("Room")]
    public Room thisRoom;
    public RoomObj objGameObject;

    [Header("Spwan Information")]
    public List<SpawnSpot> spawnPoint = new List<SpawnSpot>();
    public List<Monster> setupMonster = new List<Monster>();
    public bool isSetupComplete = false;

    [Header("Spwan Wave Information")]
    public Wave[] waves;
    public int currentWave = 0;
    public float timeBetweenWaves = 1000f;
    public float waveCountdown = 1000f;
    public SpawnStatus state = SpawnStatus.WAITING;

    public bool createdMonsterTrue = false;
    private float searchCountdown = .5f;



    public void Awake()
    {
        state = SpawnStatus.WAITING;
        TimerInit();
    }
    public void TimerInit()
    {
        waveCountdown = timeBetweenWaves;
    }
    public void findSpawnPoint()
    {
        
        SpawnSpot[] ss = transform.GetComponentsInChildren<SpawnSpot>();
        callSetup();

        // 갯수 변동 없을 시 리턴
        if (ss.Length == spawnPoint.Count)
            return;

        spawnPoint.Clear();

        for (int i=0; i<ss.Length; i++)
            spawnPoint.Add(ss[i]);
       
    }
    public void setStateChange(SpawnStatus changeState)
    {
        state = changeState;
    }

    public void callSetup()
    {
        if (isSetupComplete)
            return;

        thisRoom = this.transform.GetComponent<Room>();

        int prefabsCnt = StageManager.Instance.monsterPrefabs.Length;

        if (StageManager.Instance != null)
        {
            for (int i = 0; i < prefabsCnt; i++)
            {
                // 몬스터 거리 측정
                if (thisRoom.roomDistance >= StageManager.Instance.createdMinDistance[i])
                {
                    // 몬스터 추출
                    Monster monster = StageManager.Instance.monsterPrefabs[i];
                    if (monster != null)
                        setupMonster.Add(monster);
                }
            }
        }

        isSetupComplete = true;
    }

    public void Update()
    {
        if ((state == SpawnStatus.END) || (state == SpawnStatus.WAITING))
            return;

        // (방 입장 시 몬스터 소환)
        // 1. 몬스터 타이머를 통한 웨이브 처리
        // 2. 몬스터가 다 죽는다면 웨이브 실행(타이머 돌기 전에도)
        // 3. 웨이브 종료 시점을 방에 전달 스테이지 클리어 처리

        waveCountdown -= Time.deltaTime;

        if (spawnPoint.Count <= 0) {
            state = SpawnStatus.END;
            return;
        }

        if (state == SpawnStatus.START)
        {
            nextWaveStart();
            string nextWaveMessage = (currentWave) + "Wave"; // 1Wave

            RoomController.Instance.UI_RoomMessage.RoomMessagePrintOn(nextWaveMessage);
        }
        else if (state == SpawnStatus.BATTLE && !WaveCompleted())
        {
            if (waveCountdown <= 0f || thisRoom.monsters.Count <= 0)
            {
                if (currentWave < (waves.Length))
                {
                    //Debug.Log("currentWave : " + currentWave + ", waveLength : " + waves.Length);
                    nextWaveStart();
                    string nextWaveMessage = (currentWave) + "Wave";
                    RoomController.Instance.UI_RoomMessage.RoomMessagePrintOn(nextWaveMessage);
                    waveCountdown = timeBetweenWaves;
                }
            }
        }
    }
    public void nextWaveStart()
    {
        if (setupMonster.Count <= 0) 
        {
            state = SpawnStatus.END;
            return;
        }

        if (currentWave < (waves.Length))
            StartCoroutine(SpawnWave(waves[currentWave]));
        
        currentWave++;
    }
    public bool WaveCompleted()
    {

        // 웨이브가 종료 되고 적들이 다 제거되었을 경우 상태 변경
        if (!EnemyIsAlive() && currentWave >= (waves.Length))
        {
            currentWave = 0;
            state = SpawnStatus.END;
            return true;
        }
        return false;
        //waveCountdown = timeBetweenWaves;
    }

    public bool EnemyIsAlive()
    {
        searchCountdown -= Time.deltaTime;

        if (searchCountdown <= 0f)
        {
            thisRoom.monsterDeadCheck();

            int _monsterCount = thisRoom.monsters.Count;
            searchCountdown = 1f;

            if (_monsterCount <= 0)
                return false;
        }
        // 몬스터가 있으면 True 반환
        return true;
    }
    IEnumerator SpawnWave(Wave _wave)
    {
        // 몬스터 생성 중
        state = SpawnStatus.SPAWING;

        if (_wave.monsterCount > spawnPoint.Count)
            _wave.monsterCount = spawnPoint.Count;


        if (thisRoom.roomName == RoomName.Boss)        // Boss 몹 소환
        {
            spawnBoss();
            yield return new WaitForSeconds(0.0f);
        }
        else if (thisRoom.roomName != RoomName.Shop && thisRoom.roomName != RoomName.Item)
        {
            Shuffle(spawnPoint);

            int spawnMonsterCount = 0;

            // 소환 몬스터 Spot 갯수 만큼 반복문
            for (int i = 0; i < spawnPoint.Count; i++) 
            {
                if (spawnMonsterCount >= _wave.monsterCount)
                    break;

                if (spawnPoint[i].CanUseSpawnPoint())
                {
                    if (spawnMonster(spawnPoint[i]))
                        spawnMonsterCount++;
                }

                yield return new WaitForSeconds(0.0f);
            }
        }

        state = SpawnStatus.BATTLE;

        yield break;
    }
    public void spawnBoss()
    {
        int randSpawnPoint = Random.Range(0, spawnPoint.Count);

        GameObject createdBoss = Instantiate(StageManager.Instance.StageBoss.transform.gameObject, spawnPoint[randSpawnPoint].transform);
        createdBoss.transform.SetParent(thisRoom.transform);
        thisRoom.monsters.Add(createdBoss.transform.GetComponent<Monster>());
    }
    public bool spawnMonster(SpawnSpot spawnPosition)
    {
        //Debug.Log("thisRoom Spawn : " + thisRoom.name);
        //Debug.Log("mondter Spawn : " + spawnPosition.name);1
        if (setupMonster.Count <= 0)
            return false;

        if (!spawnPosition) 
            return false;

        int randMonster = Random.Range(0, setupMonster.Count); ;
        //int randMonster = Random.Range(0, setupMonster.Count - 1); ;
        //if(randMonster >= 2)
        //    Random.Range(0, setupMonster.Count-1);
        //else
        //    Random.Range(1, setupMonster.Count);



        GameObject createMonster = Instantiate(setupMonster[randMonster].transform.gameObject, spawnPosition.transform);
        createMonster.transform.GetComponent<Monster>().level = StageManager.Instance.currentStageLevel;
        createMonster.transform.SetParent(thisRoom.transform);
        thisRoom.monsters.Add(createMonster.transform.GetComponent<Monster>());

        return true;

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
    // 무작위 섞기
    public void Shuffle(List<SpawnSpot> shufflePos)
    {
        for (int i = 0; i < shufflePos.Count; i++)
        {
            SpawnSpot temp = shufflePos[i];
            int randomIndex = Random.Range(0, shufflePos.Count);
            shufflePos[i] = shufflePos[randomIndex];
            shufflePos[randomIndex] = temp;
        }
    }
}
