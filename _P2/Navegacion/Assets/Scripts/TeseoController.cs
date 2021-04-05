using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace UCM.IAV.Navegacion
{
    public class TeseoController : MonoBehaviour
    {
        UCM.IAV.Movimiento.ControlJugador contJug;
        bool smooth = false;
        bool state = false;
        List<Vertex> path;
        public Graph graph;

        GameObject exit;

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
            if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyUp(KeyCode.S)) smooth = !smooth;

            if (state)
            {
                Ray ray = new Ray(transform.position, -Vector3.up);
                RaycastHit[] hits;
                hits = Physics.RaycastAll(ray, 1);
                GameObject srcObj = hits[0].collider.gameObject;
                path = graph.GetPathAstar(srcObj, exit, null); // Se pasa la heurística
                if (smooth)
                    path = graph.Smooth(path); // Suavizar el camino, una vez calculado

                //idk
                //for(int i = 0; i < path.Count - 1; i++)
                //{
                //    LineRenderer lr = new LineRenderer();
                //    lr.SetPosition(0, path[i].transform.position);
                //    lr.SetPosition(1, path[i+1].transform.position);
                //    lr.SetColors(Color.red, Color.white);
                //}
                //idk


                foreach (Vertex v in path)
                {
                    //dibujar linea



                    Renderer r = v.GetComponent<Renderer>();
                    r.material.color = Color.red;
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
