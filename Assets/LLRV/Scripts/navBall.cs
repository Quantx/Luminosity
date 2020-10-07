using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class navBall : MonoBehaviour
{
    public GameObject llrv;

    public GameObject rateYaw;
    public GameObject rateRoll;
    public GameObject ratePitch;

    public GameObject errorYaw;
    public GameObject errorRoll;
    public GameObject errorPitch;

    private Vector3 lockAngle;

    // Start is called before the first frame update
    void Start()
    {
        lockAngle = transform.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        // Lock Navball to world
        transform.eulerAngles = lockAngle;

        // Process stick
        Vector3 rot = (llrv.transform.rotation * Quaternion.AngleAxis(90, llrv.transform.right)).eulerAngles;

        if (rot.x > 180) rot.x -= 360;
        if (rot.y > 180) rot.y -= 360;
        if (rot.z > 180) rot.z -= 360;

        rot.x /= 180f;
        rot.y /= 180f;
        rot.z /= 180f;

        Vector3 rtp = rateRoll.transform.localPosition;
        rtp.x = rot.y * 0.0275f;
        rateRoll.transform.localPosition = rtp;

        Vector3 ytp = rateYaw.transform.localPosition;
        ytp.x = rot.z * 0.0275f;
        rateYaw.transform.localPosition = ytp;

        Vector3 ptp = ratePitch.transform.localPosition;
        ptp.y = rot.x * 0.0275f;
        ratePitch.transform.localPosition = ptp;

        // Process rotational error
        Vector3 trgt = Quaternion.FromToRotation(Vector3.up, llrv.transform.forward).eulerAngles;

        if (trgt.x > 180) trgt.x -= 360;
        if (trgt.y > 180) trgt.y -= 360;
        if (trgt.z > 180) trgt.z -= 360;

        trgt.x /= 180f;
        trgt.y /= 180f;
        trgt.z /= 180f;

        Vector3 rep = errorRoll.transform.localPosition;
        rep.x = trgt.z * -0.021f;
        errorRoll.transform.localPosition = rep;

        Vector3 yep = errorYaw.transform.localPosition;
        yep.x = trgt.y * 0.021f;
        errorYaw.transform.localPosition = yep;

        Vector3 pep = errorPitch.transform.localPosition;
        pep.y = trgt.x * 0.021f;
        errorPitch.transform.localPosition = pep;
    }
}
