using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class speedometer : MonoBehaviour
{
    public GameObject llrv;
    public GameObject target;

    public GameObject[] speedWheels;
    public GameObject[] distWheels;

    private Vector3[] speedStart;
    private Vector3[] distStart;

    // Start is called before the first frame update
    void Start()
    {
        speedStart = new Vector3[3];
        distStart = new Vector3[3];

        for (int i = 0; i < 3; i++)
        {
            speedStart[i] = speedWheels[i].transform.localEulerAngles;
            distStart[i] = distWheels[i].transform.localEulerAngles;
        }
    }

    // Update is called once per frame
    void Update()
    {
        int i;

        float speed = llrv.GetComponent<Rigidbody>().velocity.magnitude;
        float dist = Vector3.Distance(llrv.transform.position, target.transform.position);

        speed = Mathf.Min(speed * 3.281f, 999);
        dist = Mathf.Min(dist * 0.3281f, 999);

        Vector3[] swr = new Vector3[3];
        Vector3[] dwr = new Vector3[3];

        for ( i = 0; i < 3; i++)
        {
            swr[i] = speedStart[i];
            dwr[i] = distStart[i];
        }

        swr[0].z = speedStart[0].z - ((speed % 10) / 10) * 360;
        swr[1].z = speedStart[1].z - (((speed / 10) % 10) / 10) * 360;
        swr[2].z = speedStart[2].z - (((speed / 100) % 10) / 10) * 360;

        swr[2].z = Mathf.Max(swr[2].z, speedStart[2].z - 325f);

        dwr[0].z = distStart[0].z - ((dist % 10) / 10) * 360;
        dwr[1].z = distStart[1].z - (((dist / 10) % 10) / 10) * 360;
        dwr[2].z = distStart[2].z - (((dist / 100) % 10) / 10) * 360;

        dwr[2].z = Mathf.Max(dwr[2].z, distStart[2].z - 325f);

        for ( i = 0; i < 3; i++)
        {
            speedWheels[i].transform.localEulerAngles = swr[i];
            distWheels[i].transform.localEulerAngles = dwr[i];
        }
    }
}
