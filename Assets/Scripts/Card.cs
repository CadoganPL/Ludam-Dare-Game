using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public int CardNumber;
    public int RelodTime;
    public Action MyWork;
    public Button MyButton;

    public Card(int RelodTime)
    {
        this.RelodTime = RelodTime;
    }

    public Card(Action MyWork)
    {
        this.MyWork = MyWork;
    }

    public Card(int RelodTime, Action MyWork)
    {
        this.RelodTime = RelodTime;
        this.MyWork = MyWork;
    }
}
