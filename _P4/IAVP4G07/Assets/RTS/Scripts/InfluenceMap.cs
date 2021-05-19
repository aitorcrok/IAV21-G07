using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace es.ucm.fdi.iav.rts
{
    public class InfluenceMap : Graph
    {
        public List<Unit> unitList;
        // works as vertices in regular graph
        GameObject[] locations;

        void Awake()
        {
            if (unitList == null)
                unitList = new List<Unit>();
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
        public void ComputeInfluenceSimple()
        {
            VertexInfluence v;
            float dropOff;
            List<Vertex> pending = new List<Vertex>();
            List<Vertex> visited = new List<Vertex>();
            List<Vertex> frontier;
            Vertex[] neighbours;

            foreach (Unit u in unitList)
            {
                Vector3 uPos = u.transform.position; Vertex vert = GetNearestVertex(uPos); pending.Add(vert);


                // BFS for assigning influence
                for (int i = 1; i <= u.Radius; i++)
                {
                    frontier = new List<Vertex>();
                    foreach (Vertex p in pending)
                    {
                        if (visited.Contains(p))
                            continue;
                        visited.Add(p);
                        v = p as VertexInfluence;
                        dropOff = u.GetDropOff(i);
                        v.SetValue(u.faction, dropOff);
                        neighbours = GetNeighbours(vert);
                        frontier.AddRange(neighbours);
                    }
                    pending = new List<Vertex>(frontier);
                }
            }
        }
    }
}