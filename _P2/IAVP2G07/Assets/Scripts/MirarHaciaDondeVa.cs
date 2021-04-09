using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UCM.IAV.Movimiento
{

    public class MirarHaciaDondeVa : Alinear
    {
        public override void Awake()
        {
            base.Awake();
            objetivo = new GameObject();
            objetivo.AddComponent<Agente>();
        }

        public override Direccion GetDireccion()
        {
            Vector3 dir = GetComponent<Rigidbody>().velocity;
            if (dir.magnitude == 0)
                return new Direccion();

            objetivo.GetComponent<Agente>().orientacion = Mathf.Atan2(dir.x, dir.z);
            return base.GetDireccion();
        }
    }

}


