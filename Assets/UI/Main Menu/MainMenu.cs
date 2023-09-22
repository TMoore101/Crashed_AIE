using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    //UI Varialbes
    public GameObject optionsMenu;

    //Start Game
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    //Open the options menu
    public void OpenOptions()
    {
        if (optionsMenu)
        {
            optionsMenu.SetActive(true);
            this.gameObject.SetActive(false);
        }
    }
    //Close the options menu
    public void CloseOptions()
    {
        if (optionsMenu)
        {
            optionsMenu.SetActive(false);
            this.gameObject.SetActive(true);
        }
    }

    //Quit game function
    public void QuitGame()
    {
        Application.Quit();
    }
}
