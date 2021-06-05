using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace es.ucm.fdi.iav.rts
{
    public class InfluenceMap : GraphGrid
    {
        private AllyMap allyMap;
        private EnemyMap enemyMap;
        public VertexInfluence[] influences;

        // works as vertices in regular graph
        // GameObject[] locations;

        void Awake()
        {
            allyMap = GetComponent<AllyMap>();
            enemyMap = GetComponent<EnemyMap>();
            influences = new VertexInfluence[GetRows() * GetCols()];

        }
        public List<Vertex> GetVertex()
        {
            return vertices;
        }
        public void ComputeInfluence()
        {
            VertexInfluence[] allyVertex = allyMap.influences;
            VertexInfluence[] enemyVertex = enemyMap.influences;
            foreach (VertexInfluence ver in influences)
            {
                ver.value = 0;
            }

            for (int i = 0; i < GetRows(); i++)
            {
                for (int j = 0; j < GetCols(); j++)
                {
                    int id = GridToId(j, i);

                    VertexInfluence allyVertice = allyVertex[id];
                    VertexInfluence enemyVertice = enemyVertex[id];

                    influences[id].value = allyVertice.value + enemyVertice.value;

                    float value = influences[id].value;

                    Color mycolor;

                    if (value > 0) mycolor = Color.blue;
                    else if (value < 0) mycolor = Color.red;
                    else mycolor = Color.white;

                    mycolor.a = value;

                    GetVertexObj(id).GetComponent<MeshRenderer>().material.color = mycolor;
                }
            }
        }
    }
}