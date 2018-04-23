using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using System;

public class AllCardActions : MonoBehaviour
{
    public Image FlashBang;
    public float TimeBetweenFlashbangFade;

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
        FindObjectOfType<CardsObstacleSpawner>().SpawnObstacle(CardsObstacleSpawner.obstacleType.low);
    }
    public void SpawnMidObstacle()
    {
        FindObjectOfType<CardsObstacleSpawner>().SpawnObstacle(CardsObstacleSpawner.obstacleType.medium);
    }

    public void SpawnHighObstacle()
    {
        FindObjectOfType< CardsObstacleSpawner >().SpawnObstacle(CardsObstacleSpawner.obstacleType.high);
    }

    
    public Action GetRandomAction()
    {
        int rand = UnityEngine.Random.Range(0, 5);
        switch(rand)
        {
            case 0:
                return Flashbang;
            case 1:
                return RunnerSpeedUp;
            case 2:
                return SpawnLowObstacle;
            case 3:
                return SpawnMidObstacle;
            case 4:
                return SpawnHighObstacle;
            default:
                return null;

        }
    }

    private IEnumerator SpeedUp()
    {
        GetComponent<GameManager>().globalSpeed *= 2;
        yield return new WaitForSeconds(3f);
        GetComponent<GameManager>().globalSpeed /= 2;
    }

    private IEnumerator FlashBangScreen()
    {
        FlashBang.gameObject.SetActive(true);

        for (float i = 0; i <= 1; i += 0.08f)
        {
            yield return new WaitForSeconds(0.08f * Time.deltaTime);

            Color col = FlashBang.color;
            col.a = i;

            FlashBang.color = col;
        }

        yield return new WaitForSeconds(TimeBetweenFlashbangFade);

        for (float i = 1; i >= 0; i -= 0.07f)
        {
            yield return new WaitForSeconds(0.08f * Time.deltaTime);

            Color col = FlashBang.color;
            col.a = i;

            FlashBang.color = col;
        }

        FlashBang.gameObject.SetActive(false);


    }
}
