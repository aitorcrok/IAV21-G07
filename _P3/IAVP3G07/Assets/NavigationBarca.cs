using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavigationBarca: MonoBehaviour
{
    //public Transform initialTarget;
    //public float speed;
    CallBarca call;

    private void Start()
    {
        call = GetComponentInParent<CallBarca>();
    }


    private void OnCollisionEnter(Collision collision)
    {
        //desactiva navmesh
        if (collision.gameObject.tag == "fantasma")
        {
            Debug.Log("eie");
            collision.gameObject.GetComponent<NavMeshAgent>().enabled = false;
            collision.gameObject.GetComponent<Bolt.StateMachine>().enabled = false;
            //Si la barca no esta, llama para que se la traigan
            if (call.isNextTarget) call.TraeBarca();
            else
            {
                collision.transform.position = call.puerto.position + Vector3.up * 3 / 2;
                call.MandaBarca();
            }
        }

    }

    private void OnCollisionStay(Collision collision)
    {
        //Si la barca esta, se sube en ella y la manda al otro lado
        if (collision.gameObject.tag == "fantasma")
        {
            if (!call.isNextTarget)
            {
                collision.transform.position = call.puerto.position + Vector3.up * 3 / 2;
                call.MandaBarca();
            }
        }
    }
}