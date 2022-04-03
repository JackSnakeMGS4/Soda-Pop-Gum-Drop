using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Variables")]
    [SerializeField] private float move_spd = 5.0f;
    [SerializeField] private float run_mod = 2.0f;
    [SerializeField] private float linear_drag = 1.0f;
    [SerializeField] private float running_linear_drag = 3.0f;

    [Header("Jumping Variables")]
    [SerializeField] private float jump_height = 2.0f;
    [SerializeField] private float running_jump_height = 4.0f;
    [SerializeField] private float cancel_rate = 100;
    [SerializeField] private float gravity_scale = 1.0f;
    [SerializeField] private float falling_gravity_scale = 40.0f;
    [SerializeField] private float button_timer = 0.3f;

    private float jump_force = 10.0f;
    private float ground_distance = 0.3f;
    private float jump_time = 0.0f;

    private bool is_grounded = false;
    private bool is_jumping = false;
    private bool is_jump_cancelled = false;
    private bool is_running = false;

    private Rigidbody2D rb2D;
    [SerializeField] private Transform ground_check;
    [SerializeField] private LayerMask ground_mask;
    private Vector2 vel;

    // Start is called before the first frame update
    void Start()
    {
        rb2D = gameObject.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Jump();
    }

    private void Jump()
    {
        is_grounded = Physics2D.OverlapCircle(ground_check.position, ground_distance, ground_mask);
        //Debug.Log("Grounded " + is_grounded);

        if (Input.GetButtonDown("Jump") && is_grounded)
        {
            if(is_running && (rb2D.velocity.x > 0 || rb2D.velocity.x < 0))
            {
                jump_force = Mathf.Sqrt(running_jump_height * -2 * (Physics2D.gravity.y * rb2D.gravityScale));
            }
            else
            {
                jump_force = Mathf.Sqrt(jump_height * -2 * (Physics2D.gravity.y * rb2D.gravityScale));
            }

            rb2D.AddForce(new Vector2(0, jump_force), ForceMode2D.Impulse);
            is_jumping = true;
            is_jump_cancelled = false;
            jump_time = 0f;
        }

        ////Change jump height based on input
        if (is_jumping)
        {
            //Debug.Log("Boosting Jump");
            //rb2D.velocity = new Vector2(rb2D.velocity.x, jump_force);
            jump_time += Time.deltaTime;
            if (Input.GetButtonUp("Jump"))
            {
                is_jump_cancelled = true;
            }
            if(jump_time > button_timer)
            {
                is_jumping = false;
            }
        }
        //if(Input.GetButtonUp("Jump") || jump_time > button_timer)
        //{
        //    //Debug.Log("Not Boosting Jump");
        //    is_jumping = false;
        //}

        //Apply different gravity to rise and fall
        if (rb2D.velocity.y >= 0)
        {
            rb2D.gravityScale = gravity_scale;
        }
        else if (rb2D.velocity.y < 0)
        {
            rb2D.gravityScale = falling_gravity_scale;
        }
    }

    private void FixedUpdate()
    {
        Move();
        CancelJump();
    }

    //Polish running jump???
    private void Move()
    {
        float horizontal_move = Input.GetAxisRaw("Horizontal") * move_spd;
        if (Input.GetButton("Run"))
        {
            horizontal_move *= run_mod;
            is_running = true;
        }
        else
        {
            is_running = false;
        }
        Vector2 movement = new Vector2(horizontal_move, rb2D.velocity.y);

        if(!is_running)
        {
            rb2D.drag = linear_drag;
        }
        else if(is_running)
        {
            rb2D.drag = running_linear_drag;
        }

        rb2D.velocity = movement;
        //Debug.Log(horizontal);
        //Debug.Log("Force: " + force.x);
        //Debug.Log("Velocity: " + rb2D.velocity.x);
    }

    private void CancelJump()
    {
        if (is_jump_cancelled && is_jumping && rb2D.velocity.y > 0)
        {
            rb2D.AddForce(Vector2.down * cancel_rate);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(ground_check.position, ground_distance);
    }
}
