using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SignButton : MonoBehaviour
{
    bool isEnabled = false;
    Image image;
    Text text;

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        text = GetComponentInChildren<Text>();

        image.enabled = false;

        text.text = gameObject.name;
        text.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void toggleButton(bool b) 
    {
        isEnabled = b;
        image.enabled = isEnabled;
    }

    public void mouseEnterButton() 
    {
        if (isEnabled) text.enabled = true;
    }

    public void mouseExitButton()
    {
        if (isEnabled) text.enabled = false;
    }

    public void clickButton() 
    {
        if (isEnabled) { }
    }
}
