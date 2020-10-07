using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class winZone : MonoBehaviour
{
    public GameObject llrv;
    public GameObject gameover;
    public GameObject ejectSeat;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 wpos = transform.position;
        Vector3 lpos = llrv.transform.position;
        lpos.y = wpos.y = 0f;

        float dst = Vector3.Distance(wpos, lpos);

        if ( dst < 5f )
        {
            Rigidbody rb = llrv.GetComponent<Rigidbody>();

            if ( rb.velocity.magnitude < 1f && Vector3.Dot(transform.forward, Vector3.up) > 0)
            {
                BoxCollider bc = llrv.GetComponent<BoxCollider>();
                if ( Physics.Raycast(llrv.transform.TransformPoint(bc.center), Vector3.down, bc.bounds.extents.z + 0.1f) )
                {
                    if (ejectSeat.transform.parent != null)
                        gameover.GetComponent<deathWatch>().victoryPlayer("Safely Landed");
                }
            }
        }
    }
}
