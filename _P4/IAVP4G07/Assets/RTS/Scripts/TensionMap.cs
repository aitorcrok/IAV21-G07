using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace es.ucm.fdi.iav.rts
{
    public class TensionMap : GraphGrid
    {
        private AllyMap allyMap;
        private EnemyMap enemyMap;
        public List<VertexInfluence> influences;
        // works as vertices in regular graph
        // GameObject[] locations;

        void Awake()
        {
            allyMap = GetComponent<AllyMap>();
            enemyMap = GetComponent<EnemyMap>();
        }
        public List<Vertex> GetVertex()
        {
            return vertices;
        }
        public void ComputeInfluence()
        {
            List<Vertex> allyVertex = allyMap.GetVertex();
            List<Vertex> enemyVertex = enemyMap.GetVertex();

            for (int i = 0; i < GetRows(); i++)
            {
                for (int j = 0; j < GetCols(); i++)
                {
                    int id = GridToId(j, i);

                    VertexInfluence vertex = vertices[id] as VertexInfluence;
                    VertexInfluence allyVertice = allyVertex[id] as VertexInfluence;
                    VertexInfluence enemyVertice = enemyVertex[id] as VertexInfluence;

                    vertex.value = allyVertice.value - enemyVertice.value;

                    float value = vertex.value;

                    Color mycolor = Color.green;

                    mycolor.a = value;

                    GetVertexObj(id).GetComponent<MeshRenderer>().material.color = mycolor;
                }
            }
        }

        public Transform GetMostTense(out float value)
        {
            float max = -1;
            Transform pos = null;
            for (int i = 0; i < GetRows(); i++)
            {
                for (int j = 0; j < GetCols(); i++)
                {
                    int id = GridToId(j, i);

                    VertexInfluence vertex = vertices[id] as VertexInfluence;
                    if (vertex.value > max)
                    {
                        max = vertex.value;
                        pos = vertex.transform;
                    }
                }
            }

            value = max;
            return pos;
        }

        public Transform GetLessTense(out float value)
        {
            float min = 1000;
            Transform pos = null;
            for (int i = 0; i < GetRows(); i++)
            {
                for (int j = 0; j < GetCols(); i++)
                {
                    int id = GridToId(j, i);

                    VertexInfluence vertex = vertices[id] as VertexInfluence;
                    if (vertex.value < min)
                    {
                        min = vertex.value;
                        pos = vertex.transform;
                    }
                }
            }

            value = min;
            return pos;
        }
    }
}