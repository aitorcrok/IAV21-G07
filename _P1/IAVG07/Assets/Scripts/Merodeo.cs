using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UCM.IAV.Movimiento
{
    public class Merodeo : Encarar
    {
        public float offset;
        public float radio;
        public float rate;
        //Crea el gameobject objetivo
        public override void Awake()
        {
            objetivo = new GameObject();
            objetivo.transform.position = transform.position;
            base.Awake(); 
        }
        // Obtiene la direccion
        public override Direccion GetDireccion()
        {
            Direccion dir = new Direccion();
            //intento circulo alrededor
            float rand1 = Random.Range(-1.0f, 1.0f);
            float rand2 = Random.Range(-1.0f, 1.0f);
            Vector3 vec = new Vector3(rand1, 0, rand2);
            Debug.DrawRay(transform.position, vec);
            Vector3 posObj = transform.position + (vec * radio);
            //el del objetivo delante
            //float oriMer = Random.Range(-1.0f, 1.0f) * rate;
            //float oriObj = oriMer + agente.orientacion;
            //Vector3 oriVec = OriToVec(agente.orientacion);
            //Vector3 posObj = (offset * oriVec) + transform.position;
            //posObj = posObj + (OriToVec(oriObj) * radio);
            objAux.transform.position = posObj;
            dir = base.GetDireccion();
            dir.lineal = objAux.transform.position - transform.position;
            dir.lineal.Normalize();
            dir.lineal *= agente.aceleracionMax;
            return dir;
        }
    }
}