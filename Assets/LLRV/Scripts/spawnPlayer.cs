using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnPlayer : MonoBehaviour
{
    public GameObject llrv;
    public GameObject gameover;
    public GameObject cameraRig;

    // Start is called before the first frame update
    void Start()
    {
        GameObject player = Instantiate(cameraRig, transform);

        foreach (Transform robj in player.transform)
        {
            if (robj.name.StartsWith("Camera"))
            {
                gameover.GetComponent<Canvas>().worldCamera = robj.GetComponent<Camera>();
            }
            else if (robj.name.StartsWith("Controller"))
            {
                hands cont = robj.GetComponent<hands>();
                cont.gameover = gameover;
                cont.llrv = llrv;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
