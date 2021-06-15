using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Signs : MonoBehaviour
{
    bool menu = false;
    public GameObject signMenu, signImage;
    Image image, childImage;
    Text text;
    int signID = -1;

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

    public void setSprite(Sprite s) 
    {
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
