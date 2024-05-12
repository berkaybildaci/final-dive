using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class GunScript : MonoBehaviour
{
    public int ammo;
    public bool canShoot;
    public bool pickedUp;
    public float rotateSpeed;
    public float bulletSpeed;
    public float shootSpeed;
    public bool isSemi;
    public float bulletTime;
    public float shootTime;
    private Animator animator;
    public AudioClip shootSound;
    public AudioClip emptyClipSound;
    private AudioSource source;
    public GameObject bulletPrefab;
    private ArrayList bullets;
    private ArrayList bulletTimes;
    public float volume;
    public float bulletSize;
    // Start is called before the first frame update
    void Start()
    {
        pickedUp = false;
        canShoot = false;
        shootTime = 0;
        animator = GetComponentInChildren<Animator>();
        source = GetComponent<AudioSource>();
        bullets = new ArrayList();
        bulletTimes = new ArrayList();
    }

    // Update is called once per frame
    void Update()
    {
        if(canShoot && ammo > 0)
        {
            if(isSemi && Input.GetMouseButtonDown((int)MouseButton.Left) && (Time.time - shootTime) >= shootSpeed)
            {
                animator.SetBool("IsShooting", true);
                shootTime = Time.time;
                source.PlayOneShot(shootSound, volume);
                Shoot();
            }
            else if(!isSemi && Input.GetMouseButton((int)MouseButton.Left) && (Time.time - shootTime) >= shootSpeed)
            {
                animator.SetBool("IsShooting", true);
                shootTime = Time.time;
                source.PlayOneShot(shootSound, volume);
                Shoot();
            }
        }
        else if(ammo < 0 && canShoot && Input.GetMouseButtonDown((int)MouseButton.Left))
        {
            source.PlayOneShot(emptyClipSound, volume);
        }
        if (!pickedUp)
        {
            transform.Rotate(new Vector3(0, 1, 0), rotateSpeed * Time.deltaTime);
        }
        else if(pickedUp && GetComponent<BoxCollider>() != null)
        {
            Destroy(GetComponent<BoxCollider>());
            animator.SetBool("IsIdle", false);
        }
        if(canShoot && Input.GetMouseButtonUp((int)MouseButton.Left))
        {
            animator.SetBool("IsShooting", false);
        }
        for(int i = 0; i < bullets.Count; i++)
        {
            if (Time.time-((float)bulletTimes[i]) >= bulletTime)
            {
                bulletTimes.RemoveAt(i);
                Destroy((GameObject)bullets[i]);
                bullets.RemoveAt(i);
            }
        }
    }

    private void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        // Set the bullet's position and rotation
        bullet.transform.position = transform.position + new Vector3(0.1f, -0.6f, -0);
        bullet.AddComponent<BoxCollider>();
        bullet.AddComponent<Rigidbody>();
        bullet.GetComponent <Rigidbody>().useGravity = false;
        bullet.GetComponent<Rigidbody>().velocity = GameObject.Find("Player").transform.Find("Camera").forward * bulletSpeed * Time.deltaTime;
        bullet.GetComponent<Rigidbody>().isKinematic = false;
        bullet.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        bullet.AddComponent<Projectile>();
        bullet.GetComponent<Projectile>().damage = 1;
        bullet.GetComponent<Projectile>().size = bulletSize;
        bullet.transform.parent = GameObject.Find("Bullets").transform;
        bullets.Add(bullet);
        bulletTimes.Add(Time.time);
        ammo--;
    }
}
