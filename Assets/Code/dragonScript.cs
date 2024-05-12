using TMPro;
using UnityEngine;

public class dragonScript : MonoBehaviour
{
    // Gets a vector that points from the player's position to the target's.
    public GameObject player;
    public float speed;
    public Animator anim;
    public EntityData data;
    public int damage = 5;
    private float lastGrowl = 0;
    public float IFrameTime;
    private float lastIFrame = 0;
    private TextMeshProUGUI dragHp;
    private int lives = 3;
    private float stunTime;
    public float stunDuration;
    private bool canMove = true;

    void Start()
    {
        dragHp = GameObject.Find("GUI").transform.Find("Dragon").GetComponent<TextMeshProUGUI>();
        anim = GetComponent<Animator>();
        data.anim = anim;
    }

    void Update()
    {
        if (canMove)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
            transform.rotation = Quaternion.LookRotation(-player.transform.forward, transform.up);
        }
        float dist = Vector3.Distance(player.transform.position, transform.position);
        if (dist < data.attackRange && Time.time - lastIFrame >= IFrameTime && canMove)
        {
            data.attack();
            Attack();
            lastIFrame = Time.time;
        }
        else if (dist < data.attackRange*2 && dist > data.attackRange && Time.time - lastGrowl >= 5 && canMove)
        {
            data.growl(1);
            lastGrowl = Time.time;
        }        
        if(data.health == 0)
        {
            lives--;
            if (lives == 0)
            {
                player.GetComponent<PlayerMechanics>().winScreen();
                Destroy(this.gameObject, anim.GetCurrentAnimatorStateInfo(0).length);
                data.dead();
                data.health = -1;
            }
            else
            {
                player.GetComponent<PlayerMechanics>().toolTip.SetText("Dragon has " + lives + " lives left!");
                canMove = false;
                data.health = -1;
                speed = speed * 1.25f;
                stunTime = Time.time;
            }
        }
        else
        {
            dragHp.SetText("Dragon: " + data.health.ToString());
        }
        if(!canMove && Time.time - stunTime >= stunDuration)
        {
            data.health = 100;
            canMove = true;
            data.playSound(data.hurtSound, 2);
            player.GetComponent<PlayerMechanics>().toolTip.SetText("");
        }
    }

    void Attack()
    {
        player.GetComponent<EntityData>().damageEntity(5);
    }
}