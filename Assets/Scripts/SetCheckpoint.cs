using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetCheckpoint : MonoBehaviour
{
    private ParticleSystem particle_system;

    private void Start()
    {
        particle_system = GetComponentInChildren<ParticleSystem>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            particle_system.Play();

            PlayerMovement player = collision.GetComponent<PlayerMovement>();
            player.ChangeSpawnPoint(transform);
        }
    }
}
