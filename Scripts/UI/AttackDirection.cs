using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class AttackDirection : MonoBehaviour
{
    public Entity owner;
    public GameObject dir;
   

    // Start is called before the first frame update
    void Start()
    {
        owner = gameObject.transform.parent.GetComponent<Entity>();

        dir.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (owner.state == State.Casting)
        {
            //obj.SetActive(true);
            //obj.transform.position = Vector3.Lerp(obj.transform.position, owner.target.transform.position, Time.deltaTime*3f);

            dir.SetActive(true);
        }
        else
        {
            //obj.SetActive(false);
            //obj.transform.position = gameObject.transform.position;

            dir.SetActive(false);
            dir.transform.position = gameObject.transform.position;
        }
    }
}
