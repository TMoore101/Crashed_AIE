using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;
    public static GameManager Instance
    {
        get
        {
            if (Instance == null)
            {
                Debug.LogError("GameManager not found!");
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.LogWarning("Multiple GameManagers found!");
            Destroy(this);
            return;
        }
    }

    public void Restart()
    {
        int sceneLevel = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(sceneLevel);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public void QuitToMainMenu()
    {
        SceneManager.LoadScene(0);
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }
    public void QuitToDesktop()
    {
        Application.Quit();
    }
}
