using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UCM.IAV.Navegacion
{
    public class DeteccionCono : MonoBehaviour
    {
        public GameObject agente;
        public GameObject objetivo;
        void Start()
        {
            if (agente == null) agente = gameObject;
        }
        public void OnTriggerStay(Collider coll)
        {
            if (!coll.gameObject.Equals(objetivo))
            {
                this.GetComponentInParent<Minotauro>().ParaPerseguir();
                return;
            }
            Vector3 agentePos = agente.transform.position;
            Vector3 objetivoPos = objetivo.transform.position;
            Vector3 direction = objetivoPos - agentePos;

            float length = direction.magnitude;
            direction.Normalize();
            Ray ray = new Ray(agentePos, direction);

            RaycastHit[] hits;
            hits = Physics.RaycastAll(ray, length);

            int i;
            for (i = 0; i < hits.Length; i++)
            {
                GameObject hitObj;
                hitObj = hits[i].collider.gameObject;
                if (hitObj == objetivo)
                    break;
            }
            if (i >= hits.Length) this.GetComponentInParent<Minotauro>().ParaPerseguir();
            else this.GetComponentInParent<Minotauro>().EmpiezaPerseguir();
        }
    }

}