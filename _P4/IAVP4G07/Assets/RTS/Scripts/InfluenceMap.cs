using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace es.ucm.fdi.iav.rts
{
    public class InfluenceMap : Graph
    {
        public List<Unit> unitList;
        // works as vertices in regular graph
        GameObject[] locations;
    }

    void Awake()
    {
        if (unitList == null)
            unitList = new List<Unit>();
    }
}