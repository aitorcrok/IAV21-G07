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

        private Fase actualFase;
        private bool mus;

        void Awake()
        {
            actualFase = GameManager.Instance.actualFase;
        }

        //// Start is called before the first frame update
        //void Start()
        //{
        //    RenderCards();
        //}

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

        public bool StartRoutine()
        {
            StartCoroutine("AskRoutine");
            return mus;
        }
        IEnumerator AskRoutine()
        {
            mus = true;
            yield return new WaitForSeconds(.1f);
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

