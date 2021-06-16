using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IAV.G07.MUS
{
    public enum Envite
    {
        Grande,
        Chica,
        Pares,
        Juego,
    }
    public class JoaquinPlayer : Player
    {
        private List<Card> possibleCards = new List<Card>();

        public int player; // jugador 2,3 o 4.
        private void Awake()
        {
            GameManager.Instance.AddPlayer(this, player-1);
        }
        private void Start()
        {
            _mano.Sort(CompareCardsByNumber);
            RenderCards();
            if (player > 2) //jugadores 3 y 4, que juegan en los laterales, hay que rotar sus cartas para que se vean bien en la mesa
                RotateCards();
        }


        private int EvaluateHand(Envite e)
        {
            int hand = 0, total = 0;
            int N = possibleCards.Count;
            for (int i = 0; i < N; i++)
            {
                for (int j = i; j < N; j++)
                {
                    for (int k = j; k < N; k++)
                    {
                        for (int l = k; l < N; l++)
                        {
                            List<Card> manoPosible = new List<Card>();
                            manoPosible.Add(possibleCards[i]);
                            manoPosible.Add(possibleCards[j]);
                            manoPosible.Add(possibleCards[k]);
                            manoPosible.Add(possibleCards[l]);
                            manoPosible.Sort(CompareCardsByNumber);
                            hand+= GameManager.Instance.CompareHands(_mano, manoPosible, e);
                            total++;
                        }
                    }
                }
            }
            return hand;
        }

        private int EvaluateHand()
        {
            int hand = 0;
            int N = possibleCards.Count;
            for (int i = 0; i < N; i++)
            {
                for (int j = i; j < N; j++)
                {
                    for (int k = j; k < N; k++)
                    {
                        for (int l = k; l < N; l++)
                        {
                            List<Card> manoPosible = new List<Card>();
                            manoPosible.Add(possibleCards[i]);
                            manoPosible.Add(possibleCards[j]);
                            manoPosible.Add(possibleCards[k]);
                            manoPosible.Add(possibleCards[l]);
                            manoPosible.Sort(CompareCardsByNumber);
                            foreach (Envite e in System.Enum.GetValues(typeof(Envite)))
                            {
                                hand += GameManager.Instance.CompareHands(_mano, manoPosible, e);
                            }
                        }
                    }
                }
            }
            return hand;
        }

    }
}

