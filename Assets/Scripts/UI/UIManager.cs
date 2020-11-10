using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject HUD;

    private void Start()
    {
        UnPause();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Pause();
        }
    }

    public void Pause()
    {
        HUD.SetActive(false);
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
    }

    public void UnPause()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
        HUD.SetActive(true);
    }

    public void Quit()
    {
        //If we are running in a standalone build of the game
        #if UNITY_STANDALONE
        //Quit the application
        Application.Quit();
        #endif
 
        //If we are running in the editor
        #if UNITY_EDITOR
        //Stop playing the scene
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
