using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EntityData : MonoBehaviour
{
    public float health;
    public float maxHealth;
    public Image healthBarUI;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(healthBarUI != null)
        {
            healthBarUI.fillAmount = (float)(health / maxHealth);
        }
        if(health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void damage(int pain)
    {
        health-=pain;
    }
}
