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
        }
        //transform.position = mainCamera.transform.position;
        //ransform.rotation = mainCamera.transform.rotation;
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, shotPoint.transform.position, shotPoint.transform.rotation);
    }
}
