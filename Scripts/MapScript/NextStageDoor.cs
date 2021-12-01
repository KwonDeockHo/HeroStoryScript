using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextStageDoor : MonoBehaviour
{
    public int maxScale = 1;
    public float gameObjectScale = 0;


    public void OnEnable()
    {
        transform.localScale = Vector3.zero;
        StartCoroutine("ObjectScaleAnim");
    }

    private IEnumerator ObjectScaleAnim()
    {
        yield return new WaitForSeconds(.1f);
        Vector3 scale = Vector3.one;
        transform.localScale += (scale * 0.1f);
        gameObjectScale += 0.1f;

        if (maxScale >= gameObjectScale)
            StartCoroutine("ObjectScaleAnim");
        else
            StopCoroutine("ObjectScaleAnim");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            var player = other.GetComponent<Player>();
            if (!player) return;

            StageManager.Instance.gameNextStage();

            //if (UI_Toggle.self) UI_Toggle.self.OpenUI_Store();
        }
    }
}
