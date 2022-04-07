using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class UpgradeAmmoCap : MonoBehaviour
{
    [EventRef] public string pickup_event = "";

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerCombat script = collision.GetComponent<PlayerCombat>();
            RuntimeManager.PlayOneShot(pickup_event, transform.position);
            script.IncreaseAmmoCapacity();

            Destroy(gameObject);
        }
    }
}
