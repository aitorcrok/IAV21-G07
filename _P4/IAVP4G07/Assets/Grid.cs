using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//Script utilizado para representar graficamente un vector bidimensional de datos 

//TO DO: 
//Aclarar que tipo de valor (Entero, float, struct, etc...) se va a utilizar
//Metodo publico que reciba un array bidimensional de datos y actualice los valores de la grid de acuerdo a estos
//Cambiar el metodo ActualizeCubeColor para que cambie entre el espectro deseado. 
//^ Se podría codificar mediante struct o numeros positivos y negativos

namespace es.ucm.fdi.iav.rts
{
    public class Grid : MonoBehaviour
    {
        public float cellSize; //Tamaño de las casillas
        public float cubeOffset; // Tamaño de los cubos en proporcion a las casillas
        public bool toggleCubes, toggleText;

        private Vector3 originPosition;
        private Vector3 terrainSize;
        private int width;
        private int depth;
        private float previousCellSize;
        private float previousCubeOffset;
        private bool previousToggleCubes, previousToggleText;
        private int[,] gridArray;
        private GameObject[,] gridCubeArray;
        private TextMesh[,] debugTextArray;

        private void Start()
        {
            originPosition = transform.position;
            terrainSize = GetComponentInParent<Terrain>().terrainData.size;
            width = (int)(terrainSize.x / cellSize);
            depth = (int)(terrainSize.z / cellSize);

            cubeOffset = Mathf.Clamp(cubeOffset, 0.1f, 1);
            previousCubeOffset = cubeOffset;
            previousCellSize = cellSize;
            previousToggleCubes = true;
            previousToggleText = true;

            createGrid();
        }

        private void Update() //Comprobacion del estado de las variables y actualizacion de los elementos de la grid
        {
            if (toggleCubes != previousToggleCubes) 
            {
                ActualizeCubeVisibility(toggleCubes);
                previousToggleCubes = toggleCubes;
            }

            if (toggleText != previousToggleText) 
            {
                ActualizeTextVisibility(toggleText);
                previousToggleText = toggleText;
            }


            if (cellSize != previousCellSize) 
            {
                ActualizeGridSize();
                previousCellSize = cellSize;
            }

            if (cubeOffset != previousCubeOffset)
            {
                cubeOffset = Mathf.Clamp(cubeOffset, 0.1f, 1);
                previousCubeOffset = cubeOffset;
                ActualizeCubeScale();
            }

            ActualizeCubeColor();
            ActualizeTextValue();
        }

        //Crea la grid de datos, la de texto, la de cubos y lineas para visualizarla en debug (Se pueden quitar si se quiere)
        private void createGrid()
        {
            gridArray = new int[width, depth];
            gridCubeArray = new GameObject[width, depth];
            debugTextArray = new TextMesh[width, depth];

            for (int x = 0; x < gridArray.GetLength(0); x++)
            {
                for (int z = 0; z < gridArray.GetLength(1); z++)
                {
                    debugTextArray[x, z] = CreateWorldText(null, gridArray[x, z].ToString(), GetWorldPosition(x, z) + new Vector3(cellSize, 0, cellSize) * .5f, 20, Color.white, TextAnchor.MiddleCenter, TextAlignment.Center);
                    Debug.DrawLine(GetWorldPosition(x, z), GetWorldPosition(x, z + 1), Color.white, 100f);
                    Debug.DrawLine(GetWorldPosition(x, z), GetWorldPosition(x + 1, z), Color.white, 100f);

                    gridCubeArray[x, z] = GameObject.CreatePrimitive(PrimitiveType.Cube);

                    Transform cube = gridCubeArray[x, z].GetComponent<Transform>();

                    cube.parent = gameObject.transform;
                    cube.localScale = new Vector3(cellSize * cubeOffset, cellSize * cubeOffset, cellSize * cubeOffset);
                    cube.SetPositionAndRotation(GetWorldPosition(x, z), cube.rotation);
                }
            }

            Debug.DrawLine(GetWorldPosition(0, depth), GetWorldPosition(width, depth), Color.white, 100f);
            Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, depth), Color.white, 100f);
        }

        private void ActualizeCubeScale() 
        {
            for (int x = 0; x < gridCubeArray.GetLength(0); x++)
            {
                for (int z = 0; z < gridCubeArray.GetLength(1); z++)
                {
                    Transform cube = gridCubeArray[x, z].GetComponent<Transform>();

                    cube.localScale = new Vector3(cellSize * cubeOffset, cellSize * cubeOffset, cellSize * cubeOffset);
                }
            }
        }

        private void ActualizeCubeVisibility(bool v)
        {
            for (int x = 0; x < gridCubeArray.GetLength(0); x++)
            {
                for (int z = 0; z < gridCubeArray.GetLength(1); z++)
                {
                    Transform cube = gridCubeArray[x, z].GetComponent<Transform>();

                    cube.GetComponent<MeshRenderer>().enabled = v;
                }
            }
        }

        private void ActualizeCubeColor()
        {
            for (int x = 0; x < gridCubeArray.GetLength(0); x++)
            {
                for (int z = 0; z < gridCubeArray.GetLength(1); z++)
                {
                    Transform cube = gridCubeArray[x, z].GetComponent<Transform>();

                    cube.GetComponent<MeshRenderer>().material.color = new Color(0, 0, x * z);
                }
            }
        }

        private void ActualizeTextVisibility(bool v)
        {
            for (int x = 0; x < debugTextArray.GetLength(0); x++)
            {
                for (int z = 0; z < debugTextArray.GetLength(1); z++)
                {
                    Transform text = debugTextArray[x, z].GetComponent<Transform>();

                    text.GetComponent<MeshRenderer>().enabled = v;
                }
            }
        }

        private void ActualizeTextValue()
        {
            for (int x = 0; x < debugTextArray.GetLength(0); x++)
            {
                for (int z = 0; z < debugTextArray.GetLength(1); z++)
                {
                    Transform text = debugTextArray[x, z].GetComponent<Transform>();
                    text.GetComponent<TextMesh>().text = gridArray[x,z].ToString();
                }
            }
        }

        private void ActualizeGridSize() //Se utiliza cuando se cambia el tamaño de la casilla pero esto no es algo que deba hacerse
        {
            DestroyGridCubes();

            width = (int)(terrainSize.x / cellSize);
            depth = (int)(terrainSize.z / cellSize);

            createGrid();
        }

        private void DestroyGridCubes() 
        {
            for (int x = 0; x < gridCubeArray.GetLength(0); x++)
            {
                for (int z = 0; z < gridCubeArray.GetLength(1); z++)
                {
                    Destroy(gridCubeArray[x, z]);
                }
            }
        }

        private Vector3 GetWorldPosition(int x, int z)
        {
            return new Vector3(x, originPosition.y, z) * cellSize + originPosition;
        }

        public void SetValue(int x, int z, int value)
        {
            if (x >= 0 && z >= 0 && x < width && z < depth)
            {
                gridArray[x, z] = value;
                debugTextArray[x, z].text = gridArray[x, z].ToString();
            }
        }

        public void SetValue(Vector3 worldPosition, int value)
        {
            int x, z;
            GetXZ(worldPosition, out x, out z);
            SetValue(x, z, value);
        }

        public int GetValue(int x, int z)
        {
            if (x >= 0 && z >= 0 && x < width && z < depth)
            {
                return gridArray[x, z];
            }
            else
            {
                return 0;
            }
        }

        public int GetValue(Vector3 worldPosition)
        {
            int x, z;
            GetXZ(worldPosition, out x, out z);
            return GetValue(x, z);
        }
        public void GetXZ(Vector3 worldPosition, out int x, out int z)
        {
            x = Mathf.FloorToInt((worldPosition - originPosition).x / cellSize);
            z = Mathf.FloorToInt((worldPosition - originPosition).z / cellSize);
        }

        public TextMesh CreateWorldText(Transform parent, string text, Vector3 localPosition, int fontSize, Color color, TextAnchor textAnchor, TextAlignment textAlignment)
        {
            GameObject gameObject = new GameObject("World_Text", typeof(TextMesh));
            Transform textTransform = gameObject.transform;
            textTransform.SetParent(parent, false);
            textTransform.localPosition = localPosition;
            TextMesh textMesh = gameObject.GetComponent<TextMesh>();
            textMesh.anchor = textAnchor;
            textMesh.alignment = textAlignment;
            textMesh.text = text;
            textMesh.fontSize = fontSize;
            textMesh.color = color;
            textTransform.parent = transform;
            return textMesh;
        }
    }

}