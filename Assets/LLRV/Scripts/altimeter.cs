using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class altimeter : MonoBehaviour
{
    public GameObject llrv;
    public GameObject hand1;
    public GameObject hand2;
    public GameObject hand3;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float alt = llrv.transform.position.y;

        alt *= 3.281f; // Convert to feet

        hand1.transform.localEulerAngles = new Vector3(0f, 0f, -360 * ((alt % 10) / 10));
        hand2.transform.localEulerAngles = new Vector3(0f, 0f, -360 * ((alt % 100) / 100));
        hand3.transform.localEulerAngles = new Vector3(0f, 0f, -360 * ((alt % 1000) / 1000));
    }
}
