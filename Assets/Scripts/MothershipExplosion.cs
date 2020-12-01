using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MothershipExplosion : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem explosion;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Explode()
    {
        ParticleSystem exp = Instantiate(explosion, transform.position, transform.rotation).GetComponent<ParticleSystem>();
        var main = exp.main;
        main.scalingMode = ParticleSystemScalingMode.Local;
        exp.transform.localScale = new Vector3(100, 100, 100);
        exp.Play();
        Destroy(exp.gameObject, exp.main.duration);
        Destroy(this.gameObject, 0.2f);
    }
}
