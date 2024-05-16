using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BerkayPlayerHandler : MonoBehaviour
{
    private float pickupRange;
    [SerializeField] private BerkayGunScript gunScriptInstance;
    // Start is called before the first frame update
    void Start()
    {
        pickupRange = 5f;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            RaycastHit hit;
            Vector3 forward = Camera.main.transform.forward;
            Ray ray = new Ray(Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0f)), forward);
            if (Physics.Raycast(ray, out hit, pickupRange))
            {
                if (hit.collider.gameObject.tag == "Pickup")
                {
                    hit.collider.gameObject.SetActive(false);
                    gunScriptInstance.pickedUp = true;
                    Debug.Log("Picked Up Item!");
                }
            }
        }
    }
}
