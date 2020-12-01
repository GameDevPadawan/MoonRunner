using System.Collections;
using TMPro;
using UnityEngine;

public class OnScreenPrompt : MonoBehaviour
{
    [SerializeField] private float lifeTime;
    [SerializeField] private float textTypingInterval = 0.15f;
    [SerializeField] private TextMeshProUGUI Message;
    private string _message;

    public void Setup(string message)
    {
        _message = message;
        StartCoroutine(DisplayText());
    }

    IEnumerator DisplayText()
    {
        string displayed = "";
        foreach (var x in _message)
        {
            displayed += x;
            Message.SetText(displayed);
            yield return new WaitForSeconds(textTypingInterval);
        }
        Destroy(this.gameObject, lifeTime);
    }
}
