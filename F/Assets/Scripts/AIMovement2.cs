using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMovement2 : MonoBehaviour
{
    public CharacterController characterController;

    public Animator anim;
    public float speed = 4.0f;
    public GameObject m_weapon;

    private Vector3 moveDirection = Vector3.zero;
    public Collider coll;
    public bool isdead = false;
    private float vert;
    private float hori;
    private float waitTime;
    private float startWaitTime;
    private float xmove;

    private float zmove;

    private Vector3 moveSpot;


    void Start()
    {
        // transform.position = new Vector3(Random.Range(-12f,12f), 0.04f, Random.Range(-7.5f,7.5f));
        characterController = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        coll = GetComponent<BoxCollider>();
        startWaitTime = Random.Range(0,8);
        waitTime = startWaitTime;
        do
            xmove = Random.Range(-8,8);
        while (transform.position.x + xmove > 14 || transform.position.x + xmove < -14);
        do
            zmove = Random.Range(-8,8);
        while (transform.position.z + zmove > 9.5 || transform.position.z + zmove < -9.5);
        moveSpot = new Vector3(transform.position.x + xmove, 0.04f, transform.position.z + zmove);
    }

    void Update()
    {
        if (isdead == false)
        {
            Movement();
        }
    }

    void Movement()
    {
        if (Vector3.Distance(transform.position,moveSpot) < 2)
        {
            anim.SetBool ("running", false);
            anim.SetInteger("Condition", 0);
            Vector3 facingrotation = Vector3.Normalize(new Vector3(hori, 0f, vert));
            if (facingrotation != Vector3.zero)         //This condition prevents from spamming "Look rotation viewing vector is zero" when not moving.
                transform.forward = -facingrotation;
            if (waitTime <= 0)
            {
                do
                    xmove = Random.Range(-8,8);
                while (transform.position.x + xmove > 14 || transform.position.x + xmove < -14);
                do
                    zmove = Random.Range(-8,8);
                while (transform.position.z + zmove > 9.5 || transform.position.z + zmove < -9.5);
                moveSpot = new Vector3(transform.position.x + xmove, 0.04f, transform.position.z + zmove);
                startWaitTime = Random.Range(0,8);
                waitTime = startWaitTime;
            }else
            {
                waitTime -= Time.deltaTime;
            }
        }else
        {
            if (moveSpot.x > transform.position.x && moveSpot.x -1 > transform.position.x)
            {
                hori = -1;
            }
            else if (moveSpot.x < transform.position.x && moveSpot.x -1 < transform.position.x)
            {
                hori = 1;
            }else
            {
                hori = 0;
            }
            if (moveSpot.z > transform.position.z && moveSpot.z -1 > transform.position.z)
            {
                vert = -1;
            }
            else if (moveSpot.z < transform.position.z && moveSpot.z-1 < transform.position.z)
            {
                vert = +1;
            }else
            {
                vert = 0;
            }
            anim.SetBool ("running", true);
            anim.SetInteger("Condition", 1);
            moveDirection = new Vector3(hori, 0.04f, vert);
            moveDirection *= speed;
            characterController.SimpleMove(-moveDirection * Time.deltaTime);
            Vector3 facingrotation = Vector3.Normalize(new Vector3(hori, 0f, vert));
            if (facingrotation != Vector3.zero)         //This condition prevents from spamming "Look rotation viewing vector is zero" when not moving.
                transform.forward = -facingrotation;
        } 
        if (coll.gameObject.tag == "AI")
        {
            moveSpot = transform.position;
        }
    }

    void OnTriggerEnter (Collider collider)
    {
        if (collider.gameObject.tag == "sword1" || collider.gameObject.tag == "sword2")
        {
            SoundManagerScript.PlaySound("Death");
            anim.enabled = false;
            isdead = true;
        }
    }
    public void Restart()
    {
        anim.enabled = false;
        isdead = true;
        characterController.enabled = false;
        transform.position = new Vector3(Random.Range(-12f,12f), 0.04f, Random.Range(-7.5f,7.5f));
        characterController.enabled = true;
        anim.enabled = true;
        isdead = false;
    }
}
