using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

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
    private bool is_facing_right = true;
    private bool can_move = false;

    private Rigidbody2D rb2D;
    [SerializeField] private Transform ground_check;
    [SerializeField] private LayerMask ground_mask;
    [SerializeField] private Transform respawn_point;
    private Vector2 vel;
    private Animator animator;

    public bool _Can_Move
    {
        get { return can_move; }
        set { can_move = value; }
    }

    public Transform _Respawn_Point
    {
        get { return respawn_point; }
    }

    [EventRef] public string jump_event = "";
    [EventRef] public string death_event = "";

    // Start is called before the first frame update
    void Start()
    {
        rb2D = gameObject.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();
        can_move = true;
    }

    private void Update()
    {
        if (can_move)
        {
            Jump();
        }
    }

    private void Jump()
    {
        is_grounded = Physics2D.OverlapCircle(ground_check.position, ground_distance, ground_mask);
        //Debug.Log("Grounded " + is_grounded);

        if (Input.GetButtonDown("Jump") && is_grounded)
        {
            RuntimeManager.PlayOneShot(jump_event, transform.position);

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

            //animator.SetBool("has_jumped", true);
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

        animator.SetBool("has_jumped", !is_grounded);
    }

    private void FixedUpdate()
    {
        if(can_move)
        {
            Move();
            CancelJump();
        }
        else
        {
            rb2D.drag = 3f;
        }
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
        animator.SetFloat("speed", Mathf.Abs(horizontal_move));

        if (!is_running)
        {
            rb2D.drag = linear_drag;
        }
        else if(is_running)
        {
            rb2D.drag = running_linear_drag;
        }

        if (horizontal_move < 0 && is_facing_right)
        {
            Flip();
        }
        else if(horizontal_move > 0 && !is_facing_right)
        {
            Flip();
        }

        Vector2 movement = new Vector2(horizontal_move, rb2D.velocity.y);
        rb2D.velocity = movement;
        //Debug.Log(horizontal);
        //Debug.Log("Force: " + force.x);
        //Debug.Log("Velocity: " + rb2D.velocity.x);
    }

    private void Flip()
    {
        is_facing_right = !is_facing_right;
        transform.Rotate(Vector3.up * 180f);
    }

    private void CancelJump()
    {
        if (is_jump_cancelled && is_jumping && rb2D.velocity.y > 0)
        {
            rb2D.AddForce(Vector2.down * cancel_rate);
        }
    }

    public void Respawn()
    {
        SpriteRenderer spr = GetComponentInChildren<SpriteRenderer>();
        RuntimeManager.PlayOneShot(death_event, transform.position);
        spr.enabled = false;

        transform.position = respawn_point.position;

        spr.enabled = true;
    }

    public void ChangeSpawnPoint(Transform spawn_point)
    {
        respawn_point = spawn_point;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(ground_check.position, ground_distance);
    }
}
