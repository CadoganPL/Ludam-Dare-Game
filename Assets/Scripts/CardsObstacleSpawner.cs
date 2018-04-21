using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CardsObstacleSpawner : MonoBehaviour {

    [SerializeField]
    GameObject lowObstacle;
    [SerializeField]
    GameObject mediumObstacle;
    [SerializeField]
    GameObject highObstacle;

    private int i = 0;
    public enum obstacleType
    {
        low,medium,high
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame

    public void SpawnObstacle(obstacleType type)
    {
        GameObject[] obstacleArray = GameObject.FindGameObjectsWithTag("Obstacle");
        obstacleArray = obstacleArray.OrderBy(x => Mathf.Abs(this.transform.position.x - x.transform.position.x)).ToArray();
        switch(type)
        {
            case obstacleType.low:
                Instantiate(GameObject.Find("GameManager").GetComponent<GameManager>().prefab_Block, GetComponent<GameManager>().points_SpawnLocations[0], Quaternion.identity);//.name ="Block_Card_" + i;
                break;
            case obstacleType.medium:
                Instantiate(GameObject.Find("GameManager").GetComponent<GameManager>().prefab_Block, GetComponent<GameManager>().points_SpawnLocations[1], Quaternion.identity);//.name = "Block_Card_" + i;
                break;
            case obstacleType.high:
                Instantiate(GameObject.Find("GameManager").GetComponent<GameManager>().prefab_Block, GetComponent<GameManager>().points_SpawnLocations[2], Quaternion.identity);//.name = "Block_Card_" + i;
                break;
            default:
                break;
        }
    }

    
}
