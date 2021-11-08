using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blocco_livello : MonoBehaviour
{
    // un array di gamoject chiamate barriere perche impedirano al giocatore di accedere alla scena 
    public GameObject[] barierre;

    // Start is called before the first frame update
    void Start()
    {
        // cosi faccendo possiamo attraverso il for dissattivare i vari gameocjct con le varie gare che il giocatore finisce 
        int garaAt = PlayerPrefs.GetInt("garaAt");
        Debug.Log(garaAt);
        for(int i=0; i< garaAt; i++)
        {
            barierre[i].SetActive(false);
        }

    }

    // Update is called once per frame
    void Update()
    {
        // ci serve solo per gestire manualmente il valore del player prefab
        if(Input.GetKeyDown(KeyCode.I))
        {
            PlayerPrefs.SetInt("garaAt" ,PlayerPrefs.GetInt("garaAt")+1);

            if (PlayerPrefs.GetInt("garaAt")>barierre.Length)
            {
                PlayerPrefs.SetInt("garaAt",0);
            }
        }

        //if (Input.GetKeyDown(KeyCode.O))
        //{
        //    PlayerPrefs.SetInt("garaAt", 0);
        //}
    }
}
