using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController characterController;

    public Animator anim;
    public float speed = 4.0f;
    public GameObject m_weapon;

    public int playernumber = 1;

    private Vector3 moveDirection = Vector3.zero;
    public Collider coll;
    public bool isdead = false;
    private string vert;
    private string hori;
    private string fire;
    private string sword;
    [HideInInspector]public int wins;

    void Start()
    {
        transform.position = new Vector3(Random.Range(-12f,12f), 0.04f, Random.Range(-7.5f,7.5f));
        characterController = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        coll = GetComponent<BoxCollider>();
        vert = "Vertical" + playernumber;
        hori = "Horizontal" + playernumber;
        fire = "Fire" + playernumber;
        if (playernumber == 1)
        {
            sword = "sword" + 2;
        }else
        {
            sword = "sword" + 1;
        }
    }

    void Update()
    {
        if (isdead == false)
        {
            Movement();
            GetInput();
        }
    }

    void Movement()
    {
        if (Mathf.Abs (Input.GetAxis(hori)) > 0.1f || Mathf.Abs (Input.GetAxis(vert)) > 0.1f)
        {
            if (anim.GetBool("attacking") == true) 
            {
                return;
            }else if (anim.GetBool("attacking") == false)
            {
                anim.SetBool ("running", true);
                anim.SetInteger("Condition", 1);
                moveDirection = new Vector3(Input.GetAxis(hori), 0.0f, Input.GetAxis(vert));
                moveDirection *= speed;
                characterController.SimpleMove(-moveDirection * Time.deltaTime);
                Vector3 facingrotation = Vector3.Normalize(new Vector3(Input.GetAxis(hori), 0f, Input.GetAxis(vert)));
                if (facingrotation != Vector3.zero)         //This condition prevents from spamming "Look rotation viewing vector is zero" when not moving.
                    transform.forward = -facingrotation;
            }
        }
        else{
            anim.SetBool ("running", false);
            moveDirection = new Vector3 (0.0f,0.0f,0.0f);
            if (anim.GetBool("attacking") == false)
                anim.SetInteger("Condition", 0);
        }
    }

    void GetInput()
    {
        if (Mathf.Abs (Input.GetAxis(fire)) > 0.1f)
        {
            if(anim.GetBool("running") == true)
            {
                anim.SetBool ("running", false);
                anim.SetInteger ("Condition", 0);
            }
            if (anim.GetBool("running") == false)
            {
                if (anim.GetBool("attacking") == false)
                    StartCoroutine(AttackRoutine());
            }
        }
    }

    IEnumerator AttackRoutine()
    {
        anim.SetBool("attacking", true);
        anim.SetInteger("Condition", 2);
        yield return new WaitForSeconds (2);
        anim.SetInteger("Condition", 0);
        anim.SetBool("attacking", false);
    }

    void Enableweapon()
    {
        m_weapon.GetComponent<Collider>().enabled = true;
        SoundManagerScript.PlaySound("Woosh");
    }

    void Disableweapon()
    {
        m_weapon.GetComponent<Collider>().enabled = false;
    }

    public void Restart()
    {
        Disableweapon();
        anim.enabled = false;
        isdead = true;
        characterController.enabled = false;
        transform.position = new Vector3(Random.Range(-12f,12f), 0.04f, Random.Range(-7.5f,7.5f));
        characterController.enabled = true;
        anim.enabled = true;
        isdead = false;
    }

    void OnTriggerEnter (Collider collider)
    {
        if (collider.gameObject.tag == sword)
        {
            SoundManagerScript.PlaySound("Death");
            anim.enabled = false;
            isdead = true;
            Disableweapon();
        }
    }
}
