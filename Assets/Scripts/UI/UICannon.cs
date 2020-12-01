using UnityEngine;
using UnityEngine.UI;

public class UICannon : MonoBehaviour
{
    private BigCanon bigC;
    [SerializeField] private Image fillImage;
    private void Start()
    {
        bigC = FindObjectOfType<BigCanon>();
        bigC.health.OnDamaged += UpdateCannonProgress;
        bigC.health.OnHealed += UpdateCannonProgress;
        UpdateCannonProgress();
    }

    void UpdateCannonProgress()
    {
        fillImage.fillAmount = bigC.GetCompleteRatio();
    }

    private void OnDestroy()
    {
        bigC.health.OnDamaged -= UpdateCannonProgress;
        bigC.health.OnHealed -= UpdateCannonProgress;
    }
}
