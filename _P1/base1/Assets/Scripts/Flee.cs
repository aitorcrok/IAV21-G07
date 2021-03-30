namespace UCM.IAV.Movimiento
{

    using UnityEngine;

    public class Flee : ComportamientoAgente
    {
        public float tiempoAObjetivo = 0.1f;
        public float radioExt = 3.0f;

        float velObjetivo;
        float distancia;
        float reduccion;
        public override Direccion GetDireccion()
        {
            Direccion direccion = new Direccion();
            direccion.lineal =  transform.position - objetivo.transform.position;
            distancia = direccion.lineal.magnitude;
            
            if (distancia > radioExt)
            {
                velObjetivo = 0;
            }
            else
            {
                reduccion = distancia / agente.aceleracionMax * radioExt;
                velObjetivo = agente.velocidadMax - reduccion;
            }
            direccion.lineal = direccion.lineal * velObjetivo;
            direccion.lineal = direccion.lineal - agente.velocidad;
            direccion.lineal /= tiempoAObjetivo;
            return direccion;
        }
    }
}