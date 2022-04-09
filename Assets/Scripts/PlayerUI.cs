using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private TMP_Text sodas_text;
    [SerializeField] private TMP_Text ammo_text;
    [SerializeField] private TMP_Text platforms_text;

    private PlayerCombat p_combat;

    // Start is called before the first frame update
    void Start()
    {
        p_combat = player.GetComponent<PlayerCombat>();   
    }

    // Update is called once per frame
    void Update()
    {
        sodas_text.SetText("" + p_combat._Sodas_Used);
        ammo_text.SetText("" + p_combat._Gum_Drops);
        platforms_text.SetText("" + p_combat._Gum_Platforms);
    }
}
