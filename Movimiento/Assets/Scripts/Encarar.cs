using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UCM.IAV.Movimiento
{
    public class Encarar : Alinear
    {
        protected GameObject objAux;
        //Crea el gameobject objetivo

        public override void Awake()
        {
            base.Awake();
            objAux = objetivo;
            objetivo = new GameObject();
            objetivo.AddComponent<Agente>();
        }
        private void OnDestroy()
        {
            Destroy(objetivo);
        }
        // Obtiene la direccion
        public override Direccion GetDireccion()
        {
            Vector3 dir = objAux.transform.position - transform.position;
            if(dir.magnitude > 0.0f)
            {
                float oriObj = Mathf.Atan2(dir.x, dir.z);
                oriObj *= Mathf.Rad2Deg;
                objetivo.GetComponent<Agente>().orientacion = oriObj;
            }
            return base.GetDireccion();
        }
    }

}
