using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class SetCheckpoint : MonoBehaviour
{
    [EventRef] public string checkpoint_event;

    private ParticleSystem particle_system;

    private void Start()
    {
        particle_system = GetComponentInChildren<ParticleSystem>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerMovement player = collision.GetComponent<PlayerMovement>();

            if (player._Respawn_Point != transform)
            {
                RuntimeManager.PlayOneShot(checkpoint_event, transform.position);
                particle_system.Play();
                player.ChangeSpawnPoint(transform);
            }
        }
    }
}
