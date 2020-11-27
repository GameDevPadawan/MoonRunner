using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigCanon : MonoBehaviour
{
   

    bool estado2, estado3, estadofinal;

    public GameObject[] canonstates;

    public float MaxScrap, ActualScrap;
 
    void Start()
    {
        canonstates[0].SetActive(true);
        canonstates[1].SetActive(false);
        canonstates[2].SetActive(false);
        canonstates[3].SetActive(false);
    }

    
    void Update()
    {
        if(ActualScrap > (MaxScrap * 0.33) && estado2 == false)
        {
            State2();
            estado2 = true;
        }
       else if (ActualScrap > (MaxScrap * 0.66) && estado3 == false)
        {
            State3();
            estado3 = true;
        }
        else if (ActualScrap > MaxScrap && estadofinal == false)
        {
            FinalState();
            estadofinal = true;
        }
    }

    void State2()
    {
        canonstates[0].SetActive(false);
        canonstates[1].SetActive(true);
    }
    void State3()
    {
        canonstates[1].SetActive(false);
        canonstates[2].SetActive(true);
    }
    void FinalState()
    {
        canonstates[2].SetActive(false);
        canonstates[3].SetActive(true);
        //END GAME
    }
}
