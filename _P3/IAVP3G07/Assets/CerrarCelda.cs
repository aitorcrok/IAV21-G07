using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bolt;
public class CerrarCelda : MonoBehaviour
{
    public GameObject sceneVariables;
    Vector3 target;
    SceneVariables sv;
    Vector3 originalPos;
    // Start is called before the first frame update
    void Start()
    {
        sv = sceneVariables.GetComponent<SceneVariables>();
        originalPos = transform.position;
        target = new Vector3(originalPos.x, 8.4f, originalPos.z);
    }

    // Update is called once per frame
    void Update()
    {
        if (Variables.Object(sv).Get<bool>("SenoraEncerrada"))
            Vector3.MoveTowards(transform.position,target, 7 * Time.deltaTime);
        else
            Vector3.MoveTowards(transform.position, originalPos, 7 * Time.deltaTime);


    }
}
