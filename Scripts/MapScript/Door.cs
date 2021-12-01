using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//[RequireComponent(typeof[LocalNavMeshBuilder])]
public class Door : MonoBehaviour
{
    //public Room thisRoom;
    public SubRoom thisSubRoom;
    public Room nextRoom;
    public Door SideDoor;

    public DoorType doorType;
    public Transform doorPos;

    public bool isUpdate = false;

    public Vector3 currPos;
    // 히든 방
    public bool isHidden = false;

    //public void doorMeshUpdate()
    //{
    //    if (!meshLink)
    //        meshLink = GetComponent<OffMeshLink>();

    //    //Vector3 currPos = new Vector3(transform.position.x, transform.position.y, transform.position.z) + (transform.forward);
    //    //Transform newTrans = transform;
    //    //newTrans.localPosition = currPos;

    //    meshLink.startTransform = transform;

    //    meshLink.endTransform = SideDoor.transform;
    //}

    public void setHiddenCheck(bool check)
    {
        isHidden = check;
    }
    public void setNextRoom(Room _nextRoom)
    {
        nextRoom = _nextRoom;
    }
    public void setSideDoorPos()
    {
     
    }

    public void setThisRoom(SubRoom _thisRoom)
    {
        thisSubRoom = _thisRoom;
    }

    // HiddeDoor open 조건 : 방을 클리어 하고 그 방을 다시 방문 했을때, 히든 방을 가지고 있는 벽면과 맞닿았을때 히든방 생성
    // HiddeDoor을 포함하고 있는 벽 충돌 시 트리거 발동
    public void HiddenDoorTriggerCheck(GameObject target, GameObject HitObject)
    {
        if (!RoomController.Instance.currRoom)
            return;

        // thisSubRoom은 해당 방이 Hidden방과 연결된 문이 업데이트 되지 않았을때, 한번 실행한다.
        if (HitObject.tag == "Player" && !thisSubRoom.hiddenUpdate)
        {
            //// 조건 추가 필요(다른 방을 가싿가 다시 방문 시 오픈)
            //if (RoomController.Instance.currRoom.isRoomClear && RoomController.Instance.currRoom.reVisitedRoom)
            //{
            //    RoomController.Instance.HiddenRoomOpenDoor();
            //}

            // 해당 방 클리어 후 근접
            if (RoomController.Instance.currRoom.isRoomClear)
            {
                RoomController.Instance.HiddenRoomOpenDoor(target.GetComponent<Room>());
            }
            // 해당 방과 연결된 
            //thisSubRoom.ReplaceHiddenRoomDoor();
            //thisSubRoom.hiddenUpdate = true;

            ////nextRoom.TestHiddenRoomOpenDoor();
        }
    }

    // 방 입장 시 FadeIn, FadeOut 처리 및 미니맵 관련 Room Controller
    // 방 입장 시 출동하는 Door에 대한 트리거 처리
    public void DoorTriggerCheck(GameObject target, GameObject HitObject)
    {
        if (!RoomController.Instance.currRoom)
            return;

        if (RoomController.Instance.currRoom.isRoomClear)
        {
            if (HitObject.tag == "Player")
            {
                // nextRoom의 Door의 전방에 포지션 위치
                Vector3 SideDoorEnterPos = new Vector3(SideDoor.transform.position.x, Player.player.transform.position.y, SideDoor.transform.position.z) + (SideDoor.transform.forward * 3);

                nextRoom.roomTriggerCheck(SideDoorEnterPos);

                LocalNavMeshBuilder.Instance.enabled = false;

                Player.player.TeleportTo(SideDoorEnterPos);

                LocalNavMeshBuilder.Instance.enabled = true;
            }
        }
    }
}
