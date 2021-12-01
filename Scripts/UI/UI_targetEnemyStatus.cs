using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_targetEnemyStatus : MonoBehaviour
{
    public Entity owner;


    public Image image;
    public Text _name;
    public Image healthBar;
    public Image manaBar;
    public Image shieldBar;
    public Text healthValue;
    public Text shieldValue;
    private float shieldMax;

    public GameObject targetEnemy_StatusUI;
    public GameObject buff_Panel;


    // Update is called once per frame
    void Update()
    {
        Handling();
        //owner가 없으면 Setvisible False 있으면 True
        if (!owner)
        {
            targetEnemy_StatusUI.SetActive(false);
            return;
        }
        
        if (owner) {
            TargetEnemy(owner);
            if (owner.health <= 0)
            {
                targetEnemy_StatusUI.SetActive(false);
                owner = null;
            }
        }
    }

    public void Handling()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    var enemy_entitiy = hit.transform.GetComponent<Monster>();

                    if (enemy_entitiy)
                    {
                        if (owner != enemy_entitiy)
                        {
                            owner = null;
                            targetEnemy_StatusUI.SetActive(false);
                        }
                        owner = enemy_entitiy;
                        targetEnemy_StatusUI.SetActive(true);
                    }
                    else
                    {
                        targetEnemy_StatusUI.SetActive(false);
                        owner = null;
                    }
                }
            }
        }
    }


    public void TargetEnemy(Entity entity)
    {
        if (image) image.sprite = entity.sprite;
        if (_name) _name.text = entity.entityName;//name.ToString().Replace("(Clone)", "");
        if (healthBar) healthBar.fillAmount = (float)entity.health / ((float)entity.healthMax + (float)entity.shield);
        if (shieldBar) shieldBar.fillAmount = ((float)entity.health + (float)entity.shield) / ((float)entity.healthMax + (float)entity.shield);
        if (manaBar) manaBar.fillAmount = 0;
        if (healthValue) healthValue.text = Mathf.RoundToInt(entity.health).ToString() + " / " + entity.healthMax.ToString();


        if ((float)entity.shield > 0)
        {
            shieldValue.gameObject.SetActive(true);

            if (shieldMax < entity.shield)
                shieldMax = entity.shield;

        }
        else
        {
            shieldValue.gameObject.SetActive(false);
        }

        if (shieldValue) shieldValue.text = entity.shield.ToString() + " / " + shieldMax.ToString() + "  ";

    }

}
