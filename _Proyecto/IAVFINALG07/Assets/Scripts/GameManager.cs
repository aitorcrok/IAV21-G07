using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
        Puntos,
        Fin
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
        public Apuesta getLastEnvite() { return _envites[_envites.Count - 1]; }
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
        public GameObject[] pointsGO = new GameObject[2];
        private Text[] pointsTexts = new Text[2];
        private Text[] apuestasTexts = new Text[4];
        private Fase _actualFase = Fase.Mus;
        public Fase GetActualFase() { return _actualFase; }
        private int actualTurn = 0; //0,1,2, o 3 segun el jugador que le toque
        private int _actualTeam = 0;
        private Action lastAction = Action.Inicial;
        private int lastEnvite = 0;
        private int puntosTeam1 = 0;
        private int puntosTeam2 = 0;

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
            for (int i = 0; i < 4; i++)
            {
                apuestasTexts[i] = apuestasGO[i].GetComponent<Text>();
            }
            for (int i = 0; i < 2; i++)
            {
                pointsTexts[i] = pointsGO[i].GetComponent<Text>();
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
            for (int i = 0; i < _players.Count; i++)
            {
                _players[i].OrdenarMano();
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
                turnText.text = "Turno: J" + (actualTurn + 1) + ". Mus? S/N";
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
            else if (_actualFase == Fase.Puntos)
            {
                int ganador = -1;
                int team1 = -1;
                int team2 = -1;

                for (int i = 0; i < _players.Count; i++)
                {
                    _players[i].OrdenarMano();                    
                }

                for (int i = 0; i < _envites.Count; i++)
                {
                    if (_envites[i].apuesta != 0)
                    {
                        team1 = CompareHands(_players[0].Mano(), _players[1].Mano(), (Envite)i);
                        team2 = CompareHands(_players[2].Mano(), _players[3].Mano(), (Envite)i);

                        if (team1 == 1)
                        {
                            if (team2 == 1) ganador = CompareHands(_players[0].Mano(), _players[2].Mano(), (Envite)i);
                            else ganador = CompareHands(_players[0].Mano(), _players[3].Mano(), (Envite)i);
                        }
                        else
                        {
                            if (team2 == 1) ganador = CompareHands(_players[1].Mano(), _players[2].Mano(), (Envite)i);
                            else ganador = CompareHands(_players[1].Mano(), _players[3].Mano(), (Envite)i);
                        }

                        if (ganador == 1) { 
                            puntosTeam1 += _envites[i].apuesta; pointsTexts[0].text = "Puntos Equipo 1: " + puntosTeam1;
                            Debug.Log(_envites[i].apuesta+" para T1");
                        }
                        else { 
                            puntosTeam2 += _envites[i].apuesta; pointsTexts[1].text = "Puntos Equipo 2: " + puntosTeam2;
                            Debug.Log(_envites[i].apuesta + " para T2");
                        }

                    }
                }

                _actualFase = Fase.Fin;
            }
            else {
                //Volver al menu
                Debug.Log("Sacabo");
            }

            setUI();
            Debug.Log("Puntos Equipo 1: " + puntosTeam1);
            Debug.Log("Puntos Equipo 2: " + puntosTeam2);
        }
        private void Game()
        {
            Debug.Log("Ultima accion: " + lastAction);
            if (_envites.Count == (int)GameManager.Instance.GetActualFase() - 1)
                turnText.text = "Turno: J" + (actualTurn + 1) + "/" + _actualFase.ToString() + ".La subes, la ves o pasas? S/V/P";
            else
                turnText.text = "Turno: J" + (actualTurn + 1) + "/" + _actualFase.ToString() + ". Envidas o pasas? E/P";
            Action playerAction = _players[actualTurn].actual;

            switch (playerAction)
            {
                case (Action.Envidar):
                    {
                        if (_players[actualTurn].GetComponent<UserPlayer>())
                            inputFieldGO.SetActive(true);
                        if (_players[actualTurn].getEnd())
                        {
                            lastAction = Action.Envidar;
                            inputFieldGO.SetActive(false);
                            inputField.text = "Escribe apuesta, ESC para salir";

                            _envites.Add(new Apuesta(_actualTeam, _players[actualTurn].getApuesta()));
                            setApuestasUI();
                            _players[actualTurn].resetAction();

                            if (_actualTeam == 1) actualTurn = 2;
                            else actualTurn = 0;
                        }
                        break;
                    }
                case (Action.Pasar):
                    {
                        if (_players[actualTurn].getEnd())
                        {
                            if (lastAction == Action.Envidar)
                            {
                                if (_envites[_envites.Count - 1].team == 1)
                                {
                                    if (actualTurn == 3)
                                    {
                                        changeFase();
                                    }
                                    else if (actualTurn == 2)
                                    {
                                        //Si está en el 3, se salta al jugador 4 sin pasar por el 2
                                        actualTurn = 3;
                                    }
                                }
                                else
                                {
                                    if (actualTurn == 0)
                                    {
                                        actualTurn = 1;
                                    }
                                    else if (actualTurn == 1)
                                    {
                                        changeFase();
                                    }
                                }
                            }
                            else if (lastAction == Action.Subir)
                            {
                                //la pareja que ha subido se lleva los puntos de antes de subir
                                if (_actualTeam == 1) { puntosTeam2 += lastEnvite; pointsTexts[1].text = "Puntos Equipo 2: " + puntosTeam2; }
                                else { puntosTeam1 += lastEnvite; pointsTexts[0].text = "Puntos Equipo 0: " + puntosTeam1; }

                                changeFase();
                            }
                            else if (lastAction == Action.Pasar)
                            {
                                //Hay envite?
                                if (_envites.Count == (int)GameManager.Instance.GetActualFase() - 1)
                                {
                                    _envites[_envites.Count - 1].apuesta = 0;
                                    _envites[_envites.Count - 1].team = 0;
                                    if (_actualTeam == 2)
                                    {
                                        puntosTeam1++;
                                        pointsTexts[0].text = "Puntos Equipo 1: " + puntosTeam1;
                                    }
                                    else
                                    {
                                        puntosTeam2++;
                                        pointsTexts[1].text = "Puntos Equipo 2: " + puntosTeam2;
                                    }
                                    setApuestasUI();
                                    changeFase();
                                }
                                else
                                {
                                    if (actualTurn != 3)
                                    {
                                        changeTurn();
                                    }
                                    else
                                    {
                                        _envites.Add(new Apuesta(0, 1));
                                        setApuestasUI();
                                        changeFase();
                                    }
                                }
                            }
                            else if (lastAction == Action.Inicial)
                            {
                                changeTurn();
                            }

                            lastAction = Action.Pasar;
                        }

                        break;
                    }
                case (Action.Subir):
                    {
                        lastAction = Action.Subir;

                        if (_players[actualTurn].GetComponent<UserPlayer>())
                            inputFieldGO.SetActive(true);
                        if (_players[actualTurn].getEnd())
                        {
                            inputFieldGO.SetActive(false);
                            inputField.text = "Escribe apuesta, ESC para salir";

                            //Hay que guarda la apuesta anterior no podemos borrarla por si no la ven
                            lastEnvite = _envites[_envites.Count - 1].apuesta;
                            _envites.RemoveAt(_envites.Count - 1);
                            _envites.Add(new Apuesta(_actualTeam, _players[actualTurn].getApuesta()));
                            setApuestasUI();

                            if (actualTurn < 2) actualTurn = 2;
                            else actualTurn = 0;
                        }
                        break;
                    }
                case (Action.Ver):
                    {
                        if (_players[actualTurn].getEnd())
                        {
                            _envites[_envites.Count - 1].team = _actualTeam;
                            setApuestasUI();
                            _players[actualTurn].resetAction();
                            if (_actualTeam == 1) actualTurn = 2;
                            else actualTurn = 0;
                            changeFase();
                        }
                        break;
                    }

            }
            _players[actualTurn].setEnd();
        }
        public bool checkTurn(Player p) { return GetIndexPlayer(p) == actualTurn; }
        public void changeTurn()
        {
            _players[actualTurn].resetAction();
            if (_actualTeam == 1) { actualTurn++; actualTurn++; }
            if (_actualTeam == 2)
            {
                if (actualTurn == 2) { actualTurn--; }
                else actualTurn = 0;
            }
            if (actualTurn < 2) _actualTeam = 1;
            else _actualTeam = 2;
        }

        public void changeFase()
        {
            foreach (Player p in _players)
            {
                p.resetAction();
            }
            actualTurn = 0;
            _actualTeam = 1;
            _actualFase++;
            lastAction = Action.Inicial;
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
            _players[actualTurn].OrdenarMano();
            if (_players[actualTurn].GetComponent<JoaquinPlayer>())
                _players[actualTurn].GetComponent<JoaquinPlayer>().HandValues();
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
                    apuestasTexts[0].text = "Grande Equipo: " + _envites[0].team + "\nValor: " + _envites[0].apuesta;
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

        public void changeScene(string scene)
        {
            SceneManager.LoadScene(scene);
        }

        public void resetInputField() { inputField.text = "Inválido"; }

        public void Apostar()
        {
            UserPlayer u = _players[actualTurn].GetComponent<UserPlayer>();
            if (u)
            {
                if (_players[actualTurn].actual == Action.Envidar) u.Envidar();
                else u.Subir();
            }
        }

        public int CompareHands(List<Card> first, List<Card> second, Envite e) //1 si la primera es mejor o igual, 0 si es la segunda
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
                    List<Card> first_aux = first;
                    List<Card> second_aux = second;
                    for (i = 0; i< 4; i++)
                    {
                        if (first_aux[i].num == 3) first_aux[i].num = 12;
                        if (first_aux[i].num == 2) first_aux[i].num = 1;
                        if (second_aux[i].num == 3) second_aux[i].num = 12;
                        if (second_aux[i].num == 2) second_aux[i].num = 1;
                    }
                    //Si tiene duples el valor de aux es 1(valor de la pareja mas alta)(valor de la pareja mas alta)
                    if (first_aux[0].num == first_aux[1].num && first_aux[2].num == first_aux[3].num) f_aux = 10000 + 100 * first_aux[0].num + first_aux[3].num;
                    //Si tiene medias el valor de aux es 1(valor de la carta del trio)
                    else if (first_aux[0].num == first_aux[1].num && first_aux[1].num == first_aux[2].num ||
                             first_aux[1].num == first_aux[2].num && first_aux[2].num == first_aux[3].num) f_aux = 100 + first_aux[2].num;
                    else
                    {
                        for (i = 0; i < 3; i++)
                        {
                            if (first_aux[i].num == first_aux[i + 1].num)
                            {
                                //Si tiene una pareja aux es el valor de la carta
                                f_aux = first_aux[i].num;
                                break;
                            }
                        }
                        //si no tiene pareja, pierde siempre
                        if (i == 3) return 0;
                    }
                    if (second_aux[0].num == second_aux[1].num && second_aux[2].num == second_aux[3].num) s_aux = 10000 + 100 * second_aux[0].num + second_aux[3].num;
                    else if (second_aux[0].num == second_aux[1].num && second_aux[1].num == second_aux[2].num ||
                             second_aux[1].num == second_aux[2].num && second_aux[2].num == second_aux[3].num) s_aux = 100 + second_aux[2].num;
                    else
                    {
                        for (i = 0; i < 3; i++)
                        {
                            if (second_aux[i].num == second_aux[i + 1].num)
                            {
                                s_aux = second_aux[i].num;
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
                    if (m < 31) return 1;
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
