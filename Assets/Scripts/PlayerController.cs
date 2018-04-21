using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float MoveSpeed;
    public float JumpVelocity;

    private float horizontalAxis;
    private Rigidbody2D rig;

    // Use this for initialization
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontalAxis = Input.GetAxisRaw("Horizontal");

        if (horizontalAxis != 0)
        {
            rig.velocity = new Vector2(MoveSpeed * horizontalAxis, rig.velocity.y);
        }

        if (Input.GetAxisRaw("Jump") == 1f)
        {
            rig.velocity = new Vector2(rig.velocity.x, JumpVelocity);
        }
    }
}
