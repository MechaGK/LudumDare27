using UnityEngine;
using System.Collections;

public class Gate : MonoBehaviour
{
    bool activated = false;
    Vector3 pos;
    float posTimer = 0;

    void Start()
    {
        pos = transform.position;
    }

    void Update()
    {
        if (activated)
        {
            posTimer += Time.deltaTime * 2;
            transform.position = Vector3.Lerp(pos, pos + (Vector3.up * 3f), posTimer);
        }
    }

    void Activate()
    {
        activated = true;
    }
}
