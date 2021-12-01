using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_HealthBar : MonoBehaviour
{
    public Entity player;
        
    public Image healthBar;
    public Image manaBar;
    public Image shieldBar;
    public Image expBar;
    public Text healthValue;
    public Text manaValue;
    public Text shieldValue;
    public Text expValue;
    public Text levelValue;

    private float shieldMax;
    private void Start()
    {

        shieldMax = 0;
    }

    void Update()
    {
        if (!player) player = Player.player;
        if (player)
        {
            
            if (healthBar) healthBar.fillAmount = (float)player.health / ((float)player.healthMax + (float)player.shield);
            if (shieldBar) shieldBar.fillAmount = ((float)player.health + (float)player.shield) / ((float)player.healthMax + (float)player.shield);
            if (manaBar) manaBar.fillAmount = (float)player.mana / (float)player.manaMax;
            if (healthValue) healthValue.text = ((int)player.health).ToString() + " / " + ((int)player.healthMax).ToString();
            if (manaValue) manaValue.text = ((int)player.mana).ToString() + " / " + ((int)player.manaMax).ToString();

            if (expBar) expBar.fillAmount = (float)player.experience / ((float)player.experienceMax);
            


            if ((float)player.shield > 0)
            {
                shieldValue.gameObject.SetActive(true);

                if(shieldMax < player.shield)
                    shieldMax = player.shield;               
                
            }
            else
            {
                shieldValue.gameObject.SetActive(false);
            }
               
            if (shieldValue) shieldValue.text = ((int)player.shield).ToString() + " / " + ((int)shieldMax).ToString()+"  ";

            if (expValue) expValue.text = ((int)player.experience).ToString() + " / " + ((int)player.experienceMax).ToString();
            if (levelValue) levelValue.text = player.level.ToString();

        }
    }

}
