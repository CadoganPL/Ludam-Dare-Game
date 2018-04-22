using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CardsObstacleSpawner : MonoBehaviour
{

    [SerializeField]
    GameObject lowObstacle;
    [SerializeField]
    GameObject mediumObstacle;
    [SerializeField]
    GameObject highObstacle;

    private int i = 0;
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
        obstacleArray = obstacleArray.OrderBy(x => Mathf.Abs(this.transform.position.x - x.transform.position.x)).ToArray();
        switch (type)
        {
            case obstacleType.low:
                FindObjectOfType<GameManager>().NextBlockSpawnLocation = 0;
                break;
            case obstacleType.medium:
                FindObjectOfType<GameManager>().NextBlockSpawnLocation = 1;
                break;
            case obstacleType.high:
                FindObjectOfType<GameManager>().NextBlockSpawnLocation = 2;
                break;
            default:
                break;
        }
    }


}
