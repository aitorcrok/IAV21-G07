using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace es.ucm.fdi.iav.rts
{
    public class VertexInfluence : Vertex
    {
        public Faction faction;
        public float value = 0f;
        public Guild guild;

        public bool AddValue(float v)
        {
            bool isUpdated = false;
            if (v > value)
            {
                value += v;
                isUpdated = true;
            }
            return isUpdated;
        }

        public bool SubstractValue(float v)
        {
            bool isUpdated = false;
            if (v > value)
            {
                value -= v;
                isUpdated = true;
            }
            return isUpdated;
        }
    }
}