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
            actualFase = GameManager.Instance.actualFase;
        }

        private void Start()
        {
            RenderCards();
        }

        private void Update()
        {
            if (GameManager.Instance.checkTurn(this)) //si es mi turno
            {
                if (GameManager.Instance.actualFase == Fase.Robar)
                {
                    if (Input.GetKeyDown(KeyCode.S)) { mus = 1; Debug.Log("Mus_" + GameManager.Instance.GetIndexPlayer(this)); }
                    else if (Input.GetKeyDown(KeyCode.N)) { mus = 0; Debug.Log("No Mus_" + GameManager.Instance.GetIndexPlayer(this)); }
                }
            }
        }

    }

}

