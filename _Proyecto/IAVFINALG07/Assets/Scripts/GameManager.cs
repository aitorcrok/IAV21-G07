using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IAV.G07.MUS
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager _instance;
        public static GameManager Instance { get { return _instance; } }

        public enum CardType
        {
            Basto,
            Espada,
            Copas,
            Oro
        };
        public struct Card
        {
            public Card(CardType p, int n) { palo = p; num = n; }
            public CardType palo;
            public int num;
        };

        private Stack<Card> _baraja = new Stack<Card>();
        private Stack<Card> _descartes = new Stack<Card>();
        private Queue<Card> _envites = new Queue<Card>();
        //private vector<Player> _players; //se meten los jugadores en orden, los dos primeros son el primer equipo y los dos ultimos el segundo equipo
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
        }
        // Start is called before the first frame update
        void Start()
        {
            for(int i = 0; i < 4; i++)
            {
                for(int j = 0; j < 7; j++)
                {
                    Card c = new Card((CardType)i, j+1);
                    _baraja.Push(c);
                }
                for(int j = 10; j<13; j++)
                {
                    Card c = new Card((CardType)i, j);
                    _baraja.Push(c);
                }
            }
            Barajar(_baraja);
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

        // Update is called once per frame
        void Update()
        {
        
        }
    }

}
