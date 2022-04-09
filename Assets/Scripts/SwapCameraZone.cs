using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class SwapCameraZone : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera cam;

    private void OnEnable()
    {
        CameraDirector.Register(cam);
    }

    private void OnDisable()
    {
        CameraDirector.Unregister(cam);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            if (!CameraDirector.IsActiveCamera(cam))
            {
                CameraDirector.SwitchCamera(cam);
            }
        }
    }
}
