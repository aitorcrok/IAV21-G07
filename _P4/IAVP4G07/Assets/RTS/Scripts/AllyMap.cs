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
        bool visible = false;
        bool changeVisibility = true;
        // works as vertices in regular graph
        // GameObject[] locations;

        void Awake()
        {
            AwakeGraph();
            if (unitList == null)
                unitList = new List<Unit>();
            influences = new VertexInfluence[GetCols() * GetRows()];
            for (int i = 0; i < influences.Length; i++)
            {
                influences[i] = GetVertexObjs()[i].AddComponent<VertexInfluence>();
            }
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

            for(int i = 0; i<influences.Length;i++)
            {
                influences[i].value = 0;
            }

            foreach (Unit u in unitList)
            {
                if (u != null)
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
            }

            for (int i = 0; i < GetRows(); i++)
            {
                for (int j = 0; j < GetCols(); j++)
                {
                    int id = GridToId(j, i);

                    VertexInfluence vertex = influences[id];

                    float value = vertex.value;

                    Color mycolor;

                    if (value > 0)
                        mycolor = new Color(0, 0, 1, Mathf.Abs(value));
                    else if (value < 0)
                        mycolor = new Color(1, 0, 0, Mathf.Abs(value));
                    else
                        mycolor = new Color(1, 1,1, Mathf.Abs(value));

                    GetVertexObj(id).GetComponent<MeshRenderer>().material.color = mycolor;
                    
                }
            }
        }
        public void setVisible(bool b)
        {
            visible = b; changeVisibility = true;
        }
        private void Update()
        {
            if (changeVisibility)
            {
                for (int i = 0; i < GetRows(); i++)
                {
                    for (int j = 0; j < GetCols(); j++)
                    {
                        int id = GridToId(j, i);

                        GetVertexObj(id).GetComponent<MeshRenderer>().enabled = visible;
                    }
                }
                changeVisibility = false;
            }
        }
    }
}