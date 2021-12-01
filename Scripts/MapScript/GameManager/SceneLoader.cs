using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance;

    public UI_LoadingPage loadingScreen;
    public string nextScene;

    public void Awake()
    {
        if (!Instance)
            Instance = this;
        else
            Destroy(this);

        if (!loadingScreen)
            loadingScreen = UI_LoadingPage.instance;

        if(loadingScreen) loadingScreen.loadingSceneOnOff(false);
    }
    public void LoadPlayScene()
    {
        // 타이머 시작 (게임 최초 실행 시 시작)
        InGameManager.Instance.setTimerStatus(true);
        InGameManager.Instance.TimerStart();
        LoadScene(nextScene);
    }

    public void LoadStartScene()
    {
        loadingScreen.selectRandomBackGround();
        string lobbySceneStr = "LobbyScene";
        SceneManager.LoadScene(lobbySceneStr);
    }

    public void LoadScene(string SceneName)
    {
        nextScene = SceneName;
        loadingScreen.selectRandomBackGround();
        Debug.Log("LoadScene : " + SceneName);

        StartCoroutine(LoadSceneProcess(nextScene));
    }

    public void LoadScene(int SceneIndex)
    {
        loadingScreen.selectRandomBackGround();
       // Debug.Log("LoadScene : " + SceneName);

        StartCoroutine(LoadSceneProcess(SceneIndex));
    }
    IEnumerator LoadSceneProcess(int nextScene)
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        //op.allowSceneActivation = false;
        loadingScreen.loadingSceneOnOff(true);

        //float timer = 0f;

        while (!op.isDone)
        {
            float progress = Mathf.Clamp01(op.progress / 0.9f);
            loadingScreen.ProgressSilderSetting(progress);
            //Debug.Log("Loader : " + op.progress);
            yield return null;

        }
    }

    IEnumerator LoadSceneProcess(string nextScene)
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        //op.allowSceneActivation = false;
        loadingScreen.loadingSceneOnOff(true);
        
        //float timer = 0f;

        while (!op.isDone)
        {
            float progress = Mathf.Clamp01(op.progress / 0.9f);
            loadingScreen.ProgressSilderSetting(progress);
            //Debug.Log("Loader : " + op.progress);
            yield return null;

        }
    }


}
