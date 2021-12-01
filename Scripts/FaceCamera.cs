using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    public Camera mainCamera;
    public GameObject parentGameObject;
    public bool isReverse = false;
    public void Start()
    {
        if (!mainCamera)
            mainCamera = CameraManager.Instance.transform.GetComponent<Camera>();

        if (!parentGameObject)
            parentGameObject = transform.parent.gameObject;

    }
    void Update()
    {
        if (mainCamera)
        {
            transform.forward = mainCamera.transform.forward * (isReverse ? -1 : 1);
        }
        else
            mainCamera = CameraManager.Instance.transform.GetComponent<Camera>();

    }
}
