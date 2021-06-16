using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace IAV.G07.MUS
{
    public class PlayerSigns : Signs
    {
        public GameObject signMenu;
        bool menu = false;
        Text text;

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

        private void EnableSigns(bool b)
        {
            for (int i = 0; i < signMenu.transform.childCount; i++)
            {
                signMenu.transform.GetChild(i).GetComponent<SignButton>().toggleButton(b);
            }
        }
    }
}