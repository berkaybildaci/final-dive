using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class boss : MonoBehaviour
{
    public Transform player;
    private float timer;
    public float moveSpeed = 10f;
    public float rotationSpeed = 10f;
    private Rigidbody rBody;
    public Transform shotPoint;

    public GameObject mainAttack;
    public float mainTBA = 5f;
    public float deviationMain = 1f;
    private float mainTimer = 0;


    public GameObject miniAttack;
    public float miniTBA = 1f;
    public float deviationMini = 3f;
    private float miniTimer = 0;
    public Transform shotPointM1;
    public Transform shotPointM2;


    public GameObject missileAttack;
    public float missileTBA = 10f;
    public float deviationMissile = 20f;
    private float missileTimer = 0;
    public Transform missilePoint;

    private float timeBucket = 0f;
    private Boolean ramMode;
    private Boolean lazerMode;
    public Transform lazerPoint;
    public GameObject lazer;
    private float delay;
    // Start is called before the first frame update
    void Start()
    {
        rBody = GetComponent<Rigidbody>();
        rBody.freezeRotation = true;
        ramMode = false;
        lazerMode = false;
        delay = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 relativePos = player.position - transform.position;
        relativePos.y = 0;
        Quaternion toRotation = Quaternion.LookRotation(relativePos);
        rBody.rotation = Quaternion.Slerp(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        //transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, transform.eulerAngles.z);
        Rigidbody rb = GetComponent<Rigidbody>();

        /*
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            transform.LookAt(player);
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * 10, ForceMode.Force);
        }
        */
        
        if(timeBucket > 30)
        {
            timeBucket = 0;
            if(UnityEngine.Random.Range(0f, 1f) > 0.0f)
            {
                if (UnityEngine.Random.Range(0f, 1f) > 0.99f)
                {
                    ramMode = true;
                    Invoke("resetRam", 10f);
                }
                else
                {
                    lazerMode = true;
                    Invoke("resetLazer", 10f);
                }
            }
        }
        if(!ramMode && !lazerMode)
            rb.AddForce(transform.forward * moveSpeed, ForceMode.Force);
        else
        {
            if (ramMode)
            {
                if (timeBucket > 5f)
                {
                    rb.AddForce(transform.forward * moveSpeed * 4, ForceMode.Force);
                }
                else
                {

                }
            }
            else if(lazerMode )
            {
                if (timeBucket > 5f)
                {
                    Quaternion toRotation2 = transform.rotation * Quaternion.Euler(90f, 0, 0);
                    GameObject gb = Instantiate(lazer, lazerPoint.position, transform.rotation);
                    Rigidbody rb2 = gb.GetComponent<Rigidbody>();
                    rb2.AddForce(gb.transform.forward * 500f, ForceMode.Impulse);
                    gb.transform.rotation = toRotation2;

                }
                else
                {

                }
            }
        }
        if (!ramMode && !lazerMode)
        {
            fireMain();
            fireMini();
            fireMissile();
        }
        mainTimer += Time.deltaTime;
        miniTimer += Time.deltaTime;
        missileTimer += Time.deltaTime;
        timeBucket += Time.deltaTime;
    }
    void resetRam()
    {
        ramMode = false;
    }
    void resetLazer()
    {
        lazerMode = false;
    }
    void fireMain()
    {
        if (mainTimer > mainTBA)
        {
            mainTimer = 0;
            Vector3 relativePos = player.position - (shotPoint.transform.position +
                new Vector3(UnityEngine.Random.Range(-deviationMain, deviationMain), UnityEngine.Random.Range(-deviationMain, deviationMain), UnityEngine.Random.Range(-deviationMain, deviationMain)));
            Quaternion toRotation = Quaternion.LookRotation(relativePos);
            
            GameObject go = Instantiate(mainAttack, shotPoint.position, toRotation);
            Rigidbody rb = go.GetComponent<Rigidbody>();
            rb.AddForce(go.transform.forward * 200f, ForceMode.Impulse);
            toRotation *= Quaternion.Euler(90f, 0, 0);
            go.transform.rotation = toRotation;

        }
    }

    void fireMini()
    {
        if (miniTimer > miniTBA)
        {
            miniTimer = 0;
            Vector3 relativePos = player.position - (shotPointM1.transform.position +
                new Vector3(UnityEngine.Random.Range(-deviationMini, deviationMini), UnityEngine.Random.Range(-deviationMini, deviationMini), UnityEngine.Random.Range(-deviationMini, deviationMini)));
            Quaternion toRotation = Quaternion.LookRotation(relativePos);

            GameObject go = Instantiate(miniAttack, shotPointM1.position, toRotation);
            Rigidbody rb = go.GetComponent<Rigidbody>();
            rb.AddForce(go.transform.forward * 50f, ForceMode.Impulse);
            toRotation *= Quaternion.Euler(90f, 0, 0);
            go.transform.rotation = toRotation;


            relativePos = player.position - (shotPointM2.transform.position +
                new Vector3(UnityEngine.Random.Range(-deviationMini, deviationMini), UnityEngine.Random.Range(-deviationMini, deviationMini), UnityEngine.Random.Range(-deviationMini, deviationMini)));
            toRotation = Quaternion.LookRotation(relativePos);

            go = Instantiate(miniAttack, shotPointM2.position, toRotation);
            rb = go.GetComponent<Rigidbody>();
            rb.AddForce(go.transform.forward * 50f, ForceMode.Impulse);
            toRotation *= Quaternion.Euler(90f, 0, 0);
            go.transform.rotation = toRotation;

        }
    }
    int count = 0;
    void fireMissile()
    {
        if(missileTimer > missileTBA)
        {
            count = 0;
            missileTimer = 0;
            Invoke("part1", .1f);

        }
    }
    void part1()
    {
        if (count < 50)
            Invoke("part1", .1f);
        count++;
        GameObject go = Instantiate(missileAttack, missilePoint.position, Quaternion.identity);
        Rigidbody rb = go.GetComponent<Rigidbody>();
        rb.AddForce(go.transform.up * 50f, ForceMode.Impulse);
        StartCoroutine(part2(go));

    }
    IEnumerator part2(GameObject go)
    {
        yield return new WaitForSeconds(2f);
        Rigidbody rb = go.GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        Vector3 relativePos = player.position - (go.transform.position +
                new Vector3(UnityEngine.Random.Range(-deviationMissile, deviationMissile), UnityEngine.Random.Range(-deviationMissile, deviationMissile), UnityEngine.Random.Range(-deviationMissile, deviationMissile)));
        Quaternion toRotation = Quaternion.LookRotation(relativePos);
        go.transform.rotation = toRotation;
        rb.AddForce(go.transform.forward * 80f, ForceMode.Impulse);
        toRotation *= Quaternion.Euler(90f, 0, 0);
        go.transform.rotation = toRotation;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Player") Destroy(collision.gameObject);
    }
}
