using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public GameObject grenade;
    public Transform useful;
    public Transform shotPoint;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.G)) {
           //Vector3 rot = new Vector3(useful.transform.rotation.x, transform.rotation.y, 0f).normalized;
           Rigidbody rb = Instantiate(grenade, shotPoint.position, useful.rotation).GetComponent<Rigidbody>();
            rb.AddForce(useful.transform.forward * 20, ForceMode.Impulse);
            //rb.AddForce(transform.up*5 + useful.transform.forward * 5, ForceMode.Impulse);
            //rb.AddForce(transform.forward * 8 * 2f, ForceMode.Impulse);
            //rb.AddForce(transform.up * 8 * 2f, ForceMode.Impulse);
            ApplyGrenadeDamage();

        }
    }
       void ApplyGrenadeDamage()
    {
        // Find the player GameObject
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            // Get the EntityData component from the player
            EntityData playerEntityData = player.GetComponent<EntityData>();
            if (playerEntityData != null)
            {
                // Apply grenade damage to the player's health
                playerEntityData.damage(10);
            }
        }
    }
}
