using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class CardsObstacleSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject prefab_block;
    [SerializeField]
    private GameObject lowObstacle;
    [SerializeField]
    private GameObject mediumObstacle;
    [SerializeField]
    private GameObject highObstacle;

    public enum obstacleType
    {
        low,
        medium,
        high
    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame

    public void SpawnObstacle(obstacleType type)
    {
        GameObject[] obstacleArray = GameObject.FindGameObjectsWithTag("Obstacle");
        for(int i = 0; i < obstacleArray.Length;i++) //nulling reference to obstacles which are behind spawner
        {
            if(obstacleArray[i].transform.position.x < this.transform.position.x)
            {
                obstacleArray[i] = null;
            }
        }
        obstacleArray =  obstacleArray.Where(c => c != null).ToArray(); // deleting null from array
        obstacleArray = obstacleArray.OrderBy(x => Mathf.Abs(this.transform.position.x - x.transform.position.x)).ToArray();
        float xPosition = 0;
        try
        {
            xPosition = obstacleArray[0].transform.position.x + obstacleArray[1].transform.position.x;
            xPosition /= 2;
        }
        catch (IndexOutOfRangeException e)
        {
            xPosition = obstacleArray[0].transform.position.x * 1.5f;
        }
        GameObject tmpGo = Instantiate(prefab_block,Vector2.zero, Quaternion.identity);
        GameObject tmpGoChild;
        switch (type)
        {
            case obstacleType.low:
                tmpGo.transform.position = new Vector2(xPosition, GameObject.Find("GameManager").GetComponent<GameManager>().points_SpawnLocations[0].y);
                tmpGoChild = Instantiate(lowObstacle, Vector2.zero, Quaternion.identity);
                break;
            case obstacleType.medium:
                tmpGo.transform.position = new Vector2(xPosition, GameObject.Find("GameManager").GetComponent<GameManager>().points_SpawnLocations[1].y);
                tmpGoChild = Instantiate(mediumObstacle, Vector2.zero, Quaternion.identity);
                break;
            case obstacleType.high:
                tmpGo.transform.position = new Vector2(xPosition, GameObject.Find("GameManager").GetComponent<GameManager>().points_SpawnLocations[2].y);
                tmpGoChild = Instantiate(highObstacle, Vector2.zero, Quaternion.identity);
                break;
            default:
                tmpGoChild = Instantiate(prefab_block, Vector2.zero, Quaternion.identity);
                break;
        }
        tmpGo.tag = "CardObstacle";
        tmpGoChild.transform.parent = tmpGo.transform;
        tmpGo.transform.GetChild(0).transform.localPosition = Vector2.zero;
        tmpGo.transform.GetChild(0).gameObject.tag = tmpGo.gameObject.tag;
    }


}
