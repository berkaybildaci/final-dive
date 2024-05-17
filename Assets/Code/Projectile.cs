using Unity.VisualScripting;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int damage;
    public float size;
    private float lifeTime = 3f;
    private float startTime;
    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
    }
    void Update()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        GameObject[] teammates = GameObject.FindGameObjectsWithTag("Teammate");
        GameObject[] damageable = new GameObject[enemies.Length + 1 + teammates.Length];
        damageable[0] = player;
        enemies.CopyTo(damageable, 1);
        teammates.CopyTo(damageable, 1+enemies.Length);
        foreach(GameObject entity in damageable)
        {
            float dist = Vector3.Distance(entity.transform.position, transform.position);
            if (dist <= size)
            {
                entity.GetComponent<EntityData>().damageEntity(damage);
                Destroy(transform.gameObject);
            }
        }
        if(Time.time - startTime >= lifeTime)
        {
            Destroy(transform.gameObject);
        }
    }
}
