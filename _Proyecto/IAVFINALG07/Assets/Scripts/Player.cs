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
        // Start is called before the first frame update
        private void Awake()
        {
            GameManager.Instance.AddPlayer(this);
        }
        void Start()
        {
            RenderCards();
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        //Esto sólo lo usará el jugador 1 (depende del GameManager que solo lo use el jugador 1).
        public void RenderCards()
        {
            for(int i = transform.childCount; i > 0; i--)
                Destroy(transform.GetChild(i - 1).gameObject);
            transform.DetachChildren();
            //instanciamos una imagen por carta haya en la mano
            for (int j = 0; j < _mano.Count; j++)
            {
                GameObject g = Instantiate<GameObject>(GameManager.Instance.cardPrefab, this.transform);
                g.GetComponent<Image>().sprite = _mano[j].sprite;

            }
        }
    }

}

