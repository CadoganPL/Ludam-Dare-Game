using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float jumpForce;
    public LayerMask groundLayer;
    private Vector2 pos_Start;
    private Rigidbody2D rig;
    private Transform _trans;

    void Awake()
    {
        rig = GetComponent<Rigidbody2D>();
        _trans = GetComponent<Transform>();
    }

    // Use this for initialization
    void Start()
    {
        pos_Start = _trans.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        RaycastHit2D hit = Physics2D.Raycast(_trans.position, Vector2.down,0.6f,groundLayer);
        if(hit.collider!=null)
        {
            if (Input.GetAxisRaw("Jump") == 1f)
            {
                rig.AddForce(jumpForce * Vector2.up);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Obstacle"))
        {
            GameManager.instance.gameOver = true;
        }
    }

    public void ResetPlayer()
    {
        _trans.position = pos_Start;
    }
}
