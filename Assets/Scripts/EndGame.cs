using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using FMODUnity;

public class EndGame : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera game_complete_cam;
    [SerializeField] private AudioManager audio_manager;

    [EventRef] public string win_event;

    private void OnEnable()
    {
        CameraDirector.Register(game_complete_cam);
    }

    private void OnDisable()
    {
        CameraDirector.Unregister(game_complete_cam);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log(collision.name + " enter trigger");

        if (collision.CompareTag("Player"))
        {
            RuntimeManager.PlayOneShot(win_event, transform.position);

            PlayerMovement player_movement = collision.gameObject.GetComponent<PlayerMovement>();
            player_movement._Can_Move = false;
            audio_manager.StopMusic();
            CameraDirector.SwitchCamera(game_complete_cam);
        }
    }
}
