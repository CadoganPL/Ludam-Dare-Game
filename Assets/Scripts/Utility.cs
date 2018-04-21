using System.Collections;
using UnityEngine.UI;
using UnityEngine;

namespace Utility
{
    public static class UtilityClass
    {
        public static void GrayOutImage(Image image)
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, 99f);
        }

        public static void UnGrayOutImage(Image image)
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, 255f);
        }
    }
}
