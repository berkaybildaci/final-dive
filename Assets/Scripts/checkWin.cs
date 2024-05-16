using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class checkWin : MonoBehaviour
{
    public TextMeshProUGUI text;
    // Start is called before the first frame update
    void Start()
    {
        text.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.childCount == 0)
        {
            text.enabled = true;
        }
    }
}
