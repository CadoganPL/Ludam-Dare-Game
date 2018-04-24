using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;

public class GameManager : MonoBehaviour
{

    public static GameManager instance = null;
    GameMode gameMode = 0;
    Role role;
    int gameRound = 0;
    //Obstacle
    public GameObject prefab_block;
    public GameObject prefab_obstacle_low;
    public GameObject prefab_obstacle_medium;
    public GameObject prefab_obstacle_high;
    private List<GameObject> blockPool = new List<GameObject>();
    private List<GameObject> blocksOnScreen = new List<GameObject>();
    public float spawnTimer;
    private float timeToSpawn;
    [SerializeField]
    private float AISpawnTime;
    private float AISpawnTimer;
    private Action[] AICards = new Action[3];
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
    private ButtonsManager btnManager;

    //Called before Start
    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        _player = GameObject.Find("Player").GetComponent<PlayerController>();
        btnManager = GetComponent<ButtonsManager>();

    }

    // Use this for initialization
    void Start()
    {
        GetComponent<BGScroll>().fencebg_StartPos = GetComponent<BGScroll>().fenceBG.position;
        ResetEachRound();
        PoolBlocks();
        AICards[0] = FindObjectOfType<AllCardActions>().Flashbang;
        AICards[1] = FindObjectOfType<AllCardActions>().RunnerSpeedUp;
        AICards[2] = FindObjectOfType<AllCardActions>().SpawnLowObstacle;

    }

    // Update is called once per frame
    void Update()
    {
        if(globalSpeed < 1f) //"fix" for speedUp bug. xD
        {
            globalSpeed = 1.0f;
        }
        UIUpdate();
        Spawner();
        if (GameInProgress())
        {
            score[gameRound] += Time.deltaTime;
        }
    }

    public void MakeRoleKiller()
    {
        role = Role.Killer;
    }

    public void MakeRoleRunner()
    {
        role = Role.Runner;
    }

    public Role WhichRole()
    {
        return role;
    }


    //Spawners
    void PoolBlocks()
    {
        int i = 0;
        for (i = 0; i < 15; i++)
        {
            GameObject go = Instantiate(prefab_block, points_SpawnLocations[0], Quaternion.identity) as GameObject;
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
                        int rand = UnityEngine.Random.Range(0, 3);
                        blockPool[i].transform.position = points_SpawnLocations[rand];
                        if (blockPool[i].transform.childCount == 0)
                        {
                            switch (rand)
                            {
                                case 0:
                                    Instantiate(prefab_obstacle_low, Vector2.zero, Quaternion.identity).transform.parent = blockPool[i].transform;
                                    break;
                                case 1:
                                    Instantiate(prefab_obstacle_medium, Vector2.zero, Quaternion.identity).transform.parent = blockPool[i].transform;
                                    break;
                                case 2:
                                    Instantiate(prefab_obstacle_high, Vector2.zero, Quaternion.identity).transform.parent = blockPool[i].transform;
                                    break;
                                default:
                                    break;
                            }
                            blockPool[i].transform.GetChild(0).transform.localPosition = Vector2.zero;
                            blockPool[i].transform.GetChild(0).gameObject.tag = blockPool[i].gameObject.tag;
                        }
                        blocksOnScreen.Add(blockPool[i]);

                        //if (NextBlockSpawnLocation != null)
                        //{
                        //    SpawnCardObstacle();
                        //    NextBlockSpawnLocation = null;
                        //}

                        blockPool[i].SetActive(true);
                        break;
                    }
                }
                timeToSpawn = 0;
            }
        }
        if (gameMode == GameMode.AI)
        {
            AISpawnTimer += Time.deltaTime;
            if (AISpawnTimer >= AISpawnTime)
            {
                int rand = UnityEngine.Random.Range(0, 3);
                AICards[rand]();
                AISpawnTimer = 0;
            }
        }

    }

<<<<<<< HEAD
    private void SpawnCardObstacle()
    {
        Vector2 position = new Vector2(10.5f, -0.55f);
        float yPos = points_SpawnLocations[(int)NextBlockSpawnLocation].y;
        position.y = yPos;
        if (blocksOnScreen.Count > 1)
        {
            GameObject[] blockArray = blocksOnScreen.ToArray();
            blockArray = blockArray.OrderBy(x => Mathf.Abs(x.transform.position.x - _player.transform.position.x)).ToArray();
            position.x = (blockArray[0].transform.position.x + blockArray[1].transform.position.x) / 2;
        }
        else if (blocksOnScreen.Count == 1)
        {
            GameObject[] blockArray = blocksOnScreen.ToArray();
            if (Vector2.Distance(blockArray[0].transform.position, position) < 3)
            {
                position.x += 3;
            }
        }
        int i = 0;
        for (i = 0; i < blockPool.Count; i++)
        {
            if (!blockPool[i].activeSelf)
            {
                blockPool[i].transform.position = position;
                break;
            }
        }
    }
=======
    //private void SpawnCardObstacle()
    //{
    //    Vector2 position = new Vector2(10.5f, -0.55f);
    //    float yPos = points_SpawnLocations[(int)NextBlockSpawnLocation].y;
    //    position.y = yPos;
    //    if (blocksOnScreen.Count>1)
    //    {
    //        GameObject[] blockArray = blocksOnScreen.ToArray();
    //        blockArray = blockArray.OrderBy(x => Mathf.Abs(x.transform.position.x - _player.transform.position.x)).ToArray();
    //        position.x = (blockArray[0].transform.position.x + blockArray[1].transform.position.x) / 2;
    //    }
    //    else if (blocksOnScreen.Count == 1)
    //    {
    //        GameObject[] blockArray = blocksOnScreen.ToArray();
    //        if(Vector2.Distance(blockArray[0].transform.position,position)<3)
    //        {
    //            position.x += 3;
    //        }
    //    }
    //    for (int i = 0; i < blockPool.Count; i++)
    //    {
    //        if (!blockPool[i].activeSelf)
    //        {
    //            blockPool[i].transform.position = position;
    //            break;
    //        }
    //    }
    //}
>>>>>>> pr/17

    private void RemovedObstacleFromScreen(GameObject Obstacle)
    {
        blocksOnScreen.Remove(Obstacle);
    }

    void ResetPoolObjects()
    {
        foreach (var item in blockPool)
        {
            item.SetActive(false);
            if(item.transform.childCount != 0)
            {
                Destroy(item.transform.GetChild(0).gameObject);
            }
        }
        foreach (var item in GameObject.FindGameObjectsWithTag("CardObstacle"))
        {
            Destroy(item);
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
            ScoreText.text = Mathf.Round(score[gameRound]).ToString() ;
        }
    }
    //comment
    //0-local , 1- multiplayer
    public void SelectMode(int a)
    {
        GetComponent<MenuUIManager>().HideMenus();
        gameMode = (GameMode)a;
        StartGame();
    }

    public GameMode WhichGameMode()
    {
        return gameMode;
    }

    public void WhenServerNotAvailable()
    {
        //Show Text
    }

    void SetGameOverScreen()
    {
        if (gameMode == GameMode.AI)
        {
            infoText.text = "Player 1 survived " + Mathf.Round(score[gameRound]) + " seconds";
            switchButton.gameObject.SetActive(false);
            restartButton.gameObject.SetActive(true);
            ResetEachGame();
        }
        else
        {
            if (gameRound == 0)
            {
                infoText.text = "Player 1 survived " + Mathf.Round(score[gameRound]) + " seconds";
                switchButton.gameObject.SetActive(true);
                restartButton.gameObject.SetActive(false);
                gameRound++;
            }
            else
            {
                if (score[0] > score[1])
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
        if (gameMode == GameMode.AI)
        {
            btnManager.CardOne.MyButton.interactable = false;
            btnManager.CardTwo.MyButton.interactable = false;
            btnManager.CardThree.MyButton.interactable = false;

        }
        else
        {
            btnManager.CardOne.MyButton.interactable = true;
            btnManager.CardTwo.MyButton.interactable = true;
            btnManager.CardThree.MyButton.interactable = true;
        }
        MenuUIManager menu = GetComponent<MenuUIManager>();
        if (menu.source.clip != menu.gameBGM)
        {
            menu.source.clip = menu.gameBGM;
            menu.source.Play();
        }
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
        if (GameInProgress())
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
        GetComponent<BGScroll>().ResetBG();
    }

    private void ResetEachGame()
    {
        globalSpeed = 1;
        score[0] = 0;
        score[1] = 0;
        gameRound = 0;
        btnManager.SetCards();
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

//modes
public enum GameMode
{
    Local,
    Multiplayer,
    AI
};

//Roles
public enum Role
{
    Killer,
    Runner
};
