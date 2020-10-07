using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flightStick : MonoBehaviour
{
    public GameObject stick;

    [Range(0.0f, 100.0f)]
    public float deadZone = 10f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per physics cycle
    void FixedUpdate()
    {
        Vector3 rot = stick.transform.localEulerAngles;

        if (rot.x > 180) rot.x -= 360;
        if (rot.y > 180) rot.y -= 360;
        if (rot.z > 180) rot.z -= 360;

        rot.x /= 45f;
        rot.y /= 45f;
        rot.z /= 45f;

        Rigidbody rb = GetComponent<Rigidbody>();

        if ( rot.magnitude <= deadZone / 100)
        {
            rb.AddTorque(-rb.angularVelocity * 440f);
        }
        else
        {
            rb.AddRelativeTorque(rot * 440f);
        }
    }
}
