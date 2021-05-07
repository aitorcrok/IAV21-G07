using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bolt;
public class RestoreLampara : MonoBehaviour
{
    bool destroyed = false; //true = lampara en el aire
    public float speed;
    public GameObject palanca;
    public SceneVariables sv;
    public string variableName;
    Vector3 originalPos;
    Vector3 floorPos;
    private void Start()
    {
        originalPos = transform.position;
        floorPos = new Vector3(transform.position.x, -0.2f, transform.position.z);
    }
    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKey(KeyCode.Space))
        {
            //destroyed = false;
            Debug.Log(variableName + " restaurada");
            Variables.Object(sv).Set(variableName, true);
            palanca.GetComponent<Palanca>().setPalanca(false); // se puede volver a tirar la lampara (reactivar palanca)
        }
    }
    private void Update()
    {
        if (Variables.Object(sv).Get<bool>(variableName))
            transform.position = Vector3.MoveTowards(transform.position, originalPos, Time.deltaTime * speed);
        else
            transform.position = Vector3.MoveTowards(transform.position, floorPos, Time.deltaTime * speed);
    }
    public void setRestore(bool b) { destroyed = b; }
}
