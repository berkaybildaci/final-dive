using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace cakeslice
{
    public class BerkayPlayerHandler : MonoBehaviour
    {
        private float pickupRange;
        [SerializeField] private BerkayGunScript gunScriptInstance;
        // Start is called before the first frame update
        void Start()
        {
            pickupRange = 5f;
            //GetComponent<Outline>().enabled = !GetComponent<Outline>().enabled;
        }

        // Update is called once per frame
        void Update()
        {
            HighlightItem();
            PickupItem();
        }

        void PickupItem()
        {
            if (Input.GetKeyDown(KeyCode.E))
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
        void HighlightItem()
        {
            RaycastHit hit;
            Vector3 forward = Camera.main.transform.forward;
            Ray ray = new Ray(Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0f)), forward);
            if (Physics.Raycast(ray, out hit, pickupRange))
            {
                if (hit.collider.gameObject.tag == "Pickup")
                {
                    hit.collider.GetComponent<BerkayGunPickup>().currentlyHighlighted = true;
                    hit.collider.transform.GetChild(0).GetComponent<Outline>().enabled = true;
                }
                else
                {
                    ResetHighlights();
                }
            }
            else
            {
                ResetHighlights();
            }
        }

        void ResetHighlights()
        {
            BerkayGunPickup[] gunPickups = FindObjectsOfType<BerkayGunPickup>();
            foreach (BerkayGunPickup gunPickup in gunPickups)
            {
                if (gunPickup.currentlyHighlighted == true)
                {
                    gunPickup.gameObject.transform.GetChild(0).GetComponent<Outline>().enabled = false;
                    gunPickup.currentlyHighlighted = false;
                }
            }
        }
    }
}

