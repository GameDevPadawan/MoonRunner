using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour
{
    void Awake()
    {
        UIManager.UnlockAndRevealCursor();
    }

    public void OnExitClicked()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
