using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class enemyCarHealth : MonoBehaviour
{
    [SerializeField]
    private float health; //variabile da assegnare
    [SerializeField]
    private float healthRef; //variabile che cambierà
    [SerializeField]
    private GameObject sparks;
    [SerializeField]
    private GameObject sparks2;
    [SerializeField]
    private Image lifeBar;

    private void Start()
    {
        healthRef = health; //imposto la variabile uguale a quella assegnata nell inspector
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            //diminuisce la vita di 1
            healthRef -= 1;
            lifeBar.fillAmount -= 0.3f;
        }


        if(healthRef  >= 2 && healthRef < 3)
        {
            //attiva un gameObject (in questo caso sparks)
            sparks.SetActive(true);

        }

        if(healthRef < 2 && healthRef >= 1)
        {
            //attiva un gameObject (in questo caso sparks2)
            sparks2.SetActive(true);

        }

        if(healthRef < 1)
        {

            

        }


    }


}
