using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class crossPointer : MonoBehaviour
{
    public GameObject llrv;
    // Smaller value
    public GameObject needle1fwd;
    public GameObject needle1lat;
    // Larger value
    public GameObject needle2fwd;
    public GameObject needle2lat;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Rigidbody rb = llrv.GetComponent<Rigidbody>();

        Vector3 vel = llrv.transform.InverseTransformDirection( rb.velocity );

        float fwd1 = Mathf.Clamp(vel.y * 3.281f / 20f, -1, 1);
        float fwd2 = Mathf.Clamp(vel.y * 3.281f / 200f, -1, 1);

        float lat1 = Mathf.Clamp(vel.x * -3.281f / 20f, -1, 1);
        float lat2 = Mathf.Clamp(vel.x * -3.281f / 200f, -1, 1);

        Vector3 n1f = needle1fwd.transform.localPosition;
        n1f.y = fwd1 * 0.03f;
        needle1fwd.transform.localPosition = n1f;

        Vector3 n1l = needle1lat.transform.localPosition;
        n1l.x = lat1 * 0.03f;
        needle1lat.transform.localPosition = n1l;

        Vector3 n2f = needle2fwd.transform.localPosition;
        n2f.y = fwd2 * 0.03f;
        needle2fwd.transform.localPosition = n2f;

        Vector3 n2l = needle2lat.transform.localPosition;
        n2l.x = lat2 * 0.03f;
        needle2lat.transform.localPosition = n2l;
    }
}
