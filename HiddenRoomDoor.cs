using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenRoomDoor : MonoBehaviour
{
    private void OnTriggerEnter(Collider collision)
    {
        // IsLoadingRoom : 방 생성 완료됬는지 체크
        if (collision.tag == "Player" && RoomController.Instance.isLoadingRoom)
        {
            //Debug.Log("Hidden Room Trigger");

            this.transform.parent.GetComponent<Door>().HiddenDoorTriggerCheck(transform.gameObject, collision.gameObject);
        }
    }
}
