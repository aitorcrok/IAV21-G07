/*    
   Copyright (C) 2020 Federico Peinado
   http://www.federicopeinado.com

   Este fichero forma parte del material de la asignatura Inteligencia Artificial para Videojuegos.
   Esta asignatura se imparte en la Facultad de Informática de la Universidad Complutense de Madrid (España).

   Autor: Federico Peinado 
   Contacto: email@federicopeinado.com
*/
namespace UCM.IAV.Movimiento
{

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// Clase para modelar el controlador del jugador como agente
    /// </summary>
    public class JugadorAgente : Agente
    {
        /// <summary>
        /// El componente de cuerpo rígido
        /// </summary>
        private Rigidbody _cuerpoRigido;
        /// <summary>
        /// Dirección del movimiento
        /// </summary>
        private Vector3 _dir;

        /// <summary>
        /// Al despertar, establecer el cuerpo rígido
        /// </summary>
        private void Awake()
        {
            _cuerpoRigido = GetComponent<Rigidbody>();
        }

        /// <summary>
        /// En cada tick, mover el avatar del jugador según las órdenes de este último
        /// </summary>
        public override void Update()
        {
            velocidad.x = Input.GetAxis("Horizontal");
            velocidad.z = Input.GetAxis("Vertical");
            velocidad *= velocidadMax; 
        }

        /// <summary>
        /// En cada tick fijo, según haya cuerpo rígido o no, uso el simulador físico aplicando fuerzas o no
        /// </summary>
        public override void FixedUpdate()
        {
            if (_cuerpoRigido == null)
            {
                transform.Translate(velocidad * Time.deltaTime, Space.World);
            }
            else
            {
                _cuerpoRigido.AddForce(velocidad * Time.deltaTime, ForceMode.VelocityChange);
            } 
        }

        /// <summary>
        /// En cada parte tardía del tick, encarar el agente
        /// </summary>
        public override void LateUpdate()
        {
            transform.LookAt(transform.position + velocidad);
        }
    }
}
