using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject HUD;
    public GameObject InfoPanel;
    public GameObject OnScreenPrompt;
    public Transform OnScreenPromtContainer;
    
    [SerializeField] TextMeshProUGUI scrapText;
    [SerializeField] TextMeshProUGUI playerHealth;
    [SerializeField] Image healthBarFill;
    [SerializeField] TextMeshProUGUI ammoBoxes;
    [SerializeField] Image ammoBarFill;

    [SerializeField] private GameObject overviewCam;
    private CinemachineVirtualCamera overviewCamCVC;

    private NewPlayerController player;
    private bool OverViewActive;
    private bool isPaused;

    private static Queue<string> _promptToDraw = new Queue<string>(); 
    
    
    private void Start()
    {
        UnPause();
        
        player = FindObjectOfType<NewPlayerController>();
        overviewCamCVC = overviewCam.GetComponent<CinemachineVirtualCamera>();

        player.Scrap.OnScrapChanged += UpdateScrap;
        player.Health.OnDamaged += UpdateHealth;
        player.Health.OnHealed += UpdateHealth;
        player.OnAmmoBoxesChanged += UpdateAmmoBoxes;
        
        UpdateScrap();
        UpdateHealth();
        UpdateAmmoBoxes();

    }

    void UpdateScrap() => scrapText.SetText(player.Scrap.Amount.ToString());
    void UpdateHealth()
    {
        playerHealth.SetText(player.Health.CurrentHealth.ToString());
        healthBarFill.fillAmount = player.Health.CurrentHealth / player.Health.MaxHealth;
    }
    void UpdateAmmoBoxes()
    {
        ammoBoxes.SetText(player.AmmoBoxes.ToString());
        ammoBarFill.fillAmount = player.AmmoBoxes / (float)player.MAXAmmoBoxes;
    }

    private void Update()
    {

        if (_promptToDraw != null && _promptToDraw.Any())
        {
            ScreenPrompt(_promptToDraw.Dequeue());
        }
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseToggle();
        }

        if (Input.GetKeyDown(KeyCode.Tab) && !isPaused)
        {
            ToggleViewCamActive();
        }
        
        //todo: For testing, remove
        if (Input.GetKeyDown(KeyCode.F5))
        {
            ScreenPrompt("Welcome to Moon Runner...");
        }
        if (Input.GetKeyDown(KeyCode.F8))
        {
            string header = "INFO";
            string body =
                "=============Controls=============\r\n" +
                "[W]-----------------------Forward\r\n" +
                "[S]-------------------------Reverse\r\n" +
                "[A]------------------------------Left\r\n" +
                "[D]-----------------------------Right\r\n" +
                "[R]----------------Repair/Reload\r\n" +
                "[Space]-----------------HandBrake\r\n" +
                "[Mouse]-----------------------Look\r\n" +
                "\r\n" +
                "==============Objective=============\r\n" +
                "<color=\"red\">" +
                "<b>- Build the Grand Cannon to destroy the mothership</b>\r\n" +
                "<color=\"white\">" +
                "- Deliver ammo from the factory to turrets in need\r\n" +
                "- Gather scrap from dead enemies\r\n" +
                "- Use scrap to build the Grand Cannon\r\n" +
                "- Scrap can be used to repair turrets or the base\r\n" +
                "- Dead turrets will not shoot, you must bring them back by repairing them.\r\n" +
                "- If your base loses all health you lose.\r\n" +
                "- The factory will repair you.";

            var testInfoPanel = Instantiate(InfoPanel, this.transform);
            testInfoPanel.GetComponent<InfoPanel>().Setup(body, header);
            
        }
    }

    public void ScreenPrompt(string message)
    {
        var testOnScreenPrompt = Instantiate(OnScreenPrompt, OnScreenPromtContainer);
        testOnScreenPrompt.GetComponent<OnScreenPrompt>().Setup(message);
    }

    public static void Message(string message) => _promptToDraw.Enqueue(message);

    private void ToggleViewCamActive()
    {
        if (!OverViewActive)
        {
            OverViewActive = true;
            overviewCamCVC.Priority = 10;
        }
        else
        { 
            OverViewActive = false;
            overviewCamCVC.Priority = -10;
        }
    }

    public void PauseToggle()
    {
        if (isPaused)
        {
            UnPause();
            pauseMenu.SetActive(false);
        }
        else
        {
            Pause();
            pauseMenu.SetActive(true);
            // load teh pause menu with the volume set to the correct level.
            pauseMenu.GetComponentInChildren<Slider>().value = AudioListener.volume;
        }
    }
    
    public void Pause()
    {
        HUD.SetActive(false);
        Time.timeScale = 0;
        UnlockAndRevealCursor();
        isPaused = true;
    }
    
    public void UnPause()
    {
        Time.timeScale = 1;
        HUD.SetActive(true);
        LockAndHideCursor();
        isPaused = false;
    }
    
    public static void UnlockAndRevealCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public static void LockAndHideCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


    public void Quit()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void ChangeVolume(float volume)
    {
        AudioListener.volume = volume;
    }
}
