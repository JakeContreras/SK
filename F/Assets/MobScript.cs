using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobScript : MonoBehaviour
{
    public Joystick joystick;
    public Joybutton joybutton;
    void Start()
    {
        /*joystick = FindObjectOfType<Joystick>();
        joybutton = FindObjectOfType<Joybutton>();*/
    }

    void Update()
    {
        var rigidbody = GetComponent<Rigidbody>();

        rigidbody.velocity = new Vector3(joystick.Horizontal * 100f,rigidbody.velocity.y, joystick.Vertical * 100f);
    }
}
