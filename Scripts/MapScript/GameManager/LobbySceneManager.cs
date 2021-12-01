using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbySceneManager : MonoBehaviour
{
    [Header("Stage")]
    public string nextStage;
    public SceneLoader sceneLoader;

    public void nextStageLoad()
    {
        sceneLoader.LoadScene(nextStage);
    }

    void Start()
    {
        if (InGameManager.Instance)
            Destroy(InGameManager.Instance.gameObject);

        if (Player.player)
            Destroy(Player.player.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
