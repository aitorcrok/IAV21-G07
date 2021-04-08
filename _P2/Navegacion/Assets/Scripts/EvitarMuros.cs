using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UCM.IAV.Movimiento
{
    public class EvitarMuros : Seguir
    {
        public float distanciaEvitar;
        public float mirarAdelante;
        public override void Awake() {
            base.Awake();
            objetivo = new GameObject();
        }
        public override Direccion GetDireccion()
        {
            Direccion direccion = new Direccion();
            Vector3 posicion = transform.position;
            Vector3 rayVector = agente.velocidad.normalized * mirarAdelante;
            Vector3 direction = rayVector;
            RaycastHit hit;
            if (Physics.Raycast(posicion, direction, out hit, mirarAdelante))
            {
                posicion = hit.point + hit.normal * distanciaEvitar;
                objetivo.transform.position = posicion;
                direccion = base.GetDireccion();
            }
            return direccion;
        }
    }

}
