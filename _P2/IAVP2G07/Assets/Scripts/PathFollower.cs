namespace UCM.IAV.Navegacion
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UCM.IAV.Movimiento;

    public class PathFollower : Seguir
    {
        public Path path;
        public float pathOffset = 0.0f;
        float currentParam;

        public override void Awake()
        {
            base.Awake();
            objetivo = new GameObject();
            currentParam = 0f;
        }
        public override Direccion GetDireccion()
        {
            currentParam = path.GetParam(transform.position, currentParam);
            float targetParam = currentParam + pathOffset;
            objetivo.transform.position = path.GetPosition(targetParam);
            return base.GetDireccion();
        }
    }
}
