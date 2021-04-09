using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace UCM.IAV.Navegacion
{
    public class Minotauro : MonoBehaviour
    {
        Animator animator;
        Path p;
        PathFollower pathFollower;
        UCM.IAV.Movimiento.Agente ag;
        List<Vertex> path;
        public Graph graph;

        public GameObject player;
        public GameObject obj;

        public bool following = false;
        public bool completedPath = true;
        public bool waiting = false;

        public Text stateText;
        Rigidbody rb;

        public float rangeOfVision = 5f;
        public float velocidadMerodeo;
        public float aceleracionMerodeo;
        public float velocidadPersecucion;
        public float aceleracionPersecucion;
        private void Start()
        {
            p = GetComponent<Path>();
            pathFollower = GetComponent<PathFollower>();
            rb = GetComponent<Rigidbody>();
            ag = GetComponent<UCM.IAV.Movimiento.Agente>();
            animator = GetComponentInChildren<Animator>();
            obj = player;
            ag.velocidadMax = velocidadMerodeo;
            ag.aceleracionMax = aceleracionMerodeo;
        }
        private void Update()
        {
            RaycastHit hit;
            Physics.Raycast(transform.position, -Vector3.up, out hit, 2f);
            GameObject srcObj = hit.collider.gameObject;
            detectPlayer();
            if (waiting)
            {
                waiting = true;
                animator.SetBool("Waiting", true);
            }
            else if (following)
            {
                float t;
                obj = player;
                path = graph.GetPathAstar(srcObj, obj, out t, null); // Se pasa la heurística
                path.Reverse();
                p.SetNodes(path);
            }
            else if (completedPath)
            {
                float t;
                GraphGrid g = (GraphGrid)graph;
                completedPath = false;
                Vector3 point = new Vector3(Random.Range(0, g.GetRows()), 0, Random.Range(0, g.GetCols()));
                obj = graph.GetNearestVertex(point).gameObject;
                path = graph.GetPathAstar(srcObj, obj, out t, null); // Se pasa la heurística ( a mirar !)
                path.Reverse();
                p.SetNodes(path);
                stateText.text = "Minotaur State:\nMerodeando";
            }
            else if (Vector3.Distance(obj.transform.position, transform.position) < 1)
            {
                waiting = true;
                animator.SetBool("Waiting", true);
                pathFollower.enabled = false;
                ag.enabled = false;
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                Debug.Log("I am waiting");
                stateText.text = "Minotaur State:\nDescansando";
                StartCoroutine("waitingABit");
            }
            else
            {
                float t;
                path = graph.GetPathAstar(srcObj, obj, out t, null); // Se pasa la heurística ( a mirar !)
                path.Reverse();
                p.SetNodes(path);
            }
        }
        public void EmpiezaPerseguir()
        {
            stateText.text = "Minotaur State:\nPersiguiendo";
            following = true;
            completedPath = true;
            animator.SetBool("Following", true);
            ag.velocidadMax = velocidadPersecucion;
            ag.aceleracionMax = aceleracionPersecucion;
        }

        public void ParaPerseguir()
        {
            following = false;
            animator.SetBool("Following", false);
            ag.velocidadMax = velocidadMerodeo;
            ag.aceleracionMax = aceleracionMerodeo;
        }
        IEnumerator waitingABit()
        {

            yield return new WaitForSeconds(2f);
            waiting = false;
            animator.SetBool("Waiting", false);
            completedPath = true;
            ag.enabled = true;
            pathFollower.enabled = true;
        }
        void detectPlayer()
        {
            if (Vector3.Distance(transform.position, player.transform.position) < rangeOfVision)
            {
                RaycastHit hit;
                Vector3 dir = player.transform.position - transform.position;
                if (Physics.Raycast(transform.position, dir, out hit))
                {
                    if (hit.collider.gameObject == player && !following)
                    {
                        EmpiezaPerseguir();
                    }
                }
                else if (following)
                {
                    ParaPerseguir();
                }
            }
            else if (following)
            {
                ParaPerseguir();
            }
        }
        private void OnDrawGizmos()
        {
            Gizmos.DrawLine(transform.position, player.transform.position);
        }
    }
}

