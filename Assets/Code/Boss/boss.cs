using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
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
    public GameObject indicate;


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
    private bool ramMode;
    private bool lazerMode;
    public Transform lazerPoint;
    public LineRenderer lazer;
    private float delay;
    public float superCooldown = 40f;
    public float chanceRam = 0.5f;


    
    // Start is called before the first frame update
    void Start()
    {
        rBody = GetComponent<Rigidbody>();
        rBody.freezeRotation = true;
        ramMode = false;
        lazerMode = false;
        delay = 0;
        indicate.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //Quaternion sourceRotation = transform.rotation;

        
        Quaternion sourceRotation = transform.rotation;
        Vector3 relativePos = player.position - transform.position;
        relativePos.y = 0;
        Quaternion targetRotation = Quaternion.LookRotation(relativePos);


        float maxDegreesDelta = rotationSpeed * Time.deltaTime;
        transform.rotation = Quaternion.RotateTowards(sourceRotation, targetRotation, maxDegreesDelta);
        

        /*
        Vector3 relativePos = player.position - transform.position;
        relativePos.y = 0;
        Quaternion toRotation = Quaternion.LookRotation(relativePos);
        rBody.rotation = Quaternion.SlerpUnclamped(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        */
        //transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, transform.eulerAngles.z);
        Rigidbody rb = GetComponent<Rigidbody>();


            if (timeBucket > superCooldown)
            {
                timeBucket = 0;
                if (UnityEngine.Random.Range(0f, 1f) > 0.0f)
                {
                    if (UnityEngine.Random.Range(0f, 1f) < chanceRam)
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
            if (!ramMode && !lazerMode)
                rb.AddForce(transform.forward * moveSpeed, ForceMode.Force);
            else{
                if (ramMode){
                    if (timeBucket > 5f){
                        rb.AddForce(transform.forward * moveSpeed * 6f, ForceMode.Force);
                        }
                    else{

                    }
                }
                else if (lazerMode){

                    FireLaser();
                    /*
                        Quaternion toRotation2 = transform.rotation * Quaternion.Euler(90f, 0, 0);
                        GameObject gb = Instantiate(lazer, lazerPoint.position, transform.rotation);
                        gb.GetComponent<Collider>().enabled = false;
                        StartCoroutine(enable(gb.GetComponent<Collider>()));
                        Rigidbody rb2 = gb.GetComponent<Rigidbody>();
                        rb2.AddForce(gb.transform.forward * 1000f, ForceMode.Impulse);
                        gb.transform.rotation = toRotation2;
                        timeBucket -= .05f;
                    */
                }
                else{

                }
                
            }
        if (!ramMode)
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
    void FireLaser()
    {
        lazer.enabled = true;

        Ray ray = new Ray(lazerPoint.position-(new Vector3(0f, 3.5f, 0f)), lazerPoint.forward);
        RaycastHit hit;

        lazer.SetPosition(0, ray.origin);

        if (Physics.Raycast(ray, out hit, 9999))
        {
            lazer.SetPosition(1, hit.point);  // Laser hits an object within range, ends at the object.
        }
        else
        {
            lazer.SetPosition(1, ray.GetPoint(9999));  // Laser doesn't hit any objects, extend to max range.
        }

    }

/*
IEnumerator enable(Collider c)
    {
        yield return new WaitForSeconds(.1f);
        c.enabled = true;
    }
*/
    void resetRam()
    {
        ramMode = false;
    }
    void resetLazer()
    {
        lazerMode = false;
        lazer.enabled = false;
    }
    void fireMain()
    {
        if(mainTimer + 0.5f > mainTBA)
        {
            indicate.SetActive(true);
        }
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
            indicate.SetActive(false);
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
        if (missileTimer > missileTBA)
        {
            count = 0;
            missileTimer = 0;
            Invoke("part1", .1f);

        }
    }
    void part1()
    {
        if (count < 40)
            Invoke("part1", .15f);
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
        rb.AddForce(go.transform.forward * 160f, ForceMode.Impulse);
        toRotation *= Quaternion.Euler(90f, 0, 0);
        go.transform.rotation = toRotation;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Player") Destroy(collision.gameObject);
        

    }
}