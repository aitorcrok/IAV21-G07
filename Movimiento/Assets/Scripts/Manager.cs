using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace UCM.IAV.Movimiento
{
    /// Gestiona los comportamientos de las ratas y el perro
    public class Manager : MonoBehaviour
    {
        private static Manager _instance;
        GameObject Perro;
        GameObject Suelo;
        GameObject[] Ratas;
        bool flauta = false;
        Agente a;
        /// Codigo singleton
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
        /// Busca los gameobjects de las ratas, el perro y el suelo
        void Awake()
        {
            DontDestroyOnLoad(this.gameObject);
            Perro = GameObject.FindGameObjectWithTag("perro");
            Ratas = GameObject.FindGameObjectsWithTag("rata");
            Suelo = GameObject.FindGameObjectWithTag("suelo");
            Suelo.GetComponent<Renderer>().material.color = Color.black;

        }
        /// Activa la flauta, cambiando los componentes de las ratas y el perro, y el color del suelo para señalizarlo
        public void Flauta()
        {
            flauta = !flauta;
            Perro.GetComponent<Llegada>().enabled = !flauta;
            Perro.GetComponent<Huir>().enabled = flauta;
            foreach (GameObject t in Ratas)
            {
                t.GetComponent<Merodeo>().enabled = !flauta;
                t.GetComponent<Llegada>().enabled = flauta;
                a = t.GetComponent<Agente>();
                if (flauta) a.aceleracionMax = 10;
                else a.aceleracionMax = 100;
            }
            if (flauta)
            {
                Suelo.GetComponent<Renderer>().material.color = Color.grey;
            }
            else
            {
                Suelo.GetComponent<Renderer>().material.color = Color.black;
            }
        }
    }
}