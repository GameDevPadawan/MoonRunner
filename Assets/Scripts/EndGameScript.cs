using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;

public class EndGameScript : MonoBehaviour
{
    [SerializeField]
    private Animation barrelRotationAnimation;
    [SerializeField]
    private CinemachineVirtualCamera endSceneCamera;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator PlayEndScene(BigCanon bigCanon)
    {
        endSceneCamera.Priority = 10;
        barrelRotationAnimation.wrapMode = WrapMode.Once;
        barrelRotationAnimation.Play();
        yield return new WaitForSeconds(5);
        MothershipExplosion mothershipExplosion = GameObject.FindObjectOfType<MothershipExplosion>();
        bigCanon.DrawLine(mothershipExplosion.transform.position);
        mothershipExplosion.Explode();
        UIManager.Message("YOU WIN!");
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene("Credits");
    }
}
