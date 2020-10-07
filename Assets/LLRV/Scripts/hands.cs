using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class hands : MonoBehaviour
{
    public GameObject llrv;

    public SteamVR_Action_Boolean pinch;
    public SteamVR_Action_Boolean flip;
    public SteamVR_Action_Boolean trans_fire;
    public SteamVR_Action_Vector2 trans_dir;
    public SteamVR_Input_Sources hand;
    public float buttonRayLen = 0.5f;

    public GameObject gameover;

    private List<GameObject> grabObjs;
    private GameObject flipObj = null;
    private Quaternion grabAngle;
    private string[] grabTags = { "eject_handle", "throttle", "flight_stick" };

    // Start is called before the first frame update
    void Start()
    {
        grabObjs = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if ( flip.GetStateDown(hand) )
        {
            if (flipObj != null)
            {
                Vector3 ang = flipObj.transform.localEulerAngles;
                ang.x *= -1;
                flipObj.transform.localEulerAngles = ang;
            }
        }

        if (pinch.GetStateDown(hand))
        {
            grabAngle = transform.localRotation;
        }
        else if (pinch.GetStateUp(hand))
        {
            foreach (GameObject gobj in grabObjs)
            {
                if (gobj.tag == "flight_stick") gobj.transform.localEulerAngles = Vector3.zero;
            }
        }

        // Can't do anything if we're dead
        if (gameover.GetComponent<deathWatch>().isDead()) return;

        Vector3 rayOrg = transform.position;
        //rayOrg += transform.forward * 0.2f;

        int layerMask = (1 << 5);
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, buttonRayLen, layerMask) && hit.collider.gameObject.tag == "switch")
        {
            if (flipObj != hit.collider.gameObject)
            {
                if (flipObj != null) flipObj.GetComponent<Outline>().enabled = false;

                flipObj = hit.collider.gameObject;
                flipObj.GetComponent<Outline>().enabled = true;
            }
        }
        else
        {
            if ( flipObj != null )
            {
                flipObj.GetComponent<Outline>().enabled = false;
                flipObj = null;
            }
        }

        if (!pinch.GetState(hand)) return;

        foreach ( GameObject gobj in grabObjs )
        {
            if ( gobj.tag == "eject_handle" )
            {
                Vector3 targetPostition = gobj.transform.InverseTransformPoint(transform.position);

                targetPostition.x = 0;

                gobj.transform.LookAt(gobj.transform.TransformPoint(targetPostition));

                Vector3 cor = new Vector3(gobj.transform.localEulerAngles.x, 0f, 0f);

                if (cor.x > 180) cor.x -= 360;

                if (cor.x < -90) cor.x = -90;
                if (cor.x >   5) cor.x =   5;

                gobj.transform.localEulerAngles = cor;
            }
            else if ( gobj.tag == "throttle" )
            {
                Vector3 targetPostition = gobj.transform.InverseTransformPoint(transform.position);

                targetPostition.x = 0;

                gobj.transform.LookAt(gobj.transform.TransformPoint(targetPostition));

                Vector3 cor = new Vector3(gobj.transform.localEulerAngles.x, 0f, 0f);

                if (cor.x > 180) cor.x -= 360;

                if (cor.x < -45) cor.x = -45;
                if (cor.x >  45) cor.x =  45;

                gobj.transform.localEulerAngles = cor;
            }
            else if ( gobj.tag == "flight_stick" )
            {
                gobj.transform.localRotation = (grabAngle * Quaternion.Inverse(transform.localRotation));// * gobj.transform.localRotation;
                             
                Vector3 cor = gobj.transform.localEulerAngles;

                if (cor.x > 180) cor.x -= 360;
                if (cor.y > 180) cor.y -= 360;
                if (cor.z > 180) cor.z -= 360;

                if (cor.x < -45) cor.x = -45;
                if (cor.x > 45) cor.x = 45;

                float swp = cor.y;
                cor.y = -cor.z;
                cor.z = -swp;

                if (cor.y < -45) cor.y = -45;
                if (cor.y > 45) cor.y = 45;

                if (cor.z < -45) cor.z = -45;
                if (cor.z > 45) cor.z = 45;

                gobj.transform.localEulerAngles = cor;

            }
        }
    }

    void FixedUpdate()
    {
        // Can't do anything if we're dead
        if (gameover.GetComponent<deathWatch>().isDead()) return;

        foreach (GameObject gobj in grabObjs)
        {
            Rigidbody rb = llrv.GetComponent<Rigidbody>();

            if (gobj.tag == "throttle")
            {
                if (trans_fire.GetState(hand))
                {
                    Vector3 dir = new Vector3(-trans_dir.axis.x, trans_dir.axis.y, 0f);

                    if ( dir.magnitude <= 0.1 )
                    {
                        Vector3 cancel = -rb.velocity;
                        cancel.z = 0;

                        rb.AddForce(cancel * 4400f);
                    }
                    else
                    {
                        rb.AddRelativeForce(dir * 4400f);
                    }
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (System.Array.IndexOf(grabTags, other.gameObject.tag) >= 0)
        {
            grabObjs.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (grabObjs.Remove(other.gameObject) && other.gameObject.tag == "flight_stick" )
        {
            other.gameObject.transform.localEulerAngles = Vector3.zero;
        }
    }
}