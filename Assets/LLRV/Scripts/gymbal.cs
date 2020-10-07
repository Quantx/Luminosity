using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gymbal : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.eulerAngles = new Vector3(-90f, 0f, 0f);
        transform.localEulerAngles = new Vector3(0f, transform.localEulerAngles.y, 0f);
    }
}
