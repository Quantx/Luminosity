using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class throttle : MonoBehaviour
{

    public GameObject stick;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per physics cycle
    void FixedUpdate()
    {
        float perc = stick.transform.localEulerAngles.x;

        if (perc > 180) perc -= 360;

        perc += 45;

        perc /= 90;

        perc = 1 - perc;

        Rigidbody rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * 4400f * perc);
    }
}
