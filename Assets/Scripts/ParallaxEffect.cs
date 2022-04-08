using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ParallaxEffect : MonoBehaviour
{
    private float length;
    private float start_pos;
    [SerializeField] CinemachineVirtualCamera cam;
    [SerializeField] private float parallax_effect;

    // Start is called before the first frame update
    void Start()
    {
        start_pos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float temp = (cam.transform.position.x * (1 - parallax_effect));
        float distance = (cam.transform.position.x * parallax_effect);

        transform.position = new Vector3(start_pos + distance, transform.position.y, transform.position.z);

        if (temp > start_pos + length) start_pos += length;
        else if (temp < start_pos - length) start_pos -= length;
    }
}
