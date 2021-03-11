/*    
   Copyright (C) 2020-2021 Federico Peinado
   http://www.federicopeinado.com

   Este fichero forma parte del material de la asignatura Inteligencia Artificial para Videojuegos.
   Esta asignatura se imparte en la Facultad de Informática de la Universidad Complutense de Madrid (España).

   Autor: Federico Peinado 
   Contacto: email@federicopeinado.com
*/
namespace UCM.IAV.Movimiento
{
    using UnityEngine;


    /// <summary>
    /// Clase para modelar el comportamiento de HUIR a otro agente
    /// </summary>
    public class Huir : ComportamientoAgente
    {
        /// <summary>
        /// Obtiene la dirección
        /// </summary>
        /// <returns></returns>
        public override void Awake()
        {
            base.Awake();
            objetivo = GameObject.FindGameObjectWithTag("jugador");
        }
    public override Direccion GetDireccion()
        {
            // Si fuese un comportamiento de dirección dinámico en el que buscásemos alcanzar cierta velocidad en el agente, se tendría en cuenta la velocidad actual del agente y se aplicaría sólo la aceleración necesaria
            // Vector3 deltaV = targetVelocity - body.velocity;
            // Vector3 accel = deltaV / Time.deltaTime;

            Direccion direccion = new Direccion();
            direccion.lineal = transform.position - objetivo.transform.position;
            direccion.lineal.Normalize();
            direccion.lineal *= agente.aceleracionMax;

            // Podríamos meter una rotación automática en la dirección del movimiento, si quisiéramos
            return direccion;
        }
    }
}
