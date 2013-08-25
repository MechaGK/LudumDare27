using UnityEngine;
using System.Collections;

public class Lever : MonoBehaviour
{
    public GameObject target;
    public Texture activated;

    void ApplyDamage()
    {
        target.SendMessage("Activate");
        renderer.material.mainTexture = activated;
    }
}
