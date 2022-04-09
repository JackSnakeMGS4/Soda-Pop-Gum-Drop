using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropSpike : MonoBehaviour
{
    [SerializeField] private List<GameObject> drops;
    [SerializeField] private float mass;
    [SerializeField] private float drop_delay;
    private float timer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StartTrap());
    }

    IEnumerator StartTrap()
    {
        while (true)
        {
            //Debug.Log("Setting Trap")
            yield return StartCoroutine(Drop());
        }
    }

    IEnumerator Drop()
    {
        //print("Animating spikes");
        //yield return new WaitForSeconds(drop_delay);
        //print("Dropping spike");
        foreach (GameObject go in drops)
        {
            //Debug.Log("Dropping spike");
            Animator a = go.GetComponent<Animator>();
            a.enabled = true;
            a.SetBool("should_shake", true);

            yield return new WaitForSeconds(.1f);
        }

        foreach (GameObject go in drops)
        {
            //Debug.Log("Dropping spike");
            Animator a = go.GetComponent<Animator>();
            Rigidbody2D rb2D = go.GetComponent<Rigidbody2D>();
            a.enabled = false;
            rb2D.bodyType = RigidbodyType2D.Dynamic;
            rb2D.mass = mass;

            yield return new WaitForSeconds(drop_delay);
        }
    }

    private void OnDisable()
    {
        StopCoroutine(Drop());
    }
}
