using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public GameObject playButton, exitButton, pveModeButton, eveButton, menuButton, gameTitle, modesTitle;
    bool modes = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Modes() 
    {
        modes = !modes;

        gameTitle.SetActive(!modes);
        playButton.SetActive(!modes);
        exitButton.SetActive(!modes);

        modesTitle.SetActive(modes);
        pveModeButton.SetActive(modes);
        eveButton.SetActive(modes);
        menuButton.SetActive(modes);
    }

    public void changeScene(string scene) 
    {
        SceneManager.LoadScene(scene);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
