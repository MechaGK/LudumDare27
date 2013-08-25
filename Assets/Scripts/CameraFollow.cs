using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public float yPos;
    float cameraWidth;

    void Start()
    {
        player = GameController.player.transform;
        cameraWidth = (float)camera.orthographicSize * ((float)Screen.width / (float)Screen.height);
        transform.FindChild("Timer").transform.localPosition = new Vector3(-cameraWidth + 0.5f, camera.orthographicSize - 1.5f, 1);
        transform.FindChild("Room Counter").transform.localPosition = new Vector3(-cameraWidth + 0.5f, camera.orthographicSize - 0.5f, 1);
        transform.FindChild("Gold Counter").transform.localPosition = new Vector3(cameraWidth - 0.5f, camera.orthographicSize - 1.3f, 1);
        transform.FindChild("Enemies Counter").transform.localPosition = new Vector3(cameraWidth - 0.5f, camera.orthographicSize - 0.5f, 1);
    }

    void Update()
    {
        float previousRoom = GameController.previousRoomEnd;
        float roomEnd = GameController.roomEnd;

        if (cameraWidth * 2 < roomEnd - previousRoom)
        {
            transform.position = new Vector3(
                Mathf.Clamp(player.position.x, previousRoom + cameraWidth, roomEnd - cameraWidth),
                yPos,
                -10);
        }
        else
        {
            transform.position = new Vector3(
                (roomEnd + previousRoom) / 2,
                yPos,
                -10);
        }
    }
}