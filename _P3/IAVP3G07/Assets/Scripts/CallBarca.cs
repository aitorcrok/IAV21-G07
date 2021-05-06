using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallBarca : MonoBehaviour
{
    public bool isNextTarget; //indica si va o viene
    public Transform nextTarget;
    public GameObject barca;
    GoToTarget gtt;
    bool isColiding = false;
    private void Start()
    {
        gtt = barca.GetComponent<GoToTarget>();
    }
    private void OnTriggerExit(Collider other)
    {
        isColiding = false;
    }
    // Start is called before the first frame update
    private void OnTriggerStay(Collider other)
    {
        //entra al ontriggerstay dos veces no se por que entonces hago esto para que solo haga el proceso una vez

        if (Input.GetKey(KeyCode.Space))
        {
            if (isColiding) return;
            isColiding = true;
            if (isNextTarget)
            {
                Debug.Log("ven perro");
                gtt.setTarget(this.transform.GetChild(0));
                isNextTarget = false;
            }
            else
            {
                Debug.Log("llévame contigo, perro");
                Vector3 quecoñopasaaqui = this.transform.GetChild(0).position;
                Debug.Log("Ve a " + nextTarget.position + " desde " + quecoñopasaaqui);
                gtt.setTarget(nextTarget);
                isNextTarget = true;
            }
        }
    }
}
