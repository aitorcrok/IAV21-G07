using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallBarca : MonoBehaviour
{
    public bool isNextTarget; //indica si va o viene
    public Transform nextTarget, puerto;
    public GameObject barca;
    GoToTarget gtt;
    bool isColliding = false;
    private void Start()
    {
        gtt = barca.GetComponent<GoToTarget>();
        puerto = this.transform.GetChild(0);
    }
    private void OnTriggerExit(Collider other)
    {
        isColliding = false;
    }
    // Start is called before the first frame update
    private void OnTriggerStay(Collider other)
    {
        //entra al ontriggerstay dos veces no se por que entonces hago esto para que solo haga el proceso una vez
        if (other.gameObject.tag == "Player" && Input.GetKey(KeyCode.Space))
        {
            //if (isColliding) return;
            //isColliding = true;
            Debug.Log("laputamadre");
            if (isNextTarget)
            {
                TraeBarca();
            }
            else
            {
                MandaBarca();
                other.transform.position = puerto.position + Vector3.up * 3 / 2;
            }
        }
    }
    public void TraeBarca()
    {
        Debug.Log("ven perro");
        gtt.setTarget(puerto);
    }
    public void MandaBarca()
    {
        Debug.Log("llévame contigo, perro");
        Vector3 quecoñopasaaqui = puerto.position;
        Debug.Log("Ve a " + nextTarget.position + " desde " + quecoñopasaaqui);
        gtt.setTarget(nextTarget);
    }
}
