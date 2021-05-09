using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GoToTarget : MonoBehaviour
{
    public Transform target;
    public float speed;
    public CallBarca puertoA, puertoB;
    bool moving = true;

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        if(transform.position == target.position)
        {
            OnArrive();
        }
    }

    public void setTarget(Transform t) { target = t; moving = true; }

    private void OnCollisionEnter(Collision collision)
    {
        collision.transform.SetParent(this.transform);
        //Debug.Log("setparent");
    }
    private void OnCollisionExit(Collision collision)
    {
        collision.transform.SetParent(null);
        //Debug.Log("setparent a null");

    }
    private void OnArrive()
    {
        if (moving)
        {
            //Actualiza el puerto destino al llegar
            puertoA.isNextTarget = !puertoA.isNextTarget;
            puertoB.isNextTarget = !puertoB.isNextTarget;
            moving = false;

            if (transform.childCount > 0)
            {
                //Coloca al pasajero en el puerto
                //TODO Esto a veces se queda atascado, el plan seria no encajarlo en el puerto
                if (!puertoA.isNextTarget) transform.GetChild(0).position = puertoA.gameObject.transform.position;
                if (!puertoB.isNextTarget) transform.GetChild(0).position = puertoB.gameObject.transform.position;

                //Reactiva la Navmesh del fantasma
                if (transform.GetChild(0).tag == "fantasma")
                {
                    //Debug.Log("aia");

                    //cambia los costes de los nodos haciendo que al que llegues se considere mas barato
                    if (!puertoA.isNextTarget)
                    {
                        puertoB.gameObject.transform.GetChild(1).GetComponentInChildren<OffMeshLink>().costOverride = 10;
                        puertoA.gameObject.transform.GetChild(1).GetComponentInChildren<OffMeshLink>().costOverride = 0;
                    }

                    if (!puertoB.isNextTarget)
                    {
                        puertoA.gameObject.transform.GetChild(1).GetComponentInChildren<OffMeshLink>().costOverride = 10;
                        puertoB.gameObject.transform.GetChild(1).GetComponentInChildren<OffMeshLink>().costOverride = 0;

                    }

                    transform.GetChild(0).GetComponent<NavMeshAgent>().enabled = true;
                    transform.GetChild(0).GetComponent<Bolt.StateMachine>().enabled = true;
                }
            }
        }
    }
}
