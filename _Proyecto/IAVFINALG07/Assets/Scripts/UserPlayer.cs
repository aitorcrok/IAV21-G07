using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IAV.G07.MUS
{
    public class UserPlayer : Player
    {
        private void Awake()
        {
            GameManager.Instance.AddPlayer(this, 0);
            actualFase = GameManager.Instance.GetActualFase();
        }

        private void Start()
        {
            RenderCards();
        }

        //private void Update()
        //{
        //   //ESTE UPDATE ES EL QUE ESTÁ AHORA MISMO EN PLAYER.
        //}

        public void Descartar()
        {

        }
        public void Pasar() { }

        public void Ver() { }

    }

}

