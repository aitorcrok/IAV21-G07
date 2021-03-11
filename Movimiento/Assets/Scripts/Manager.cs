using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace UCM.IAV.Movimiento
{
    public class Manager : MonoBehaviour
    {
        private static Manager _instance;
        GameObject Jugador;
        GameObject Perro;
        GameObject Suelo;
        GameObject[] Ratas;
        bool flauta = false;

        public static Manager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = GameObject.FindObjectOfType<Manager>();
                }

                return _instance;
            }
        }

        void Awake()
        {
            DontDestroyOnLoad(this.gameObject);
            Jugador = GameObject.FindGameObjectWithTag("jugador");
            Perro = GameObject.FindGameObjectWithTag("perro");
            Ratas = GameObject.FindGameObjectsWithTag("rata");
            Suelo = GameObject.FindGameObjectWithTag("suelo");
            Suelo.GetComponent<Renderer>().material.color = Color.black;

        }

        public void Flauta()
        {
            flauta = !flauta;
            Perro.GetComponent<Llegada>().enabled = !flauta;
            Perro.GetComponent<Huir>().enabled = flauta;
            foreach (GameObject t in Ratas)
            {
                t.GetComponent<Merodeo>().enabled = !flauta;
                t.GetComponent<Llegada>().enabled = flauta;
            }
            if (flauta)
            {
                Suelo.GetComponent<Renderer>().material.color = Color.green;
            }
            else
            {
                Suelo.GetComponent<Renderer>().material.color = Color.black;
            }
        }
    }
}