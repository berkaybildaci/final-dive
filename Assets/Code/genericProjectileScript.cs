using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class genericProjectileScript : MonoBehaviour
{
    public float lifeTime;
    private float startTime;
    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - startTime >= lifeTime)
        {
            Destroy(transform.gameObject);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.GetComponent<EntityData>() != null && collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<EntityData>().damage(5);
        }
        Destroy(gameObject);
    }
}
