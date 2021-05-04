using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bolt;

public class Palanca : MonoBehaviour
{
    public GameObject lampara;
    public float speed;
    public SceneVariables sv;
    public string LamparaVariableName; //nombre de la variable de bolt
    bool palanca = false; //si esta en false es que se puede activar
    Vector3 floorPos;//posicion del suelo
    private void Start()
    {
        floorPos = new Vector3(lampara.transform.position.x, -4.5f, lampara.transform.position.z);
    }
    private void OnTriggerStay(Collider other)
    {
        if(!palanca && Input.GetKeyDown(KeyCode.Space)) { 
            palanca = true; //palanca activada, se desactivara cuando se suba la lampara
            Debug.Log("jaja tire la " + LamparaVariableName);
            Variables.Object(sv).Set(LamparaVariableName, false);
        }

    }
    private void Update()
    {
        if (palanca)
        {
            if (lampara.transform.position.y > floorPos.y)
                lampara.transform.position = floorPos;
            else
                lampara.GetComponent<RestoreLampara>().setRestore(false); ;
        }
    }
    public void setPalanca(bool b) { palanca = b; }
}
