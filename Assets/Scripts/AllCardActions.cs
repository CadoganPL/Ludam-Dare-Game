using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class AllCardActions : MonoBehaviour
{
    public Image FlashBang;
    public float TimeBetweenFlashbangFade;

    public void Flashbang()
    {
        StartCoroutine(FlashBangScreen());
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

        yield return new WaitForSeconds(TimeBetweenFlashbangFade);

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
