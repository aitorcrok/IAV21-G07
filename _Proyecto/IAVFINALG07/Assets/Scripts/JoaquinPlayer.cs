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


        public float[] goodHands = { 0.84f, 0.84f, 0.95f, 1};
        public float[] greatHands = { 0.88f, 0.88f, 0.96f, 1 };

        private float[] handValue = { 0, 0, 0, 0 };

        public bool juegaChica = false;

        public float thinkingTime = 3;
        public int farolChance = 10;

        private float time = 0;

        public int player; // jugador 2,3 o 4.

        public Signs sign;
        private void Awake()
        {
            GameManager.Instance.AddPlayer(this, player-1);
        }
        private void Start()
        {
            //OrdenarMano();

            InitAI();

            if (player > 2) //jugadores 3 y 4, que juegan en los laterales, hay que rotar sus cartas para que se vean bien en la mesa
                RotateCards();
        }

        private void Update()
        {
            //Hace lo mismo que el player para hacer pruebas
            if (GameManager.Instance.checkTurn(this)) //si es mi turno
            {
                if (Time.time - time > thinkingTime)
                {
                    time = Time.time;
                    if (GameManager.Instance.GetActualFase() == Fase.Mus)
                    {
                        Debug.Log("La IA se piensa el mus");
                        if (CheckMus()) { mus = 1; Debug.Log("Mus_" + GameManager.Instance.GetIndexPlayer(this)); }
                        else { mus = 0; Debug.Log("No Mus_" + GameManager.Instance.GetIndexPlayer(this)); }

                    }
                    else if (GameManager.Instance.GetActualFase() == Fase.Descartar && !endTurn)
                    {
                        Discard();
                        //Debug.Log("Elige cartas para descartar, 1-4, pulsa Enter cuando estes listx");
                        //if (Input.GetKeyDown(KeyCode.Alpha1)) { _mano[0].changeDescarte(); }
                        //else if (Input.GetKeyDown(KeyCode.Alpha2)) { _mano[1].changeDescarte(); }
                        //else if (Input.GetKeyDown(KeyCode.Alpha3)) { _mano[2].changeDescarte(); }
                        //else if (Input.GetKeyDown(KeyCode.Alpha4)) { _mano[3].changeDescarte(); }
                        //else if (Input.GetKeyDown(KeyCode.Return)) { endTurn = true; } //fin de descarte

                    }
                    else if ((GameManager.Instance.GetActualFase() == Fase.Grande || GameManager.Instance.GetActualFase() == Fase.Chica ||
                            GameManager.Instance.GetActualFase() == Fase.Pares || GameManager.Instance.GetActualFase() == Fase.Juego) && !endTurn)
                    {
                        if (CheckPass()) /*pasa turno*/ { endTurn = true; envidar = false; actual = Action.Pasar; }
                        else if (GameManager.Instance.getEnvites() < (int)GameManager.Instance.GetActualFase() - 1 &&
                            actual == Action.Inicial && CheckEnvidar()) /*envida*/{ envidar = true; actual = Action.Envidar; }
                        else if (GameManager.Instance.getEnvites() == (int)GameManager.Instance.GetActualFase() - 1)
                        {
                            Ver();
                            envidar = false;
                        }
                    }
                }               
            }
            else time = Time.time;
        }

        public void UpdateSign()
        {
            if (_mano[0] == _mano[1] && _mano[2] == _mano[3]) sign.setSign(SignEnum.Duples);
            else if (_mano[2].num == 1 || _mano[2].num == 2) sign.setSign(SignEnum.TresAses);
            else if (_mano[1].num == 1 || _mano[1].num == 2) sign.setSign(SignEnum.DosAses);
            else if ((_mano[2].num == 12 || _mano[2].num == 3) && (_mano[3].num == 1 || _mano[3].num == 2)) sign.setSign(SignEnum.TresReyesAs);
            else
            {
                int aux, n = 0;
                for (int i = 0; i < 4; i++)
                {
                    aux = _mano[i].num;
                    if (aux > 10 || aux == 3) aux = 10;
                    else if (aux == 2) aux = 1;
                    n += aux;
                }
                if (n == 31) sign.setSign(SignEnum.TreintaUna);
                else if (_mano[2].num == 12 || _mano[2].num == 3) sign.setSign(SignEnum.TresReyes);
                else if (_mano[1].num == 12 || _mano[1].num == 3) sign.setSign(SignEnum.DosReyes);
                else sign.setSign(SignEnum.Ciego);
            }
        }

        private void InitAI()
        {
            InitPossible();
            HandValues();
            UpdateSign();
        }

        private void InitPossible()
        {
            //crea la baraja ordenada
            for (int i = 0; i < 4; i++)
            {
                CardType t = (CardType)i;
                for (int j = 0; j < 7; j++)
                {
                    string path = t.ToString() + "_" + (j + 1);
                    Card c = new Card(t, j + 1, Resources.Load<Sprite>(path));
                    possibleCards.Add(c);
                }
                for (int j = 10; j < 13; j++)
                {
                    string path = t.ToString() + "_" + j;
                    Card c = new Card(t, j, Resources.Load<Sprite>(path));
                    possibleCards.Add(c);
                }
            }
            for (int i = 0; i < 4; i++)
            {
                NotPossible(_mano[i]); 
            }
        }

        public void NotPossible(Card c)
        {
            possibleCards.Remove(c);
        }


        public void HandValues()
        {
            int total, n;

            foreach (Envite e in System.Enum.GetValues(typeof(Envite)))
            {
                n = EvaluateHand(e, out total);
                handValue[(int)e] = (float)n / (float)total;
            }
        }


        private bool CheckMus()
        {
            for (int i = 0; i < 4; i++)
            {
                if (i == (int)Envite.Chica && !juegaChica) i++;
                if (handValue[i] >= goodHands[i]) return false;
            }
            return true;
            //if (juegaChica)
            //    return (grandeValue < goodGrande || chicaValue < goodChica || paresValue < goodPares || juegoValue < goodJuego);
            //else
            //    return (grandeValue < goodGrande || paresValue < goodPares || juegoValue < goodJuego);
        }
        private void Discard()
        {
            for (int i = 0; i < 3; i++)
            {
                if(_mano[i].num != 3 && _mano[i].num != 12) _mano[i].changeDescarte();
            }
            endTurn = true;
        }
        private bool CheckPass()
        {
            int env = (int)GameManager.Instance.GetActualFase() - 2;
            return handValue[env] < goodHands[env];
        }
        private bool CheckEnvidar()
        {
            int env = (int)GameManager.Instance.GetActualFase() - 2;
            if (handValue[env] >= greatHands[env]) apuesta = 5;
            else if (handValue[env] >= goodHands[env] || Random.Range(1, 100) <= farolChance) apuesta = 2;
            else return false;
            endTurn = true;
            return true;
        }
        private bool Ver()
        {
            int env = (int)GameManager.Instance.GetActualFase() - 2;
            int apuesta = GameManager.Instance.getLastEnvite().apuesta;
            if ((handValue[env] >= goodHands[env] && apuesta < 5) || (handValue[env] >= greatHands[env] && apuesta >= 5))/*ver*/ { endTurn = true; actual = Action.Ver; }
            else if (handValue[env] >= greatHands[env]) /*subir*/ { actual = Action.Subir; apuesta = 5; }
            else return false;
            endTurn = true;
            return true;
        }

        private int EvaluateHand(Envite e, out int total)
        {
            int hand = 0;
            total = 0;
            int N = possibleCards.Count;
            for (int i = 0; i < N; i++)
            {
                for (int j = i + 1; j < N; j++)
                {
                    for (int k = j + 1; k < N; k++)
                    {
                        for (int l = k + 1; l < N; l++)
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
                for (int j = i + 1; j < N; j++)
                {
                    for (int k = j + 1; k < N; k++)
                    {
                        for (int l = k + 1; l < N; l++)
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

