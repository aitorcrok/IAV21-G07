namespace UCM.IAV.Movimiento
{

    using UnityEngine;


    /// <summary>
    /// Clase para modelar el comportamiento de LLEGAR a otro agente
    /// </summary>
    public class Llegada : ComportamientoAgente
    {
        public float tiempoAObjetivo = 0.1f;
        public float margen = 0.3f;
        public float radioExt = 3.0f;

        float velObjetivo;
        float distancia;

        /// Obtiene el gameobject del jugador
        public override void Awake()
        {
            base.Awake();
            objetivo = GameObject.FindGameObjectWithTag("jugador");
        }

        /// Obtiene la dirección
        public override Direccion GetDireccion()
        {
            Direccion direccion = new Direccion();
            direccion.lineal = objetivo.transform.position - transform.position;
            distancia = direccion.lineal.magnitude;
            direccion.lineal.Normalize();

            if (distancia > radioExt)
            {
                velObjetivo = agente.aceleracionMax;
            }
            else if (distancia < margen)
            {
                    velObjetivo = 0;
            }
            else
            {
                velObjetivo = agente.aceleracionMax * distancia / radioExt;
            }
            direccion.lineal = direccion.lineal * velObjetivo;
            direccion.lineal = direccion.lineal - agente.velocidad;
            //direccion.lineal /= tiempoAObjetivo;
            return direccion;
        }
    }
}
