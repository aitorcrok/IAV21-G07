using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bolt;

public class Palanca : MonoBehaviour
{
    public GameObject lampara;
    public SceneVariables sv;
    public string LamparaVariableName; //nombre de la variable de bolt
    bool palanca = false; //si esta en false es que se puede activar

    private void OnTriggerStay(Collider other)
    {
        if(!palanca && Input.GetKeyDown(KeyCode.Space)) { 
            palanca = true; //palanca activada, se desactivara cuando se suba la lampara
            Debug.Log("jaja tire la " + LamparaVariableName);
            Variables.Object(sv).Set(LamparaVariableName, false);
            lampara.GetComponent<RestoreLampara>().setRestore(true); //se puede salvar la lampara
        }

    }
    public void setPalanca(bool b) { palanca = b; }
}
