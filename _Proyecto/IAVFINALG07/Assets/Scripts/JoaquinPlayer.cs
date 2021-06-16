using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IAV.G07.MUS
{
    public class JoaquinPlayer : Player
    {

        public enum Envite
        {
            Grande,
            Chica,
            Pares,
            Juego,
        }
        private List<Card> possibleCards = new List<Card>();

        private static int CompareCardsByNumber(Card a, Card b)
        {
            int x, y;
            x = b.num;
            if (x == 3) x = 12;
            else if (x == 2) x = 1;
            y = a.num;
            if (y == 3) y = 12;
            else if (y == 2) y = 1;
            return x.CompareTo(y);
        }
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
                            hand+= CompareHands(_mano, manoPosible, e);
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
                                hand += CompareHands(_mano, manoPosible, e);
                            }
                        }
                    }
                }
            }
            return hand;
        }

        private int CompareHands(List<Card> first, List<Card> second, Envite e)
        {
            //Faltaria tener en cuenta quien es mano en caso de manos iguales

            int i;
            switch (e)
            {
                case Envite.Grande:
                    //Va comparando de carta mas alta a mas baja de ambas manos, cuando son distintas gana la que sea mayor
                    i = -1;
                    int aux, aux2;
                    do
                    {
                        i++;
                        aux = first[i].num;
                        if (aux == 3) aux = 12;
                        else if (aux == 2) aux = 1;

                        aux2 = second[i].num;
                        if (aux2 == 3) aux2 = 12;
                        else if (aux2 == 2) aux2 = 1;
                    }
                    while (aux == aux2 && i < 3);

                    if (aux >= aux2) return 1;
                    else return 0;
                case Envite.Chica:
                    //Va comparando de carta mas baja a mas alta de ambas manos, cuando son distintas gana la que sea menor

                    i = 4;
                    do
                    {
                        i--;
                        aux = first[i].num;
                        if (aux == 3) aux = 12;
                        else if (aux == 2) aux = 1;

                        aux2 = second[i].num;
                        if (aux2 == 3) aux2 = 12;
                        else if (aux2 == 2) aux2 = 1;
                    }
                    while (aux == aux2 && i > 0);

                    //Si i es menor de cero las manos son iguales
                    if (aux <= aux2) return 1;
                    else return 0;
                case Envite.Pares:
                    //Para evaluar los pares se usan dos variables auxiliares, una por mano, que 
                    int f_aux = 0;
                    int s_aux = 0;
                    //Si tiene duples el valor de aux es 1(valor de la pareja mas alta)(valor de la pareja mas alta)
                    if (first[0].num == first[1].num && first[2].num == first[3].num) f_aux = 10000 + 100 * first[0].num + first[3].num;
                    //Si tiene medias el valor de aux es 1(valor de la carta del trio)
                    else if (first[0].num == first[1].num && first[1].num == first[2].num ||
                             first[1].num == first[2].num && first[2].num == first[3].num) f_aux = 100 + first[2].num;
                    else
                    {
                        for (i = 0; i < 3; i++)
                        {
                            if (first[i].num == first[i+1].num) 
                            {
                                //Si tiene una pareja aux es el valor de la carta
                                f_aux = first[i].num;
                                break;
                            }
                        }
                        //si no tiene pareja, pierde siempre
                        if (i == 3) return 0;
                    }
                    if (second[0].num == second[1].num && second[2].num == second[3].num) s_aux = 10000 + 100 * second[0].num + second[3].num;
                    else if (second[0].num == second[1].num && second[1].num == second[2].num ||
                             second[1].num == second[2].num && second[2].num == second[3].num) s_aux = 100 + second[2].num;
                    else
                    {
                        for (i = 0; i < 3; i++)
                        {
                            if (second[i].num == second[i + 1].num)
                            {
                                s_aux = second[i].num;
                                break;
                            }
                        }
                        if (i == 3) return 1;
                    }

                    //Compara los valores aux de las dos manos
                    if (f_aux >= s_aux) return 1;
                    else return 0;

                case Envite.Juego:
                    int n = 0, m = 0;

                    for (i = 0; i < 4; i++)
                    {
                        aux = first[i].num;
                        if (aux > 10 || aux == 3) aux = 10;
                        else if (aux == 2) aux = 1;
                        n += aux;
                        aux = second[i].num;
                        if (aux > 10 || aux == 3) aux = 10;
                        else if (aux == 2) aux = 1;
                        m += aux;
                    }
                    if (n < 31) return 0;
                    if (n == 31) return 1;
                    if (n == 32 && m != 31) return 1;

                    if (m > 32 && n > m) return 1;
                    else return 0;
                default:
                    return 0;
            }
        }
    }
}

