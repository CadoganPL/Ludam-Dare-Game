using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float jumpForce;
    [SerializeField]
    private float slideTime;
    public LayerMask groundLayer;
    private Vector2 pos_Start;
    private Rigidbody2D rig;
    private Transform _trans;
    private CircleCollider2D _col;
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
        _col = GetComponent<CircleCollider2D>();
    }

    // Use this for initialization
    void Start()
    {
        pos_Start = _trans.position;
        anim.SetTrigger("idle");
    }

    // Update is called once per frame
<<<<<<< HEAD
    void Update()
    {

    }
=======
>>>>>>> pr/17


    void Update()
    {
        if (!GameManager.instance.GameInProgress())
        {
            return;
        }

        if (GameManager.instance.WhichGameMode() == GameMode.Multiplayer && GameManager.instance.WhichRole() == Role.Killer)
        {
            return;
        }

        RaycastHit2D hit = Physics2D.Raycast(_trans.position, Vector2.down, 0.4f, groundLayer);
        if (hit.collider != null)
        {
            if (!isSliding)
            {
                anim.SetBool("run", true);
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }
            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                if (!isSliding)
                {
<<<<<<< HEAD
                    StartSliding();
=======
                    anim.SetBool("run", false);
                    anim.SetTrigger("slide");
                    StartCoroutine(IResetSlide(slideTime));
                    _col.offset = colliderValues[1];
                    _col.radius = .25f;
                    isSliding = true;
>>>>>>> pr/17
                }
            }
            if (Input.GetKeyUp(KeyCode.LeftControl))
            {
                if (isSliding)
                {
<<<<<<< HEAD
                    StopSliding();
=======
                    StopCoroutine(IResetSlide(slideTime));
                    isSliding = false;
                    _col.offset = colliderValues[0];
                    _col.radius = .3f;
                    anim.SetTrigger("slideend");
>>>>>>> pr/17
                }

            }
        }

    }

    public void StopSliding()
    {
        StopCoroutine(IResetSlide(slideTime));
        isSliding = false;
        _col.offset = colliderValues[0];
        _col.radius = .35f;
        anim.SetTrigger("slideend");

        if (GameManager.instance.WhichGameMode() == GameMode.Multiplayer && GameManager.instance.WhichRole() == Role.Runner)
        {
            FindObjectOfType<Client>().Send("CSLIDESTOP|");
        }
    }

    public void StartSliding()
    {
        anim.SetBool("run", false);
        anim.SetTrigger("slide");
        StartCoroutine(IResetSlide(slideTime));
        _col.offset = colliderValues[1];
        _col.radius = .06f;
        isSliding = true;

        if (GameManager.instance.WhichGameMode() == GameMode.Multiplayer && GameManager.instance.WhichRole() == Role.Runner)
        {
            FindObjectOfType<Client>().Send("CSLIDESTART|");
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Obstacle" || other.gameObject.tag == "CardObstacle")
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

    public void Jump()
    {
        anim.SetBool("run", false);
        anim.SetTrigger("jumpUp");
        rig.AddForce(jumpForce * Vector2.up);

        if (GameManager.instance.WhichGameMode() == GameMode.Multiplayer && GameManager.instance.WhichRole() == Role.Runner)
        {
            FindObjectOfType<Client>().Send("CJUMP|");
        }
    }

    IEnumerator IResetSlide(float delay)
    {
        yield return new WaitForSeconds(delay);
        isSliding = false;
        _col.offset = colliderValues[0];
        _col.radius = .3f;
        anim.SetTrigger("slideend");

    }
}
