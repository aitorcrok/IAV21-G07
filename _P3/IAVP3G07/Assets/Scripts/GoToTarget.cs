using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GoToTarget : MonoBehaviour
{
    public Transform initialTarget;
    public float speed;

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, initialTarget.position, speed * Time.deltaTime);
        if(transform.position == initialTarget.position)
        {
            if(transform.childCount > 0 && transform.GetChild(0).tag == "fantasma")
            {
                NavMeshAgent ag = transform.GetChild(0).GetComponent<NavMeshAgent>();
                ag.enabled = true;
            }
        }
    }

    public void setTarget(Transform t) { initialTarget = t; }

    private void OnCollisionEnter(Collision collision)
    {
        collision.gameObject.transform.SetParent(this.transform);
        if(collision.gameObject.tag == "fantasma")
        {
            NavMeshAgent ag = collision.gameObject.GetComponent<NavMeshAgent>();
            ag.enabled = false;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        collision.gameObject.transform.SetParent(null);
    }
}
