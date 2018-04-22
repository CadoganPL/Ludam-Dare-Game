using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class GameManager : MonoBehaviour
{

    public static GameManager instance = null;
    //modes
    enum GameMode {Local,Network };
    int gameMode = 0;
    int gameRound = 0;
    //Obstacle
    public GameObject prefab_Block;
    private List<GameObject> blockPool = new List<GameObject>();
    private List<GameObject> blocksOnScreen = new List<GameObject>();
    public float spawnTimer;
    private float timeToSpawn;
    public float globalSpeed = 1f;
    public Vector2[] points_SpawnLocations;
    //player
    private PlayerController _player;

    //States
    public float[] score = new float[2];
    public bool gameStart, gamePause, gameOver;

    /// <summary>
    /// 0 : Bottom, 1 : Middle, 2 : Top, null : Not set(Give blocks Randomly)
    /// </summary>
    public int? NextBlockSpawnLocation = null;

    //UI
    public GameObject[] UIPanels;
    [SerializeField]
    private Text ScoreText;
    [SerializeField]
    private Text infoText;
    [SerializeField]
    private Button switchButton;
    [SerializeField]
    private Button restartButton;
    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        _player = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    // Use this for initialization
    void Start()
    {
        ResetEachRound();
        PoolBlocks();
    }

    // Update is called once per frame
    void Update()
    {
        UIUpdate();
        Spawner();
        if (GameInProgress())
        {
            score[gameRound] += Time.deltaTime;
        }
    }


    //Spawners
    void PoolBlocks()
    {
        int i = 0;
        for (i = 0; i < 10; i++)
        {
            GameObject go = Instantiate(prefab_Block, points_SpawnLocations[0], Quaternion.identity) as GameObject;
            go.name = "Block_" + i;
            go.SetActive(false);
            blockPool.Add(go);
        }
    }

    void Spawner()
    {
        if (GameInProgress())
        {
            timeToSpawn += Time.deltaTime;
            int i = 0;
            if (timeToSpawn >= spawnTimer)
            {
                for (i = 0; i < blockPool.Count; i++)
                {
                    if (!blockPool[i].activeSelf)
                    {
                        int rand = Random.Range(0, 3);
                        blockPool[i].transform.position = points_SpawnLocations[rand];
                        blocksOnScreen.Add(blockPool[i]);

                        if (NextBlockSpawnLocation != null)
                        {
                            SpawnCardObstacle();
                            NextBlockSpawnLocation = null;
                        }

                        blockPool[i].SetActive(true);
                        break;
                    }
                }
                timeToSpawn = 0;
            }
        }

    }

    private void SpawnCardObstacle()
    {
        GameObject[] blockArray = blocksOnScreen.ToArray();
        blockArray = blockArray.OrderBy(x => Mathf.Abs(x.transform.position.x - _player.transform.position.x)).ToArray();
        Vector2 position = new Vector2((blockArray[0].transform.position.x + blockArray[1].transform.position.x) / 2, 0f);
        position.y = points_SpawnLocations[(int)NextBlockSpawnLocation].y;
        Instantiate(prefab_Block, position, Quaternion.identity);
    }

    private void RemovedObstacleFromScreen(GameObject Obstacle)
    {
        blocksOnScreen.Remove(Obstacle);
    }

    void ResetPoolObjects()
    {
        int i = 0;
        for (i = 0; i < blockPool.Count; i++)
        {
            blockPool[i].SetActive(false);
        }
    }

    //UI
    private void UIUpdate()
    {
        if (gameOver)
        {
            if (!UIPanels[1].activeSelf)
            {
                OpenUIPanel(1);
                SetGameOverScreen();
            }
        }
        else
        {
            ScoreText.text = "Time: " + Mathf.Round(score[gameRound]);
        }
    }

    //0-local , 1- multiplayer
    public void SelectMode(int a)
    {
        gameMode = a;
        StartGame();
    }


    void SetGameOverScreen()
    {
        if(gameRound==0)
        {
            infoText.text = "Player 1 survived " + Mathf.Round(score[gameRound]) + " seconds";
            switchButton.gameObject.SetActive(true);
            restartButton.gameObject.SetActive(false);
            gameRound++;
        }
        else
        {
            if(score[0]>score[1])
            {
                infoText.text = "Player 1 won";
            }
            else if (score[0] == score[1])
            {
                infoText.text = "Game Tie";
            }
            else
            {
                infoText.text = "Player 2 won";
            }
            switchButton.gameObject.SetActive(false);
            restartButton.gameObject.SetActive(true);
            ResetEachGame();
        }
    }

    public void CloseAlUIPanels()
    {
        int i = 0;
        for (i = 0; i < UIPanels.Length; i++)
        {
            UIPanels[i].SetActive(false);
        }
    }

    public void OpenUIPanel(int i)
    {
        CloseAlUIPanels();
        UIPanels[i].SetActive(true);
    }

    public void StartGame()
    {
        OpenUIPanel(2);
        ResetEachGame();
        ResetEachRound();
        gameStart = true;

    }

    public void QuitGame()
    {
        OpenUIPanel(0);
        ResetEachGame();
        ResetEachRound();
        if (Time.timeScale == 0)
            Time.timeScale = 1;
    }

    public void RestartGame()
    {
        OpenUIPanel(2);
        ResetEachRound();
        gameStart = true;
    }

    public void QuitApp()
    {
        Application.Quit();
    }

    public void PauseGame()
    {
        if(GameInProgress())
        {
            OpenUIPanel(3);
            gamePause = true;
            Time.timeScale = 0;
        }
        else
        {
            OpenUIPanel(2);
            gamePause = false;
            Time.timeScale = 1;
        }
    }

    private void ResetEachRound()
    {
        gameStart = false;
        gamePause = false;
        gameOver = false;
        NextBlockSpawnLocation = null;
        globalSpeed = 1;
        timeToSpawn = 0;
        _player.ResetPlayer();
        ResetPoolObjects();
    }

    private void ResetEachGame()
    {
        score[0] = 0;
        score[1] = 0;
        gameRound = 0;
    }

    public bool GameInProgress()
    {
        if (gameStart && !gamePause && !gameOver)
        {
            return true;
        }
        return false;
    }
}
