using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class launchSequence : MonoBehaviour
{
    
    public GameObject sled;
    public GameObject doorLeft;
    public GameObject doorRight;

    public GameObject startLight;
    public GameObject doorLight;

    public GameObject timer;

    public GameObject sec1;
    public GameObject sec2;

    public GameObject mil1;
    public GameObject mil2;

    public GameObject[] toggles;
    private float[] togPos;

    public GameObject engine;

    public GameObject ejectLeft;
    public GameObject ejectRight;
    public GameObject ejectSeat;

    public GameObject gameover;

    public AudioClip[] speeches;

    public Sprite[] bubbleFont;    

    private float counter = 10f;

    private int stage = 0;


    // Start is called before the first frame update
    void Start()
    {
        togPos = new float[toggles.Length];

        for ( int i = 0; i < toggles.Length; i++ )
        {
            togPos[i] = toggles[i].transform.localEulerAngles.x;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Stage: " + stage);

        if ( ejectSeat.transform.parent == null )
        {
            if ( sled.transform.position.z >= 20f )
            {
                if (stage < 9)
                {
                    counter = 15f;
                    stage = 9;
                }
            }
            else
            {
                gameover.GetComponent<deathWatch>().killPlayer("Indoor Ejector Seat", true);
            }
        }

        switch (stage)
        {
            case 0: // 10 Second idle to let player look around
                stageIdle();
                break;
            case 1: // Intro speech
                speechWait();
                break;
            case 2: // Wait for player to pull ejection handels
                ejectionCheckDown();
                break;
            case 3: // Tell player to put handles back up
                speechWait();
                break;
            case 4: // Wait for player to reset ejection handels
                ejectionCheckUp();
                break;
            case 5: // Tell player startup sequence
                speechWait();
                break;
            case 6: // Wait for player to correctly startup the craft
                stageStartup();
                break;
            case 7: // Initiate the countdown
                stageCountdown();
                break;
            case 8: // Sled is in motion
                stageRunning();
                break;
            case 9:
                stageEject();
                break;
        }
    }

    private void lockSwitches(bool lockDown)
    {
        for (int i = 0; i < toggles.Length; i++)
        {
            Vector3 ang = toggles[i].transform.localEulerAngles;
            ang.x = togPos[i] * (lockDown ? -1 : 1);
            toggles[i].transform.localEulerAngles = ang;
        }
    }

    private void stageIdle()
    {
        lockSwitches(false);
        counter = Mathf.Max(counter - Time.deltaTime, 0f);

        if (counter == 0f)
        {
            AudioSource fas = GetComponent<AudioSource>();

            fas.clip = speeches[0];
            fas.Play();

            stage++;
        }
    }

    private void speechWait()
    {
        if ( stage <= 4 ) lockSwitches(false);
        AudioSource fas = GetComponent<AudioSource>();

        if (!fas.isPlaying) stage++;
    }

    private void ejectionCheckDown()
    {
        lockSwitches(false);

        float lAng = ejectLeft.transform.localEulerAngles.x;
        float rAng = ejectRight.transform.localEulerAngles.x;

        if (lAng > 180) lAng -= 360;
        if (rAng > 180) rAng -= 360;

        if (lAng < -45f && rAng < -45f)
        {
            AudioSource fas = GetComponent<AudioSource>();

            fas.clip = speeches[1];
            fas.Play();

            stage++;
        }
    }

    private void ejectionCheckUp()
    {
        lockSwitches(false);

        float lAng = ejectLeft.transform.localEulerAngles.x;
        float rAng = ejectRight.transform.localEulerAngles.x;

        if (lAng > 180) lAng -= 360;
        if (rAng > 180) rAng -= 360;

        if (lAng > -5f && rAng > -5f)
        {
            AudioSource fas = GetComponent<AudioSource>();

            fas.clip = speeches[2];
            fas.Play();

            stage++;
        }
    }

    private void stageStartup()
    {
        turbofanThrust tft = engine.GetComponent<turbofanThrust>();

        bool launch = tft.getThrottle() > tft.getMaxThrottle() / 4;

        for (int i = 0; i < toggles.Length; i++)
        {
            if (toggles[i].transform.localEulerAngles.x == togPos[i]) launch = false;
        }

        if (launch)
        {
            startLight.GetComponent<Light>().enabled = false;
            doorLight.GetComponent<Light>().enabled = true;
            doorLight.GetComponent<AudioSource>().Play();
            timer.GetComponent<Canvas>().enabled = true;

            AudioSource fas = GetComponent<AudioSource>();

            fas.clip = speeches[3];
            fas.Play();

            counter = 43f;
            stage++;
        }
    }

    private void stageCountdown()
    {
        lockSwitches(true);

        int lastCounter = (int)counter;
        counter = Mathf.Max(counter - Time.deltaTime, 0f);

        if (counter > 40f) return;

        sec1.GetComponent<Image>().sprite = bubbleFont[(int)(counter / 10)];
        sec2.GetComponent<Image>().sprite = bubbleFont[(int)(counter % 10)];

        mil1.GetComponent<Image>().sprite = bubbleFont[(int)((counter * 10) % 10)];
        mil2.GetComponent<Image>().sprite = bubbleFont[(int)((counter * 100) % 10)];

        if ( lastCounter != ((int)counter) )
        {
            timer.GetComponent<AudioSource>().Play();

            if ( (int) counter == 19 )
            {
                doorLeft.GetComponent<AudioSource>().Play();
                doorRight.GetComponent<AudioSource>().Play();
            }
        }

        float doorPerc = (10 - Mathf.Clamp(counter - 10f, 0f, 10f)) / 10f;
        doorLeft.transform.localEulerAngles = new Vector3(0f, 0f, -15f * doorPerc);
        doorRight.transform.localEulerAngles = new Vector3(0f, 0f, 15f * doorPerc);

        if ( counter <= 0 )
        {
            sled.GetComponent<Rigidbody>().isKinematic = false;
            sec2.GetComponent<Image>().sprite = bubbleFont[0];
            stage++;
        }
    }

    private void stageRunning()
    {
        lockSwitches(true);

        Vector3 pos = sled.transform.position;
        if (pos.z > 3015f)
        {
            Rigidbody rb = sled.GetComponent<Rigidbody>();
            rb.velocity = Vector3.zero;
            rb.isKinematic = true;

            pos.z = 3015f;
            sled.transform.position = pos;

            gameover.GetComponent<deathWatch>().killPlayer("Crashed and burned!");
        }
    }

    private void stageEject()
    {
        Vector3 pos = sled.transform.position;
        if (pos.z > 3015f)
        {
            Rigidbody rb = sled.GetComponent<Rigidbody>();
            rb.velocity = Vector3.zero;
            rb.isKinematic = true;

            pos.z = 3015f;
            sled.transform.position = pos;
        }

        counter = Mathf.Max(counter - Time.deltaTime, 0f);

        if ( counter == 0f )
        {
            gameover.GetComponent<deathWatch>().victoryPlayer("Saftey test passed!");
        }
    }
}