using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float jumpForce;
    public float jumpVel;
    public LayerMask groundLayer;
    private Vector2 pos_Start;
    private Rigidbody2D rig;
    private Transform _trans;
    private BoxCollider2D _col;
    private bool isJumping;
    public Transform _bodyTrans;
    //placeholder values before art
    public Vector3[] playerTransforms;
    public Vector2[] colliderValues;

    void Awake()
    {
        rig = GetComponent<Rigidbody2D>();
        _trans = GetComponent<Transform>();
        _col = GetComponent<BoxCollider2D>();
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
            isJumping = false;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                rig.velocity += jumpForce * Vector2.up;
                isJumping = true;
            }
            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                _col.offset = colliderValues[2];
                _col.size = colliderValues[3];
                _bodyTrans.localPosition = playerTransforms[2];
                _bodyTrans.localScale = playerTransforms[3];

            }
            if (Input.GetKeyUp(KeyCode.LeftControl))
            {
                _col.offset = colliderValues[0];
                _col.size = colliderValues[1];
                _bodyTrans.localPosition = playerTransforms[0];
                _bodyTrans.localScale = playerTransforms[1];
            }
        }
        if(isJumping)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                rig.velocity += jumpVel * Vector2.up;
            }
            if (Input.GetKeyUp(KeyCode.Space))
            {
                isJumping = false;
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
        rig.velocity = Vector2.zero;
        _trans.position = pos_Start;
        isJumping = false;
    }
}
