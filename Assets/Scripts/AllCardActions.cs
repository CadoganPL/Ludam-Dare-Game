using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class AllCardActions : MonoBehaviour
{
    public Image FlashBang;

    public void Flashbang()
    {
        StartCoroutine(FlashBangScreen());
    }

    private IEnumerator FlashBangScreen()
    {
        float i = 0.0f;

        for (i = 0; i <= 255; i += 0.1f)
        {
            yield return new WaitForSeconds(0.08f * Time.deltaTime);

            FlashBang.color = new Color(FlashBang.color.r, FlashBang.color.g, FlashBang.color.b, i);
        }

        yield return new WaitForSeconds(1f);

        for (i = 255; i <= 0; i -= 0.1f)
        {
            yield return new WaitForSeconds(0.08f * Time.deltaTime);

            FlashBang.color = new Color(FlashBang.color.r, FlashBang.color.g, FlashBang.color.b, i);
        }
    }
}
