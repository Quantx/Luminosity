using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class turbofanThrust : MonoBehaviour
{
    public GameObject llrv;
    public GameObject toggle;
    private float togVal;
    public GameObject hand;
    public GameObject engine;
    public GameObject fan;
    public float maxFanRPM = 50f;
    private float maxThrottle;
    public float deltaThrottle = 650f;
    private float throttle;
    public GameObject fuelPump;
    private bool badStart = false;

    public AudioClip engineStart;
    public AudioClip engineLoop;
    public AudioClip engineStop;
    private int engineMode = 0; //0 = Off, 1 = Starting up, 2 = Running, 3 = Stoping
    private int lastEngineMode = 0;

    // Start is called before the first frame update
    void Start()
    {
        Rigidbody rb = llrv.GetComponent<Rigidbody>();

        maxThrottle = rb.mass * (Physics.gravity.magnitude * 5 / 6);

        togVal = toggle.transform.localEulerAngles.x;
    }

    // Update is called once per physics cycle
    void FixedUpdate()
    {
        float psi = fuelPump.GetComponent<fuelPump>().getPsi();

        if (togVal != toggle.transform.localEulerAngles.x)
        {
            if (psi < 10) badStart = true;
        }
        else
        {
            badStart = false;
        }

        AudioSource sfx = engine.GetComponent<AudioSource>();

        if (togVal == toggle.transform.localEulerAngles.x || psi < 10 || badStart)
        {
            throttle -= deltaThrottle * Time.deltaTime;
            engineMode = 3;
        }
        else
        {
            throttle += deltaThrottle * Time.deltaTime;
            engineMode = 1;
        }

        throttle = Mathf.Clamp(throttle, 0f, maxThrottle);

        if (engineMode == 3 && throttle == 0f) engineMode = 0;
        else if (engineMode == 1 && throttle == maxThrottle) engineMode = 2;


            float perc = throttle / maxThrottle;

        if ( engineMode != lastEngineMode )
        {
            switch (engineMode)
            {
                case 1:
                    sfx.clip = engineStart;
                    sfx.loop = false;
                    sfx.time = perc * engineStart.length;
                    sfx.Play();
                    break;
                case 2:
                    sfx.clip = engineLoop;
                    sfx.loop = true;
                    sfx.time = 0;
                    sfx.Play();
                    break;
                case 3:
                    sfx.clip = engineStop;
                    sfx.loop = false;
                    sfx.time = engineStop.length - (perc * engineStop.length);
                    sfx.Play();
                    break;
                default:
                    sfx.Stop();
                    break;
            }
        }

        lastEngineMode = engineMode;

        fan.transform.Rotate(0, 0, 6 * perc * maxFanRPM * Time.deltaTime, Space.Self);

        perc *= -210;

        Vector3 rot = hand.transform.localEulerAngles;
        rot.z = perc;
        hand.transform.localEulerAngles = rot;

        Rigidbody rb = llrv.GetComponent<Rigidbody>();

        if (!rb.isKinematic)
        {
            rb.AddForce(engine.transform.forward * throttle);
        }
    }

    public float getThrottle()
    {
        return throttle;
    }

    public float getMaxThrottle()
    {
        return maxThrottle;
    }
}
