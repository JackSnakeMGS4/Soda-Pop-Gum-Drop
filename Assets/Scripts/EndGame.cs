using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class EndGame : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera player_cam;
    [SerializeField] private CinemachineVirtualCamera game_complete_cam;
    [SerializeField] private AudioManager audio_manager;

    private void OnEnable()
    {
        CameraDirector.Register(player_cam);
        CameraDirector.Register(game_complete_cam);
        CameraDirector.SwitchCamera(player_cam);
    }

    private void OnDisable()
    {
        CameraDirector.Unregister(player_cam);
        CameraDirector.Unregister(game_complete_cam);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log(collision.name + " enter trigger");

        if (collision.CompareTag("Player"))
        {
            PlayerMovement player_movement = collision.gameObject.GetComponent<PlayerMovement>();
            player_movement._Can_Move = false;
            audio_manager.StopMusic();
            CameraDirector.SwitchCamera(game_complete_cam);
        }
    }
}
