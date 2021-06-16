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
    public enum SignEnum
    {
        None,
        DosReyes,
        TresReyes,
        DosAses,
        TresAses,
        Duples,
        TreintaUna,
        TresReyesAs,
        Ciego
    };
    public class Apuesta
    {
        public Apuesta(int t, int a) { team = t; apuesta = a; }
        public int team;
        public int apuesta;
    }
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
    public enum Action
    {
        Inicial,
        Envidar,
        Pasar,
        Ver,
        Subir
    }
    public class GameManager : MonoBehaviour
    {
        private static GameManager _instance;
        public static GameManager Instance { get { return _instance; } }
        private Stack<Card> _baraja = new Stack<Card>();
        private Stack<Card> _descartes = new Stack<Card>();
        private List<Apuesta> _envites = new List<Apuesta>();
        public int getEnvites() { return _envites.Count; }
        public Apuesta getLastEnvite() { return _envites[_envites.Count-1]; }
        public GameObject cardPrefab;
        public GameObject barajaTextGO;
        public GameObject inputFieldGO;
        private InputField inputField;
        public string GetInputFieldText() { return inputField.text; }
        private Text barajaText;
        private Text descartesText;
        private Text turnText;
        public GameObject descartesTextGO;
        public GameObject turnTextGO;
        public GameObject[] apuestasGO = new GameObject[4];
        private Text[] apuestasTexts = new Text[4];
        private Fase _actualFase = Fase.Mus;
        public Fase GetActualFase() { return _actualFase; }
        private int actualTurn = 0; //0,1,2, o 3 segun el jugador que le toque
        private int _actualTeam = 0;
        private Action lastAction = Action.Inicial;
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
            turnText = turnTextGO.GetComponent<Text>();
            for(int i = 0; i < 4; i++)
            {
                apuestasTexts[i] = apuestasGO[i].GetComponent<Text>();
            }
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
            if (actualTurn < 2) _actualTeam = 1;
            else _actualTeam = 2;
            if (_actualFase == Fase.Mus)
            {
                turnText.text = "Turno: J" + (actualTurn + 1)+". Mus? S/N";
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
                turnText.text = "Turno: J" + (actualTurn + 1) + ".Elige descartes (1-4)";
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
            else if (_actualFase == Fase.Grande || _actualFase == Fase.Chica || _actualFase == Fase.Pares || _actualFase == Fase.Juego)
            {
                Game();
            }
            else if (_actualFase == Fase.Final)
            {

            }
            setUI();
        }
        private void Game()
        {
            if(lastAction == Action.Envidar)
                turnText.text = "Turno: J" + (actualTurn + 1) + "/"+_actualFase.ToString()+".La subes, la ves o pasas? S/V/P";
            else
                turnText.text = "Turno: J" + (actualTurn + 1) + "/" + _actualFase.ToString() + ". Envidas o pasas? E/P";
            Action playerAction = _players[actualTurn].actual;
            
            switch (playerAction)
            {
                case (Action.Envidar):
                    {
                        if (_players[actualTurn].GetComponent</*UserPlayer*/Player>())
                            inputFieldGO.SetActive(true);
                        if (_players[actualTurn].getEnd())
                        {
                            lastAction = Action.Envidar;
                            inputFieldGO.SetActive(false);
                            inputField.text = "Escribe apuesta, ESC para salir";
                            
                            _envites.Add(new Apuesta(_actualTeam,_players[actualTurn].getApuesta()));
                            setApuestasUI();
                            _players[actualTurn].resetAction();

                            if (_actualTeam==1) actualTurn = 2;
                            else actualTurn = 0;

                        }
                        break;
                    }
                case (Action.Pasar):
                    {
                        if (_players[actualTurn].getEnd())
                        {
                            if(lastAction == Action.Envidar)
                            {
                                if(_envites[_envites.Count - 1].team == 1 && actualTurn==3) 
                                {
                                    //si está en el jugador 4 y la apuesta es del equipo 1, quiere decir que ambos jugadores del equipo 2 han decidido pasar de la apuesta.
                                    actualTurn = 0;
                                    //_actualFase++;
                                    changeFase();
                                    lastAction = Action.Inicial;
                                }
                                else if(_envites[_envites.Count-1].team ==2 && actualTurn == 1)
                                {
                                    //si está en el jugador 2 y la apuesta es del equipo 2, quiere decir que ambos jugadores del equipo 1 han decidido pasar de la apuesta.
                                    actualTurn = 0;
                                    //_actualFase++;
                                    changeFase();
                                    lastAction = Action.Inicial;
                                }
                                else
                                {
                                    _players[actualTurn].resetAction();
                                    changeTurn();
                                }
                            }
                            else
                            {
                                _players[actualTurn].resetAction();
                                changeTurn();
                                if (actualTurn == 0) //si al cambiar de turno le toca al primero otra vez, ha dado la vuelta y todos pasan
                                {
                                    _envites.Add(new Apuesta(0, 1));
                                    setApuestasUI();
                                    //_actualFase++;
                                    changeFase();
                                    lastAction = Action.Inicial;
                                }
                            }
                           
                        }
                        
                        break;
                    }
                case (Action.Subir):
                    {
                        if (_players[actualTurn].GetComponent</*UserPlayer*/Player>())
                            inputFieldGO.SetActive(true);
                        if (_players[actualTurn].getEnd())
                        {
                            inputFieldGO.SetActive(false);
                            inputField.text = "Escribe apuesta, ESC para salir";
                            
                            _envites.RemoveAt(_envites.Count - 1);
                            _envites.Add(new Apuesta(_actualTeam,_players[actualTurn].getApuesta()));
                            setApuestasUI();
                            _players[actualTurn].resetAction();

                            if (actualTurn < 2) actualTurn = 2;
                            else actualTurn = 0;
                            

                        }
                        break;
                    }
                case (Action.Ver):
                    {
                        if (_players[actualTurn].getEnd())
                        {
                            _envites[_envites.Count-1].team = _actualTeam;
                            setApuestasUI();
                            _players[actualTurn].resetAction();
                            if (_actualTeam == 1) actualTurn = 2;
                            else actualTurn = 0;

                            //_actualFase++;
                            changeFase();
                            lastAction = Action.Inicial;
                        }
                        break;
                    }

            }
            _players[actualTurn].setEnd();
        }
        public bool checkTurn(Player p) { return GetIndexPlayer(p) == actualTurn; }
        public void changeTurn()
        {
            if(_actualTeam == 1) { actualTurn++; actualTurn++; }
            if(_actualTeam == 2)
            {
                if(actualTurn == 2) { actualTurn--; }
                else actualTurn = 0;
            }
        }

        public void changeFase()
        {
            actualTurn = 0;
            _actualTeam = 1;
            _actualFase++;
        }

        public void Descartar()
        {
            for (int i = 3; i >= 0; --i) //recorrer las cuatro cartas del jugador
            {
                var c = _players[actualTurn].Mano()[i];

                if (c.descarte == true) //si ha decidido descartarse esta carta
                {
                    //se le quita la carta
                    _descartes.Push(c);
                    _players[actualTurn].Mano().RemoveAt(i);

                    //se le añade otra
                    if (_baraja.Count == 0)//si no quedan cartas en la baraja para añadir, rebaraja con la baraja de descartes
                        reBarajar();
                    _players[actualTurn].Mano().Add(_baraja.Peek());
                    _baraja.Pop();
                }
            }
            _players[actualTurn].RenderCards();
            if (actualTurn > 1) //jugadores 3 y 4, que juegan en los laterales, hay que rotar sus cartas para que se vean bien en la mesa
                _players[actualTurn].RotateCards();
        }

        public void reBarajar()
        { //se llama si no quedan cartas en la baraja
            Debug.Log("NO QUEDAN CARTAS. REBARAJANDO CON LOS DESCARTES");
            foreach (Card c in _descartes)
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
        public void setApuestasUI()
        {
            switch (_actualFase)
            {
                case Fase.Grande:
                    apuestasTexts[0].text = "Grande Equipo: " + _envites[0].team + "\nValor: "+_envites[0].apuesta;
                    break;
                case Fase.Chica:
                    apuestasTexts[1].text = "Chica Equipo: " + _envites[1].team + "\nValor: " + _envites[1].apuesta;
                    break;
                case Fase.Pares:
                    apuestasTexts[2].text = "Pares Equipo: " + _envites[2].team + "\nValor: " + _envites[2].apuesta;
                    break;
                case Fase.Juego:
                    apuestasTexts[3].text = "Juego Equipo: " + _envites[3].team + "\nValor: " + _envites[3].apuesta;
                    break;
                default:
                    break;
            }
        }

        public void setSign(SignEnum s, int p) 
        {
            for (int i = 0; i < _players.Count; i++) 
            {
                _players[i].setSign(s, p);
            }
        }

        public void resetInputField() { inputField.text = "Inválido"; }

        public void Apostar()
        {
            if(_players[actualTurn].actual == Action.Envidar) _players[actualTurn].Envidar();
            else _players[actualTurn].Subir();
        }
    }

}
