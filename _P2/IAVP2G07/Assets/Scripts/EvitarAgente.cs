namespace UCM.IAV.Movimiento
{

    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class EvitarAgente : ComportamientoAgente
    {
        public float collisionRadius = 0.4f;
        GameObject[] targets;

        // Busca al resto de ratas
        void Start()
        {
            targets = GameObject.FindGameObjectsWithTag("rata");
        }

        //Obtiene la direccion
        public override Direccion GetDireccion()
        {
            Direccion dir = new Direccion();
            float shortestTime = Mathf.Infinity;
            GameObject firstTarget = null;
            float firstMinSeparation = 0.0f;
            float firstDistance = 0.0f;
            Vector3 firstRelativePos = Vector3.zero;
            Vector3 firstRelativeVel = Vector3.zero;

            foreach (GameObject t in targets)
            {
                Vector3 relativePos;
                Agente targetAgent = t.GetComponent<Agente>();
                relativePos = t.transform.position - transform.position;
                Vector3 relativeVel = targetAgent.velocidad - agente.velocidad;
                float relativeSpeed = relativeVel.magnitude;
                float timeToCollision = Vector3.Dot(relativePos, relativeVel);
                timeToCollision /= relativeSpeed * relativeSpeed * -1;
                float distance = relativePos.magnitude;
                float minSeparation = distance - relativeSpeed * timeToCollision;
                if (minSeparation > 2 * collisionRadius)
                    continue;
                if (timeToCollision > 0.0f && timeToCollision < shortestTime)
                {
                    shortestTime = timeToCollision;
                    firstTarget = t;
                    firstMinSeparation = minSeparation;
                    firstRelativePos = relativePos;
                    firstRelativeVel = relativeVel;
                }
            }
            if (firstTarget == null)
                return dir;
            if (firstMinSeparation <= 0.0f || firstDistance < 2 * collisionRadius)
                firstRelativePos = firstTarget.transform.position;
            else
                firstRelativePos += firstRelativeVel * shortestTime;
            firstRelativePos.Normalize();
            dir.lineal = -firstRelativePos * agente.aceleracionMax;
            return dir;
        }
    }
}