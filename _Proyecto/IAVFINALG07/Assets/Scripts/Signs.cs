using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace IAV.G07.MUS 
{
    public class Signs : MonoBehaviour
    {
        public GameObject gameManager;
        bool menu = false;
        public GameObject signMenu, signImage;
        Image image, childImage;
        Text text;
        SignEnum signID = 0;

        // Start is called before the first frame update
        void Start()
        {
            image = GetComponent<Image>();
            childImage = signImage.GetComponent<Image>();
            text = GetComponentInChildren<Text>();

            childImage.enabled = false;
            EnableSigns(false);
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void toggleMenu()
        {
            menu = !menu;
            EnableSigns(menu);
            text.enabled = !menu;
        }

        public void setSign(SignEnum s) 
        {
            signID = s;
            gameManager.GetComponent<GameManager>().setSign(s, 0);
        }

        public void setSprite(Sprite s)
        {
            childImage.enabled = true;
            childImage.sprite = s;
        }

        private void EnableSigns(bool b)
        {
            for (int i = 0; i < signMenu.transform.childCount; i++)
            {
                signMenu.transform.GetChild(i).GetComponent<SignButton>().toggleButton(b);
            }
        }
    }
}

