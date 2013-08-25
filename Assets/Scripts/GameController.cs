using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour
{
    static bool firstStart = true;

    public static GameObject player;
    public GameObject death;
    public static int gold;
    static int checkpointGold;
    static int checkpointEnemies;

    public float[] roomsEnd;
    public static float[] theRoomsEnd;
    public static float roomEnd;
    public static float previousRoomEnd = -3;
    static int currentRoom = 1;

    float timer;
    public static int enemies;
    int totalEnemies;
    int totalGold;
    public TextMesh uiTimer;
    public TextMesh roomCounter;
    public TextMesh goldCounter;
    public TextMesh enemyCounter;
    float dTimer = 0;

    bool gameOver = false;
    public GameObject gameOverText;
    public TextMesh gameOverTitle;
    public TextMesh gameOverXAction;
    public TextMesh gameOverZAction;
    static bool checkpointActivated;

    public bool gameWin = false;

    public static bool CheckpointActivated
    {
        set
        {
            checkpointGold = gold;
            checkpointEnemies = enemies;
            checkpointActivated = value;
        }

        get
        {
            return checkpointActivated;
        }
    }

    public Vector3 checkpointPosition;

    public static int CurrentRoom
    {
        set
        {
            currentRoom = value;
            previousRoomEnd = theRoomsEnd[currentRoom - 1];
            roomEnd = theRoomsEnd[currentRoom];
        }

        get
        {
            return CurrentRoom;
        }
    }

    void Awake()
    {
        totalGold = GameObject.FindGameObjectsWithTag("Gold").Length;
        totalEnemies = GameObject.FindGameObjectsWithTag("Enemy").Length;
        theRoomsEnd = roomsEnd;
        CurrentRoom = 1;
        player = GameObject.FindGameObjectWithTag("Player");
        player.transform.position = checkpointActivated ? checkpointPosition : player.transform.position;
        Camera.main.transform.position = player.transform.position;
        timer = 10.0f;
    }

    void Update()
    {
        roomCounter.text = "Room: " + Mathf.Clamp(currentRoom, 1, roomsEnd.Length - 1).ToString() + "/" + (roomsEnd.Length - 1).ToString();
        goldCounter.text = "Gold collected: " + gold.ToString() + "/" + totalGold.ToString();
        enemyCounter.text = "Enemies killed: " + enemies.ToString() + "/" + totalEnemies.ToString();

        if (gold == totalGold)
        {
            goldCounter.color = Color.green;
        }

        if (enemies == totalEnemies)
        {
            enemyCounter.color = Color.green;
        }

        if (currentRoom - 1 == roomsEnd.Length - 2)
        {
            roomCounter.color = Color.green;
        }

        if (player.transform.position.x > roomEnd)
        {
            if (currentRoom + 1 < roomsEnd.Length)
            {
                CurrentRoom = currentRoom + 1;
                timer = 10.0f;
            }
            else
            {
                gameWin = true;
            }
        }

        uiTimer.text = timer.ToString("F3");
        uiTimer.color = Color.Lerp(Color.red, Color.white, timer / 10);

        if (!gameWin)
        {
            if (timer - Time.deltaTime >= 0)
            {
                timer -= Time.deltaTime;
            }
            else
            {
                timer = 0;
            }
        }

        if (timer == 0)
        {
            player.GetComponent<PlayerMovementCC>().enabled = false;
            player.transform.GetChild(1).GetComponent<PlayerAttack>().enabled = false;

            Color temp = death.renderer.material.color;

            temp.a += Time.deltaTime;
            death.renderer.material.color = temp;

            if (death.renderer.material.color.a >= 0.99 && !gameOver)
            {
                gameOver = true;

                gameOverText.SetActive(true);
                gameOverXAction.text += checkpointActivated ? "checkpoint" : "entrance";
                gameOverZAction.text = checkpointActivated ? "'Z' To retry from entrance" : "";
            }
        }

        if (gameOver)
        {
            if (Input.GetButton("Retry"))
            {
                Retry();
            }

            if (Input.GetButton("Cancel"))
            {
                Application.Quit();
            }
        }

        if (gameWin)
        {
            player.GetComponent<PlayerMovementCC>().enabled = false;
            player.transform.GetChild(1).GetComponent<PlayerAttack>().enabled = false;

            Color temp = death.renderer.material.color;

            temp.a += Time.deltaTime;
            death.renderer.material.color = temp;

            if (death.renderer.material.color.a >= 0.99 && !gameOverText.activeSelf)
            {
                gameOverText.SetActive(true);
                gameOverTitle.text = "Congratulations!\nYou completed the game";
                gameOverXAction.text = enemies + gold == totalEnemies + totalGold ? "You even got all gold and killed all enemies!" : "Try to get all gold and kill all enemies!";
                gameOverZAction.text = "'X' to try again!";
            }

            if (Input.GetButton("Retry"))
            {
                if (checkpointActivated)
                {
                    Retry(12, checkpointPosition);
                }
                else
                {
                    Retry();
                }
            }

            if (Input.GetButton("Cancel") && checkpointActivated)
            {
                Retry();
            }
        }
    }

    void Retry()
    {
        theRoomsEnd = roomsEnd;
        CheckpointActivated = false;
        CurrentRoom = 1;
        gold = 0;
        enemies = 0;
        gameOver = false;
        Application.LoadLevel(Application.loadedLevel);
    }

    void Retry(int room, Vector3 pos)
    {
        CurrentRoom = room;
        gameOver = false;
        gold = checkpointGold;
        enemies = checkpointEnemies;
        firstStart = false;
        Application.LoadLevel(Application.loadedLevel);
    }
}
