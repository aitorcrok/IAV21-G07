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
        //Esto es literalmente igual que llegada pero con angulos
        //IMPORTANTE: todo en GRADOS, los radianes mal
        public override Direccion GetDireccion()
        {
            Direccion dir = new Direccion();
            float oriObj = objetivo.GetComponent<Agente>().orientacion * Mathf.Rad2Deg;
            float rot = oriObj - transform.eulerAngles.y;

            rot = RadianesAGrados(rot);
            float tamRot = Mathf.Abs(rot);
            float rotObj= 0;
            if (tamRot > radioObj)
            {
                if (tamRot > radioS)
                    rotObj = agente.rotacionMax;
                else
                    rotObj = agente.rotacionMax * tamRot / radioS;
                rotObj *= rot / tamRot;
            }

            //CREO QUE LO DE ANGULARVELOCITY ESTA MAL Y POR ESO TIEMBLA
            dir.angular = rotObj - GetComponent<Rigidbody>().angularVelocity.y * Mathf.Rad2Deg;
            //dir.angular /= tiempoAObjetivo;
            float acelAng = Mathf.Abs(dir.angular);
            if(acelAng > agente.aceleracionAngularMax)
            {
                dir.angular /= acelAng;
                dir.angular *= agente.aceleracionAngularMax;
            }
            return dir;
        }
    }

}
