using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [Header("Bomb Settings")]
    [SerializeField] private float throw_strength = 1.0f;
    [SerializeField] private float throw_arch = 1.0f;
    [SerializeField] private float max_throw_strength = 10.0f;
    [SerializeField] private float max_throw_arch = 20.0f;
    [SerializeField] private float charge_rate = 1.0f;
    [SerializeField] private float button_timer = 0.3f;
    [SerializeField] private float throw_cooldown = 2.0f;
    [SerializeField] private List<GameObject> platform_types;
    private float total_throw_strength = 0f;
    private float total_throw_arch = 0f;
    private float charge_time = 0f;
    private float throw_timer = 0f;
    private bool is_charging_throw = false;

    [Header("Projectile Settings")]
    [SerializeField] private float shot_speed = 1.0f;
    [SerializeField] private int max_gum_drops = 5;
    private int gum_drops_left;

    [Header("Assets Used")]
    [SerializeField] private GameObject platform_can;
    [SerializeField] private GameObject projectile;
    [SerializeField] private Transform throw_spawn;
    [SerializeField] private Transform projectile_spawn;
    private List<GameObject> gum_platforms;
    private PlayerCombat player_combat_script;
    private GameObject current_platform_type;

    // Start is called before the first frame update
    void Start()
    {
        gum_platforms = new List<GameObject>();
        player_combat_script = gameObject.GetComponent<PlayerCombat>();
        current_platform_type = platform_types[0];
        gum_drops_left = max_gum_drops;
    }

    // Update is called once per frame
    void Update()
    {
        HandleBomb();
        HandleGumDrop();
        HandlePlatformReload();
    }

    private void HandleBomb()
    {
        if (Input.GetButtonDown("Fire1") && !is_charging_throw && throw_timer >= throw_cooldown)
        {
            is_charging_throw = true;
            charge_time = 0f;
            total_throw_strength = throw_strength;
            total_throw_arch = throw_arch;
            throw_timer = 0f;
        }

        if (is_charging_throw)
        {
            charge_time += Time.deltaTime;
            if (Input.GetButtonUp("Fire1") || charge_time > button_timer)
            {
                //Debug.Log("Throwing Soda Can");
                GameObject bomb = Instantiate(platform_can, throw_spawn.position, throw_spawn.rotation);
                BombMovement script = bomb.GetComponent<BombMovement>();
                Vector3 dir = throw_spawn.position - transform.position;
                dir = dir.normalized;
                script.BombSettings(total_throw_strength, total_throw_arch, dir, current_platform_type, player_combat_script);

                is_charging_throw = false;
            }
            else
            {
                if(total_throw_strength < max_throw_strength)
                {
                    total_throw_strength += charge_rate;
                }
                if(total_throw_arch < max_throw_arch)
                {
                    total_throw_arch += charge_rate;
                }
            }
        }

        if (!is_charging_throw)
        {
            throw_timer += Time.deltaTime;
        }
        //Debug.Log("Throw values: " + total_throw_strength + "STR; " + total_throw_arch + "ARCH");
    }

    private void HandleGumDrop()
    {
        if (Input.GetButtonDown("Fire2") && gum_drops_left > 0)
        {
            //Debug.Log("Shooting Gum Drop");
            GameObject shot = Instantiate(projectile, projectile_spawn.position, projectile_spawn.rotation);
            ProjectileMovement script = shot.GetComponent<ProjectileMovement>();
            Vector3 dir = projectile_spawn.position - transform.position;
            dir = dir.normalized;
            script.ProjectileSettings(shot_speed, dir);
            gum_drops_left--;
        }
    }

    private void HandlePlatformReload()
    {
        if (Input.GetButtonDown("Reload"))
        {
            if (gum_platforms.Count > 0)
            {
                //for (int i = 0; i < gum_platforms.Count; i++)
                //{
                //    //if platform was last used platform then don't destroy

                //    //destroy and remove all other platforms
                //    //google a way to remove list items at runtime
                //    GameObject obj = gum_platforms[i];
                //    gum_platforms.RemoveAt(i);
                //    Destroy(gum_platforms[i]);
                //}

                int last_used_platform = 0;
                Destroy(gum_platforms[last_used_platform]);
                gum_platforms.RemoveAt(last_used_platform);

                gum_drops_left = max_gum_drops - gum_platforms.Count;
            }
            else
            {
                gum_drops_left = max_gum_drops;
            }

            //Debug.Log("Total platforms: " + gum_platforms.Count);
        }
    }

    public void AddToPlatformsList(GameObject platform = null)
    {
        gum_platforms.Add(platform);
    }

    private void OnDrawGizmos()
    {
        //Gizmos.color = Color.red;
        //Gizmos.DrawLine(gameObject.transform.position, bomb_spawn.position);
    }
}
