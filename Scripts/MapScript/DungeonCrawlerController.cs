using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class DungeonCrawlerController : MonoBehaviour
{
    public static DungeonCrawlerController Instance;
    
    public Dictionary<RoomName, int> roomScore = new Dictionary<RoomName, int>
    {
        { RoomName.Start,   100},
        { RoomName.Normal,  150},
        { RoomName.Battle,  500},
        { RoomName.Boss,    1000},
        { RoomName.Hidden,  300},
        { RoomName.Item,    0},
        { RoomName.Shop,    0}
    };

    public List<Vector3Int> direction4 = new List<Vector3Int>
    {
        new Vector3Int( 0, 0,  1),       // down
        new Vector3Int( 1, 0,  0),       // right
        new Vector3Int(-1, 0,  0),       // left
        new Vector3Int( 0, 0, -1)        // up
    };

    public List<Vector3Int> direction8 = new List<Vector3Int>
    {   // Down                           // Up
        new Vector3Int( 0, 0,  1),        new Vector3Int( 0, 0, -1),        
        // Left                           // Right
        new Vector3Int(-1, 0,  0),        new Vector3Int( 1, 0,  0),        
        // UpLeft                         // UpRight
        new Vector3Int(-1, 0, -1),        new Vector3Int( 1, 0, -1),
        // DiwbLeft                       // DownRight
        new Vector3Int(-1, 0,  1),        new Vector3Int( 1, 0,  1)
    };
    public Dictionary<int, List<Vector3Int>> downPatten = new Dictionary<int, List<Vector3Int>>
    {
        {  0, new List<Vector3Int>      { new Vector3Int(0, 0, 1),   new Vector3Int(-1, 0, 1),    new Vector3Int(0, 0, 2),   new Vector3Int(-1, 0, 2) } }, // ㅁ
        {  1, new List<Vector3Int>      { new Vector3Int(0, 0, 1),   new Vector3Int(0, 0, 2),    new Vector3Int(-1, 0, 2)    } }, // ┓
        {  2, new List<Vector3Int>      { new Vector3Int(0, 0, 1),   new Vector3Int(0, 0, 2),    new Vector3Int(1, 0, 2)     } }, // ┏
        {  3, new List<Vector3Int>      { new Vector3Int(0, 0, 1),   new Vector3Int(0, 0, 2)                                 } }, // 아래 |
        {  4, new List<Vector3Int>      { new Vector3Int(0, 0, 1),   new Vector3Int(-1, 0, 1)                                 } }, // 아래 |
        {  5, new List<Vector3Int>      { new Vector3Int(0, 0, 1),   new Vector3Int(1, 0, 1)                                 } }, // 아래 |
        {  6, new List<Vector3Int>      { new Vector3Int(0, 0, 1)                                                            } }, // 아래 |
    };
    public Dictionary<int, List<Vector3Int>> upPatten = new Dictionary<int, List<Vector3Int>>
    {
        {  0, new List<Vector3Int>      { new Vector3Int(0, 0, -1),   new Vector3Int(0, 0, -2),  new Vector3Int(-1, 0, -1),  new Vector3Int(-1, 0, -2)} }, // ㅁ
        {  1, new List<Vector3Int>      { new Vector3Int(0, 0, -1),   new Vector3Int(0, 0, -2),   new Vector3Int(1, 0, -2)    } }, // ┏
        {  2, new List<Vector3Int>      { new Vector3Int(0, 0, -1),   new Vector3Int(0, 0, -2),   new Vector3Int(-1, 0, -2)   } }, // ┐
        {  3, new List<Vector3Int>      { new Vector3Int(0, 0, -1),   new Vector3Int(0, 0, -2)                                } }, // 위 |
        {  4, new List<Vector3Int>      { new Vector3Int(0, 0, -1),   new Vector3Int(1, 0, -1)                                } }, // 위 |
        {  5, new List<Vector3Int>      { new Vector3Int(0, 0, -1),   new Vector3Int(-1, 0, -1)                                } }, // 위 |
        {  6, new List<Vector3Int>      { new Vector3Int(0, 0, -1)                                                           } }, // 위 |
    };
    public Dictionary<int, List<Vector3Int>> leftPatten = new Dictionary<int, List<Vector3Int>>
    {
        {  0, new List<Vector3Int>      { new Vector3Int(-1, 0, 0),  new Vector3Int(-2, 0, 0),   new Vector3Int(-1, 0, -1),  new Vector3Int(-2, 0, -1) } }, // ㅁ
        {  1, new List<Vector3Int>      { new Vector3Int(-1, 0, 0),  new Vector3Int(-2, 0, 0),   new Vector3Int(-2, 0, -1)   } }, // └ 
        {  2, new List<Vector3Int>      { new Vector3Int(-1, 0, 0),  new Vector3Int(-2, 0, 0),   new Vector3Int(-2, 0, 1)    } }, // ┌
        {  3, new List<Vector3Int>      { new Vector3Int(-1, 0, 0),  new Vector3Int(-2, 0, 0)                                } }, // 왼쪽  --
        {  4, new List<Vector3Int>      { new Vector3Int(-1, 0, 0),  new Vector3Int(-1, 0, -1)                                } }, // 왼쪽  --
        {  5, new List<Vector3Int>      { new Vector3Int(-1, 0, 0),  new Vector3Int(-1, 0, 1)                                } }, // 왼쪽  --
        {  6, new List<Vector3Int>      { new Vector3Int(-1, 0, 0)                                                           } }, // 왼쪽  --
    };
    public Dictionary<int, List<Vector3Int>> rightPatten = new Dictionary<int, List<Vector3Int>>
    {
        {  0, new List<Vector3Int>      { new Vector3Int(1, 0, 0),   new Vector3Int(2, 0, 0),    new Vector3Int(1, 0, 1) ,   new Vector3Int(2, 0, 1) } }, // ㅁ
        {  1, new List<Vector3Int>      { new Vector3Int(1, 0, 0),   new Vector3Int(2, 0, 0),    new Vector3Int(2, 0, 1)     } }, // ┐
        {  2, new List<Vector3Int>      { new Vector3Int(1, 0, 0),   new Vector3Int(2, 0, 0),    new Vector3Int(2, 0, -1)    } }, // ┛ 
        {  3, new List<Vector3Int>      { new Vector3Int(1, 0, 0),   new Vector3Int(2, 0, 0)                                 } }, // 오른쪽  --
        {  4, new List<Vector3Int>      { new Vector3Int(1, 0, 0),   new Vector3Int(1, 0, 1)                                 } }, // 오른쪽  --
        {  5, new List<Vector3Int>      { new Vector3Int(1, 0, 0),   new Vector3Int(1, 0, -1)                                 } }, // 오른쪽  --
        {  6, new List<Vector3Int>      { new Vector3Int(1, 0, 0)                                                            } },
    };

    public List<RoomInfo> mainRoot = new List<RoomInfo>();
    public List<Vector3Int> shortParentRoot = new List<Vector3Int>();


    private int minRoomCnt = 10;
    private int maxRoomCnt = 15;
    private int maxDistance = 8;

    public int currRoomCnt = 0;

    public int mainRoomCount = 0;


    public Vector3Int startPoint;
    public Vector3Int BossPoint;

    public List<RoomInfo> specialRoomList = new List<RoomInfo>();

    public RoomInfo[,] posArr = new RoomInfo[10, 10];

    public void Awake()
    {
        if (!Instance)
            Instance = this;
        else
            Destroy(this);
    }
    public void CreatedRoom(int nMinRoomCnt, int nMaxRoomCnt, int nMaxDistance)
    {

        if (0 < nMinRoomCnt){
            minRoomCnt = nMinRoomCnt;
        }

        if (0 < nMaxRoomCnt) {
            maxRoomCnt = nMaxRoomCnt;
        }

        if (0 < nMaxDistance) {
            maxDistance = nMaxDistance;
        }

        // 배열 ReSize
        posArr = (RoomInfo[,])ResizeArray(posArr, new int[] { (maxDistance * 2), (maxDistance * 2) });

        RealaseRoom();

        //int x = Random.Range(0, maxDistance) + (int)(maxDistance / 2);
        //int z = Random.Range(0, maxDistance) + (int)(maxDistance / 2);

        int x = (int)(maxDistance);
        int z = (int)(maxDistance);

        startPoint = new Vector3Int(x, 0, z);
        Stopwatch sw = new Stopwatch();
        sw.Start();

        posArr[startPoint.z, startPoint.x] = singleRoom(new RoomInfo(), startPoint, RoomType.Single);
        posArr[startPoint.z, startPoint.x].weightCnt = 0;
        posArr[startPoint.z, startPoint.x].roomName = RoomName.Start;

        mainRoot.Add(posArr[startPoint.z, startPoint.x]);
        countRoomWeight(startPoint, startPoint);

        currRoomCnt++;

        while (true)
        {
            if (!(minRoomCnt <= currRoomCnt && currRoomCnt <= maxRoomCnt))
            {
                //makeMainRoot();
                RoomSort(mainRoot);

                int Mainrand = Random.Range(0, mainRoot.Count);

                Vector3Int position = new Vector3Int(mainRoot[Mainrand].currPos.x, 0, mainRoot[Mainrand].currPos.z);
                makeRoomArray(position);
            }
            else
                break;
        }
        sw.Stop();

        countRoomWeight(startPoint, startPoint);
        //makeMainRoot();
        RoomSort(mainRoot);


        //UnityEngine.Debug.Log("방생성 시간(Normal) : " + sw.ElapsedMilliseconds.ToString() + "ms");

        // 처리 프로세스
        // while, for, foreach 등

        ////// 특수방 생성 1 : Boss
        selectBossRoom();

        sw.Reset();

        sw.Start();

        // BOSS 방까지의 경로를 구하기 위한 메쏘드
        shortMainRoot(startPoint);

        // Room이 겹친 부분을 삭제
        //allRoundCheckMapDelete();

        sw.Stop();

        //UnityEngine.Debug.Log("방생성 시간(shortMainRoot) : " + sw.ElapsedMilliseconds.ToString() + "ms");
        sw.Reset();



        findSpecialRoomList();



        ////// 특수방 생성 2 : Shop
        selectSpecialRoom(RoomName.Shop, 3);

        sw.Stop();

        //UnityEngine.Debug.Log("방생성 시간(Shop) : " + sw.ElapsedMilliseconds.ToString() + "ms");

        sw.Reset();
        sw.Start();

        //////// 특수방 생성 3 : Item
        selectSpecialRoom(RoomName.Item, 2);
        //sw.Stop();
        //UnityEngine.Debug.Log("방생성 시간(Item) : " + sw.ElapsedMilliseconds.ToString() + "ms");

        sw.Reset();
        sw.Start();

        ////// 특수방 생성 4 : Battle
        selectSpecialRoom(RoomName.Battle, 3);

        sw.Stop();
        //UnityEngine.Debug.Log("방생성 시간(Battle) : " + sw.ElapsedMilliseconds.ToString() + "ms");

        ////// 특수방 생성 5 : Hidden
        selectHiddenRoom();

        setupPosition();


    }

    public void selectSpecialRoom(RoomName specialRoomName, int minDistance, bool rand = false)
    {
        if(rand)
            Shuffle(specialRoomList);
        else
            RoomSort(specialRoomList);

        // 방의 최소 거리
        Vector3Int specialRoomPos = new Vector3Int();
        bool specialRoomSelected = false;

        for (int i=0; i<  specialRoomList.Count; i++)
        {
            if (specialRoomList[i].weightCnt >= minDistance)
            {
                specialRoomPos = specialRoomList[i].currPos;

                for (int idx = 0; idx < direction4.Count; idx++)
                {
                    Vector3Int sideMove = specialRoomPos + direction4[idx];

                    if (possibleArr(sideMove) 
                        && posArr[sideMove.z, sideMove.x].roomName == RoomName.Normal
                        && posArr[sideMove.z, sideMove.x].roomState)
                    {
                        Vector3Int selectShopPos = specialRoomList[i].currPos;

                        posArr[specialRoomPos.z, specialRoomPos.x].roomName = specialRoomName;
                        posArr[specialRoomPos.z, specialRoomPos.x].roomState = true;
                        posArr[specialRoomPos.z, specialRoomPos.x].currPos = specialRoomPos;
                        posArr[specialRoomPos.z, specialRoomPos.x].parentPos = specialRoomPos;
                        posArr[specialRoomPos.z, specialRoomPos.x].CenterPos = specialRoomPos;
                        posArr[specialRoomPos.z, specialRoomPos.x].roomType = RoomType.Single;
                        posArr[specialRoomPos.z, specialRoomPos.x].weightCnt = specialRoomList[i].weightCnt;
                        posArr[specialRoomPos.z, specialRoomPos.x].roomScore = roomScore[specialRoomName];

                        specialRoomSelected = true;
                        specialRoomList.RemoveAt(i);
                        break;
                    }
                }
                if (specialRoomSelected) 
                {
                    for (int j = 0; j < direction4.Count; j++)
                    {
                        Vector3Int sideMove = specialRoomPos + direction4[j];

                        for (int k = 0; k < specialRoomList.Count; k++)
                        {
                            if (sideMove == specialRoomList[k].currPos)
                            {
                                specialRoomList.RemoveAt(k);
                                k--;
                            }
                        }
                    }
                    break;
                }
            }
        }
    }
    public void selectHiddenRoom()
    {
        // Hidden 20개 이상 2 : 10개 이상 1개
        // ex) Hidden side 3개인 방이 1개밖에 없을 경우 우선적으로 1개 select
        // 이후 2개짜리 방 중 하나 select

        // 1. 3개짜리 방을 select 
        // 2. 3개짜리 방이 원하는 만큼 count가 없을 경우 2개 짜리 방으로 select
        //int hiddenRoomCount = Mathf.RoundToInt(mainRoot.Count / 10) < 1 ? 1 : Mathf.RoundToInt(mainRoot.Count / 10);
        int hiddenRoomCount = 2;

        for (int sideCnt = direction4.Count - 1; 0 < sideCnt; sideCnt--)
        {
            List<Vector3Int> HiddenRoomList = new List<Vector3Int>();
            bool isPossible = false;

            for (int i = 0; i < maxDistance * 2; i++)
            {
                for (int j = 0; j < maxDistance * 2; j++)
                {
                    Vector3Int move = new Vector3Int(i, 0, j);
                    int sideCount = aroundRoomCount(move);

                    if (!posArr[move.z, move.x].roomState && sideCount == sideCnt)
                    {
                        for (int side = 0; side < direction4.Count; side++)
                        {
                            Vector3Int checkMove = move + direction4[side];
                            if (possibleArr(checkMove))
                            {
                                if (posArr[checkMove.z, checkMove.x].roomName == RoomName.Boss
                                    || posArr[checkMove.z, checkMove.x].roomName == RoomName.Shop)
                                {
                                    sideCount--;
                                }
                            }
                        }

                        if (sideCount == sideCnt)
                        {
                            HiddenRoomList.Add(move);
                            isPossible = true;
                        }
                    }
                }
            }

            if (isPossible && 0 < hiddenRoomCount)
            {
                // 만약 3개 짜리 후보가 없을 경우
                // 아래에 해당하는 반복문 실행 X

                // 20개 기준 시
                // 만약 3개 짜리 후보가 1개만 있을 경우
                // 만약 2개 짜리 후보에서 1개 추출
                // HiddenRoomList.Count = 1, hiddenRoomCount = 2

                int tempList = (HiddenRoomList.Count < hiddenRoomCount) ? HiddenRoomList.Count : hiddenRoomCount;
                
                List<Vector3Int> hiddenRoom = ChooseSet(tempList, HiddenRoomList);

                hiddenRoomCount -= tempList;

                for (int i = 0; i < hiddenRoom.Count; i++)
                {
                    posArr[hiddenRoom[i].z, hiddenRoom[i].x].roomName = RoomName.Hidden;
                    posArr[hiddenRoom[i].z, hiddenRoom[i].x].roomState = true;
                    posArr[hiddenRoom[i].z, hiddenRoom[i].x].currPos = hiddenRoom[i];
                    posArr[hiddenRoom[i].z, hiddenRoom[i].x].parentPos = hiddenRoom[i];
                    posArr[hiddenRoom[i].z, hiddenRoom[i].x].CenterPos = hiddenRoom[i];
                    posArr[hiddenRoom[i].z, hiddenRoom[i].x].roomType = RoomType.Single;
                    posArr[hiddenRoom[i].z, hiddenRoom[i].x].weightCnt = 0;
                    posArr[hiddenRoom[i].z, hiddenRoom[i].x].roomScore = roomScore[RoomName.Hidden];
                }
            }

            if (isPossible && hiddenRoomCount <= 0)
                break;
        }   
    }
    public void allRoundCheckMapDelete()
    {
        for (int i = 0; i < maxDistance * 2; i++)
        {
            for (int j = 0; j < maxDistance * 2; j++)
            {
                Vector3Int setPos = new Vector3Int(i, 0, j);
                bool isTrue = false;

                if (posArr[setPos.z, setPos.x].roomState
                    && posArr[setPos.z, setPos.x].roomType == RoomType.Single)
                {
                    for (int idx = 0; idx < shortParentRoot.Count; idx++)
                    {
                        if (shortParentRoot[idx] == setPos)
                        {
                            isTrue = true;
                            break;
                        }
                    }

                    // 삭제하는 주변 방이 1개만 연결되지 않았을 경우

                    for (int idx = 0; idx < direction4.Count; idx++)
                    {
                        Vector3Int sidePos = setPos + direction4[idx];


                        if (possibleArr(sidePos) &&
                            (aroundRoomCount(sidePos) <= 2))
                        {
                            isTrue = true;
                            break;
                        }
                    }

                    if ((aroundRoomCount(setPos) == 4) && !isTrue)
                    {
                        deleteMainRoot(setPos);
                    }
                }
            }
        }
    }
    public void deleteMainRoot(Vector3Int pos)
    {
        for (int i = 0; i < mainRoot.Count; i++)
        {
            if (mainRoot[i].currPos == pos)
            {
                mainRoot.RemoveAt(i);

                posArr[pos.z, pos.x].roomName = RoomName.Normal;
                posArr[pos.z, pos.x].roomState = false;
                posArr[pos.z, pos.x].currPos = pos;
                posArr[pos.z, pos.x].parentPos = pos;
                posArr[pos.z, pos.x].CenterPos = pos;
                posArr[pos.z, pos.x].roomType = 0;
                posArr[pos.z, pos.x].weightCnt = -1;
                posArr[pos.z, pos.x].roomScore = 0;

                currRoomCnt--;
            }
        }
    }

    public bool shortMainRoot(Vector3Int currentPos)
    {
        if (shortParentRoot.Count <= 0)
            shortParentRoot.Add(currentPos);

        if (!possibleArr(currentPos))
            return false;

        if (currentPos == BossPoint)
            return true;

        // currentPos = 현재 위치
        int weight = posArr[currentPos.z, currentPos.x].weightCnt;

        for (int i = 0; i < direction4.Count; i++)
        {
            Vector3Int newPosition = (currentPos + direction4[i]);
            // 1. 다음 행선지가 +1의 값을 가지고 있지 않다면 false
            // 2. 갈 곳이 없다면 false 반환

            if (possibleArr(newPosition) && posArr[newPosition.z, newPosition.x].roomState
                && posArr[newPosition.z, newPosition.x].weightCnt == weight + 1)
            {
                shortParentRoot.Add(newPosition);

                if (!shortMainRoot(newPosition))
                    shortParentRoot.RemoveAt(shortParentRoot.Count - 1);
                else
                    return true;
            }
        }
        return false;
    }


    public void findSpecialRoomList()
    {
        for(int i=0; i< (maxDistance * 2); i++)
        {
            for(int j=0; j< (maxDistance * 2); j++)
            {
                if (!posArr[j, i].roomState)
                {
                    if (aroundRoomCount(posArr[j, i].currPos) == 1)
                    {

                        for (int idx = 0; idx < direction4.Count; idx++)
                        {
                            Vector3Int findPos = posArr[j, i].currPos + direction4[idx];

                            if (possibleArr(findPos))
                            {
                                if (posArr[findPos.z, findPos.x].roomState) 
                                {
                                    RoomInfo tmpRoom = new RoomInfo();
                                    tmpRoom.currPos = posArr[j, i].currPos;
                                    tmpRoom.weightCnt = posArr[findPos.z, findPos.x].weightCnt + 1;
                                    tmpRoom.parentPos = posArr[j, i].currPos;

                                    specialRoomList.Add(tmpRoom);
                                }
                            }
                        }
                    }
                }
            }
        }
    }


    public List<Vector3Int> ChooseSet(int numRequired, List<Vector3Int> list)
    {
        List<Vector3Int> result = new List<Vector3Int>();

        int numToChoose = numRequired;

        for (int numLeft = list.Count; numLeft > 0; numLeft--)
        {
            float prob = (float)numToChoose / (float)numLeft;

            if (Random.value <= prob)
            {
                numToChoose--;
                result.Add(list[numLeft - 1]);

                if (numToChoose == 0)
                {
                    break;
                }
            }
        }
        return result;
    }


    public RoomInfo singleRoom(RoomInfo room, Vector3Int pos, RoomType roomtype)
    {
        RoomInfo single = room;
        single.roomID = name + "(" + pos.x + ", " + pos.y + ", " + pos.z + ")";
        single.roomName = RoomName.Normal;
        single.currPos = pos;
        single.parentPos = pos;
        single.roomType = roomtype;
        single.roomState = true;
        single.roomScore = roomScore[RoomName.Normal];


        return single;
    }

    // 시작 방에서 해당 방까지의 거리 계산
    public void countRoomWeight(Vector3Int currentPos, Vector3Int prePos)
    {
        // currentPos = 현재 위치
        // prePos      = 이전 위치
        if (!possibleArr(currentPos))
            return;

        int weight = posArr[currentPos.z, currentPos.x].weightCnt;

        for (int i = 0; i < direction4.Count; i++)
        {
            Vector3Int newPosition = currentPos + direction4[i];

            if (possibleArr(newPosition) && newPosition != prePos)
            {
                // 새로운 위치가 활성화가 되었을 경우
                if (posArr[newPosition.z, newPosition.x].roomState)
                {
                    // 새로운 위치가 탐색했던 곳일 경우
                    if (posArr[newPosition.z, newPosition.x].weightCnt != -1)
                    {
                        if ((weight + 1) <= posArr[newPosition.z, newPosition.x].weightCnt)
                        {
                            posArr[newPosition.z, newPosition.x].weightCnt = weight + 1;
                            countRoomWeight(newPosition, currentPos);
                        }
                    }// 새로운 위치가 탐색하지 않은 곳일 경우
                    else
                    {
                        posArr[newPosition.z, newPosition.x].weightCnt = weight + 1;
                        countRoomWeight(newPosition, currentPos);
                    }
                }
            }
        }
    }

    public void RoomSort(List<RoomInfo> root)
    {
        root.Sort(delegate (RoomInfo A, RoomInfo B)
        {
            if (A.weightCnt > B.weightCnt)
                return 1;
            else if (A.weightCnt < B.weightCnt)
                return -1;
            else
                return 0;
        });
    }

    public void vectorPosSort(List<Vector3Int> root)
    {
        root.Sort(delegate (Vector3Int A, Vector3Int B)
        {
            if (posArr[A.z, A.x].weightCnt > posArr[B.z, B.x].weightCnt)
                return 1;
            else if (posArr[A.z, A.x].weightCnt < posArr[B.z, B.x].weightCnt)
                return -1;
            else
                return 0;
        });
    }

    public bool possibleArr(Vector3Int pos)
    {
        if ((0 <= (pos).x && (pos).x < (maxDistance * 2))
            && (0 <= (pos).z && (pos).z < (maxDistance * 2)))
        {
            return true;
        }
        else
            return false;
    }


    public void selectBossRoom()
    {

        bool selectBossRoom = false;

        Dictionary<Vector3Int, Vector3Int> bossPosDic = new Dictionary<Vector3Int, Vector3Int>();
        List<Vector3Int> bossPos = new List<Vector3Int>();
        
        for (int idx = mainRoot.Count - 1; 0 < idx; idx--)
        {
            if (!selectBossRoom && mainRoot[idx].weightCnt <= 6)
            {
                Vector3Int pos = mainRoot[idx].currPos;
                
                // 2. 거리가 6 이상인 목록이 없을 경우 거리가 순차적으로 감소하며 Boss방 Select

                for (int i = 0; i < direction4.Count; i++)
                {
                    selectBossRoom = false;
                    Vector3Int bossMove = posArr[pos.z, pos.x].currPos + direction4[i];

                    if (possibleArr(bossMove))
                    {
                        if ((aroundRoomCount(bossMove) < 2) && !posArr[bossMove.z, bossMove.x].roomState)
                        {
                            bossPosDic.Add(bossMove, pos);
                            bossPos.Add(bossMove);
                            if (bossPosDic.Count >= 3)
                                break;
                        }
                    }
                }
                if (bossPosDic.Count >= 3)
                    break;
            }
        }
        // 3개의 후보 중 선택
        int randIndex = Random.Range(0, bossPos.Count);
        
        Vector3Int bossSideRoom = bossPosDic[bossPos[randIndex]];

        posArr[bossPos[randIndex].z, bossPos[randIndex].x].roomName     = RoomName.Boss;
        posArr[bossPos[randIndex].z, bossPos[randIndex].x].roomState    = true;
        posArr[bossPos[randIndex].z, bossPos[randIndex].x].currPos      = bossPos[randIndex];
        posArr[bossPos[randIndex].z, bossPos[randIndex].x].parentPos    = bossPos[randIndex];
        posArr[bossPos[randIndex].z, bossPos[randIndex].x].CenterPos    = bossPos[randIndex];
        posArr[bossPos[randIndex].z, bossPos[randIndex].x].weightCnt    = posArr[bossSideRoom.z, bossSideRoom.x].weightCnt + 1;
        posArr[bossPos[randIndex].z, bossPos[randIndex].x].roomType     = RoomType.Single;
        posArr[bossPos[randIndex].z, bossPos[randIndex].x].roomScore    = roomScore[RoomName.Boss];

        BossPoint = bossPos[randIndex];
        selectBossRoom = true;
    }

    // 방의 배열을 초기화
    public void roomPosInit()
    {
        for (int i = 0; i < (maxDistance * 2); i++)
        {
            for (int j = 0; j < (maxDistance * 2); j++)
            {
                posArr[j, i] = new RoomInfo();
                posArr[j, i].roomState = false;
                posArr[j, i].currPos = new Vector3Int(i, 0, j);
                posArr[j, i].weightCnt = -1;
            }
        }
    }
    // 모든 변수를 초기화
    public void RealaseRoom()
    {
        roomPosInit();
        mainRoot.Clear();
        shortParentRoot.Clear();
        specialRoomList.Clear();
        currRoomCnt = 0;
    }

    // 배열의 방들을 RoomController의 List로 변환
    public void setupPosition()
    {
        List<RoomInfo> positions = new List<RoomInfo>();

        for (int i = 0; i < (maxDistance * 2); i++)
        {
            for (int j = 0; j < (maxDistance * 2); j++)
            {
                if (posArr[j, i].roomState)
                {
                    Vector3Int tmp = new Vector3Int(i, 0, j);

                    posArr[j, i] = singleRoom(posArr[j, i], posArr[j, i].roomType);

                    posArr[j, i].currPos = tmp - startPoint;
                    posArr[j, i].parentPos = posArr[j, i].parentPos - startPoint;
                    posArr[j, i].CenterPos = posArr[j, i].CenterPos - startPoint;

                    positions.Add(posArr[j, i]);
                }
            }
        }
        mainRoomCount = mainRoot.Count;

        for (int i = 0; i < positions.Count; i++)
            RoomController.Instance.LoadRoom(positions[i]);

            }
    public void makeMainRoot()
    {
        mainRoot.Clear();

        for (int i = 0; i < (maxDistance * 2); i++)
        {
            for (int j = 0; j < (maxDistance * 2); j++)
            {
                if (posArr[j, i].roomState)
                {
                    mainRoot.Add(posArr[j, i]);
                }
            }
        }
    }

    public RoomInfo singleRoom(RoomInfo pos, RoomType name)
    {
        RoomInfo single = pos;
        single.roomID = name + "(" + pos.currPos.x + ", " + pos.currPos.y + ", " + pos.currPos.z + ")";
        single.roomName = pos.roomName;
        single.currPos = pos.currPos;
        single.parentPos = pos.parentPos;
        single.roomType = pos.roomType;
        single.weightCnt = pos.weightCnt;
        single.roomScore = roomScore[pos.roomName];
        return single;
    }


    public bool possiblePatten(Vector3Int pos, List<Vector3Int> move)
    {
        bool possible = true;
        for (int i = 0; i < move.Count; i++)
        {
            Vector3Int next = pos + move[i];

            if (possibleArr(next))
            {
                if (posArr[next.z, next.x].roomState)
                    return false;
            }
            else
                return false;

        }

        return possible;
    }

    public int aroundRoomCount(Vector3Int pos)
    {
        int count = 0;

        // LEFT
        if ((0 <= (pos.x - 1) && (pos.x - 1) < (maxDistance * 2)))
        {
            if (posArr[pos.z, pos.x - 1].roomState)
            {
                count += 1;
            }
        }

        // RIGHT
        if ((0 <= (pos.x + 1) && (pos.x + 1) < (maxDistance * 2)))
        {
            if (posArr[pos.z, pos.x + 1].roomState)
            {
                count += 1;
            }
        }

        // TOP
        if ((0 <= (pos.z - 1) && (pos.z - 1) < (maxDistance * 2)))
        {
            if (posArr[pos.z - 1, pos.x].roomState)
            {
                count += 1;
            }
        }
        // DOWN
        if ((0 <= (pos.z + 1) && (pos.z + 1) < (maxDistance * 2)))
        {
            if (posArr[pos.z + 1, pos.x].roomState)
            {
                count += 1;
            }
        }

        return count;
    }
    public void Shuffle(List<Vector3Int> shufflePos)
    {
        for (int i = 0; i < shufflePos.Count; i++)
        {
            Vector3Int temp = shufflePos[i];
            int randomIndex = Random.Range(0, shufflePos.Count);
            shufflePos[i] = shufflePos[randomIndex];
            shufflePos[randomIndex] = temp;
        }
    }
    public void Shuffle(List<RoomInfo> shuffleRoominfo)
    {
        for (int i = 0; i < shuffleRoominfo.Count; i++)
        {
            RoomInfo temp = shuffleRoominfo[i];
            int randomIndex = Random.Range(0, shuffleRoominfo.Count);
            shuffleRoominfo[i] = shuffleRoominfo[randomIndex];
            shuffleRoominfo[randomIndex] = temp;
        }
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

    // 방의 갯수가 최소, 최대크기에 적합한지 체크
    public bool roomCountCheck()
    {
        return ((minRoomCnt <= currRoomCnt && currRoomCnt <= maxRoomCnt));
    }

    // Room Position 생성
    public void makeRoomArray(Vector3Int start)
    {
        if (start.x >= (maxDistance * 2) || start.z >= (maxDistance * 2))
            return;

        if ((minRoomCnt <= currRoomCnt && currRoomCnt <= maxRoomCnt))
            return;

        // 사각형 방, 기억자, 니은자, -, |
        double[] persent = { 0.01, 0.03, 0.03, 0.1, 0.1, 0.1, 0.8 };

        // 각 방향 패턴을 List화
        List<Dictionary<int, List<Vector3Int>>> allPatten
            = new List<Dictionary<int, List<Vector3Int>>> { downPatten, rightPatten, leftPatten, upPatten };


        for (int direction = 0; direction < direction4.Count; direction++)
        {
            bool directionsRand = (Random.value > 0.5f);

            if (directionsRand)
            {

                int thisPatten = (int)Choose(persent);

                if (!possiblePatten(start, allPatten[direction][thisPatten]))
                    return;

                if (!roomCountCheck())
                {
                    Vector3Int lastMove = start;
                    Vector3 currCenterPos = Vector3.zero;
                    int maxCount = allPatten[direction][thisPatten].Count;
                    for (int i = 0; i < maxCount; i++)
                    {
                        Vector3Int firstPos = allPatten[direction][thisPatten][0];
                        Vector3Int SecondPos = allPatten[direction][thisPatten][maxCount - 1];
                        currCenterPos = new Vector3((float)(firstPos.x + SecondPos.x) / 2, 0, (float)(firstPos.z + SecondPos.z) / 2);
                        Vector3Int move = start + allPatten[direction][thisPatten][i];

                        posArr[move.z, move.x].roomState = true;
                        posArr[move.z, move.x].roomName = RoomName.Normal;
                        posArr[move.z, move.x].currPos = start + allPatten[direction][thisPatten][i];
                        posArr[move.z, move.x].weightCnt = -1;
                        lastMove = move;
                        posArr[move.z, move.x].roomScore = roomScore[RoomName.Normal];

                        switch (allPatten[direction][thisPatten].Count)
                        {
                            case 2:
                                posArr[move.z, move.x].roomType = RoomType.Double;
                                posArr[move.z, move.x].parentPos = start + allPatten[direction][thisPatten][0];
                                posArr[move.z, move.x].CenterPos = start + currCenterPos;
                                break;
                            case 3:
                                posArr[move.z, move.x].roomType = RoomType.Triple;
                                posArr[move.z, move.x].parentPos = start + allPatten[direction][thisPatten][0];
                                posArr[move.z, move.x].CenterPos = start + allPatten[direction][thisPatten][1];

                                break;
                            case 4:
                                posArr[move.z, move.x].roomType = RoomType.Quad;
                                posArr[move.z, move.x].parentPos = start + allPatten[direction][thisPatten][0];
                                posArr[move.z, move.x].CenterPos = start + currCenterPos;
                                break;
                            default:
                                posArr[move.z, move.x].roomType = RoomType.Single;
                                posArr[move.z, move.x].parentPos = start + allPatten[direction][thisPatten][0];
                                posArr[move.z, move.x].CenterPos = start + currCenterPos;
                                break;
                        }
                        mainRoot.Add(posArr[move.z, move.x]);
                    }
                    currRoomCnt++;
                    makeRoomArray(lastMove);
                }
            }
        }
    }
    private static System.Array ResizeArray(System.Array arr, int[] newSizes)
    {
        if (newSizes.Length != arr.Rank)
            return null;

        var temp = System.Array.CreateInstance(arr.GetType().GetElementType(), newSizes);
        int length = arr.Length <= temp.Length ? arr.Length : temp.Length;
        System.Array.ConstrainedCopy(arr, 0, temp, 0, length);
        return temp;
    }
}
