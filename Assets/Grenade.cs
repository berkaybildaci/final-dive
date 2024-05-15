using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public GameObject grenade;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.G)) {
           Rigidbody rb = Instantiate(grenade, transform.position + new Vector3(0f, 0f, 2f), UnityEngine.Quaternion.identity).GetComponent<Rigidbody>();
           rb.AddForce(transform.rotation.eulerAngles.normalized*10, ForceMode.Impulse);
           //rb.AddForce(transform.up * 8 * 2f, ForceMode.Impulse);
        }
    }
}
