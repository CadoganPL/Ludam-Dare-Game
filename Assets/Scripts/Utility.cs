using System.Collections;
using UnityEngine.UI;
using UnityEngine;

namespace Utility
{
    public static class UtilityClass
    {
        public static void UnableButton(Button button)
        {
            button.interactable = false;
        }

        public static void EnableButton(Button button)
        {
            button.interactable = true;
        }
    }
}
