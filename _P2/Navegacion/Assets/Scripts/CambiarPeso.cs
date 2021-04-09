using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace UCM.IAV.Navegacion
{
    public class CambiarPeso : MonoBehaviour
    {


        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Vertex")
            {
                other.GetComponent<Vertex>().peso = 5;
                //Debug.Log(other.GetComponent<Vertex>().id + " peso = 5");
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.tag == "Vertex")
            {
                other.GetComponent<Vertex>().peso = 1;
                //Debug.Log(other.GetComponent<Vertex>().id + " peso = 1");
            }
        }
    }
}
