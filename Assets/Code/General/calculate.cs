using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class calculate : MonoBehaviour
{
    public GameObject parent;
    public GameObject useful;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Euler(new Vector3(useful.transform.rotation.x,0f, 0f));
        //transform.position = parent.transform.position + new Vector3(0f, 4.5f, 1.2f);
    }
}
