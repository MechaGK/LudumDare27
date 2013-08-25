using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class ResetGameController : MonoBehaviour
{
    public bool reset;
    public GameController gc;

    void Update()
    {
        if (reset)
        {
            GameController.theRoomsEnd = gc.roomsEnd;
            GameController.CheckpointActivated = false;
            GameController.CurrentRoom = 1;
            GameController.gold = 0;
            reset = false;
        }
    }
}
