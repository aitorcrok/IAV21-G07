using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UCM.IAV.Navegacion
{
    public class Minotauro : MonoBehaviour
    {
        UCM.IAV.Movimiento.EvitarMuros evitarMuros;
        UCM.IAV.Movimiento.Merodeo merodeo;
        Path p;
        List<Vertex> path;
        public Graph graph;

        public GameObject player;

        private void Start()
        {
            evitarMuros = GetComponent<UCM.IAV.Movimiento.EvitarMuros>();
            merodeo = GetComponent<UCM.IAV.Movimiento.Merodeo>();
        }

        private void Update()
        {
            Ray ray = new Ray(transform.position, -Vector3.up);
            RaycastHit[] hits;
            hits = Physics.RaycastAll(ray, 1);
            GameObject srcObj = hits[0].collider.gameObject;
            path = graph.GetPathAstar(srcObj, player, null); // Se pasa la heurística
        }
        public void EmpiezaPerseguir()
        {
            evitarMuros.enabled = true;
            merodeo.enabled = false;
            Debug.Log(evitarMuros.enabled);

        }

        public void ParaPerseguir()
        {
            evitarMuros.enabled = false;
            merodeo.enabled = true;
        }
    }
}

