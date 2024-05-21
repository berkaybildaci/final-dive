using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace cakeslice
{
    public class BerkayGunPickup : MonoBehaviour
    {
        public bool currentlyHighlighted;
        public GameObject gunModel;
        // Start is called before the first frame update
        void Start()
        {
            currentlyHighlighted = false;
            gunModel.GetComponent<Outline>().enabled = false;
        }

        // Update is called once per frame
        void Update()
        {
            transform.Rotate(0, 1, 0);
        }
    }
}

