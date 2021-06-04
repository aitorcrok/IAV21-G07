/*    
    Obra original:
        Copyright (c) 2018 Packt
        Unity 2018 Artificial Intelligence Cookbook - Second Edition, by Jorge Palacios
        https://github.com/PacktPublishing/Unity-2018-Artificial-Intelligence-Cookbook-Second-Edition
        MIT License

    Modificaciones:
        Copyright (C) 2020-2021 Federico Peinado
        http://www.federicopeinado.com

        Este fichero forma parte del material de la asignatura Inteligencia Artificial para Videojuegos.
        Esta asignatura se imparte en la Facultad de Informática de la Universidad Complutense de Madrid (España).
        Contacto: email@federicopeinado.com
*/
namespace es.ucm.fdi.iav.rts
{

    using UnityEngine;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using UnityEngine.UI;

    public class GraphGrid : Graph
    {
        //public bool get8Vicinity = false;
        public float cellSize = 1f;
        //[Range(0, Mathf.Infinity)]
        //public float defaultCost = 1f;
        //[Range(0, Mathf.Infinity)]
        //public float maximumCost = Mathf.Infinity;
        int numCols;
        int numRows;
        GameObject[] vertexObjs;


        protected int GridToId(int x, int y)
        {
            return Math.Max(numRows, numCols) * y + x;
        }

        private Vector2 IdToGrid(int id)
        {
            Vector2 location = Vector2.zero;
            location.y = Mathf.Floor(id / numCols);
            location.x = Mathf.Floor(id % numCols);
            return location;
        }

        private void LoadMap(int width, int height)
        {
            try
            {
                int j = 0;
                int i = 0;
                int id = 0;

                Vector3 position = Vector3.zero;
                Vector3 scale = Vector3.zero;
 
                numRows = height;
                numCols = width;

                vertices = new List<Vertex>(numRows * numCols);
                neighbors = new List<List<Vertex>>(numRows * numCols);
                vertexObjs = new GameObject[numRows * numCols];

                for (i = 0; i < numRows; i++)
                {
                    for (j = 0; j < numCols; j++)
                    {
                        position.x = j * cellSize;
                        position.z = i * cellSize;
                        id = GridToId(j, i);
                            
                        vertexObjs[id] = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        vertexObjs[id].transform.SetPositionAndRotation(position, Quaternion.identity);

                        vertexObjs[id].name = vertexObjs[id].name.Replace("(Clone)", id.ToString());
                        Vertex v = vertexObjs[id].AddComponent<Vertex>();
                        v.id = id;
                        vertices.Add(v);
                        neighbors.Add(new List<Vertex>());
                        float y = vertexObjs[id].transform.localScale.y;
                        scale = new Vector3(cellSize, y, cellSize);
                        vertexObjs[id].transform.localScale = scale;
                        vertexObjs[id].transform.parent = gameObject.transform;
                    }
                }

                // now onto the neighbours
                for (i = 0; i < numRows; i++)
                {
                    for (j = 0; j < numCols; j++)
                    {
                        SetNeighbours(j, i);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        public override void Load()
        {
            Vector3 terrainSize = RTSGameManager.Instance.GetTerrain().terrainData.size;
            int width = (int)(terrainSize.x / cellSize);
            int depth = (int)(terrainSize.z / cellSize);

            LoadMap(width, depth);
        }

        protected void SetNeighbours(int x, int y, bool get8 = false)
        {
            int col = x;
            int row = y;
            int i, j;
            int vertexId = GridToId(x, y);
            neighbors[vertexId] = new List<Vertex>();
            Vector2[] pos = new Vector2[0];
            if (get8)
            {
                pos = new Vector2[8];
                int c = 0;
                for (i = row - 1; i <= row + 1; i++)
                {
                    for (j = col - 1; j <= col; j++)
                    {
                        pos[c] = new Vector2(j, i);
                        c++;
                    }
                }
            }
            else
            {
                pos = new Vector2[4];
                pos[0] = new Vector2(col, row - 1);
                pos[1] = new Vector2(col - 1, row);
                pos[2] = new Vector2(col + 1, row);
                pos[3] = new Vector2(col, row + 1);
            }
            foreach (Vector2 p in pos)
            {
                i = (int)p.y;
                j = (int)p.x;
                if (i < 0 || j < 0)
                    continue;
                if (i >= numRows || j >= numCols)
                    continue;
                if (i == row && j == col)
                    continue;
                int id = GridToId(j, i);
                neighbors[vertexId].Add(vertices[id]);
            }
        }

        public override Vertex GetNearestVertex(Vector3 position)
        {
            int col = (int)(position.x / cellSize);
            int row = (int)(position.z / cellSize);
            Vector2 p = new Vector2(col, row);
            List<Vector2> explored = new List<Vector2>();
            Queue<Vector2> queue = new Queue<Vector2>();
            queue.Enqueue(p);
            p = queue.Dequeue();
            col = (int)p.x;
            row = (int)p.y;
            int id = GridToId(col, row);
            Debug.Log(id);
            return vertices[id];
        }

        public int GetCols() { return numCols; }
        public int GetRows() { return numRows; }
    }
}
