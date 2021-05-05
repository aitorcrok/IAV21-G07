using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToTarget : MonoBehaviour
{
    public Transform initialTarget;
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, initialTarget.position, 3 * Time.deltaTime);
    }

    public void setTarget(Transform t) { initialTarget = t; }

    private void OnCollisionEnter(Collision collision)
    {
        collision.gameObject.transform.SetParent(this.transform);
    }
    private void OnCollisionExit(Collision collision)
    {
        collision.gameObject.transform.SetParent(null);
    }
}
