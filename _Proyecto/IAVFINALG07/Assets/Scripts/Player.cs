using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace IAV.G07.MUS
{
    public class Player : MonoBehaviour
    {
        protected List<Card> _mano = new List<Card>();
        public List<Card> Mano() { return _mano; }

        public SignEnum[] señas = { 0, 0, 0, 0 };

        protected Fase actualFase;
        protected int mus = -1;

        protected bool endTurn = false;
        protected int apuesta = 0;
        public int getApuesta() { return apuesta; }
        bool envidar = false;
        public bool getEnvite() { return envidar; }
        public bool getEnd() { return endTurn; }
        public void setEnd() { endTurn = false; }
        public int getMus() { return mus; }
        public void setMus(int i) { mus = i; }
        public void setSign(SignEnum s, int i) { señas[i] = s; }

        public Action actual = Action.Inicial;
        public void resetAction() { actual = Action.Inicial; }

        //Esto sólo lo usará el jugador 1 (depende del GameManager que solo lo use el jugador 1).
        public void RenderCards()
        {
            for (int i = transform.childCount; i > 0; i--)
                Destroy(transform.GetChild(i - 1).gameObject);
            transform.DetachChildren();
            //instanciamos una imagen por carta haya en la mano
            int index = GameManager.Instance.GetIndexPlayer(this);
            for (int j = 0; j < _mano.Count; j++)
            {
                GameObject g = Instantiate<GameObject>(GameManager.Instance.cardPrefab, this.transform);
                if (index == 0) g.GetComponent<Image>().sprite = _mano[j].sprite;
            }
        }

        public void RotateCards()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).Rotate(new Vector3(0, 0, 1), 90);
            }
        }
        //ESTE UPDATE REALMENTE ES DE USERPLAYER, PERO PARA HACER PRUEBAS ESTOY HACIENDO QUE TODOS LOS JUGADORES LOS CONTROLE YO
        private void Update()
        {
            //Hace lo mismo que el player para hacer pruebas
            if (GameManager.Instance.checkTurn(this)) //si es mi turno
            {
                if (GameManager.Instance.GetActualFase() == Fase.Mus)
                {
                    Debug.Log("Quieres hacer mus? S/N");
                    if (Input.GetKeyDown(KeyCode.S)) { mus = 1; Debug.Log("Mus_" + GameManager.Instance.GetIndexPlayer(this)); }
                    else if (Input.GetKeyDown(KeyCode.N)) { mus = 0; Debug.Log("No Mus_" + GameManager.Instance.GetIndexPlayer(this)); }
                }
                else if (GameManager.Instance.GetActualFase() == Fase.Descartar && !endTurn)
                {
                    Debug.Log("Elige cartas para descartar, 1-4, pulsa Enter cuando estes listx");
                    if (Input.GetKeyDown(KeyCode.Alpha1)) { _mano[0].changeDescarte(); }
                    else if (Input.GetKeyDown(KeyCode.Alpha2)) { _mano[1].changeDescarte(); }
                    else if (Input.GetKeyDown(KeyCode.Alpha3)) { _mano[2].changeDescarte(); }
                    else if (Input.GetKeyDown(KeyCode.Alpha4)) { _mano[3].changeDescarte(); }
                    else if (Input.GetKeyDown(KeyCode.Return)) { endTurn = true; } //fin de descarte

                }
                else if ((GameManager.Instance.GetActualFase() == Fase.Grande || GameManager.Instance.GetActualFase() == Fase.Chica ||
                        GameManager.Instance.GetActualFase() == Fase.Pares || GameManager.Instance.GetActualFase() == Fase.Juego) && !endTurn)
                {
                    if (Input.GetKeyDown(KeyCode.P)) /*pasa turno*/ { endTurn = true; envidar = false; actual = Action.Pasar; Debug.Log("Pasar"); }
                    else if (GameManager.Instance.getEnvites() < (int)GameManager.Instance.GetActualFase() - 1 &&
                        actual == Action.Inicial && Input.GetKeyDown(KeyCode.E)) /*envida*/{ envidar = true; actual = Action.Envidar; }
                    else if (GameManager.Instance.getEnvites() == (int)GameManager.Instance.GetActualFase() - 1)
                    {
                        if (Input.GetKeyDown(KeyCode.V))/*ver*/ { endTurn = true; actual = Action.Ver; Debug.Log("Ver"); }
                        else if (Input.GetKeyDown(KeyCode.S)) /*subir*/ { actual = Action.Subir; }
                        envidar = false;
                    }
                }
            }
        }
        //public void Pasar(){ }        
        public void Envidar()
        {
            if (Int32.TryParse(GameManager.Instance.GetInputFieldText(), out apuesta))
            {
                if (apuesta >= 2)
                {
                    Debug.Log("Envite: " + apuesta);
                    endTurn = true;
                }
                else
                {
                    GameManager.Instance.resetInputField();
                }
            }
            else
            {
                GameManager.Instance.resetInputField();
            }

        }
        public void Subir()
        {
            if (Int32.TryParse(GameManager.Instance.GetInputFieldText(), out apuesta))
            {
                if (apuesta > GameManager.Instance.getLastEnvite().apuesta)
                {
                    Debug.Log("Sube a : " + apuesta);
                    endTurn = true;
                }
                else
                {
                    GameManager.Instance.resetInputField();
                }
            }
            else
            {
                GameManager.Instance.resetInputField();
            }
        }
    }

}

