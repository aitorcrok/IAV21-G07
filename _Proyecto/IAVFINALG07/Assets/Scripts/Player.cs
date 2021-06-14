using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace IAV.G07.MUS
{
    public class Player : MonoBehaviour
    {
        private List<Card> _mano = new List<Card>();
        public List<Card> Mano() { return _mano; }

        protected Fase actualFase;
        protected int mus = -1;
        public int getMus() { return mus;}
        public void setMus(int i) { mus = i; }

        //Esto sólo lo usará el jugador 1 (depende del GameManager que solo lo use el jugador 1).
        public void RenderCards()
        {
            for(int i = transform.childCount; i > 0; i--)
                Destroy(transform.GetChild(i - 1).gameObject);
            transform.DetachChildren();
            //instanciamos una imagen por carta haya en la mano
            int index = GameManager.Instance.GetIndexPlayer(this);
            for (int j = 0; j < _mano.Count; j++)
            {
                GameObject g = Instantiate<GameObject>(GameManager.Instance.cardPrefab, this.transform);
                if(index == 0)g.GetComponent<Image>().sprite = _mano[j].sprite;
            }
        } 
        public void Descartar()
        {

        }
        public void Pasar(){ }        
        public void Envidar(){ }
        public void Ver(){ }
        public void Subir(){ }
    }

}

