using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class seatEject : MonoBehaviour
{
    public GameObject llrv;
    public GameObject leftHandle;
    public GameObject rightHandle;
    public GameObject toggle;
    private float togVal;
    private float thrustStart = -1f;
    public GameObject battery;
    private bool charged = false;

    public GameObject parachute;
    private bool chuteDeploy = false;
    public AudioClip ejectSfx;

    public GameObject gameover;

    // Start is called before the first frame update
    void Start()
    {
        togVal = toggle.transform.localEulerAngles.x;
    }

    // Update is called once per frame
    void Update()
    {
        float speed;

        if ( transform.parent == null )
        {
            speed = GetComponent<Rigidbody>().velocity.magnitude;
        }
        else
        {
            speed = llrv.GetComponent<Rigidbody>().velocity.magnitude;
        }

        AudioSource fas = GetComponent<AudioSource>();
        fas.volume = Mathf.Clamp(speed, 0f, 10f) / 10f;

        float lAng = leftHandle.transform.localEulerAngles.x;
        float rAng = rightHandle.transform.localEulerAngles.x;

        if (lAng > 180) lAng -= 360;
        if (rAng > 180) rAng -= 360;

        float volts = battery.GetComponent<power>().getVolts();

        if (volts >= 12 && !charged)
        {
            charged = true;
            // Accidental ejection
            if (togVal != toggle.transform.localEulerAngles.x)
            {
                lAng = rAng = -90f;
            }
        }

        if (transform.parent != null && Vector3.Dot(transform.forward, Vector3.down) < 0 && (lAng < -45f && rAng < -45f) && togVal != toggle.transform.localEulerAngles.x && charged)
        {
            transform.parent = null;
            transform.position += transform.forward * 2f;

            Rigidbody rb = gameObject.AddComponent<Rigidbody>();
            rb.mass = 140; // Weight of the seat & occupant

            fas.PlayOneShot(ejectSfx, 1);
            GetComponent<ParticleSystem>().Play();

            thrustStart = Time.time;
        }

        if (thrustStart > 0 && Time.time >= thrustStart + 5f && !chuteDeploy)
        {
            Rigidbody rb = gameObject.GetComponent<Rigidbody>();

            rb.velocity = Vector3.zero;
            rb.useGravity = false;

            chuteDeploy = true;

            Vector3 offset = new Vector3(0f, -0.35f, 0.9f);

            Rigidbody prb = parachute.GetComponent<Rigidbody>();

            parachute.GetComponent<ConstantForce>().force += new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, -1f)).normalized;
            parachute.transform.position = prb.position = transform.TransformPoint(offset);
            prb.isKinematic = false;

            CharacterJoint cj = gameObject.AddComponent<CharacterJoint>();
            cj.connectedBody = prb;
            cj.anchor = offset;
        }

        if (thrustStart > 0 && Time.time >= thrustStart + 10f)
        {
            if (SceneManager.GetActiveScene().name == "Tutorial")
            {
                gameover.GetComponent<deathWatch>().victoryPlayer("Saftey Test Passed!");
            }
            else
            {
                gameover.GetComponent<deathWatch>().killPlayer("Test vehicle lost");
            }
        }
    }

    void FixedUpdate()
    {
        if (thrustStart > 0 && Time.time < thrustStart + 0.5f)
        {
            Rigidbody rb = gameObject.GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * 14f * Physics.gravity.magnitude * rb.mass);
        }
    }
}
