using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace es.ucm.fdi.iav.rts
{
    public class TensionMap : GraphGrid
    {
        private AllyMap allyMap;
        private EnemyMap enemyMap;
        public VertexInfluence[] influences;
        bool visible = false;
        bool changeVisibility = true;
        // works as vertices in regular graph
        // GameObject[] locations;

        void Awake()
        {
            AwakeGraph();
            allyMap = GetComponent<AllyMap>();
            enemyMap = GetComponent<EnemyMap>();
            influences = new VertexInfluence[GetRows() * GetCols()];
            for (int i = 0; i < influences.Length; i++)
            {
                influences[i] = GetVertexObjs()[i].AddComponent<VertexInfluence>();
            }
        }
        public List<Vertex> GetVertex()
        {
            return vertices;
        }
        public void ComputeInfluence()
        {
            VertexInfluence[] allyVertex = allyMap.influences;
            VertexInfluence[] enemyVertex = enemyMap.influences;
            for (int i = 0; i < influences.Length; i++)
            {
                influences[i].value = 0;
            }

            for (int i = 0; i < GetRows(); i++)
            {
                for (int j = 0; j < GetCols(); j++)
                {
                    int id = GridToId(j, i);

                    VertexInfluence allyVertice = allyVertex[id];
                    VertexInfluence enemyVertice = enemyVertex[id];

                    influences[id].value = allyVertice.value - enemyVertice.value;

                    float value = influences[id].value;
                    if (value != 0) Debug.Log(value);
                    Color mycolor = new Color(0,1,0, Mathf.Abs(value));

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
                for (int j = 0; j < GetCols(); j++)
                {
                    int id = GridToId(j, i);

                    VertexInfluence vertex = influences[id];
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
                for (int j = 0; j < GetCols(); j++)
                {
                    int id = GridToId(j, i);

                    VertexInfluence vertex = influences[id];
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