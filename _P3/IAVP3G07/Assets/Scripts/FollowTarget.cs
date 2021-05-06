using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    public GameObject target;
    Vector3 deltaPos;

    // Start is called before the first frame update
    void Start()
    {
        deltaPos = transform.position - target.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(target.transform.position.x + deltaPos.x, transform.position.y, target.transform.position.z + deltaPos.z);
    }
}
