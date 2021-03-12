using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UCM.IAV.Movimiento
{
    public class Alinear : ComportamientoAgente
    {
        public float radioObj;
        public float radioS;
        public float tiempoAObjetivo = 0.1f;
        // Obtiene la direccion
        public override Direccion GetDireccion()
        {
            Direccion dir = new Direccion();
            float oriObj = objetivo.GetComponent<Agente>().orientacion;
            float rot = oriObj - agente.orientacion;
            rot = RadianesAGrados(rot);
            float tamRot = Mathf.Abs(rot);
            float rotObj;
            if (tamRot < radioObj)
                return dir;
            else if (tamRot > radioS)
                rotObj = agente.rotacionMax;
            else
                rotObj = agente.rotacionMax * tamRot / radioS;
            rotObj *= rot / tamRot;
            dir.angular = rotObj - agente.rotacion;
            dir.angular /= tiempoAObjetivo;
            //float acelAng = Mathf.Abs(dir.angular);
            //if(acelAng > agente.aceleracionAngularMax)
            //{
            //    dir.angular /= acelAng;
            //    dir.angular *= agente.aceleracionAngularMax;
            //}
            return dir;
        }
    }

}
