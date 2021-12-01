using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_TestMode : MonoBehaviour
{
    public void OnClickLevelUpButton()
    {
        if (!Player.player) return;
        Player.player.LevelUp();
    }

    public void OnClickSpawnMonster(Entity entity)
    {
        if (!entity) return;
        GameObject go = Instantiate(entity.gameObject);
        go.transform.position = Extensions.FindEmptySpace(Player.player.transform.position, entity.agent.radius, -1);
    }
}
