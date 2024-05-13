using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int damage;
    public float size;
    // Start is called before the first frame update
    void Update()
    {
        foreach(GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            float dist = Vector3.Distance(enemy.transform.position, transform.position);
            if (dist <= size)
            {
                enemy.GetComponent<EntityData>().damageEntity(damage);
                Destroy(transform.gameObject);
            }
        }
    }
}
