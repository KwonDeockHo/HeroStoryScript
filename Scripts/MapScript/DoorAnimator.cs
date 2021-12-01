using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAnimator : MonoBehaviour
{
    public Animator animator;
    public Door parentDoor;
    public bool animatorClose = false;
    public bool isOpen = false;

    public AudioSource audioSource;

    public void Awake()
    {
        if (GetComponent<Animator>() != null)
            animator = GetComponent<Animator>();
        else
        {
            animator = GetComponentInChildren<Animator>();
        }

        if (transform.parent.GetComponent<Door>() != null)
            parentDoor = transform.parent.GetComponent<Door>();
    }


    public void OpenDoor()
    {
        Debug.Log("Door Open Call");
        if (animatorClose)
        {
            animator.enabled = false;
            animatorClose = true;
        }

        animator.SetBool("IsOpen", true);
        animator.SetBool("IsClose", false);

        isOpen  = true;
        PlaySound_OpenDoor();
    }
    public void CloseDoor()
    {
        Debug.Log("Door Close Call");

        if (!animatorClose)
        {
            animator.enabled = true;
            animatorClose = false;
        }


        animator.SetBool("IsOpen", false);
        animator.SetBool("IsClose", true);

        isOpen = false;
    }

    public void DoorTriggerCheck(GameObject target, GameObject HitObject)
    {
        if (parentDoor != null)
        {
            if (target.tag == "DoorTrigger" && HitObject.tag == "Player")
            {
                Debug.Log("Romm 들어가기");
                parentDoor.DoorTriggerCheck(target.gameObject, HitObject.gameObject);
            }
        }
    }

    public void PlaySound_OpenDoor()
    {
        if(SettingManager.self && audioSource)
        {
            audioSource.volume = SettingManager.self.CalcFxValue();
            audioSource.Play();
        }
    }    
}
