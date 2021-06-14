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
                if (GameManager.Instance.actualFase == Fase.Mus)
                {
                    if (Input.GetKeyDown(KeyCode.S)) { mus = 1; Debug.Log("Mus_" + GameManager.Instance.GetIndexPlayer(this)); }
                    else if (Input.GetKeyDown(KeyCode.N)) { mus = 0; Debug.Log("No Mus_" + GameManager.Instance.GetIndexPlayer(this)); }
                }
                else if (GameManager.Instance.actualFase == Fase.Descartar && !endTurn)
                {
                    if (Input.GetKeyDown(KeyCode.Alpha1)) { _mano[0].changeDescarte(); }
                    else if (Input.GetKeyDown(KeyCode.Alpha2)) { _mano[1].changeDescarte(); }
                    else if (Input.GetKeyDown(KeyCode.Alpha3)) { _mano[2].changeDescarte(); }
                    else if (Input.GetKeyDown(KeyCode.Alpha4)) { _mano[3].changeDescarte(); }
                    else if (Input.GetKeyDown(KeyCode.Return)) { endTurn = true; } //fin de descarte

                }
            }
        }

        public void Descartar()
        {

        }
        public void Pasar() { }
        public void Envidar() { }
        public void Ver() { }
        public void Subir() { }

    }

}

