using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallBarca : MonoBehaviour
{
    public Transform nextTarget;
    public GameObject barca;
    GoToTarget gtt;
    private void Start()
    {
        gtt = barca.GetComponent<GoToTarget>();
    }
    // Start is called before the first frame update
    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKey(KeyCode.Space))
            gtt.setTarget(nextTarget);
    }
}
