using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void OnPlayClicked()
    {
        SceneManager.LoadScene("Scene1");
    }

    public void OnTutorialClicked()
    {
        SceneManager.LoadScene("Tutorial");
    }

    public void OnCreditsClicked()
    {
        SceneManager.LoadScene("Credits");
    }

    public void OnQuitClicked()
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
