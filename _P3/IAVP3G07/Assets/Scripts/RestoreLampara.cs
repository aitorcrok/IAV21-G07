using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bolt;
public class RestoreLampara : MonoBehaviour
{
    bool aire = true; //true = lampara en el aire
    public float speed;
    Vector3 originalPos;
    public GameObject palanca;
    public SceneVariables sv;
    public string variableName;
    private void Start()
    {
        originalPos = transform.position;
    }
    private void OnTriggerStay(Collider other)
    {
        if (!aire && Input.GetKey(KeyCode.Space))
        {
            aire = true;
            Debug.Log(variableName + " restaurada");
            Variables.Object(sv).Set(variableName, true);
        }
    }
    private void Update()
    {
        if (aire) {
            if (transform.position.y < originalPos.y)
                transform.position = originalPos;
            else
                palanca.GetComponent<Palanca>().setPalanca(false);
        }    
    }
    public void setRestore(bool b) { aire = b; }
}
