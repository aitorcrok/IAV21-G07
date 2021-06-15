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
        Envite,
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
    public class Card
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
        private Queue<int> _envites = new Queue<int>();
        public GameObject cardPrefab;
        public GameObject barajaTextGO;
        public GameObject inputFieldGO;
        private InputField inputField;
        public string GetInputFieldText() { return inputField.text; }
        private Text barajaText;
        private Text descartesText;
        public GameObject descartesTextGO;
        private Fase _actualFase = Fase.Mus;
        public Fase GetActualFase() { return _actualFase; }
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
            barajaText = barajaTextGO.GetComponent<Text>();
            descartesText = descartesTextGO.GetComponent<Text>();
            inputField = inputFieldGO.GetComponent<InputField>();
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

            if (_actualFase == Fase.Mus)
            {
                int mus;
                //Esto no es exactamente en este orden, hay que tener en cuenta la mano
                mus = _players[actualTurn].getMus();
                //repartir
                if (mus != -1)
                {
                    if (mus == 0) //no mus
                    {
                        _actualFase = Fase.Grande;
                        foreach (Player p in _players)
                            p.setMus(-1);
                        actualTurn = 0;
                    }
                    else
                    {
                        _players[actualTurn].setMus(-1);
                        if (actualTurn == 3) //si es el ultimo, hace cosas antes de preguntar al siguiente de nuevo
                        {
                            _actualFase = Fase.Descartar;
                            //eliges cartas que descartarse y las cambias
                        }
                        changeTurn();
                    }
                }

            }

            else if (_actualFase == Fase.Descartar)
            {
                Debug.Log("Descartando jugador "+actualTurn +"..." );
                if (_players[actualTurn].getEnd()) //si el jugador ha acabado de decidirse, se descartan sus cartas
                {
                    Descartar();
                    _players[actualTurn].setEnd();

                    if (actualTurn == 3) //si han acabado de descartar todos los jugadores, se vuelve a la fase de mus
                    {
                        _actualFase = Fase.Mus;
                    }
                    changeTurn();
                }
            }
            else if(_actualFase == Fase.Envite)
            {
                if (_players[actualTurn].GetComponent</*UserPlayer*/Player>())
                    inputFieldGO.SetActive(true);
                if (_players[actualTurn].getEnd())
                {
                    inputFieldGO.SetActive(false);
                    inputField.text = "Escribe apuesta, ESC para salir";
                    _envites.Enqueue(_players[actualTurn].getApuesta());
                    Debug.Log("A�ADIDA APUESTA DE " + _players[actualTurn].getApuesta());
                    if (actualTurn < 2) actualTurn = 2;
                    else actualTurn = 0;
                    _actualFase = Fase.Chica;
                }

            }
            else if (_actualFase == Fase.Grande || _actualFase == Fase.Chica || _actualFase == Fase.Pares || _actualFase == Fase.Juego)
            {
                Game();
            }  
            else if (_actualFase == Fase.Final)
            {

            }
            Debug.Log("Turno actual: " + actualTurn +", Fase actual: "+ _actualFase);
            setUI();
        }
        private void Game()
        {
            Debug.Log("Jugadr " + actualTurn + ", envidas o pasas?...");
            if (_players[actualTurn].getEnd()) //si el jugador ha acabado de decidirse
            {
                if (!_players[actualTurn].getEnvite())
                {
                    changeTurn();
                    if (actualTurn == 0) //si al cambiar de turno le toca al primero otra vez, ha dado la vuelta y todos pasan
                    {
                        _envites.Enqueue(1);
                        Debug.Log("A�ADIDA APUESTA DE " + 1);
                        _actualFase++;
                    }
                }
                else
                {
                    _actualFase = Fase.Envite;
                }
                _players[actualTurn].setEnd();
            }
        }
        public bool checkTurn(Player p) { return GetIndexPlayer(p) == actualTurn; }
        public void changeTurn()
        {
            actualTurn++;
            if (actualTurn > 3) actualTurn = 0;
        }

        public void Descartar()
        {
            for(int i=3; i >= 0; --i) //recorrer las cuatro cartas del jugador
            {
                var c = _players[actualTurn].Mano()[i];

                if (c.descarte == true) //si ha decidido descartarse esta carta
                {
                    //se le quita la carta
                    _descartes.Push(c);
                    _players[actualTurn].Mano().RemoveAt(i);

                    //se le a�ade otra
                    if(_baraja.Count == 0)//si no quedan cartas en la baraja para a�adir, rebaraja con la baraja de descartes
                        reBarajar();
                    _players[actualTurn].Mano().Add(_baraja.Peek());
                    _baraja.Pop();
                }
            }
            _players[actualTurn].RenderCards();
            if (actualTurn >1) //jugadores 3 y 4, que juegan en los laterales, hay que rotar sus cartas para que se vean bien en la mesa
                _players[actualTurn].RotateCards();
        }

        public void reBarajar(){ //se llama si no quedan cartas en la baraja
            Debug.Log("NO QUEDAN CARTAS. REBARAJANDO CON LOS DESCARTES");
            foreach(Card c in _descartes)
            {
                c.changeDescarte();
                _baraja.Push(c);
            }

            _descartes.Clear();
            Barajar(_baraja);

        }

        public void setUI()
        {
            barajaText.text = "BARAJA: " + _baraja.Count;
            descartesText.text = "DESCARTES: " + _descartes.Count;
        }

        public void resetInputField() { inputField.text = "Inv�lido"; }
        public void Envidar()
        {
            _players[actualTurn].Envidar();
        }
    }

}
