using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float jumpForce;
    public float jumpVel;
    [SerializeField]
    private float slideTime;
    [SerializeField]
    private float jumpTime;
    public LayerMask groundLayer;
    private Vector2 pos_Start;
    private Rigidbody2D rig;
    private Transform _trans;
    private BoxCollider2D _col;
    [SerializeField]
    private bool isJumping;
    [SerializeField]
    private bool isSliding;
    public Transform _bodyTrans;
    [SerializeField]
    private Animator anim;
    //placeholder values before art
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
        anim.SetTrigger("idle");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        if (!GameManager.instance.GameInProgress())
            return;
        if (isSliding)
        {
            anim.SetTrigger("slide");
            return;
        }
        RaycastHit2D hit = Physics2D.Raycast(_trans.position, Vector2.down,0.4f,groundLayer);
        if(hit.collider!=null)
        {
            Debug.Log("hiitttig");
            anim.SetBool("run", true);
            isJumping = false;
            if (!isJumping)
            {
                if (Input.GetKeyDown(KeyCode.W))
                {
                    anim.SetTrigger("jumpUp");
                    anim.SetBool("run", false);
                    StartCoroutine(IResetJump(jumpTime));
                    rig.velocity += jumpForce * Vector2.up;
                    isJumping = true;
                }
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    if (!isSliding)
                    {
                        Debug.Log("slide");
                        anim.SetBool("run", false);
                        StartCoroutine(IResetSlide(slideTime));
                        _col.offset = colliderValues[2];
                        _col.size = colliderValues[3];
                        isSliding = true;
                    }
                }
                if (Input.GetKeyUp(KeyCode.Space))
                {
                    if (isSliding)
                    {
                        StopCoroutine(IResetSlide(slideTime));
                        isSliding = false;
                        anim.SetBool("run",true);
                    }

                }
            }        
        }
        if(isJumping)
        {
            if (Input.GetKey(KeyCode.W))
            {
                rig.velocity += jumpVel * Vector2.up;
            }
            if (Input.GetKeyUp(KeyCode.W))
            {
                StopCoroutine(IResetJump(jumpTime));
                isJumping = false;
                anim.SetTrigger("jumpDown");
            }
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Obstacle"))
        {
            GameManager.instance.gameOver = true;
            anim.SetTrigger("dead");
        }
    }

    public void ResetPlayer()
    {
        rig.velocity = Vector2.zero;
        _trans.position = pos_Start;
        isJumping = false;
        anim.SetTrigger("idle");
    }

    IEnumerator IResetSlide(float delay)
    {
        yield return new WaitForSeconds(delay);
        isSliding = false;
        _col.offset = colliderValues[0];
        _col.size = colliderValues[1];
        anim.SetBool("run", true);

    }

    IEnumerator IResetJump(float delay)
    {
        yield return new WaitForSeconds(delay);
        isJumping = false;
        anim.SetTrigger("jumpDown");
    }
}
