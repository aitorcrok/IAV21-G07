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
        protected bool envidar = false;
        public bool getEnvite() { return envidar; }
        public bool getEnd() { return endTurn; }
        public void setEnd() { endTurn = false; }
        public int getMus() { return mus; }
        public void setMus(int i) { mus = i; }
        public void setSign(SignEnum s, int i) { señas[i] = s; }

        public Action actual = Action.Inicial;
        public void resetAction() { actual = Action.Inicial; }

        //Esto sólo lo usará el jugador 1 (depende del GameManager que solo lo use el jugador 1).
        public void RenderCards(bool b)
        {
            for (int i = transform.childCount; i > 0; i--)
                Destroy(transform.GetChild(i - 1).gameObject);
            transform.DetachChildren();
            //instanciamos una imagen por carta haya en la mano
            int index = GameManager.Instance.GetIndexPlayer(this);
            for (int j = 0; j < _mano.Count; j++)
            {
                GameObject g = Instantiate<GameObject>(GameManager.Instance.cardPrefab, this.transform);
                if (index == 0 || b)
                    g.GetComponent<Image>().sprite = _mano[j].sprite;

            }
        }

        public void RotateCards()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).Rotate(new Vector3(0, 0, 1), 90);
            }
        }

        //public void Pasar(){ }        

        protected static int CompareCardsByNumber(Card a, Card b)
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

        public void OrdenarMano(bool b)
        {
            _mano.Sort(CompareCardsByNumber);
            RenderCards(b);
        }
    }

}

