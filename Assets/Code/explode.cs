
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explode : MonoBehaviour
{
    public float explosionRadius = 50f;
    public float explosionForce = 1000f;
    public ParticleSystem grenadeExplosion;
    public float startTime = 60f;
    private float currentTime;
    public GameObject grenadeVisual;
    // Start is called before the first frame update
    void Start()
    {
        currentTime = startTime;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(1, 1, 1);
    }
    private void OnCollisionEnter(Collision collision)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider nearbyObject in colliders)
        {
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Debug.Log("Applying explosion force to: " + nearbyObject.gameObject.name);
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
            }

        }
        Destroy(gameObject);
        GameObject ps = Instantiate(grenadeExplosion.gameObject, transform.position, transform.rotation);
        ps.GetComponent<ParticleSystem>().Play();
        ps.GetComponent<genericProjectileScript>().enabled = true;

    }
}