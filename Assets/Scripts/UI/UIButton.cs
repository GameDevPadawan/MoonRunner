using UnityEngine;
using UnityEngine.EventSystems;

public class UIButton : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] private AudioSource PointerEnterSound;
    
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        PointerEnterSound.Play();
    }
}
