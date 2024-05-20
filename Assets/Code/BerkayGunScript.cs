using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BerkayGunScript : MonoBehaviour
{
    [SerializeField] private GameObject mainCamera;
    public bool pickedUp;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject shotPoint;
    [SerializeField] private GameObject muzzleFlash;
    private float lastShot = 0f;
    public float shotDelay;
    // Start is called before the first frame update
    void Start()
    {
        pickedUp = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(pickedUp)
        {
            gameObject.GetComponent<MeshRenderer>().enabled = true;
            if(Input.GetMouseButton(0) && (Time.time - lastShot >= shotDelay))
            {
                lastShot = Time.time;
                Shoot();
            }
        }
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, shotPoint.transform.position, shotPoint.transform.rotation);
        bullet.GetComponent<Rigidbody>().velocity = shotPoint.transform.forward * 80f;
        muzzleFlash.GetComponent<ParticleSystem>().Play();
    }
}
