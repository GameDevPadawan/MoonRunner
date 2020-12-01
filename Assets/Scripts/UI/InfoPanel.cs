using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InfoPanel : MonoBehaviour
{
    private UIManager _uiManager;
    
    [SerializeField] private TextMeshProUGUI HeaderText;
    [SerializeField] private TextMeshProUGUI InfoText;
    [SerializeField] private Button OKButton;

    private void Start()
    {
        var exists = FindObjectsOfType<InfoPanel>();
        foreach (var panel in exists)
        {
            if(panel != this)
            {
                Destroy(panel.gameObject);
            }
        }
        _uiManager = FindObjectOfType<UIManager>();
        _uiManager.Pause();
        OKButton.onClick.AddListener(CloseInfoPanel);
        
    }

    public void Setup(string infoText, string headerText = "Info")
    {
        HeaderText.SetText(headerText);
        InfoText.SetText(infoText);
    }

    public void CloseInfoPanel()
    {
        Destroy(this.gameObject);
    }

    private void OnDestroy()
    {
        _uiManager.UnPause();
    }
}
