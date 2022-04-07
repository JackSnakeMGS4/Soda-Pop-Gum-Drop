using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public static class CameraDirector
{
    static List<CinemachineVirtualCamera> cameras = new List<CinemachineVirtualCamera>();

    static CinemachineVirtualCamera active_cam = null;

    public static bool IsActiveCamera(CinemachineVirtualCamera camera)
    {
        return camera == active_cam;
    }

    public static void SwitchCamera(CinemachineVirtualCamera camera)
    {
        camera.Priority = 10;
        active_cam = camera;

        foreach (CinemachineVirtualCamera c in cameras)
        {
            if(c != camera && c.Priority != 0)
            {
                c.Priority = 0;
            }
        }
    }

    public static void Register(CinemachineVirtualCamera camera)
    {
        cameras.Add(camera);
    }

    public static void Unregister(CinemachineVirtualCamera camera)
    {
        cameras.Remove(camera);
    }
}
