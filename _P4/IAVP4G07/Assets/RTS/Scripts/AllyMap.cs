using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//INFORMACION RELEVANTE:
//ESTO ES UN COMPONENTE, HAY QUE AÑADIRSELO A UN GAMEOBJECT
//VA CON GRAPHGRID, ASI QUE TIENE MOVIDAS DE CARGA DE MAPAS POR ARCHIVO: MAL
//AUN NO SABEMOS COMO CREAR BIEN LOS GRAFOS


namespace es.ucm.fdi.iav.rts
{
    public class AllyMap : GraphGrid
    {
        public List<Unit> unitList;
        public VertexInfluence[] influences;
        // works as vertices in regular graph
        // GameObject[] locations;

        void Awake()
        {
            if (unitList == null)
                unitList = new List<Unit>();
            influences = new VertexInfluence[GetCols() * GetRows()];
        }
        public void AddUnit(Unit u)
        {
            if (unitList.Contains(u))
                return;
            unitList.Add(u); 
        }
        public void RemoveUnit(Unit u) 
        {
            unitList.Remove(u);
        }
        public List<Vertex> GetVertex()
        {
            return vertices;
        }
        public void ComputeInfluence()
        {
            VertexInfluence v;
            float dropOff;
            List<Vertex> pending = new List<Vertex>();
            List<Vertex> visited = new List<Vertex>();
            List<Vertex> frontier;
            Vertex[] neighbours;

            foreach (VertexInfluence ver in influences)
            {
                ver.value = 0;
            }

            foreach (Unit u in unitList)
            {
                Vector3 uPos = u.transform.position;
                Vertex vert = GetNearestVertex(uPos);
                pending.Add(vert);


                // BFS for assigning influence
                for (int i = 1; i <= u.Radius; i++)
                {
                    frontier = new List<Vertex>();
                    foreach (Vertex p in pending)
                    {
                        if (visited.Contains(p))
                            continue;
                        visited.Add(p);
                        dropOff = u.GetDropOff(i);
                        influences[p.id].AddValue(dropOff);
                        neighbours = GetNeighbours(vert);
                        frontier.AddRange(neighbours);
                    }
                    pending = new List<Vertex>(frontier);
                }
            }

            for (int i = 0; i < GetRows(); i++)
            {
                for (int j = 0; j < GetCols(); j++)
                {
                    int id = GridToId(j, i);

                    VertexInfluence vertex = influences[id];

                    float value = vertex.value;

                    Color mycolor = Color.blue;

                    //if (value > 0) mycolor = Color.blue;
                    //else if (value < 0) mycolor = Color.red;
                    //else mycolor = Color.white;

                    mycolor.a = value;

                    GetVertexObj(id).GetComponent<MeshRenderer>().material.color = mycolor;
                }
            }
        }
    }
}