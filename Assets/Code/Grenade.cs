using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public GameObject grenade;
    public Transform useful;
    public Transform shotPoint;
    public AudioClip throwSound; // Add this line
    private AudioSource audioSource; // Add this line

    void Start()
    {
        audioSource = GetComponent<AudioSource>(); // Add this line
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            Rigidbody rb = Instantiate(grenade, shotPoint.position, useful.rotation).GetComponent<Rigidbody>();
            rb.AddForce(useful.transform.forward * 20, ForceMode.Impulse);
            audioSource.PlayOneShot(throwSound); // Add this line
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
