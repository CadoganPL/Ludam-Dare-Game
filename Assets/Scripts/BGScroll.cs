using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGScroll : MonoBehaviour {

    [SerializeField]
    private float speed;
    [SerializeField]
    private float offsetspeed;
    [SerializeField]
    private GameObject mountain;
    [SerializeField]
    private Transform fenceBG;
    private Vector2 fencebg_StartPos;


    // Use this for initialization
    void Start () {
        fencebg_StartPos = fenceBG.position;

    }

    // Update is called once per frame
    void Update () {
        if(GameManager.instance.GameInProgress())
        {
            Vector2 offset = new Vector2(Time.deltaTime * offsetspeed * GameManager.instance.globalSpeed, 0);
            mountain.GetComponent<Renderer>().material.mainTextureOffset += offset;

            fenceBG.Translate(Vector2.left * Time.deltaTime * speed * GameManager.instance.globalSpeed);

        }
    }

    public void ResetBG()
    {

        fenceBG.position = fencebg_StartPos;
        mountain.GetComponent<Renderer>().material.mainTextureOffset = Vector2.zero;
    }
}
