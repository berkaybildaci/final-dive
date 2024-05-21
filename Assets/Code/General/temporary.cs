using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class temporary : MonoBehaviour
{
    private int hits;
    public TextMeshProUGUI text;
    // Start is called before the first frame update
    void Start()
    {
        hits = 0;
    }

    // Update is called once per frame
    void Update()
    {
        text.text = "hits: " + hits;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name != "Terrain") hits++;
    }
}
