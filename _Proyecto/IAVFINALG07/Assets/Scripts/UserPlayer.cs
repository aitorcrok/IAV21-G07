using System.Collections;
using System;
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
            //RenderCards();
        }

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
                    if (Input.GetKeyDown(KeyCode.P)) /*pasa turno*/ { endTurn = true; envidar = false; actual = Action.Pasar; }
                    else if (GameManager.Instance.getEnvites() < (int)GameManager.Instance.GetActualFase() - 1 &&
                        actual == Action.Inicial && Input.GetKeyDown(KeyCode.E)) /*envida*/{ envidar = true; actual = Action.Envidar; }
                    else if (GameManager.Instance.getEnvites() == (int)GameManager.Instance.GetActualFase() - 1)
                    {
                        if (Input.GetKeyDown(KeyCode.V))/*ver*/ { endTurn = true; actual = Action.Ver; }
                        else if (Input.GetKeyDown(KeyCode.S)) /*subir*/ { actual = Action.Subir; }
                        envidar = false;
                    }
                }
            }
        }


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

