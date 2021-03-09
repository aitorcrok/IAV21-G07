namespace UCM.IAV.Movimiento
{

    using UnityEngine;

    public class Flee : ComportamientoAgente
    {
        public float tiempoAObjetivo = 0.1f;
        public float margen = 0.3f;
        public float radioExt = 3.0f;
        public float velMaxima = 8;
        float velObjetivo;
        float distancia;

        public override Direccion GetDireccion()
        {
            Direccion direccion = new Direccion();
            direccion.lineal =  transform.position - objetivo.transform.position;
            distancia = direccion.lineal.magnitude;
            

            float reduccion = 0;
            if (distancia > radioExt)
            {
                return direccion;
            }
            else if (distancia < margen)
            {
                reduccion = 0f;
            }
            else
            {
                reduccion = distancia / agente.aceleracionMax * radioExt;
            }
            velObjetivo = agente.velocidadMax - reduccion;
            direccion.lineal = direccion.lineal * velObjetivo;
            direccion.lineal = direccion.lineal - agente.velocidad;
            direccion.lineal /= tiempoAObjetivo;
            return direccion;
        }
    }
}