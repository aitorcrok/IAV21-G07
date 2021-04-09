using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UCM.IAV.Navegacion
{
    public class C : GraphVisibility
    {
        public override void Start()
        {
            instIdToId = new Dictionary<int, int>();
        }
    }

}

