using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//INFORMACION RELEVANTE:
//ESTO ES UN COMPONENTE, HAY QUE AÑADIRSELO A UN GAMEOBJECT
//VA CON GRAPHGRID, ASI QUE TIENE MOVIDAS DE CARGA DE MAPAS POR ARCHIVO: MAL
//AUN NO SABEMOS COMO CREAR BIEN LOS GRAFOS


namespace es.ucm.fdi.iav.rts
{
    public class InfluenceMap : GraphGrid
    {
        public List<Unit> unitList;
        public float dropOffThreshold;
        private Guild[] guildList;
        // works as vertices in regular graph
        GameObject[] locations;

        void Awake()
        {
            if (unitList == null)
                unitList = new List<Unit>();
            guildList = gameObject.GetComponents<Guild>();
        }
        public void AddUnit(Unit u)
        {
            if (unitList.Contains(u))
                return;
            unitList.Add(u); 
        }
        public void RemoveUnit(Unit u) 
        {
            unitList.Remove(u);
        }
        public void ComputeInfluenceSimple()
        {
            VertexInfluence v;
            float dropOff;
            List<Vertex> pending = new List<Vertex>();
            List<Vertex> visited = new List<Vertex>();
            List<Vertex> frontier;
            Vertex[] neighbours;

            foreach (Unit u in unitList)
            {
                Vector3 uPos = u.transform.position; Vertex vert = GetNearestVertex(uPos); pending.Add(vert);


                // BFS for assigning influence
                for (int i = 1; i <= u.Radius; i++)
                {
                    frontier = new List<Vertex>();
                    foreach (Vertex p in pending)
                    {
                        if (visited.Contains(p))
                            continue;
                        visited.Add(p);
                        v = p as VertexInfluence;
                        dropOff = u.GetDropOff(i);
                        v.SetValue(u.faction, dropOff);
                        neighbours = GetNeighbours(vert);
                        frontier.AddRange(neighbours);
                    }
                    pending = new List<Vertex>(frontier);
                }
            }

            for (int i = 0; i < GetRows(); i++)
            {
                for (int j = 0; j < GetCols(); i++)
                {
                    int id = GridToId(j, i);

                    VertexInfluence vertex = vertices[id] as VertexInfluence;

                    float value = vertex.value;
                    Faction f = vertex.faction;

                    Color mycolor;

                    if (f == Faction.BLUE) mycolor = Color.blue;
                    else if (f == Faction.RED) mycolor = Color.red;
                    else mycolor = Color.green;

                    mycolor.a = value;

                    GetVertexObj(id).GetComponent<Renderer>().GetComponent<Material>().color = mycolor;
                }
            }
        }
        public List<GuildRecord> ComputeMapFlooding()
        {
            GPWiki.BinaryHeap<GuildRecord> open;
            open = new GPWiki.BinaryHeap<GuildRecord>();
            List<GuildRecord> closed;
            closed = new List<GuildRecord>();

            foreach (Guild g in guildList)
            {
                GuildRecord gr = new GuildRecord();
                gr.location = GetNearestVertex(g.baseObject.transform.position);
                gr.guild = g;
                gr.strength = g.GetDropOff(0f);
                open.Add(gr);
            }
            while (open.Count != 0)
            {
                GuildRecord current;
                current = open.Remove();
                Vertex currObj;
                currObj = GetVertexObj(current.location.id);
                Vector3 currPos;
                currPos = currObj.transform.position;
                Vertex[] neighbours;
                neighbours = GetNeighbours(current.location);
                foreach (Vertex n in neighbours)
                {
                    Vertex nObj = GetVertexObj(n.id);
                    Vector3 nPos = nObj.transform.position;
                    float dist = Vector3.Distance(currPos, nPos);
                    float strength = current.guild.GetDropOff(dist);
                    if (strength < dropOffThreshold) 
                            continue;

                    GuildRecord neighGR = new GuildRecord();
                    neighGR.location = n;
                    neighGR.strength = strength; 
                    VertexInfluence vi;
                    vi = nObj.GetComponent<VertexInfluence>();
                    neighGR.guild = vi.guild;

                    if (closed.Contains(neighGR))
                    {
                        Vertex location = neighGR.location;
                        int index = closed.FindIndex(x => x.location == location);
                        GuildRecord gr = closed[index];
                        if (gr.guild.name != current.guild.name && gr.strength < strength)
                            continue;
                    }
                    else if (open.Contains(neighGR)) 
                    { 
                        bool mustContinue = false;
                        foreach (GuildRecord gr in open)
                        {
                            if (gr.Equals(neighGR)) 
                            {
                                mustContinue = true;
                                break;
                            }
                        } 
                        if (mustContinue) 
                            continue; 
                    }
                    else 
                    {
                        neighGR = new GuildRecord();
                        neighGR.location = n;
                    }
                    neighGR.guild = current.guild;
                    neighGR.strength = strength;
                    open.Add(neighGR);
                }
                closed.Add(current);
            }
            return closed;
        }
    }
}