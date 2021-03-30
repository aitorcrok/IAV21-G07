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
        public GameObject Perro;
        /// <summary>
        /// El componente de cuerpo rígido
        /// </summary>
        private Rigidbody _cuerpoRigido;
        /// <summary>
        /// Dirección del movimiento
        /// </summary>
        private Vector3 _dir;

        private Flee f;
        private Llegada l;
        private Merodeo m;
        private Llegada rl;

        GameObject[] targets;
        /// <summary>
        /// Al despertar, establecer el cuerpo rígido
        /// </summary>
        private void Awake()
        {
            _cuerpoRigido = GetComponent<Rigidbody>();
            f = Perro.GetComponent<Flee>();
            f.enabled = false;
            l = Perro.GetComponent<Llegada>();
            l.enabled = true;

            targets = GameObject.FindGameObjectsWithTag("Rata");
        }

        /// <summary>
        /// En cada tick, mover el avatar del jugador según las órdenes de este último
        /// </summary>
        public override void Update()
        {
            velocidad.x = Input.GetAxis("Horizontal");
            velocidad.z = Input.GetAxis("Vertical");
            velocidad *= velocidadMax;


            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyUp(KeyCode.Space))
            {
                f.enabled = !f.enabled;
                l.enabled = !l.enabled;
                foreach (GameObject t in targets)
                {
                    m = t.GetComponent<Merodeo>();
                    rl = t.GetComponent<Llegada>();

                    m.enabled = !m.enabled;
                    rl.enabled = !rl.enabled;
                }
            }            
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
