using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class power : MonoBehaviour
{
    public GameObject hand;
    public GameObject toggle;
    public float deltaVolt = 10f;
    private float togVal;
    private float volts = 0f;

    // Start is called before the first frame update
    void Start()
    {
        togVal = toggle.transform.localEulerAngles.x; 
    }

    // Update is called once per frame
    void Update()
    {
        if (togVal == toggle.transform.localEulerAngles.x)
        {
            volts -= deltaVolt * Time.deltaTime;
        }
        else
        {
            volts += deltaVolt * Time.deltaTime;
        }


        volts = Mathf.Clamp(volts, 0f, 24f);

        float perc = volts / 50f;
        perc *= -83.5f;

        Vector3 rot = hand.transform.localEulerAngles;
        rot.z = perc;
        hand.transform.localEulerAngles = rot;
    }

    public float getVolts()
    {
        return volts;
    }
}
