using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMovement : MonoBehaviour
{
    private float speed;

    private bool has_been_shot = false;

    private Rigidbody2D rb2D;

    public void ProjectileSettings(float shot_speed)
    {
        speed = shot_speed;
    }

    // Start is called before the first frame update
    void Start()
    {
        rb2D = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        MoveProjectile();
    }

    private void MoveProjectile()
    {
        Vector2 force = new Vector2(speed, 0f);
        if (!has_been_shot)
        {
            rb2D.AddForce(force, ForceMode2D.Impulse);
            has_been_shot = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag != "Player")
        {
            Destroy(this.gameObject);
        }
    }
}
