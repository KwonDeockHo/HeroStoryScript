using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class minimapCam : MonoBehaviour
{
    public static minimapCam Instance;
    public Vector3 cameraPosition;
    public Player player;
    public Vector3 roomMaxPosition;
    public int cameraHeight = 20;

    public Vector3 maxRightPos = Vector3.zero;
    public Vector3 maxUpPos = Vector3.zero;

    public int roomSize = 48;
    public void Start()
    {
        if (!Instance)
            Instance = this;
        else
            Destroy(this);


        cameraPosition = new Vector3(0, cameraHeight, 0);
        this.gameObject.transform.position = cameraPosition;
    }

    // 방을 처음 방문했을때 호출
    public void moveMiniCamCamera(Room currRoom)
    {
        // 미니맵 세팅 
        // 1. 초기 0 위치에서 시작
        // 2. 위, 오른쪽 이동 시 새로 파악된 방에 대한 크기 비교
        //     * 위 : 카메라 위치 아래로 방 Center 크기 만큼 이동
        //     * 오른쪽 : 카메라 위치 왼쪽으로 방 Center 크기 만큼 이동
        // 
        // 현재 이동한 방이 미니맵의 왼쪽에 위치할 경우 카메라 이동 없음.
        Vector3 currRoomPos = currRoom.currPos * roomSize;

        if (currRoomPos.x < cameraPosition.x && currRoomPos.z < cameraPosition.z)
            return;

        // 현재 활성화된 맵들의 가장 높은 Z값
        // 모든 맵들 중 가장 높은 Z 값

        // // 현재 활성화된 맵들의 가장 높은 X값
        // 모든 맵들 중 가장 높은 X 값
        for (int i = 0; i < RoomController.Instance.loadedRooms.Count; i++)
        {
            // 미니맵이 활성화 되었을 경우만 계산
            if (RoomController.Instance.loadedRooms[i].rooms.minimapRoom.minimapAlive)
            {
                // 현재 카메라 위치와 오른쪽 방에 있는 위치와 차이가 2단계 이상일때 이동
                // 반복문을 통한 이동( 각 미니맵들의 포지션 )
                Vector3 cameraPos = (RoomController.Instance.loadedRooms[i].currPos * roomSize);

              //  Debug.Log("cameraPosition : " + cameraPosition + ", maxRightPos : " + maxRightPos + ", cameraPos : " + cameraPos);

                if (cameraPosition.x < cameraPos.x && roomSize * 1 < (maxRightPos.x - cameraPos.x)) {
                    cameraPosition.x = cameraPos.x;
                }

                if (cameraPosition.z < cameraPos.z && roomSize * 1 < (maxUpPos.z - cameraPos.z)) {
                    cameraPosition.z = cameraPos.z;
                }
            }     
            
        }
        transform.position = new Vector3(cameraPosition.x, cameraHeight, cameraPosition.z);

    }

}
