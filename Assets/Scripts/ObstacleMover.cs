using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleMover : MonoBehaviour {

    public float speed;
    private Rigidbody2D rig;

    void Awake()
    {
        rig = GetComponent<Rigidbody2D>();
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void FixedUpdate()
    {
        if(GameManager.instance.gameOver)
        {
            if(rig.velocity!=Vector2.zero)
            {
                rig.velocity = Vector2.zero;
            }
        }
        if(GameManager.instance.GameInProgress())
        {
            rig.velocity = Vector2.left * speed * GameManager.instance.globalSpeed;
        }
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Disabler"))
        {
            gameObject.SetActive(false);
        }
    }
}
