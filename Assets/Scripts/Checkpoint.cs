using UnityEngine;
using System.Collections;

public class Checkpoint : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameController.CheckpointActivated = true;
            GetComponent<TextMesh>().color = Color.green;
        }
    }
}
