using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fuelPump : MonoBehaviour
{
    public GameObject battery;
    public GameObject hand;
    public GameObject toggle;
    public float deltaPsi = 1f;
    private float togVal;
    private float psi;

    // Start is called before the first frame update
    void Start()
    {
        togVal = toggle.transform.localEulerAngles.x;
    }

    // Update is called once per frame
    void Update()
    {
        float volts = battery.GetComponent<power>().getVolts();

        if (togVal == toggle.transform.localEulerAngles.x || volts <= 23 )
        {
            psi -= deltaPsi * Time.deltaTime;
        }
        else
        {
            psi += deltaPsi * Time.deltaTime;
        }


        psi = Mathf.Clamp(psi, 0f, 20f);

        float perc = psi / 30f;
        perc *= -270;
        perc += 135;

        Vector3 rot = hand.transform.localEulerAngles;
        rot.z = perc;
        hand.transform.localEulerAngles = rot;
    }

    public float getPsi()
    {
        return psi;
    }
}
