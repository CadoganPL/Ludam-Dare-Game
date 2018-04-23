using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class CardsObstacleSpawner : MonoBehaviour
{

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

        switch (type)
        {
            case obstacleType.low:
                Instantiate(lowObstacle, new Vector2(xPosition, GameObject.Find("GameManager").GetComponent<GameManager>().points_SpawnLocations[0].y), Quaternion.identity).tag="CardObstacle";//.name ="Block_Card_" i; 
                break;
            case obstacleType.medium:
                Instantiate(mediumObstacle, new Vector2(xPosition, GameObject.Find("GameManager").GetComponent<GameManager>().points_SpawnLocations[1].y), Quaternion.identity).tag = "CardObstacle";//.name = "Block_Card_" i; 
                break;
            case obstacleType.high:
                Instantiate(highObstacle, new Vector2(xPosition, GameObject.Find("GameManager").GetComponent<GameManager>().points_SpawnLocations[2].y), Quaternion.identity).tag = "CardObstacle";//.name = "Block_Card_" i; 
                break;
            default:
                break;
        }
    }


}
