using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class boss : MonoBehaviour
{
    public Transform player;
    private float timer;
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

    // Start is called before the first frame update
    void Start()
    {
        rBody = GetComponent<Rigidbody>();
        rBody.freezeRotation = true;
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
        rb.AddForce(transform.forward * 10, ForceMode.Force);
        /*
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            transform.LookAt(player);
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * 10, ForceMode.Force);
        }
        */
        fireMain();
        fireMini();
        mainTimer += Time.deltaTime;
        miniTimer += Time.deltaTime;
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
}
