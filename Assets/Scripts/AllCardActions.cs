using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using System;

public class AllCardActions : MonoBehaviour
{
    public Image FlashBang;

    public void Flashbang()
    {
        StartCoroutine(FlashBangScreen());
    }

    public void RunnerSpeedUp()
    {
        StartCoroutine(SpeedUp());
    }

    public void SpawnLowObstacle()
    {
        GameObject.Find("CardObstaclesSpawner").GetComponent<CardsObstacleSpawner>().SpawnObstacle(CardsObstacleSpawner.obstacleType.low);
    }
    public void SpawnMidObstacle()
    {
        GameObject.Find("CardObstaclesSpawner").GetComponent<CardsObstacleSpawner>().SpawnObstacle(CardsObstacleSpawner.obstacleType.medium);
    }

    public void SpawnHighObstacle()
    {
        GameObject.Find("CardObstaclesSpawner").GetComponent<CardsObstacleSpawner>().SpawnObstacle(CardsObstacleSpawner.obstacleType.high);
    }



    private IEnumerator SpeedUp()
    {
        GetComponent<GameManager>().globalSpeed *= 2;
        yield return new WaitForSeconds(3f);
        GetComponent<GameManager>().globalSpeed /= 2;
    }

    private IEnumerator FlashBangScreen()
    {
        FlashBang.enabled = true;

        for (float i = 0; i <= 1; i += 0.08f)
        {
            yield return new WaitForSeconds(0.08f * Time.deltaTime);

            Color col = FlashBang.color;
            col.a = i;

            FlashBang.color = col;
        }

        yield return new WaitForSeconds(1f);

        for (float i = 1; i >= 0; i -= 0.07f)
        {
            print(i);
            yield return new WaitForSeconds(0.08f * Time.deltaTime);

            Color col = FlashBang.color;
            col.a = i;

            FlashBang.color = col;
        }

        FlashBang.enabled = false;


    }
}
