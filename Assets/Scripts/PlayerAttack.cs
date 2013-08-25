using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerAttack : MonoBehaviour
{
    List<GameObject> insiders = new List<GameObject>();
    public Renderer arms;

    public bool armsReset;
    float armsTimer;

    void Update()
    {
        if (Input.GetButtonDown("Fire1") && armsReset)
        {
            foreach (GameObject insider in insiders.ToArray())
            {
                if (insider == null)
                {
                    insiders.Remove(insider);
                }
                else
                {
                    insider.SendMessage("ApplyDamage", SendMessageOptions.DontRequireReceiver);
                }
            }

            armsReset = false;
            armsTimer = 0.5f;
        }

        if (!armsReset)
        {
            if (armsTimer > 0)
            {
                armsTimer -= Time.deltaTime;
            }
            else
            {
                armsReset = true;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        insiders.Add(other.gameObject);
        
    }

    void OnTriggerExit(Collider other)
    {
        foreach(GameObject insider in insiders.ToArray())
        {
            if (other.gameObject.GetInstanceID() == insider.GetInstanceID())
            {
                insiders.Remove(insider);
            }
        }
    }
}
