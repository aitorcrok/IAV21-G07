/*    
   Copyright (C) 2020 Federico Peinado
   http://www.federicopeinado.com

   Este fichero forma parte del material de la asignatura Inteligencia Artificial para Videojuegos.
   Esta asignatura se imparte en la Facultad de Inform�tica de la Universidad Complutense de Madrid (Espa�a).

   Autor: Federico Peinado 
   Contacto: email@federicopeinado.com
*/
namespace UCM.IAV.Movimiento
{

    using UnityEngine;

    /// <summary>
    /// Clara para el comportamiento de agente que consiste en ser el jugador
    /// </summary>

    public class ControlJugador: ComportamientoAgente
    {
        Animator animator;
        bool moving = false;
        Rigidbody rb;

        private void Start()
        {
            animator = GetComponentInChildren<Animator>();
            rb = GetComponent<Rigidbody>();
        }

        /// <summary>
        /// Obtiene la direcci�n
        /// </summary>
        /// <returns></returns>
        public override Direccion GetDireccion()
        {
            Direccion direccion = new Direccion();
            direccion.lineal.x = Input.GetAxis("Horizontal");
            direccion.lineal.z = Input.GetAxis("Vertical");

            if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0) animator.SetBool("Moving", true);
            else { 
                animator.SetBool("Moving", false);
                rb.velocity = Vector3.zero;
            }

            direccion.lineal.Normalize();
            direccion.lineal *= agente.aceleracionMax;

            // Podr�amos meter una rotaci�n autom�tica en la direcci�n del movimiento, si quisi�ramos

            return direccion;
        }
    }
}