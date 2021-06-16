using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace IAV.G07.MUS
{
    public class Signs : MonoBehaviour
    {
        public GameObject gameManager, signImage;
        public int playerID;
        protected Image image, childImage;
        protected SignEnum signID = 0;

        protected string[] signSprites =
        {
            "None",
            "Dos_Reyes",
            "Tres_Reyes",
            "Dos_Ases",
            "Tres_Ases",
            "Duples",
            "Treinta_Una",
            "Tres_Reyes_As",
            "Ciego"
        };

        // Start is called before the first frame update
        void Awake()
        {
            image = GetComponent<Image>();
            childImage = signImage.GetComponent<Image>();

            childImage.enabled = false;
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void setSign(SignEnum s)
        {
            signID = s;
            gameManager.GetComponent<GameManager>().setSign(s, playerID);
            childImage.enabled = true;
            childImage.sprite = Resources.Load<Sprite>(signSprites[(int)s]);
        }
    }
}

