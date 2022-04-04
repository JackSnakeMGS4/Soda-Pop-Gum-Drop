using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombMovement : MonoBehaviour
{
    [SerializeField] private GameObject plaftorm_type;
    [SerializeField] private float gravity_scale = 5.0f;
    [SerializeField] private float falling_gravity_scale = 15.0f;
    private float strength;
    private float arch;
    private Vector2 dir;

    private bool has_been_thrown = false;

    private Rigidbody2D rb2D;
    private PlayerCombat player;

    public void BombSettings(float throw_strength, float throw_arch, Vector2 direction, GameObject platform_type = null, PlayerCombat player = null)
    {
        strength = throw_strength;
        arch = throw_arch;
        this.player = player;
        dir = direction;
    }

    // Start is called before the first frame update
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        MoveBomb();
    }

    public void MoveBomb()
    {
        Vector2 force = new Vector2(dir.x * strength, dir.y * arch);
        if (!has_been_thrown)
        {
            rb2D.AddForce(force, ForceMode2D.Impulse);
            has_been_thrown = true;
        }

        if(rb2D.velocity.y >= 0)
        {
            rb2D.gravityScale = gravity_scale;
        }
        else if (rb2D.velocity.y < 0)
        {
            rb2D.gravityScale = falling_gravity_scale;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("Colliding with " + collision.gameObject.name);
        if(collision.gameObject.tag != "Player")
        {
            if(collision.gameObject.tag == "Gum Drop")
            {
                GameObject platform = Instantiate(plaftorm_type, transform.position, transform.rotation);
                player.AddToPlatformsList(platform);
            }
            Destroy(this.gameObject);
        }
    }
}
