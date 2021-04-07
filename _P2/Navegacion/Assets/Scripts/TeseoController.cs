using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace UCM.IAV.Navegacion
{
    public class TeseoController : MonoBehaviour
    {
        UCM.IAV.Movimiento.ControlJugador contJug;
        public bool smooth = false;
        bool state = false;
        List<Vertex> path;
        public Graph graph;
        public GameObject lineContainer;
        public GameObject line;
        GameObject exit;
        float t;

        // Start is called before the first frame update
        void Start()
        {
            contJug = GetComponent<UCM.IAV.Movimiento.ControlJugador>();
        }

        // Update is called once per frame
        void Update()
        {
            // Comprueba el input de teclado para llamar al manager
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyUp(KeyCode.Space)) hiloAriadna();
            if (Input.GetKeyDown(KeyCode.S)) smooth = !smooth;

            if (state)
            {
                Ray ray = new Ray(transform.position, -Vector3.up);
                RaycastHit[] hits;
                hits = Physics.RaycastAll(ray, 1);
                GameObject srcObj = hits[0].collider.gameObject;
                path = graph.GetPathAstar(srcObj, exit, null); // Se pasa la heurística
                if (smooth)
                    path = graph.Smooth(path); // Suavizar el camino, una vez calculado

                foreach(Transform child in lineContainer.transform)
                {
                    Destroy(child.gameObject);
                }
                GameObject l;
                LineRenderer lr;
                int i;
                for (i = 0; i < path.Count - 1; i++)
                {
                    l = Instantiate(line, lineContainer.transform);
                    lr = l.GetComponent<LineRenderer>();
                    lr.SetPosition(0, new Vector3(path[i].transform.position.x, path[i].transform.position.z, -0.1f));
                    lr.SetPosition(1, new Vector3(path[i + 1].transform.position.x, path[i + 1].transform.position.z, -0.1f));
                }
            }
        }

        void hiloAriadna()
        {
            contJug.enabled = !contJug.enabled;
            state = !state;
            if (state)
            {

                Ray ray = new Ray(Vector3.up, -Vector3.up);
                RaycastHit[] hits;
                hits = Physics.RaycastAll(ray, 1);
                exit = hits[0].collider.gameObject;
            }
        }
    }

}
