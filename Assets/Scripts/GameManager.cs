using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager instance = null;
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
    public int score;
    public bool gameStart, gamePause, gameOver;

    /// <summary>
    /// 0 : Bottom, 1 : Middle, 2 : Top, null : Not set(Give blocks Randomly)
    /// </summary>
    public int? NextBlockSpawnLocation = null;

    //UI
    public GameObject[] UIPanels;

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
        ResetSceneVariables();
        PoolBlocks();
    }

    // Update is called once per frame
    void Update()
    {
        UIUpdate();
        Spawner();
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
        Vector2 position = (blocksOnScreen.Sort(x => Mathf.Abs(x.transform.position - _player.transform.position))[0].position.x + blocksOnScreen.Sort(x => distanceToPlayer)[1].position.x) / 2;

        Instantiate(prefab_Block, (Vector3)points_SpawnLocations[(int)NextBlockSpawnLocation], Quaternion.identity);
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
        ResetSceneVariables();
        gameStart = true;
    }

    public void QuitGame()
    {
        CloseAlUIPanels();
    }

    public void RestartGame()
    {
        StartGame();
    }

    private void ResetSceneVariables()
    {
        gameStart = false;
        gamePause = false;
        gameOver = false;
        NextBlockSpawnLocation = null;
        score = 0;
        globalSpeed = 1;
        timeToSpawn = 0;
        _player.ResetPlayer();
        ResetPoolObjects();
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
