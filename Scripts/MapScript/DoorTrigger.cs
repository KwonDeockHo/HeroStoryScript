using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    public DoorAnimator parentDoor;
    public Collider doorCol;

    public void Awake()
    {
        if (transform.parent.GetComponent<DoorAnimator>() != null)
            parentDoor = transform.parent.GetComponent<DoorAnimator>();

        if (!doorCol)
            doorCol = transform.GetComponent<Collider>();
    }

    public void OnEnable()
    {
        if (!doorCol)
            doorCol = transform.GetComponent<Collider>();
        else
            return;

        //doorCol.enabled = false;

        //Invoke("ResetCollider2", 2f);
        //StartCoroutine("ResetCollider");
    }

    // 방 입장 시 FadeIn, FadeOut 처리 및 미니맵 관련 Room Controller
    public void OnTriggerEnter(Collider collision)
    {
        if (!RoomController.Instance.currRoom)
            return;

        if (RoomController.Instance.currRoom.isRoomClear)
        {
            if (collision.tag == "Player")
            {
                parentDoor.DoorTriggerCheck(transform.gameObject, collision.gameObject);   
            }
        }
    }
    void  ResetCollider2()
    {
        doorCol.enabled = true;
    }
    IEnumerator ResetCollider()
    {
        yield return new WaitForSeconds(2f);
        doorCol.enabled = true;
    }
}
