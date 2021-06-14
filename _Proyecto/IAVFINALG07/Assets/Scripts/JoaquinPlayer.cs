using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IAV.G07.MUS
{
    public class JoaquinPlayer : Player
    {
        public int player; // jugador 2,3 o 4.
        private void Awake()
        {
            GameManager.Instance.AddPlayer(this, player-1);
        }
        private void Start()
        {
            RenderCards();
            if (player > 2) //jugadores 3 y 4, que juegan en los laterales, hay que rotar sus cartas para que se vean bien en la mesa
                RotateCards();
        }
    }
}

