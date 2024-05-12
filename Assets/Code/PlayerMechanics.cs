using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMechanics : MonoBehaviour
{
    public EntityData data;
    public EntityMovement move;
    public AudioClip bossTrack;
    public AudioClip fightMusic1;
    public AudioClip fightMusic2;
    public AudioClip fightMusic3;
    public AudioClip fightMusic4;
    public AudioClip fightMusic5;
    public AudioClip fightMusic6;
    public AudioClip deathMusic;
    public AudioClip escapeTrack;
    private Animator animatorRight;
    private Animator animatorLeft;
    public GameObject leftArm;
    private float timeLeftHand = 0;
    private float timeRightHand = 0;
    private ArrayList weapons;
    private int curWeaponIndex;
    public TextMeshProUGUI toolTip;
    private TextMeshProUGUI healthCount;
    private TextMeshProUGUI ammoCount;
    public GameObject UI;
    private int clip = -1;
    private float clipStart = 0;
    private AudioClip[] clips;
    private bool start = false;
    private GameObject[] enemies;
    private float lastTip = 0;
    public float tipSpeed;
    private string[] tips;
    private int tipCount = 0;
    private bool canDie;
    // Start is called before the first frame update
    void Start()
    {
        canDie = true;
        animatorRight = transform.Find("Camera").Find("toyGuyRight").GetComponent<Animator>();
        animatorLeft = transform.Find("Camera").Find("toyGuyLeft").GetComponent<Animator>();
        data = GetComponent<EntityData>();
        move = GetComponent<EntityMovement>();
        weapons = new ArrayList();
        tips = new string[7];
        tips[0] = "Use W, A, S, and D keys to move around";
        tips[1] = "Use spacebar once to jump and twice to double jump";
        tips[2] = "Left Click to shoot. Walk over to the gun to pick it up or add to your ammo";
        tips[3] = "Press R to dash in the direction you are facing";
        tips[4] = "Manage your happiness and dont let anger consume you";
        tips[5] = "You find more ammo by running around the map. Watch for the dragon!";
        tips[6] = "Use the current platform to learn the movement. Then jump and dash towards the desert area to start.";
        clips = new AudioClip[6];
        clips[0] = fightMusic1;
        clips[1] = fightMusic2;
        clips[2] = fightMusic3;
        clips[3] = fightMusic4;
        clips[4] = fightMusic5;
        clips[5] = fightMusic6;
        curWeaponIndex = 0;
        UI = GameObject.Find("GUI");
        toolTip = UI.transform.Find("ToolTip").GetComponent<TextMeshProUGUI>();
        healthCount = UI.transform.Find("Health").GetComponent<TextMeshProUGUI>();
        ammoCount = UI.transform.Find("Ammo").GetComponent<TextMeshProUGUI>();
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        UI.transform.Find("Loose").gameObject.SetActive(false);
        UI.transform.Find("Win").gameObject.SetActive(false);
        foreach (GameObject enemy in enemies)
        {
            enemy.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(start)
        {
            nextTrack();
        }
        else
        {
            nextTip();
        }
        if(weapons.Count > 0)
        {
            ammoCount.SetText(((GameObject)weapons[curWeaponIndex]).GetComponent<GunScript>().ammo.ToString());
        }
        healthCount.SetText(data.health.ToString());
        if(data.health == 0)
        {
            move.canMove = false;
            data.dead(true);
            deathScreen();
            foreach (GameObject enemy in enemies)
            {
                if(enemy.IsDestroyed())
                {
                    continue;
                }
                enemy.SetActive(false);
            }
            data.health = -1;
        }
        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            //punch object
            //glory kills
            leftArm.SetActive(true);
            animatorLeft.SetTrigger("OnPunch");
            timeLeftHand = Time.time;
        }
        if(Input.GetKeyDown(KeyCode.E))
        {
            //changing weapons
            animatorRight.SetTrigger("OnSwap");
            if (weapons.Count > 1)
            {
                changeWeapon();
            }
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            //Dash Animation
            leftArm.SetActive(true);
            animatorLeft.SetTrigger("OnDash");
            timeLeftHand = Time.time;
        }
        if (leftArm.activeSelf && Time.time - timeLeftHand > animatorLeft.GetCurrentAnimatorStateInfo(0).length+0.1 && !animatorLeft.IsInTransition(0))
        {
            leftArm.SetActive(false);
        }
        if (timeRightHand != 0 && Time.time - timeRightHand > animatorRight.GetCurrentAnimatorStateInfo(0).length + 0.1 && !animatorRight.IsInTransition(0))
        {
            ((GameObject)weapons[curWeaponIndex]).SetActive(true);
            timeRightHand = 0;
        }
    }

    private void nextTip()
    {
        if(Time.time - lastTip >= tipSpeed)
        {
            lastTip = Time.time;
            toolTip.SetText(tips[tipCount]);
            tipCount++;
            if(tipCount == tips.Length)
            {
                tipCount = 0;
            }
        }
    }

    private void changeWeapon()
    {
        ((GameObject) weapons[curWeaponIndex]).SetActive(false);
        if(curWeaponIndex++ >= weapons.Count)
        {
            curWeaponIndex = 0;
        }
        else
        {
            curWeaponIndex++;
        }
        ((GameObject)weapons[curWeaponIndex]).SetActive(true);
    }

    public void deathScreen()
    {
        if(canDie)
        {
            UI.transform.Find("Loose").gameObject.SetActive(true);
        }
    }

    public void nextTrack()
    {
        if(clip == -1 || Time.time -  clipStart > clips[clip].length)
        {
            clip++;
            if(clip >= clips.Length)
            {
                clip = 0;
            }
            data.playSound(clips[clip], 0.3f);
            clipStart = Time.time;
        }
    }

    void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.tag == "PickUp")
        {
            Transform heldGun = transform.Find("Camera").Find("toyGuyRight").Find("Armature").Find("Bone").Find("WeaponAnchor").Find(collision.gameObject.name);
            if (heldGun != null)
            {
                heldGun.GetComponent<GunScript>().ammo += collision.gameObject.GetComponent<GunScript>().ammo;
                Destroy(collision.gameObject);
            }
            else if(Input.GetKey(KeyCode.Q))
            {
                toolTip.SetText("");
                animatorRight.SetTrigger("OnGrab");
                collision.gameObject.GetComponent<GunScript>().pickedUp = true;
                collision.gameObject.GetComponent<GunScript>().canShoot = true;
                collision.transform.position = transform.Find("Camera").Find("toyGuyRight").Find("Armature").Find("Bone").Find("WeaponAnchor").position;
                collision.transform.SetParent(transform.Find("Camera").Find("toyGuyRight").Find("Armature").Find("Bone").Find("WeaponAnchor"));
                collision.gameObject.transform.rotation = transform.Find("Camera").rotation;
                if (curWeaponIndex < weapons.Count)
                {
                    ((GameObject)weapons[curWeaponIndex]).SetActive(false);
                }
                timeRightHand = Time.time;
                collision.transform.SetParent(transform.Find("Camera").Find("toyGuyRight").Find("Armature").Find("Bone").Find("WeaponAnchor"));
                weapons.Add(collision.gameObject);
                curWeaponIndex = weapons.Count - 1;
            }
            else
            {
                toolTip.SetText("[Q] - To Equip");
            }
        }
    }

    public void winScreen()
    {
        UI.transform.Find("Win").gameObject.SetActive(true);
        canDie = false;
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Start"))
        {
            start = true;
            toolTip.SetText("");
            foreach(GameObject enemy in enemies)
            {
                enemy.SetActive(true);
            }
            collision.gameObject.SetActive(false);
        }
        else if(collision.gameObject.CompareTag("Death Box"))
        {
            data.health = 0;
        }
    }

    void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.tag == "PickUp")
        {
            toolTip.SetText("");
        }
    }
}
