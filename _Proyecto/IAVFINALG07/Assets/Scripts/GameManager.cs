using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace IAV.G07.MUS
{
    public enum CardType
    {
        Bastos,
        Espadas,
        Copas,
        Oros
    };
    public struct Card
    {
        public Card(CardType p, int n, Sprite i) { palo = p; num = n; sprite = i; }
        public CardType palo;
        public int num;
        public Sprite sprite;
    };
    public class GameManager : MonoBehaviour
    {
        private static GameManager _instance;
        public static GameManager Instance { get { return _instance; } }
        private Stack<Card> _baraja = new Stack<Card>();
        private Stack<Card> _descartes = new Stack<Card>();
        private Queue<Card> _envites = new Queue<Card>();
        public GameObject cardPrefab;
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
            for(int i = 0; i < _players.Capacity; i++)
                _players.Add(null);
        }
        // Start is called before the first frame update
        void Start()
        {
            for(int i = 0; i < 4; i++)
            {
                CardType t = (CardType)i;
                for (int j = 0; j < 7; j++)
                {
                    string path = t.ToString() + "_" + (j+1);
                    Card c = new Card(t, j+1, Resources.Load<Sprite>(path));
                    _baraja.Push(c);
                }
                for(int j = 10; j<13; j++)
                {
                    string path = t.ToString() + "_" + j;
                    Card c = new Card(t, j, Resources.Load<Sprite>(path));
                    _baraja.Push(c);
                }
            }
            Barajar(_baraja);
            //inicializar mano de los jugadores (a.k.a robar 4 cartas) ESTO NO IRIA AQUI SINO EN OTRO SITIO PERO YO LO PONGO AQUI POR AHORA
            for(int i = 0; i < _players.Count; i++)
            {
                for(int k = 0; k<4; k++)
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

        //UPDATE PARA HACER PRUEBAS DE AÑADIR Y ELIMINAR CARTAS
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (_players[0].Mano().Count > 0)
                {
                    var c = _players[0].Mano()[0];
                    _descartes.Push(c);
                    _players[0].Mano().RemoveAt(0);
                    _players[0].RenderCards();
                    Debug.Log(_descartes.Count + " cartas en los DESCARTES");
                }
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                if (_players[0].Mano().Count < 5)
                {
                    _players[0].Mano().Add(_baraja.Peek());
                    _baraja.Pop();
                    _players[0].RenderCards();
                    Debug.Log(_baraja.Count + " cartas en la BARAJA");
                }

            }           
        }
    }

}
