using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace UCM.IAV.Navegacion
{
    public class TeseoController : MonoBehaviour
    {
        UCM.IAV.Movimiento.ControlJugador contJug;
        PathFollower pathFollower;
        Path p;
        public bool smooth = false;
        bool state = false;
        List<Vertex> path;
        public Graph graph;
        public GameObject lineContainer;
        public GameObject line;
        GameObject exit;

        public Text suavizadoText;
        // Start is called before the first frame update
        void Start()
        {
            contJug = GetComponent<UCM.IAV.Movimiento.ControlJugador>();
            pathFollower = GetComponent<PathFollower>();
            p = GetComponent<Path>();
        }

        // Update is called once per frame
        void Update()
        {
            // Comprueba el input de teclado para llamar al manager
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyUp(KeyCode.Space)) hiloAriadna();
            if (Input.GetKeyDown(KeyCode.S)) Suavizado();

            if (state)
            {
                Ray ray = new Ray(transform.position, -Vector3.up);
                RaycastHit[] hits;
                hits = Physics.RaycastAll(ray, 1);
                GameObject srcObj = hits[0].collider.gameObject;
                float t;
                path = graph.GetPathAstar(srcObj, exit, out t, graph.HeurPeso); // Se pasa la heurística
                if (smooth)
                    path = graph.Smooth(path); // Suavizar el camino, una vez calculado
                GraphGrid g = (GraphGrid)graph;
                g.UpdateTimeText(t);

                List<Vertex> aux = path;
                aux.Reverse();
                p.SetNodes(aux);

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
        void Suavizado()
        {
            smooth = !smooth;
            suavizadoText.text = "Suavizado:\n" + smooth;
        }
        void hiloAriadna()
        {
            contJug.enabled = !contJug.enabled;
            pathFollower.enabled = !pathFollower.enabled;
            state = !state;
            if (state)
            {

                Ray ray = new Ray(Vector3.up, -Vector3.up);
                RaycastHit[] hits;
                hits = Physics.RaycastAll(ray, 1);
                exit = hits[0].collider.gameObject;
            }

            else 
            {
                foreach (Transform child in lineContainer.transform)
                {
                    Destroy(child.gameObject);
                }
            }
        }
    }

}
