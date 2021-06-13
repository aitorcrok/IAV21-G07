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
        }

        private void Start()
        {
            RenderCards();
        }
    }

}

