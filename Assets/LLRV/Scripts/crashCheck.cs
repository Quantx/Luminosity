using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class crashCheck : MonoBehaviour
{
    public GameObject gameover;
    public GameObject ejectSeat;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        BoxCollider bc = GetComponent<BoxCollider>();
        if ( Physics.Raycast(transform.TransformPoint(bc.center), Vector3.down, bc.bounds.extents.z + 0.1f) && Vector3.Dot(transform.forward, Vector3.down) > 0 )
        {
            if ( ejectSeat.transform.parent != null )
                gameover.GetComponent<deathWatch>().killPlayer("Landed upside down!");
        }

        if ( transform.position.x < -500f || transform.position.x > 500f
          || transform.position.z < -500f || transform.position.z > 500f )
        {
            if (ejectSeat.transform.parent != null)
                gameover.GetComponent<deathWatch>().killPlayer("Flew too far away!");
        }
    }
}
