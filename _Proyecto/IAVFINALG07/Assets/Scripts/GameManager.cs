using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace IAV.G07.MUS
{
    public enum Fase
    {
        Mus,
        Descartar,
        Grande,
        Chica,
        Pares,
        Juego,
        Final
    }
    public enum CardType
    {
        Bastos,
        Espadas,
        Copas,
        Oros
    };
    public struct Card
    {
        public Card(CardType p, int n, Sprite i) { palo = p; num = n; sprite = i; descarte = false; }
        public CardType palo;
        public int num;
        public Sprite sprite;
        public bool descarte;

        public void changeDescarte()
        {
            descarte = !descarte;
        }
    };
    public class GameManager : MonoBehaviour
    {
        private static GameManager _instance;
        public static GameManager Instance { get { return _instance; } }
        private Stack<Card> _baraja = new Stack<Card>();
        private Stack<Card> _descartes = new Stack<Card>();
        private Queue<Card> _envites = new Queue<Card>();
        public GameObject cardPrefab;

        public Fase actualFase = Fase.Mus;
        public KeyCode actualInput;
        private int actualTurn = 0; //0,1,2, o 3 segun el jugador que le toque
        private List<Player> _players = new List<Player>(4); //se meten los jugadores en orden, los dos primeros son el primer equipo y los dos ultimos el segundo equipo
        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                _instance = this;
            }
            for (int i = 0; i < _players.Capacity; i++)
                _players.Add(null);
        }
        // Start is called before the first frame update
        void Start()
        {
            Iniciar();
        }

        private void Iniciar()
        {
            //crea la baraja ordenada
            for (int i = 0; i < 4; i++)
            {
                CardType t = (CardType)i;
                for (int j = 0; j < 7; j++)
                {
                    string path = t.ToString() + "_" + (j + 1);
                    Card c = new Card(t, j + 1, Resources.Load<Sprite>(path));
                    _baraja.Push(c);
                }
                for (int j = 10; j < 13; j++)
                {
                    string path = t.ToString() + "_" + j;
                    Card c = new Card(t, j, Resources.Load<Sprite>(path));
                    _baraja.Push(c);
                }
            }
            //baraja el mazo de cartas
            Barajar(_baraja);
            //inicializar mano de los jugadores (a.k.a robar 4 cartas) ESTO NO IRIA AQUI SINO EN OTRO SITIO PERO YO LO PONGO AQUI POR AHORA
            for (int i = 0; i < _players.Count; i++)
            {
                for (int k = 0; k < 4; k++)
                {
                    _players[i].Mano().Add(_baraja.Peek());
                    _baraja.Pop();
                }
            }
        }

        //Fisher-Yates shuffle
        //http://en.wikipedia.org/wiki/Fisher-Yates_shuffle
        static System.Random r = new System.Random();
        private void Barajar(Stack<Card> b)
        {
            Card[] aux = b.ToArray();
            b.Clear();
            for (int n = aux.Length - 1; n > 0; --n)
            {
                int k = r.Next(n + 1);
                Card temp = aux[n];
                aux[n] = aux[k];
                aux[k] = temp;
            }
            foreach (Card c in aux)
                b.Push(c);
        }

        public void AddPlayer(Player p, int index) { _players[index] = p; }

        public int GetIndexPlayer(Player p)
        {
            if (p == null)
                throw new ArgumentNullException();

            for (int index = 0; index < _players.Count; index++)
            {
                if (_players[index] == p)
                    return index;
            }
            throw new ArgumentException("El gestor del juego no conoce este controlador");
        }
        private void Update()
        {
            if (actualFase == Fase.Mus)
            {
                int mus;
                //Esto no es exactamente en este orden, hay que tener en cuenta la mano
                mus = _players[actualTurn].getMus();
                //repartir
                if (mus != -1)
                {
                    if (mus == 0) //no mus
                    {
                        actualFase = Fase.Grande;
                        foreach (Player p in _players)
                            p.setMus(-1);
                        actualTurn = 0;
                    }
                    else
                    {
                        _players[actualTurn].setMus(-1);
                        if (actualTurn == 3) //si es el ultimo, hace cosas antes de preguntar al siguiente de nuevo
                        {
                            Debug.Log("Descartense");
                            actualFase = Fase.Descartar;
                            //eliges cartas que descartarse y las cambias
                        }
                        changeTurn();
                    }
                }

            }

            else if (actualFase == Fase.Descartar)
            {
                //_players[actualTurn].Descartar();
                if (_players[actualTurn].getEnd())
                {
                    Descartar();
                    _players[actualTurn].setEnd();

                    if (actualTurn == 3)
                    {
                        actualFase = Fase.Mus;
                    }
                    changeTurn();
                }
            }

            else if (actualFase == Fase.Grande)
            {

                actualFase = Fase.Chica;
            }
            else if (actualFase == Fase.Chica)
            {

                actualFase = Fase.Pares;
            }
            else if (actualFase == Fase.Pares)
            {

                actualFase = Fase.Juego;
            }
            else if (actualFase == Fase.Juego)
            {

                actualFase = Fase.Final;
            }
            else if (actualFase == Fase.Final)
            {

            }
            Debug.Log("Turno actual: " + actualTurn);
        }

        public bool checkTurn(Player p) { return GetIndexPlayer(p) == actualTurn; }
        public void changeTurn()
        {
            actualTurn++;
            if (actualTurn > 3) actualTurn = 0;
        }

        public void Descartar()
        {
            for(int i=3; i >= 0; --i)
            {
                var c = _players[actualTurn].Mano()[i];

                if (c.descarte == true)
                {
                    _descartes.Push(c);
                    _players[actualTurn].Mano().RemoveAt(i);

                    if(_baraja.Count == 0)
                    {
                        reBarajar();
                    }

                    _players[actualTurn].Mano().Add(_baraja.Peek());
                    _baraja.Pop();

                }
            }

            _players[actualTurn].RenderCards();
        }

        public void reBarajar(){ //se llama si no quedan cartas en la baraja

            foreach(Card c in _descartes)
            {
                c.changeDescarte();
                _baraja.Push(c);
            }

            _descartes.Clear();
            Barajar(_baraja);

        }
    }

}
