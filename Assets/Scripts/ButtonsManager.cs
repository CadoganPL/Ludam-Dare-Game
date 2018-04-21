using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using Utility;

public class ButtonsManager : MonoBehaviour
{
    public float CardUnactiveTime = 30.00f;

    private float cardOneTimer, cardTwoTimer, cardThreeTimer;
    private bool cardOneTimerSet, cardTwoTimerSet, cardThreeTimerSet;
    private Image cardOneImage, cardTwoImage, cardThreeImage;

    public bool CardOneTimerSet { get { return cardOneTimerSet; } }
    public bool CardTwoTimerSet { get { return cardTwoTimerSet; } }
    public bool CardThreeTimerSet { get { return cardThreeTimerSet; } }

    private void Update()
    {
        if (cardOneTimerSet)
        {
            cardOneTimer -= Time.deltaTime;

            if(cardOneTimer <= 0)
            {
                cardOneTimer = 0;
                
                cardOneTimerSet = false;
                print("re-enabling Card One");
            }
        }

        if (cardTwoTimerSet)
        {
            cardTwoTimer -= Time.deltaTime;

            if (cardTwoTimer <= 0)
            {       
                cardTwoTimer = 0;

                cardTwoTimerSet = false;
                print("re-enabling Card Two");
            }
        }

        if (cardThreeTimerSet)
        {
            cardThreeTimer -= Time.deltaTime;

            if (cardThreeTimer <= 0)
            {       
                cardThreeTimer = 0;
                    
                cardThreeTimerSet = false;
                print("re-enabling Card Three");
            }
        }
    }

    public void CardOneButton(Image CardImage)
    {
        if(cardOneImage == null)
        {
            cardOneImage = CardImage;
        }

        print("Seeing what function I do by GameManager");

        print("Using Card One, adding timer");

        DisableCardToUse(CardImage, 1);
    }

    public void CardTwoButton(Image CardImage)
    {
        if (cardTwoImage == null)
        {
            cardTwoImage = CardImage;
        }

        print("Seeing what function I do by GameManager");

        print("Using Card Two, adding timer");

        DisableCardToUse(CardImage, 2);
    }

    public void CardThreeButton(Image CardImage)
    {
        if (cardTwoImage == null)
        {
            cardTwoImage = CardImage;
        }

        print("Seeing what function I do by GameManager");

        print("Using Card Three, adding timer");

        DisableCardToUse(CardImage, 3);
    }

    private void DisableCardToUse(Image CardImage,int WhichCard)
    {
        UtilityClass.GrayOutImage(CardImage);
        
        switch (WhichCard)
        {
            case 1:
                cardOneTimer = CardUnactiveTime;
                cardOneTimerSet = true;
                break;

            case 2:
                cardTwoTimer = CardUnactiveTime;
                cardTwoTimerSet = true;
                break;

            case 3:
                cardThreeTimer = CardUnactiveTime;
                cardThreeTimerSet = true;
                break;

            default:
                print("Wrong Card Number");
                break;
        }
    }


    private void ReEnableCardToUse(Image CardImage, int WhichCard)
    {
        UtilityClass.UnGrayOutImage(CardImage);

        switch (WhichCard)
        {
            case 1:
                cardOneTimerSet = false;
                break;

            case 2:
                cardTwoTimerSet = false;
                break;

            case 3:
                cardThreeTimerSet = false;
                break;

            default:
                print("Wrong Card Number");
                break;
        }
    }
}
