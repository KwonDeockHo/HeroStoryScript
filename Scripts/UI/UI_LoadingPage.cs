using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_LoadingPage : MonoBehaviour
{
    public static UI_LoadingPage instance;
    public SceneLoader sceneLoader;
    public GameObject backGroundImg;
    public Text backGroundTooltip;

    public Sprite[] backGroundList;
    public string[] tipMessageText;

    public Text progressBarText;
    public Slider progressSlider;

    // Start is called before the first frame update
    void Start()
    {
        if (!instance)
            instance = this;
        else
            Destroy(this);

        if (!sceneLoader)
            sceneLoader = SceneLoader.Instance;

    }
    public void loadingSceneOnOff(bool state)
    {
        this.gameObject.SetActive(state);
    }
    public void selectRandomBackGround()
    {
        int randBackgroundImgIndex = Random.Range(0, backGroundList.Length);
        int randBackgroundTextIndex = Random.Range(0, tipMessageText.Length);

        backGroundImg.GetComponent<Image>().sprite = backGroundList[randBackgroundImgIndex];
        backGroundTooltip.text = tipMessageText[randBackgroundTextIndex].ToString();
    }

    public void ProgressSilderSetting(float persent)
    {
        progressSlider.value = persent;
        progressBarText.text = persent * 100f + "%";
    }

}
