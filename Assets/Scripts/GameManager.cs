using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager instance = null;
    //Obstacle
    public Transform point_Spawner;
    public GameObject prefab_Block;
    private List<GameObject> blockPool = new List<GameObject>();
    public float spawnTimer;
    private float timeToSpawn;
    public float globalSpeed = 1f;
    //player
    private PlayerController _player;

    //States
    public int score;
    public bool gameStart, gamePause, gameOver;

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
	void Start () {
        ResetSceneVariables();
        PoolBlocks();
    }
	
	// Update is called once per frame
	void Update () {
        UIUpdate();
        Spawner();
    }


    //Spawners
    void PoolBlocks()
    {
        int i = 0;
        for (i = 0; i < 10; i++)
        {
            GameObject go = Instantiate(prefab_Block, point_Spawner.position, Quaternion.identity) as GameObject;
            go.name = "Block_" + i;
            go.SetActive(false);
            blockPool.Add(go);
        }
    }

    void Spawner()
    {
        if(GameInProgress())
        {
            timeToSpawn += Time.deltaTime;
            int i = 0;
            if (timeToSpawn >= spawnTimer)
            {
                for (i = 0; i < blockPool.Count; i++)
                {
                    if (!blockPool[i].activeSelf)
                    {
                        blockPool[i].transform.position = point_Spawner.position;
                        blockPool[i].SetActive(true);
                        break;
                    }
                }
                timeToSpawn = 0;
            }
        }
 
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
        if(gameOver)
        {
            if(!UIPanels[1].activeSelf)
            {
                OpenUIPanel(1);
                Time.timeScale = 0.0f;
            }
        }
    }
    
    public void CloseAlUIPanels()
    {
        int i = 0;
        for(i=0;i<UIPanels.Length;i++)
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
        score = 0;
        globalSpeed = 1;
        timeToSpawn = 0;
        _player.ResetPlayer();
        ResetPoolObjects();
        Time.timeScale = 1.0f;
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
