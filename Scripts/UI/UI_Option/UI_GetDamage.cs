using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_GetDamage : MonoBehaviour
{
    public static UI_GetDamage self;
    public Image getDamage;
    public Image getStun;

    public bool isGetDamage;
    public bool isGetStun;

    // Start is called before the first frame update
    void Start()
    {
        if (self) Destroy(this);
        else self = this;

        isGetDamage = true;
        isGetStun = true;
    }

    void Update()
    {
        Player player = Player.player;
        if (!player) return;

        if (isGetDamage == true)
            if(player.isDamage == true)
                StartCoroutine("DamageUI");
        
        if(isGetStun == true)
            if(player.state == State.Stun)
                StartCoroutine("DamageUI");
    }


    IEnumerator DamageUI()
    {
        for (float i = 0.4f; i >= 0; i -= 0.1f)
        {
            Color color = getDamage.color;
            color.a = i;

            getDamage.color = color;

            yield return new WaitForSeconds(0.2f);
        }
        if(Player.player)
            Player.player.isDamage = false;
    }
    
}
