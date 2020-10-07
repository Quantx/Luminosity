using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class altRate : MonoBehaviour
{
    public GameObject llrv;
    public GameObject radar;
    public GameObject altUI;
    public GameObject rateUI;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float alt = 2000f;

        int layerMask = ~(1 << 8);

        RaycastHit hit;
        if (Physics.Raycast(radar.transform.position, radar.transform.forward, out hit, alt, layerMask))
        {
            alt = hit.distance;
        }

        alt *= 3.281f; // Convert to feet

        alt /= 1000f;

        Vector3 aup = altUI.transform.localPosition;
        aup.y = Mathf.Clamp(0.25f + alt * -0.5f, -0.285f, 0.25f);
        altUI.transform.localPosition = aup;

        Rigidbody rb = llrv.GetComponent<Rigidbody>();

        float rate = rb.velocity.y;

        rate *= 3.281f; // Convert to ft/s

        rate /= 10f;

        Vector3 rup = rateUI.transform.localPosition;
        rup.y = Mathf.Clamp(rate * -0.04f, -0.285f, 0.285f);
        rateUI.transform.localPosition = rup;
    }
}
